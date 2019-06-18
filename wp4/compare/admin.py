#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib import admin
from django.utils import timezone
from reversion_compare.admin import CompareVersionAdmin

# Register your models here.
from .models import Patient, RetrievalTeam, Donor, Recipient, Organ
from .models import ProcurementResource, OrganAllocation, Randomisation


# CORE Admin classes used throughout the whole system
class BaseModelAdmin(admin.ModelAdmin):
    actions_on_top = True
    actions_on_bottom = True
    save_on_top = True
    empty_value_display = '-not recorded-'


class VersionedModelAdmin(CompareVersionAdmin):
    actions_on_top = True
    actions_on_bottom = True
    save_on_top = True
    empty_value_display = '-not recorded-'


class AuditedModelAdmin(CompareVersionAdmin):
    actions_on_top = True
    actions_on_bottom = True
    save_on_top = True
    empty_value_display = '-not recorded-'

    fields = (('live', 'record_locked'), )

    actions = ['lock_records', 'unlock_records']

    def lock_records(self, request, queryset):
        queryset.update(record_locked=True)
    lock_records.short_description = "Lock the selected records"
    lock_records.allowed_permissions = ('delete', )

    def unlock_records(self, request, queryset):
        queryset.update(record_locked=False)
    unlock_records.short_description = "Unlock the selected records"
    unlock_records.allowed_permissions = ('delete', )

    def get_queryset(self, request):
        """
        Returns a QuerySet of all model instances that can be edited by the
        admin site. Includes those that are soft deleted by LiveField, and thus allows
        the admin site to hard delete those records.

        Based on django/contrib/admin/options.py: BaseModelAdmin.get_queryset
        """
        qs = self.model.all_objects.get_queryset()
        ordering = self.get_ordering(request)
        if ordering:
            qs = qs.order_by(*ordering)
        return qs


# =========== Models.Core ===============
@admin.register(Patient)
class PatientAdmin(AuditedModelAdmin):
    list_display = (
        'id',
        'trial_id',
        'live',
        'record_locked',
        'number',
        'gender',
        'age_from_dob',
        'is_alive',
        'recipient',
        'donor'
    )
    ordering = ('id',)
    fields = AuditedModelAdmin.fields + (
        'number',
        'date_of_birth',
        'date_of_birth_unknown',
        'date_of_death',
        'date_of_death_unknown',
        'gender',
        'weight',
        'height',
        'ethnicity',
        'blood_group'
    )


@admin.register(Randomisation)
class RandomisationAdmin(CompareVersionAdmin):
    list_display = ('id', 'list_code', 'donor', 'result', 'allocated_on', 'allocated_by')
    ordering = ('list_code', 'id',)
    fields = ('result', 'list_code', ('donor', 'allocated_on', 'allocated_by'))
    date_hierarchy = 'allocated_on'
    list_filter = ('list_code', 'result', ('allocated_by', admin.RelatedOnlyFieldListFilter),)


@admin.register(RetrievalTeam)
class RetrievalTeamAdmin(CompareVersionAdmin):
    list_display = ('centre_code', 'based_at', )
    ordering = ('centre_code',)
    fields = ('centre_code', 'based_at')


# =========== Models.Donor ===============
@admin.register(Donor)
class DonorAdmin(AuditedModelAdmin):
    list_display = (
        'id',
        'trial_id',
        'live',
        'record_locked',
        'person',
        'retrieval_team',
        'perfusion_technician',
        'is_randomised',
        'is_offline',
        'count_of_eligible_organs',
        'procurement_form_completed',
        'admin_notes'
    )
    list_per_page = 50
    list_select_related = True
    ordering = ('retrieval_team', 'trial_id')
    fields = None
    fieldsets = [
        ('Core', {'fields': AuditedModelAdmin.fields, }),
        ('Case information', {'fields': [
            'procurement_form_completed',
            'admin_notes',
            'trial_id',
            'sequence_number',
            'multiple_recipients',
            ('not_randomised_because', 'not_randomised_because_other'),
        ]}),
        ('Trial Procedure', {'fields': [
            'retrieval_team',
            'perfusion_technician',
            'transplant_coordinator',
            ('call_received', 'call_received_unknown'),
            'retrieval_hospital',
            ('scheduled_start', 'scheduled_start_unknown'),
            ('technician_arrival', 'technician_arrival_unknown'),
            ('ice_boxes_filled', 'ice_boxes_filled_unknown'),
            ('depart_perfusion_centre', 'depart_perfusion_centre_unknown'),
            ('arrival_at_donor_hospital', 'arrival_at_donor_hospital_unknown')
        ]}),
        ('Donor Details', {'fields': [
            'person',
            'age',
            ('date_of_admission', 'date_of_admission_unknown'),
            ('admitted_to_itu', 'date_admitted_to_itu', 'date_admitted_to_itu_unknown'),
            'date_of_procurement',
            ('other_organs_procured', 'other_organs_lungs', 'other_organs_pancreas',
             'other_organs_liver','other_organs_tissue')
        ]}),
        ('PreOp Data', {'fields': [
            ('diagnosis', 'diagnosis_other'),
            'diabetes_melitus',
            'alcohol_abuse',
            'hypertension',
            'cardiac_arrest',
            ('systolic_blood_pressure', 'diastolic_blood_pressure'),
            ('diuresis_last_day', 'diuresis_last_day_unknown'),
            ('diuresis_last_hour', 'diuresis_last_hour_unknown'),
            'dopamine',
            'dobutamine',
            'nor_adrenaline',
            'vasopressine',
            'other_medication_details'
        ]}),
        ('Lab Results', {'fields': [
            ('last_creatinine', 'last_creatinine_unit'),
            ('max_creatinine', 'max_creatinine_unit'),
            ('panel_reactive_antibodies', 'panel_reactive_antibodies_unknown')
        ]}),
        ('Operation Data', {'fields': [
            'life_support_withdrawal',
            ('systolic_pressure_low', 'systolic_pressure_low_unknown'),
            ('o2_saturation', 'o2_saturation_unknown'),
            ('circulatory_arrest', 'circulatory_arrest_unknown'),
            'length_of_no_touch',
            'death_diagnosed',
            ('perfusion_started', 'perfusion_started_unknown'),
            ('systemic_flush_used', 'systemic_flush_used_other'),
            'systemic_flush_volume_used',
            'heparin'
        ]}),
    ]

    def get_queryset(self, request):
        return super(DonorAdmin, self).get_queryset(request=request).select_related(
            'person',
            'retrieval_team',
            'retrieval_team__based_at',
            'perfusion_technician',
            'transplant_coordinator'
        )


class ProcurementResourceInline(admin.TabularInline):
    model = ProcurementResource
    fields = ('type', 'lot_number', ('expiry_date', 'expiry_date_unknown'), 'live')
    can_delete = False
    extra = 0


@admin.register(Organ)
class OrganAdmin(AuditedModelAdmin):
    list_display = (
        'id',
        'trial_id',
        'live',
        'record_locked',
        'location',
        'transplantable',
        'is_allocated',
        'explain_is_allocated',
        'reallocation_count',
        'transplantation_form_completed',
        'admin_notes',
    )
    ordering = ('id', )
    fields = None
    fieldsets = [
        ('Core', {'fields': AuditedModelAdmin.fields, }),
        ('Context', {'fields': [
            'donor',
            'location',
        ]}),
        ('Transplantation Form metadata', {'fields': [
            'admin_notes',
            'not_allocated_reason',
            'transplantation_form_completed',
            'transplantation_notes',
            'included_for_analysis',
            'excluded_from_analysis_because',
            'intention_to_treat',
            'actual_treatment_received'
        ]}),
        ('Inspection', {'fields': [
            'removal',
            'renal_arteries',
            ('graft_damage', 'graft_damage_other', ),
            'washout_perfusion',
            ('transplantable', 'not_transplantable_reason')
        ]}),
        ('Randomisation', {'fields': [
            'preservation'
        ]}),
        ('Perfusion', {'fields': [
            ('perfusion_possible', 'perfusion_not_possible_because', ),
            'perfusion_started',
            'patch_holder',
            ('artificial_patch_used', 'artificial_patch_size', 'artificial_patch_number'),
            'oxygen_bottle_full',
            'oxygen_bottle_open',
            ('oxygen_bottle_changed', 'oxygen_bottle_changed_at', 'oxygen_bottle_changed_at_unknown'),
            ('ice_container_replenished', 'ice_container_replenished_at', 'ice_container_replenished_at_unknown'),
            ('perfusate_measurable', 'perfusate_measure'),
            'perfusion_machine'
        ]})
    ]
    inlines = [
        ProcurementResourceInline
    ]


# =========== Models.Transplantation ===============
@admin.register(OrganAllocation)
class OrganAllocationAdmin(AuditedModelAdmin):
    ordering = ('organ__pk', 'id')
    list_display = (
        'id',
        'organ',
        'live',
        'record_locked',
        'reallocated',
        'transplant_hospital',
        'perfusion_technician',
    )
    fields = AuditedModelAdmin.fields + (
        'organ',
        'perfusion_technician',
        ('call_received', 'call_received_unknown'),
        'transplant_hospital',
        'theatre_contact',
        ('scheduled_start', 'scheduled_start_unknown'),
        ('technician_arrival', 'technician_arrival_unknown'),
        ('depart_perfusion_centre', 'depart_perfusion_centre_unknown'),
        ('arrival_at_recipient_hospital', 'arrival_at_recipient_hospital_unknown'),
        'journey_remarks',
        'reallocated',
        ('reallocation_reason', 'reallocation_reason_other'),
        'reallocation'
    )


@admin.register(Recipient)
class RecipientAdmin(AuditedModelAdmin):
    list_display = (
        'id',
        'trial_id',
        'live',
        'record_locked',
        'person',
        'allocation',
        'signed_consent',
        'single_kidney_transplant',
        'successful_conclusion',
    )
    list_select_related = True
    ordering = ('organ__trial_id', )
    fields = None
    fieldsets = [
        ('Core', {'fields': AuditedModelAdmin.fields, }),
        ('Case information', {'fields': [
            ('person', 'organ', 'allocation'),
            'signed_consent',
            'single_kidney_transplant',
        ]}),
        ('Recipient Details', {'fields': [
            ('renal_disease', 'renal_disease_other'),
            'pre_transplant_diuresis',
        ]}),
        ('Peri-operative data', {'fields': [
            'knife_to_skin',
            'perfusate_measure',
            'perfusion_stopped',
            'organ_cold_stored',
            'tape_broken',
            'removed_from_machine_at',
            'oxygen_full_and_open',
            ('organ_untransplantable', 'organ_untransplantable_reason'),
            'anesthesia_started_at',
            'incision',
            'transplant_side',
            ('arterial_problems', 'arterial_problems_other'),
            ('venous_problems', 'venous_problems_other'),
            ('anastomosis_started_at', 'anastomosis_started_at_unknown'),
            ('reperfusion_started_at', 'reperfusion_started_at_unknown'),
            'mannitol_used',
            ('other_diurectics', 'other_diurectics_details'),
            'systolic_blood_pressure',
            'cvp',
            'intra_operative_diuresis',
            'successful_conclusion',
            'operation_concluded_at',
        ]}),
        ('Machine cleanup record', {'fields': [
            'probe_cleaned',
            'ice_removed',
            'oxygen_flow_stopped',
            'oxygen_bottle_removed',
            'box_cleaned',
            'batteries_charged',
            'cleaning_log'
        ]}),
    ]

    def get_queryset(self, request):
        return super(RecipientAdmin, self).get_queryset(request=request).select_related(
            'person',
            'organ',
            'allocation',
        )

