#!/usr/bin/python
# coding: utf-8

from django.contrib import admin

from .models import PerfusionMachine, PerfusionFile


class PerfusionMachineFileInline(admin.TabularInline):
    model = PerfusionFile
    fields = ('file',)
    extra = 1


class PerfusionMachineAdmin(admin.ModelAdmin):
    fields = ('machine_serial_number', 'machine_reference_number')
    inlines = [
        PerfusionMachineFileInline,
    ]

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.save()

admin.site.register(PerfusionMachine, PerfusionMachineAdmin)


class PerfusionMachineFileAdmin(admin.ModelAdmin):
    fields = ('machine', 'file')

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.save()

admin.site.register(PerfusionFile, PerfusionMachineFileAdmin)