#!/usr/bin/python
# coding: utf-8
from django.db import models
from django.contrib.auth.models import User
from django.utils.translation import ugettext_lazy as _, ungettext_lazy as __
from django.utils import timezone


# Mostly replaces Specimens -- TODO: Remodel this with SampleEvent and Specimen models, plus SampleWorksheet
class Sample(models.Model):
    DONOR_BLOOD_1 = 1
    DONOR_BLOOD_2 = 2
    DONOR_URINE_1 = 3
    DONOR_URINE_2 = 4
    KIDNEY_PERFUSATE_1 = 5
    KIDNEY_PERFUSATE_2 = 6
    KIDNEY_PERFUSATE_3 = 7
    RECIPIENT_BLOOD_1 = 8
    RECIPIENT_BLOOD_2 = 9
    KIDNEY_TISSUE_1 = 10
    TYPE_CHOICES = (
        (DONOR_BLOOD_1, _("SA10 Donor blood 1")),
        (DONOR_BLOOD_2, _("SA11 Donor blood 2")),
        (DONOR_URINE_1, _("SA12 Donor urine 1")),
        (DONOR_URINE_2, _("SA13 Donor urine 2")),
        (KIDNEY_PERFUSATE_1, _("SA14 Kidney perfusate 1")),
        (KIDNEY_PERFUSATE_2, _("SA15 Kidney perfusate 1")),
        (KIDNEY_PERFUSATE_3, _("SA16 Kidney perfusate 1")),
        (RECIPIENT_BLOOD_1, _("SA17 Recipient blood 1")),
        (RECIPIENT_BLOOD_2, _("SA18 Recipient blood 1")),
        (KIDNEY_TISSUE_1, _("SA19 Kidney tissue 1")),
    )
    type = models.PositiveSmallIntegerField(_("SA05 sample type"), choices=TYPE_CHOICES)
    barcode = models.CharField(verbose_name=_("SA01 barcode number"), max_length=20)
    taken_at = models.DateTimeField(verbose_name=_("SA02 date and time taken"))
    centrifugation = models.DateTimeField(verbose_name=_("SA03 centrifugation"), null=True, blank=True)
    comment = models.CharField(verbose_name=_("SA04 comment"), max_length=2000, blank=True)
    #  TODO: Specimen state?
    #  TODO: Who took the sample?
    #  TODO: Difference between worksheet and specimen barcodes?
    #  TODO: Reperfusion?
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    def linked_to(self):
        if self.type is self.DONOR_BLOOD_1:
            return self.donor_blood_1
        if self.type is self.DONOR_BLOOD_2:
            return self.donor_blood_2
        if self.type is self.DONOR_URINE_1:
            return self.donor_urine_1
        if self.type is self.DONOR_URINE_2:
            return self.donor_urine_2
        if self.type is self.KIDNEY_PERFUSATE_1:
            return self.kidney_perfusate_1
        if self.type is self.KIDNEY_PERFUSATE_2:
            return self.kidney_perfusate_2
        if self.type is self.KIDNEY_PERFUSATE_3:
            return self.kidney_perfusate_3
        return None

    def __unicode__(self):
        return self.barcode

    class Meta:
        ordering = ['taken_at']
        verbose_name = _('SAm1 sample')
        verbose_name_plural = _('SAm2 samples')
