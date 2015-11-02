#!/usr/bin/python
# coding: utf-8
from django.db import models
from django.contrib.auth.models import User
from django.utils.translation import ugettext_lazy as _, ungettext_lazy as __
from django.utils import timezone


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


class UrineSample(BarCodedItem):
    event = models.ForeignKey(Event)
    centrifuged_at = models.DateTimeField(verbose_name=_("SA02 centrifuged at"), null=True, blank=True)


class PerfusateSample(BarCodedItem):
    event = models.ForeignKey(Event)
    centrifuged_at = models.DateTimeField(verbose_name=_("SA02 centrifuged at"), null=True, blank=True)


class BloodSample(BarCodedItem):
    event = models.ForeignKey(Event)
    centrifuged_at = models.DateTimeField(verbose_name=_("SA02 centrifuged at"), null=True, blank=True)
    blood_type = None


class TissueSample(BarCodedItem):
    event = models.ForeignKey(Event)
    tissue_type = None


class DonorWorksheet(BarCodedItem):
    donor = None


class RecipientWorksheet(BarCodedItem):
    recipient = None


class Deviation(models.Model):
    description = None
    occurred_at = None
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)





    def __unicode__(self):
        return self.barcode

    class Meta:
        ordering = ['taken_at']
        verbose_name = _('SAm1 sample')
        verbose_name_plural = _('SAm2 samples')
