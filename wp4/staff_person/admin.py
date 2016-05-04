#!/usr/bin/python
# coding: utf-8
from django.contrib import admin
from wp4.compare.admin import VersionControlAdmin
from .models import StaffPerson


class StaffPersonAdmin(VersionControlAdmin):
    list_display = ('id', 'first_names', 'last_names', 'user', 'telephone', 'email', 'based_at')

admin.site.register(StaffPerson, StaffPersonAdmin)
