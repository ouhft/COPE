#!/usr/bin/python
# coding: utf-8
from django.contrib import admin

from .models import AdverseEvent


class VersionControlAdmin(admin.ModelAdmin):
    exclude = ('version', 'created_on', 'created_by', 'record_locked')

    def save_model(self, request, obj, form, change):
        obj.save(created_by=request.user)


class AdverseEventAdmin(VersionControlAdmin):
    pass

admin.site.register(AdverseEvent, AdverseEventAdmin)
