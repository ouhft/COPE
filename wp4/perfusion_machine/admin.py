#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals


from django.contrib import admin
from wp4.compare.admin import BaseModelAdmin
from .models import PerfusionMachine, PerfusionFile


class PerfusionMachineFileInline(admin.TabularInline):
    model = PerfusionFile
    fields = ('file',)
    extra = 1


class PerfusionMachineAdmin(BaseModelAdmin):
    fields = ('machine_serial_number', 'machine_reference_number')
    inlines = [
        PerfusionMachineFileInline,
    ]

admin.site.register(PerfusionMachine, PerfusionMachineAdmin)


class PerfusionMachineFileAdmin(BaseModelAdmin):
    fields = ('machine', 'file')

admin.site.register(PerfusionFile, PerfusionMachineFileAdmin)