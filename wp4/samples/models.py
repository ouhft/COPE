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
    type = models.PositiveSmallIntegerField(_("EV01 sample type"), choices=TYPE_CHOICES)
    taken_at = models.DateTimeField(verbose_name=_("EV02 date and time taken"))
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    class Meta:
        ordering = ['taken_at']
        verbose_name = _('EVm1 sample event')
        verbose_name_plural = _('EVm2 sample events')

    def __unicode__(self):
        return "%s (%s)" % (self.get_type_display(), self.taken_at)


class BarCodedItem(models.Model):
    barcode = models.CharField(verbose_name=_("SA01 barcode number"), max_length=20)
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    class Meta:
        abstract = True

    def __unicode__(self):
        return "%s" % self.barcode


class Worksheet(BarCodedItem):
    person = models.ForeignKey(OrganPerson)

    class Meta:
        ordering = ['person']
        verbose_name = _('WSm1 worksheet')
        verbose_name_plural = _('WSm2 worksheets')

    def __unicode__(self):
        return "%s" % self.barcode


class WorksheetMixin(models.Model):
    worksheet = models.ForeignKey(Worksheet, null=True, blank=True)
    collected = models.NullBooleanField(verbose_name=_("WM01 sample collected?"), blank=True)
    notes = models.TextField(verbose_name=_("WM02 notes"), blank=True)

    class Meta:
        abstract = True


class UrineSample(BarCodedItem, WorksheetMixin):
    person = models.ForeignKey(OrganPerson)
    event = models.ForeignKey(Event, limit_choices_to={'type': Event.TYPE_URINE})
    centrifuged_at = models.DateTimeField(verbose_name=_("SA02 centrifuged at"), null=True, blank=True)

    class Meta:
        ordering = ['person', 'event__taken_at']
        verbose_name = _('USm1 urine sample')
        verbose_name_plural = _('USm2 urine samples')


class BloodSample(BarCodedItem, WorksheetMixin):
    SAMPLE_SST = 1
    SAMPLE_EDSA = 2
    SAMPLE_CHOICES = (
        (SAMPLE_SST, _("BSc01 Blood SST")),
        (SAMPLE_EDSA, _("BSc02 Blood EDSA")))
    person = models.ForeignKey(OrganPerson)
    event = models.ForeignKey(Event, limit_choices_to={'type': Event.TYPE_BLOOD})
    centrifuged_at = models.DateTimeField(verbose_name=_("SA02 centrifuged at"), null=True, blank=True)
    blood_type = models.PositiveSmallIntegerField(verbose_name=_("BS02 blood sample type"), choices=SAMPLE_CHOICES)

    class Meta:
        ordering = ['person', 'event__taken_at']
        verbose_name = _('BSm1 blood sample')
        verbose_name_plural = _('BSm2 blood samples')


class PerfusateSample(BarCodedItem, WorksheetMixin):
    organ = models.ForeignKey(Organ)
    event = models.ForeignKey(Event, limit_choices_to={'type': Event.TYPE_PERFUSATE})
    centrifuged_at = models.DateTimeField(verbose_name=_("SA02 centrifuged at"), null=True, blank=True)

    class Meta:
        ordering = ['organ', 'event__taken_at']
        verbose_name = _('PSm1 perfusate sample')
        verbose_name_plural = _('PSm2 perfusate samples')


class TissueSample(BarCodedItem, WorksheetMixin):
    SAMPLE_R = "R"
    SAMPLE_F = "F"
    SAMPLE_CHOICES = (
        (SAMPLE_F, _("TSc01 ReK1F")),
        (SAMPLE_R, _("TSc02 ReK1R")))
    organ = models.ForeignKey(Organ)
    event = models.ForeignKey(Event, limit_choices_to={'type': Event.TYPE_TISSUE})
    tissue_type = models.CharField(max_length=1, choices=SAMPLE_CHOICES)

    class Meta:
        ordering = ['organ', 'event__taken_at']
        verbose_name = _('TSm1 tissue sample')
        verbose_name_plural = _('TSm2 tissue samples')
