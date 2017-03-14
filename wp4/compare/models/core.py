#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from bdateutil import relativedelta
from livefield import LiveField, LiveManager

from django.conf import settings
from django.core.exceptions import ObjectDoesNotExist
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models
from django.utils import timezone
from django.utils.functional import cached_property
from django.utils.translation import ugettext_lazy as _

from ..validators import validate_not_in_future


# Common CONSTANTS
NO = 0  #: CONSTANT for YES_NO_UNKNOWN_CHOICES
YES = 1  #: CONSTANT for YES_NO_UNKNOWN_CHOICES
UNKNOWN = 2  #: CONSTANT for YES_NO_UNKNOWN_CHOICES
YES_NO_UNKNOWN_CHOICES = (
    (UNKNOWN, _("MMc03 Unknown")),
    (NO, _("MMc01 No")),
    (YES, _("MMc02 Yes"))
)  #: Need Yes to be the last choice for any FieldWithFollowUp where additional elements appear on Yes

# Originally from Organ
LEFT = "L"  #: CONSTANT for LOCATION_CHOICES
RIGHT = "R"  #: CONSTANT for LOCATION_CHOICES
LOCATION_CHOICES = (
    (LEFT, _('ORc01 Left')),
    (RIGHT, _('ORc02 Right'))
)   #: Organ location choices

# Originally from Organ
PRESERVATION_HMP = 0  #: CONSTANT for PRESERVATION_CHOICES
PRESERVATION_HMPO2 = 1  #: CONSTANT for PRESERVATION_CHOICES
PRESERVATION_NOT_SET = 9  #: CONSTANT for PRESERVATION_CHOICES
PRESERVATION_CHOICES = (
    (PRESERVATION_NOT_SET, _("ORc11 Not Set")),
    (PRESERVATION_HMP, "HMP"),
    (PRESERVATION_HMPO2, "HMP O2")
)  #: Organ preservation choices

# Originally from Randomisation
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


class BaseModelMixin(models.Model):
    """
    Common data record fields for tracking changes
    """
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(
        settings.AUTH_USER_MODEL,
        help_text="User account for the person logged in when this record was made/updated",
        related_name='%(app_label)s_%(class)s_created_by'
    )
    # https://docs.djangoproject.com/en/1.10/topics/db/models/#abstract-related-name

    class Meta:
        abstract = True

    def save(self, created_by=None, force_insert=False, force_update=False, using=None,
             update_fields=None):
        """
        Meta save function that allows models to have their save() method called, and to set the required
        created_by link at the same time.
        """
        self.created_on = timezone.now()
        if created_by:
            self.created_by = created_by
        if not self.created_by:
            raise Exception("%s Record does not have created_by set" % type(self).__name__)
        return super(BaseModelMixin, self).save(force_insert, force_update, using, update_fields)


class VersionControlMixin(BaseModelMixin):
    """
    Internal common attributes to aide system auditing of records
    """
    version = models.PositiveIntegerField(default=0, help_text="Internal tracking version number")
    record_locked = models.BooleanField(default=False, help_text="Not presently implemented or used")
    live = LiveField()  # Wanted this to be record_active, but that means modifying the LiveField code

    objects = LiveManager()
    all_objects = LiveManager(include_soft_deleted=True)

    # NB: Used in multiple apps
    class Meta:
        abstract = True
        # NB: Class names are not yet dynamically added to permissions as we would want. May appear in django 1.11?
        # http://stackoverflow.com/questions/4963428/how-to-dynamically-name-permissions-in-a-django-abstract-model-class
        permissions = (
            ("view", "Can view the data, but not modify it"),
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )

    def save(self, created_by=None, force_insert=False, force_update=False, using=None,
             update_fields=None):
        """
        Meta save function that increments the version number over the previous saved version.
        """
        if self.record_locked:
            raise Exception("%s Record is locked, and can not be saved" % type(self).__name__)
        self.version += 1
        return super(VersionControlMixin, self).save(
            created_by,
            force_insert,
            force_update,
            using,
            update_fields
        )


class MissingUser(Exception):
    """A request was missing from the object manager"""
    pass


class MissingUserLocation(Exception):
    """A request.user has no valid based_at location"""
    pass


class ModelByRequestManagerBase(models.Manager):
    country_id = None
    hospital_id = None
    current_user = None

    def for_user(self, user=None):
        """
        To enable per user filtering, we need to have this passed into the manager by a mechanism such as this. If
        no user is specified, then the manager should continue to work as the default manager

        :param user:
        :return:
        """
        if user is None:
            raise MissingUser("ObjectManager.for_user has no user specified")
        else:
            self.current_user = user

            if self.current_user.based_at is None:
                raise MissingUserLocation("ObjectManager.get_queryset current user has no location set in profile")
            else:
                self.country_id = self.current_user.based_at.country
                self.hospital_id = self.current_user.based_at.id

        return self.get_queryset()


class OrganPersonRequestManager(ModelByRequestManagerBase):
    def get_queryset(self):
        """
        Test for permissions to view and restrict based on rules

        :param request:
        :return:
        """
        qs = super(OrganPersonRequestManager, self).get_queryset().\
            prefetch_related('donor').\
            prefetch_related('recipient')

        if self.current_user is not None:
            if self.current_user.is_superuser:
                # http://stackoverflow.com/questions/2507086/django-auth-has-perm-returns-true-while-list-of-permissions-is-empty/2508576
                # Superusers get *all* permissions :-/
                return qs

            if self.current_user.has_perm('restrict_to_local'):
                return qs.filter(
                    models.Q(donor__retrieval_team__based_at_id=self.hospital_id) |
                    models.Q(recipient__allocation__transplant_hospital_id=self.hospital_id)
                )

            if self.current_user.has_perm('restrict_to_national'):
                return qs.filter(
                    models.Q(donor__retrieval_team__based_at__country=self.country_id) |
                    models.Q(recipient__allocation__transplant_hospital__based_at__country=self.country_id)
                )

        return qs


class OrganPerson(VersionControlMixin):
    """
    Base attributes for a person involved in this case as a donor or recipient
    """
    MALE = 'M'  #: CONSTANT for GENDER_CHOICES
    FEMALE = 'F'  #: CONSTANT for GENDER_CHOICES
    GENDER_CHOICES = (
        (MALE, _('OPc01 Male')),
        (FEMALE, _('OPc02 Female'))
    )  #: OrganPerson gender choices

    CAUCASIAN = 1  #: CONSTANT for ETHNICITY_CHOICES
    BLACK = 2  #: CONSTANT for ETHNICITY_CHOICES
    OTHER_ETHNICITY = 3  #: CONSTANT for ETHNICITY_CHOICES
    ETHNICITY_CHOICES = (
        (CAUCASIAN, _('OPc03 Caucasian')),
        (BLACK, _('OPc04 Black')),
        (OTHER_ETHNICITY, _('OPc05 Other'))
    )  #: OrganPerson ethnicity choices

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
    )  #: OrganPerson blood_group choices

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

    objects = OrganPersonRequestManager()

    class Meta(VersionControlMixin.Meta):
        ordering = ['number']
        verbose_name = _('OPm1 trial person')
        verbose_name_plural = _('OPm2 organ people')

    def clean(self):
        """
        Clears date_of_birth if unknown is flagged.
        Clears date_of_death if unknown is flagged.
        Error if date_of_death is in the future (OPv02).
        Error if date_of_death is before date_of_birth (OPv03)
        """
        # Clean the fields that at Not Known
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

    # def _trial_id(self):
    #     """
    #     Returns the composite trial id string by calling either donor.trial_id() or recipient.trial_id()
    #
    #     :return: If no donor or recipient trial id found, returns "No Trial ID Assigned"
    #     :rtype: str
    #     """
    #     if self.is_donor:
    #         return self.donor.trial_id()
    #     elif self.is_recipient:
    #         return self.recipient.trial_id()
    #     return _("OPm01 No Trial ID Assigned")
    #
    # trial_id = cached_property(_trial_id, name='trial_id')

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

    def _worksheet(self):
        """
        Looks for the first sample worksheet linked to this person

        :return: Samples.Worksheet object, or None
        :rtype: wp4.samples.models.worksheet
        """
        for worksheet in self.worksheet_set.all():
            return worksheet
        return None

    worksheet = cached_property(_worksheet, name='worksheet')

    def __str__(self):
        if settings.DEBUG:
            return '%s : (%s, %s) %s' % (
                self.id, self.get_gender_display(), self.age_from_dob, self.number
            )
        else:
            return '(%s, %s) %s' % (
                self.get_gender_display(), self.age_from_dob, self.number
            )


class RetrievalTeamManager(models.Manager):
    def get_queryset(self):
        return super(RetrievalTeamManager, self).get_queryset().select_related('based_at')


class RetrievalTeam(BaseModelMixin):
    """
    Lookup class for the preset Retrieval Team list. Doesn't inherit from VersionControlMixin as this
    is primarily a preset list of data, with helper functions attached.
    """
    from wp4.locations.models import Hospital

    centre_code = models.PositiveSmallIntegerField(
        verbose_name=_("RT01 centre code"),
        validators=[MinValueValidator(10), MaxValueValidator(99)],
        help_text="Value must be in the range 10-99"
    )
    based_at = models.ForeignKey(Hospital, verbose_name=_("RT02 base hospital"))

    objects = RetrievalTeamManager()

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
        if self.based_at.country == self.UNITED_KINGDOM:
            if is_online:
                return LIVE_UNITED_KINGDOM
            else:
                return PAPER_UNITED_KINGDOM
        else:
            if is_online:
                return LIVE_EUROPE
            else:
                return PAPER_EUROPE

    def _name(self):
        """
        Human readable name for the retrieval team

        :return: (Centre Code) Team Location Description
        :rtype: str
        """
        return '(%d) %s' % (self.centre_code, self.based_at.full_description)

    name = cached_property(_name, name='name')

    def _based_in_country(self):
        return self.based_at.get_country_display()

    based_in_country = cached_property(_based_in_country, name='based_in_country')

    def __str__(self):
        return self.name

    class Meta:
        ordering = ['centre_code']
        verbose_name = _('RTm1 retrieval team')
        verbose_name_plural = _('RTm2 retrieval teams')
