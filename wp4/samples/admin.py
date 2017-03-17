#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib import admin
from django.contrib.admin.options import IS_POPUP_VAR
from django.utils import timezone

from wp4.compare.admin import VersionControlAdmin
from .models import UrineSample, BloodSample, PerfusateSample, TissueSample, Event


class BloodSampleInline(admin.TabularInline):
    model = BloodSample
    fields = ('collected', 'barcode', 'blood_type', 'centrifuged_at', 'notes')
    can_delete = False
    extra = 2
    max_num = 2
    min_num = 2


class UrineSampleInline(admin.TabularInline):
    model = UrineSample
    fields = ('collected', 'barcode', 'centrifuged_at', 'notes')
    can_delete = False
    extra = 1
    max_num = 1
    min_num = 1


class PerfusateSampleInline(admin.TabularInline):
    model = PerfusateSample
    fields = ('collected', 'barcode', 'organ', 'centrifuged_at', 'notes')
    can_delete = False
    extra = 1
    max_num = 1
    min_num = 1


class TissueSampleInline(admin.TabularInline):
    model = TissueSample
    fields = ('collected', 'barcode', 'organ', 'tissue_type', 'notes')
    can_delete = False
    extra = 2
    max_num = 2
    min_num = 2


class EventAdmin(VersionControlAdmin):
    fields = ('type', 'name', 'taken_at')
    list_display = ('id', 'type', 'name', 'taken_at')
    ordering = ('type', 'taken_at')

    inlines = []  # Overrided by the get_inline_instances function below

    def get_inline_instances(self, request, obj=None):
        if obj is None:
            self.inlines = []
        elif obj.type == Event.TYPE_BLOOD:
            self.inlines = [BloodSampleInline]
        elif obj.type == Event.TYPE_URINE:
            self.inlines = [UrineSampleInline]
        elif obj.type == Event.TYPE_PERFUSATE:
            self.inlines = [PerfusateSampleInline]
        elif obj.type == Event.TYPE_TISSUE:
            self.inlines = [TissueSampleInline]
        return super(EventAdmin, self).get_inline_instances(request, obj)

    def get_readonly_fields(self, request, obj=None):
        if obj:  # editing an existing object
            return self.readonly_fields + ('type',)
        return self.readonly_fields

    def response_add(self, request, obj, post_url_continue=None):
        """
        Determines the HttpResponse for the add_view stage. It mostly defers to
        its superclass implementation but is customized because the Event model
        has a slightly different workflow.
        """
        # We should allow further modification of the event just added i.e. the
        # 'Save' button should behave like the 'Save and continue editing'
        # button except in two scenarios:
        # * The user has pressed the 'Save and add another' button
        # * We are adding an event in a popup
        if '_addanother' not in request.POST and IS_POPUP_VAR not in request.POST:
            request.POST['_continue'] = 1
        return super(EventAdmin, self).response_add(request, obj, post_url_continue)

    # def save_formset(self, request, form, formset, change):
    #     # Overrides VersionControlAdmin.save_formset(). Doesn't allow deletion.
    #     if formset.model == UrineSample:
    #         for subform in formset:
    #             # subform.instance.person = form.instance.worksheet.person
    #             subform.instance.created_by = request.user
    #             subform.instance.created_on = timezone.now()
    #     if formset.model == BloodSample:
    #         for subform in formset:
    #             # subform.instance.person = form.instance.worksheet.person
    #             subform.instance.created_by = request.user
    #             subform.instance.created_on = timezone.now()
    #     if formset.model == TissueSample:
    #         for subform in formset:
    #             subform.instance.created_by = request.user
    #             subform.instance.created_on = timezone.now()
    #     if formset.model == PerfusateSample:
    #         for subform in formset:
    #             subform.instance.created_by = request.user
    #             subform.instance.created_on = timezone.now()
    #     formset.save()

admin.site.register(Event, EventAdmin)
