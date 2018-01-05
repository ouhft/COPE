#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.core.validators import MinValueValidator, MaxValueValidator
from django.db import models
from django.urls import reverse
from django.utils.translation import ugettext_lazy as _

from wp4.compare.models.core import AuditControlModelBase
from wp4.compare.models.transplantation import Recipient
from wp4.compare.validators import validate_not_in_future

from .managers import HealthEconomicsModelForUserManager, ResourceLogModelForUserManager


class QualityOfLife(AuditControlModelBase):
    """
    WORK IN PROGRESS - Class definition prone to rapid change

    Collects Quality of Life results for a given recipient on a specific date
    """
    recipient = models.ForeignKey(Recipient, on_delete=models.PROTECT)  # Internal link to the Recipient
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

    # FK related name links
    # followup_3m
    # followup_1y

    objects = HealthEconomicsModelForUserManager()

    class Meta:
        verbose_name = _("QLm1 Quality of Life record")
        verbose_name_plural = _("QLm2 Quality of Life records")
        permissions = (
            ("view_qualityoflife", "Can only view the data"),
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )

    def country_for_restriction(self):
        """
        Get the country to be used for geographic restriction of this data
        :return: Int: Value from list in Locations.Models. Should be in range [1,4,5]
        """
        return self.recipient.country_for_restriction

    def location_for_restriction(self):
        """
        Get the location to be used for geographic restriction of this data
        :return: Int: Hospital object id
        """
        return self.recipient.location_for_restriction

    def get_absolute_url(self):
        return reverse("wp4:health-economics:update", kwargs={"pk": self.pk})

    def __str__(self):
        return "{0} @ {1}".format(self.recipient.trial_id, self.date_recorded)


class ResourceLog(AuditControlModelBase):
    """
    WORK IN PROGRESS

    Collects the data from the Participant Resource Use Log. Acts as the central link to several
    smaller classes that collectively capture the data.
    """
    recipient = models.ForeignKey(Recipient, on_delete=models.PROTECT)  # Internal link to the Recipient
    date_given = models.DateField(
        verbose_name=_("RL01 date given"),
        blank=True, null=True,
        validators=[validate_not_in_future],
        help_text="Date can not be in the future"
    )
    date_returned = models.DateField(
        verbose_name=_("RL02 date returned"),
        blank=True, null=True,
        validators=[validate_not_in_future],
        help_text="Date can not be in the future"
    )

    # FK related names:
    # visits
    # hospitalisations
    # rehabilitations
    notes = models.TextField(verbose_name=_("RL03 notes"), null=True, blank=True)

    objects = HealthEconomicsModelForUserManager()

    class Meta:
        verbose_name = _("RLm1 Resource Usage Log")
        verbose_name_plural = _("RLm2 Resource Usage Logs")
        permissions = (
            ("view_resourcelog", "Can only view the data"),
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )

    def country_for_restriction(self):
        """
        Get the country to be used for geographic restriction of this data
        :return: Int: Value from list in Locations.Models. Should be in range [1,4,5]
        """
        return self.recipient.country_for_restriction

    def location_for_restriction(self):
        """
        Get the location to be used for geographic restriction of this data
        :return: Int: Hospital object id
        """
        return self.recipient.location_for_restriction

    @property
    def count_visits(self, visit_type=None):
        # TODO: Implement me!
        return 0


class ResourceVisit(AuditControlModelBase):
    TYPE_VISIT_GP = 1
    TYPE_GP_VISIT = 2
    TYPE_SPECIALIST = 3
    TYPE_HOSPITAL = 4
    TYPE_CHOICES = (
        (TYPE_VISIT_GP, _("RVc01 appointment at GP")),
        (TYPE_GP_VISIT, _("RVc02 GP visited")),
        (TYPE_SPECIALIST, _("RVc03 appointment at specialist")),
        (TYPE_HOSPITAL, _("RVc04 A&E")),
    )
    log = models.ForeignKey(ResourceLog, on_delete=models.PROTECT, related_name="visits")
    type = models.PositiveSmallIntegerField(verbose_name=_('RV01 visit type'), choices=TYPE_CHOICES)

    objects = ResourceLogModelForUserManager()

    class Meta:
        permissions = (
            ("view_resourcevisit", "Can only view the data"),
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )

    def country_for_restriction(self):
        """
        Get the country to be used for geographic restriction of this data
        :return: Int: Value from list in Locations.Models. Should be in range [1,4,5]
        """
        return self.log.country_for_restriction

    def location_for_restriction(self):
        """
        Get the location to be used for geographic restriction of this data
        :return: Int: Hospital object id
        """
        return self.log.location_for_restriction


class ResourceHospitalAdmission(AuditControlModelBase):
    log = models.ForeignKey(ResourceLog, on_delete=models.PROTECT, related_name="hospitalisations")
    reason = models.CharField(verbose_name=_('RH01 reason'), max_length=200, blank=True)
    had_surgery = models.NullBooleanField(verbose_name=_("RH02 had surgery"), blank=True)
    days_in_itu = models.PositiveSmallIntegerField(verbose_name=_("RH03 days in itu"), default=0)
    days_in_hospital = models.PositiveSmallIntegerField(verbose_name=_("RH04 days in hospital"), default=1)

    objects = ResourceLogModelForUserManager()

    class Meta:
        permissions = (
            ("view_resourcehospitaladmission", "Can only view the data"),
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )

    def country_for_restriction(self):
        """
        Get the country to be used for geographic restriction of this data
        :return: Int: Value from list in Locations.Models. Should be in range [1,4,5]
        """
        return self.log.country_for_restriction

    def location_for_restriction(self):
        """
        Get the location to be used for geographic restriction of this data
        :return: Int: Hospital object id
        """
        return self.log.location_for_restriction


class ResourceRehabilitation(AuditControlModelBase):
    log = models.ForeignKey(ResourceLog, on_delete=models.PROTECT, related_name="rehabilitations")
    reason = models.CharField(verbose_name=_('RR01 reason'), max_length=200, blank=True)
    days_in_hospital = models.PositiveSmallIntegerField(verbose_name=_("RR02 days in hospital"), default=1)

    objects = ResourceLogModelForUserManager()

    class Meta:
        permissions = (
            ("view_resourcerehabilitation", "Can only view the data"),
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )

    def country_for_restriction(self):
        """
        Get the country to be used for geographic restriction of this data
        :return: Int: Value from list in Locations.Models. Should be in range [1,4,5]
        """
        return self.log.country_for_restriction

    def location_for_restriction(self):
        """
        Get the location to be used for geographic restriction of this data
        :return: Int: Hospital object id
        """
        return self.log.location_for_restriction
