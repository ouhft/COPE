#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.core.urlresolvers import reverse
from django.core.validators import MinValueValidator
from django.db import models
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _

from wp4.compare.models.core import AuditControlModelBase
from wp4.compare.models import Organ, YES_NO_UNKNOWN_CHOICES
from wp4.health_economics.models import QualityOfLife
from .managers import FollowupModelForUserManager


class FollowUpBase(AuditControlModelBase):
    UNIT_MGDL = 1
    UNIT_UMOLL = 2
    UNIT_MMOLL = 3
    UNIT_CHOICES = (
        (UNIT_MGDL, "mg/dl"),
        (UNIT_UMOLL, "umol/L"),
        (UNIT_MMOLL, "mmol/L")
    )

    # Form metadata
    start_date = models.DateField(verbose_name=_("FB01 start date"), null=True)  # Note, not blank!
    # form_completed = models.BooleanField(verbose_name=_("FB02 form completed"), default=False)
    notes = models.TextField(verbose_name=_("FB03 general notes"), blank=True)

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
    # replace this with a property that looks at graft_failure_date: No unless date set.
    graft_failure = models.NullBooleanField(verbose_name=_("FB10 graft failure"), blank=True)
    graft_failure_date = models.DateField(verbose_name=_("FB11 date of graft failure"), null=True, blank=True)
    graft_failure_type = models.PositiveSmallIntegerField(
        verbose_name=_("FB12 graft failure"),
        choices=FAILURE_CHOICES,
        null=True,
        blank=True
    )
    graft_failure_type_other = models.CharField(
        verbose_name=_("FB13 Other failure type"),
        max_length=200,
        blank=True
    )
    # replace this with a property that looks at graft_removal_date: No unless date set.
    graft_removal = models.NullBooleanField(verbose_name=_("FB14 graft removal"), blank=True)
    graft_removal_date = models.DateField(verbose_name=_("FB15 date of graft removal"), null=True, blank=True)

    DIALYSIS_TYPE_CHOICES = (
        (1, _("FBc10 CAPD")),
        (2, _("FBc11 Hemodialysis")),
        (3, _("FBc12 Unknown")),
    )
    # currently_on_dialysis
    dialysis_type = models.PositiveSmallIntegerField(
        verbose_name=_("FB16 Dialysis type"),
        choices=DIALYSIS_TYPE_CHOICES,
        null=True,
        blank=True
    )

    immunosuppression_1 = models.BooleanField(
        verbose_name=_("FB30 Post tx Azathioprine"),
        default=False
    )
    immunosuppression_2 = models.BooleanField(
        verbose_name=_("FB31 Post tx Cyclosporin"),
        default=False
    )
    immunosuppression_3 = models.BooleanField(
        verbose_name=_("FB32 Post tx MMF"),
        default=False
    )
    immunosuppression_4 = models.BooleanField(
        verbose_name=_("FB33 Post tx Prednisolone"),
        default=False
    )
    immunosuppression_5 = models.BooleanField(
        verbose_name=_("FB34 Post tx Sirolomus"),
        default=False
    )
    immunosuppression_6 = models.BooleanField(
        verbose_name=_("FB35 Post tx Tacrolimus"),
        default=False
    )
    immunosuppression_7 = models.BooleanField(
        verbose_name=_("FB36 Post tx Other"),
        default=False
    )
    immunosuppression_other = models.CharField(
        verbose_name=_("FB37 Other immunosuppression"),
        max_length=200,
        blank=True
    )

    rejection = models.NullBooleanField(verbose_name=_("FB19 rejection"), blank=True)
    # rejection_periods
    rejection_prednisolone = models.NullBooleanField(verbose_name=_("FB20 treated with prednisolone"), blank=True)
    rejection_drug = models.NullBooleanField(verbose_name=_("FB21 treated with other drug"), blank=True)
    rejection_drug_other = models.CharField(
        verbose_name=_("FB22 Other rejection drug"),
        max_length=200,
        blank=True)
    rejection_biopsy = models.NullBooleanField(verbose_name=_("FB23 biopsy proven"), blank=True)

    calcineurin = models.NullBooleanField(verbose_name=_("FB24 calcineurin inhibitor"), blank=True)

    class Meta:
        abstract = True

    @property
    def is_recipient_alive(self):
        return True if self.organ.recipient.person.date_of_death is None else False

    @property
    def has_graft_failed(self):
        return True if self.graft_failure_date is not None else False

    @property
    def was_graft_removed(self):
        return True if self.graft_failure_date is not None else False


class FollowUpInitial(FollowUpBase):
    organ = models.OneToOneField(Organ, related_name="followup_initial", verbose_name=_("FI01 Trial id"))

    serum_creatinine_unit = models.PositiveSmallIntegerField(
        verbose_name=_("FI02 creatinine unit"),
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL
    )
    serum_creatinine_1 = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FI03 creatinine 1'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_2 = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FI04 creatinine 2'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_3 = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FI05 creatinine 3'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_4 = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FI06 creatinine 4'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_5 = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FI07 creatinine 5'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_6 = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FI08 creatinine 6'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    serum_creatinine_7 = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FI09 creatinine 7'),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )

    dialysis_requirement_1 = models.NullBooleanField(
        verbose_name=_("FI10 dialysis on day 1"),
        blank=True
    )
    dialysis_requirement_2 = models.NullBooleanField(
        verbose_name=_("FI11 dialysis on day 2"),
        blank=True
    )
    dialysis_requirement_3 = models.NullBooleanField(
        verbose_name=_("FI12 dialysis on day 3"),
        blank=True
    )
    dialysis_requirement_4 = models.NullBooleanField(
        verbose_name=_("FI13 dialysis on day 4"),
        blank=True
    )
    dialysis_requirement_5 = models.NullBooleanField(
        verbose_name=_("FI14 dialysis on day 5"),
        blank=True
    )
    dialysis_requirement_6 = models.NullBooleanField(
        verbose_name=_("FI15 dialysis on day 6"),
        blank=True
    )
    dialysis_requirement_7 = models.NullBooleanField(
        verbose_name=_("FI16 dialysis on day 7"),
        blank=True
    )

    DIALYSIS_CAUSE_CHOICES = (
        (0, _("FIc00 No dialysis")),
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
        (2, _("FIc11 ATG")),
        (3, _("FIc12 None"))
    )
    induction_therapy = models.PositiveSmallIntegerField(
        verbose_name=_("FI25 Induction therapy"),
        choices=INDUCTION_CHOICES,
        null=True,
        blank=True
    )
    discharge_date = models.DateField(
        verbose_name=_("FI26 date of primary post tx discharge"),
        null=True, blank=True
    )

    objects = FollowupModelForUserManager()

    class Meta:
        verbose_name = _("FIm1 Initial FollowUp")
        verbose_name_plural = _("FIm2 Initial FollowUps")
        permissions = (
            ("view_followupinitial", "Can only view the data"),
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )

    def country_for_restriction(self):
        """
        Get the country to be used for geographic restriction of this data
        :return: Int: Value from list in Locations.Models. Should be in range [1,4,5]
        """
        return self.organ.country_for_restriction

    def location_for_restriction(self):
        """
        Get the location to be used for geographic restriction of this data
        :return: Int: Hospital object id
        """
        return self.organ.location_for_restriction

    def __str__(self):
        return '%s (%s)' % (self.trial_id, self.start_date)

    def get_absolute_url(self):
        return reverse('wp4:followup:initial_detail', kwargs={'pk': self.pk})

    @property
    def trial_id(self):
        return self.organ.trial_id


class FollowUp3M(FollowUpBase):
    organ = models.OneToOneField(Organ, related_name="followup_3m", verbose_name=_("F301 Trial id"))

    serum_creatinine_unit = models.PositiveSmallIntegerField(
        verbose_name=_("F310 creatinine unit"),
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL
    )
    serum_creatinine = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('F311 creatinine '),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    creatinine_clearance = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('F302 creatinine clearance'),
        validators=[MinValueValidator(0.0), ],
        blank=True,
        null=True
    )
    currently_on_dialysis = models.PositiveSmallIntegerField(
        verbose_name=_("F303 currently on dialysis"),
        choices=YES_NO_UNKNOWN_CHOICES,
        null=True,
        blank=True
    )
    dialysis_date = models.DateField(
        verbose_name=_("F304 date of last dialysis"),
        null=True,
        blank=True
    )
    number_of_dialysis_sessions = models.PositiveSmallIntegerField(
        verbose_name=_("F305 number of dialysis sessions"),
        null=True,
        blank=True
    )
    rejection_periods = models.PositiveSmallIntegerField(
        verbose_name=_("F306 rejection periods"),
        null=True,
        blank=True
    )
    graft_complications = models.TextField(verbose_name=_("F307 graft function complications"), blank=True)
    # QoL is in health_economics app
    quality_of_life = models.OneToOneField(
        QualityOfLife,
        related_name="followup_3m",
        verbose_name=_("F308 quality of life"),
        null=True,
        blank=True
    )

    objects = FollowupModelForUserManager()

    class Meta:
        verbose_name = _("F3m1 3 Month FollowUp")
        verbose_name_plural = _("F3m2 3 Month FollowUps")
        permissions = (
            ("view_followup3m", "Can only view the data"),
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )

    def country_for_restriction(self):
        """
        Get the country to be used for geographic restriction of this data
        :return: Int: Value from list in Locations.Models. Should be in range [1,4,5]
        """
        return self.organ.country_for_restriction

    def location_for_restriction(self):
        """
        Get the location to be used for geographic restriction of this data
        :return: Int: Hospital object id
        """
        return self.organ.location_for_restriction

    @property
    def started_within_window(self):
        if self.start_date \
                and self.organ.followup_3m_completed_by >= self.start_date >= self.organ.followup_3m_begin_by:
            return True
        return False

    def __str__(self):
        return '%s (%s)' % (self.trial_id, self.start_date)

    def get_absolute_url(self):
        return reverse('wp4:followup:month3_detail', kwargs={'pk': self.pk})

    @property
    def trial_id(self):
        return self.organ.trial_id


class FollowUp6M(FollowUpBase):
    organ = models.OneToOneField(Organ, related_name="followup_6m", verbose_name=_("F601 Trial id"))

    serum_creatinine_unit = models.PositiveSmallIntegerField(
        verbose_name=_("F610 creatinine unit"),
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL
    )
    serum_creatinine = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('F611 creatinine '),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    creatinine_clearance = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('F602 creatinine clearance'),
        validators=[MinValueValidator(0.0), ],
        blank=True,
        null=True
    )
    currently_on_dialysis = models.PositiveSmallIntegerField(
        verbose_name=_("F603 currently on dialysis"),
        choices=YES_NO_UNKNOWN_CHOICES,
        null=True,
        blank=True
    )
    dialysis_date = models.DateField(
        verbose_name=_("F604 date of last dialysis"),
        null=True,
        blank=True
    )
    number_of_dialysis_sessions = models.PositiveSmallIntegerField(
        verbose_name=_("F605 number of dialysis sessions"),
        null=True,
        blank=True
    )
    rejection_periods = models.PositiveSmallIntegerField(
        verbose_name=_("F606 rejection periods"),
        null=True,
        blank=True
    )
    graft_complications = models.TextField(verbose_name=_("F607 graft function complications"), blank=True)

    objects = FollowupModelForUserManager()

    class Meta:
        verbose_name = _("F6m1 6 Month FollowUp")
        verbose_name_plural = _("F6m2 6 Month FollowUps")
        permissions = (
            ("view_followup6m", "Can only view the data"),
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )

    def country_for_restriction(self):
        """
        Get the country to be used for geographic restriction of this data
        :return: Int: Value from list in Locations.Models. Should be in range [1,4,5]
        """
        return self.organ.country_for_restriction

    def location_for_restriction(self):
        """
        Get the location to be used for geographic restriction of this data
        :return: Int: Hospital object id
        """
        return self.organ.location_for_restriction

    @property
    def started_within_window(self):
        if self.start_date \
                and self.organ.followup_6m_completed_by >= self.start_date >= self.organ.followup_6m_begin_by:
            return True
        return False

    def __str__(self):
        return '%s (%s)' % (self.trial_id, self.start_date)

    def get_absolute_url(self):
        return reverse('wp4:followup:month6_detail', kwargs={'pk': self.pk})

    @property
    def trial_id(self):
        return self.organ.trial_id


class FollowUp1Y(FollowUpBase):
    organ = models.OneToOneField(Organ, related_name="followup_1y", verbose_name=_("FY01 Trial id"))

    serum_creatinine_unit = models.PositiveSmallIntegerField(
        verbose_name=_("FY10 creatinine unit"),
        choices=FollowUpBase.UNIT_CHOICES,
        default=FollowUpBase.UNIT_MGDL
    )
    serum_creatinine = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FY11 creatinine '),
        validators=[MinValueValidator(0.0), ],
        blank=True, null=True
    )
    creatinine_clearance = models.DecimalField(
        max_digits=6,
        decimal_places=2,
        verbose_name=_('FY02 creatinine clearance'),
        validators=[MinValueValidator(0.0), ],
        blank=True,
        null=True
    )
    currently_on_dialysis = models.PositiveSmallIntegerField(
        verbose_name=_("FY03 currently on dialysis"),
        choices=YES_NO_UNKNOWN_CHOICES,
        null=True,
        blank=True
    )
    dialysis_date = models.DateField(
        verbose_name=_("FY04 date of last dialysis"),
        null=True,
        blank=True
    )
    number_of_dialysis_sessions = models.PositiveSmallIntegerField(
        verbose_name=_("FY05 number of dialysis sessions"),
        null=True,
        blank=True
    )
    rejection_periods = models.PositiveSmallIntegerField(
        verbose_name=_("FY06 rejection periods"),
        null=True,
        blank=True
    )
    graft_complications = models.TextField(verbose_name=_("FY07 graft function complications"), blank=True)
    # QoL is in health_economics app
    quality_of_life = models.OneToOneField(
        QualityOfLife,
        related_name="followup_1y",
        verbose_name=_("FY08 quality of life"),
        null=True,
        blank=True
    )

    objects = FollowupModelForUserManager()

    class Meta:
        verbose_name = _("FYm1 1 Year FollowUp")
        verbose_name_plural = _("FYm2 1 Year FollowUps")
        permissions = (
            ("view_followup1y", "Can only view the data"),
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )

    def country_for_restriction(self):
        """
        Get the country to be used for geographic restriction of this data
        :return: Int: Value from list in Locations.Models. Should be in range [1,4,5]
        """
        return self.organ.country_for_restriction

    def location_for_restriction(self):
        """
        Get the location to be used for geographic restriction of this data
        :return: Int: Hospital object id
        """
        return self.organ.location_for_restriction

    @property
    def started_within_window(self):
        if self.start_date \
                and self.organ.followup_final_completed_by >= self.start_date >= self.organ.followup_final_begin_by:
            return True
        return False

    def __str__(self):
        return '%s (%s)' % (self.trial_id, self.start_date)

    def get_absolute_url(self):
        return reverse('wp4:followup:final_detail', kwargs={'pk': self.pk})

    @property
    def trial_id(self):
        return self.organ.trial_id
