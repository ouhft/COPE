#!/usr/bin/python
# coding: utf-8
from django.db import models
from django.utils.translation import ugettext_lazy as _, ungettext_lazy as __

from ..compare.models import VersionControlModel, Organ
from ..staff_person.models import StaffPerson


class ClavienDindoGrading(models.Model):
    label = models.CharField(max_length=10)
    description = models.CharField(max_length=300)


class AlternativeGrading(models.Model):
    label = models.CharField(max_length=10)
    description = models.CharField(max_length=300)


class AdverseEvent(VersionControlModel):
    # From fixtures/gradings.json
    GRADE_I = 1
    GRADE_II = 2
    GRADE_III = 3
    GRADE_III_A = 4
    GRADE_III_B = 5
    GRADE_IV = 6
    GRADE_IV_A = 7
    GRADE_IV_B = 8
    GRADE_V = 9
    GRADE_1 = 1
    GRADE_2 = 2
    GRADE_3 = 3
    GRADE_4 = 4
    GRADE_5 = 5

    # Event basics
    sequence_number = models.PositiveSmallIntegerField(verbose_name=_("AE01 sequence number"), default=0)
    onset_at_date = models.DateField(verbose_name=_("AE02 onset date"))
    resolution_at_date = models.DateField(verbose_name=_("AE03 resolution date"), blank=True, null=True)

    organ = models.ForeignKey(Organ, verbose_name=_("AE04"))
    device_related = models.BooleanField(verbose_name=_("AE05 device related"), default=False)

    description = models.CharField(verbose_name=_("AE06 description"), max_length=1000, default="")
    action = models.CharField(verbose_name=_("AE07 action"), max_length=1000, default="")
    outcome = models.CharField(verbose_name=_("AE08 outcome"), max_length=1000, default="")

    # Serious Event questions
    contact = models.ForeignKey(StaffPerson, verbose_name=_("AE09 primary contact"), blank=True, null=True)
    # # Death
    # date_of_death = models.DateField()
    # treatment_related = models.PositiveSmallIntegerField(
    #     verbose_name=_(''),
    #     choices=YES_NO_UNKNOWN_CHOICES,
    #     blank=True, null=True)
    # # TODO: ICD10 link to go in here
    # cause_of_death_comment = models.CharField(max_length=500)
    # # Hospitalisation
    # admitted_to_itu = models.BooleanField(verbose_name=_('DO34 admitted to ITU'), default=False)
    # dialysis_needed = models.BooleanField(verbose_name=_('DO34 admitted to ITU'), default=False)
    # biopsy_taken = models.BooleanField(verbose_name=_('DO34 admitted to ITU'), default=False)
    # prolongation_of_hospitalisation = models.BooleanField(verbose_name=_('DO34 admitted to ITU'), default=False)
    # # Device specific
    # device_deficiency = models.BooleanField(verbose_name=_('DO34 admitted to ITU'), default=False)
    # device_user_error = models.BooleanField(verbose_name=_('DO34 admitted to ITU'), default=False)
    # # Lesser issues
    # unable_to_work = models.BooleanField(verbose_name=_('DO34 admitted to ITU'), default=False)
    # interfering_symptom = models.BooleanField(verbose_name=_('DO34 admitted to ITU'), default=False)
    # symptom_with_no_sequalae = models.BooleanField(verbose_name=_('DO34 admitted to ITU'), default=False)
    #
    # grade_first_30_days = models.ForeignKey(ClavienDindoGrading)
    # grade_first_30_days_d = models.BooleanField(
    #     help_text="If the patients suffers from a complication at the time of discharge, the suffix  “d” (for "
    #               "‘disability’) is added to the respective grade of complication. This label indicates the need for "
    #               "a follow-up to fully evaluate the complication.")
    # grade_post_30_days = models.ForeignKey(AlternativeGrading)

    class Meta:
        order_with_respect_to = 'organ'
        # ordering = ['sequence_number']
        verbose_name = _('AEm1 adverse event')
        verbose_name_plural = _('AEm2 adverse events')
