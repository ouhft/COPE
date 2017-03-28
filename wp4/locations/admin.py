#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib import admin

from wp4.compare.admin import AuditedModelAdmin
from .models import Hospital


@admin.register(Hospital)
class HospitalAdmin(AuditedModelAdmin):
    list_display = ('name', 'country', 'is_active', 'is_project_site')
    fields = AuditedModelAdmin.fields + (
        'name',
        'country',
        'is_active',
        'is_project_site'
    )
