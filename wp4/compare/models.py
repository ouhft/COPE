#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals
from random import random
import datetime

from django.contrib.auth.models import User
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _, ungettext_lazy as __

from ..staff_person.models import StaffJob, StaffPerson, Hospital
from ..samples.models import Sample

# Common CONSTANTS
NO = 0
YES = 1
UNKNOWN = 2
# NOT_ANSWERED = 9   # will be recorded as a null value
YES_NO_UNKNOWN_CHOICES = (
    (UNKNOWN, _("MM03 Unknown")),
    (NO, _("MM01 No")),
    (YES, _("MM02 Yes"))
) # Need Yes to be the last choice for any FieldWithFollowUp


LEFT = "L"
RIGHT = "R"
LOCATION_CHOICES = (
    (LEFT, _('OR01 Left')),
    (RIGHT, _('OR02 Right'))
)


class VersionControlModel(models.Model):
    version = models.PositiveIntegerField(default=0)
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)
    record_locked = models.BooleanField(default=False)

    # TODO: Add save method here that aborts saving if record_locked is already true
    # TODO: Add version control via django-reversion

    class Meta:
        abstract = True



class RetrievalTeam(models.Model):
    centre_code = models.PositiveSmallIntegerField(
        verbose_name=_("HO02 centre code"),
        validators=[
            MinValueValidator(10),
            MaxValueValidator(99)
        ])
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
        (MALE, _('DO20 Male')),
        (FEMALE, _('DO21 Female'))
    )

    CAUCASIAN = 1
    BLACK = 2
    OTHER_ETHNICITY = 3
    ETHNICITY_CHOICES = (
        (CAUCASIAN, _('DO22 Caucasian')),
        (BLACK, _('DO23 Black')),
        (OTHER_ETHNICITY, _('DO24 Other'))
    )

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
        (BLOOD_UNKNOWN, _('DO29 Unknown'))
    )

    number = models.CharField(
        verbose_name=_('DO30 NHSBT Number'),  # "ET Donor number/ NHSBT Number",
        max_length=20,
        blank=True,
    )
    date_of_birth = models.DateField(
        verbose_name=_('DO32 date of birth'),
        blank=True, null=True,
    )
    date_of_birth_unknown = models.BooleanField(default=False)  # Internal flag
    gender = models.CharField(verbose_name=_('DO37 gender'), choices=GENDER_CHOICES, max_length=1, default=MALE)
    weight = models.PositiveSmallIntegerField(
        verbose_name=_('DO39 Weight (kg)'),
        validators=[
            MinValueValidator(20),
            MaxValueValidator(200)
        ],
        blank=True, null=True
    )
    height = models.PositiveSmallIntegerField(
        verbose_name=_('DO40 Height (cm)'),
        validators=[
            MinValueValidator(100),
            MaxValueValidator(250)
        ],
        blank=True, null=True
    )
    ethnicity = models.IntegerField(
        verbose_name=_('DO41 ethnicity'),
        choices=ETHNICITY_CHOICES,
        blank=True, null=True
    )
    blood_group = models.PositiveSmallIntegerField(
        verbose_name=_('DO42 blood group'),
        choices=BLOOD_GROUP_CHOICES,
        blank=True, null=True
    )

    class Meta:
        abstract = True


class Donor(OrganPerson):
    # Donor Form Case data
    sequence_number = models.PositiveSmallIntegerField(verbose_name=_("DO02 sequence number"), default=0)
    multiple_recipients = models.NullBooleanField(verbose_name=_("DO01 Multiple recipients"), default=None)

    # Procedure data
    retrieval_team = models.ForeignKey(RetrievalTeam, verbose_name=_("DO01 retrieval team"))
    perfusion_technician = models.ForeignKey(
        StaffPerson,
        verbose_name=_('DO03 name of transplant technician'),
        limit_choices_to={"jobs": StaffJob.PERFUSION_TECHNICIAN},
        related_name="donor_perfusion_technician_set"
    )
    transplant_coordinator = models.ForeignKey(
        StaffPerson,
        verbose_name=_('DO04 name of the SN-OD'),  # 'name of transplant co-ordinator',
        limit_choices_to={"jobs": StaffJob.TRANSPLANT_COORDINATOR},
        related_name="donor_transplant_coordinator_set",
        blank=True,
        null=True
    )
    call_received = models.DateTimeField(
        verbose_name=_('DO05 Consultant to MTO called at'),  # 'transplant co-ordinator received call at',
        blank=True, null=True
    )

    retrieval_hospital = models.ForeignKey(
        Hospital,
        verbose_name=_('DO06 donor hospital'),
        blank=True, null=True
    )
    scheduled_start = models.DateTimeField(
        verbose_name=_('DO07 time of withdrawal therapy'),
        blank=True, null=True
    )
    technician_arrival = models.DateTimeField(
        verbose_name=_('DO08 arrival time of technician at hub'),
        blank=True, null=True
    )
    ice_boxes_filled = models.DateTimeField(
        verbose_name=_('DO09 ice boxes filled'),  # with sufficient amount of ice (for kidney assist)
        blank=True, null=True
    )
    depart_perfusion_centre = models.DateTimeField(
        verbose_name=_('DO10 departure from base hospital at'),
        blank=True, null=True
    )
    arrival_at_donor_hospital = models.DateTimeField(
        verbose_name=_('DO11 arrival at donor hospital'),
        blank=True, null=True
    )

    # Donor details (in addition to OrganPerson)
    age = models.PositiveSmallIntegerField(
        verbose_name=_('DO31 age'),
        validators=[
            MinValueValidator(50),
            MaxValueValidator(99)
        ]
    )
    date_of_admission = models.DateField(
        verbose_name=_('DO33 date of admission into hospital'),
        blank=True, null=True
    )
    admitted_to_itu = models.BooleanField(verbose_name=_('DO34 admitted to ITU'), default=False)
    date_admitted_to_itu = models.DateField(verbose_name=_('DO35 when admitted to ITU'), blank=True, null=True)
    date_of_procurement = models.DateField(verbose_name=_('DO36 date of procurement'), blank=True, null=True)
    other_organs_procured = models.BooleanField(verbose_name=_("DO43 other organs procured"), default=False)
    other_organs_lungs = models.BooleanField(verbose_name=_("DO44 lungs"), default=False)
    other_organs_pancreas = models.BooleanField(verbose_name=_("DO45 pancreas"), default=False)
    other_organs_liver = models.BooleanField(verbose_name=_("DO46 liver"), default=False)
    other_organs_tissue = models.BooleanField(verbose_name=_("DO47 tissue"), default=False)

    # DonorPreop data
    CEREBROVASCULAR_ACCIDENT = 1
    HYPOXIA = 2
    TRAUMA = 3
    OTHER_DIAGNOSIS = 4
    DIAGNOSIS_CHOICES = (
        (CEREBROVASCULAR_ACCIDENT, _("DO50 Cerebrovascular Accident")),
        (HYPOXIA, _("DO51 Hypoxia")),
        (TRAUMA, _("DO52 Trauma")),
        (OTHER_DIAGNOSIS, _("DO53 Other"))
    )
    diagnosis = models.PositiveSmallIntegerField(
        verbose_name=_('DO54 diagnosis'),
        choices=DIAGNOSIS_CHOICES,
        blank=True, null=True)
    diagnosis_other = models.CharField(verbose_name=_('DO55 other diagnosis'), max_length=250, blank=True)
    diabetes_melitus = models.PositiveSmallIntegerField(
        verbose_name=_('DO56 diabetes mellitus'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    alcohol_abuse = models.PositiveSmallIntegerField(
        verbose_name=_('DO57 alcohol abuse'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    cardiac_arrest = models.NullBooleanField(
        verbose_name=_('DO58 cardiac arrest'),  # 'Cardiac Arrest (During ITU stay, prior to Retrieval Procedure)',
        blank=True, null=True)
    systolic_blood_pressure = models.PositiveSmallIntegerField(
        verbose_name=_('DO59 last systolic blood pressure'),  # "Last Systolic Blood Pressure (Before switch off)",
        validators=[
            MinValueValidator(10),
            MaxValueValidator(200)
        ],
        blank=True, null=True)
    diastolic_blood_pressure = models.PositiveSmallIntegerField(
        verbose_name=_('DO60 last diastolic blood pressure'),  # "Last Diastolic Blood Pressure (Before switch off)",
        validators=[
            MinValueValidator(10),
            MaxValueValidator(200)
        ],
        blank=True, null=True)
    diuresis_last_day = models.PositiveSmallIntegerField(
        verbose_name=_('DO61 diuresis last day (ml)'),
        blank=True, null=True)
    diuresis_last_day_unknown = models.BooleanField(default=False)  # Internal flag
    diuresis_last_hour = models.PositiveSmallIntegerField(
        verbose_name=_('DO62 diuresis last hour (ml)'),
        blank=True, null=True)
    diuresis_last_hour_unknown = models.BooleanField(default=False)  # Internal flag
    dopamine = models.PositiveSmallIntegerField(
        verbose_name=_('DO63 dopamine'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    dobutamine = models.PositiveSmallIntegerField(
        verbose_name=_('DO64 dobutamine'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    nor_adrenaline = models.PositiveSmallIntegerField(
        verbose_name=_('DO65 nor adrenaline'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    vasopressine = models.PositiveSmallIntegerField(
        verbose_name=_('DO66 vasopressine'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    other_medication_details = models.CharField(
        verbose_name=_('DO67 other medication'),
        max_length=250,
        blank=True)

    # Lab results
    UNIT_MGDL = 1
    UNIT_UMOLL = 2
    UNIT_CHOICES = (
        (UNIT_MGDL, "mg/dl"),
        (UNIT_UMOLL, "umol/L")
    )
    last_creatinine = models.FloatField(
        verbose_name=_('DO70 last creatinine'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    last_creatinine_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)
    max_creatinine = models.FloatField(verbose_name=_('DO72 max creatinine'), blank=True, null=True)
    max_creatinine_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)

    # Operation Data - Extraction
    SOLUTION_UW = 1
    SOLUTION_MARSHALL = 2
    SOLUTION_HTK = 3
    SOLUTION_OTHER = 4
    FLUSH_SOLUTION_CHOICES = (
        (SOLUTION_HTK, "HTK"),
        (SOLUTION_MARSHALL, "Marshall's"),
        (SOLUTION_UW, "UW"),
        (SOLUTION_OTHER, "Other")
    )
    life_support_withdrawal = models.DateTimeField(
        verbose_name=_('DO80 withdrawal of life support'),
        blank=True, null=True
    )
    systolic_pressure_low = models.DateTimeField(
        verbose_name=_('DO81 systolic arterial pressure'),  # < 50 mm Hg (inadequate organ perfusion)
        blank=True,
        null=True
    )
    o2_saturation = models.DateTimeField(
        verbose_name=_('DO82 O2 saturation below 80%'),
        blank=True,
        null=True
    )
    circulatory_arrest = models.DateTimeField(
        verbose_name=_('DO83 end of cardiac output'),  # (=start of no touch period)',
        blank=True,
        null=True
    )
    length_of_no_touch = models.PositiveSmallIntegerField(
        verbose_name=_('DO84 length of no touch period (minutes)'),
        blank=True, null=True,
        validators=[
            MinValueValidator(1),
            MaxValueValidator(60)
        ]
    )
    death_diagnosed = models.DateTimeField(
        verbose_name=_('DO85 knife to skin time'),
        blank=True, null=True
    )
    perfusion_started = models.DateTimeField(
        verbose_name=_('DO86 start in-situ cold perfusion'),
        blank=True, null=True
    )
    systemic_flush_used = models.PositiveSmallIntegerField(
        verbose_name=_('DO87 systemic (aortic) flush solution used'),
        choices=FLUSH_SOLUTION_CHOICES,
        blank=True, null=True
    )
    systemic_flush_used_other = models.CharField(
        verbose_name=_('DO88 systemic flush used'),
        max_length=250,
        blank=True
    )
    systemic_flush_volume_used = models.PositiveSmallIntegerField(
        verbose_name=_('DO89 aortic - volume (ml)'),
        blank=True, null=True
    )
    heparin = models.NullBooleanField(
        verbose_name=_('DO90 heparin'),  # (administered to donor/in flush solution)
        blank=True, null=True)

    # Sampling data
    donor_blood_1_EDTA = models.OneToOneField(
        Sample,
        verbose_name=_('DO91 db 1.1 edta'),
        related_name="donor_blood_1",
        limit_choices_to={'type': Sample.DONOR_BLOOD_1},
        blank=True, null=True
    )
    donor_blood_1_SST = models.OneToOneField(
        Sample,
        verbose_name=_('DO92 db 1.2 sst'),
        related_name="donor_blood_2",
        limit_choices_to={'type': Sample.DONOR_BLOOD_2},
        blank=True, null=True
    )
    donor_urine_1 = models.OneToOneField(
        Sample,
        verbose_name=_('DO93 du 1'),
        related_name="donor_urine_1",
        limit_choices_to={'type': Sample.DONOR_URINE_1},
        blank=True, null=True
    )
    donor_urine_2 = models.OneToOneField(
        Sample,
        verbose_name=_('DO94 du 2'),
        related_name="donor_urine_2",
        limit_choices_to={'type': Sample.DONOR_URINE_2},
        blank=True, null=True
    )

    class Meta:
        order_with_respect_to = 'retrieval_team'
        ordering = ['sequence_number']
        verbose_name = _('DOm1 donor')
        verbose_name_plural = _('DOm2 donors')

    def clean(self):
        if self.arrival_at_donor_hospital and self.depart_perfusion_centre:
            if self.arrival_at_donor_hospital < self.depart_perfusion_centre:
                raise ValidationError(
                    _(
                        "DOv01 Time travel detected! Arrival at donor hospital occurred before departure from "
                        "perfusion centre")
                )
        if self.date_of_birth:
            if self.date_of_birth > datetime.datetime.now().date():
                raise ValidationError(_("DOv02 Time travel detected! Donor's date of birth is in the future!"))
            if self.date_of_procurement:
                age_difference = self.date_of_procurement - self.date_of_birth
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
            if self.age != self.age_from_dob():
                raise ValidationError(
                    _("DOv05 Age does not match age as calculated (%(num)d years) from Date of Birth"
                      % {'num': self.age_from_dob()})
                )
        if self.date_of_procurement:
            if self.date_of_procurement < self.date_of_admission:
                raise ValidationError(_("DOv06 Date of procurement occurs before date of admission"))

        if self.admitted_to_itu and not self.date_admitted_to_itu:
            raise ValidationError(_("DOv07 Missing the date admitted to ITU"))
        if self.diagnosis == self.OTHER_DIAGNOSIS and not self.diagnosis_other:
            raise ValidationError(_("DOv08 Missing the other diagnosis"))

        if self.diuresis_last_day_unknown:
            self.diuresis_last_day = None
        if self.diuresis_last_hour_unknown:
            self.diuresis_last_hour = None

        if self.life_support_withdrawal and self.life_support_withdrawal.date() < self.date_of_admission:
            raise ValidationError(_("DOv09 Life support withdrawn before admission to hospital"))
        if self.circulatory_arrest and self.death_diagnosed:
            if self.circulatory_arrest > self.death_diagnosed:
                raise ValidationError(_("DOv10 Donor was diagnosed as dead before circulation stopped"))

        if self.systemic_flush_used and self.systemic_flush_used == self.SOLUTION_OTHER \
                and not self.systemic_flush_used_other:
            raise ValidationError(_("DOv10 Missing the details of the other systemic flush solution used"))

    def save(self, force_insert=False, force_update=False, using=None, update_fields=None):
        # On creation, get and save the sequence number from the retrieval team
        if self.sequence_number < 1:
            self.sequence_number = self.retrieval_team.next_sequence_number()
        super(Donor, self).save(force_insert, force_update, using, update_fields)

    def randomise(self):
        # Randomise if eligible and not already done
        if self.left_kidney().preservation is None \
                and self.multiple_recipients is not False \
                and self.left_kidney().transplantable \
                and self.right_kidney().transplantable:
            left_o2 = random() >= 0.5  # True/False
            left_kidney = self.left_kidney()
            right_kidney = self.right_kidney()
            if left_o2:
                left_kidney.preservation = Organ.HMPO2
                right_kidney.preservation = Organ.HMP
            else:
                left_kidney.preservation = Organ.HMP
                right_kidney.preservation = Organ.HMPO2
            left_kidney.save()
            right_kidney.save()

    def __unicode__(self):
        return '%s (%s)' % (self.number, self.trial_id())

    def bmi_value(self):
        # http://www.nhs.uk/chq/Pages/how-can-i-work-out-my-bmi.aspx?CategoryID=51 for formula
        height_in_m = self.height / 100
        return (self.weight / height_in_m) / height_in_m
    bmi_value.short_description = 'BMI Value'

    def age_from_dob(self):
        today = datetime.date.today()
        if self.date_of_birth < today:
            years = today.year - self.date_of_birth.year
        else:
            years = today.year - self.date_of_birth.year - 1
        return years

    def left_kidney(self):
        try:
            return self.organ_set.filter(location__exact=LEFT)[0]
        except IndexError:  # Organ.DoesNotExist:
            if self.id > 0:
                return Organ(location=LEFT, donor=self)
            else:
                return Organ(location=LEFT)

    def right_kidney(self):
        try:
            return self.organ_set.filter(location__exact=RIGHT)[0]
        except IndexError:  # Organ.DoesNotExist:
            if self.id > 0:
                return Organ(location=RIGHT, donor=self)
            else:
                return Organ(location=RIGHT)

    def centre_code(self):
        try:
            return self.retrieval_team.centre_code
        except RetrievalTeam.DoesNotExist:
            return 0
    centre_code.short_description = 'Centre Code'

    def trial_id(self):
        if self.centre_code() == 0:
            return ""
        return 'WP4%s%s' % (format(self.centre_code(), '02'), format(self.sequence_number, '03'))
    trial_id.short_description = 'Trial ID'


class PerfusionMachine(models.Model):
    # Device accountability
    machine_serial_number = models.CharField(
        verbose_name=_('PM01 machine serial number'),
        max_length=50
    )
    machine_reference_number = models.CharField(
        verbose_name=_('PM02 machine reference number'),
        max_length=50
    )
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    def __unicode__(self):
        return 's/n: ' + self.machine_serial_number

    class Meta:
        verbose_name = _('PMm1 perfusion machine')
        verbose_name_plural = _('PMm2 perfusion machines')


class PerfusionFile(models.Model):
    machine = models.ForeignKey(PerfusionMachine, verbose_name=_('PF01 perfusion machine'))
    file = models.FileField(blank=True)
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    class Meta:
        verbose_name = _('PFm1 perfusion machine file')
        verbose_name_plural = _('PFm2 perfusion machine files')


class Organ(VersionControlModel):  # Or specifically, a Kidney
    donor = models.ForeignKey(Donor)
    location = models.CharField(
        verbose_name=_('OR03 kidney location'),
        max_length=1,
        choices=LOCATION_CHOICES
    )

    # Inspection data
    ARTERIAL_DAMAGE = 1
    VENOUS_DAMAGE = 2
    URETERAL_DAMAGE = 3
    PARENCHYMAL_DAMAGE = 4
    OTHER_DAMAGE = 6
    NO_DAMAGE = 5
    GRAFT_DAMAGE_CHOICES = (
        (NO_DAMAGE, _("OR15 None")),
        (ARTERIAL_DAMAGE, _("OR10 Arterial Damage")),
        (VENOUS_DAMAGE, _("OR11 Venous Damage")),
        (URETERAL_DAMAGE, _("OR12 Ureteral Damage")),
        (PARENCHYMAL_DAMAGE, _("OR13 Parenchymal Damage")),
        (OTHER_DAMAGE, _("OR14 Other Damage"))
    )

    HOMEGENOUS = 1
    PATCHY = 2
    BLUE = 3
    PERFUSION_UNKNOWN = 9
    WASHOUT_PERFUSION_CHOICES = (
        (HOMEGENOUS, _("OR16 Homogenous")),
        (PATCHY, _("OR17 Patchy")),
        (BLUE, _("OR18 Blue")),
        (PERFUSION_UNKNOWN, _("OR19 Unknown"))
        # NHS Form has: Good, Fair, Poor, Patchy, Unknown
    )

    HMP = 0
    HMPO2 = 1
    PRESERVATION_CHOICES = (
        (HMP, "HMP"),
        (HMPO2, "HMP O2")
    )
    removal = models.DateTimeField(
        verbose_name=_('OR21 time out'),
        blank=True, null=True
    )
    renal_arteries = models.PositiveSmallIntegerField(
        verbose_name=_('OR22 number of renal arteries'),
        blank=True, null=True
    )
    graft_damage = models.PositiveSmallIntegerField(
        verbose_name=_('OR23 renal graft damage'),
        choices=GRAFT_DAMAGE_CHOICES,
        default=NO_DAMAGE
    )
    graft_damage_other = models.CharField(
        verbose_name=_('OR24 other damage done'),
        max_length=250,
        blank=True
    )
    washout_perfusion = models.PositiveSmallIntegerField(
        verbose_name=_('OR25 perfusion characteristics'),
        choices=WASHOUT_PERFUSION_CHOICES,
        blank=True, null=True
    )
    transplantable = models.NullBooleanField(
        verbose_name=_('OR26 is transplantable'),
        blank=True, null=True)
    not_transplantable_reason = models.CharField(
        verbose_name=_('OR27 not transplantable because'),
        max_length=250,
        blank=True
    )

    # Randomisation data
    # can_donate = models.BooleanField('Donor is eligible as DCD III and > 50 years old') -- donor info!
    # can_transplant = models.BooleanField('') -- derived from left and right being transplantable
    preservation = models.PositiveSmallIntegerField(choices=PRESERVATION_CHOICES, blank=True, null=True)

    # Perfusion data
    SMALL = 1
    LARGE = 2
    DOUBLE_ARTERY = 3
    PATCH_HOLDER_CHOICES = (
        (SMALL, _("OR30 Small")),
        (LARGE, _("OR31 Large")),
        (DOUBLE_ARTERY, _("OR32 Double Artery"))
    )
    ARTIFICIAL_PATCH_CHOICES = (
        (SMALL, _("OR33 Small")),
        (LARGE, _("OR34 Large"))
    )
    perfusion_possible = models.NullBooleanField(
        verbose_name=_('OR35 machine perfusion possible?'),
        blank=True, null=True)
    perfusion_not_possible_because = models.CharField(
        verbose_name=_('OR36 not possible because'),
        max_length=250,
        blank=True
    )
    perfusion_started = models.DateTimeField(
        verbose_name=_('OR37 machine perfusion'),
        blank=True, null=True
    )
    patch_holder = models.PositiveSmallIntegerField(
        verbose_name=_('OR38 used patch holder'),
        choices=PATCH_HOLDER_CHOICES,
        blank=True, null=True
    )
    artificial_patch_used = models.NullBooleanField(
        verbose_name=_('OR39 artificial patch used'),
        blank=True, null=True)
    artificial_patch_size = models.PositiveSmallIntegerField(
        verbose_name=_('OR40 artificial patch size'),
        choices=ARTIFICIAL_PATCH_CHOICES,
        blank=True, null=True
    )
    artificial_patch_number = models.PositiveSmallIntegerField(
        verbose_name=_('OR41 number of patches'),
        blank=True,
        null=True,
        validators=[
            MinValueValidator(1),
            MaxValueValidator(2)
        ]
    )
    oxygen_bottle_full = models.NullBooleanField(
        verbose_name=_('OR42 is oxygen bottle full'),
        blank=True, null=True)
    oxygen_bottle_open = models.NullBooleanField(
        verbose_name=_('OR43 oxygen bottle opened'),
        blank=True, null=True)
    oxygen_bottle_changed = models.NullBooleanField(
        verbose_name=_('OR44 oxygen bottle changed'),
        blank=True, null=True)
    oxygen_bottle_changed_at = models.DateTimeField(
        verbose_name=_('OR45 oxygen bottle changed at'),
        blank=True, null=True
    )
    ice_container_replenished = models.NullBooleanField(
        verbose_name=_('OR46 ice container replenished'),
        blank=True, null=True)
    ice_container_replenished_at = models.DateTimeField(
        verbose_name=_('OR47 ice container replenished at'),
        blank=True, null=True
    )
    perfusate_measurable = models.NullBooleanField(
        verbose_name=_('OR48 perfusate measurable'),
        # logistically possible to measure pO2 perfusate (use blood gas analyser)',
        blank=True, null=True)
    perfusate_measure = models.FloatField(
        verbose_name=_('OR49 value pO2'),
        blank=True, null=True
    )  # TODO: Check the value range for this
    # NB: There are ProcurementResources likely linked to this Organ
    perfusion_machine = models.ForeignKey(
        PerfusionMachine,
        verbose_name=_('OR50 perfusion machine'),
        blank=True, null=True)
    perfusion_file = models.ForeignKey(
        PerfusionFile,
        verbose_name=_('OR51 machine file'),
        blank=True, null=True
    )

    # Sampling data
    perfusate_1 = models.ForeignKey(
        Sample,
        verbose_name=_('OR60 p1'),
        related_name="kidney_perfusate_1",
        limit_choices_to={'type': Sample.KIDNEY_PERFUSATE_1},
        blank=True, null=True
    )
    perfusate_2 = models.ForeignKey(
        Sample,
        verbose_name=_('OR60 p2'),
        related_name="kidney_perfusate_2",
        limit_choices_to={'type': Sample.KIDNEY_PERFUSATE_2},
        blank=True, null=True
    )
    perfusate_3 = models.ForeignKey(
        Sample,
        verbose_name=_('OR61 p3'),
        related_name="kidney_perfusate_3",
        limit_choices_to={'type': Sample.KIDNEY_PERFUSATE_3},
        blank=True, null=True
    )

    def trial_id(self):
        return self.donor.trial_id() + self.location

    def __unicode__(self):
        return '%s : %s kidney preserved with %s' % (
            self.trial_id(), self.get_location_display(), self.get_preservation_display()
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
        (DISPOSABLES, _("PR01 Disposables")),
        (EXTRA_CANNULA_SMALL, _("PR02 Extra cannula small (3mm)")),
        (EXTRA_CANNULA_LARGE, _("PR03 Extra cannula large (5mm)")),
        (EXTRA_PATCH_HOLDER_SMALL, _("PR04 Extra patch holder small")),
        (EXTRA_PATCH_HOLDER_LARGE, _("PR05 Extra patch holder large")),
        (EXTRA_DOUBLE_CANNULA_SET, _("PR06 Extra double cannula set")),
        (PERFUSATE_SOLUTION, _("PR07 Perfusate solution")),
    )
    organ = models.ForeignKey(
        Organ,
        verbose_name=_('PR10 related kidney'))
    type = models.CharField(
        verbose_name=_('PR11 resource used'),
        choices=TYPE_CHOICES,
        max_length=5)
    lot_number = models.CharField(
        verbose_name=_('PR12 lot number'),
        max_length=50)
    expiry_date = models.DateField(verbose_name=_('PR13 expiry date'))
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    def __unicode__(self):
        return self.get_type_display() + ' for ' + self.organ.trial_id()

    class Meta:
        verbose_name = _('PRm1 procurement resource')
        verbose_name_plural = _('PRm2 procurement resources')


class Recipient(OrganPerson):
    organ = models.ForeignKey(Organ)
    # sequence_number = models.PositiveSmallIntegerField(default=0)
    # Allocation data
    REALLOCATION_CROSSMATCH = 1
    REALLOCATION_UNKNOWN = 2
    REALLOCATION_OTHER = 3
    REALLOCATION_CHOICES = (
        (REALLOCATION_CROSSMATCH, _('RE01 Positive crossmatch')),
        (REALLOCATION_UNKNOWN, _('RE02 Unknown')),
        (REALLOCATION_OTHER, _('RE03 Other'))
    )
    perfusion_technician = models.ForeignKey(
        StaffPerson,
        verbose_name=_('DO03 name of transplant technician'),
        limit_choices_to={"jobs": StaffJob.PERFUSION_TECHNICIAN},
        related_name="recipient_perfusion_technician_set",
        blank=True, null=True)
    call_received = models.DateTimeField(
        verbose_name=_('DO05 Consultant to MTO called at'),  # 'transplant co-ordinator received call at',
        blank=True, null=True)
    transplant_hospital = models.ForeignKey(
        Hospital,
        verbose_name=_('DO06 donor hospital'),
        blank=True, null=True)
    transplant_coordinator = models.ForeignKey(
        StaffPerson,
        verbose_name=_('DO04 name of the SN-OD'),  # 'name of transplant co-ordinator',
        limit_choices_to={"jobs": StaffJob.TRANSPLANT_COORDINATOR},
        related_name="recipient_transplant_coordinator_set",
        blank=True, null=True)
    scheduled_start = models.DateTimeField(
        verbose_name=_('DO07 time of withdrawal therapy'),
        blank=True, null=True)
    technician_arrival = models.DateTimeField(
        verbose_name=_('DO08 arrival time of technician at hub'),
        blank=True, null=True)
    depart_perfusion_centre = models.DateTimeField(
        verbose_name=_('DO10 departure from base hospital at'),
        blank=True, null=True)
    arrival_at_recipient_hospital = models.DateTimeField(
        verbose_name=_('DO11 arrival at donor hospital'),
        blank=True, null=True
    )
    journey_remarks = models.TextField(verbose_name=_("journey notes"), blank=True)
    reallocated = models.BooleanField(verbose_name=_("reallocated"), default=False)
    reallocation_reason = models.PositiveSmallIntegerField(
        verbose_name=_('reason for re-allocation'),
        choices=REALLOCATION_CHOICES,
        blank=True, null=True)
    reallocation_reason_other = models.CharField(verbose_name=_('other reason'), max_length=250, blank=True)
    reallocation_recipient = models.OneToOneField('Recipient', default=None, blank=True, null=True)

    # Recipient details (in addition to OrganPerson)
    RENAL_DISEASE_CHOICES = (
        (1, _('Glomerular diseases')),
        (2, _('Polycystic kidneys')),
        (3, _('Uncertain etiology')),
        (4, _('Tubular and interstitial diseases')),
        (5, _('Retransplant graft failure')),
        (6, _('diabetic nephropathyes')),
        (7, _('hypertensive nephropathyes')),
        (8, _('congenital rare disorders')),
        (9, _('renovascular and other diseases')),
        (10, _('neoplasms')),
        (11, _('other')),
    )
    renal_disease = models.PositiveSmallIntegerField(
        verbose_name=_('DO54 renal disease'),
        choices=RENAL_DISEASE_CHOICES,
        blank=True, null=True
    )
    renal_disease_other = models.CharField(
        verbose_name=_('DO55 other renal disease'),
        max_length=250,
        blank=True)
    pre_transplant_diuresis = models.PositiveSmallIntegerField(
        verbose_name=_('DO61 diuresis (ml/24hr)'),
        blank=True, null=True)

    # Peri-operative data
    INCISION_CHOICES = (
        (1, _('midline laparotomy')),
        (2, _('hockey stick')),
        (3, _('unknown'))
    )
    ARTERIAL_PROBLEM_CHOICES = (
        (1, _('None')),
        (2, _('ligated polar artery')),
        (3, _('reconstructed polar artery')),
        (4, _('repaired intima dissection')),
        (5, _('other'))
    )
    VENOUS_PROBLEM_CHOICES = (
        (1, _('none')),
        (2, _('laceration')),
        (3, _('elongation plasty')),
        (4, _('other'))
    )
    knife_to_skin = models.DateTimeField(
        verbose_name=_('DO85 knife to skin time'),
        blank=True, null=True)
    perfusate_measure = models.FloatField(
        verbose_name=_('pO2 perfusate'),
        blank=True, null=True)  # TODO: Check the value range for this
    perfusion_stopped = models.DateTimeField(
        verbose_name=_('stop machine perfusion'),
        blank=True, null=True)
    organ_cold_stored = models.BooleanField(verbose_name=_('kidney was cold stored?'), default=False)
    tape_broken = models.NullBooleanField(
        verbose_name=_('tape over regulator broken'),
        blank=True, null=True)
    removed_from_machine_at = models.DateTimeField(
        verbose_name=_('kidney removed from matchine at'),
        blank=True, null=True)
    oxygen_full_and_open = models.PositiveSmallIntegerField(
        verbose_name=_('oxygen full and open'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    organ_untransplantable = models.NullBooleanField(
        verbose_name=_('kidney discarded'),
        blank=True, null=True)
    organ_untransplantable_reason = models.CharField(
        verbose_name=_('untransplantable because'),
        max_length=250,
        blank=True)
    anesthesia_started_at = models.DateTimeField(
        verbose_name=_('start anesthesia at'),
        blank=True, null=True)
    incision = models.PositiveSmallIntegerField(
        verbose_name=_('incision'),
        choices=INCISION_CHOICES,
        blank=True, null=True)
    transplant_side = models.CharField(
        verbose_name=_('transplant side'),
        max_length=1,
        choices=LOCATION_CHOICES)
    arterial_problems = models.PositiveSmallIntegerField(
        verbose_name=_('arterial problems'),
        choices=ARTERIAL_PROBLEM_CHOICES,
        blank=True, null=True)
    arterial_problems_other = models.CharField(
        verbose_name=_('arterial problems other'),
        max_length=250,
        blank=True)
    venous_problems = models.PositiveSmallIntegerField(
        verbose_name=_('venous problems'),
        choices=VENOUS_PROBLEM_CHOICES,
        blank=True, null=True)
    venous_problems_other = models.CharField(
        verbose_name=_('venous problems other'),
        max_length=250,
        blank=True)
    anastomosis_started_at = models.DateTimeField(
        verbose_name=_('start anastomosis at'),
        blank=True, null=True)
    reperfusion_started_at = models.DateTimeField(
        verbose_name=_('start reperfusion at'),
        blank=True, null=True)
    mannitol_used = models.PositiveSmallIntegerField(
        verbose_name=_('mannitol used'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    other_diurectics = models.PositiveSmallIntegerField(
        verbose_name=_('other diurectics used'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    other_diurectics_details = models.CharField(
        verbose_name=_('other diurectics detail'),
        max_length=250,
        blank=True)
    systolic_blood_pressure = models.PositiveSmallIntegerField(
        verbose_name=_('systolic blood pressure at reperfusion'),
        validators=[
            MinValueValidator(10),
            MaxValueValidator(200)
        ],
        blank=True, null=True)
    cvp = models.PositiveSmallIntegerField(
        verbose_name=_('cvp at reperfusion'),
        blank=True, null=True)
    intra_operative_diuresis = models.PositiveSmallIntegerField(
        verbose_name=_('intra-operative diuresis'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    successful_conclusion = models.BooleanField(verbose_name=_("successful conclusion"), default=False)
    operation_concluded_at = models.DateTimeField(verbose_name=_("operation concluded at"), null=True, blank=True)

    # SAMPLE DATA
    # P#, RB1, RB2, ReK1R, ReK1F

    # Machine cleanup record
    probe_cleaned = models.NullBooleanField(
        verbose_name=_('temperature and flow probe cleaned'),
        blank=True, null=True)
    ice_removed = models.NullBooleanField(
        verbose_name=_('ice and water removed'),
        blank=True, null=True)
    oxygen_flow_stopped = models.NullBooleanField(
        verbose_name=_('oxygen flow stopped'),
        blank=True, null=True)
    oxygen_bottle_removed = models.NullBooleanField(
        verbose_name=_('oxygen bottle removed'),
        blank=True, null=True)
    box_cleaned = models.NullBooleanField(
        verbose_name=_('box kidney assist cleaned'),
        blank=True, null=True)
    batteries_charged = models.NullBooleanField(
        verbose_name=_('batteries charged'),
        blank=True, null=True)
    cleaning_log = models.TextField(verbose_name=_("cleaning log notes"), blank=True)

    def trial_id(self):
        return self.organ.__unicode__()

    def clean(self):
        pass

    def __unicode__(self):
        return '%s (%s)' % (self.number, self.trial_id())

    class Meta:
        order_with_respect_to = 'organ'
        ordering = ['sequence_number']
        verbose_name = _('REm1 recipient')
        verbose_name_plural = _('REm2 recipients')
        get_latest_by = 'created_on'
