#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals


from django.contrib import admin
from wp4.compare.admin import BaseModelAdmin
from .models import Machine


class PerfusionMachineAdmin(BaseModelAdmin):
    fields = ('machine_serial_number', 'machine_reference_number')

admin.site.register(Machine, PerfusionMachineAdmin)
