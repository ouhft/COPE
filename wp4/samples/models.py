#!/usr/bin/python
# coding: utf-8
from django.db import models
from django.contrib.auth.models import User
from django.utils.translation import ugettext_lazy as _
from django.utils import timezone

from ..compare.models import OrganPerson, Organ, Donor, Recipient


class Event(models.Model):
    TYPE_BLOOD = 1
    TYPE_URINE = 2
    TYPE_PERFUSATE = 3
    TYPE_TISSUE = 4
    TYPE_CHOICES = (
        (TYPE_BLOOD, _("EVc01 Blood")),
        (TYPE_URINE, _("EVc02 Urine")),
        (TYPE_PERFUSATE, _("EVc03 Perfusate")),
        (TYPE_TISSUE, _("EVc04 Tissue")),
    )
    type = models.PositiveSmallIntegerField(_("EV01 event type"), choices=TYPE_CHOICES)
    taken_at = models.DateTimeField(verbose_name=_("EV02 date and time taken"))
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)


class BarCodedItem(models.Model):
    barcode = models.CharField(verbose_name=_("SA01 barcode number"), max_length=20)
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    class Meta:
        abstract = True


class Worksheet(BarCodedItem):
    person = models.ForeignKey(OrganPerson)


class UrineSample(BarCodedItem):
    person = models.ForeignKey(OrganPerson)
    event = models.ForeignKey(Event)
    worksheet = models.ForeignKey(Worksheet, null=True, blank=True)
    centrifuged_at = models.DateTimeField(verbose_name=_("SA02 centrifuged at"), null=True, blank=True)


class BloodSample(BarCodedItem):
    SAMPLE_SST = 1
    SAMPLE_EDSA = 2
    SAMPLE_CHOICES = (
        (SAMPLE_SST, _("BSc01 Blood SST")),
        (SAMPLE_EDSA, _("BSc02 Blood EDSA")))
    person = models.ForeignKey(OrganPerson)
    event = models.ForeignKey(Event)
    worksheet = models.ForeignKey(Worksheet, null=True, blank=True)
    centrifuged_at = models.DateTimeField(verbose_name=_("SA02 centrifuged at"), null=True, blank=True)
    blood_type = models.PositiveSmallIntegerField(verbose_name=_("BS02 blood sample type"), choices=SAMPLE_CHOICES)


class PerfusateSample(BarCodedItem):
    organ = models.ForeignKey(Organ)
    event = models.ForeignKey(Event)
    worksheet = models.ForeignKey(Worksheet, null=True, blank=True)
    centrifuged_at = models.DateTimeField(verbose_name=_("SA02 centrifuged at"), null=True, blank=True)


class TissueSample(BarCodedItem):
    SAMPLE_R = "R"
    SAMPLE_F = "F"
    SAMPLE_CHOICES = (
        (SAMPLE_F, _("TSc01 ReK1F")),
        (SAMPLE_R, _("TSc02 ReK1R")))
    organ = models.ForeignKey(Organ)
    event = models.ForeignKey(Event)
    worksheet = models.ForeignKey(Worksheet, null=True, blank=True)
    tissue_type = models.CharField(max_length=1, choices=SAMPLE_CHOICES)


class Deviation(models.Model):
    worksheet = models.ForeignKey(Worksheet)
    description = models.TextField()
    occurred_at = models.DateTimeField()
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    # def __unicode__(self):
    #     return self.barcode
    #
    # class Meta:
    #     ordering = ['taken_at']
    #     verbose_name = _('SAm1 sample')
    #     verbose_name_plural = _('SAm2 samples')
