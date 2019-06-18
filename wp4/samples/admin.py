#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib import admin

from wp4.compare.admin import BaseModelAdmin, AuditedModelAdmin
from .models import UrineSample, BloodSample, PerfusateSample, TissueSample, Event, WP7Record

# NB: HIGHLY NON-PERFORMANT!!! Editing of specific Samples is very slow (>90 seconds)
admin.site.register(Event)  # TODO: This could be expanded for admin use


@admin.register(BloodSample)
class BloodSampleAdmin(AuditedModelAdmin):
    # Currently far far too slow, so needs a custom form to speed up loading
    list_display = [
        'id',
        'event',
        'live',
        'record_locked',
        'barcode',
        'collected',
        'blood_type',
        'person',
        'centrifuged_at',
        'notes',
        # 'wp7_location' -- results in KeyError Exception value: manager
    ]
    list_filter = ('collected', 'blood_type', 'record_locked', 'live')
    fields = AuditedModelAdmin.fields + (
        'event',
        'barcode',
        'collected',
        'blood_type',
        'person',
        'centrifuged_at',
        'notes'
    )

@admin.register(UrineSample)
class UrineSampleAdmin(AuditedModelAdmin):
    # Currently far far too slow, so needs a custom form to speed up loading
    list_display = [
        'id',
        'event',
        'live',
        'record_locked',
        'barcode',
        'collected',
        'person',
        'centrifuged_at',
        'notes',
    ]
    list_filter = ('collected', 'record_locked', 'live')
    fields = AuditedModelAdmin.fields + (
        'event',
        'barcode',
        'collected',
        'person',
        'centrifuged_at',
        'notes'
    )

@admin.register(PerfusateSample)
class PerfusateSampleAdmin(AuditedModelAdmin):
    # Currently far far too slow, so needs a custom form to speed up loading
    list_display = [
        'id',
        'event',
        'live',
        'record_locked',
        'barcode',
        'collected',
        'organ',
        'centrifuged_at',
        'notes',
    ]
    list_filter = ('collected', 'record_locked', 'live')
    fields = AuditedModelAdmin.fields + (
        'event',
        'barcode',
        'collected',
        'organ',
        'centrifuged_at',
        'notes'
    )


@admin.register(TissueSample)
class TissueSampleAdmin(AuditedModelAdmin):
    # Currently far far too slow, so needs a custom form to speed up loading
    list_display = [
        'id',
        'event',
        'live',
        'record_locked',
        'barcode',
        'collected',
        'tissue_type',
        'organ',
        'notes',
    ]
    list_filter = ('collected', 'tissue_type', 'record_locked', 'live')
    fields = AuditedModelAdmin.fields + (
        'event',
        'barcode',
        'collected',
        'tissue_type',
        'organ',
        'notes'
    )

@admin.register(WP7Record)
class WP7RecordAdmin(BaseModelAdmin):
    list_display = [
        'id',
        'barcode',
        'box_number',
        'position_in_box',
        'content_type',
        'object_id',
        'content_object'
    ]
    list_per_page = 100
    list_filter = (('content_type', admin.RelatedOnlyFieldListFilter),)
