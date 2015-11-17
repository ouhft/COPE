#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.db import models
from django.contrib.auth.models import User
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _, ungettext_lazy as __


# Common CONSTANTS
UNITED_KINGDOM = 1
BELGIUM = 4
NETHERLANDS = 5
COUNTRY_CHOICES = (
    (UNITED_KINGDOM, _('MM10 United Kingdom')),
    (BELGIUM, _('MM11 Belgium')),
    (NETHERLANDS, _('MM12 Netherlands'))
)


# Consider making this part of a LOCATION class
class Hospital(models.Model):
    name = models.CharField(verbose_name=_("HO01 hospital name"), max_length=100)
    country = models.PositiveSmallIntegerField(verbose_name=_("HO02 country"), choices=COUNTRY_CHOICES)
    is_active = models.BooleanField(verbose_name=_("HO03 is active"), default=True)
    is_project_site = models.BooleanField(verbose_name=_("HO04 is project site"), default=False)
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    def full_description(self):
        return '%s, %s' % (self.name, self.get_country_display())

    def __unicode__(self):
        return self.full_description()

    class Meta:
        ordering = ['country', 'name']
        verbose_name = _('HOm1 hospital')
        verbose_name_plural = _('HOm2 hospitals')
