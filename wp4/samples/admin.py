#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib import admin

from wp4.compare.admin import BaseModelAdmin, AuditedModelAdmin
from .models import UrineSample, BloodSample, PerfusateSample, TissueSample, Event, WP7Record

# HIGHLY NON-PERFORMANT!!! Needs to be reworked to be even vaguely usable in the admin
admin.site.register(Event)
admin.site.register(UrineSample)
admin.site.register(PerfusateSample)
admin.site.register(TissueSample)


@admin.register(BloodSample)
class BloodSampleAdmin(AuditedModelAdmin):
    # Currently far far too slow, so needs a custom form to speed up loading
    list_display = [
        'id',
        'barcode',
        'collected',
        'blood_type',
        'person',
        'centrifuged_at',
        # 'notes',
        # 'wp7_location' -- results in KeyError Exception value: manager
    ]
    list_filter = ('collected', 'blood_type', )
    fields = AuditedModelAdmin.fields + (
        'barcode',
        'collected',
        'blood_type',
        'person',
        'centrifuged_at',
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
