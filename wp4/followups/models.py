#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib.auth.models import User
from django.core.urlresolvers import reverse
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _

from wp4.compare.models import VersionControlModel, Organ, YES_NO_UNKNOWN_CHOICES


class FollowUpBase(VersionControlModel):
    start_date = models.DateField(verbose_name=_("FB01 start date"), default=timezone.now)
    completed = models.BooleanField(verbose_name=_("FB21 form completed"), default=False)

    FAILURE_OTHER = 10
    FAILURE_CHOICES = (
        (1, _("FBc01 Immunological")),
        (2, _("FBc02 Preservation")),
        (3, _("FBc03 Technical - artery")),
        (4, _("FBc04 Technical - venous")),
        (5, _("FBc05 Infection - bacterial")),
        (6, _("FBc06 Infection - viral")),
        (FAILURE_OTHER, _("FBc07 Other")),
    )
    graft_failure = models.NullBooleanField(verbose_name=_("FB02 graft failure"), blank=True)
    graft_failure_date = models.DateField(verbose_name=_("FB05 date of graft failure"), null=True, blank=True)
    graft_failure_type = models.PositiveSmallIntegerField(
        verbose_name=_("FB03 graft failure"),
        choices=FAILURE_CHOICES,
        null=True,
        blank=True)
    graft_failure_type_other = models.CharField(
        verbose_name=_("FB04 Other failure type"),
        max_length=200,
        blank=True)
    graft_removal = models.NullBooleanField(verbose_name=_("FB06 graft removal"), blank=True)
    graft_removal_date = models.DateField(verbose_name=_("FB07 date of graft removal"), null=True, blank=True)

    UNIT_MGDL = 1
    UNIT_UMOLL = 2
    UNIT_MMOLL = 3
    UNIT_CHOICES = (
        (UNIT_MGDL, "mg/dl"),
        (UNIT_UMOLL, "umol/L"),
        (UNIT_MMOLL, "mmol/L")
    )
    serum_creatinine_1 = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FB08 creatinine 1'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_1_unit = models.PositiveSmallIntegerField(choices=UNIT_CHOICES, default=UNIT_MGDL)
    # serum_creatinine * 7
    # urine_creatinine
    # creatinine_clearance
    dialysis_requirement_1 = models.DateField(verbose_name=_("FB10 date of dialysis requirement 1"), null=True, blank=True)

    DIALYSIS_TYPE_CHOICES = (
        (1, _("FBc10 CAPD")),
        (2, _("FBc11 Hemodialysis")),
        (3, _("FBc12 Unknown")),
    )
    # currently_on_dialysis
    dialysis_type = models.PositiveSmallIntegerField(
        verbose_name=_("FB11 Dialysis type"),
        choices=DIALYSIS_TYPE_CHOICES,
        null=True,
        blank=True
    )

    # dialysis_cause
    # number_of_dialysis
    # HLA mismatches
    IMMUNOSUPPRESSION_CHOICES = (
        (1, _("FBc20 Azathioprine")),
        (2, _("FBc21 Cyclosporin")),
        (3, _("FBc22 MMF")),
        (4, _("FBc23 Prednisolone")),
        (5, _("FBc24 Sirolomus")),
        (6, _("FBc25 Tacrolimus")),
        (7, _("FBc26 Other")),
    )
    immunosuppression = models.PositiveSmallIntegerField(
        verbose_name=_("FB12 Post tx immunosuppression"),
        choices=IMMUNOSUPPRESSION_CHOICES,
        null=True,
        blank=True
    )
    immunosuppression_other = models.CharField(
        verbose_name=_("FB13 Other immunosuppression"),
        max_length=200,
        blank=True
    )

    rejection = models.NullBooleanField(verbose_name=_("FB14 rejection"), blank=True)
    # rejection_periods
    rejection_prednisolone = models.NullBooleanField(verbose_name=_("FB15 treated with prednisolone"), blank=True)
    rejection_drug = models.NullBooleanField(verbose_name=_("FB16 treated with other drug"), blank=True)
    rejection_drug_other = models.CharField(
        verbose_name=_("FB17 Other rejection drug"),
        max_length=200,
        blank=True)
    rejection_biopsy = models.NullBooleanField(verbose_name=_("FB18 biopsy proven"), blank=True)

    calcineurin = models.NullBooleanField(verbose_name=_("FB19 calcineurin inhibitor"), blank=True)
    # graft_complications
    # quality of life Qs
    # discharge_date
    notes = models.TextField(verbose_name=_("FB20 general notes"), blank=True)

    class Meta:
        abstract = True


class FollowUpInitial(FollowUpBase):
    organ = models.OneToOneField(Organ, related_name="followup_initial")

    serum_creatinine_2 = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FI01 creatinine 2'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_2_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL
    )
    serum_creatinine_3 = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FI02 creatinine 3'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_3_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL
    )
    serum_creatinine_4 = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FI03 creatinine 4'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_4_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL
    )
    serum_creatinine_5 = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FI04 creatinine 5'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_5_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL
    )
    serum_creatinine_6 = models.FloatField(
        verbose_name=_('FI05 creatinine 6'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_6_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL
    )
    serum_creatinine_7 = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FI06 creatinine 7'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_7_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL
    )

    dialysis_requirement_2 = models.DateField(
        verbose_name=_("FI10 date of dialysis requirement 2"),
        null=True, blank=True
    )
    dialysis_requirement_3 = models.DateField(
        verbose_name=_("FI11 date of dialysis requirement 3"),
        null=True, blank=True
    )
    dialysis_requirement_4 = models.DateField(
        verbose_name=_("FI12 date of dialysis requirement 4"),
        null=True, blank=True
    )
    dialysis_requirement_5 = models.DateField(
        verbose_name=_("FI13 date of dialysis requirement 5"),
        null=True, blank=True
    )
    dialysis_requirement_6 = models.DateField(
        verbose_name=_("FI14 date of dialysis requirement 6"),
        null=True, blank=True
    )
    dialysis_requirement_7 = models.DateField(
        verbose_name=_("FI15 date of dialysis requirement 7"),
        null=True, blank=True
    )

    DIALYSIS_CAUSE_CHOICES = (
        (1, _("FIc01 Delayed graft function")),
        (2, _("FIc02 Hyperkalemia")),
        (3, _("FIc03 Fluid overload")),
        (4, _("FIc04 Other")),
    )
    dialysis_cause = models.PositiveSmallIntegerField(
        verbose_name=_("FI20 Dialysis cause"),
        choices=DIALYSIS_CAUSE_CHOICES,
        null=True,
        blank=True
    )
    dialysis_cause_other = models.CharField(
        verbose_name=_("FI21 Other dialysis cause"),
        max_length=200,
        blank=True
    )

    hla_mismatch_a = models.CharField(verbose_name=_("FI22 HLA A"), max_length=10, blank=True)
    hla_mismatch_b = models.CharField(verbose_name=_("FI23 HLA B"), max_length=10, blank=True)
    hla_mismatch_dr = models.CharField(verbose_name=_("FI24 HLA DR"), max_length=10, blank=True)

    INDUCTION_CHOICES = (
        (1, _("FIc10 IL 2")),
        (2, _("FIc11 ATG"))
    )
    induction_therapy = models.PositiveSmallIntegerField(
        verbose_name=_("FI25 Induction therapy"),
        choices=INDUCTION_CHOICES,
        null=True,
        blank=True
    )

    discharge_date = models.DateField(verbose_name=_("FI26 date of primary post tx discharge"), null=True, blank=True)

    class Meta:
        verbose_name = _("FIm1 Initial FollowUp")
        verbose_name_plural = _("FIm2 Initial FollowUps")

    def __unicode__(self):
        return '%s (%s)' % (self.trial_id, self.start_date)

    def get_absolute_url(self):
        return reverse('wp4:followup:initial_detail', kwargs={'pk': self.pk})

    @property
    def trial_id(self):
        return self.organ.trial_id


class FollowUp3M(FollowUpBase):
    organ = models.OneToOneField(Organ, related_name="followup_3m")

    urine_creatinine = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('F301 urine creatinine'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    urine_creatinine_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL
    )
    creatinine_clearance = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('F302 creatinine clearance'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    currently_on_dialysis = models.PositiveSmallIntegerField(
        verbose_name=_("F303 currently on dialysis"),
        choices=YES_NO_UNKNOWN_CHOICES,
        null=True,
        blank=True
    )
    number_of_dialysis_sessions = models.PositiveSmallIntegerField(
        verbose_name=_("F304 number of dialysis sessions"),
        null=True,
        blank=True
    )
    rejection_periods = models.PositiveSmallIntegerField(
        verbose_name=_("F305 rejection periods"),
        null=True, blank=True
    )

    graft_complications = models.TextField(verbose_name=_("F306 graft function complications"), blank=True)

    qol_mobility = models.PositiveSmallIntegerField(
        verbose_name=_("F307 qol mobility score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True
    )
    qol_selfcare = models.PositiveSmallIntegerField(
        verbose_name=_("F308 qol self care score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True
    )
    qol_usual_activities = models.PositiveSmallIntegerField(
        verbose_name=_("F309 qol usual activites score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True
    )
    qol_pain = models.PositiveSmallIntegerField(
        verbose_name=_("F310 qol pain or discomfort score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True
    )
    qol_anxiety = models.PositiveSmallIntegerField(
        verbose_name=_("F311 qol anxiety or depression score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True
    )
    vas_score = models.PositiveSmallIntegerField(
        verbose_name=_("F312 vas score"),
        validators=[MinValueValidator(0), MaxValueValidator(100)],
        blank=True, null=True
    )

    class Meta:
        verbose_name = _("F3m1 3 Month FollowUp")
        verbose_name_plural = _("F3m2 3 Month FollowUps")

    def __unicode__(self):
        return '%s (%s)' % (self.trial_id, self.start_date)

    def get_absolute_url(self):
        # return reverse('followup:initial_detail', kwargs={'pk': self.pk})
        return ""

    @property
    def trial_id(self):
        return self.organ.trial_id


class FollowUp6M(FollowUpBase):
    organ = models.OneToOneField(Organ, related_name="followup_6m")

    urine_creatinine = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('F601 urine creatinine'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    urine_creatinine_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL
    )
    creatinine_clearance = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('F602 creatinine clearance'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )

    currently_on_dialysis = models.PositiveSmallIntegerField(
        verbose_name=_("F603 currently on dialysis"),
        choices=YES_NO_UNKNOWN_CHOICES,
        null=True,
        blank=True
    )
    rejection_periods = models.PositiveSmallIntegerField(
        verbose_name=_("F606 rejection periods"),
        null=True, blank=True
    )
    number_of_dialysis_sessions = models.PositiveSmallIntegerField(
        verbose_name=_("F604 number of dialysis sessions"),
        null=True,
        blank=True
    )
    graft_complications = models.TextField(verbose_name=_("F605 graft function complications"), blank=True)

    class Meta:
        verbose_name = _("F6m1 6 Month FollowUp")
        verbose_name_plural = _("F6m2 6 Month FollowUps")

    def __unicode__(self):
        return '%s (%s)' % (self.trial_id, self.start_date)

    def get_absolute_url(self):
        # return reverse('followup:initial_detail', kwargs={'pk': self.pk})
        return ""

    @property
    def trial_id(self):
        return self.organ.trial_id


class FollowUp1Y(FollowUpBase):
    organ = models.OneToOneField(Organ, related_name="followup_1y")

    urine_creatinine = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FY01 urine creatinine'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    urine_creatinine_unit = models.PositiveSmallIntegerField(
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL
    )

    creatinine_clearance = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FY02 creatinine clearance'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    currently_on_dialysis = models.PositiveSmallIntegerField(
        verbose_name=_("FY03 currently on dialysis"),
        choices=YES_NO_UNKNOWN_CHOICES,
        null=True,
        blank=True
    )
    number_of_dialysis_sessions = models.PositiveSmallIntegerField(
        verbose_name=_("FY04 number of dialysis sessions"),
        null=True,
        blank=True
    )
    rejection_periods = models.PositiveSmallIntegerField(verbose_name=_("FY05 rejection periods"), null=True, blank=True)

    graft_complications = models.TextField(verbose_name=_("FY06 graft function complications"), blank=True)

    qol_mobility = models.PositiveSmallIntegerField(
        verbose_name=_("FY10 qol mobility score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True
    )
    qol_selfcare = models.PositiveSmallIntegerField(
        verbose_name=_("FY11 qol self care score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True
    )
    qol_usual_activities = models.PositiveSmallIntegerField(
        verbose_name=_("FY12 qol usual activites score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True
    )
    qol_pain = models.PositiveSmallIntegerField(
        verbose_name=_("FY13 qol pain or discomfort score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True)
    qol_anxiety = models.PositiveSmallIntegerField(
        verbose_name=_("FY14 qol anxiety or depression score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True
    )
    vas_score = models.PositiveSmallIntegerField(
        verbose_name=_("FY15 vas score"),
        validators=[MinValueValidator(0), MaxValueValidator(100)],
        blank=True, null=True
    )

    class Meta:
        verbose_name = _("FYm1 1 Year FollowUp")
        verbose_name_plural = _("FYm2 1 Year FollowUps")

    def __unicode__(self):
        return '%s (%s)' % (self.trial_id, self.start_date)

    def get_absolute_url(self):
        # return reverse('followup:initial_detail', kwargs={'pk': self.pk})
        return ""

    @property
    def trial_id(self):
        return self.organ.trial_id
