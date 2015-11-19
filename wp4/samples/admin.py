#!/usr/bin/python
# coding: utf-8
from django.contrib import admin
from django.utils import timezone

from .models import Event, UrineSample, BloodSample, PerfusateSample, TissueSample, Worksheet


# class EventAdmin(admin.ModelAdmin):
#     fields = ('type', 'taken_at')
#
#     def save_model(self, request, obj, form, change):
#         obj.created_by = request.user
#         obj.created_on = timezone.now()
#         obj.save()
#
# admin.site.register(Event, EventAdmin)

#
# class UrineSampleInline(admin.TabularInline):
#     model = UrineSample
#     fields = ('barcode', 'person', 'event', 'centrifuged_at', 'collected', 'notes')
#     can_delete = True
#     extra = 1
#
#
# class BloodSampleInline(admin.TabularInline):
#     model = BloodSample
#     fields = ('barcode', 'person', 'event', 'blood_type', 'centrifuged_at', 'collected', 'notes')
#     can_delete = True
#     extra = 1
#
#
# class PerfusateSampleInline(admin.TabularInline):
#     model = PerfusateSample
#     fields = ('barcode', 'organ', 'event', 'centrifuged_at', 'collected', 'notes')
#     can_delete = True
#     extra = 1
#
#
# class TissueSampleInline(admin.TabularInline):
#     model = TissueSample
#     fields = ('barcode', 'organ', 'event', 'tissue_type', 'collected', 'notes')
#     can_delete = True
#     extra = 1
#
#
# class WorksheetAdmin(admin.ModelAdmin):
#     list_display = ('id', 'barcode', 'person', 'person__is_donor', 'person__is_recipient')
#     fields = ('barcode', 'person')
#     inlines = [
#         UrineSampleInline,
#         BloodSampleInline,
#         PerfusateSampleInline,
#         TissueSampleInline
#     ]
#
#     def save_model(self, request, obj, form, change):
#         obj.created_by = request.user
#         obj.created_on = timezone.now()
#         obj.save()
#
#     def save_formset(self, request, form, formset, change):
#         if formset.model == UrineSample:
#             # print("DEBUG: self.instance is %s" % dir(self.instance))
#             for subform in formset:
#                 # TODO: Find a way to auto-populate person from Worksheet person
#                 # subform.instance.person = self.instance.person
#                 subform.instance.created_by = request.user
#                 subform.instance.created_on = timezone.now()
#         if formset.model == BloodSample:
#             for subform in formset:
#                 # TODO: Find a way to auto-populate person from Worksheet person
#                 # subform.instance.person = self.instance.person
#                 subform.instance.created_by = request.user
#                 subform.instance.created_on = timezone.now()
#         if formset.model == TissueSample:
#             for subform in formset:
#                 subform.instance.created_by = request.user
#                 subform.instance.created_on = timezone.now()
#         if formset.model == PerfusateSample:
#             for subform in formset:
#                 subform.instance.created_by = request.user
#                 subform.instance.created_on = timezone.now()
#         formset.save()
#
# admin.site.register(Worksheet, WorksheetAdmin)


# class UrineSampleAdmin(admin.ModelAdmin):
#     fields = ('worksheet', 'barcode', 'person', 'event', 'centrifuged_at', 'collected', 'notes')
#
#     def save_model(self, request, obj, form, change):
#         obj.created_by = request.user
#         obj.created_on = timezone.now()
#         obj.save()
#
# admin.site.register(UrineSample, UrineSampleAdmin)
#
#
# class BloodSampleAdmin(admin.ModelAdmin):
#     fields = ('worksheet', 'barcode', 'person', 'event', 'blood_type', 'centrifuged_at', 'collected', 'notes')
#
#     def save_model(self, request, obj, form, change):
#         obj.created_by = request.user
#         obj.created_on = timezone.now()
#         obj.save()
#
# admin.site.register(BloodSample, BloodSampleAdmin)
#
#
# class PerfusateSampleAdmin(admin.ModelAdmin):
#     fields = ('worksheet', 'barcode', 'organ', 'event', 'centrifuged_at', 'collected', 'notes')
#
#     def save_model(self, request, obj, form, change):
#         obj.created_by = request.user
#         obj.created_on = timezone.now()
#         obj.save()
#
# admin.site.register(PerfusateSample, PerfusateSampleAdmin)
#
#
# class TissueSampleAdmin(admin.ModelAdmin):
#     fields = ('worksheet', 'barcode', 'organ', 'event', 'tissue_type', 'collected', 'notes')
#
#     def save_model(self, request, obj, form, change):
#         obj.created_by = request.user
#         obj.created_on = timezone.now()
#         obj.save()
#
# admin.site.register(TissueSample, TissueSampleAdmin)