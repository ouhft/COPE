#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals
from django.contrib.auth.models import User
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models
from django.utils.translation import ugettext_lazy as _
from django.utils import timezone
from wp4.perfusion_machine.models import PerfusionMachine, PerfusionFile
from ..validators import validate_between_1900_2050, validate_not_in_future
from .core import VersionControlModel
from .core import LOCATION_CHOICES, PRESERVATION_CHOICES, PRESERVATION_NOT_SET
from .donor import Donor


class ClosedOrganManager(models.Manager):
    """
    Get only the Organs that have had completed outcomes. These are:
    - Organ.not_allocated_reason has a value
    - Recipient.form_completed is True
    But wait, what about if the Organ is allocated to a non project site hospital, doesn't that also
    close the case? Yes, yes it does, and we can more trivially cover this by writing a message into
    not_allocated_reason of $MESSAGE$
    """
    def get_queryset(self):
        # return super(ClosedOrganManager, self).get_queryset().filter(
        #     models.Q(not_allocated_reason__gt='') |
        #     models.Q(recipient__form_completed=True)
        # )
        return super(ClosedOrganManager, self).get_queryset().exclude(
            transplantation_form_completed=True
        )


class AllocatableOrganManager(models.Manager):
    """
    Get only the Organs that are available for allocation, which means they're Unallocated (not
    deliberately), Randomised, and still Transplantable (but not necessarily a completed P form).
    """
    def get_queryset(self):
        return super(AllocatableOrganManager, self).get_queryset().filter(
            organallocation__isnull=True, transplantable=True
        ).exclude(preservation=PRESERVATION_NOT_SET).exclude(transplantation_form_completed=True)


class OpenOrganManager(models.Manager):
    """
    Get only the Organs that are open for transplantation, which means they've one or more allocations,
    and have not had their form closed (either by allocation outside of project area, or completed form).
    """
    def get_queryset(self):
        pks_to_exclude = [o.pk for o in Organ.allocatable_objects.all()]
        return super(OpenOrganManager, self).get_queryset().\
            exclude(transplantation_form_completed=True).\
            exclude(id__in=pks_to_exclude)


class Organ(VersionControlModel):  # Or specifically, a Kidney
    donor = models.ForeignKey(Donor)  # Internal value
    location = models.CharField(
        verbose_name=_('OR01 kidney location'),
        max_length=1,
        choices=LOCATION_CHOICES
    )

    # Transplantation Form metadata
    not_allocated_reason = models.CharField(
        verbose_name=_('OR31 not transplantable because'),
        max_length=250,
        blank=True
    )
    admin_notes = models.TextField(verbose_name=_("DO50 Admin notes"), blank=True)
    transplantation_notes = models.TextField(verbose_name=_("DO51 Transplantation notes"), blank=True)
    transplantation_form_completed = models.BooleanField(default=False)  # Internal value

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
        (GRAFT_DAMAGE_OTHER, _("ORc06 Other Damage"))
    )

    WASHOUT_PERFUSION_HOMEGENOUS = 1
    WASHOUT_PERFUSION_PATCHY = 2
    WASHOUT_PERFUSION_BLUE = 3
    WASHOUT_PERFUSION_UNKNOWN = 9
    WASHOUT_PERFUSION_CHOICES = (
        # NHS Form has: Good, Fair, Poor, Patchy, Unknown
        (WASHOUT_PERFUSION_HOMEGENOUS, _("ORc07 Homogenous")),
        (WASHOUT_PERFUSION_PATCHY, _("ORc08 Patchy")),
        (WASHOUT_PERFUSION_BLUE, _("ORc09 Blue")),
        (WASHOUT_PERFUSION_UNKNOWN, _("ORc10 Unknown"))
    )

    removal = models.DateTimeField(
        verbose_name=_('OR02 time out'),
        blank=True,
        null=True,
        validators=[validate_between_1900_2050, validate_not_in_future]
    )
    renal_arteries = models.PositiveSmallIntegerField(
        verbose_name=_('OR03 number of renal arteries'),
        blank=True, null=True,
        validators=[MinValueValidator(0), MaxValueValidator(5)]
    )
    graft_damage = models.PositiveSmallIntegerField(
        verbose_name=_('OR04 renal graft damage'),
        choices=GRAFT_DAMAGE_CHOICES,
        default=GRAFT_DAMAGE_NONE
    )
    graft_damage_other = models.CharField(verbose_name=_('OR05 other damage done'), max_length=250, blank=True)
    washout_perfusion = models.PositiveSmallIntegerField(
        verbose_name=_('OR06 perfusion characteristics'),
        choices=WASHOUT_PERFUSION_CHOICES,
        blank=True, null=True
    )
    transplantable = models.NullBooleanField(verbose_name=_('OR07 is transplantable'), blank=True, null=True)
    not_transplantable_reason = models.CharField(
        verbose_name=_('OR08 not transplantable because'),
        max_length=250,
        blank=True
    )

    # Randomisation data
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

    # Commonly filtered options
    objects = models.Manager()
    allocatable_objects = AllocatableOrganManager()
    open_objects = OpenOrganManager()
    closed_objects = ClosedOrganManager()

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

        if self.donor.procurement_form_completed:
            pass

    def trial_id(self):
        return self.donor.trial_id() + self.location

    def is_allocated(self):
        if self.not_allocated_reason is None:
            for allocation in self.organallocation_set.all():
                if allocation.reallocated is False and self.recipient.allocation is not None:
                    return True
        return False

    def reallocation_count(self):
        count = 0
        for allocation in self.organallocation_set.all():
            if allocation.reallocated is True:
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
