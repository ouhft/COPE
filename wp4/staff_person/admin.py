#!/usr/bin/python
# coding: utf-8
from django.contrib import admin
from django.utils import timezone

from .models import StaffPerson


class VersionControlAdmin(admin.ModelAdmin):
    exclude = ('version', 'created_on', 'created_by', 'record_locked')

    def save_model(self, request, obj, form, change):
        # TODO: this will have to respect the version control work later...
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.version += 1
        obj.save()


class StaffPersonAdmin(VersionControlAdmin):
    list_display = ('id', 'first_names', 'last_names', 'user', 'telephone', 'email', 'based_at')

admin.site.register(StaffPerson, StaffPersonAdmin)
