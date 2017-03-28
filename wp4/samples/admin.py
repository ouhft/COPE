#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib import admin

from .models import UrineSample, BloodSample, PerfusateSample, TissueSample, Event

# HIGHLY NON-PERFORMANT!!! Needs to be reworked to be even vaguely usable in the admin
admin.site.register(Event)
admin.site.register(BloodSample)
admin.site.register(UrineSample)
admin.site.register(PerfusateSample)
admin.site.register(TissueSample)
