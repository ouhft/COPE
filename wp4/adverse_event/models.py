#!/usr/bin/python
# coding: utf-8
from __future__ import unicode_literals

from django.core.urlresolvers import reverse
from django.db import models
from django.utils.translation import ugettext_lazy as _

from ..compare.models import VersionControlMixin, Organ, YES_NO_UNKNOWN_CHOICES
from ..staff_person.models import StaffPerson


class AdverseEvent(VersionControlMixin):
    """
    Collects (serious) adverse event information related to a specific Organ within the study

    Based strictly on the paper form for initial version, even though that form contains issues. Fields
    commented out in code likely refer to items added to help improve the process, but were rejected.
    """

    # Event basics
    organ = models.ForeignKey(Organ, verbose_name=_("AE04 trial id link"))
    # Because even if the form wants to record a trial id string, it's going to be pointless if it doesn't actually
    # link to a record
    # sequence_number = models.PositiveSmallIntegerField(verbose_name=_("AE01 sequence number"), default=0)
    # resolution_at_date = models.DateField(verbose_name=_("AE03 resolution date"), blank=True, null=True)

    # Page 1
    serious_eligible_1 = models.BooleanField(verbose_name=_("AE10 lead to death"), default=False)
    serious_eligible_2 = models.BooleanField(verbose_name=_("AE11 life threatening illness"), default=False)
    serious_eligible_3 = models.BooleanField(verbose_name=_("AE12 permanent impairment"), default=False)
    serious_eligible_4 = models.BooleanField(verbose_name=_("AE13 hospitalisation"), default=False)
    serious_eligible_5 = models.BooleanField(verbose_name=_("AE14 prolonged hospitalisation"), default=False)
    serious_eligible_6 = models.BooleanField(verbose_name=_("AE15 surgery required"), default=False)

    # Page 2
    onset_at_date = models.DateField(verbose_name=_("AE02 onset date"))
    event_ongoing = models.BooleanField(verbose_name=_("AE03 event ongoing"), default=True)
    description = models.TextField(verbose_name=_("AE06 description"), null=True, blank=True)
    action = models.TextField(verbose_name=_("AE07 action"), null=True, blank=True)
    outcome = models.TextField(verbose_name=_("AE08 outcome"), null=True, blank=True)
    alive_query_1 = models.BooleanField(verbose_name=_("AE20 incapcity"), default=False)
    alive_query_2 = models.BooleanField(verbose_name=_("AE21 usual activity"), default=False)
    alive_query_3 = models.BooleanField(verbose_name=_("AE22 no sequelae"), default=False)
    alive_query_4 = models.BooleanField(verbose_name=_("AE23 device deficiency"), default=False)
    alive_query_5 = models.BooleanField(verbose_name=_("AE24 device user error"), default=False)
    alive_query_6 = models.BooleanField(verbose_name=_("AE25 life threatening illness"), default=False)
    alive_query_7 = models.BooleanField(verbose_name=_("AE26 permanent impairment"), default=False)
    alive_query_8 = models.BooleanField(verbose_name=_("AE27 prolonged hospitalisation"), default=False)
    alive_query_9 = models.BooleanField(verbose_name=_("AE28 surgery required"), default=False)

    # Page 3
    rehospitalisation = models.BooleanField(verbose_name=_("AE30 rehospitalisation"), default=False)
    date_of_admission = models.DateField(verbose_name=_("AE31 date of admission"), null=True, blank=True)
    date_of_discharge = models.DateField(verbose_name=_("AE32 date of discharge"), null=True, blank=True)
    admitted_to_itu = models.BooleanField(verbose_name=_('AE33 admitted to ITU'), default=False)
    dialysis_needed = models.BooleanField(verbose_name=_('AE34 dialysis needed'), default=False)
    biopsy_taken = models.BooleanField(verbose_name=_('AE35 biopsy taken'), default=False)
    surgery_required = models.BooleanField(verbose_name=_("AE36 surgery required"), default=False)
    rehospitalisation_comments = models.TextField(verbose_name=_("AE37 comments"), null=True, blank=True)

    death = models.BooleanField(verbose_name=_("AE40 led to death"), default=False)
    date_of_death = models.DateField(verbose_name=_("AE41 date of death"), null=True, blank=True)
    treatment_related = models.PositiveSmallIntegerField(
        verbose_name=_('AE49 treatment related?'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )
    cause_of_death_1 = models.BooleanField(verbose_name=_("AE42 cerebrovascular"), default=False)
    cause_of_death_2 = models.BooleanField(verbose_name=_("AE43 multi organ"), default=False)
    cause_of_death_3 = models.BooleanField(verbose_name=_("AE44 pneumonia"), default=False)
    cause_of_death_4 = models.BooleanField(verbose_name=_("AE45 sepsis"), default=False)
    cause_of_death_5 = models.BooleanField(verbose_name=_("AE46 transplant related"), default=False)
    cause_of_death_6 = models.BooleanField(verbose_name=_("AE47 other"), default=False)
    cause_of_death_comment = models.CharField(verbose_name=_("AE48 comments"), max_length=500, null=True, blank=True)

    # Page 4
    contact = models.ForeignKey(StaffPerson, verbose_name=_("AE09 primary contact"), blank=True, null=True)

    @property
    def is_serious(self):
        """
        If any of the serious_eligible questions are True, then this is a serious event

        :return: True if any serious_event fields are True
        :rtype: bool
        """
        if self.serious_eligible_1:
            return True
        if self.serious_eligible_2:
            return True
        if self.serious_eligible_3:
            return True
        if self.serious_eligible_4:
            return True
        if self.serious_eligible_5:
            return True
        if self.serious_eligible_6:
            return True
        return False

    class Meta(VersionControlMixin.Meta):
        order_with_respect_to = 'organ'
        # ordering = ['sequence_number']
        verbose_name = _('AEm1 adverse event')
        verbose_name_plural = _('AEm2 adverse events')

    def get_absolute_url(self):
        return reverse("wp4:adverse_event:update", kwargs={"pk": self.pk})

    def __str__(self):
        return "{0} @ {1}".format(self.organ.trial_id, self.onset_at_date)