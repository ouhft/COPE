from django.db import models

# Create your models here.
class Person(models.Model):
    first_names = models.CharField()
    last_names = models.CharField()
    # full_name is derived from the above

class User(Person):
    # Link this to the Auth system


class VersionControlModel(models.Model):
    version = models.PositiveIntegerField()
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
    OTHER = 3
    ETHNICITY_CHOICES = (
        (CAUCASIAN, 'Caucasian'),
        (BLACK, 'Black'),
        (OTHER, 'Other')
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

    number = models.CharField(verbose_name="ET Donor number/ NHSBT Number")
    age = models.PositiveSmallIntegerField()
    date_of_birth = models.DateField()
    date_of_admission = models.DateField()
    admitted_to_itu = models.BooleanField()
    date_admitted_to_itu = models.DateField()
    date_of_procurement = models.DateField()
    gender = models.CharField(choices=GENDER_CHOICES, max_length=1)
    ethnicity = models.IntegerField(choices=ETHNICITY_CHOICES)
    weight = models.PositiveSmallIntegerField(min(20), max(200), verbose_name='Weight (kg)')
    height = models.PositiveSmallIntegerField(min(100), max(250), verbose_name='Height (cm)')
    # BMI is a derived value
    blood_group = models.PositiveSmallIntegerField(choices=BLOOD_GROUP_CHOICES)


class OrgansOffered(models.Model):
    LIVER = 1
    LUNG = 2
    PANCREAS = 3
    ORGAN_CHOICES = (
        (LIVER, 'Liver'),
        (LUNG, 'Lung'),
        (PANCREAS, 'Pancreas')
    )
    organ = models.PositiveSmallIntegerField(choices=ORGAN_CHOICES)
    donor = models.ForeignKey(Donor)


class RetrievalTeam(models.Model):
    name = models.CharField()
    country = models.CharField()
    centre_code = models.PositiveSmallIntegerField(min(10), max(99))


class RetrievalHospital(models.Model):
    name = models.CharField()
    is_active = models.BooleanField(default=True)


class Trial(VersionControlModel):
    retrieval_team = models.ForeignKey(RetrievalTeam)
    criteria_check_1 = models.BooleanField()
    criteria_check_2 = models.BooleanField()
    criteria_check_3 = models.BooleanField()
    criteria_check_4 = models.BooleanField()
    is_active = models.BooleanField()  # Can be automatically set based on answers in the system
    trial_id = models.CharField()  # Generated from data "WP4" + RetrievalTeam.CentreCode + Trial.ID(padded to three numbers)


class Procedure(VersionControlModel):
    perfusion_technician = models.ForeignKey(Person)
    transplant_coordinator = models.ForeignKey(Person)
    call_received = models.DateTimeField()
    retrieval_hospital = models.ForeignKey(RetrievalHospital)
    scheduled_start = models.DateTimeField()
    technician_arrival = models.DateTimeField()
    ice_boxes_filled = models.DateTimeField()
    depart_perfusion_centre = models.DateTimeField()
    arrival_at_donor_hospital = models.DateTimeField()


