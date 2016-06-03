#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib import admin

from wp4.compare.admin import VersionControlAdmin
from .models import QualityOfLife, ResourceLog, ResourceVisit, ResourceHospitalAdmission, ResourceRehabilitation


class QualityOfLifeAdmin(VersionControlAdmin):
    fields = (
        'recipient',
        'date_recorded',
        'qol_mobility',
        'qol_selfcare',
        'qol_usual_activities',
        'qol_pain',
        'qol_anxiety',
        'vas_score',
    )

admin.site.register(QualityOfLife, QualityOfLifeAdmin)


class ResourceVisitInline(admin.TabularInline):
    model = ResourceVisit
    fields = ('type', )
    can_delete = True
    extra = 1


class ResourceHospitalAdmissionInline(admin.TabularInline):
    model = ResourceHospitalAdmission
    fields = ('reason', 'had_surgery', 'days_in_itu', 'days_in_hospital')
    can_delete = True
    extra = 1


class ResourceRehabilitationInline(admin.TabularInline):
    model = ResourceRehabilitation
    fields = ('reason', 'days_in_hospital')
    can_delete = True
    extra = 1


class ResourceLogAdmin(VersionControlAdmin):
    fields = (
        'recipient',
        'date_given',
        'date_returned',
        'notes',
    )
    inlines = [
        ResourceVisitInline,
        ResourceHospitalAdmissionInline,
        ResourceRehabilitationInline
    ]

admin.site.register(ResourceLog, ResourceLogAdmin)
