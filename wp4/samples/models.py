#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib.contenttypes.fields import GenericForeignKey, GenericRelation
from django.contrib.contenttypes.models import ContentType
from django.core.validators import ValidationError
from django.core.urlresolvers import reverse
from django.db import models
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _


from wp4.compare.models.core import AuditControlModelBase
from wp4.compare.models import Patient, Organ
from .managers import EventModelForUserManager, SampleModelForUserManager


class BarCodeMixin(models.Model):
    """
    Extends with a barcode field
    """
    barcode = models.CharField(verbose_name=_("BC01 barcode number"), max_length=20, blank=True, db_index=True)

    class Meta:
        abstract = True

    def __str__(self):
        return "{0}: {1}".format(self.id, self.barcode)


class DeviationMixin(models.Model):
    """
    In place of Deviation records on a worksheet, we have a way of tracking them per sample.
    Also holds the meta question of whether this specific sample was actually taken or not.
    """
    collected = models.NullBooleanField(verbose_name=_("DM01 sample collected?"), blank=True)
    notes = models.TextField(verbose_name=_("DM02 notes"), blank=True, help_text="Deviation record")

    class Meta:
        abstract = True


class WP7Record(BarCodeMixin):
    """
    Based on an export from the WP7 Database. A record in this table means a sample exists on file in the Biobank
    """
    content_type = models.ForeignKey(ContentType, on_delete=models.CASCADE, null=True, blank=True)
    object_id = models.PositiveIntegerField(null=True, blank=True)
    content_object = GenericForeignKey()

    box_number = models.CharField(max_length=20, null=True, blank=True)
    position_in_box = models.CharField(max_length=3, null=True, blank=True)


class Event(AuditControlModelBase):
    """
    A Sample Event is a timepoint and process where a specific type of biological sample was extracted
    from a person. This even can validate whether enough biological samples were taken (e.g. a blood
    sample results in two separate containers; whereas a perfusate sample has only one), and acts as the
    link between a Worksheet and the various Sample types
    """
    TYPE_BLOOD = 1  #: Constant for TYPE_CHOICES
    TYPE_URINE = 2  #: Constant for TYPE_CHOICES
    TYPE_PERFUSATE = 3  #: Constant for TYPE_CHOICES
    TYPE_TISSUE = 4  #: Constant for TYPE_CHOICES
    TYPE_CHOICES = (
        (TYPE_BLOOD, _("EVc01 Blood")),
        (TYPE_URINE, _("EVc02 Urine")),
        (TYPE_PERFUSATE, _("EVc03 Perfusate")),
        (TYPE_TISSUE, _("EVc04 Tissue")),
    )  #: Limit of choices for Event type data
    NAME_DONOR_BLOOD1 = 1  #: Constant for NAME_CHOICES
    NAME_DONOR_URINE1 = 2  #: Constant for NAME_CHOICES
    NAME_DONOR_URINE2 = 3  #: Constant for NAME_CHOICES
    NAME_ORGAN_PERFUSATE1 = 4  #: Constant for NAME_CHOICES
    NAME_ORGAN_PERFUSATE2 = 5  #: Constant for NAME_CHOICES
    NAME_ORGAN_PERFUSATE3 = 6  #: Constant for NAME_CHOICES
    NAME_ORGAN_TISSUE = 7  #: Constant for NAME_CHOICES
    NAME_RECIPIENT_BLOOD1 = 8  #: Constant for NAME_CHOICES
    NAME_RECIPIENT_BLOOD2 = 9  #: Constant for NAME_CHOICES
    NAME_CHOICES = (
        (NAME_DONOR_BLOOD1, _("EVc05 donor blood 1")),
        (NAME_DONOR_URINE1, _("EVc06 donor urine 1")),
        (NAME_DONOR_URINE2, _("EVc07 donor urine 2")),
        (NAME_ORGAN_PERFUSATE1, _("EVc08 organ perfusate 1")),
        (NAME_ORGAN_PERFUSATE2, _("EVc09 organ perfusate 2")),
        (NAME_ORGAN_PERFUSATE3, _("EVc10 organ perfusate 3")),
        (NAME_ORGAN_TISSUE, _("EVc11 organ tissue 1")),
        (NAME_RECIPIENT_BLOOD1, _("EVc12 recipient blood 1")),
        (NAME_RECIPIENT_BLOOD2, _("EVc13 recipient blood 2")),
    )  #: Limit of choices for Event name data

    type = models.PositiveSmallIntegerField(_("EV01 sample type"), choices=TYPE_CHOICES)
    name = models.PositiveSmallIntegerField(_("EV03 sample process name"), choices=NAME_CHOICES)
    taken_at = models.DateTimeField(verbose_name=_("EV02 date and time taken"), null=True, blank=True)

    objects = EventModelForUserManager()

    class Meta:
        ordering = ['type', 'name']
        verbose_name = _('EVm1 sample event')
        verbose_name_plural = _('EVm2 sample events')

    def clean(self):
        """
        If type is...

        * BLOOD - check for two different types of BloodSample
        * URINE - check for one type of UrineSample
        * PERFUSATE - check for one type of PerfusateSample
        * TISSUE - check for two different types of TissueSample

        ... and that a suitable Event name was chosen, and that taken_at value  is not in the future.

        :return: None
        """
        if self.type == Event.TYPE_BLOOD:
            sample_set = self.bloodsample_set.all()
            count = len(sample_set)
            if count > 0 and count != 2:
                raise ValidationError(_("EVv01 this event should have two blood sample records"))
            if sample_set[0].blood_type == sample_set[1].blood_type:
                raise ValidationError(_("EVv06 the blood samples must not have matching types"))
            if self.name not in (Event.NAME_DONOR_BLOOD1, Event.NAME_RECIPIENT_BLOOD1, Event.NAME_RECIPIENT_BLOOD2):
                raise ValidationError(_("EVv08 invalid name selected (please select a blood related name)"))
        if self.type == Event.TYPE_URINE:
            count = len(self.urinesample_set.all())
            if count > 0 and count != 1:
                raise ValidationError(_("EVv02 this event should have one urine sample record"))
            if self.name not in (Event.NAME_DONOR_URINE1, Event.NAME_DONOR_URINE2):
                raise ValidationError(_("EVv09 invalid name selected (please select a urine related name)"))
        if self.type == Event.TYPE_PERFUSATE:
            count = len(self.perfusatesample_set.all())
            if count > 0 and count != 1:
                raise ValidationError(_("EVv03 this event should have one perfusate sample record"))
            if self.name not in (Event.NAME_ORGAN_PERFUSATE1, Event.NAME_ORGAN_PERFUSATE2, Event.NAME_ORGAN_PERFUSATE3):
                raise ValidationError(_("EVv10 invalid name selected (please select a perfusate related name)"))
        if self.type == Event.TYPE_TISSUE:
            sample_set = self.tissuesample_set.all()
            count = len(sample_set)
            if count > 0 and count != 2:
                raise ValidationError(_("EVv04 this event should have two tissue sample records"))
            if sample_set[0].tissue_type and sample_set[1].tissue_type and sample_set[0].tissue_type == sample_set[1].tissue_type:
                raise ValidationError(_("EVv07 the tissue samples must not have matching types"))
            if self.name != Event.NAME_ORGAN_TISSUE:
                raise ValidationError(_("EVv11 invalid name selected (please select a tissue related name)"))
        if self.taken_at:
            if self.taken_at > timezone.now():
                raise ValidationError(_("EVv05 Time travel detected! Taken at date and time is in the future!"))

    def __str__(self):
        return "%s (%s)" % (self.get_type_display(), self.taken_at)

    def get_absolute_url(self):
        return reverse("wp4:samples:event_detail", kwargs={"pk": self.pk})


class BloodSample(AuditControlModelBase, BarCodeMixin, DeviationMixin):
    """
    Sample class type for Blood samples.

    Also contains a helper (repeated, but direct) link to the Patient this sample was from (mainly
    to aide reverse sample look ups from Patient)
    """
    SAMPLE_SST = 1  #: Constant for SAMPLE_CHOICES
    SAMPLE_EDSA = 2  #: Constant for SAMPLE_CHOICES
    SAMPLE_CHOICES = (
        (SAMPLE_SST, _("BSc01 Blood SST")),
        (SAMPLE_EDSA, _("BSc02 Blood EDSA"))
    )  #: Limit of choices for BloodSample blood_type

    event = models.ForeignKey(
        Event,
        limit_choices_to={'type': Event.TYPE_BLOOD},
        help_text="Link to an event of type Blood"
    )
    blood_type = models.PositiveSmallIntegerField(verbose_name=_("BS02 blood sample type"), choices=SAMPLE_CHOICES)
    person = models.ForeignKey(Patient, verbose_name=_("BS03 sample from"))
    centrifuged_at = models.DateTimeField(verbose_name=_("BS01 centrifuged at"), null=True, blank=True)
    wp7_location = GenericRelation(WP7Record)

    objects = SampleModelForUserManager()

    class Meta:
        ordering = ['person', 'event__taken_at']
        verbose_name = _('BSm1 blood sample')
        verbose_name_plural = _('BSm2 blood samples')

    def clean(self):
        """
        If sample is marked as collected, confirm taken_at date is present, otherwise that deviation
        notes have been recorded.

        Ensure that centrifuged_at datetime is not in the future, and that it occured after taken_at datetime

        :return: None
        """
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


class UrineSample(AuditControlModelBase, BarCodeMixin, DeviationMixin):
    """
    Sample class type for Urine samples.

    Also contains a helper (repeated, but direct) link to the Patient this sample was from (mainly
    to aide reverse sample look ups from Patient)
    """
    event = models.ForeignKey(
        Event,
        limit_choices_to={'type': Event.TYPE_URINE},
        help_text="Link to an event of type Urine"
    )
    person = models.ForeignKey(Patient, verbose_name=_("US02 sample from"))
    centrifuged_at = models.DateTimeField(verbose_name=_("US01 centrifuged at"), null=True, blank=True)
    wp7_location = GenericRelation(WP7Record)

    objects = SampleModelForUserManager()

    class Meta:
        ordering = ['person', 'event__taken_at']
        verbose_name = _('USm1 urine sample')
        verbose_name_plural = _('USm2 urine samples')

    def clean(self):
        """
        If sample is marked as collected, confirm taken_at date is present, otherwise that deviation
        notes have been recorded.

        Ensure that centrifuged_at datetime is not in the future, and that it occured after taken_at datetime

        :return: None
        """
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


class PerfusateSample(AuditControlModelBase, BarCodeMixin, DeviationMixin):
    """
    Sample class type for Perfusate samples.

    Also contains a helper (repeated, but direct) link to the Patient this sample was from (mainly
    to aide reverse sample look ups from Patient)
    """
    event = models.ForeignKey(
        Event,
        limit_choices_to={'type': Event.TYPE_PERFUSATE},
        help_text="Link to an event of type Perfusate"
    )
    organ = models.ForeignKey(Organ, verbose_name=_("PS02 sample from"))
    centrifuged_at = models.DateTimeField(verbose_name=_("PS01 centrifuged at"), null=True, blank=True)
    wp7_location = GenericRelation(WP7Record)

    objects = SampleModelForUserManager()

    class Meta:
        ordering = ['organ', 'event__taken_at']
        verbose_name = _('PSm1 perfusate sample')
        verbose_name_plural = _('PSm2 perfusate samples')

    def clean(self):
        """
        If sample is marked as collected, confirm taken_at date is present, otherwise that deviation
        notes have been recorded.

        Ensure that centrifuged_at datetime is not in the future, and that it occured after taken_at datetime

        :return: None
        """
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


class TissueSample(AuditControlModelBase, BarCodeMixin, DeviationMixin):
    """
    Sample class type for Perfusate samples.

    Also contains a helper (repeated, but direct) link to the Patient this sample was from (mainly
    to aide reverse sample look ups from Patient)
    """
    SAMPLE_R = "R"  #: Constant for SAMPLE_CHOICES
    SAMPLE_F = "F"  #: Constant for SAMPLE_CHOICES
    SAMPLE_CHOICES = (
        (SAMPLE_F, _("TSc01 ReK1F")),
        (SAMPLE_R, _("TSc02 ReK1R"))
    )  #: Limit of choices for BloodSample tissue_type

    event = models.ForeignKey(
        Event,
        limit_choices_to={'type': Event.TYPE_TISSUE},
        help_text="Link to an event of type Tissue"
    )
    organ = models.ForeignKey(Organ, verbose_name=_("TS01 sample from"))
    tissue_type = models.CharField(max_length=1, choices=SAMPLE_CHOICES, verbose_name=_("TS02 tissue sample type"))
    wp7_location = GenericRelation(WP7Record)

    objects = SampleModelForUserManager()

    class Meta:
        ordering = ['organ', 'event__taken_at']
        verbose_name = _('TSm1 tissue sample')
        verbose_name_plural = _('TSm2 tissue samples')

    def clean(self):
        """
        If sample is marked as collected, confirm taken_at date is present, otherwise that deviation
        notes have been recorded.

        :return: None
        """
        if self.collected is not None:
            if not self.collected and self.notes == "":
                raise ValidationError(_("TSv01 you must enter some notes about why this sample was not collected"))
            if self.collected and not self.event.taken_at:
                raise ValidationError(_("TSv02 Please record the time the sample was taken"))
