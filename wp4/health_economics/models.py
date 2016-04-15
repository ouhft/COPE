#!/usr/bin/python
# coding: utf-8
from __future__ import unicode_literals

from django.core.validators import MinValueValidator, MaxValueValidator
from django.db import models
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _
from wp4.compare.models import VersionControlModel, Recipient


class QualityOfLife(VersionControlModel):
    recipient = models.ForeignKey(Recipient)
    qol_mobility = models.PositiveSmallIntegerField(
        verbose_name=_("QL01 qol mobility score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True
    )
    qol_selfcare = models.PositiveSmallIntegerField(
        verbose_name=_("QL02 qol self care score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True
    )
    qol_usual_activities = models.PositiveSmallIntegerField(
        verbose_name=_("QL03 qol usual activites score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True
    )
    qol_pain = models.PositiveSmallIntegerField(
        verbose_name=_("QL04 qol pain or discomfort score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True
    )
    qol_anxiety = models.PositiveSmallIntegerField(
        verbose_name=_("QL05 qol anxiety or depression score"),
        validators=[MinValueValidator(1), MaxValueValidator(5)],
        blank=True, null=True
    )
    vas_score = models.PositiveSmallIntegerField(
        verbose_name=_("QL06 vas score"),
        validators=[MinValueValidator(0), MaxValueValidator(100)],
        blank=True, null=True
    )

    class Meta:
        verbose_name = _("QLm1 Quality of Life record")
        verbose_name_plural = _("QLm2 Quality of Life records")