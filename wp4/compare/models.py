#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals
from random import random
import datetime
from django.conf import settings
from django.contrib.auth.models import User
from django.core.exceptions import ObjectDoesNotExist
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models, transaction
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _
from ..staff_person.models import StaffJob, StaffPerson
from ..locations.models import Hospital, UNITED_KINGDOM, BELGIUM, NETHERLANDS, COUNTRY_CHOICES
from ..perfusion_machine.models import PerfusionFile, PerfusionMachine
from .validators import validate_between_1900_2050, validate_not_in_past, validate_not_in_future

# Common CONSTANTS
NO = 0
YES = 1
UNKNOWN = 2
YES_NO_UNKNOWN_CHOICES = (
    (UNKNOWN, _("MMc03 Unknown")),
    (NO, _("MMc01 No")),
    (YES, _("MMc02 Yes"))
)  # Need Yes to be the last choice for any FieldWithFollowUp

LEFT = "L"
RIGHT = "R"
LOCATION_CHOICES = (
    (LEFT, _('ORc01 Left')),
    (RIGHT, _('ORc02 Right')))


def random_5050():
    return random() >= 0.5  # True/False


class VersionControlModel(models.Model):
    version = models.PositiveIntegerField(default=0)
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)
    record_locked = models.BooleanField(default=False)

    # TODO: Add save method here that aborts saving if record_locked is already true
    #  TODO: Add version control via django-reversion
    # NB: Used in Samples App also
    class Meta:
        abstract = True


class RetrievalTeam(models.Model):
    centre_code = models.PositiveSmallIntegerField(
        verbose_name=_("RT01 centre code"),
        validators=[MinValueValidator(10), MaxValueValidator(99)])
    based_at = models.ForeignKey(Hospital, verbose_name=_("RT02 base hospital"))
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    def next_sequence_number(self):
        try:
            number = self.donor_set.latest('sequence_number').sequence_number + 1
        except Donor.DoesNotExist:
            number = 1
        return number

    def name(self):
        return '(%d) %s' % (self.centre_code, self.based_at.full_description())

    def __unicode__(self):
        return self.name()

    class Meta:
        ordering = ['centre_code']
        verbose_name = _('RTm1 retrieval team')
        verbose_name_plural = _('RTm2 retrieval teams')


class OrganPerson(VersionControlModel):
    """
    Base attributes for a person involved in this case as a donor or recipient
    """
    MALE = 'M'
    FEMALE = 'F'
    GENDER_CHOICES = (
        (MALE, _('OPc01 Male')),
        (FEMALE, _('OPc02 Female'))
    )

    CAUCASIAN = 1
    BLACK = 2
    OTHER_ETHNICITY = 3
    ETHNICITY_CHOICES = (
        (CAUCASIAN, _('OPc03 Caucasian')),
        (BLACK, _('OPc04 Black')),
        (OTHER_ETHNICITY, _('OPc05 Other')))

    BLOOD_O = 1
    BLOOD_A = 2
    BLOOD_B = 3
    BLOOD_AB = 4
    BLOOD_UNKNOWN = 5
    BLOOD_GROUP_CHOICES = (
        (BLOOD_O, 'O'),
        (BLOOD_A, 'A'),
        (BLOOD_B, 'B'),
        (BLOOD_AB, 'AB'),
        (BLOOD_UNKNOWN, _('OPc06 Unknown')))

    # "ET Donor number/ NHSBT Number",
    number = models.CharField(verbose_name=_('OP01 NHSBT Number'), max_length=20, blank=True)
    date_of_birth = models.DateField(verbose_name=_('OP02 date of birth'), blank=True, null=True,
                                     validators=[validate_not_in_future])
    date_of_birth_unknown = models.BooleanField(default=False)  # Internal flag
    # May be possible to get DoD from donor.death_diagnosed
    # date_of_death = models.DateField(verbose_name=_('OP08 date of death'), blank=True, null=True)
    # date_of_death_unknown = models.BooleanField(default=False)  # Internal flag
    gender = models.CharField(verbose_name=_('OP03 gender'), choices=GENDER_CHOICES, max_length=1, default=MALE)
    weight = models.PositiveSmallIntegerField(
        verbose_name=_('OP04 Weight (kg)'),
        validators=[MinValueValidator(20), MaxValueValidator(200)],
        blank=True, null=True)
    height = models.PositiveSmallIntegerField(
        verbose_name=_('OP05 Height (cm)'),
        validators=[MinValueValidator(100), MaxValueValidator(250)],
        blank=True, null=True)
    ethnicity = models.IntegerField(verbose_name=_('OP06 ethnicity'), choices=ETHNICITY_CHOICES, blank=True, null=True)
    blood_group = models.PositiveSmallIntegerField(
        verbose_name=_('OP07 blood group'),
        choices=BLOOD_GROUP_CHOICES,
        blank=True, null=True)

    class Meta:
        ordering = ['number']
        verbose_name = _('OPm1 trial person')
        verbose_name_plural = _('OPm2 organ people')

    def clean(self):
        # Clean the fields that at Not Known
        if self.date_of_birth_unknown:
            self.date_of_birth = None
        # if self.date_of_death_unknown:
        #     self.date_of_death = None

        # if self.date_of_birth:
        #     if self.date_of_birth > datetime.datetime.now().date():
        #         raise ValidationError(_("OPv01 Time travel detected! Person's date of birth is in the future!"))

        if self.date_of_death:
            if self.date_of_death > datetime.datetime.now().date():
                raise ValidationError(_("OPv02 Creepy prediction! Person's date of death is in the future!"))

        if self.date_of_birth and self.date_of_death:
            if self.date_of_death < self.date_of_birth:
                raise ValidationError(
                    _("OPv03 Time running backwards! Person's date of death is before they were born!"))

    def bmi_value(self):
        # http://www.nhs.uk/chq/Pages/how-can-i-work-out-my-bmi.aspx?CategoryID=51 for formula
        if self.height < 1 or self.weight < 1:
            return _("DOv12 Not Available")
        height_in_m = self.height / 100
        return (self.weight / height_in_m) / height_in_m

    bmi_value.short_description = 'BMI Value'

    def age_from_dob(self):
        # TODO: Get the date calculations fixed so they do actual math and then show the years
        today = datetime.date.today()
        if not self.date_of_birth:
            return None
        elif self.date_of_death:
            years = self.date_of_death.year - self.date_of_birth.year
        elif self.date_of_birth < today:
            years = today.year - self.date_of_birth.year
        else:
            years = today.year - self.date_of_birth.year - 1
        return years

    def trial_id(self):
        if self.is_donor:
            return self.donor.trial_id()
        if self.is_recipient:
            return self.recipient.trial_id()
        return _("OPm01 No Trial ID Assigned")

    @property
    def is_recipient(self):
        try:
            return self.recipient is not None
        except ObjectDoesNotExist:
            return False

    @property
    def is_donor(self):
        try:
            return self.donor is not None
        except ObjectDoesNotExist:
            return False

    @property
    def date_of_death(self):
        if self.is_donor and self.donor.death_diagnosed:
            return self.donor.death_diagnosed.date()
        return None

    @property
    def worksheet(self):
        for worksheet in self.worksheet_set.all():
            return worksheet
        return None

    def __unicode__(self):
        if settings.DEBUG:
            return '%s : (%s, %s) %s' % (
                self.id, self.get_gender_display(), self.age_from_dob(), self.number
            )
        else:
            return '(%s, %s) %s' % (
                self.get_gender_display(), self.age_from_dob(), self.number
            )


class Donor(VersionControlModel):
    # Donor Form Case data
    person = models.OneToOneField(OrganPerson)  # Internal link
    sequence_number = models.PositiveSmallIntegerField(default=0)  # Internal value
    multiple_recipients = models.PositiveSmallIntegerField(
        verbose_name=_('DO02 Multiple recipients'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    form_completed = models.BooleanField(default=False)  # Internal value

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
    diabetes_melitus = models.PositiveSmallIntegerField(verbose_name=_('DO24 diabetes mellitus'),
                                                        choices=YES_NO_UNKNOWN_CHOICES, blank=True, null=True)
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
        ordering = ['sequence_number']
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
        if left_kidney.preservation == Organ.PRESERVATION_NOT_SET \
                and self.multiple_recipients is not False \
                and left_kidney.transplantable \
                and right_kidney.transplantable:
            # left_o2 = random() >= 0.5  # True/False
            left_o2 = Randomisation.get_and_assign_result(self.retrieval_team.based_at.country, self)
            if left_o2:
                left_kidney.preservation = Organ.PRESERVATION_HMPO2
                right_kidney.preservation = Organ.PRESERVATION_HMP
            else:
                left_kidney.preservation = Organ.PRESERVATION_HMP
                right_kidney.preservation = Organ.PRESERVATION_HMPO2
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
        if self.left_kidney().preservation == Organ.PRESERVATION_NOT_SET:
            return False
        return True

    def is_eligible(self):
        """
        :return: Number of eligible kidneys from this donor. 0, 1, or 2 kidneys, and -1 for not randomised
        """
        eligible_kidney_count = 0
        left_kidney = self.left_kidney()
        right_kidney = self.right_kidney()
        if left_kidney.preservation != Organ.PRESERVATION_NOT_SET \
                and self.multiple_recipients is not False:
            if left_kidney.transplantable:
                eligible_kidney_count += 1
            if right_kidney.transplantable:
                eligible_kidney_count += 1
        else:
            eligible_kidney_count = -1
        return eligible_kidney_count


class Organ(VersionControlModel):  # Or specifically, a Kidney
    donor = models.ForeignKey(Donor)  # Internal value
    location = models.CharField(
        verbose_name=_('OR01 kidney location'),
        max_length=1,
        choices=LOCATION_CHOICES)

    # Inspection data
    GRAFT_DAMAGE_ARTERIAL = 1
    GRAFT_DAMAGE_VENOUS = 2
    GRAFT_DAMAGE_URETERAL = 3
    GRAFT_DAMAGE_PARENCHYMAL = 4
    GRAFT_DAMAGE_OTHER = 6
    GRAFT_DAMAGE_NONE = 5
    GRAFT_DAMAGE_CHOICES = (
        (GRAFT_DAMAGE_NONE, _("ORc01 None")),
        (GRAFT_DAMAGE_ARTERIAL, _("ORc02 Arterial Damage")),
        (GRAFT_DAMAGE_VENOUS, _("ORc03 Venous Damage")),
        (GRAFT_DAMAGE_URETERAL, _("ORc04 Ureteral Damage")),
        (GRAFT_DAMAGE_PARENCHYMAL, _("ORc05 Parenchymal Damage")),
        (GRAFT_DAMAGE_OTHER, _("ORc06 Other Damage")))

    WASHOUT_PERFUSION_HOMEGENOUS = 1
    WASHOUT_PERFUSION_PATCHY = 2
    WASHOUT_PERFUSION_BLUE = 3
    WASHOUT_PERFUSION_UNKNOWN = 9
    WASHOUT_PERFUSION_CHOICES = (
        # NHS Form has: Good, Fair, Poor, Patchy, Unknown
        (WASHOUT_PERFUSION_HOMEGENOUS, _("ORc07 Homogenous")),
        (WASHOUT_PERFUSION_PATCHY, _("ORc08 Patchy")),
        (WASHOUT_PERFUSION_BLUE, _("ORc09 Blue")),
        (WASHOUT_PERFUSION_UNKNOWN, _("ORc10 Unknown")))

    removal = models.DateTimeField(verbose_name=_('OR02 time out'), blank=True, null=True,
                                   validators=[validate_between_1900_2050, validate_not_in_future])
    renal_arteries = models.PositiveSmallIntegerField(
        verbose_name=_('OR03 number of renal arteries'),
        blank=True, null=True,
        validators=[MinValueValidator(0), MaxValueValidator(5)])
    graft_damage = models.PositiveSmallIntegerField(
        verbose_name=_('OR04 renal graft damage'),
        choices=GRAFT_DAMAGE_CHOICES,
        default=GRAFT_DAMAGE_NONE)
    graft_damage_other = models.CharField(verbose_name=_('OR05 other damage done'), max_length=250, blank=True)
    washout_perfusion = models.PositiveSmallIntegerField(
        verbose_name=_('OR06 perfusion characteristics'),
        choices=WASHOUT_PERFUSION_CHOICES,
        blank=True, null=True)
    transplantable = models.NullBooleanField(verbose_name=_('OR07 is transplantable'), blank=True, null=True)
    not_transplantable_reason = models.CharField(
        verbose_name=_('OR08 not transplantable because'),
        max_length=250,
        blank=True)

    # Randomisation data
    PRESERVATION_HMP = 0
    PRESERVATION_HMPO2 = 1
    PRESERVATION_NOT_SET = 9
    PRESERVATION_CHOICES = (
        (PRESERVATION_NOT_SET, _("ORc11 Not Set")),
        (PRESERVATION_HMP, "HMP"),
        (PRESERVATION_HMPO2, "HMP O2"))
    # can_donate = models.BooleanField('Donor is eligible as DCD III and > 50 years old') -- donor info!
    # can_transplant = models.BooleanField('') -- derived from left and right being transplantable
    preservation = models.PositiveSmallIntegerField(choices=PRESERVATION_CHOICES, default=PRESERVATION_NOT_SET)

    # Perfusion data
    PATCH_SMALL = 1
    PATCH_LARGE = 2
    PATCH_DOUBLE_ARTERY = 3
    PATCH_HOLDER_CHOICES = (
        (PATCH_SMALL, _("ORc12 Small")),
        (PATCH_LARGE, _("ORc13 Large")),
        (PATCH_DOUBLE_ARTERY, _("ORc14 Double Artery")))
    ARTIFICIAL_PATCH_CHOICES = (
        (PATCH_SMALL, _("ORc12 Small")),
        (PATCH_LARGE, _("ORc13 Large")))
    perfusion_possible = models.NullBooleanField(
        verbose_name=_('OR09 machine perfusion possible?'),
        blank=True, null=True)
    perfusion_not_possible_because = models.CharField(
        verbose_name=_('OR10 not possible because'),
        max_length=250,
        blank=True)
    perfusion_started = models.DateTimeField(verbose_name=_('OR11 machine perfusion'), blank=True, null=True,
                                             validators=[validate_between_1900_2050, validate_not_in_future])
    patch_holder = models.PositiveSmallIntegerField(
        verbose_name=_('OR12 used patch holder'),
        choices=PATCH_HOLDER_CHOICES,
        blank=True, null=True)
    artificial_patch_used = models.NullBooleanField(verbose_name=_('OR13 artificial patch used'), blank=True, null=True)
    artificial_patch_size = models.PositiveSmallIntegerField(
        verbose_name=_('OR14 artificial patch size'),
        choices=ARTIFICIAL_PATCH_CHOICES,
        blank=True, null=True)
    artificial_patch_number = models.PositiveSmallIntegerField(
        verbose_name=_('OR15 number of patches'),
        blank=True,
        null=True,
        validators=[MinValueValidator(1), MaxValueValidator(2)])
    oxygen_bottle_full = models.NullBooleanField(
        verbose_name=_('OR16 is oxygen bottle full'),
        blank=True, null=True)
    oxygen_bottle_open = models.NullBooleanField(verbose_name=_('OR17 oxygen bottle opened'), blank=True, null=True)
    oxygen_bottle_changed = models.NullBooleanField(verbose_name=_('OR18 oxygen bottle changed'), blank=True, null=True)
    oxygen_bottle_changed_at = models.DateTimeField(
        verbose_name=_('OR19 oxygen bottle changed at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    oxygen_bottle_changed_at_unknown = models.BooleanField(default=False)  # Internal flag
    ice_container_replenished = models.NullBooleanField(
        verbose_name=_('OR20 ice container replenished'),
        blank=True, null=True)
    ice_container_replenished_at = models.DateTimeField(
        verbose_name=_('OR21 ice container replenished at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    ice_container_replenished_at_unknown = models.BooleanField(default=False)  # Internal flag
    perfusate_measurable = models.NullBooleanField(
        # logistically possible to measure pO2 perfusate (use blood gas analyser)',
        verbose_name=_('OR22 perfusate measurable'),
        blank=True, null=True)
    perfusate_measure = models.FloatField(verbose_name=_('OR23 value pO2'), blank=True, null=True)
    # TODO: Check the value range for perfusate_measure
    perfusion_machine = models.ForeignKey(PerfusionMachine, verbose_name=_('OR24 perfusion machine'), blank=True,
                                          null=True)
    perfusion_file = models.ForeignKey(PerfusionFile, verbose_name=_('OR25 machine file'), blank=True, null=True)

    def clean(self):
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
            raise ValidationError(_("ORv02 Please enter the time perfusion started at"))

        if self.donor.form_completed:
            pass

    def trial_id(self):
        return self.donor.trial_id() + self.location

    def is_allocated(self):
        for allocation in self.organallocation_set.all():
            if allocation.reallocated == False and self.recipient.allocation is not None:
                return True
        return False

    def reallocation_count(self):
        count = 0
        for allocation in self.organallocation_set.all():
            if allocation.reallocated == True:
                count += 1
        return count

    def __unicode__(self):
        return '%s : %s' % (
            self.trial_id(), "Randomised" if self.donor.is_randomised() else "Not yet eligible"
        )

    class Meta:
        verbose_name = _('ORm1 organ')
        verbose_name_plural = _('ORm2 organs')


class ProcurementResource(models.Model):
    DISPOSABLES = "D"
    EXTRA_CANNULA_SMALL = "C-SM"
    EXTRA_CANNULA_LARGE = "C-LG"
    EXTRA_PATCH_HOLDER_SMALL = "PH-SM"
    EXTRA_PATCH_HOLDER_LARGE = "PH-LG"
    EXTRA_DOUBLE_CANNULA_SET = "DB-C"
    PERFUSATE_SOLUTION = "P"
    TYPE_CHOICES = (
        (DISPOSABLES, _("PRc01 Disposables")),
        (EXTRA_CANNULA_SMALL, _("PRc02 Extra cannula small (3mm)")),
        (EXTRA_CANNULA_LARGE, _("PRc03 Extra cannula large (5mm)")),
        (EXTRA_PATCH_HOLDER_SMALL, _("PRc04 Extra patch holder small")),
        (EXTRA_PATCH_HOLDER_LARGE, _("PRc05 Extra patch holder large")),
        (EXTRA_DOUBLE_CANNULA_SET, _("PRc06 Extra double cannula set")),
        (PERFUSATE_SOLUTION, _("PRc07 Perfusate solution")))
    organ = models.ForeignKey(Organ, verbose_name=_('PR01 related kidney'))
    type = models.CharField(verbose_name=_('PR02 resource used'), choices=TYPE_CHOICES, max_length=5)
    lot_number = models.CharField(verbose_name=_('PR03 lot number'), max_length=50, blank=True)
    expiry_date = models.DateField(verbose_name=_('PR04 expiry date'), blank=True, null=True)
    expiry_date_unknown = models.BooleanField(default=False)  # Internal flag
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    def __unicode__(self):
        return self.get_type_display() + ' for ' + self.organ.trial_id()

    class Meta:
        verbose_name = _('PRm1 procurement resource')
        verbose_name_plural = _('PRm2 procurement resources')

    def clean(self):
        # Clean the fields that at Not Known
        if self.expiry_date_unknown:
            self.expiry_date = None


class OrganAllocation(VersionControlModel):
    organ = models.ForeignKey(Organ)  # Internal link

    #  Allocation data
    REALLOCATION_CROSSMATCH = 1
    REALLOCATION_UNKNOWN = 2
    REALLOCATION_OTHER = 3
    REALLOCATION_CHOICES = (
        (REALLOCATION_CROSSMATCH, _('OAc01 Positive crossmatch')),
        (REALLOCATION_UNKNOWN, _('OAc02 Unknown')),
        (REALLOCATION_OTHER, _('OAc03 Other')))
    perfusion_technician = models.ForeignKey(
        StaffPerson,
        verbose_name=_('OA01 name of transplant technician'),
        limit_choices_to={"jobs": StaffJob.PERFUSION_TECHNICIAN},
        related_name="recipient_perfusion_technician_set",
        blank=True, null=True)
    call_received = models.DateTimeField(
        verbose_name=_('OA02 call received from transplant co-ordinator at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    call_received_unknown = models.BooleanField(default=False)  # Internal flag
    transplant_hospital = models.ForeignKey(Hospital, verbose_name=_('OA03 transplant hospital'))
    theatre_contact = models.ForeignKey(
        StaffPerson,
        verbose_name=_('OA04 name of the theatre contact'),
        limit_choices_to={"jobs": StaffJob.THEATRE_CONTACT},
        related_name="recipient_transplant_coordinator_set",
        blank=True, null=True)
    scheduled_start = models.DateTimeField(verbose_name=_('OA05 scheduled start'), blank=True, null=True,
                                           validators=[validate_between_1900_2050])
    scheduled_start_unknown = models.BooleanField(default=False)  # Internal flag
    technician_arrival = models.DateTimeField(
        verbose_name=_('OA06 arrival time at hub'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    technician_arrival_unknown = models.BooleanField(default=False)  # Internal flag
    depart_perfusion_centre = models.DateTimeField(
        verbose_name=_('OA07 departure from hub'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    depart_perfusion_centre_unknown = models.BooleanField(default=False)  # Internal flag
    arrival_at_recipient_hospital = models.DateTimeField(
        verbose_name=_('OA08 arrival at transplant hospital'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    arrival_at_recipient_hospital_unknown = models.BooleanField(default=False)  # Internal flag
    journey_remarks = models.TextField(verbose_name=_("OA09 journey notes"), blank=True)
    reallocated = models.NullBooleanField(verbose_name=_("OA10 reallocated"), blank=True, default=None)
    reallocation_reason = models.PositiveSmallIntegerField(
        verbose_name=_('OA11 reason for re-allocation'),
        choices=REALLOCATION_CHOICES,
        blank=True, null=True)
    reallocation_reason_other = models.CharField(verbose_name=_('OA12 other reason'), max_length=250, blank=True)
    reallocation = models.OneToOneField('OrganAllocation', default=None, blank=True, null=True)

    class Meta:
        order_with_respect_to = 'organ'
        verbose_name = _('OAm1 organ allocation')
        verbose_name_plural = _('OAm2 organ allocations')
        get_latest_by = 'created_on'

    def clean(self):
        # Clean the fields that at Not Known
        if self.call_received_unknown:
            self.call_received = None
        if self.scheduled_start_unknown:
            self.scheduled_start = None
        if self.technician_arrival_unknown:
            self.technician_arrival = None
        if self.depart_perfusion_centre_unknown:
            self.depart_perfusion_centre = None
        if self.arrival_at_recipient_hospital_unknown:
            self.arrival_at_recipient_hospital = None

    def __unicode__(self):
        try:
            recipient_string = self.recipient
        except AttributeError:
            recipient_string = "None"
        return 'Organ: %s | Recipient: %s' % (self.organ.pk, recipient_string)


class Recipient(VersionControlModel):
    person = models.OneToOneField(OrganPerson)  # Internal link
    organ = models.OneToOneField(Organ)  # Internal link
    allocation = models.OneToOneField(OrganAllocation)  # Internal link
    form_completed = models.BooleanField(default=False)  # Internal value

    # Trial signoffs
    signed_consent = models.NullBooleanField(
        verbose_name=_("RE13 informed consent given"), blank=True, default=None)
    single_kidney_transplant = models.NullBooleanField(
        verbose_name=_("RE14 receiving one kidney"), blank=True, default=None)

    # Recipient details (in addition to OrganPerson)
    RENAL_DISEASE_CHOICES = (
        (1, _('REc04 Glomerular diseases')),
        (2, _('REc05 Polycystic kidneys')),
        (3, _('REc06 Uncertain etiology')),
        (4, _('REc07 Tubular and interstitial diseases')),
        (5, _('REc08 Retransplant graft failure')),
        (6, _('REc09 diabetic nephropathyes')),
        (7, _('REc10 hypertensive nephropathyes')),
        (8, _('REc11 congenital rare disorders')),
        (9, _('REc12 renovascular and other diseases')),
        (10, _('REc13 neoplasms')),
        (11, _('REc14 other')),)
    renal_disease = models.PositiveSmallIntegerField(
        verbose_name=_('RE15 renal disease'),
        choices=RENAL_DISEASE_CHOICES,
        blank=True, null=True)
    renal_disease_other = models.CharField(verbose_name=_('RE16 other renal disease'), max_length=250, blank=True)
    pre_transplant_diuresis = models.PositiveSmallIntegerField(
        verbose_name=_('RE17 diuresis (ml/24hr)'),
        blank=True, null=True)

    # Peri-operative data
    INCISION_CHOICES = (
        (1, _('REc15 midline laparotomy')),
        (2, _('REc16 hockey stick')),
        (3, _('REc17 unknown')))
    ARTERIAL_PROBLEM_CHOICES = (
        (1, _('REc18 None')),
        (2, _('REc19 ligated polar artery')),
        (3, _('REc20 reconstructed polar artery')),
        (4, _('REc21 repaired intima dissection')),
        (5, _('REc22 other')))
    VENOUS_PROBLEM_CHOICES = (
        (1, _('REc23 none')),
        (2, _('REc24 laceration')),
        (3, _('REc25 elongation plasty')),
        (4, _('REc26 other')))
    knife_to_skin = models.DateTimeField(verbose_name=_('RE18 knife to skin time'), blank=True, null=True,
                                         validators=[validate_between_1900_2050, validate_not_in_future])
    perfusate_measure = models.FloatField(verbose_name=_('RE19 pO2 perfusate'), blank=True, null=True)
    # TODO: Check the value range for perfusate_measure
    perfusion_stopped = models.DateTimeField(verbose_name=_('RE20 stop machine perfusion'), blank=True, null=True,
                                             validators=[validate_between_1900_2050, validate_not_in_future])
    organ_cold_stored = models.BooleanField(verbose_name=_('RE21 kidney was cold stored?'), default=False)
    tape_broken = models.NullBooleanField(verbose_name=_('RE22 tape over regulator broken'), blank=True, null=True)
    removed_from_machine_at = models.DateTimeField(
        verbose_name=_('RE23 kidney removed from machine at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    oxygen_full_and_open = models.PositiveSmallIntegerField(
        verbose_name=_('RE24 oxygen full and open'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    organ_untransplantable = models.NullBooleanField(verbose_name=_('RE25 kidney discarded'), blank=True, null=True)
    organ_untransplantable_reason = models.CharField(
        verbose_name=_('RE26 untransplantable because'),
        max_length=250,
        blank=True)
    anesthesia_started_at = models.DateTimeField(verbose_name=_('RE27 start anesthesia at'), blank=True, null=True,
                                                 validators=[validate_between_1900_2050, validate_not_in_future])
    incision = models.PositiveSmallIntegerField(
        verbose_name=_('RE28 incision'),
        choices=INCISION_CHOICES,
        blank=True, null=True)
    transplant_side = models.CharField(verbose_name=_('RE29 transplant side'), max_length=1, choices=LOCATION_CHOICES,
                                       blank=True)
    arterial_problems = models.PositiveSmallIntegerField(
        verbose_name=_('RE30 arterial problems'),
        choices=ARTERIAL_PROBLEM_CHOICES,
        blank=True, null=True)
    arterial_problems_other = models.CharField(verbose_name=_('RE31 arterial problems other'), max_length=250,
                                               blank=True)
    venous_problems = models.PositiveSmallIntegerField(
        verbose_name=_('RE32 venous problems'),
        choices=VENOUS_PROBLEM_CHOICES,
        blank=True, null=True)
    venous_problems_other = models.CharField(verbose_name=_('RE33 venous problems other'), max_length=250, blank=True)
    anastomosis_started_at = models.DateTimeField(verbose_name=_('RE34 start anastomosis at'), blank=True, null=True,
                                                  validators=[validate_between_1900_2050, validate_not_in_future])
    anastomosis_started_at_unknown = models.BooleanField(default=False)  # Internal flag
    reperfusion_started_at = models.DateTimeField(verbose_name=_('RE35 start reperfusion at'), blank=True, null=True,
                                                  validators=[validate_between_1900_2050, validate_not_in_future])
    reperfusion_started_at_unknown = models.BooleanField(default=False)  # Internal flag
    mannitol_used = models.PositiveSmallIntegerField(
        verbose_name=_('RE36 mannitol used'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    other_diurectics = models.PositiveSmallIntegerField(
        verbose_name=_('RE37 other diurectics used'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    other_diurectics_details = models.CharField(verbose_name=_('RE38 other diurectics detail'), max_length=250,
                                                blank=True)
    systolic_blood_pressure = models.PositiveSmallIntegerField(
        verbose_name=_('RE39 systolic blood pressure at reperfusion'),
        validators=[MinValueValidator(10), MaxValueValidator(200)],
        blank=True, null=True)
    cvp = models.PositiveSmallIntegerField(verbose_name=_('RE40 cvp at reperfusion'), blank=True, null=True)
    intra_operative_diuresis = models.PositiveSmallIntegerField(
        verbose_name=_('RE41 intra-operative diuresis'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    successful_conclusion = models.BooleanField(verbose_name=_("RE42 successful conclusion"), default=False)
    operation_concluded_at = models.DateTimeField(verbose_name=_("RE43 operation concluded at"), null=True, blank=True,
                                                  validators=[validate_between_1900_2050, validate_not_in_future])

    # Machine cleanup record
    probe_cleaned = models.NullBooleanField(verbose_name=_('RE44 temperature and flow probe cleaned'), blank=True,
                                            null=True)
    ice_removed = models.NullBooleanField(verbose_name=_('RE45 ice and water removed'), blank=True, null=True)
    oxygen_flow_stopped = models.NullBooleanField(verbose_name=_('RE46 oxygen flow stopped'), blank=True, null=True)
    oxygen_bottle_removed = models.NullBooleanField(verbose_name=_('RE47 oxygen bottle removed'), blank=True, null=True)
    box_cleaned = models.NullBooleanField(verbose_name=_('RE48 box kidney assist cleaned'), blank=True, null=True)
    batteries_charged = models.NullBooleanField(verbose_name=_('RE49 batteries charged'), blank=True, null=True)
    cleaning_log = models.TextField(verbose_name=_("RE50 cleaning log notes"), blank=True)

    class Meta:
        order_with_respect_to = 'organ'
        verbose_name = _('REm1 recipient')
        verbose_name_plural = _('REm2 recipients')
        get_latest_by = 'created_on'

    def clean(self):
        # Clean the fields that at Not Known
        if self.anastomosis_started_at_unknown:
            self.anastomosis_started_at = None
        if self.reperfusion_started_at_unknown:
            self.reperfusion_started_at = None

    def __unicode__(self):
        return '%s (%s)' % (self.person.number, self.trial_id())

    def age_from_dob(self):
        return self.person.age_from_dob()

    def trial_id(self):
        return self.organ.trial_id()


class Randomisation(models.Model):
    """
    Populated from the supplied CSV file via the fixture. A 'True' result is HMPO2 for the Left Organ
    """
    donor = models.OneToOneField(Donor, null=True, blank=True, default=None)  # Internal key
    country = models.PositiveSmallIntegerField(verbose_name=_("RA01 country"), choices=COUNTRY_CHOICES)
    result = models.BooleanField(verbose_name=_("RA02 result"), default=random_5050)
    allocated_on = models.DateTimeField(default=timezone.now)

    @staticmethod
    def get_and_assign_result(country_code, link_donor):
        options = Randomisation.objects.filter(country=country_code, donor=None).order_by('id')
        if len(options) < 1:
            raise Exception()
        result = options[0]
        result.donor = link_donor
        result.save()
        return result.result
