#!/usr/bin/python
# coding: utf-8
from django.db import models

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


# Create your models here.
class Person(models.Model):
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
        (SYSTEMS_ADMINISTRATOR, "Sys-admin")
    )
    first_names = models.CharField()
    last_names = models.CharField()
    # full_name is derived from the above
    job = models.CharField()


class User(Person):
    user_account = ""  # Link this to the Auth system


class RetrievalTeam(models.Model):
    name = models.CharField()
    country = models.CharField()
    centre_code = models.PositiveSmallIntegerField(min(10), max(99))
    created_on = models.DateTimeField(auto_now_add=True)
    created_by = models.ForeignKey(User)


# Consider making this part of a LOCATION class
class RetrievalHospital(models.Model):
    name = models.CharField()
    is_active = models.BooleanField(default=True)
    created_on = models.DateTimeField(auto_now_add=True)
    created_by = models.ForeignKey(User)


class VersionControlModel(models.Model):
    version = models.PositiveIntegerField()
    created_on = models.DateTimeField(auto_now_add=True)
    created_by = models.ForeignKey(User)


# Mostly replaces Specimens
class Sample(models.Model):
    barcode = models.CharField()
    taken_at = models.DateTimeField()
    centrifugation = models.DateTimeField(null=True)
    comment = models.CharField()
    #  TODO: Specimen state?
    #  TODO: Who took the sample?
    #  TODO: Difference between worksheet and specimen barcodes?
    #  TODO: Reperfusion?
    created_on = models.DateTimeField(auto_now_add=True)
    created_by = models.ForeignKey(User)


class Donor(VersionControlModel):
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

    number = models.CharField("ET Donor number/ NHSBT Number")
    age = models.PositiveSmallIntegerField()
    date_of_birth = models.DateField()
    date_of_admission = models.DateField()
    admitted_to_itu = models.BooleanField()
    date_admitted_to_itu = models.DateField()
    date_of_procurement = models.DateField()
    gender = models.CharField(choices=GENDER_CHOICES, max_length=1)
    ethnicity = models.IntegerField(choices=ETHNICITY_CHOICES)
    weight = models.PositiveSmallIntegerField('Weight (kg)', min(20), max(200))
    height = models.PositiveSmallIntegerField('Height (cm)', min(100), max(250))
    # BMI is a derived value
    blood_group = models.PositiveSmallIntegerField(choices=BLOOD_GROUP_CHOICES)

    # Trial data
    retrieval_team = models.ForeignKey(RetrievalTeam)
    criteria_check_1 = models.BooleanField()
    criteria_check_2 = models.BooleanField()
    criteria_check_3 = models.BooleanField()
    criteria_check_4 = models.BooleanField()
    is_active = models.BooleanField()  # Can be automatically set based on answers in the system
    # Generated from data "WP4" + RetrievalTeam.CentreCode + Trial.ID(padded to three numbers)
    # trial_id = models.CharField()

    # Procedure data
    perfusion_technician = models.ForeignKey(Person, limit_choices_to={"job": Person.PERFUSION_TECHNICIAN})
    transplant_coordinator = models.ForeignKey(Person, limit_choices_to={"job": Person.TRANSPLANT_COORDINATOR})
    call_received = models.DateTimeField()
    retrieval_hospital = models.ForeignKey(RetrievalHospital)
    scheduled_start = models.DateTimeField()
    technician_arrival = models.DateTimeField()
    ice_boxes_filled = models.DateTimeField()
    depart_perfusion_centre = models.DateTimeField()
    arrival_at_donor_hospital = models.DateTimeField()

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
    diagnosis = models.PositiveSmallIntegerField(choices=DIAGNOSIS_CHOICES)
    diagnosis_other = models.CharField()
    diabetes_melitus = models.PositiveSmallIntegerField(choices=YES_NO_UNKNOWN_CHOICES, null=True)
    alcohol_abuse = models.PositiveSmallIntegerField(choices=YES_NO_UNKNOWN_CHOICES, null=True)
    cardiac_arrest = models.NullBooleanField(
        "Cardiac Arrest (During ITU stay, prior to Retrieval Procedure"
    )
    systolic_blood_pressure = models.PositiveSmallIntegerField(
        "Last Systolic Blood Pressure (Before switch off)"
    )
    diastolic_blood_pressure = models.PositiveSmallIntegerField(
        "Last Diastolic Blood Pressure (Before switch off)"
    )
    hypotensive = models.BooleanField(null=True)
    diuresis_last_day = models.PositiveSmallIntegerField()
    diuresis_last_day_unknown = models.BooleanField()
    diuresis_last_hour = models.PositiveSmallIntegerField()
    diuresis_last_hour_unknown = models.BooleanField()
    dopamine = models.PositiveSmallIntegerField(choices=YES_NO_UNKNOWN_CHOICES, null=True)
    dobutamine = models.PositiveSmallIntegerField(choices=YES_NO_UNKNOWN_CHOICES, null=True)
    nor_adrenaline = models.PositiveSmallIntegerField(choices=YES_NO_UNKNOWN_CHOICES, null=True)
    vasopressine = models.PositiveSmallIntegerField(choices=YES_NO_UNKNOWN_CHOICES, null=True)
    other_medication_details = models.CharField()

    # Lab results
    UNIT_MGDL = 1
    UNIT_UMOLL = 2
    UNIT_CHOICES = (
        (UNIT_MGDL, "mg/dl"),
        (UNIT_UMOLL, "umol/L")
    )
    last_creatinine = models.FloatField()
    last_creatinine_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES)
    max_creatinine = models.FloatField()
    max_creatinine_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES)

    # Organs Offered
    LIVER = 1
    LUNG = 2
    PANCREAS = 3
    ORGAN_CHOICES = (
        (LIVER, 'Liver'),
        (LUNG, 'Lung'),
        (PANCREAS, 'Pancreas')
    )

    organ = models.PositiveSmallIntegerField(choices=ORGAN_CHOICES)

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
    life_support_withdrawl = models.DateTimeField()
    systolic_pressure_low = models.DateTimeField()
    circulatory_arrest = models.DateTimeField()
    length_of_no_touch = models.PositiveSmallIntegerField()
    death_diagnosed = models.DateTimeField()
    perfusion_started = models.DateTimeField()
    systemic_flush_used = models.PositiveSmallIntegerField(choices=FLUSH_SOLUTION_CHOICES)
    systemic_flush_used_other = models.CharField()
    heparin = models.NullBooleanField()

    # Sampling data
    donor_blood_1_EDTA = models.ForeignKey(Sample)
    donor_blood_1_SST = models.ForeignKey(Sample)
    donor_urine_1 = models.ForeignKey(Sample)
    donor_urine_2 = models.ForeignKey(Sample)


class PerfusionMachine(models.Model):
    # Device accountability
    machine_serial_number = models.CharField()
    machine_reference_number = models.CharField()
    created_on = models.DateTimeField(auto_now_add=True)
    created_by = models.ForeignKey(User)


class PerfusionFile(models.Model):
    machine = models.ForeignKey(PerfusionMachine)
    file = models.FileField()
    created_on = models.DateTimeField(auto_now_add=True)
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
    removal = models.DateTimeField()
    renal_arteries = models.PositiveSmallIntegerField()
    graft_damage = models.PositiveSmallIntegerField(choices=GRAFT_DAMAGE_CHOICES)
    washout_perfusion = models.PositiveSmallIntegerField(choices=WASHOUT_PERFUSION_CHOICES)
    transplantable = models.BooleanField()
    not_transplantable_reason = models.CharField()

    # Randomisation data
    can_donate = models.BooleanField()
    can_transplant = models.BooleanField()
    preservation = models.PositiveSmallIntegerField(choices=PRESERVATION_CHOICES)
    #  Trial ID for Organ is dervived : WP4 _____ L/R

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
    perfusion_possible = models.BooleanField()
    perfusion_not_possible_because = models.CharField()
    perfusion_started = models.DateTimeField()
    perfusion_machine = models.ForeignKey(PerfusionMachine)
    perfusion_file = models.ForeignKey(PerfusionFile)
    patch_holder = models.PositiveSmallIntegerField(choices=PATCH_HOLDER_CHOICES)
    artificial_patch_used = models.BooleanField()
    artificial_patch_size = models.PositiveSmallIntegerField(choices=ARTIFICIAL_PATCH_CHOICES)
    artificial_patch_number = models.PositiveSmallIntegerField(min(1), max(2))
    # NB: There are ProcurementResources likely linked to this Organ
    oxygen_bottle_full = models.BooleanField()
    oxygen_bottle_open = models.BooleanField()
    oxygen_bottle_changed = models.BooleanField()
    oxygen_bottle_changed_at = models.DateTimeField()
    ice_container_replenished = models.BooleanField()
    ice_container_replenished_at = models.DateTimeField()
    perfusate_measurable = models.BooleanField()
    perfusate_measure = models.FloatField()  # TODO: Check the value range for this

    # Sampling data
    perfusate_1 = models.ForeignKey(Sample)
    perfusate_2 = models.ForeignKey(Sample)


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
    type = models.CharField(choices=TYPE_CHOICES)
    lot_number = models.CharField()
    expiry_date = models.DateField()
    created_on = models.DateTimeField(auto_now_add=True)
    created_by = models.ForeignKey(User)
