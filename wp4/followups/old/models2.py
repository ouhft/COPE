#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib.auth.models import User
from django.core.urlresolvers import reverse
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _
from bdateutil import relativedelta
from wp4.compare.models import VersionControlModel, Organ, YES_NO_UNKNOWN_CHOICES
from wp4.health_economics.models import QualityOfLife


class FollowUpBase(VersionControlModel):
    # Form metadata
    start_date = models.DateField(verbose_name=_("FB01 start date"), default=timezone.now)
    completed = models.BooleanField(verbose_name=_("FB21 form completed"), default=False)
    notes = models.TextField(verbose_name=_("FB20 general notes"), blank=True)

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
    # graft_failure = models.NullBooleanField(verbose_name=_("FB02 graft failure"), blank=True)
    graft_failure_date = models.DateField(verbose_name=_("FB05 date of graft failure"), null=True, blank=True)
    graft_failure_type = models.PositiveSmallIntegerField(
        verbose_name=_("FB03 graft failure"),
        choices=FAILURE_CHOICES,
        null=True,
        blank=True
    )
    graft_failure_type_other = models.CharField(
        verbose_name=_("FB04 Other failure type"),
        max_length=200,
        blank=True
    )
    # replace this with a property that looks at graft_removal_date: No unless date set.
    # graft_removal = models.NullBooleanField(verbose_name=_("FB06 graft removal"), blank=True)
    graft_removal_date = models.DateField(verbose_name=_("FB07 date of graft removal"), null=True, blank=True)

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

    class Meta:
        abstract = True

    @property
    def recipient_alive(self):
        return True if self.organ.recipient.person.date_of_death is None else False

    @property
    def graft_failure(self):
        return True if self.graft_failure_date is not None else False

    @property
    def graft_removal(self):
        return True if self.graft_failure_date is not None else False

    class FollowUpBase(VersionControlModel):
        # Form metadata
        start_date = models.DateField(verbose_name=_("FB01 start date"), default=timezone.now)
        completed = models.BooleanField(verbose_name=_("FB21 form completed"), default=False)

        # Is there a live recipient to follow up on?
        # See property is_alive for the yes no display, the question exists alone in the form
        # For date of death, refer to organ.recipient.person.date_of_death
        on_dialysis_at_death = models.NullBooleanField(
            verbose_name=_("FB30 on dialysis at death"),
            blank=True
        )
        # Last creatinine is already captured as self.serum_creatinine_1_unit
        # the eGFR and mGFR are calculations producing results from data we already collect

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
        # graft_failure = models.NullBooleanField(verbose_name=_("FB02 graft failure"), blank=True)
        graft_failure_date = models.DateField(verbose_name=_("FB05 date of graft failure"), null=True, blank=True)
        graft_failure_type = models.PositiveSmallIntegerField(
            verbose_name=_("FB03 graft failure"),
            choices=FAILURE_CHOICES,
            null=True,
            blank=True
        )
        graft_failure_type_other = models.CharField(
            verbose_name=_("FB04 Other failure type"),
            max_length=200,
            blank=True
        )
        # replace this with a property that looks at graft_removal_date: No unless date set.
        # graft_removal = models.NullBooleanField(verbose_name=_("FB06 graft removal"), blank=True)
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
        last_dialysis_at = models.DateField(
            verbose_name=_("FB10 date of last dialysis"),
            null=True, blank=True
        )

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

        @property
        def recipient_alive(self):
            return True if self.organ.recipient.person.date_of_death is None else False

        @property
        def graft_failure(self):
            return True if self.graft_failure_date is not None else False

        @property
        def graft_removal(self):
            return True if self.graft_failure_date is not None else False

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

        dialysis_requirement_1 = models.NullBooleanField(
            verbose_name=_("FI09 dialysis on day 1"),
            blank=True
        )
        dialysis_requirement_2 = models.NullBooleanField(
            verbose_name=_("FI10 dialysis on day 2"),
            blank=True
        )
        dialysis_requirement_3 = models.NullBooleanField(
            verbose_name=_("FI11 dialysis on day 3"),
            blank=True
        )
        dialysis_requirement_4 = models.NullBooleanField(
            verbose_name=_("FI12 dialysis on day 4"),
            blank=True
        )
        dialysis_requirement_5 = models.NullBooleanField(
            verbose_name=_("FI13 dialysis on day 5"),
            blank=True
        )
        dialysis_requirement_6 = models.NullBooleanField(
            verbose_name=_("FI14 dialysis on day 6"),
            blank=True
        )
        dialysis_requirement_7 = models.NullBooleanField(
            verbose_name=_("FI15 dialysis on day 7"),
            blank=True
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
            (2, _("FIc11 ATG")),
            (3, _("FIc12 None"))
        )
        induction_therapy = models.PositiveSmallIntegerField(
            verbose_name=_("FI25 Induction therapy"),
            choices=INDUCTION_CHOICES,
            null=True,
            blank=True
        )

        discharge_date = models.DateField(verbose_name=_("FI26 date of primary post tx discharge"), null=True,
                                          blank=True)

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

        def _set_common_for_each_day(
                self, on_dialysis_at_death=None, graft_failure_type=None,
                graft_failure_type_other=None, dialysis_type=None, dialysis_cause=None,
                dialysis_cause_other=None
        ):
            self.on_dialysis_at_death = on_dialysis_at_death
            self.graft_failure_type = graft_failure_type
            self.graft_failure_type_other = graft_failure_type_other
            self.dialysis_type = dialysis_type
            self.dialysis_cause = dialysis_cause
            self.dialysis_cause_other = dialysis_cause_other

        def day1(self, recipient_alive=None, on_dialysis_at_death=None, graft_failure=None,
                 graft_failure_type=None, graft_failure_type_other=None, graft_removal=None,
                 dialysis_required=None, dialysis_type=None, dialysis_cause=None,
                 dialysis_cause_other=None, serum_creatinine=None, serum_creatinine_unit=None):
            """
            Map the formset form values to the corresponding model field, and return a dictionary of
            this day's values to populate the formset with

            :param recipient_alive: Yes/No - If no, saves the date as the recipients date_of_death
            :param on_dialysis_at_death: Maps to the single field of same name
            :param graft_failure: Yes/No - If yes, saves the date into graft_failure_date
            :param graft_failure_type: Maps to the single field of same name
            :param graft_failure_type_other: Maps to the single field of same name
            :param graft_removal: Yes/No - If yes, saves the date into graft_removal_date
            :param dialysis_required: Yes/No, maps to the relevant dialysis_requirement_X field
            :param dialysis_type:  Maps to the single field of same name
            :param dialysis_cause:  Maps to the single field of same name
            :param dialysis_cause_other:  Maps to the single field of same name
            :param serum_creatinine: Maps to the relevant serum_creatinine_X field
            :param serum_creatinine_unit: Maps to the relevant serum_creatinine_X_unit field
            :return: a dictionary of this day's values to populate the formset intial data with
            """
            if recipient_alive is False:
                self.organ.recipient.person.date_of_death = self.start_date
            elif recipient_alive is True:
                self.organ.recipient.person.date_of_death = None

            if graft_failure is True:
                self.graft_failure_date = self.start_date
            elif graft_failure is False:
                self.graft_failure_date = None

            if graft_removal is True:
                self.graft_removal_date = self.start_date
            elif graft_removal is False:
                self.graft_removal_date = None

            if dialysis_required is not None:
                self.dialysis_requirement_1 = dialysis_required

            if serum_creatinine is not None:
                self.serum_creatinine_1 = serum_creatinine
                self.serum_creatinine_1_unit = serum_creatinine_unit

            self._set_common_for_each_day(
                on_dialysis_at_death=on_dialysis_at_death,
                graft_failure_type=graft_failure_type,
                graft_failure_type_other=graft_failure_type_other,
                dialysis_type=dialysis_type,
                dialysis_cause=dialysis_cause,
                dialysis_cause_other=dialysis_cause_other
            )

            return {
                'recipient_alive': self.organ.recipient.person.is_alive,
                'on_dialysis_at_death': self.on_dialysis_at_death,
                'graft_failure': self.graft_failure,
                'graft_failure_type': self.graft_failure_type,
                'graft_failure_type_other': self.graft_failure_type_other,
                'graft_removal': self.graft_removal,
                'dialysis_required': self.dialysis_requirement_1,
                'dialysis_type': self.dialysis_type,
                'dialysis_cause': self.dialysis_cause,
                'dialysis_cause_other': self.dialysis_cause_other,
                'serum_creatinine': self.serum_creatinine_1,
                'serum_creatinine_unit': self.serum_creatinine_1_unit
            }

        def day2(self, recipient_alive=None, on_dialysis_at_death=None, graft_failure=None,
                 graft_failure_type=None, graft_failure_type_other=None, graft_removal=None,
                 dialysis_required=None, dialysis_type=None, dialysis_cause=None,
                 dialysis_cause_other=None, serum_creatinine=None, serum_creatinine_unit=None):
            """
            Map the formset form values to the corresponding model field, and return a dictionary of
            this day's values to populate the formset with

            :param recipient_alive: Yes/No - If no, saves the date as the recipients date_of_death
            :param on_dialysis_at_death: Maps to the single field of same name
            :param graft_failure: Yes/No - If yes, saves the date into graft_failure_date
            :param graft_failure_type: Maps to the single field of same name
            :param graft_failure_type_other: Maps to the single field of same name
            :param graft_removal: Yes/No - If yes, saves the date into graft_removal_date
            :param dialysis_required: Yes/No, maps to the relevant dialysis_requirement_X field
            :param dialysis_type:  Maps to the single field of same name
            :param dialysis_cause:  Maps to the single field of same name
            :param dialysis_cause_other:  Maps to the single field of same name
            :param serum_creatinine: Maps to the relevant serum_creatinine_X field
            :param serum_creatinine_unit: Maps to the relevant serum_creatinine_X_unit field
            :return: a dictionary of this day's values to populate the formset intial data with
            """
            date2 = self.start_date + relativedelta(days=+1)
            if recipient_alive is False:
                self.organ.recipient.person.date_of_death = date2
            elif recipient_alive is True:
                self.organ.recipient.person.date_of_death = None

            if graft_failure is True:
                self.graft_failure_date = date2
            elif graft_failure is False:
                self.graft_failure_date = None

            if graft_removal is True:
                self.graft_removal_date = date2
            elif graft_removal is False:
                self.graft_removal_date = None

            if dialysis_required is not None:
                self.dialysis_requirement_2 = dialysis_required

            if serum_creatinine is not None:
                self.serum_creatinine_2 = serum_creatinine
                self.serum_creatinine_2_unit = serum_creatinine_unit

            self._set_common_for_each_day(
                on_dialysis_at_death=on_dialysis_at_death,
                graft_failure_type=graft_failure_type,
                graft_failure_type_other=graft_failure_type_other,
                dialysis_type=dialysis_type,
                dialysis_cause=dialysis_cause,
                dialysis_cause_other=dialysis_cause_other
            )

            return {
                'recipient_alive': self.organ.recipient.person.is_alive,
                'on_dialysis_at_death': self.on_dialysis_at_death,
                'graft_failure': self.graft_failure,
                'graft_failure_type': self.graft_failure_type,
                'graft_failure_type_other': self.graft_failure_type_other,
                'graft_removal': self.graft_removal,
                'dialysis_required': self.dialysis_requirement_2,
                'dialysis_type': self.dialysis_type,
                'dialysis_cause': self.dialysis_cause,
                'dialysis_cause_other': self.dialysis_cause_other,
                'serum_creatinine': self.serum_creatinine_2,
                'serum_creatinine_unit': self.serum_creatinine_2_unit
            }

        def day3(self, recipient_alive=None, on_dialysis_at_death=None, graft_failure=None,
                 graft_failure_type=None, graft_failure_type_other=None, graft_removal=None,
                 dialysis_required=None, dialysis_type=None, dialysis_cause=None,
                 dialysis_cause_other=None, serum_creatinine=None, serum_creatinine_unit=None):
            """
            Map the formset form values to the corresponding model field, and return a dictionary of
            this day's values to populate the formset with

            :param recipient_alive: Yes/No - If no, saves the date as the recipients date_of_death
            :param on_dialysis_at_death: Maps to the single field of same name
            :param graft_failure: Yes/No - If yes, saves the date into graft_failure_date
            :param graft_failure_type: Maps to the single field of same name
            :param graft_failure_type_other: Maps to the single field of same name
            :param graft_removal: Yes/No - If yes, saves the date into graft_removal_date
            :param dialysis_required: Yes/No, maps to the relevant dialysis_requirement_X field
            :param dialysis_type:  Maps to the single field of same name
            :param dialysis_cause:  Maps to the single field of same name
            :param dialysis_cause_other:  Maps to the single field of same name
            :param serum_creatinine: Maps to the relevant serum_creatinine_X field
            :param serum_creatinine_unit: Maps to the relevant serum_creatinine_X_unit field
            :return: a dictionary of this day's values to populate the formset intial data with
            """
            date3 = self.start_date + relativedelta(days=+2)
            if recipient_alive is False:
                self.organ.recipient.person.date_of_death = date3
            elif recipient_alive is True:
                self.organ.recipient.person.date_of_death = None

            if graft_failure is True:
                self.graft_failure_date = date3
            elif graft_failure is False:
                self.graft_failure_date = None

            if graft_removal is True:
                self.graft_removal_date = date3
            elif graft_removal is False:
                self.graft_removal_date = None

            if dialysis_required is not None:
                self.dialysis_requirement_3 = dialysis_required

            if serum_creatinine is not None:
                self.serum_creatinine_3 = serum_creatinine
                self.serum_creatinine_3_unit = serum_creatinine_unit

            self._set_common_for_each_day(
                on_dialysis_at_death=on_dialysis_at_death,
                graft_failure_type=graft_failure_type,
                graft_failure_type_other=graft_failure_type_other,
                dialysis_type=dialysis_type,
                dialysis_cause=dialysis_cause,
                dialysis_cause_other=dialysis_cause_other
            )

            return {
                'recipient_alive': self.organ.recipient.person.is_alive,
                'on_dialysis_at_death': self.on_dialysis_at_death,
                'graft_failure': self.graft_failure,
                'graft_failure_type': self.graft_failure_type,
                'graft_failure_type_other': self.graft_failure_type_other,
                'graft_removal': self.graft_removal,
                'dialysis_required': self.dialysis_requirement_3,
                'dialysis_type': self.dialysis_type,
                'dialysis_cause': self.dialysis_cause,
                'dialysis_cause_other': self.dialysis_cause_other,
                'serum_creatinine': self.serum_creatinine_3,
                'serum_creatinine_unit': self.serum_creatinine_3_unit
            }

        def day4(self, recipient_alive=None, on_dialysis_at_death=None, graft_failure=None,
                 graft_failure_type=None, graft_failure_type_other=None, graft_removal=None,
                 dialysis_required=None, dialysis_type=None, dialysis_cause=None,
                 dialysis_cause_other=None, serum_creatinine=None, serum_creatinine_unit=None):
            """
            Map the formset form values to the corresponding model field, and return a dictionary of
            this day's values to populate the formset with

            :param recipient_alive: Yes/No - If no, saves the date as the recipients date_of_death
            :param on_dialysis_at_death: Maps to the single field of same name
            :param graft_failure: Yes/No - If yes, saves the date into graft_failure_date
            :param graft_failure_type: Maps to the single field of same name
            :param graft_failure_type_other: Maps to the single field of same name
            :param graft_removal: Yes/No - If yes, saves the date into graft_removal_date
            :param dialysis_required: Yes/No, maps to the relevant dialysis_requirement_X field
            :param dialysis_type:  Maps to the single field of same name
            :param dialysis_cause:  Maps to the single field of same name
            :param dialysis_cause_other:  Maps to the single field of same name
            :param serum_creatinine: Maps to the relevant serum_creatinine_X field
            :param serum_creatinine_unit: Maps to the relevant serum_creatinine_X_unit field
            :return: a dictionary of this day's values to populate the formset intial data with
            """
            date4 = self.start_date + relativedelta(days=+3)
            if recipient_alive is False:
                self.organ.recipient.person.date_of_death = date4
            elif recipient_alive is True:
                self.organ.recipient.person.date_of_death = None

            if graft_failure is True:
                self.graft_failure_date = date4
            elif graft_failure is False:
                self.graft_failure_date = None

            if graft_removal is True:
                self.graft_removal_date = date4
            elif graft_removal is False:
                self.graft_removal_date = None

            if dialysis_required is not None:
                self.dialysis_requirement_4 = dialysis_required

            if serum_creatinine is not None:
                self.serum_creatinine_4 = serum_creatinine
                self.serum_creatinine_4_unit = serum_creatinine_unit

            self._set_common_for_each_day(
                on_dialysis_at_death=on_dialysis_at_death,
                graft_failure_type=graft_failure_type,
                graft_failure_type_other=graft_failure_type_other,
                dialysis_type=dialysis_type,
                dialysis_cause=dialysis_cause,
                dialysis_cause_other=dialysis_cause_other
            )

            return {
                'recipient_alive': self.organ.recipient.person.is_alive,
                'on_dialysis_at_death': self.on_dialysis_at_death,
                'graft_failure': self.graft_failure,
                'graft_failure_type': self.graft_failure_type,
                'graft_failure_type_other': self.graft_failure_type_other,
                'graft_removal': self.graft_removal,
                'dialysis_required': self.dialysis_requirement_4,
                'dialysis_type': self.dialysis_type,
                'dialysis_cause': self.dialysis_cause,
                'dialysis_cause_other': self.dialysis_cause_other,
                'serum_creatinine': self.serum_creatinine_4,
                'serum_creatinine_unit': self.serum_creatinine_4_unit
            }

        def day5(self, recipient_alive=None, on_dialysis_at_death=None, graft_failure=None,
                 graft_failure_type=None, graft_failure_type_other=None, graft_removal=None,
                 dialysis_required=None, dialysis_type=None, dialysis_cause=None,
                 dialysis_cause_other=None, serum_creatinine=None, serum_creatinine_unit=None):
            """
            Map the formset form values to the corresponding model field, and return a dictionary of
            this day's values to populate the formset with

            :param recipient_alive: Yes/No - If no, saves the date as the recipients date_of_death
            :param on_dialysis_at_death: Maps to the single field of same name
            :param graft_failure: Yes/No - If yes, saves the date into graft_failure_date
            :param graft_failure_type: Maps to the single field of same name
            :param graft_failure_type_other: Maps to the single field of same name
            :param graft_removal: Yes/No - If yes, saves the date into graft_removal_date
            :param dialysis_required: Yes/No, maps to the relevant dialysis_requirement_X field
            :param dialysis_type:  Maps to the single field of same name
            :param dialysis_cause:  Maps to the single field of same name
            :param dialysis_cause_other:  Maps to the single field of same name
            :param serum_creatinine: Maps to the relevant serum_creatinine_X field
            :param serum_creatinine_unit: Maps to the relevant serum_creatinine_X_unit field
            :return: a dictionary of this day's values to populate the formset intial data with
            """
            date5 = self.start_date + relativedelta(days=+4)
            if recipient_alive is False:
                self.organ.recipient.person.date_of_death = date5
            elif recipient_alive is True:
                self.organ.recipient.person.date_of_death = None

            if graft_failure is True:
                self.graft_failure_date = date5
            elif graft_failure is False:
                self.graft_failure_date = None

            if graft_removal is True:
                self.graft_removal_date = date5
            elif graft_removal is False:
                self.graft_removal_date = None

            if dialysis_required is not None:
                self.dialysis_requirement_5 = dialysis_required

            if serum_creatinine is not None:
                self.serum_creatinine_5 = serum_creatinine
                self.serum_creatinine_5_unit = serum_creatinine_unit

            self._set_common_for_each_day(
                on_dialysis_at_death=on_dialysis_at_death,
                graft_failure_type=graft_failure_type,
                graft_failure_type_other=graft_failure_type_other,
                dialysis_type=dialysis_type,
                dialysis_cause=dialysis_cause,
                dialysis_cause_other=dialysis_cause_other
            )

            return {
                'recipient_alive': self.organ.recipient.person.is_alive,
                'on_dialysis_at_death': self.on_dialysis_at_death,
                'graft_failure': self.graft_failure,
                'graft_failure_type': self.graft_failure_type,
                'graft_failure_type_other': self.graft_failure_type_other,
                'graft_removal': self.graft_removal,
                'dialysis_required': self.dialysis_requirement_5,
                'dialysis_type': self.dialysis_type,
                'dialysis_cause': self.dialysis_cause,
                'dialysis_cause_other': self.dialysis_cause_other,
                'serum_creatinine': self.serum_creatinine_5,
                'serum_creatinine_unit': self.serum_creatinine_5_unit
            }

        def day6(self, recipient_alive=None, on_dialysis_at_death=None, graft_failure=None,
                 graft_failure_type=None, graft_failure_type_other=None, graft_removal=None,
                 dialysis_required=None, dialysis_type=None, dialysis_cause=None,
                 dialysis_cause_other=None, serum_creatinine=None, serum_creatinine_unit=None):
            """
            Map the formset form values to the corresponding model field, and return a dictionary of
            this day's values to populate the formset with

            :param recipient_alive: Yes/No - If no, saves the date as the recipients date_of_death
            :param on_dialysis_at_death: Maps to the single field of same name
            :param graft_failure: Yes/No - If yes, saves the date into graft_failure_date
            :param graft_failure_type: Maps to the single field of same name
            :param graft_failure_type_other: Maps to the single field of same name
            :param graft_removal: Yes/No - If yes, saves the date into graft_removal_date
            :param dialysis_required: Yes/No, maps to the relevant dialysis_requirement_X field
            :param dialysis_type:  Maps to the single field of same name
            :param dialysis_cause:  Maps to the single field of same name
            :param dialysis_cause_other:  Maps to the single field of same name
            :param serum_creatinine: Maps to the relevant serum_creatinine_X field
            :param serum_creatinine_unit: Maps to the relevant serum_creatinine_X_unit field
            :return: a dictionary of this day's values to populate the formset intial data with
            """
            date6 = self.start_date + relativedelta(days=+5)
            if recipient_alive is False:
                self.organ.recipient.person.date_of_death = date6
            elif recipient_alive is True:
                self.organ.recipient.person.date_of_death = None

            if graft_failure is True:
                self.graft_failure_date = date6
            elif graft_failure is False:
                self.graft_failure_date = None

            if graft_removal is True:
                self.graft_removal_date = date6
            elif graft_removal is False:
                self.graft_removal_date = None

            if dialysis_required is not None:
                self.dialysis_requirement_6 = dialysis_required

            if serum_creatinine is not None:
                self.serum_creatinine_6 = serum_creatinine
                self.serum_creatinine_6_unit = serum_creatinine_unit

            self._set_common_for_each_day(
                on_dialysis_at_death=on_dialysis_at_death,
                graft_failure_type=graft_failure_type,
                graft_failure_type_other=graft_failure_type_other,
                dialysis_type=dialysis_type,
                dialysis_cause=dialysis_cause,
                dialysis_cause_other=dialysis_cause_other
            )

            return {
                'recipient_alive': self.organ.recipient.person.is_alive,
                'on_dialysis_at_death': self.on_dialysis_at_death,
                'graft_failure': self.graft_failure,
                'graft_failure_type': self.graft_failure_type,
                'graft_failure_type_other': self.graft_failure_type_other,
                'graft_removal': self.graft_removal,
                'dialysis_required': self.dialysis_requirement_6,
                'dialysis_type': self.dialysis_type,
                'dialysis_cause': self.dialysis_cause,
                'dialysis_cause_other': self.dialysis_cause_other,
                'serum_creatinine': self.serum_creatinine_6,
                'serum_creatinine_unit': self.serum_creatinine_6_unit
            }

        def day7(self, recipient_alive=None, on_dialysis_at_death=None, graft_failure=None,
                 graft_failure_type=None, graft_failure_type_other=None, graft_removal=None,
                 dialysis_required=None, dialysis_type=None, dialysis_cause=None,
                 dialysis_cause_other=None, serum_creatinine=None, serum_creatinine_unit=None):
            """
            Map the formset form values to the corresponding model field, and return a dictionary of
            this day's values to populate the formset with

            :param recipient_alive: Yes/No - If no, saves the date as the recipients date_of_death
            :param on_dialysis_at_death: Maps to the single field of same name
            :param graft_failure: Yes/No - If yes, saves the date into graft_failure_date
            :param graft_failure_type: Maps to the single field of same name
            :param graft_failure_type_other: Maps to the single field of same name
            :param graft_removal: Yes/No - If yes, saves the date into graft_removal_date
            :param dialysis_required: Yes/No, maps to the relevant dialysis_requirement_X field
            :param dialysis_type:  Maps to the single field of same name
            :param dialysis_cause:  Maps to the single field of same name
            :param dialysis_cause_other:  Maps to the single field of same name
            :param serum_creatinine: Maps to the relevant serum_creatinine_X field
            :param serum_creatinine_unit: Maps to the relevant serum_creatinine_X_unit field
            :return: a dictionary of this day's values to populate the formset intial data with
            """
            date7 = self.start_date + relativedelta(days=+6)
            if recipient_alive is False:
                self.organ.recipient.person.date_of_death = date7
            elif recipient_alive is True:
                self.organ.recipient.person.date_of_death = None

            if graft_failure is True:
                self.graft_failure_date = date7
            elif graft_failure is False:
                self.graft_failure_date = None

            if graft_removal is True:
                self.graft_removal_date = date7
            elif graft_removal is False:
                self.graft_removal_date = None

            if dialysis_required is not None:
                self.dialysis_requirement_7 = dialysis_required

            if serum_creatinine is not None:
                self.serum_creatinine_7 = serum_creatinine
                self.serum_creatinine_7_unit = serum_creatinine_unit

            self._set_common_for_each_day(
                on_dialysis_at_death=on_dialysis_at_death,
                graft_failure_type=graft_failure_type,
                graft_failure_type_other=graft_failure_type_other,
                dialysis_type=dialysis_type,
                dialysis_cause=dialysis_cause,
                dialysis_cause_other=dialysis_cause_other
            )

            return {
                'recipient_alive': self.organ.recipient.person.is_alive,
                'on_dialysis_at_death': self.on_dialysis_at_death,
                'graft_failure': self.graft_failure,
                'graft_failure_type': self.graft_failure_type,
                'graft_failure_type_other': self.graft_failure_type_other,
                'graft_removal': self.graft_removal,
                'dialysis_required': self.dialysis_requirement_7,
                'dialysis_type': self.dialysis_type,
                'dialysis_cause': self.dialysis_cause,
                'dialysis_cause_other': self.dialysis_cause_other,
                'serum_creatinine': self.serum_creatinine_7,
                'serum_creatinine_unit': self.serum_creatinine_7_unit
            }

    class FollowUp3M(FollowUpBase):
        organ = models.OneToOneField(Organ, related_name="followup_3m")

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
        # QoL is in health_economics app
        quality_of_life = models.ForeignKey(QualityOfLife, verbose_name=_("F307 quality of life"))

        class Meta:
            verbose_name = _("F3m1 3 Month FollowUp")
            verbose_name_plural = _("F3m2 3 Month FollowUps")

        def __unicode__(self):
            return '%s (%s)' % (self.trial_id, self.start_date)

        def get_absolute_url(self):
            return reverse('wp4:followup:month3_detail', kwargs={'pk': self.pk})

        @property
        def trial_id(self):
            return self.organ.trial_id

    class FollowUp6M(FollowUpBase):
        organ = models.OneToOneField(Organ, related_name="followup_6m")

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
        rejection_periods = models.PositiveSmallIntegerField(verbose_name=_("FY05 rejection periods"), null=True,
                                                             blank=True)

        graft_complications = models.TextField(verbose_name=_("FY06 graft function complications"), blank=True)

        # QoL is in health_economics app
        quality_of_life = models.ForeignKey(QualityOfLife, verbose_name=_("FY07 quality of life"))

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

        def clean(self):
            pass
