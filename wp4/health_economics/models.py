#!/usr/bin/python
# coding: utf-8
from __future__ import unicode_literals

from django.core.validators import MinValueValidator, MaxValueValidator
from django.db import models
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _

from wp4.compare.models import VersionControlModel, Recipient
from wp4.compare.validators import validate_not_in_future


class QualityOfLife(VersionControlModel):
    """
    WORK IN PROGRESS - Class definition prone to rapid change

    Collects Quality of Life results for a given recipient on a specific date
    """
    recipient = models.ForeignKey(Recipient, help_text="Internal link to the Recipient")
    date_recorded = models.DateField(
        verbose_name=_("QL07 date recorded"),
        blank=True, null=True,
        validators=[validate_not_in_future],
        help_text="Date can not be in the future"
    )
    qol_mobility = models.PositiveSmallIntegerField(
        verbose_name=_("QL01 qol mobility score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True,
        help_text="Answer must be in range 1-5"
    )
    qol_selfcare = models.PositiveSmallIntegerField(
        verbose_name=_("QL02 qol self care score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True,
        help_text="Answer must be in range 1-5"
    )
    qol_usual_activities = models.PositiveSmallIntegerField(
        verbose_name=_("QL03 qol usual activites score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True,
        help_text="Answer must be in range 1-5"
    )
    qol_pain = models.PositiveSmallIntegerField(
        verbose_name=_("QL04 qol pain or discomfort score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True,
        help_text="Answer must be in range 1-5"
    )
    qol_anxiety = models.PositiveSmallIntegerField(
        verbose_name=_("QL05 qol anxiety or depression score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True,
        help_text="Answer must be in range 1-5"
    )
    vas_score = models.PositiveSmallIntegerField(
        verbose_name=_("QL06 vas score"),
        validators=[MinValueValidator(0), MaxValueValidator(100)],
        blank=True, null=True,
        help_text="Answer must be in range 0-100"
    )

    class Meta:
        verbose_name = _("QLm1 Quality of Life record")
        verbose_name_plural = _("QLm2 Quality of Life records")


