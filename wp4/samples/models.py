#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals
import datetime

from django.contrib.auth.models import User
from django.core.validators import ValidationError
from django.db import models
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _

from ..compare.models import OrganPerson, Organ


class BarCodedItem(models.Model):
    barcode = models.CharField(verbose_name=_("SA01 barcode number"), max_length=20, blank=True)
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
    worksheet = models.ForeignKey(Worksheet)
    type = models.PositiveSmallIntegerField(_("EV01 sample type"), choices=TYPE_CHOICES)
    taken_at = models.DateTimeField(verbose_name=_("EV02 date and time taken"), null=True, blank=True)
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    class Meta:
        ordering = ['taken_at']
        verbose_name = _('EVm1 sample event')
        verbose_name_plural = _('EVm2 sample events')

    def clean(self):
        if self.type == Event.TYPE_BLOOD:
            sample_set = self.bloodsample_set.all()
            count = len(sample_set)
            if count > 0 and count != 2:
                raise ValidationError(_("EVv01 this event should have two blood sample records"))
            if sample_set[0].blood_type == sample_set[1].blood_type:
                raise ValidationError(_("EVv06 the blood samples must not have matching types"))
        if self.type == Event.TYPE_URINE:
            count = len(self.urinesample_set.all())
            if count > 0 and count != 1:
                raise ValidationError(_("EVv02 this event should have one urine sample record"))
        if self.type == Event.TYPE_PERFUSATE:
            count = len(self.perfusatesample_set.all())
            if count > 0 and count != 1:
                raise ValidationError(_("EVv03 this event should have one perfusate sample record"))
        if self.type == Event.TYPE_TISSUE:
            sample_set = self.tissuesample_set.all()
            count = len(sample_set)
            if count > 0 and count != 2:
                raise ValidationError(_("EVv04 this event should have two tissue sample records"))
            if sample_set[0].blood_type == sample_set[1].blood_type:
                raise ValidationError(_("EVv07 the tissue samples must not have matching types"))
        if self.taken_at:
            if self.taken_at > timezone.now():
                raise ValidationError(_("EVv05 Time travel detected! Taken at date and time is in the future!"))

    def __unicode__(self):
        return "%s (%s)" % (self.get_type_display(), self.taken_at)


class DeviationMixin(models.Model):
    """
    In place of Deviation records on a worksheet, we have a way of tracking them per sample
    """
    collected = models.NullBooleanField(verbose_name=_("WM01 sample collected?"), blank=True)
    notes = models.TextField(verbose_name=_("WM02 notes"), blank=True)

    class Meta:
        abstract = True


class BloodSample(BarCodedItem, DeviationMixin):
    SAMPLE_SST = 1
    SAMPLE_EDSA = 2
    SAMPLE_CHOICES = (
        (SAMPLE_SST, _("BSc01 Blood SST")),
        (SAMPLE_EDSA, _("BSc02 Blood EDSA")))
    person = models.ForeignKey(OrganPerson)
    event = models.ForeignKey(Event, limit_choices_to={'type': Event.TYPE_BLOOD})
    centrifuged_at = models.DateTimeField(verbose_name=_("BS01 centrifuged at"), null=True, blank=True)
    blood_type = models.PositiveSmallIntegerField(verbose_name=_("BS02 blood sample type"), choices=SAMPLE_CHOICES)

    class Meta:
        ordering = ['person', 'event__taken_at']
        verbose_name = _('BSm1 blood sample')
        verbose_name_plural = _('BSm2 blood samples')

    def clean(self):
        if self.collected is not None:
            if not self.collected and self.notes == "":
                raise ValidationError(_("BSv01 you must enter some notes about why this sample was not collected"))
            if self.collected and not self.event.taken_at:
                raise ValidationError(_("BSv04 Please record the time the sample was taken"))

        if self.centrifuged_at:
            if self.centrifuged_at > timezone.now():
                raise ValidationError(_("BSv02 Time travel detected! Centrifuged at date and time is in the future!"))
            if self.event.taken_at:
                if self.centrifuged_at < self.event.taken_at:
                    raise ValidationError(_("BSv03 Body in spin dryer! Sample was centrifuged before it was taken?"))


class UrineSample(BarCodedItem, DeviationMixin):
    person = models.ForeignKey(OrganPerson)
    event = models.ForeignKey(Event, limit_choices_to={'type': Event.TYPE_URINE})
    centrifuged_at = models.DateTimeField(verbose_name=_("US01 centrifuged at"), null=True, blank=True)

    class Meta:
        ordering = ['person', 'event__taken_at']
        verbose_name = _('USm1 urine sample')
        verbose_name_plural = _('USm2 urine samples')

    def clean(self):
        if self.collected is not None:
            if not self.collected and self.notes == "":
                raise ValidationError(_("USv01 you must enter some notes about why this sample was not collected"))
            if self.collected and not self.event.taken_at:
                raise ValidationError(_("USv04 Please record the time the sample was taken"))

        if self.centrifuged_at:
            if self.centrifuged_at > timezone.now():
                raise ValidationError(_("USv02 Time travel detected! Centrifuged at date and time is in the future!"))
            if self.event.taken_at:
                if self.centrifuged_at < self.event.taken_at:
                    raise ValidationError(_("USv03 Body in spin dryer! Sample was centrifuged before it was taken?"))


class PerfusateSample(BarCodedItem, DeviationMixin):
    organ = models.ForeignKey(Organ)
    event = models.ForeignKey(Event, limit_choices_to={'type': Event.TYPE_PERFUSATE})
    centrifuged_at = models.DateTimeField(verbose_name=_("PS01 centrifuged at"), null=True, blank=True)

    class Meta:
        ordering = ['organ', 'event__taken_at']
        verbose_name = _('PSm1 perfusate sample')
        verbose_name_plural = _('PSm2 perfusate samples')

    def clean(self):
        if self.collected is not None:
            if not self.collected and self.notes == "":
                raise ValidationError(_("PSv01 you must enter some notes about why this sample was not collected"))
            if self.collected and not self.event.taken_at:
                raise ValidationError(_("PSv04 Please record the time the sample was taken"))

        if self.centrifuged_at:
            if self.centrifuged_at > timezone.now():
                raise ValidationError(_("PSv02 Time travel detected! Centrifuged at date and time is in the future!"))
            if self.event.taken_at:
                if self.centrifuged_at < self.event.taken_at:
                    raise ValidationError(_("PSv03 Body in spin dryer! Sample was centrifuged before it was taken?"))


class TissueSample(BarCodedItem, DeviationMixin):
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

    def clean(self):
        if self.collected is not None:
            if not self.collected and self.notes == "":
                raise ValidationError(_("TSv01 you must enter some notes about why this sample was not collected"))
            if self.collected and not self.event.taken_at:
                raise ValidationError(_("TSv02 Please record the time the sample was taken"))
