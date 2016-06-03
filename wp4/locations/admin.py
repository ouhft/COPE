#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib import admin

from wp4.compare.admin import VersionControlAdmin
from .models import Hospital


class HospitalAdmin(VersionControlAdmin):
    list_display = ('name', 'country', 'is_active', 'is_project_site')

admin.site.register(Hospital, HospitalAdmin)
