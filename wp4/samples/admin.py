#!/usr/bin/python
# coding: utf-8
from django.contrib import admin
from django.utils import timezone

from .models import Event, UrineSample, BloodSample, PerfusateSample, TissueSample, Worksheet, Deviation


class EventAdmin(admin.ModelAdmin):
    exclude = ('created_on', 'created_by')

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()

admin.site.register(Event, EventAdmin)


class WorksheetAdmin(admin.ModelAdmin):
    exclude = ('created_on', 'created_by')

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()

admin.site.register(Worksheet, WorksheetAdmin)
# TODO: Add Deviation as an inline set of forms


class UrineSampleAdmin(admin.ModelAdmin):
    exclude = ('created_on', 'created_by')

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()

admin.site.register(UrineSample, UrineSampleAdmin)


class BloodSampleAdmin(admin.ModelAdmin):
    exclude = ('created_on', 'created_by')

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()

admin.site.register(BloodSample, BloodSampleAdmin)


class PerfusateSampleAdmin(admin.ModelAdmin):
    exclude = ('created_on', 'created_by')

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()

admin.site.register(PerfusateSample, PerfusateSampleAdmin)


class TissueSampleAdmin(admin.ModelAdmin):
    exclude = ('created_on', 'created_by')

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()

admin.site.register(TissueSample, TissueSampleAdmin)