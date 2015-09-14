#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib.auth.models import User
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _, ungettext_lazy as __

from ..compare.models import Organ


class VersionControlModel(models.Model):
    version = models.PositiveIntegerField(default=0)
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)
    record_locked = models.BooleanField(default=False)

    # TODO: Add save method here that aborts saving if record_locked is already true
    # TODO: Add version control via django-reversion

    class Meta:
        abstract = True


class FollowUpBase(VersionControlModel):
    FORMTYPE_INITIAL = 1
    FORMTYPE_3M = 2
    FORMTYPE_6M = 3
    FORMTYPE_1Y = 4
    FORMTYPE_CHOICES = (
        (FORMTYPE_INITIAL, _("Follow Up Days 1-7")),
        (FORMTYPE_3M, _("Follow Up 3 Months")),
        (FORMTYPE_6M, _("Follow Up 6 Months")),
        (FORMTYPE_1Y, _("Follow Up 1 Year")),
    )
    formtype = models.PositiveSmallIntegerField(
        verbose_name=_("form type"),
        choices=FORMTYPE_CHOICES,
        default=FORMTYPE_INITIAL)
    organ = models.ForeignKey(Organ)
    start_date = models.DateField(verbose_name=_("start date"), default=timezone.now)

    FAILURE_OTHER = 10
    FAILURE_CHOICES = (
        (1, _("Immunological")),
        (2, _("Preservation")),
        (3, _("Technical - artery")),
        (4, _("Technical - venous")),
        (5, _("Infection - bacterial")),
        (6, _("Infection - viral")),
        (FAILURE_OTHER, _("Other")),
    )
    graft_failure = models.NullBooleanField(verbose_name=_("graft failure"), null=True, blank=True)
    graft_failure_type = models.PositiveSmallIntegerField(
        verbose_name=_("graft failure"),
        choices=FAILURE_CHOICES,
        null=True,
        blank=True)
    graft_failure_type_other = models.CharField(
        verbose_name=_("Other failure type"),
        max_length=200,
        blank=True)
    graft_failure_date = models.DateField(verbose_name=_("date of graft failure"), null=True, blank=True)
    graft_removal = models.NullBooleanField(verbose_name=_("graft removal"), null=True, blank=True)
    graft_removal_date = models.DateField(verbose_name=_("date of graft removal"), null=True, blank=True)

    UNIT_MGDL = 1
    UNIT_UMOLL = 2
    UNIT_MMOLL = 3
    UNIT_CHOICES = (
        (UNIT_MGDL, "mg/dl"),
        (UNIT_UMOLL, "umol/L"),
        (UNIT_MMOLL, "mmol/L")
    )
    creatinine_1 = models.FloatField(
        verbose_name=_('creatinine 1'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    creatinine_1_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)
    creatinine_2 = models.FloatField(
        verbose_name=_('creatinine 2'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    creatinine_2_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)
    creatinine_3 = models.FloatField(
        verbose_name=_('creatinine 3'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    creatinine_3_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)
    creatinine_4 = models.FloatField(
        verbose_name=_('creatinine 4'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    creatinine_4_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)
    creatinine_5 = models.FloatField(
        verbose_name=_('creatinine 5'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    creatinine_5_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)
    creatinine_6 = models.FloatField(
        verbose_name=_('creatinine 6'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    creatinine_6_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)
    creatinine_7 = models.FloatField(
        verbose_name=_('creatinine 7'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    creatinine_7_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)

    dialysis_requirement_1 = models.DateField(verbose_name=_("date of dialysis requirement 1"), null=True, blank=True)
    dialysis_requirement_2 = models.DateField(verbose_name=_("date of dialysis requirement 2"), null=True, blank=True)
    dialysis_requirement_3 = models.DateField(verbose_name=_("date of dialysis requirement 3"), null=True, blank=True)
    dialysis_requirement_4 = models.DateField(verbose_name=_("date of dialysis requirement 4"), null=True, blank=True)
    dialysis_requirement_5 = models.DateField(verbose_name=_("date of dialysis requirement 5"), null=True, blank=True)
    dialysis_requirement_6 = models.DateField(verbose_name=_("date of dialysis requirement 6"), null=True, blank=True)
    dialysis_requirement_7 = models.DateField(verbose_name=_("date of dialysis requirement 7"), null=True, blank=True)

    DIALYSIS_TYPE_CHOICES = (
        (1, _("CAPD")),
        (2, _("Hemodialysis")),
        (3, _("Unknown")),
    )
    dialysis_type = models.PositiveSmallIntegerField(
        verbose_name=_("Dialysis type"),
        choices=DIALYSIS_TYPE_CHOICES,
        null=True,
        blank=True)

    DIALYSIS_CAUSE_CHOICES = (
        (1, _("Delayed graft function")),
        (2, _("Hyperkalemia")),
        (3, _("Fluid overload")),
        (4, _("Other")),
    )
    dialysis_cause = models.PositiveSmallIntegerField(
        verbose_name=_("Dialysis cause"),
        choices=DIALYSIS_CAUSE_CHOICES,
        null=True,
        blank=True)
    dialysis_cause_other = models.CharField(
        verbose_name=_("Other dialysis cause"),
        max_length=200,
        blank=True)

    hla_mismatch_a = models.CharField(verbose_name=_("HLA A"), max_length=10, blank=True)
    hla_mismatch_b = models.CharField(verbose_name=_("HLA B"), max_length=10, blank=True)
    hla_mismatch_dr = models.CharField(verbose_name=_("HLA DR"), max_length=10, blank=True)

    INDUCTION_CHOICES = (
        (1, _("IL 2")),
        (2, _("ATG"))
    )
    induction_therapy = models.PositiveSmallIntegerField(
        verbose_name=_("Induction therapy"),
        choices=INDUCTION_CHOICES,
        null=True,
        blank=True)

    IMMUNOSUPPRESSION_CHOICES = (
        (1, _("Azathioprine")),
        (2, _("Cyclosporin")),
        (3, _("MMF")),
        (4, _("Prednisolone")),
        (5, _("Sirolomus")),
        (6, _("Tacrolimus")),
        (7, _("Other")),
    )
    immunosuppression = models.PositiveSmallIntegerField(
        verbose_name=_("Post tx immunosuppression"),
        choices=IMMUNOSUPPRESSION_CHOICES,
        null=True,
        blank=True)
    immunosuppression_other = models.CharField(
        verbose_name=_("Other immunosuppression"),
        max_length=200,
        blank=True)

    rejection = models.NullBooleanField(verbose_name=_("rejection"), null=True, blank=True)
    rejection_prednisolone = models.NullBooleanField(verbose_name=_("treated with prednisolone"), null=True, blank=True)
    rejection_drug = models.NullBooleanField(verbose_name=_("treated with other drug"), null=True, blank=True)
    rejection_drug_other = models.CharField(
        verbose_name=_("Other rejection drug"),
        max_length=200,
        blank=True)
    rejection_biopsy = models.NullBooleanField(verbose_name=_("biopsy proven"), null=True, blank=True)

    calcineurin = models.NullBooleanField(verbose_name=_("calcineurin inhibitor"), null=True, blank=True)

    discharge_date = models.DateField(verbose_name=_("date of primary post tx discharge"), null=True, blank=True)
    notes = models.TextField(verbose_name=_("general notes"), blank=True)

    class Meta:
        abstract = True


class FollowUpInitial(FollowUpBase):
    pass


class FollowUp3M(FollowUpBase):
    pass


class FollowUp6M(FollowUpBase):
    pass


class FollowUp1Y(FollowUpBase):
    pass
