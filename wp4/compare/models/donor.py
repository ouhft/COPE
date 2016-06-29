#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from bdateutil import relativedelta
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models, transaction
from django.utils import timezone
from django.utils.functional import cached_property
from django.utils.translation import ugettext_lazy as _
from random import random

from wp4.staff_person.models import StaffJob, StaffPerson
from wp4.locations.models import Hospital

from ..validators import validate_between_1900_2050, validate_not_in_future
from .core import VersionControlMixin, OrganPerson, RetrievalTeam
from .core import YES_NO_UNKNOWN_CHOICES, PRESERVATION_HMP, PRESERVATION_HMPO2, PRESERVATION_NOT_SET
from .core import LEFT, RIGHT
from .core import LIST_CHOICES
from .core import PAPER_EUROPE, PAPER_UNITED_KINGDOM


class Donor(VersionControlMixin):
    """
    Extension of an OrganPerson record (via OneToOne link for good ORM/DB management) to capture
    the Donor specific data

    Also holds the meta-data specific to the Procurement Form
    """
    person = models.OneToOneField(OrganPerson, help_text="Internal link to OrganPerson")
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

    # Procedure data
    retrieval_team = models.ForeignKey(RetrievalTeam, verbose_name=_("DO01 retrieval team"))
    perfusion_technician = models.ForeignKey(
        StaffPerson,
        verbose_name=_('DO03 name of transplant technician'),
        limit_choices_to={"jobs": StaffJob.PERFUSION_TECHNICIAN},
        related_name="donor_perfusion_technician_set"
    )  #: Choices limited to staff with StaffJob.PERFUSION_TECHNICIAN set
    transplant_coordinator = models.ForeignKey(
        StaffPerson,
        verbose_name=_('DO04 name of the SN-OD'),
        limit_choices_to={"jobs": StaffJob.TRANSPLANT_COORDINATOR},
        related_name="donor_transplant_coordinator_set",
        blank=True,
        null=True
    )  #: Choices limited to staff with StaffJob.TRANSPLANT_COORDINATOR set
    call_received = models.DateTimeField(
        verbose_name=_('DO05 Consultant to MTO called at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    call_received_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    retrieval_hospital = models.ForeignKey(Hospital, verbose_name=_('DO06 donor hospital'), blank=True, null=True)
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

    # Donor details (in addition to OrganPerson)
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
    )  #: Changes to this require sync to the OrganPerson.date_of_death, see the save method
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

    class Meta:
        order_with_respect_to = 'retrieval_team'
        verbose_name = _('DOm1 donor')
        verbose_name_plural = _('DOm2 donors')

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
            if self.retrieval_hospital is None:
                raise ValidationError(_("DOv12 Missing retrieval hospital"))
            if self.person.number == "":
                raise ValidationError(_("DOv13 Please enter the NHSBT number"))

    def save(self, created_by=None, force_insert=False, force_update=False, using=None, update_fields=None):
        """
        Updated save method to ensure that if death_diagnosed is set, then this is reflected in the
        same value for person.date_of_death

        :param created_by: User. As parent method
        :param force_insert: bool. As parent method
        :param force_update: bool. As parent method
        :param using:  As parent method
        :param update_fields:  As parent method
        :return: super(Donor, self).save()
        """
        if self.death_diagnosed:
            # Have to sync this data to the OrganPerson.date_of_death
            self.person.date_of_death = self.death_diagnosed
            self.person.save(created_by)
        return super(Donor, self).save(created_by, force_insert, force_update, using, update_fields)

    @transaction.atomic
    def randomise(self, is_online=True):
        """
        Randomise if eligible and not already done. This is determined by looking at the elibility
        criteria, and checking that the Organ.preservation is not set. Once randomised, the
        sequence_number is allocated for the Trial ID to be defined. The location of the
        retrieval_team is used to determine which geographical list set to use, and the is_online
        value determines which of the two lists to pick from.

        :param is_online: bool. Pick from an online list if True, or an Offline list if False
        :return: True if randomisation occurs, False otherwise
        :rtype: bool
        """
        #
        left_kidney = self.left_kidney
        right_kidney = self.right_kidney
        if left_kidney.preservation == PRESERVATION_NOT_SET \
                and self.multiple_recipients is not False \
                and left_kidney.transplantable \
                and right_kidney.transplantable:
            if Randomisation.get_and_assign_result(
                self.retrieval_team.get_randomisation_list(is_online),
                self
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
        import wp4.compare.models.organ as organ_model
        if self._left_kidney is None:
            try:
                self._left_kidney = self.organ_set.filter(location__exact=LEFT)[0]
            except IndexError:  # Organ.DoesNotExist:
                new_organ = organ_model.Organ(location=LEFT, created_by=self.created_by)
                if self.id > 0:
                    new_organ.donor = self
                new_organ.save()
                self._left_kidney = new_organ
            # Consider saving self here... if we could determine created_by is set
        return self._left_kidney

    @property
    def right_kidney(self):
        """
        Property to emulate a safe get_or_create call to Organ. If no linked organ, a new record is
        created with sensible defaults and linked.

        :return: Organ record for the right kidney
        :rtype: wp4.compare.models.organ.Organ
        """
        import wp4.compare.models.organ as organ_model
        if self._right_kidney is None:
            try:
                self._right_kidney = self.organ_set.filter(location__exact=RIGHT)[0]
            except IndexError:  # Organ.DoesNotExist:
                new_organ = organ_model.Organ(location=RIGHT, created_by=self.created_by)
                if self.id > 0:
                    new_organ.donor = self
                new_organ.save()
                self._right_kidney = new_organ
            # Consider saving self here... if we could determine created_by is set
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

    def _trial_id(self):
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
            trial_id = "No Trial ID Assigned (DO%s)" % format(self.id, '03')
        elif self.is_offline:
            trial_id += "9" + format(self.sequence_number, '02')
        else:
            trial_id += format(self.sequence_number, '03')
        return trial_id
    trial_id = cached_property(_trial_id, name='trial_id')

    def _is_offline(self):
        """
        Determine if this was randomised using the relevant online or offline lists

        :return: True, if randomisation.list_code matches PAPER_EUROPE or PAPER_UNITED_KINGDOM
        :rtype: bool
        """
        try:
            return self.randomisation.list_code in [PAPER_EUROPE, PAPER_UNITED_KINGDOM]
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

    def _is_eligible(self):
        """
        Number of eligible kidneys from this donor.

        :return: 0, 1, or 2 kidneys, and -1 for not randomised
        :rtype: int
        """
        eligible_kidney_count = 0
        left_kidney = self.left_kidney
        right_kidney = self.right_kidney
        if left_kidney.preservation != PRESERVATION_NOT_SET \
                and self.multiple_recipients is not False:
            if left_kidney.transplantable:
                eligible_kidney_count += 1
            if right_kidney.transplantable:
                eligible_kidney_count += 1
        else:
            eligible_kidney_count = -1
        return eligible_kidney_count
    is_eligible = cached_property(_is_eligible, name='is_eligible')


def random_5050():
    """
    Handy method to return a True/False value with a 0.5 distribution using random()

    :return: True or False
    :rtype: bool
    """
    return random() >= 0.5  # True/False


class Randomisation(models.Model):
    """
    Populated from the supplied CSV file via the fixture. A 'True' result is HMP+O2 for the Left Organ
    """
    donor = models.OneToOneField(
        Donor,
        null=True, blank=True,
        default=None,
        help_text="Internal link to the Donor"
    )
    list_code = models.PositiveSmallIntegerField(
        verbose_name=_("RA01 list code"),
        choices=LIST_CHOICES
    )  #: Choices limited to LIST_CHOICES
    result = models.BooleanField(verbose_name=_("RA02 result"), default=random_5050)
    allocated_on = models.DateTimeField(verbose_name=_("RA03 allocated on"), default=timezone.now)

    @staticmethod
    def get_and_assign_result(list_code, link_donor):
        """
        Finds the next unassigned record on the randomisation list, and then assigns it to the
        Donor record supplied, whilst returning the result

        :param list_code: int. Value from LIST_CHOICES
        :param link_donor: Donor object. Donor record to link against this randomisation
        :return: 'True' result is HMP+O2 for the Left Organ
        :rtype: bool
        """
        options = Randomisation.objects.filter(list_code=list_code, donor=None).order_by('id')
        if len(options) < 1:
            raise Exception("No remaining values for randomisation")
        result = options[0]
        result.donor = link_donor
        result.allocated_on = timezone.now()
        result.save()
        return result.result

    def __str__(self):
        return '%s : %s' % (format(self.id, '03'), self.get_list_code_display())
