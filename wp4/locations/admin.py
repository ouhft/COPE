#!/usr/bin/python
# coding: utf-8
from django.contrib import admin
from django.utils import timezone
from reversion_compare.admin import CompareVersionAdmin

from .models import Hospital


class HospitalAdmin(CompareVersionAdmin):
    list_display = ('name', 'country', 'is_active', 'is_project_site')
    exclude = ('created_on', 'created_by')

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()

admin.site.register(Hospital, HospitalAdmin)
