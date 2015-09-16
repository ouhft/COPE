#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib.auth.models import User
from django.core.urlresolvers import reverse, reverse_lazy
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _, ungettext_lazy as __

from ..compare.models import Organ, YES_NO_UNKNOWN_CHOICES


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
    # FORMTYPE_INITIAL = 1
    # FORMTYPE_3M = 2
    # FORMTYPE_6M = 3
    # FORMTYPE_1Y = 4
    # FORMTYPE_CHOICES = (
    #     (FORMTYPE_INITIAL, _("Follow Up Days 1-7")),
    #     (FORMTYPE_3M, _("Follow Up 3 Months")),
    #     (FORMTYPE_6M, _("Follow Up 6 Months")),
    #     (FORMTYPE_1Y, _("Follow Up 1 Year")),
    # )
    # formtype = models.PositiveSmallIntegerField(
    #     verbose_name=_("form type"),
    #     choices=FORMTYPE_CHOICES,
    #     default=FORMTYPE_INITIAL)
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
    serum_creatinine_1 = models.FloatField(
        verbose_name=_('creatinine 1'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_1_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)
    # serum_creatinine * 7
    # urine_creatinine
    # creatinine_clearance
    dialysis_requirement_1 = models.DateField(verbose_name=_("date of dialysis requirement 1"), null=True, blank=True)

    DIALYSIS_TYPE_CHOICES = (
        (1, _("CAPD")),
        (2, _("Hemodialysis")),
        (3, _("Unknown")),
    )
    # currently_on_dialysis
    dialysis_type = models.PositiveSmallIntegerField(
        verbose_name=_("Dialysis type"),
        choices=DIALYSIS_TYPE_CHOICES,
        null=True,
        blank=True)

    # dialysis_cause
    # number_of_dialysis
    # HLA mismatches
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
    # rejection_periods
    rejection_prednisolone = models.NullBooleanField(verbose_name=_("treated with prednisolone"), null=True, blank=True)
    rejection_drug = models.NullBooleanField(verbose_name=_("treated with other drug"), null=True, blank=True)
    rejection_drug_other = models.CharField(
        verbose_name=_("Other rejection drug"),
        max_length=200,
        blank=True)
    rejection_biopsy = models.NullBooleanField(verbose_name=_("biopsy proven"), null=True, blank=True)

    calcineurin = models.NullBooleanField(verbose_name=_("calcineurin inhibitor"), null=True, blank=True)
    # graft_complications
    # quality of life Qs
    # discharge_date
    notes = models.TextField(verbose_name=_("general notes"), blank=True)

    class Meta:
        abstract = True


class FollowUpInitial(FollowUpBase):
    organ = models.OneToOneField(Organ, related_name="followup_initial")
    serum_creatinine_2 = models.FloatField(
        verbose_name=_('creatinine 2'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_2_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL)
    serum_creatinine_3 = models.FloatField(
        verbose_name=_('creatinine 3'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_3_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL)
    serum_creatinine_4 = models.FloatField(
        verbose_name=_('creatinine 4'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_4_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL)
    serum_creatinine_5 = models.FloatField(
        verbose_name=_('creatinine 5'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_5_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL)
    serum_creatinine_6 = models.FloatField(
        verbose_name=_('creatinine 6'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_6_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL)
    serum_creatinine_7 = models.FloatField(
        verbose_name=_('creatinine 7'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_7_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL)

    dialysis_requirement_2 = models.DateField(verbose_name=_("date of dialysis requirement 2"), null=True, blank=True)
    dialysis_requirement_3 = models.DateField(verbose_name=_("date of dialysis requirement 3"), null=True, blank=True)
    dialysis_requirement_4 = models.DateField(verbose_name=_("date of dialysis requirement 4"), null=True, blank=True)
    dialysis_requirement_5 = models.DateField(verbose_name=_("date of dialysis requirement 5"), null=True, blank=True)
    dialysis_requirement_6 = models.DateField(verbose_name=_("date of dialysis requirement 6"), null=True, blank=True)
    dialysis_requirement_7 = models.DateField(verbose_name=_("date of dialysis requirement 7"), null=True, blank=True)

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

    discharge_date = models.DateField(verbose_name=_("date of primary post tx discharge"), null=True, blank=True)

    def __unicode__(self):
        return '%s (%s)' % (self.trial_id(), self.start_date)

    def get_absolute_url(self):
        return reverse('followup:initial_detail', kwargs={'pk': self.pk})

    def trial_id(self):
        return self.organ.trial_id()
    trial_id.short_description = 'Trial ID'


class FollowUp3M(FollowUpBase):
    organ = models.OneToOneField(Organ, related_name="followup_3m")
    urine_creatinine = models.FloatField(
        verbose_name=_('urine creatinine'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    urine_creatinine_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL)

    creatinine_clearance = models.FloatField(
        verbose_name=_('creatinine clearance'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    currently_on_dialysis = models.PositiveSmallIntegerField(
        verbose_name=_("currently on dialysis"),
        choices=YES_NO_UNKNOWN_CHOICES,
        null=True,
        blank=True)
    number_of_dialysis_sessions = models.PositiveSmallIntegerField(
        verbose_name=_("number of dialysis sessions"),
        null=True,
        blank=True)
    rejection_periods = models.PositiveSmallIntegerField(verbose_name=_("rejection periods"), null=True, blank=True)

    graft_complications = models.TextField(verbose_name=_("graft function complications"), blank=True)

    qol_mobility = models.PositiveSmallIntegerField(
        verbose_name=_("qol mobility score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True)
    qol_selfcare = models.PositiveSmallIntegerField(
        verbose_name=_("qol self care score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True)
    qol_usual_activities = models.PositiveSmallIntegerField(
        verbose_name=_("qol usual activites score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True)
    qol_pain = models.PositiveSmallIntegerField(
        verbose_name=_("qol pain or discomfort score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True)
    qol_anxiety = models.PositiveSmallIntegerField(
        verbose_name=_("qol anxiety or depression score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True)
    vas_score = models.PositiveSmallIntegerField(
        verbose_name=_("vas score"),
        validators=[MinValueValidator(0), MaxValueValidator(100)],
        blank=True, null=True)


class FollowUp6M(FollowUpBase):
    organ = models.OneToOneField(Organ, related_name="followup_6m")
    urine_creatinine = models.FloatField(
        verbose_name=_('urine creatinine'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    urine_creatinine_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL)

    creatinine_clearance = models.FloatField(
        verbose_name=_('creatinine clearance'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )

    currently_on_dialysis = models.PositiveSmallIntegerField(
        verbose_name=_("currently on dialysis"),
        choices=YES_NO_UNKNOWN_CHOICES,
        null=True,
        blank=True)
    number_of_dialysis_sessions = models.PositiveSmallIntegerField(
        verbose_name=_("number of dialysis sessions"),
        null=True,
        blank=True)
    graft_complications = models.TextField(verbose_name=_("graft function complications"), blank=True)


class FollowUp1Y(FollowUpBase):
    organ = models.OneToOneField(Organ, related_name="followup_1y")
    urine_creatinine = models.FloatField(
        verbose_name=_('urine creatinine'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    urine_creatinine_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL)

    creatinine_clearance = models.FloatField(
        verbose_name=_('creatinine clearance'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    currently_on_dialysis = models.PositiveSmallIntegerField(
        verbose_name=_("currently on dialysis"),
        choices=YES_NO_UNKNOWN_CHOICES,
        null=True,
        blank=True)
    number_of_dialysis_sessions = models.PositiveSmallIntegerField(
        verbose_name=_("number of dialysis sessions"),
        null=True,
        blank=True)
    rejection_periods = models.PositiveSmallIntegerField(verbose_name=_("rejection periods"), null=True, blank=True)

    graft_complications = models.TextField(verbose_name=_("graft function complications"), blank=True)

    qol_mobility = models.PositiveSmallIntegerField(
        verbose_name=_("qol mobility score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True)
    qol_selfcare = models.PositiveSmallIntegerField(
        verbose_name=_("qol self care score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True)
    qol_usual_activities = models.PositiveSmallIntegerField(
        verbose_name=_("qol usual activites score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True)
    qol_pain = models.PositiveSmallIntegerField(
        verbose_name=_("qol pain or discomfort score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True)
    qol_anxiety = models.PositiveSmallIntegerField(
        verbose_name=_("qol anxiety or depression score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True)
    vas_score = models.PositiveSmallIntegerField(
        verbose_name=_("vas score"),
        validators=[MinValueValidator(0), MaxValueValidator(100)],
        blank=True, null=True)
