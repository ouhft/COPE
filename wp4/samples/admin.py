#!/usr/bin/python
# coding: utf-8
from django.contrib import admin
from django.contrib.admin.options import IS_POPUP_VAR
from django.utils import timezone

from .models import UrineSample, BloodSample, PerfusateSample, TissueSample, Event, Worksheet


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


class EventAdmin(admin.ModelAdmin):
    fields = ('worksheet', 'type', 'taken_at')

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

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()

    def save_formset(self, request, form, formset, change):
        if formset.model == UrineSample:
            for subform in formset:
                subform.instance.person = form.instance.worksheet.person
                subform.instance.created_by = request.user
                subform.instance.created_on = timezone.now()
        if formset.model == BloodSample:
            for subform in formset:
                subform.instance.person = form.instance.worksheet.person
                subform.instance.created_by = request.user
                subform.instance.created_on = timezone.now()
        if formset.model == TissueSample:
            for subform in formset:
                # TODO: Find a way to auto-populate organ from Worksheet person, if plausible
                subform.instance.created_by = request.user
                subform.instance.created_on = timezone.now()
        if formset.model == PerfusateSample:
            for subform in formset:
                # TODO: Find a way to auto-populate organ from Worksheet person, if plausible
                subform.instance.created_by = request.user
                subform.instance.created_on = timezone.now()
        formset.save()

admin.site.register(Event, EventAdmin)


class EventInline(admin.TabularInline):
    model = Event
    fields = ('worksheet', 'type', 'taken_at')
    can_delete = True
    extra = 1


class WorksheetAdmin(admin.ModelAdmin):
    list_display = ('id', 'barcode', 'person',)
    fields = ('barcode', 'person')
    inlines = [EventInline]

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()

    def save_formset(self, request, form, formset, change):
        if formset.model == Event:
            for subform in formset:
                subform.instance.created_by = request.user
                subform.instance.created_on = timezone.now()
        formset.save()

admin.site.register(Worksheet, WorksheetAdmin)
