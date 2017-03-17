#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

import datetime
from bdateutil import relativedelta

from django.core.exceptions import ObjectDoesNotExist
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models, transaction
from django.utils import timezone
from django.utils.functional import cached_property
from django.utils.translation import ugettext_lazy as _

from wp4.perfusion_machine.models import Machine
from wp4.staff.models import Person
from ..validators import validate_between_1900_2050, validate_not_in_future
from ..managers.core import DonorModelForUserManager
from ..managers.core import ClosedOrganModelForUserManager, AllocatableModelForUserManager, OpenOrganModelForUserManager
from ..managers.core import OrganModelForUserManager, ProcurementResourceModelForUserManager
from .core import Patient, Randomisation, RetrievalTeam
from . import YES_NO_UNKNOWN_CHOICES, PRESERVATION_HMP, PRESERVATION_HMPO2, PRESERVATION_NOT_SET
from . import LOCATION_CHOICES, PRESERVATION_CHOICES
from . import LEFT, RIGHT
from . import AuditControlModelBase


class Donor(AuditControlModelBase):
    """
    Extension of an Patient record (via OneToOne link for good ORM/DB management) to capture
    the Donor specific data

    Also holds the meta-data specific to the Procurement Form
    """
    person = models.OneToOneField(Patient, help_text="Internal link to Patient")
    sequence_number = models.PositiveSmallIntegerField(
        default=0,
        validators=[MaxValueValidator(99)],
        help_text="Internal value for tracking trial ID sequence number. Value of 1-99"
    )

    # Donor Form metadata
    NON_RANDOMISATION_CHOICES = (
        (0, _("DOc15 Not Applicable")),
        (1, _("DOc10 Donor not proceeding")),
        (2, _("DOc11 One or more kidneys allocated to non-trial location")),
        (3, _("DOc12 Kidneys not allocated")),
        (4, _("DOc13 Kidneys not transplanable")),
        (5, _("DOc14 Other")),
    )  #: Donor not_randomised_because choices
    multiple_recipients = models.PositiveSmallIntegerField(
        verbose_name=_('DO02 Multiple recipients'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )  #: Choices limited to YES_NO_UNKNOWN_CHOICES
    not_randomised_because = models.PositiveSmallIntegerField(
        verbose_name=_("DO51 Why was this not randomised?"),
        choices=NON_RANDOMISATION_CHOICES,
        default=0
    )
    not_randomised_because_other = models.CharField(verbose_name=_('DO52 More details'), max_length=250, blank=True)
    procurement_form_completed = models.BooleanField(
        verbose_name=_("DO99 Form complete"),
        default=False,
        help_text=_("Select Yes when you believe the form is complete and you have no more data to enter")
    )
    admin_notes = models.TextField(verbose_name=_("DO50 Admin notes"), blank=True)
    trial_id = models.CharField(verbose_name=_('DO99 donor id'), max_length=10, blank=True)

    # Procedure data
    retrieval_team = models.ForeignKey(RetrievalTeam, verbose_name=_("DO01 retrieval team"))
    perfusion_technician = models.ForeignKey(
        Person,
        verbose_name=_('DO03 name of transplant technician'),
        related_name="donor_perfusion_technician_set",
    )
    transplant_coordinator = models.ForeignKey(
        Person,
        verbose_name=_('DO04 name of the SN-OD'),
        related_name="donor_transplant_coordinator_set",
        blank=True,
        null=True
    )
    call_received = models.DateTimeField(
        verbose_name=_('DO05 Consultant to MTO called at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    call_received_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    # retrieval_hospital changed to charfield from foreignkey as per issue #211
    retrieval_hospital = models.CharField(verbose_name=_('DO06 donor hospital'), max_length=100, blank=True)
    scheduled_start = models.DateTimeField(
        verbose_name=_('DO07 time of withdrawal therapy'),
        blank=True, null=True,
        validators=[validate_between_1900_2050],
        help_text="Date must be fall within 1900-2050"
    )
    scheduled_start_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    technician_arrival = models.DateTimeField(
        verbose_name=_('DO08 arrival time of technician'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    technician_arrival_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    ice_boxes_filled = models.DateTimeField(
        verbose_name=_('DO09 ice boxes filled'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    ice_boxes_filled_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    depart_perfusion_centre = models.DateTimeField(
        verbose_name=_('DO10 departure from base hospital at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future]
    )
    depart_perfusion_centre_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    arrival_at_donor_hospital = models.DateTimeField(
        verbose_name=_('DO11 arrival at donor hospital'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    arrival_at_donor_hospital_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")

    # Donor details (in addition to Patient)
    age = models.PositiveSmallIntegerField(
        verbose_name=_('DO12 age'),
        validators=[MinValueValidator(50), MaxValueValidator(99)],
        help_text="Age must be in the range 50-99"
    )
    date_of_admission = models.DateField(
        verbose_name=_('DO13 date of admission'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    date_of_admission_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    admitted_to_itu = models.BooleanField(verbose_name=_('DO14 admitted to ITU'), default=False)
    date_admitted_to_itu = models.DateField(
        verbose_name=_('DO15 when admitted to ITU'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    date_admitted_to_itu_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    date_of_procurement = models.DateField(
        verbose_name=_('DO16 date of procurement'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    other_organs_procured = models.BooleanField(verbose_name=_("DO17 other organs procured"), default=False)
    other_organs_lungs = models.BooleanField(verbose_name=_("DO18 lungs"), default=False)
    other_organs_pancreas = models.BooleanField(verbose_name=_("DO19 pancreas"), default=False)
    other_organs_liver = models.BooleanField(verbose_name=_("DO20 liver"), default=False)
    other_organs_tissue = models.BooleanField(verbose_name=_("DO21 tissue"), default=False)

    # DonorPreop data
    DIAGNOSIS_CEREBROVASCULAR_ACCIDENT = 1  #: Constant for DIAGNOSIS_CHOICES
    DIAGNOSIS_HYPOXIA = 2  #: Constant for DIAGNOSIS_CHOICES
    DIAGNOSIS_TRAUMA = 3  #: Constant for DIAGNOSIS_CHOICES
    DIAGNOSIS_OTHER = 4  #: Constant for DIAGNOSIS_CHOICES
    DIAGNOSIS_CHOICES = (
        (DIAGNOSIS_CEREBROVASCULAR_ACCIDENT, _("DOc01 Cerebrovascular Accident")),
        (DIAGNOSIS_HYPOXIA, _("DOc02 Hypoxia")),
        (DIAGNOSIS_TRAUMA, _("DOc03 Trauma")),
        (DIAGNOSIS_OTHER, _("DOc04 Other"))
    )  #: Donor diagnosis choices
    diagnosis = models.PositiveSmallIntegerField(
        verbose_name=_('DO22 diagnosis'),
        choices=DIAGNOSIS_CHOICES,
        blank=True, null=True
    )
    diagnosis_other = models.CharField(verbose_name=_('DO23 other diagnosis'), max_length=250, blank=True)
    diabetes_melitus = models.PositiveSmallIntegerField(
        verbose_name=_('DO24 diabetes mellitus'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )  #: Choices limited to YES_NO_UNKNOWN_CHOICES
    alcohol_abuse = models.PositiveSmallIntegerField(
        verbose_name=_('DO25 alcohol abuse'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )  #: Choices limited to YES_NO_UNKNOWN_CHOICES
    cardiac_arrest = models.NullBooleanField(
        verbose_name=_('DO26 cardiac arrest'),
        blank=True, null=True
    )  #: Cardiac Arrest (During ITU stay, prior to Retrieval Procedure)
    systolic_blood_pressure = models.PositiveSmallIntegerField(
        verbose_name=_('DO27 last systolic blood pressure'),
        validators=[MinValueValidator(10), MaxValueValidator(200)],
        blank=True, null=True,
        help_text="Value must be in the range 10-200"
    )  #: Last Systolic Blood Pressure (Before switch off)
    diastolic_blood_pressure = models.PositiveSmallIntegerField(
        verbose_name=_('DO28 last diastolic blood pressure'),
        validators=[MinValueValidator(10), MaxValueValidator(200)],
        blank=True, null=True,
        help_text="Value must be in the range 10-200"
    )  #: Last Diastolic Blood Pressure (Before switch off)
    diuresis_last_day = models.PositiveSmallIntegerField(
        verbose_name=_('DO29 diuresis last day (ml)'),
        blank=True, null=True
    )
    diuresis_last_day_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    diuresis_last_hour = models.PositiveSmallIntegerField(
        verbose_name=_('DO30 diuresis last hour (ml)'),
        blank=True, null=True
    )
    diuresis_last_hour_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    dopamine = models.PositiveSmallIntegerField(
        verbose_name=_('DO31 dopamine'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )  #: Choices limited to YES_NO_UNKNOWN_CHOICES
    dobutamine = models.PositiveSmallIntegerField(
        verbose_name=_('DO32 dobutamine'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )  #: Choices limited to YES_NO_UNKNOWN_CHOICES
    nor_adrenaline = models.PositiveSmallIntegerField(
        verbose_name=_('DO33 nor adrenaline'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )  #: Choices limited to YES_NO_UNKNOWN_CHOICES
    vasopressine = models.PositiveSmallIntegerField(
        verbose_name=_('DO34 vasopressine'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )  #: Choices limited to YES_NO_UNKNOWN_CHOICES
    other_medication_details = models.CharField(
        verbose_name=_('DO35 other medication'),
        max_length=250,
        blank=True
    )

    # Lab results
    UNIT_MGDL = 1  #: Constant for UNIT_CHOICES
    UNIT_UMOLL = 2  #: Constant for UNIT_CHOICES
    UNIT_CHOICES = (
        (UNIT_MGDL, "mg/dl"),
        (UNIT_UMOLL, "umol/L")
    )  #: Donor last_creatinine_unit and max_creatinine_unit choices
    last_creatinine = models.FloatField(
        verbose_name=_('DO36 last creatinine'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    last_creatinine_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)
    max_creatinine = models.FloatField(verbose_name=_('DO37 max creatinine'), blank=True, null=True)
    max_creatinine_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)

    # Operation Data - Extraction
    SOLUTION_UW = 1  #: Constant for SOLUTION_CHOICES
    SOLUTION_MARSHALL = 2  #: Constant for SOLUTION_CHOICES
    SOLUTION_HTK = 3  #: Constant for SOLUTION_CHOICES
    SOLUTION_OTHER = 4  #: Constant for SOLUTION_CHOICES
    SOLUTION_CHOICES = (
        (SOLUTION_HTK, "HTK"),
        (SOLUTION_MARSHALL, "Marshall's"),
        (SOLUTION_UW, "UW"),
        (SOLUTION_OTHER, _("DOc04 Other"))
    )  #: Donor systemic_flush_used choices
    life_support_withdrawal = models.DateTimeField(
        verbose_name=_('DO38 withdrawal of life support'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    systolic_pressure_low = models.DateTimeField(
        verbose_name=_('DO39 systolic arterial pressure'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )  #: < 50 mm Hg (inadequate organ perfusion)
    systolic_pressure_low_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    o2_saturation = models.DateTimeField(
        verbose_name=_('DO40 O2 saturation below 80%'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    o2_saturation_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    circulatory_arrest = models.DateTimeField(
        verbose_name=_('DO41 end of cardiac output'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )  #: Also known as the start of no touch period
    circulatory_arrest_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    length_of_no_touch = models.PositiveSmallIntegerField(
        verbose_name=_('DO42 length of no touch period (minutes)'),
        blank=True, null=True,
        validators=[MinValueValidator(1), MaxValueValidator(60)],
        help_text="Value must be in the range 1-60 minutes"
    )
    death_diagnosed = models.DateTimeField(
        verbose_name=_('DO43 knife to skin time'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text=_('DO43h This also counts as Date of Death for donor. Date must be fall within 1900-2050, and not be in the future')
    )  #: Changes to this require sync to the Patient.date_of_death, see the save method
    perfusion_started = models.DateTimeField(
        verbose_name=_('DO44 start in-situ cold perfusion'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    perfusion_started_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    systemic_flush_used = models.PositiveSmallIntegerField(
        verbose_name=_('DO45 systemic (aortic) flush solution used'),
        choices=SOLUTION_CHOICES,
        blank=True, null=True
    )
    systemic_flush_used_other = models.CharField(
        verbose_name=_('DO46 systemic flush used'),
        max_length=250,
        blank=True
    )
    systemic_flush_volume_used = models.PositiveSmallIntegerField(
        verbose_name=_('DO47 aortic - volume (ml)'),
        blank=True, null=True
    )  #: NB: this doesn't appear on the paper forms
    heparin = models.NullBooleanField(
        verbose_name=_('DO48 heparin'),
        blank=True, null=True
    )  #: As administered to donor/in flush solution

    # Internal references back to Organ, to help speed up the linking and querying from the DB
    _left_kidney = models.OneToOneField('Organ', related_name='left_kidney', null=True, default=None)
    _right_kidney = models.OneToOneField('Organ', related_name='right_kidney', null=True, default=None)

    objects = DonorModelForUserManager()

    class Meta:
        order_with_respect_to = 'retrieval_team'
        verbose_name = _('DOm1 donor')
        verbose_name_plural = _('DOm2 donors')
        permissions = (
            ("view_donor", "Can only view the data"),
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )

    def country_for_restriction(self):
        """
        Get the country to be used for geographic restriction of this data
        :return: Int: Value from list in Locations.Models. Should be in range [1,4,5]
        """
        return self.retrieval_team.based_at.country

    def location_for_restriction(self):
        """
        Get the location to be used for geographic restriction of this data
        :return: Int: Hospital object id
        """
        return self.retrieval_team.based_at.id

    def clean(self):
        """
        Clears the following fields of data if their corresponding unknown flag is set to True

        * call_received
        * scheduled_start
        * technician_arrival
        * ice_boxes_filled
        * depart_perfusion_centre
        * arrival_at_donor_hospital
        * date_of_admission
        * date_admitted_to_itu
        * diuresis_last_day
        * diuresis_last_hour
        * systolic_pressure_low
        * o2_saturation
        * circulatory_arrest
        * perfusion_started

        Error if:

        * arrival_at_donor_hospital before depart_perfusion_centre (DOv01)
        * age does not match person.age_from_dob (DOv05)
        * date_of_birth is less than 50 years before date_of_procurement (DOv03)
        * date_of_birth is more than 100 years before date_of_procurement (DOv04)
        * date_of_procurement before date_of_admission (DOv06)
        * admitted_to_itu without recording date_admitted_to_itu (DOv07)
        * diagnosis is set to Other, and diagnosis_other is empty (DOv08)
        * date_admitted_to_itu before date_of_admission (DOv12)
        * life_support_withdrawal before date_of_admission (DOv09)
        * circulatory_arrest after death_diagnosed (DOv10) - a pre-requisite for DCD case eligibility
        * systemic_flush_used is set to Other, but systemic_flush_used_other is empty (DOv11)
        * procurement_form_completed is True, but: retrieval_hospital is not set (DOv12); and person.number is empty (DOv13)
        """
        # Clean the fields that at Not Known
        if self.call_received_unknown:
            self.call_received = None
        if self.scheduled_start_unknown:
            self.scheduled_start = None
        if self.technician_arrival_unknown:
            self.technician_arrival = None
        if self.ice_boxes_filled_unknown:
            self.ice_boxes_filled = None
        if self.depart_perfusion_centre_unknown:
            self.depart_perfusion_centre = None
        if self.arrival_at_donor_hospital_unknown:
            self.arrival_at_donor_hospital = None
        if self.date_of_admission_unknown:
            self.date_of_admission = None
        if self.date_admitted_to_itu_unknown:
            self.date_admitted_to_itu = None
        if self.diuresis_last_day_unknown:
            self.diuresis_last_day = None
        if self.diuresis_last_hour_unknown:
            self.diuresis_last_hour = None
        if self.systolic_pressure_low_unknown:
            self.systolic_pressure_low = None
        if self.o2_saturation_unknown:
            self.o2_saturation = None
        if self.circulatory_arrest_unknown:
            self.circulatory_arrest = None
        if self.perfusion_started_unknown:
            self.perfusion_started = None

        if self.arrival_at_donor_hospital and self.depart_perfusion_centre:
            if self.arrival_at_donor_hospital < self.depart_perfusion_centre:
                raise ValidationError(
                    # TODO: Fix the lack of space in the strings when joined
                    _("DOv01 Time travel detected! Arrival at donor hospital occurred before departure"
                        "from perfusion centre")
                )
        if self.person_id is not None and self.person.date_of_birth:
            if self.age != self.person.age_from_dob:
                the_end = self.person.date_of_death if self.person.date_of_death else timezone.now().date()
                lower_date = the_end + relativedelta(years=-self.age, months=-12, days=+1)
                upper_date = the_end + relativedelta(years=-self.age)
                raise ValidationError(
                    # TODO: Fix the lack of space in the strings when joined
                    _("DOv05 Age (%(age)d) does not match age as calculated (%(num)d years) from Date of"
                        "Birth. DoB should be between %(date1)s and %(date2)s."
                        % {
                            'age': self.age,
                            'num': self.person.age_from_dob,
                            'date1': lower_date.strftime('%d %b %Y'),
                            'date2': upper_date.strftime('%d %b %Y')
                        })
                )
            if self.date_of_procurement:
                calculated_age = relativedelta(self.date_of_procurement, self.person.date_of_birth).years
                if calculated_age < 50:
                    raise ValidationError(
                        _("DOv03 Date of birth is less than 50 years from the date of procurement (%(num)d)"
                          % {'num': calculated_age})
                    )
                elif calculated_age > 100:
                    raise ValidationError(
                        _("DOv04 Date of birth is more than 100 years from the date of procurement (%(num)d)"
                          % {'num': calculated_age})
                    )
        if self.date_of_procurement and self.date_of_admission:
            if self.date_of_procurement < self.date_of_admission:
                raise ValidationError(_("DOv06 Date of procurement occurs before date of admission"))

        if self.admitted_to_itu and not self.date_admitted_to_itu:
            raise ValidationError(_("DOv07 Missing the date admitted to ITU"))
        if self.diagnosis == self.DIAGNOSIS_OTHER and not self.diagnosis_other:
            raise ValidationError(_("DOv08 Missing the other diagnosis"))
        if self.date_admitted_to_itu and self.date_of_admission:
            if self.date_admitted_to_itu < self.date_of_admission:
                raise ValidationError(_("DOv12 Donor in ICU before they were admitted to hospital"))

        if self.life_support_withdrawal and self.date_of_admission:
            if self.life_support_withdrawal.date() < self.date_of_admission:
                raise ValidationError(_("DOv09 Life support withdrawn before admission to hospital"))
        if self.circulatory_arrest and self.death_diagnosed:
            if self.circulatory_arrest > self.death_diagnosed:
                raise ValidationError(_("DOv10 Donor was diagnosed as dead before circulation stopped"))

        if self.systemic_flush_used and self.systemic_flush_used == self.SOLUTION_OTHER \
                and not self.systemic_flush_used_other:
            raise ValidationError(_("DOv11 Missing the details of the other systemic flush solution used"))

        if self.procurement_form_completed:
            # This is not a critical piece of information any more
            # if self.retrieval_hospital is None:
            #     raise ValidationError(_("DOv12 Missing retrieval hospital"))
            if self.person.number == "":
                raise ValidationError(_("DOv13 Please enter the NHSBT number"))

    def save(self, force_insert=False, force_update=False, using=None, update_fields=None):
        """
        Updated save method to ensure that if death_diagnosed is set, then this is reflected in the
        same value for person.date_of_death

        :param force_insert: bool. As parent method
        :param force_update: bool. As parent method
        :param using:  As parent method
        :param update_fields:  As parent method
        :return: super(Donor, self).save()
        """
        if self.death_diagnosed:
            # Have to sync this data to the Patient.date_of_death
            self.person.date_of_death = self.death_diagnosed
            self.person.save()
        return super(Donor, self).save(force_insert, force_update, using, update_fields)

    @transaction.atomic
    def randomise(self, is_online=True, active_user=None):
        """
        Randomise if eligible and not already done. This is determined by looking at the elibility
        criteria, and checking that the Organ.preservation is not set. Once randomised, the
        sequence_number is allocated for the Trial ID to be defined. The location of the
        retrieval_team is used to determine which geographical list set to use, and the is_online
        value determines which of the two lists to pick from.

        :param is_online: bool. Pick from an online list if True, or an Offline list if False
        :param active_user. The user that is triggering the randomisation process
        :return: True if randomisation occurs, False otherwise
        :rtype: bool
        """
        from ..utils import update_trial_ids_and_save

        left_kidney = self.left_kidney
        right_kidney = self.right_kidney
        if left_kidney.preservation == PRESERVATION_NOT_SET \
                and self.multiple_recipients is not False \
                and left_kidney.transplantable \
                and right_kidney.transplantable:
            if Randomisation.get_and_assign_result(
                self.retrieval_team.get_randomisation_list(is_online),
                self,
                active_user
            ):
                left_kidney.preservation = PRESERVATION_HMPO2
                right_kidney.preservation = PRESERVATION_HMP
            else:
                left_kidney.preservation = PRESERVATION_HMP
                right_kidney.preservation = PRESERVATION_HMPO2
            left_kidney.save()
            right_kidney.save()

            # On randomise (not save anymore), get and save the sequence number from the retrieval team
            if self.sequence_number < 1:
                self.sequence_number = self.retrieval_team.next_sequence_number()
                self.save()

            update_trial_ids_and_save(self)
            return True
        return False

    def __str__(self):
        return self.trial_id

    @property
    def left_kidney(self):
        """
        Property to emulate a safe get_or_create call to Organ. If no linked organ, a new record is
        created with sensible defaults and linked.

        :return: Organ record for the left kidney
        :rtype: wp4.compare.models.organ.Organ
        """
        from wp4.compare.models import Organ
        if self._left_kidney is None:
            try:
                self._left_kidney = self.organ_set.filter(location__exact=LEFT)[0]
            except IndexError:  # Organ.DoesNotExist:
                # print("DEBUG: Donor.left_kidney() raised IndexError")
                new_organ = Organ(location=LEFT)
                if self.id > 0:
                    new_organ.donor = self
                new_organ.save()
                self._left_kidney = new_organ
            self.save()
        return self._left_kidney

    @property
    def right_kidney(self):
        """
        Property to emulate a safe get_or_create call to Organ. If no linked organ, a new record is
        created with sensible defaults and linked.

        :return: Organ record for the right kidney
        :rtype: wp4.compare.models.organ.Organ
        """
        from wp4.compare.models import Organ
        if self._right_kidney is None:
            try:
                self._right_kidney = self.organ_set.filter(location__exact=RIGHT)[0]
            except IndexError:  # Organ.DoesNotExist:
                # print("DEBUG: Donor.right_kidney() raised IndexError")
                new_organ = Organ(location=RIGHT)
                if self.id > 0:
                    new_organ.donor = self
                new_organ.save()
                self._right_kidney = new_organ
            self.save()
        return self._right_kidney

    def _centre_code(self):
        """
        Does a safe lookup for the centre_code of the retrieval_team. If no team set, returns 0

        :return: Centre code, or 0
        :rtype: int
        """
        try:
            return self.retrieval_team.centre_code
        except RetrievalTeam.DoesNotExist:
            return 0
    centre_code = cached_property(_centre_code, name='centre_code')

    def make_trial_id(self):
        """
        Returns the composite trial id string.

        :return: 'WP4cctnns' - where:

                * cc = 2 digit centre code
                * t = single digit, 0 for online, 9 for offline randomisation
                * nn = 2 digit sequence number, starting at 01
                * s = (optional) single character denoting organ location, L for Left, R for Right

                If no centre_code set or a lack of sequence_number, returns "No Trial ID Assigned (DOnnn)"
                where nnn = the object's ID

        :rtype: str
        """
        trial_id = "WP4" + format(self.centre_code, '02')
        if self.centre_code == 0 or self.sequence_number < 1:
            trial_id = "DO%s" % format(self.id, '03')
        elif self.is_offline:
            trial_id += "9" + format(self.sequence_number, '02')
        else:
            trial_id += format(self.sequence_number, '03')
        return trial_id

    def _is_offline(self):
        """
        Determine if this was randomised using the relevant online or offline lists

        :return: True, if randomisation.list_code matches PAPER_EUROPE or PAPER_UNITED_KINGDOM
        :rtype: bool
        """
        try:
            return self.randomisation.list_code in [Randomisation.PAPER_EUROPE, Randomisation.PAPER_UNITED_KINGDOM]
        except:
            pass  # We have no idea what error is being thrown or caught because there is no error!
        return False
    is_offline = cached_property(_is_offline, name='is_offline')

    def _is_randomised(self):
        """
        Checks the left_kidney.preservation value, and if PRESERVATION_NOT_SET, returns False.

        :return: True if the left kidney has been randomised
        :rtype: bool
        """
        if self.left_kidney.preservation == PRESERVATION_NOT_SET:
            return False
        return True
    is_randomised = cached_property(_is_randomised, name='is_randomised')

    def _count_eligible(self):
        """
        Number of eligible kidneys from this donor.

        :return: 0, 1, or 2 kidneys, and -1 for not randomised
        :rtype: int
        """
        eligible_kidney_count = 0
        left_kidney = self.left_kidney
        right_kidney = self.right_kidney
        if left_kidney.preservation != PRESERVATION_NOT_SET and self.multiple_recipients is not False:
            if left_kidney.transplantable:
                eligible_kidney_count += 1
            if right_kidney.transplantable:
                eligible_kidney_count += 1
        else:
            eligible_kidney_count = -1
        return eligible_kidney_count
    count_of_eligible_organs = cached_property(_count_eligible, name='count_of_eligible_organs')


class Organ(AuditControlModelBase):
    """
    The focus of the trial, specifically a Kidney.
    """
    donor = models.ForeignKey(Donor, help_text="Internal link to the Donor")
    location = models.CharField(
        verbose_name=_('OR01 kidney location'),
        max_length=1,
        choices=LOCATION_CHOICES
    )  #: Choices limited to LOCATION_CHOICES

    # Transplantation Form metadata
    not_allocated_reason = models.CharField(
        verbose_name=_('OR31 not transplantable because'),
        max_length=250,
        blank=True
    )
    admin_notes = models.TextField(verbose_name=_("OR50 Admin notes"), blank=True)
    transplantation_notes = models.TextField(verbose_name=_("OR51 Transplantation notes"), blank=True)
    transplantation_form_completed = models.BooleanField(
        verbose_name=_("OR99 Form complete"),
        default=False,
        help_text="Select Yes when you believe the form is complete and you have no more data to enter"
    )
    trial_id = models.CharField(verbose_name=_('OR99 organ id'), max_length=10, blank=True)

    # Inspection data
    GRAFT_DAMAGE_ARTERIAL = 1  #: Constant for GRAFT_DAMAGE_CHOICES
    GRAFT_DAMAGE_VENOUS = 2  #: Constant for GRAFT_DAMAGE_CHOICES
    GRAFT_DAMAGE_URETERAL = 3  #: Constant for GRAFT_DAMAGE_CHOICES
    GRAFT_DAMAGE_PARENCHYMAL = 4  #: Constant for GRAFT_DAMAGE_CHOICES
    GRAFT_DAMAGE_NONE = 5  #: Constant for GRAFT_DAMAGE_CHOICES
    GRAFT_DAMAGE_OTHER = 6  #: Constant for GRAFT_DAMAGE_CHOICES
    GRAFT_DAMAGE_CHOICES = (
        (GRAFT_DAMAGE_NONE, _("ORc01 None")),
        (GRAFT_DAMAGE_ARTERIAL, _("ORc02 Arterial Damage")),
        (GRAFT_DAMAGE_VENOUS, _("ORc03 Venous Damage")),
        (GRAFT_DAMAGE_URETERAL, _("ORc04 Ureteral Damage")),
        (GRAFT_DAMAGE_PARENCHYMAL, _("ORc05 Parenchymal Damage")),
        (GRAFT_DAMAGE_OTHER, _("ORc06 Other Damage"))
    )  #: Organ graft_damage choices

    WASHOUT_PERFUSION_HOMEGENOUS = 1  #: Constant for WASHOUT_PERFUSION_CHOICES
    WASHOUT_PERFUSION_PATCHY = 2  #: Constant for WASHOUT_PERFUSION_CHOICES
    WASHOUT_PERFUSION_BLUE = 3  #: Constant for WASHOUT_PERFUSION_CHOICES
    WASHOUT_PERFUSION_UNKNOWN = 9  #: Constant for WASHOUT_PERFUSION_CHOICES
    WASHOUT_PERFUSION_CHOICES = (
        # NHS Form has: Good, Fair, Poor, Patchy, Unknown
        (WASHOUT_PERFUSION_HOMEGENOUS, _("ORc07 Homogenous")),
        (WASHOUT_PERFUSION_PATCHY, _("ORc08 Patchy")),
        (WASHOUT_PERFUSION_BLUE, _("ORc09 Blue")),
        (WASHOUT_PERFUSION_UNKNOWN, _("ORc10 Unknown"))
    )  #: Organ washout_perfusion choices

    removal = models.DateTimeField(
        verbose_name=_('OR02 time out'),
        blank=True,
        null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    renal_arteries = models.PositiveSmallIntegerField(
        verbose_name=_('OR03 number of renal arteries'),
        blank=True, null=True,
        validators=[MinValueValidator(0), MaxValueValidator(5)],
        help_text="Number of arteries must be in range 0-5"
    )
    graft_damage = models.PositiveSmallIntegerField(
        verbose_name=_('OR04 renal graft damage'),
        choices=GRAFT_DAMAGE_CHOICES,
        default=GRAFT_DAMAGE_NONE
    )  #: Choices limited to GRAFT_DAMAGE_CHOICES
    graft_damage_other = models.CharField(verbose_name=_('OR05 other damage done'), max_length=250, blank=True)
    washout_perfusion = models.PositiveSmallIntegerField(
        verbose_name=_('OR06 perfusion characteristics'),
        choices=WASHOUT_PERFUSION_CHOICES,
        blank=True, null=True
    )  #: Choices limited to WASHOUT_PERFUSION_CHOICES
    transplantable = models.NullBooleanField(
        verbose_name=_('OR07 is transplantable'),
        blank=True, null=True,
        help_text=_("OR07h This answer can be amended after randomisation and saving of the form if necessary")
    )
    not_transplantable_reason = models.CharField(
        verbose_name=_('OR08 not transplantable because'),
        max_length=250,
        blank=True
    )

    # Randomisation data
    # can_donate = models.BooleanField('Donor is eligible as DCD III and > 50 years old') -- donor info!
    # can_transplant = models.BooleanField('') -- derived from left and right being transplantable
    preservation = models.PositiveSmallIntegerField(
        choices=PRESERVATION_CHOICES,
        default=PRESERVATION_NOT_SET
    )  #: Choices limited to PRESERVATION_CHOICES

    # Perfusion data
    PATCH_SMALL = 1  #: Constant for PATCH_HOLDER_CHOICES
    PATCH_LARGE = 2  #: Constant for PATCH_HOLDER_CHOICES
    PATCH_DOUBLE_ARTERY = 3  #: Constant for PATCH_HOLDER_CHOICES
    PATCH_HOLDER_CHOICES = (
        (PATCH_SMALL, _("ORc12 Small")),
        (PATCH_LARGE, _("ORc13 Large")),
        (PATCH_DOUBLE_ARTERY, _("ORc14 Double Artery"))
    )  #: Organ patch_holder choices
    ARTIFICIAL_PATCH_CHOICES = (
        (PATCH_SMALL, _("ORc12 Small")),
        (PATCH_LARGE, _("ORc13 Large"))
    )  #: Organ artificial_patch_size choices
    perfusion_possible = models.NullBooleanField(
        verbose_name=_('OR09 machine perfusion possible?'),
        blank=True, null=True
    )
    perfusion_not_possible_because = models.CharField(
        verbose_name=_('OR10 not possible because'),
        max_length=250,
        blank=True
    )
    perfusion_started = models.DateTimeField(
        verbose_name=_('OR11 machine perfusion'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    patch_holder = models.PositiveSmallIntegerField(
        verbose_name=_('OR12 used patch holder'),
        choices=PATCH_HOLDER_CHOICES,
        blank=True, null=True
    )  #: Choices limited to PATCH_HOLDER_CHOICES
    artificial_patch_used = models.NullBooleanField(verbose_name=_('OR13 artificial patch used'), blank=True, null=True)
    artificial_patch_size = models.PositiveSmallIntegerField(
        verbose_name=_('OR14 artificial patch size'),
        choices=ARTIFICIAL_PATCH_CHOICES,
        blank=True, null=True
    )  #: Choices limited to ARTIFICIAL_PATCH_CHOICES
    artificial_patch_number = models.PositiveSmallIntegerField(
        verbose_name=_('OR15 number of patches'),
        blank=True,
        null=True,
        validators=[MinValueValidator(1), MaxValueValidator(2)]
    )  #: Limited to range 1-2
    oxygen_bottle_full = models.NullBooleanField(
        verbose_name=_('OR16 is oxygen bottle full'),
        blank=True, null=True
    )
    oxygen_bottle_open = models.NullBooleanField(verbose_name=_('OR17 oxygen bottle opened'), blank=True, null=True)
    oxygen_bottle_changed = models.NullBooleanField(verbose_name=_('OR18 oxygen bottle changed'), blank=True, null=True)
    oxygen_bottle_changed_at = models.DateTimeField(
        verbose_name=_('OR19 oxygen bottle changed at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    oxygen_bottle_changed_at_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    ice_container_replenished = models.NullBooleanField(
        verbose_name=_('OR20 ice container replenished'),
        blank=True, null=True
    )
    ice_container_replenished_at = models.DateTimeField(
        verbose_name=_('OR21 ice container replenished at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    ice_container_replenished_at_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    perfusate_measurable = models.NullBooleanField(
        verbose_name=_('OR22 perfusate measurable'),
        blank=True, null=True
    )  #: logistically possible to measure pO2 perfusate (use blood gas analyser)
    perfusate_measure = models.FloatField(verbose_name=_('OR23 value pO2'), blank=True, null=True)
    # TODO: Check the value range for perfusate_measure
    perfusion_machine = models.ForeignKey(
        Machine,
        verbose_name=_('OR24 perfusion machine'),
        blank=True,
        null=True
    )
    # Removed perfusion_file field as per Issue #208

    # Commonly filtered options
    objects = OrganModelForUserManager()  #: Default Organ manager
    allocatable_objects = AllocatableModelForUserManager()  #: AllocatableModelForUserManager
    open_objects = OpenOrganModelForUserManager()  #: OpenOrganModelForUserManager
    closed_objects = ClosedOrganModelForUserManager()  #: ClosedOrganModelForUserManager

    class Meta:
        verbose_name = _('ORm1 organ')
        verbose_name_plural = _('ORm2 organs')
        default_manager_name = 'objects'
        base_manager_name = 'objects'
        permissions = (
            ("view_organ", "Can only view the data"),
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )

    def country_for_restriction(self):
        """
        Get the country to be used for geographic restriction of this data.

        Organs move around, thus we have to look through their likely locations in reverse to determine where it is

        :return: Int: Value from list in Locations.Models. Should be in range [1,4,5]
        """
        if self.safe_recipient is not None:
            return self.safe_recipient.country_for_restriction()
        elif self.final_allocation is not None:
            return self.final_allocation.country_for_restriction()
        else:
            return self.donor.country_for_restriction()

    def location_for_restriction(self):
        """
        Get the location to be used for geographic restriction of this data

        Organs move around, thus we have to look through their likely locations in reverse to determine where it is

        :return: Int: Hospital object id
        """
        if self.safe_recipient is not None:
            return self.safe_recipient.location_for_restriction()
        elif self.final_allocation is not None:
            return self.final_allocation.location_for_restriction()
        else:
            return self.donor.location_for_restriction()

    def clean(self):
        """
        Clears the following fields of data if their corresponding unknown flag is set to True

        * oxygen_bottle_changed_at
        * ice_container_replenished_at

        Error if:

        * transplantable is False, and not_transplantable_reason is empty (ORv01)
        * perfusion_possible is False, and perfusion_not_possible_because is empty (ORv02)
        * perfusion_possible is True, and perfusion_started is not set (ORv03)

        """
        # Clean the fields that at Not Known
        if self.oxygen_bottle_changed_at_unknown:
            self.oxygen_bottle_changed_at = None
        if self.ice_container_replenished_at_unknown:
            self.ice_container_replenished_at = None

        if self.transplantable is False and self.not_transplantable_reason == "":
            raise ValidationError(_("ORv01 Please enter a reason for not being transplantable"))

        if self.perfusion_possible is False and self.perfusion_not_possible_because == "":
            raise ValidationError(_("ORv02 Please enter a reason perfusion wasn't possible"))

        if self.perfusion_possible is True and self.perfusion_started is None:
            raise ValidationError(_("ORv03 Please enter the time perfusion started at"))

    @property
    def safe_recipient(self):
        """
        Helper method to return either the related Recipient record, or a safe value of None,
        without the drama of exceptions when it doesn't exist

        :rtype: wp4.compare.models.transplantation.Recipient
        """
        try:
            if self.recipient is not None:
                return self.recipient  # We have a recipient
        except AttributeError:  # RelatedObjectDoesNotExist
            return None

    def _final_allocation(self):
        """
        Work out if there are any OrganAllocations, and then return the latest one

        :return: OrganAllocation, or None
        :rtype: wp4.compare.models.transplantation.OrganAllocation
        """
        return self.organallocation_set.order_by('id').last()

    final_allocation = cached_property(_final_allocation, name='final_allocation')

    def make_trial_id(self):
        """
        Returns the Donor Trial ID combined with the Location (L or R) for the Organ

        :return: 'WP4cctnns'
        :rtype: str
        """
        print("DEBUG: Organ.make_trial_id: {0}".format(self.donor.trial_id))
        return self.donor.trial_id + self.location

    @property
    def is_allocated(self):
        """
        Allocation status

        Determine if an organ has been allocated. Allocated means:

        * To a recipient at a project site
        * Final allocation is to a non-project site

        :return: True, if either of those criteria are met
        :rtype: bool
        """
        if self.safe_recipient is not None:
            return True  # We have a recipient, which can only occur if at a project site

        final_allocation = self.final_allocation
        if final_allocation is not None:
            if final_allocation.reallocated is False and final_allocation.transplant_hospital.is_project_site is False:
                # We can't say not-reallocated without saving a transplant hospital as well
                return True
        return False

    @property
    def explain_is_allocated(self):
        """
        Allocation status description: An explanation as to the allocation status for this organ

        :return: Message describing status of allocation
        :rtype: str
        """
        final_allocation = self.final_allocation
        if self.is_allocated:
            if self.safe_recipient is not None:
                return "Allocated to Recipient"

            if final_allocation.reallocated is False and final_allocation.transplant_hospital.is_project_site is False:
                return "Allocated to a non-project site"  # This should be caught by not_allocated_reason
        else:
            if self.not_allocated_reason:
                return "Not allocated because: %s" % self.not_allocated_reason

            if final_allocation is None:
                return "No allocations created (and no explanation as yet)"

            if final_allocation.reallocated is True:
                return "ERROR: last allocation shows a reallocation"  # This shouldn't occur!

            return "Re-allocation status not yet set"  # Possible for a form that is WIP

        return "ERROR: Unknown allocation status (test data?)"

    @property
    def explain_closed_status(self):
        """
        Work out why this form was closed, and display a suitable summary message

        NOT IMPLEMENTED (yet)

        :return: Message describing the likely cause for the form being closed
        :rtype: str
        """
        # TODO: Write this function
        return "Unknown closed status"

    @property
    def was_cold_stored(self):
        """
        A kidney should be flagged as COLD STORED when (1) OR (2) where
        (1) if wp4.compare.models.organ.Organ - perfusion_possible (django.db.models.NullBooleanField) – “OR09 machine
        perfusion possible?” is FALSE
        (2) if "Was the kidney cold stored because of a machine problem" in the Transplantation form is TRUE

        See discussion in Issue 166 for more detail

        :return: True if criteria are met, False otherwise
        :rtype: bool
        """
        if self.perfusion_possible or (self.safe_recipient and self.safe_recipient.organ_cold_stored):
            return True
        return False

    def _reallocation_count(self):
        """
        Counts the number of organ allocations where reallocated is true

        :return: Count of reallocations
        :rtype: int
        """
        count = 0
        for allocation in self.organallocation_set.all():
            if allocation.reallocated is True:
                count += 1
        return count

    reallocation_count = cached_property(_reallocation_count, name='reallocation_count')

    def _graft_failed(self):
        """
        Scan through the follow ups to determine if there is a graft failure reported for this organ. We can afford
        to presume that if something fails in this sequence there is no point looking for later related objects.

        :return: 0, if no failures. 1 for Initial, 2 for Month 3, 3 for Month 6, and 4 for Month 12
        :rtype: int
        """
        try:
            if self.followup_initial.graft_failure is True:
                return 1
            if self.followup_3m.graft_failure is True:
                return 2
            if self.followup_6m.graft_failure is True:
                return 3
            if self.followup_1y.graft_failure is True:
                return 4
        except ObjectDoesNotExist:
            pass
        return 0

    graft_failed = cached_property(_graft_failed, name='graft_failed')

    def _followup_final_begin_by(self):
        """
        Returns the date that the final follow up can begin by. This is the randomisation date + 300 days
        :return:
        """
        return (self.donor.randomisation.allocated_on + datetime.timedelta(days=300)).date()

    followup_final_begin_by = cached_property(_followup_final_begin_by, name='followup_final_begin_by')

    def _followup_final_completed_by(self):
        """
        Returns the date that the final follow up should be completed by. This is the randomisation date + 365+65 days
        :return:
        """
        return (self.donor.randomisation.allocated_on + datetime.timedelta(days=430)).date()

    followup_final_completed_by = cached_property(_followup_final_completed_by, name='followup_final_completed_by')

    def followup_done_within_followup_final_window(self):
        try:
            if self.followup_1y.start_date >= self.followup_final_begin_by \
                and self.followup_1y.start_date <= self.followup_final_completed_by:
                return True
        except Organ.followup_1y.RelatedObjectDoesNotExist:
            pass
        return False

    def __str__(self):
        return self.trial_id


class ProcurementResource(AuditControlModelBase):
    """
    Repeatable list of resources used during organ extraction. Primarily distinguished by the type. This is an
    extension of the Procurement form for each organ. Geographical restrictions on this are mostly moot, but defer to
    Organ if needed
    """
    DISPOSABLES = "D"  #: Constant for TYPE_CHOICES
    EXTRA_CANNULA_SMALL = "C-SM"  #: Constant for TYPE_CHOICES
    EXTRA_CANNULA_LARGE = "C-LG"  #: Constant for TYPE_CHOICES
    EXTRA_PATCH_HOLDER_SMALL = "PH-SM"  #: Constant for TYPE_CHOICES
    EXTRA_PATCH_HOLDER_LARGE = "PH-LG"  #: Constant for TYPE_CHOICES
    EXTRA_DOUBLE_CANNULA_SET = "DB-C"  #: Constant for TYPE_CHOICES
    PERFUSATE_SOLUTION = "P"  #: Constant for TYPE_CHOICES
    TYPE_CHOICES = (
        (DISPOSABLES, _("PRc01 Disposables")),
        (EXTRA_CANNULA_SMALL, _("PRc02 Extra cannula small (3mm)")),
        (EXTRA_CANNULA_LARGE, _("PRc03 Extra cannula large (5mm)")),
        (EXTRA_PATCH_HOLDER_SMALL, _("PRc04 Extra patch holder small")),
        (EXTRA_PATCH_HOLDER_LARGE, _("PRc05 Extra patch holder large")),
        (EXTRA_DOUBLE_CANNULA_SET, _("PRc06 Extra double cannula set")),
        (PERFUSATE_SOLUTION, _("PRc07 Perfusate solution"))
    )  #: ProcurementResource type choices

    organ = models.ForeignKey(Organ, verbose_name=_('PR01 related kidney'))
    type = models.CharField(verbose_name=_('PR02 resource used'), choices=TYPE_CHOICES, max_length=5)
    lot_number = models.CharField(verbose_name=_('PR03 lot number'), max_length=50, blank=True)
    expiry_date = models.DateField(verbose_name=_('PR04 expiry date'), blank=True, null=True)
    expiry_date_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")

    objects = ProcurementResourceModelForUserManager()

    class Meta:
        verbose_name = _('PRm1 procurement resource')
        verbose_name_plural = _('PRm2 procurement resources')
        permissions = (
            ("view_procurementresource", "Can only view the data"),
        )

    def clean(self):
        """
        Clears the value of expiry_date if marked as unknown
        """
        # Clean the fields that at Not Known
        if self.expiry_date_unknown:
            self.expiry_date = None

    def __str__(self):
        return self.get_type_display() + ' for ' + self.organ.trial_id
