#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals
from random import random

from django.db import models
from django.utils.translation import ugettext_lazy as _
from django.utils import timezone

import wp4.compare.models as compare_models


LIVE_UNITED_KINGDOM = 1
LIVE_EUROPE = 2
PAPER_EUROPE = 3
PAPER_UNITED_KINGDOM = 4
LIST_CHOICES = (
    (LIVE_UNITED_KINGDOM, _("RNc01 UK Live list")),
    (LIVE_EUROPE, _("RNc02 Europe Live list")),
    (PAPER_UNITED_KINGDOM, _("RNc03 UK Offline list")),
    (PAPER_EUROPE, _("RNc04 Europe Offline list")),
)


def random_5050():
    return random() >= 0.5  # True/False


class Randomisation(models.Model):
    """
    Populated from the supplied CSV file via the fixture. A 'True' result is HMP+O2 for the Left Organ
    """
    donor = models.OneToOneField(compare_models.Donor, null=True, blank=True, default=None)  # Internal key
    list_code = models.PositiveSmallIntegerField(verbose_name=_("RA01 list code"), choices=LIST_CHOICES)
    result = models.BooleanField(verbose_name=_("RA02 result"), default=random_5050)
    allocated_on = models.DateTimeField(verbose_name=_("RA03 allocated on"), default=timezone.now)

    @staticmethod
    def get_and_assign_result(list_code, link_donor):
        options = Randomisation.objects.filter(country=list_code, donor=None).order_by('id')
        if len(options) < 1:
            raise Exception("No remaining values for randomisation")
        result = options[0]
        result.donor = link_donor
        result.allocated_on = timezone.now()
        result.save()
        return result.result
