#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from bdateutil import relativedelta
from random import random
from livefield.managers import LiveManager

from django.conf import settings
from django.core.exceptions import ObjectDoesNotExist
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models
from django.utils import timezone
from django.utils.functional import cached_property
from django.utils.translation import ugettext_lazy as _

from wp4.staff.models import Person
from . import AuditControlModelBase
from ..validators import validate_not_in_future
from ..managers.core import RetrievalTeamModelForUserManager


class Patient(AuditControlModelBase):
    """
    Base attributes for a person involved in this case as a donor or recipient.

    Patients are not localised in and of themselves, but Donors and Recipients are, thus this class has no geographic
    permissions set. Similiarly, because this data is a subset of the Donor and Recipient records it will be treated
    using the permissions given to those objects.
    """
    MALE = 'M'  #: CONSTANT for GENDER_CHOICES
    FEMALE = 'F'  #: CONSTANT for GENDER_CHOICES
    GENDER_CHOICES = (
        (MALE, _('OPc01 Male')),
        (FEMALE, _('OPc02 Female'))
    )  #: Patient gender choices

    CAUCASIAN = 1  #: CONSTANT for ETHNICITY_CHOICES
    BLACK = 2  #: CONSTANT for ETHNICITY_CHOICES
    OTHER_ETHNICITY = 3  #: CONSTANT for ETHNICITY_CHOICES
    ETHNICITY_CHOICES = (
        (CAUCASIAN, _('OPc03 Caucasian')),
        (BLACK, _('OPc04 Black')),
        (OTHER_ETHNICITY, _('OPc05 Other'))
    )  #: Patient ethnicity choices

    BLOOD_O = 1  #: CONSTANT for BLOOD_GROUP_CHOICES
    BLOOD_A = 2  #: CONSTANT for BLOOD_GROUP_CHOICES
    BLOOD_B = 3  #: CONSTANT for BLOOD_GROUP_CHOICES
    BLOOD_AB = 4  #: CONSTANT for BLOOD_GROUP_CHOICES
    BLOOD_UNKNOWN = 5  #: CONSTANT for BLOOD_GROUP_CHOICES
    BLOOD_GROUP_CHOICES = (
        (BLOOD_O, 'O'),
        (BLOOD_A, 'A'),
        (BLOOD_B, 'B'),
        (BLOOD_AB, 'AB'),
        (BLOOD_UNKNOWN, _('OPc06 Unknown'))
    )  #: Patient blood_group choices

    # "ET Donor number/ NHSBT Number",
    number = models.CharField(verbose_name=_('OP01 NHSBT Number'), max_length=20, blank=True)
    date_of_birth = models.DateField(
        verbose_name=_('OP02 date of birth'),
        blank=True, null=True,
        validators=[validate_not_in_future],
        help_text="Date can not be in the future"
    )
    date_of_birth_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    date_of_death = models.DateField(
        verbose_name=_('OP08 date of death'),
        blank=True, null=True,
        validators=[validate_not_in_future],
        help_text="Date can not be in the future"
    )
    date_of_death_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    gender = models.CharField(verbose_name=_('OP03 gender'), choices=GENDER_CHOICES, max_length=1, default=MALE)
    weight = models.DecimalField(
        max_digits=4,
        decimal_places=1,
        verbose_name=_('OP04 Weight (kg)'),
        validators=[MinValueValidator(20.0), MaxValueValidator(200.0)],
        blank=True, null=True,
        help_text="Answer must be in range 20.0-200.0kg"
    )
    height = models.PositiveSmallIntegerField(
        verbose_name=_('OP05 Height (cm)'),
        validators=[MinValueValidator(100), MaxValueValidator(250)],
        blank=True, null=True,
        help_text="Answer must be in range 100-250cm"
    )
    ethnicity = models.IntegerField(verbose_name=_('OP06 ethnicity'), choices=ETHNICITY_CHOICES, blank=True, null=True)
    blood_group = models.PositiveSmallIntegerField(
        verbose_name=_('OP07 blood group'),
        choices=BLOOD_GROUP_CHOICES,
        blank=True, null=True
    )

    objects = LiveManager()

    class Meta:
        ordering = ['number']
        verbose_name = _('OPm1 trial person')
        verbose_name_plural = _('OPm2 organ people')
        # db_table = 'compare_patient'

    def clean(self):
        """
        Clears date_of_birth if unknown is flagged.
        Clears date_of_death if unknown is flagged.
        Error if date_of_death is in the future (OPv02).
        Error if date_of_death is before date_of_birth (OPv03)
        """
        # Clean the fields that are Unknown
        if self.date_of_birth_unknown:
            self.date_of_birth = None
        if self.date_of_death_unknown:
            self.date_of_death = None

        if self.date_of_death:
            if self.date_of_death > timezone.now().date():
                raise ValidationError(_("OPv02 Creepy prediction! Person's date of death is in the future!"))

        if self.date_of_birth and self.date_of_death:
            if self.date_of_death < self.date_of_birth:
                raise ValidationError(
                    _("OPv03 Time running backwards! Person's date of death is before they were born!"))

    @property
    def bmi_value(self):
        """
        Calculated BMI based on stored height and weight information.
        Uses http://www.nhs.uk/chq/Pages/how-can-i-work-out-my-bmi.aspx?CategoryID=51 for formula

        :return: BMI value (or None)
        :rtype: float
        """
        if self.height < 1 or self.weight < 1:
            return None  # _("DOv12 Not Available")
        height_in_m = self.height / 100
        return (self.weight / height_in_m) / height_in_m

    def _age_from_dob(self):
        """
        Determines a person's age from their Date of Birth, compared initially against a Date of Death
        (if it exists), or against the current date if not applicable.

        :return: age in years as a whole number, if date of birth is known, otherwise None
        :rtype: int
        """
        the_end = self.date_of_death if self.date_of_death else timezone.now().date()
        if self.date_of_birth:
            return relativedelta(the_end, self.date_of_birth).years
        return None
    age_from_dob = cached_property(_age_from_dob, name='age_from_dob')

    def _is_recipient(self):
        """
        Determine if a recipient record is linked to this person

        :return: True if recipient link exists
        :rtype: bool
        """
        try:
            return self.recipient is not None
        except ObjectDoesNotExist:
            return False
    is_recipient = cached_property(_is_recipient, name='is_recipient')

    def _is_donor(self):
        """
        Determine if a donor record is linked to this person

        :return: True if donor link exists
        :rtype: bool
        """
        try:
            return self.donor is not None
        except ObjectDoesNotExist:
            return False
    is_donor = cached_property(_is_donor, name='is_donor')

    @property
    def is_alive(self):
        """
        If no date of death is known, then presume person is alive.

        :return: True if date_of_death is unknown
        :rtype: bool
        """
        return True if self.date_of_death is None else False

    @property
    def trial_id(self):
        """
        Determine if donor or recipient, and then pass back their trial id
        :return:
        """
        if self.is_donor:
            return self.donor.trial_id
        return self.recipient.trial_id

    def __str__(self):
        if settings.DEBUG:
            return '%s : (%s, %s) %s' % (
                self.id, self.get_gender_display(), self.age_from_dob, self.number
            )
        else:
            return '(%s, %s) %s' % (
                self.get_gender_display(), self.age_from_dob, self.number
            )


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

    Randomisations are system defined and controlled, so whilst users shouldn't be troubled with permissions, we do
    want to use the hide permission here on users that are to be "blinded" to the effects of randomisation
    
    Allocation information should not be set until the record has been linked to a donor
    """
    LIVE_UNITED_KINGDOM = 1  #: CONSTANT for LIST_CHOICES
    LIVE_EUROPE = 2  #: CONSTANT for LIST_CHOICES
    PAPER_EUROPE = 3  #: CONSTANT for LIST_CHOICES
    PAPER_UNITED_KINGDOM = 4  #: CONSTANT for LIST_CHOICES
    LIST_CHOICES = (
        (LIVE_UNITED_KINGDOM, _("RNc01 UK Live list")),
        (LIVE_EUROPE, _("RNc02 Europe Live list")),
        (PAPER_UNITED_KINGDOM, _("RNc03 UK Offline list")),
        (PAPER_EUROPE, _("RNc04 Europe Offline list")),
    )  #: Randomisation list choices

    donor = models.OneToOneField(
        'Donor',
        null=True, blank=True,
        default=None,
        help_text="Internal link to the Donor"
    )
    list_code = models.PositiveSmallIntegerField(
        verbose_name=_("RA01 list code"),
        choices=LIST_CHOICES
    )  #: Choices limited to LIST_CHOICES
    result = models.BooleanField(verbose_name=_("RA02 result"), default=random_5050)
    allocated_on = models.DateTimeField(verbose_name=_("RA03 allocated on"), null=True, blank=True)
    allocated_by = models.ForeignKey(Person, verbose_name=_("RA04 allocated by"), default=None, null=True)

    class Meta:
        permissions = (
            ("hide_randomisation", "User should be kept ignorant of this data"),
        )

    @staticmethod
    def get_and_assign_result(list_code, link_donor, active_user):
        """
        Finds the next unassigned record on the randomisation list, and then assigns it to the
        Donor record supplied, whilst returning the result

        :param list_code: int. Value from LIST_CHOICES
        :param link_donor: Donor object. Donor record to link against this randomisation
        :param active_user: Person object. Currently logged in user
        :return: 'True' result is HMP+O2 for the Left Organ
        :rtype: bool
        """
        options = Randomisation.objects.filter(list_code=list_code, donor=None).order_by('id')
        if len(options) < 1:
            raise Exception("No remaining values for randomisation")
        result = options[0]
        result.donor = link_donor
        result.allocated_on = timezone.now()
        result.allocated_by = active_user
        result.save()
        return result.result

    def __str__(self):
        return '%s : %s' % (format(self.id, '03'), self.get_list_code_display())


class RetrievalTeam(models.Model):
    """
    Lookup class for the preset Retrieval Team list. Doesn't inherit from AuditControlModelBase as this
    is primarily a preset list of data, with helper functions attached.
    """
    from wp4.locations.models import Hospital, UNITED_KINGDOM

    centre_code = models.PositiveSmallIntegerField(
        verbose_name=_("RT01 centre code"),
        validators=[MinValueValidator(10), MaxValueValidator(99)],
        help_text="Value must be in the range 10-99"
    )
    based_at = models.ForeignKey(Hospital, verbose_name=_("RT02 base hospital"))

    objects = RetrievalTeamModelForUserManager()

    class Meta:
        ordering = ['centre_code']
        verbose_name = _('RTm1 retrieval team')
        verbose_name_plural = _('RTm2 retrieval teams')
        permissions = (
            ("view_retrievalteam", "Can only view the data"),
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )

    def country_for_restriction(self):
        """
        Get the country to be used for geographic restriction of this data
        :return: Int: Value from list in Locations.Models. Should be in range [1,4,5]
        """
        return self.based_at.country

    def location_for_restriction(self):
        """
        Get the location to be used for geographic restriction of this data
        :return: Int: Hospital object id
        """
        return self.based_at.id

    def next_sequence_number(self, is_online=True):
        """
        Return the next available sequence number, taking into account that the donor must already be
        linked to a randomisation record, and thus we're able to deduce if this is an online or offline
        case.

        :param: is_online: bool. Are we looking for the online or the offline sequence
        :return: Next free number in a linear sequence
        :rtype: int
        """
        list_code = self.get_randomisation_list(is_online)
        donor_set = self.donor_set.filter(randomisation__list_code=list_code)
        try:
            number = donor_set.latest('sequence_number').sequence_number + 1
        except models.Model.DoesNotExist:
            number = 1
        return number

    def get_randomisation_list(self, is_online=True):
        """
        Returns the id of the relevant randomisation list for the location of this team

        :param is_online: True, select from the online lists. False, select from the offline lists
        :return: Number matching one of the LIST_CHOICE constants
        :rtype: int
        """
        if self.based_at.country == RetrievalTeam.UNITED_KINGDOM:
            if is_online:
                return Randomisation.LIVE_UNITED_KINGDOM
            else:
                return Randomisation.PAPER_UNITED_KINGDOM
        else:
            if is_online:
                return Randomisation.LIVE_EUROPE
            else:
                return Randomisation.PAPER_EUROPE

    def _name(self):
        """
        Human readable name for the retrieval team

        :return: (Centre Code) Team Location Description
        :rtype: str
        """
        return '({0:d}) {1}'.format(self.centre_code, self.based_at.full_description)

    name = cached_property(_name, name='name')

    def _based_in_country(self):
        return self.based_at.get_country_display()

    based_in_country = cached_property(_based_in_country, name='based_in_country')

    def __str__(self):
        return self.name
