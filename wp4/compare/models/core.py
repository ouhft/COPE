#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals
import datetime
from django.conf import settings
from django.contrib.auth.models import User
from django.core.exceptions import ObjectDoesNotExist
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models
from django.utils.translation import ugettext_lazy as _
from django.utils import timezone
from wp4.locations.models import Hospital, UNITED_KINGDOM, BELGIUM, NETHERLANDS
from ..validators import validate_not_in_future

# Common CONSTANTS
NO = 0
YES = 1
UNKNOWN = 2
YES_NO_UNKNOWN_CHOICES = (
    (UNKNOWN, _("MMc03 Unknown")),
    (NO, _("MMc01 No")),
    (YES, _("MMc02 Yes"))
)  # Need Yes to be the last choice for any FieldWithFollowUp

# Originally from Organ
LEFT = "L"
RIGHT = "R"
LOCATION_CHOICES = (
    (LEFT, _('ORc01 Left')),
    (RIGHT, _('ORc02 Right')))

# Originally from Organ
PRESERVATION_HMP = 0
PRESERVATION_HMPO2 = 1
PRESERVATION_NOT_SET = 9
PRESERVATION_CHOICES = (
    (PRESERVATION_NOT_SET, _("ORc11 Not Set")),
    (PRESERVATION_HMP, "HMP"),
    (PRESERVATION_HMPO2, "HMP O2"))

# Originally from Randomisation
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


class VersionControlModel(models.Model):
    """
    Internal common attributes to aide record auditing
    """
    version = models.PositiveIntegerField(default=0)
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)
    record_locked = models.BooleanField(default=False)

    # TODO: Add save method here that aborts saving if record_locked is already true
    # NB: Used in Samples and FollowUp Apps too
    class Meta:
        abstract = True

    def save(self, force_insert=False, force_update=False, using=None,
             update_fields=None):
        self.created_on = timezone.now()
        self.version += 1
        return super(VersionControlModel, self).save(force_insert, force_update, using, update_fields)


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
    weight = models.DecimalField(
        max_digits=4,
        decimal_places=1,
        verbose_name=_('OP04 Weight (kg)'),
        validators=[MinValueValidator(20.0), MaxValueValidator(200.0)],
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


class RetrievalTeam(models.Model):
    centre_code = models.PositiveSmallIntegerField(
        verbose_name=_("RT01 centre code"),
        validators=[MinValueValidator(10), MaxValueValidator(99)])
    based_at = models.ForeignKey(Hospital, verbose_name=_("RT02 base hospital"))
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    def next_sequence_number(self, is_online=True):
        """
        Return the next available sequence number, taking into account that the donor must already be
        linked to a randomisation record, and thus we're able to deduce if this is an online or offline
        case.
        :param: is_online: bool. Are we looking for the online or the offline sequence
        :return: int, next free number
        """
        print("DEBUG: next_sequence_number called with is_online=%s" % is_online)
        list_code = self.get_randomisation_list(is_online)
        print("DEBUG: next_sequence_number list_code=%s" % list_code)
        donor_set = self.donor_set.filter(randomisation__list_code=list_code)
        try:
            number = donor_set.latest('sequence_number').sequence_number + 1
        except models.Model.DoesNotExist:
            number = 1
        print("DEBUG: next_sequence_number number=%s" % number)
        return number

    def get_randomisation_list(self, is_online=True):
        """
        Returns the id of the relevant randomisation list for the location of this team
        :param is_online: True, select from the online lists. False, select from the offline lists
        :return: int matching one of the LIST_CHOICE constants
        """
        if self.based_at.country == UNITED_KINGDOM:
            if is_online:
                return LIVE_UNITED_KINGDOM
            else:
                return PAPER_UNITED_KINGDOM
        else:
            if is_online:
                return LIVE_EUROPE
            else:
                return PAPER_EUROPE

    def name(self):
        return '(%d) %s' % (self.centre_code, self.based_at.full_description())

    def __unicode__(self):
        return self.name()

    class Meta:
        ordering = ['centre_code']
        verbose_name = _('RTm1 retrieval team')
        verbose_name_plural = _('RTm2 retrieval teams')
