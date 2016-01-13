#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals
import datetime
from random import random
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models, transaction
from django.utils.translation import ugettext_lazy as _
from django.utils import timezone
from wp4.staff_person.models import StaffJob, StaffPerson
from wp4.locations.models import Hospital
from ..validators import validate_between_1900_2050, validate_not_in_future
from .core import VersionControlModel, OrganPerson, RetrievalTeam
from .core import YES_NO_UNKNOWN_CHOICES, PRESERVATION_HMP, PRESERVATION_HMPO2, PRESERVATION_NOT_SET
from .core import LEFT, RIGHT


class Donor(VersionControlModel):
    # Donor Form Case data
    person = models.OneToOneField(OrganPerson)  # Internal link
    sequence_number = models.PositiveSmallIntegerField(default=0)  # Internal value
    multiple_recipients = models.PositiveSmallIntegerField(
        verbose_name=_('DO02 Multiple recipients'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    form_completed = models.BooleanField(default=False)  # Internal value
    admin_notes = models.TextField(verbose_name=_("DO50 Admin notes"), blank=True)

    # Procedure data
    retrieval_team = models.ForeignKey(RetrievalTeam, verbose_name=_("DO01 retrieval team"))
    perfusion_technician = models.ForeignKey(
        StaffPerson,
        verbose_name=_('DO03 name of transplant technician'),
        limit_choices_to={"jobs": StaffJob.PERFUSION_TECHNICIAN},
        related_name="donor_perfusion_technician_set")
    transplant_coordinator = models.ForeignKey(
        StaffPerson,
        verbose_name=_('DO04 name of the SN-OD'),
        limit_choices_to={"jobs": StaffJob.TRANSPLANT_COORDINATOR},
        related_name="donor_transplant_coordinator_set",
        blank=True,
        null=True)
    call_received = models.DateTimeField(verbose_name=_('DO05 Consultant to MTO called at'), blank=True, null=True,
                                         validators=[validate_between_1900_2050, validate_not_in_future])
    call_received_unknown = models.BooleanField(default=False)  # Internal flag
    retrieval_hospital = models.ForeignKey(Hospital, verbose_name=_('DO06 donor hospital'), blank=True, null=True)
    scheduled_start = models.DateTimeField(verbose_name=_('DO07 time of withdrawal therapy'), blank=True, null=True,
                                           validators=[validate_between_1900_2050])
    scheduled_start_unknown = models.BooleanField(default=False)  # Internal flag
    technician_arrival = models.DateTimeField(verbose_name=_('DO08 arrival time of technician'), blank=True, null=True,
                                              validators=[validate_between_1900_2050, validate_not_in_future])
    technician_arrival_unknown = models.BooleanField(default=False)  # Internal flag
    ice_boxes_filled = models.DateTimeField(verbose_name=_('DO09 ice boxes filled'), blank=True, null=True,
                                            validators=[validate_between_1900_2050, validate_not_in_future])
    ice_boxes_filled_unknown = models.BooleanField(default=False)  # Internal flag
    depart_perfusion_centre = models.DateTimeField(
        verbose_name=_('DO10 departure from base hospital at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    depart_perfusion_centre_unknown = models.BooleanField(default=False)  # Internal flag
    arrival_at_donor_hospital = models.DateTimeField(
        verbose_name=_('DO11 arrival at donor hospital'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    arrival_at_donor_hospital_unknown = models.BooleanField(default=False)  # Internal flag

    # Donor details (in addition to OrganPerson)
    age = models.PositiveSmallIntegerField(
        verbose_name=_('DO12 age'),
        validators=[MinValueValidator(50), MaxValueValidator(99)])
    date_of_admission = models.DateField(verbose_name=_('DO13 date of admission'), blank=True, null=True,
                                         validators=[validate_between_1900_2050, validate_not_in_future])
    date_of_admission_unknown = models.BooleanField(default=False)  # Internal flag
    admitted_to_itu = models.BooleanField(verbose_name=_('DO14 admitted to ITU'), default=False)
    date_admitted_to_itu = models.DateField(verbose_name=_('DO15 when admitted to ITU'), blank=True, null=True,
                                            validators=[validate_between_1900_2050, validate_not_in_future])
    date_admitted_to_itu_unknown = models.BooleanField(default=False)  # Internal flag
    date_of_procurement = models.DateField(verbose_name=_('DO16 date of procurement'), blank=True, null=True,
                                           validators=[validate_between_1900_2050, validate_not_in_future])
    other_organs_procured = models.BooleanField(verbose_name=_("DO17 other organs procured"), default=False)
    other_organs_lungs = models.BooleanField(verbose_name=_("DO18 lungs"), default=False)
    other_organs_pancreas = models.BooleanField(verbose_name=_("DO19 pancreas"), default=False)
    other_organs_liver = models.BooleanField(verbose_name=_("DO20 liver"), default=False)
    other_organs_tissue = models.BooleanField(verbose_name=_("DO21 tissue"), default=False)

    # DonorPreop data
    DIAGNOSIS_CEREBROVASCULAR_ACCIDENT = 1
    DIAGNOSIS_HYPOXIA = 2
    DIAGNOSIS_TRAUMA = 3
    DIAGNOSIS_OTHER = 4
    DIAGNOSIS_CHOICES = (
        (DIAGNOSIS_CEREBROVASCULAR_ACCIDENT, _("DOc01 Cerebrovascular Accident")),
        (DIAGNOSIS_HYPOXIA, _("DOc02 Hypoxia")),
        (DIAGNOSIS_TRAUMA, _("DOc03 Trauma")),
        (DIAGNOSIS_OTHER, _("DOc04 Other")))
    diagnosis = models.PositiveSmallIntegerField(verbose_name=_('DO22 diagnosis'), choices=DIAGNOSIS_CHOICES,
                                                 blank=True, null=True)
    diagnosis_other = models.CharField(verbose_name=_('DO23 other diagnosis'), max_length=250, blank=True)
    diabetes_melitus = models.PositiveSmallIntegerField(
        verbose_name=_('DO24 diabetes mellitus'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )
    alcohol_abuse = models.PositiveSmallIntegerField(
        verbose_name=_('DO25 alcohol abuse'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    cardiac_arrest = models.NullBooleanField(
        verbose_name=_('DO26 cardiac arrest'),  # 'Cardiac Arrest (During ITU stay, prior to Retrieval Procedure)',
        blank=True, null=True)
    systolic_blood_pressure = models.PositiveSmallIntegerField(
        verbose_name=_('DO27 last systolic blood pressure'),  # "Last Systolic Blood Pressure (Before switch off)",
        validators=[MinValueValidator(10), MaxValueValidator(200)],
        blank=True, null=True)
    diastolic_blood_pressure = models.PositiveSmallIntegerField(
        verbose_name=_('DO28 last diastolic blood pressure'),  # "Last Diastolic Blood Pressure (Before switch off)",
        validators=[MinValueValidator(10), MaxValueValidator(200)],
        blank=True, null=True)
    diuresis_last_day = models.PositiveSmallIntegerField(
        verbose_name=_('DO29 diuresis last day (ml)'),
        blank=True, null=True)
    diuresis_last_day_unknown = models.BooleanField(default=False)  # Internal flag
    diuresis_last_hour = models.PositiveSmallIntegerField(
        verbose_name=_('DO30 diuresis last hour (ml)'),
        blank=True, null=True)
    diuresis_last_hour_unknown = models.BooleanField(default=False)  # Internal flag
    dopamine = models.PositiveSmallIntegerField(
        verbose_name=_('DO31 dopamine'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    dobutamine = models.PositiveSmallIntegerField(
        verbose_name=_('DO32 dobutamine'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    nor_adrenaline = models.PositiveSmallIntegerField(
        verbose_name=_('DO33 nor adrenaline'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    vasopressine = models.PositiveSmallIntegerField(
        verbose_name=_('DO34 vasopressine'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    other_medication_details = models.CharField(
        verbose_name=_('DO35 other medication'),
        max_length=250,
        blank=True)

    # Lab results
    UNIT_MGDL = 1
    UNIT_UMOLL = 2
    UNIT_CHOICES = (
        (UNIT_MGDL, "mg/dl"),
        (UNIT_UMOLL, "umol/L"))
    last_creatinine = models.FloatField(verbose_name=_('DO36 last creatinine'), validators=[MinValueValidator(0.0), ],
                                        blank=True, null=True)
    last_creatinine_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)
    max_creatinine = models.FloatField(verbose_name=_('DO37 max creatinine'), blank=True, null=True)
    max_creatinine_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)

    # Operation Data - Extraction
    SOLUTION_UW = 1
    SOLUTION_MARSHALL = 2
    SOLUTION_HTK = 3
    SOLUTION_OTHER = 4
    SOLUTION_CHOICES = (
        (SOLUTION_HTK, "HTK"),
        (SOLUTION_MARSHALL, "Marshall's"),
        (SOLUTION_UW, "UW"),
        (SOLUTION_OTHER, _("DOc04 Other")))
    life_support_withdrawal = models.DateTimeField(
        verbose_name=_('DO38 withdrawal of life support'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    systolic_pressure_low = models.DateTimeField(
        verbose_name=_('DO39 systolic arterial pressure'),  # < 50 mm Hg (inadequate organ perfusion)
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    systolic_pressure_low_unknown = models.BooleanField(default=False)  # Internal flag
    o2_saturation = models.DateTimeField(
        verbose_name=_('DO40 O2 saturation below 80%'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    o2_saturation_unknown = models.BooleanField(default=False)  # Internal flag
    circulatory_arrest = models.DateTimeField(
        verbose_name=_('DO41 end of cardiac output'),  # (=start of no touch period)',
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    circulatory_arrest_unknown = models.BooleanField(default=False)  # Internal flag
    length_of_no_touch = models.PositiveSmallIntegerField(
        verbose_name=_('DO42 length of no touch period (minutes)'),
        blank=True, null=True,
        validators=[MinValueValidator(1), MaxValueValidator(60)])
    death_diagnosed = models.DateTimeField(
        verbose_name=_('DO43 knife to skin time'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    perfusion_started = models.DateTimeField(
        verbose_name=_('DO44 start in-situ cold perfusion'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    perfusion_started_unknown = models.BooleanField(default=False)  # Internal flag
    systemic_flush_used = models.PositiveSmallIntegerField(
        verbose_name=_('DO45 systemic (aortic) flush solution used'),
        choices=SOLUTION_CHOICES,
        blank=True, null=True)
    systemic_flush_used_other = models.CharField(
        verbose_name=_('DO46 systemic flush used'),
        max_length=250,
        blank=True)
    systemic_flush_volume_used = models.PositiveSmallIntegerField(
        # TODO: this doesn't appear on the paper forms??
        verbose_name=_('DO47 aortic - volume (ml)'),
        blank=True, null=True)
    heparin = models.NullBooleanField(
        verbose_name=_('DO48 heparin'),  # (administered to donor/in flush solution)
        blank=True, null=True)

    class Meta:
        order_with_respect_to = 'retrieval_team'
        verbose_name = _('DOm1 donor')
        verbose_name_plural = _('DOm2 donors')

    def clean(self):
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
                    _(
                        "DOv01 Time travel detected! Arrival at donor hospital occurred before departure from "
                        "perfusion centre")
                )
        if self.person_id is not None and self.person.date_of_birth:
            if self.date_of_procurement:
                age_difference = self.date_of_procurement - self.person.date_of_birth
                age_difference_in_years = age_difference.days / 365.2425
                if age_difference < datetime.timedelta(days=(365.2425 * 50)):
                    raise ValidationError(
                        _("DOv03 Date of birth is less than 50 years from the date of procurement (%(num)d)"
                          % {'num': age_difference_in_years})
                    )
                if age_difference > datetime.timedelta(days=(365.2425 * 100)):
                    raise ValidationError(
                        _("DOv04 Date of birth is more than 100 years from the date of procurement (%(num)d)"
                          % {'num': age_difference_in_years})
                    )
            if self.age != self.person.age_from_dob():
                raise ValidationError(
                    _("DOv05 Age does not match age as calculated (%(num)d years) from Date of Birth"
                      % {'num': self.person.age_from_dob()})
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

        if self.form_completed:
            if self.retrieval_hospital is None:
                raise ValidationError(_("DOv12 Missing retrieval hospital"))
            if self.person.number == "":
                raise ValidationError(_("DOv13 Please enter the NHSBT number"))

    @transaction.atomic
    def randomise(self):
        # Randomise if eligible and not already done
        left_kidney = self.left_kidney()
        right_kidney = self.right_kidney()
        if left_kidney.preservation == PRESERVATION_NOT_SET \
                and self.multiple_recipients is not False \
                and left_kidney.transplantable \
                and right_kidney.transplantable:
            # left_o2 = random() >= 0.5  # True/False
            left_o2 = Randomisation.get_and_assign_result(self.retrieval_team.based_at.country, self)
            if left_o2:
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

    def __unicode__(self):
        return '%s' % (self.trial_id())

    def left_kidney(self):
        # Emulate a get_or_create call here
        try:
            return self.organ_set.filter(location__exact=LEFT)[0]
        except IndexError:  # Organ.DoesNotExist:
            new_organ = Organ(location=LEFT, created_by=self.created_by)
            if self.id > 0:
                new_organ.donor = self
            new_organ.save()
            return new_organ

    def right_kidney(self):
        # Emulate a get_or_create call here
        try:
            return self.organ_set.filter(location__exact=RIGHT)[0]
        except IndexError:  # Organ.DoesNotExist:
            new_organ = Organ(location=RIGHT, created_by=self.created_by)
            if self.id > 0:
                new_organ.donor = self
            new_organ.save()
            return new_organ

    def centre_code(self):
        try:
            return self.retrieval_team.centre_code
        except RetrievalTeam.DoesNotExist:
            return 0

    centre_code.short_description = 'Centre Code'

    def trial_id(self):
        if self.centre_code() == 0 or self.sequence_number < 1:
            return "No Trial ID Assigned (DO%s)" % format(self.id, '03')
        return 'WP4%s%s' % (format(self.centre_code(), '02'), format(self.sequence_number, '03'))

    trial_id.short_description = 'Trial ID'

    def is_randomised(self):
        if self.left_kidney().preservation == PRESERVATION_NOT_SET:
            return False
        return True

    def is_eligible(self):
        """
        :return: Number of eligible kidneys from this donor. 0, 1, or 2 kidneys, and -1 for not randomised
        """
        eligible_kidney_count = 0
        left_kidney = self.left_kidney()
        right_kidney = self.right_kidney()
        if left_kidney.preservation != PRESERVATION_NOT_SET \
                and self.multiple_recipients is not False:
            if left_kidney.transplantable:
                eligible_kidney_count += 1
            if right_kidney.transplantable:
                eligible_kidney_count += 1
        else:
            eligible_kidney_count = -1
        return eligible_kidney_count


LIVE_UNITED_KINGDOM = 1
LIVE_EUROPE = 2
PAPER_EUROPE = 3
PAPER_UNITED_KINGDOM = 4
LIST_CHOICES = (
    (LIVE_UNITED_KINGDOM, _("RNc01 UK Live list")),
    (LIVE_EUROPE, _("RNc02 Europe Live list")),
    (PAPER_UNITED_KINGDOM, _("RNc03 UK Offline list")),
    (PAPER_EUROPE, _("RNc04 Europe Offline list")),
)


def random_5050():
    return random() >= 0.5  # True/False


class Randomisation(models.Model):
    """
    Populated from the supplied CSV file via the fixture. A 'True' result is HMP+O2 for the Left Organ
    """
    donor = models.OneToOneField(Donor, null=True, blank=True, default=None)  # Internal key
    list_code = models.PositiveSmallIntegerField(verbose_name=_("RA01 list code"), choices=LIST_CHOICES)
    result = models.BooleanField(verbose_name=_("RA02 result"), default=random_5050)
    allocated_on = models.DateTimeField(verbose_name=_("RA03 allocated on"), default=timezone.now)

    @staticmethod
    def get_and_assign_result(list_code, link_donor):
        options = Randomisation.objects.filter(country=list_code, donor=None).order_by('id')
        if len(options) < 1:
            raise Exception("No remaining values for randomisation")
        result = options[0]
        result.donor = link_donor
        result.allocated_on = timezone.now()
        result.save()
        return result.result