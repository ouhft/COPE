#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib import admin
from reversion_compare.admin import CompareVersionAdmin

from .models import Person


class PersonAdmin(CompareVersionAdmin):
    actions_on_top = True
    actions_on_bottom = True
    save_on_top = True

    list_display = ('id', 'first_name', 'last_name', 'telephone', 'email', 'based_at')

admin.site.register(Person, PersonAdmin)
