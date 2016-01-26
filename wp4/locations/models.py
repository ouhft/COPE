#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.db import models
from django.contrib.auth.models import User
from django.core.urlresolvers import reverse
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _


# Common CONSTANTS
UNITED_KINGDOM = 1
BELGIUM = 4
NETHERLANDS = 5
COUNTRY_CHOICES = (
    (UNITED_KINGDOM, _('LOc01 United Kingdom')),
    (BELGIUM, _('LOc02 Belgium')),
    (NETHERLANDS, _('LOc03 Netherlands')),
    (10, _("LOc04 Albania")),
    (11, _("LOc05 Andorra")),
    (12, _("LOc06 Armenia")),
    (13, _("LOc07 Austria")),
    (14, _("LOc08 Azerbaijan")),
    (15, _("LOc09 Belarus")),
    (16, _("LOc10 Bosnia and Herzegovina")),
    (17, _("LOc11 Bulgaria")),
    (18, _("LOc12 Croatia")),
    (19, _("LOc13 Cyprus")),
    (20, _("LOc14 Czech Republic")),
    (21, _("LOc15 Denmark")),
    (22, _("LOc16 Estonia")),
    (23, _("LOc17 Finland")),
    (24, _("LOc18 France")),
    (25, _("LOc19 Georgia")),
    (26, _("LOc20 Germany")),
    (27, _("LOc21 Greece")),
    (28, _("LOc22 Hungary")),
    (29, _("LOc23 Iceland")),
    (30, _("LOc24 Ireland")),
    (31, _("LOc25 Italy")),
    (32, _("LOc26 Kazakhstan")),
    (33, _("LOc27 Kosovo")),
    (34, _("LOc28 Latvia")),
    (35, _("LOc29 Liechtenstein")),
    (36, _("LOc30 Lithuania")),
    (37, _("LOc31 Luxembourg")),
    (38, _("LOc32 Macedonia")),
    (39, _("LOc33 Malta")),
    (40, _("LOc34 Moldova")),
    (41, _("LOc35 Monaco")),
    (42, _("LOc36 Montenegro")),
    (43, _("LOc37 Norway")),
    (44, _("LOc38 Poland")),
    (45, _("LOc39 Portugal")),
    (46, _("LOc40 Romania")),
    (47, _("LOc41 Russia")),
    (48, _("LOc42 San Marino")),
    (49, _("LOc43 Serbia")),
    (50, _("LOc44 Slovakia")),
    (51, _("LOc45 Slovenia")),
    (52, _("LOc46 Spain")),
    (53, _("LOc47 Sweden")),
    (54, _("LOc48 Switzerland")),
    (55, _("LOc49 Turkey")),
    (56, _("LOc50 Ukraine")),
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

    def get_absolute_url(self):
        return reverse("locations:detail", kwargs={"pk": self.pk})
