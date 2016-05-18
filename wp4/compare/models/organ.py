#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models
from django.utils.functional import cached_property
from django.utils.translation import ugettext_lazy as _

from wp4.perfusion_machine.models import PerfusionMachine, PerfusionFile

from ..validators import validate_between_1900_2050, validate_not_in_future
from .core import BaseModelMixin, VersionControlMixin
from .core import LOCATION_CHOICES, PRESERVATION_CHOICES, PRESERVATION_NOT_SET
from .donor import Donor


class ClosedOrganManager(models.Manager):
    """
    Get only the Organs that have had completed outcomes. These are:

    * Organ.not_allocated_reason has a value... which results in...
    * Recipient.transplantation_form_completed is True

    So wait, what about if the Organ is allocated to a non project site hospital, doesn't that also
    close the case? Yes, yes it does, and we can more trivially cover this by writing a message into
    not_allocated_reason of $MESSAGE$, and then setting the transplantation_form_completed flag
    """
    def get_queryset(self):
        """
        Filter Organs results by only returning where transplantation_form_completed=True

        :return: Queryset
        """
        return super(ClosedOrganManager, self).get_queryset().\
            select_related('recipient'). \
            select_related('recipient__person').\
            prefetch_related('recipient__person__worksheet_set'). \
            select_related('donor'). \
            select_related('donor__person'). \
            select_related('donor__randomisation'). \
            select_related('donor__retrieval_team').\
            prefetch_related('organallocation_set').\
            filter(
                transplantation_form_completed=True
            )


class AllocatableOrganManager(models.Manager):
    """
    Get only the Organs that are available for allocation, which means they're Unallocated (not
    deliberately), Randomised, and still Transplantable (but not necessarily a completed P form).
    """
    def get_queryset(self):
        """
        Filter Organ results by organallocation__isnull=True AND transplantable=True, excluding results
        where preservation=PRESERVATION_NOT_SET OR transplantation_form_completed=True

        :return: Queryset
        """
        return super(AllocatableOrganManager, self).get_queryset().\
            select_related('donor'). \
            select_related('donor__person'). \
            select_related('donor__randomisation'). \
            select_related('donor__retrieval_team').\
            filter(
                organallocation__isnull=True, transplantable=True
            ).exclude(preservation=PRESERVATION_NOT_SET).exclude(transplantation_form_completed=True)


class OpenOrganManager(models.Manager):
    """
    Get only the Organs that are open for transplantation, which means they've one or more allocations,
    and have not had their form closed (either by allocation outside of project area, or completed form).
    """
    def get_queryset(self):
        """
        Filter Organ results by transplantable=True, excluding results where
        transplantation_form_completed=True OR organallocation__isnull=True OR
        preservation=PRESERVATION_NOT_SET

        :return: Queryset
        """
        return super(OpenOrganManager, self).get_queryset().\
            select_related('recipient'). \
            select_related('recipient__person').\
            prefetch_related('recipient__person__worksheet_set'). \
            select_related('donor'). \
            select_related('donor__person'). \
            select_related('donor__randomisation'). \
            select_related('donor__retrieval_team').\
            prefetch_related('organallocation_set').\
            filter(transplantable=True).\
            exclude(transplantation_form_completed=True).\
            exclude(organallocation__isnull=True).\
            exclude(preservation=PRESERVATION_NOT_SET)


class Organ(VersionControlMixin):
    """
    The focus of the trial, specifically a Kidney.
    """
    donor = models.ForeignKey(Donor, help_text="Internal link to the Donor")
    location = models.CharField(
        verbose_name=_('OR01 kidney location'),
        max_length=1,
        choices=LOCATION_CHOICES
    )  #: Choices limited to LOCATION_CHOICES

    # Transplantation Form metadata
    not_allocated_reason = models.CharField(
        verbose_name=_('OR31 not transplantable because'),
        max_length=250,
        blank=True
    )
    admin_notes = models.TextField(verbose_name=_("OR50 Admin notes"), blank=True)
    transplantation_notes = models.TextField(verbose_name=_("OR51 Transplantation notes"), blank=True)
    transplantation_form_completed = models.BooleanField(
        verbose_name=_("OR99 Form complete"),
        default=False,
        help_text="Select Yes when you believe the form is complete and you have no more data to enter"
    )

    # Inspection data
    GRAFT_DAMAGE_ARTERIAL = 1  #: Constant for GRAFT_DAMAGE_CHOICES
    GRAFT_DAMAGE_VENOUS = 2  #: Constant for GRAFT_DAMAGE_CHOICES
    GRAFT_DAMAGE_URETERAL = 3  #: Constant for GRAFT_DAMAGE_CHOICES
    GRAFT_DAMAGE_PARENCHYMAL = 4  #: Constant for GRAFT_DAMAGE_CHOICES
    GRAFT_DAMAGE_NONE = 5  #: Constant for GRAFT_DAMAGE_CHOICES
    GRAFT_DAMAGE_OTHER = 6  #: Constant for GRAFT_DAMAGE_CHOICES
    GRAFT_DAMAGE_CHOICES = (
        (GRAFT_DAMAGE_NONE, _("ORc01 None")),
        (GRAFT_DAMAGE_ARTERIAL, _("ORc02 Arterial Damage")),
        (GRAFT_DAMAGE_VENOUS, _("ORc03 Venous Damage")),
        (GRAFT_DAMAGE_URETERAL, _("ORc04 Ureteral Damage")),
        (GRAFT_DAMAGE_PARENCHYMAL, _("ORc05 Parenchymal Damage")),
        (GRAFT_DAMAGE_OTHER, _("ORc06 Other Damage"))
    )  #: Organ graft_damage choices

    WASHOUT_PERFUSION_HOMEGENOUS = 1  #: Constant for WASHOUT_PERFUSION_CHOICES
    WASHOUT_PERFUSION_PATCHY = 2  #: Constant for WASHOUT_PERFUSION_CHOICES
    WASHOUT_PERFUSION_BLUE = 3  #: Constant for WASHOUT_PERFUSION_CHOICES
    WASHOUT_PERFUSION_UNKNOWN = 9  #: Constant for WASHOUT_PERFUSION_CHOICES
    WASHOUT_PERFUSION_CHOICES = (
        # NHS Form has: Good, Fair, Poor, Patchy, Unknown
        (WASHOUT_PERFUSION_HOMEGENOUS, _("ORc07 Homogenous")),
        (WASHOUT_PERFUSION_PATCHY, _("ORc08 Patchy")),
        (WASHOUT_PERFUSION_BLUE, _("ORc09 Blue")),
        (WASHOUT_PERFUSION_UNKNOWN, _("ORc10 Unknown"))
    )  #: Organ washout_perfusion choices

    removal = models.DateTimeField(
        verbose_name=_('OR02 time out'),
        blank=True,
        null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    renal_arteries = models.PositiveSmallIntegerField(
        verbose_name=_('OR03 number of renal arteries'),
        blank=True, null=True,
        validators=[MinValueValidator(0), MaxValueValidator(5)],
        help_text="Number of arteries must be in range 0-5"
    )
    graft_damage = models.PositiveSmallIntegerField(
        verbose_name=_('OR04 renal graft damage'),
        choices=GRAFT_DAMAGE_CHOICES,
        default=GRAFT_DAMAGE_NONE
    )  #: Choices limited to GRAFT_DAMAGE_CHOICES
    graft_damage_other = models.CharField(verbose_name=_('OR05 other damage done'), max_length=250, blank=True)
    washout_perfusion = models.PositiveSmallIntegerField(
        verbose_name=_('OR06 perfusion characteristics'),
        choices=WASHOUT_PERFUSION_CHOICES,
        blank=True, null=True
    )  #: Choices limited to WASHOUT_PERFUSION_CHOICES
    transplantable = models.NullBooleanField(
        verbose_name=_('OR07 is transplantable'),
        blank=True, null=True,
        help_text=_("OR07h This answer can be amended after randomisation and saving of the form if necessary")
    )
    not_transplantable_reason = models.CharField(
        verbose_name=_('OR08 not transplantable because'),
        max_length=250,
        blank=True
    )

    # Randomisation data
    # can_donate = models.BooleanField('Donor is eligible as DCD III and > 50 years old') -- donor info!
    # can_transplant = models.BooleanField('') -- derived from left and right being transplantable
    preservation = models.PositiveSmallIntegerField(
        choices=PRESERVATION_CHOICES,
        default=PRESERVATION_NOT_SET
    )  #: Choices limited to PRESERVATION_CHOICES

    # Perfusion data
    PATCH_SMALL = 1  #: Constant for PATCH_HOLDER_CHOICES
    PATCH_LARGE = 2  #: Constant for PATCH_HOLDER_CHOICES
    PATCH_DOUBLE_ARTERY = 3  #: Constant for PATCH_HOLDER_CHOICES
    PATCH_HOLDER_CHOICES = (
        (PATCH_SMALL, _("ORc12 Small")),
        (PATCH_LARGE, _("ORc13 Large")),
        (PATCH_DOUBLE_ARTERY, _("ORc14 Double Artery"))
    )  #: Organ patch_holder choices
    ARTIFICIAL_PATCH_CHOICES = (
        (PATCH_SMALL, _("ORc12 Small")),
        (PATCH_LARGE, _("ORc13 Large"))
    )  #: Organ artificial_patch_size choices
    perfusion_possible = models.NullBooleanField(
        verbose_name=_('OR09 machine perfusion possible?'),
        blank=True, null=True
    )
    perfusion_not_possible_because = models.CharField(
        verbose_name=_('OR10 not possible because'),
        max_length=250,
        blank=True
    )
    perfusion_started = models.DateTimeField(
        verbose_name=_('OR11 machine perfusion'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    patch_holder = models.PositiveSmallIntegerField(
        verbose_name=_('OR12 used patch holder'),
        choices=PATCH_HOLDER_CHOICES,
        blank=True, null=True
    )  #: Choices limited to PATCH_HOLDER_CHOICES
    artificial_patch_used = models.NullBooleanField(verbose_name=_('OR13 artificial patch used'), blank=True, null=True)
    artificial_patch_size = models.PositiveSmallIntegerField(
        verbose_name=_('OR14 artificial patch size'),
        choices=ARTIFICIAL_PATCH_CHOICES,
        blank=True, null=True
    )  #: Choices limited to ARTIFICIAL_PATCH_CHOICES
    artificial_patch_number = models.PositiveSmallIntegerField(
        verbose_name=_('OR15 number of patches'),
        blank=True,
        null=True,
        validators=[MinValueValidator(1), MaxValueValidator(2)]
    )  #: Limited to range 1-2
    oxygen_bottle_full = models.NullBooleanField(
        verbose_name=_('OR16 is oxygen bottle full'),
        blank=True, null=True
    )
    oxygen_bottle_open = models.NullBooleanField(verbose_name=_('OR17 oxygen bottle opened'), blank=True, null=True)
    oxygen_bottle_changed = models.NullBooleanField(verbose_name=_('OR18 oxygen bottle changed'), blank=True, null=True)
    oxygen_bottle_changed_at = models.DateTimeField(
        verbose_name=_('OR19 oxygen bottle changed at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    oxygen_bottle_changed_at_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    ice_container_replenished = models.NullBooleanField(
        verbose_name=_('OR20 ice container replenished'),
        blank=True, null=True
    )
    ice_container_replenished_at = models.DateTimeField(
        verbose_name=_('OR21 ice container replenished at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    ice_container_replenished_at_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    perfusate_measurable = models.NullBooleanField(
        verbose_name=_('OR22 perfusate measurable'),
        blank=True, null=True
    )  #: logistically possible to measure pO2 perfusate (use blood gas analyser)
    perfusate_measure = models.FloatField(verbose_name=_('OR23 value pO2'), blank=True, null=True)
    # TODO: Check the value range for perfusate_measure
    perfusion_machine = models.ForeignKey(
        PerfusionMachine,
        verbose_name=_('OR24 perfusion machine'),
        blank=True,
        null=True
    )
    perfusion_file = models.ForeignKey(PerfusionFile, verbose_name=_('OR25 machine file'), blank=True, null=True)

    # Commonly filtered options
    objects = models.Manager()  #: Default Organ manager
    allocatable_objects = AllocatableOrganManager()  #: AllocatableOrganManager
    open_objects = OpenOrganManager()  #: OpenOrganManager
    closed_objects = ClosedOrganManager()  #: ClosedOrganManager

    def clean(self):
        """
        Clears the following fields of data if their corresponding unknown flag is set to True

        * oxygen_bottle_changed_at
        * ice_container_replenished_at

        Error if:

        * transplantable is False, and not_transplantable_reason is empty (ORv01)
        * perfusion_possible is False, and perfusion_not_possible_because is empty (ORv02)
        * perfusion_possible is True, and perfusion_started is not set (ORv03)

        """
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
            raise ValidationError(_("ORv03 Please enter the time perfusion started at"))


    @property
    def safe_recipient(self):
        """
        Helper method to return either the related Recipient record, or a safe value of None,
        without the drama of exceptions when it doesn't exist

        :rtype: wp4.compare.models.transplantation.Recipient
        """
        try:
            if self.recipient is not None:
                return self.recipient  # We have a recipient
        except AttributeError:  # RelatedObjectDoesNotExist
            return None

    def _final_allocation(self):
        """
        Work out if there are any OrganAllocations, and then return the latest one

        :return: OrganAllocation, or None
        :rtype: wp4.compare.models.transplantation.OrganAllocation
        """
        return self.organallocation_set.order_by('id').last()

    final_allocation = cached_property(_final_allocation, name='final_allocation')

    def _trial_id(self):
        """
        Returns the Donor Trial ID combined with the Location (L or R) for the Organ

        :return: 'WP4cctnns'
        :rtype: str
        """
        return self.donor.trial_id + self.location

    trial_id = cached_property(_trial_id, name='trial_id')

    @property
    def is_allocated(self):
        """
        Allocation status

        Determine if an organ has been allocated. Allocated means:

        * To a recipient at a project site
        * Final allocation is to a non-project site

        :return: True, if either of those criteria are met
        :rtype: bool
        """
        if self.safe_recipient is not None:
            return True  # We have a recipient, which can only occur if at a project site

        final_allocation = self.final_allocation
        if final_allocation is not None:
            if final_allocation.reallocated is False and final_allocation.transplant_hospital.is_project_site is False:
                # We can't say not-reallocated without saving a transplant hospital as well
                return True
        return False

    @property
    def explain_is_allocated(self):
        """
        Allocation status description: An explanation as to the allocation status for this organ

        :return: Message describing status of allocation
        :rtype: str
        """
        final_allocation = self.final_allocation
        if self.is_allocated:
            if self.safe_recipient is not None:
                return "Allocated to Recipient"

            if final_allocation.reallocated is False and final_allocation.transplant_hospital.is_project_site is False:
                return "Allocated to a non-project site"  # This should be caught by not_allocated_reason
        else:
            if self.not_allocated_reason:
                return "Not allocated because: %s" % self.not_allocated_reason

            if final_allocation is None:
                return "No allocations created (and no explanation as yet)"

            if final_allocation.reallocated is True:
                return "ERROR: last allocation shows a reallocation"  # This shouldn't occur!

            return "Re-allocation status not yet set"  # Possible for a form that is WIP

        return "ERROR: Unknown allocation status (test data?)"

    @property
    def explain_closed_status(self):
        """
        Work out why this form was closed, and display a suitable summary message

        NOT IMPLEMENTED (yet)

        :return: Message describing the likely cause for the form being closed
        :rtype: str
        """
        # TODO: Write this function
        return u"Unknown closed status"

    def _reallocation_count(self):
        """
        Counts the number of organ allocations where reallocated is true

        :return: Count of reallocations
        :rtype: int
        """
        count = 0
        for allocation in self.organallocation_set.all():
            if allocation.reallocated is True:
                count += 1
        return count

    reallocation_count = cached_property(_reallocation_count, name='reallocation_count')

    def __unicode__(self):
        return self.trial_id

    class Meta:
        verbose_name = _('ORm1 organ')
        verbose_name_plural = _('ORm2 organs')


class ProcurementResource(BaseModelMixin):
    """
    Repeatable list of resources used during organ extraction. Primarily distinguished by the type.
    """
    DISPOSABLES = "D"  #: Constant for TYPE_CHOICES
    EXTRA_CANNULA_SMALL = "C-SM"  #: Constant for TYPE_CHOICES
    EXTRA_CANNULA_LARGE = "C-LG"  #: Constant for TYPE_CHOICES
    EXTRA_PATCH_HOLDER_SMALL = "PH-SM"  #: Constant for TYPE_CHOICES
    EXTRA_PATCH_HOLDER_LARGE = "PH-LG"  #: Constant for TYPE_CHOICES
    EXTRA_DOUBLE_CANNULA_SET = "DB-C"  #: Constant for TYPE_CHOICES
    PERFUSATE_SOLUTION = "P"  #: Constant for TYPE_CHOICES
    TYPE_CHOICES = (
        (DISPOSABLES, _("PRc01 Disposables")),
        (EXTRA_CANNULA_SMALL, _("PRc02 Extra cannula small (3mm)")),
        (EXTRA_CANNULA_LARGE, _("PRc03 Extra cannula large (5mm)")),
        (EXTRA_PATCH_HOLDER_SMALL, _("PRc04 Extra patch holder small")),
        (EXTRA_PATCH_HOLDER_LARGE, _("PRc05 Extra patch holder large")),
        (EXTRA_DOUBLE_CANNULA_SET, _("PRc06 Extra double cannula set")),
        (PERFUSATE_SOLUTION, _("PRc07 Perfusate solution"))
    )  #: ProcurementResource type choices
    organ = models.ForeignKey(Organ, verbose_name=_('PR01 related kidney'))
    type = models.CharField(verbose_name=_('PR02 resource used'), choices=TYPE_CHOICES, max_length=5)
    lot_number = models.CharField(verbose_name=_('PR03 lot number'), max_length=50, blank=True)
    expiry_date = models.DateField(verbose_name=_('PR04 expiry date'), blank=True, null=True)
    expiry_date_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")

    def __unicode__(self):
        return self.get_type_display() + ' for ' + self.organ.trial_id

    class Meta:
        verbose_name = _('PRm1 procurement resource')
        verbose_name_plural = _('PRm2 procurement resources')

    def clean(self):
        """
        Clears the value of expiry_date if marked as unknown
        """
        # Clean the fields that at Not Known
        if self.expiry_date_unknown:
            self.expiry_date = None
