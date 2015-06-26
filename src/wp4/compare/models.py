#!/usr/bin/python
# coding: utf-8
from django.core.urlresolvers import reverse
from django.core.validators import MinValueValidator, MaxValueValidator
from django.db import models
from django.contrib.auth.models import User
from django.utils import timezone

# Common constants for some questions
NO = 0
YES = 1
UNKNOWN = 2
# NOT_ANSWERED = 9   # will be recorded as a null value
YES_NO_UNKNOWN_CHOICES = (
    (NO, "No"),
    (YES, "Yes"),
    (UNKNOWN, "Unknown")
)


class VersionControlModel(models.Model):
    version = models.PositiveIntegerField(default=0)
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    class Meta:
        abstract = True


# Create your models here.
class Person(VersionControlModel):
    PERFUSION_TECHNICIAN = "PT"
    TRANSPLANT_COORDINATOR = "TC"
    RESEARCH_NURSE = "RN"
    NATIONAL_COORDINATOR = "NC"
    CENTRAL_COORDINATOR = "CC"
    BIOBANK_COORDINATOR = "BC"
    STATISTICIAN = "S"
    SYSTEMS_ADMINISTRATOR = "SA"
    JOB_CHOICES = (
        (PERFUSION_TECHNICIAN, "Perfusion Technician"),
        (TRANSPLANT_COORDINATOR, "Transplant Co-ordinator"),
        (RESEARCH_NURSE, "Research Nurse / Follow-up"),
        (NATIONAL_COORDINATOR, "National Co-ordinator"),
        (CENTRAL_COORDINATOR, "Central Co-ordinator"),
        (BIOBANK_COORDINATOR, "Biobank Co-ordinator"),
        (STATISTICIAN, "Statistician"),
        (SYSTEMS_ADMINISTRATOR, "Sys-admin"),
    )
    first_names = models.CharField(max_length=50)
    last_names = models.CharField(max_length=50)
    job = models.CharField(max_length=2, choices=JOB_CHOICES)
    telephone = models.CharField(max_length=20)
    user = models.OneToOneField(User, blank=True, null=True, related_name="people_set")

    def full_name(self):
        return self.first_names + ' ' + self.last_names

    def get_absolute_url(self):
        return reverse('person-detail', kwargs={'pk': self.pk})

    def __unicode__(self):
        return self.full_name() + ' : ' + self.get_job_display()

    class Meta:
        verbose_name_plural = "people"


# Consider making this part of a LOCATION class
class Hospital(models.Model):
    name = models.CharField(max_length=100)
    centre_code = models.PositiveSmallIntegerField(validators=[
        MinValueValidator(10),
        MaxValueValidator(99)
    ])
    country = models.CharField(max_length=50)
    is_active = models.BooleanField(default=True)
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    def full_description(self):
        return '(%d) %s, %s' % (self.centre_code, self.name, self.country)

    def __unicode__(self):
        return self.full_description()

    class Meta:
        ordering = ['centre_code']


class RetrievalTeam(models.Model):
    name = models.CharField(max_length=100)
    based_at = models.ForeignKey(Hospital)
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    def next_sequence_number(self):
        try:
            number = self.donor_set.latest('sequence_number').sequence_number + 1
        except Donor.DoesNotExist:
            number = 1
        return number

    def __unicode__(self):
        return '%s from %s' % (self.name, self.based_at.full_description())

    class Meta:
        order_with_respect_to = 'based_at'


# Mostly replaces Specimens
class Sample(models.Model):
    barcode = models.CharField(max_length=20)
    taken_at = models.DateTimeField()
    centrifugation = models.DateTimeField(null=True, blank=True)
    comment = models.CharField(max_length=2000, null=True, blank=True)
    #  TODO: Specimen state?
    #  TODO: Who took the sample?
    #  TODO: Difference between worksheet and specimen barcodes?
    #  TODO: Reperfusion?
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    def __unicode__(self):
        return self.barcode


class Donor(VersionControlModel):
    # Procedure data
    retrieval_team = models.ForeignKey(RetrievalTeam)
    sequence_number = models.PositiveSmallIntegerField(default=0)
    perfusion_technician = models.ForeignKey(
        Person,
        verbose_name='name of transplant technician',
        limit_choices_to={"job": Person.PERFUSION_TECHNICIAN},
        related_name="perfusion_technician_set"
    )
    transplant_coordinator = models.ForeignKey(
        Person,
        verbose_name='name of the SN-OD', #'name of transplant co-ordinator',
        limit_choices_to={"job": Person.TRANSPLANT_COORDINATOR},
        related_name="transplant_coordinator_set",
        blank=True,
        null=True
    )
    call_received = models.DateTimeField(
        'Consultant to MTO called at',  # 'transplant co-ordinator received call at', 
        blank=True, null=True
    )

    retrieval_hospital = models.ForeignKey(Hospital, blank=True, null=True)
    scheduled_start = models.DateTimeField('scheduled time of withdrawal therapy', blank=True, null=True)
    technician_arrival = models.DateTimeField('arrival time of technician at hub', blank=True, null=True)
    ice_boxes_filled = models.DateTimeField('ice boxes filled with sufficient amount of ice (for kidney assist)', blank=True, null=True)
    depart_perfusion_centre = models.DateTimeField('departure from hub at', blank=True, null=True)
    arrival_at_donor_hospital = models.DateTimeField('arrival at donor hospital', blank=True, null=True)

    # Donor details
    MALE = 'M'
    FEMALE = 'F'
    GENDER_CHOICES = (
        (MALE, 'Male'),
        (FEMALE, 'Female')
    )

    CAUCASIAN = 1
    BLACK = 2
    OTHER_ETHNICITY = 3
    ETHNICITY_CHOICES = (
        (CAUCASIAN, 'Caucasian'),
        (BLACK, 'Black'),
        (OTHER_ETHNICITY, 'Other')
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
        (BLOOD_UNKNOWN, 'Unknown')
    )

    number = models.CharField("ET Donor number/ NHSBT Number", max_length=20)
    age = models.PositiveSmallIntegerField(validators=[
        MinValueValidator(50),
        MaxValueValidator(99)
    ])
    date_of_birth = models.DateField(blank=True, null=True)  # TODO: Define DoB validator that matches the ages ones
    date_of_admission = models.DateField('date of admission into hospital', blank=True, null=True)
    admitted_to_itu = models.NullBooleanField('admitted to ITU?', blank=True, null=True)
    date_admitted_to_itu = models.DateField('when admitted to ITU', blank=True, null=True)
    date_of_procurement = models.DateField('date of procurement', blank=True, null=True)
    gender = models.CharField(choices=GENDER_CHOICES, max_length=1, blank=True, null=True)
    weight = models.PositiveSmallIntegerField('Weight (kg)', validators=[
        MinValueValidator(20),
        MaxValueValidator(200)
    ], blank=True, null=True)
    height = models.PositiveSmallIntegerField('Height (cm)', validators=[
        MinValueValidator(100),
        MaxValueValidator(250)
    ], blank=True, null=True)
    ethnicity = models.IntegerField(choices=ETHNICITY_CHOICES, blank=True, null=True)
    blood_group = models.PositiveSmallIntegerField(choices=BLOOD_GROUP_CHOICES, blank=True, null=True)
    # Reference set to organs_offered

    # DonorPreop data
    CEREBRIVASCULAR_ACCIDENT = 1
    HYPOXIA = 2
    TRAUMA = 3
    OTHER_DIAGNOSIS = 4
    DIAGNOSIS_CHOICES = (
        (CEREBRIVASCULAR_ACCIDENT, "Cerebrivascular Accident"),
        (HYPOXIA, "Hypoxia"),
        (TRAUMA, "Trauma"),
        (OTHER_DIAGNOSIS, "Other")
    )
    diagnosis = models.PositiveSmallIntegerField(choices=DIAGNOSIS_CHOICES, blank=True, null=True)
    diagnosis_other = models.CharField(max_length=250, blank=True, null=True)
    diabetes_melitus = models.PositiveSmallIntegerField(choices=YES_NO_UNKNOWN_CHOICES, blank=True, null=True)
    alcohol_abuse = models.PositiveSmallIntegerField(choices=YES_NO_UNKNOWN_CHOICES, blank=True, null=True)
    cardiac_arrest = models.NullBooleanField(
        'Cardiac Arrest (During ITU stay, prior to Retrieval Procedure)', blank=True, null=True
    )
    systolic_blood_pressure = models.PositiveSmallIntegerField(
        "Last Systolic Blood Pressure (Before switch off)",
        validators=[
            MinValueValidator(10),
            MaxValueValidator(200)
        ], blank=True, null=True
    )
    diastolic_blood_pressure = models.PositiveSmallIntegerField(
        "Last Diastolic Blood Pressure (Before switch off)",
        validators=[
            MinValueValidator(10),
            MaxValueValidator(200)
        ], blank=True, null=True
    )
    hypotensive = models.NullBooleanField('hypotensive', blank=True, null=True)  # TODO: Check me. Can't find this on the form
    diuresis_last_day = models.PositiveSmallIntegerField('Diuresis last day (ml)', blank=True, null=True)
    diuresis_last_day_unknown = models.BooleanField(default=False)
    diuresis_last_hour = models.PositiveSmallIntegerField('Diuresis last hour (ml', blank=True, null=True)
    diuresis_last_hour_unknown = models.BooleanField(default=False)
    dopamine = models.PositiveSmallIntegerField(choices=YES_NO_UNKNOWN_CHOICES, blank=True, null=True)
    dobutamine = models.PositiveSmallIntegerField(choices=YES_NO_UNKNOWN_CHOICES, blank=True, null=True)
    nor_adrenaline = models.PositiveSmallIntegerField(choices=YES_NO_UNKNOWN_CHOICES, blank=True, null=True)
    vasopressine = models.PositiveSmallIntegerField(choices=YES_NO_UNKNOWN_CHOICES, blank=True, null=True)
    other_medication_details = models.CharField(max_length=250, blank=True, null=True)

    # Lab results
    UNIT_MGDL = 1
    UNIT_UMOLL = 2
    UNIT_CHOICES = (
        (UNIT_MGDL, "mg/dl"),
        (UNIT_UMOLL, "umol/L")
    )
    last_creatinine = models.FloatField(blank=True, null=True)
    last_creatinine_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)
    max_creatinine = models.FloatField(blank=True, null=True)
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
    life_support_withdrawal = models.DateTimeField('withdrawal of life support', blank=True, null=True)
    systolic_pressure_low = models.DateTimeField(
        'systolic arterial pressure < 50 mm Hg (inadequate organ perfusion)',
        blank=True,
        null=True
    )
    circulatory_arrest = models.DateTimeField(
        'end of cardiac output (=start of no touch period)',
        blank=True,
        null=True
    )
    length_of_no_touch = models.PositiveSmallIntegerField('length of no touch period (minutes)', blank=True, null=True)
    death_diagnosed = models.DateTimeField('diagnosis of death', blank=True, null=True)
    perfusion_started = models.DateTimeField('start in-situ cold perfusion', blank=True, null=True)
    systemic_flush_used = models.PositiveSmallIntegerField(
        'systemic (aortic) flush solution used',
        choices=FLUSH_SOLUTION_CHOICES,
        blank=True, null=True
    )
    systemic_flush_used_other = models.CharField(max_length=250, blank=True, null=True)
    heparin = models.NullBooleanField('heparin (administered to donor/in flush solution)')

    # Sampling data
    donor_blood_1_EDTA = models.ForeignKey(Sample, related_name="donor_blood_1_EDTA_set", blank=True, null=True)
    donor_blood_1_SST = models.ForeignKey(Sample, related_name="donor_blood_1_SST_set", blank=True, null=True)
    donor_urine_1 = models.ForeignKey(Sample, related_name="donor_urine_1_set", blank=True, null=True)
    donor_urine_2 = models.ForeignKey(Sample, related_name="donor_urine_2_set", blank=True, null=True)

    def bmi_value(self):
        # http://www.nhs.uk/chq/Pages/how-can-i-work-out-my-bmi.aspx?CategoryID=51 for formula
        height_in_m = self.height / 100
        return (self.weight / height_in_m) / height_in_m

    def left_kidney(self):
        try:
            return self.organ_set.filter(location__exact=Organ.LEFT)[0]
        except IndexError:  #Organ.DoesNotExist:
            if self.id > 0:
                return Organ(location=Organ.LEFT, donor=self)
            else:
                return Organ(location=Organ.LEFT)

    def right_kidney(self):
        try:
            return self.organ_set.filter(location__exact=Organ.RIGHT)[0]
        except IndexError:  #Organ.DoesNotExist:
            if self.id > 0:
                return Organ(location=Organ.RIGHT, donor=self)
            else:
                return Organ(location=Organ.RIGHT)

    def centre_code(self):
        try:
            return self.retrieval_team.based_at.centre_code
        except RetrievalTeam.DoesNotExist:
            return 0

    def trial_id(self):
        if self.centre_code() == 0:
            return ""
        return 'WP4%s%s' % (format(self.centre_code(), '02'), format(self.sequence_number, '03'))

    def save(self, force_insert=False, force_update=False, using=None, update_fields=None):
        # On creation, get and save the sequence number from the retrieval team
        if self.sequence_number < 1:
            self.sequence_number = self.retrieval_team.next_sequence_number()
        super(Donor, self).save(force_insert, force_update, using, update_fields)

    def __unicode__(self):
        return '%s (%s)' % (self.number, self.trial_id())

    class Meta:
        order_with_respect_to = 'retrieval_team'
        ordering = ['sequence_number']


class OrgansOffered(models.Model):
    LIVER = 1
    LUNG = 2
    PANCREAS = 3
    TISSUE = 4
    ORGAN_CHOICES = (
        (LIVER, 'Liver'),
        (LUNG, 'Lung'),
        (PANCREAS, 'Pancreas'),
        (TISSUE, "Tissue")
    )

    organ = models.PositiveSmallIntegerField(choices=ORGAN_CHOICES)
    donor = models.ForeignKey(Donor)
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    def __unicode__(self):
        return self.get_organ_display() + ' offered'

    class Meta:
        verbose_name_plural = "organs offered"


class PerfusionMachine(models.Model):
    # Device accountability
    machine_serial_number = models.CharField(max_length=50)
    machine_reference_number = models.CharField(max_length=50)
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    def __unicode__(self):
        return 's/n: ' + self.machine_serial_number


class PerfusionFile(models.Model):
    machine = models.ForeignKey(PerfusionMachine)
    file = models.FileField()
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)


class Organ(VersionControlModel):  # Or specifically, a Kidney
    LEFT = "L"
    RIGHT = "R"
    LOCATION_CHOICES = (
        (LEFT, "Left"),
        (RIGHT, "Right")
    )
    donor = models.ForeignKey(Donor)
    location = models.CharField(max_length=1, choices=LOCATION_CHOICES)

    # Inspection data
    ARTERIAL_DAMAGE = 1
    VENOUS_DAMAGE = 2
    URETERAL_DAMAGE = 3
    PARENCHYMAL_DAMAGE = 4
    NO_DAMAGE = 5
    GRAFT_DAMAGE_CHOICES = (
        (ARTERIAL_DAMAGE, "Arterial Damage"),
        (VENOUS_DAMAGE, "Venous Damage"),
        (URETERAL_DAMAGE, "Ureteral Damage"),
        (PARENCHYMAL_DAMAGE, "Parenchymal Damage"),
        (NO_DAMAGE, "None")
    )

    HOMEGENOUS = 1
    PATCHY = 2
    BLUE = 3
    PERFUSION_UNKNOWN = 9
    WASHOUT_PERFUSION_CHOICES = (
        (HOMEGENOUS, "Homogenous"),
        (PATCHY, "Patchy"),
        (BLUE, "Blue"),
        (PERFUSION_UNKNOWN, "Unknown")
    )

    HMP = 0
    HMPO2 = 1
    PRESERVATION_CHOICES = (
        (HMP, "HMP"),
        (HMPO2, "HMP O2")
    )
    removal = models.DateTimeField(blank=True, null=True)
    renal_arteries = models.PositiveSmallIntegerField(blank=True, null=True)
    graft_damage = models.PositiveSmallIntegerField(choices=GRAFT_DAMAGE_CHOICES, default=NO_DAMAGE)
    washout_perfusion = models.PositiveSmallIntegerField(choices=WASHOUT_PERFUSION_CHOICES, blank=True, null=True)
    transplantable = models.NullBooleanField(blank=True, null=True)
    not_transplantable_reason = models.CharField(max_length=250, blank=True, null=True)

    # Randomisation data
    # can_donate = models.BooleanField('Donor is eligible as DCD III and > 50 years old') -- donor info!
    # can_transplant = models.BooleanField('') -- derived from left and right being transplantable
    preservation = models.PositiveSmallIntegerField(choices=PRESERVATION_CHOICES, blank=True, null=True)

    # Perfusion data
    SMALL = 1
    LARGE = 2
    DOUBLE_ARTERY = 3
    PATCH_HOLDER_CHOICES = (
        (SMALL, "Small"),
        (LARGE, "Large"),
        (DOUBLE_ARTERY, "Double Artery")
    )
    ARTIFICIAL_PATCH_CHOICES = (
        (SMALL, "Small"),
        (LARGE, "Large")
    )
    perfusion_possible = models.NullBooleanField(blank=True, null=True)
    perfusion_not_possible_because = models.CharField(max_length=250, blank=True, null=True)
    perfusion_started = models.DateTimeField(blank=True, null=True)
    patch_holder = models.PositiveSmallIntegerField(choices=PATCH_HOLDER_CHOICES, blank=True, null=True)
    artificial_patch_used = models.NullBooleanField(blank=True, null=True)
    artificial_patch_size = models.PositiveSmallIntegerField(choices=ARTIFICIAL_PATCH_CHOICES, blank=True, null=True)
    artificial_patch_number = models.PositiveSmallIntegerField(
        blank=True,
        null=True,
        validators=[
            MinValueValidator(1),
            MaxValueValidator(2)
        ]
    )
    oxygen_bottle_full = models.NullBooleanField(blank=True, null=True)
    oxygen_bottle_open = models.NullBooleanField(blank=True, null=True)
    oxygen_bottle_changed = models.NullBooleanField(blank=True, null=True)
    oxygen_bottle_changed_at = models.DateTimeField(blank=True, null=True)
    ice_container_replenished = models.NullBooleanField(blank=True, null=True)
    ice_container_replenished_at = models.DateTimeField(blank=True, null=True)
    perfusate_measurable = models.NullBooleanField(blank=True, null=True)
    perfusate_measure = models.FloatField(blank=True, null=True)  # TODO: Check the value range for this
    # NB: There are ProcurementResources likely linked to this Organ
    perfusion_machine = models.ForeignKey(PerfusionMachine, blank=True, null=True)
    perfusion_file = models.ForeignKey(PerfusionFile, blank=True, null=True)

    # Sampling data
    perfusate_1 = models.ForeignKey(Sample, related_name="perfusate_1_set", blank=True, null=True)
    perfusate_2 = models.ForeignKey(Sample, related_name="perfusate_2_set", blank=True, null=True)

    def trial_id(self):
        return self.donor.trial_id() + self.location

    def __unicode__(self):
        return '%s : %s kidney preserved with %s' % (
            self.trial_id(),self.get_location_display(), self.get_preservation_display()
        )


class ProcurementResource(models.Model):
    DISPOSABLES = "D"
    EXTRA_CANNULA_SMALL = "C-SM"
    EXTRA_CANNULA_LARGE = "C-LG"
    EXTRA_PATCH_HOLDER_SMALL = "PH-SM"
    EXTRA_PATCH_HOLDER_LARGE = "PH-LG"
    EXTRA_DOUBLE_CANNULA_SET = "DB-C"
    PERFUSATE_SOLUTION = "P"
    TYPE_CHOICES = (
        (DISPOSABLES, "Disposables"),
        (EXTRA_CANNULA_SMALL, "Extra cannula small (3mm)"),
        (EXTRA_CANNULA_LARGE, "Extra cannula large (5mm)"),
        (EXTRA_PATCH_HOLDER_SMALL, "Extra patch holder small"),
        (EXTRA_PATCH_HOLDER_LARGE, "Extra patch holder large"),
        (EXTRA_DOUBLE_CANNULA_SET, "Extra double cannula set"),
        (PERFUSATE_SOLUTION, "Perfusate solution"),
    )
    organ = models.ForeignKey(Organ)
    type = models.CharField(choices=TYPE_CHOICES, max_length=5)
    lot_number = models.CharField(max_length=50)
    expiry_date = models.DateField()
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    def __unicode__(self):
        return self.get_type_display() + ' for ' + self.organ.trial_id()
