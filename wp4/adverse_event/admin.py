#!/usr/bin/python
# coding: utf-8
from __future__ import unicode_literals

from django.contrib import admin

from wp4.compare.admin import AuditedModelAdmin
from .models import Event, Category


@admin.register(Event)
class AdverseEventAdmin(AuditedModelAdmin):
    list_select_related = True
    list_display = (
        'id',
        'organ',
        'live',
        'onset_at_date',
        'event_ongoing',
        'is_serious',
        'contact',
    )
    fields = AuditedModelAdmin.fields + (
        'organ',
        'serious_eligible_1',
        'serious_eligible_2',
        'serious_eligible_3',
        'serious_eligible_4',
        'serious_eligible_5',
        'serious_eligible_6',
        'onset_at_date',
        'event_ongoing',
        'description',
        'action',
        'outcome',
        'alive_query_1',
        'alive_query_2',
        'alive_query_3',
        'alive_query_4',
        'alive_query_5',
        'alive_query_6',
        'alive_query_7',
        'alive_query_8',
        'alive_query_9',
        'rehospitalisation',
        'date_of_admission',
        'date_of_discharge',
        'admitted_to_itu',
        'dialysis_needed',
        'biopsy_taken',
        'surgery_required',
        'rehospitalisation_comments',
        'death',
        'treatment_related',
        'cause_of_death_1',
        'cause_of_death_2',
        'cause_of_death_3',
        'cause_of_death_4',
        'cause_of_death_5',
        ('cause_of_death_6', 'cause_of_death_comment'),
        'contact',
    )

admin.site.register(Category)
