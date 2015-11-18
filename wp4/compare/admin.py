from django.contrib import admin
from django.utils import timezone

# Register your models here.
from .models import OrganPerson, RetrievalTeam, Donor, PerfusionMachine, PerfusionFile, Recipient, \
    Organ, ProcurementResource, OrganAllocation


class VersionControlAdmin(admin.ModelAdmin):
    exclude = ('version', 'created_on', 'created_by', 'record_locked')

    def save_model(self, request, obj, form, change):
        # TODO: this will have to respect the version control work later...
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.version += 1
        obj.save()


class RetrievalTeamAdmin(admin.ModelAdmin):
    list_display = ('based_at', 'centre_code')
    ordering = ('centre_code',)
    fields = ('centre_code', 'based_at')

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()

admin.site.register(RetrievalTeam, RetrievalTeamAdmin)


class PerfusionMachineFileInline(admin.TabularInline):
    model = PerfusionFile
    fields = ('file',)


class PerfusionMachineAdmin(admin.ModelAdmin):
    fields = ('machine_serial_number', 'machine_reference_number')
    inlines = [
        PerfusionMachineFileInline,
    ]

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()

admin.site.register(PerfusionMachine, PerfusionMachineAdmin)


class OrganPersonAdmin(VersionControlAdmin):
    list_display = ('__unicode__', 'number', 'gender', 'age_from_dob', 'date_of_death', 'recipient', 'donor')
    ordering = ('id',)
    fields = (
        'number', 'date_of_birth', 'date_of_birth_unknown', 'date_of_death', 'date_of_death_unknown',
        'gender', 'weight', 'height', 'ethnicity', 'blood_group')

admin.site.register(OrganPerson, OrganPersonAdmin)


class DonorAdmin(VersionControlAdmin):
    list_display = ('__unicode__', 'person', 'sequence_number', 'retrieval_team', 'is_randomised', 'trial_id')
    fieldsets = [
        ('Case information', {'fields': ['sequence_number', 'multiple_recipients']}),
        ('Trial Procedure', {'fields': [
            'retrieval_team', 'perfusion_technician', 'transplant_coordinator', 'call_received', 'retrieval_hospital',
            'scheduled_start', 'technician_arrival', 'ice_boxes_filled', 'depart_perfusion_centre',
            'arrival_at_donor_hospital'
        ]}),
        ('Donor Details', {'fields': [
            'person',
            'age', 'date_of_admission', 'admitted_to_itu', 'date_admitted_to_itu',
            'date_of_procurement', 'other_organs_procured',
            'other_organs_lungs', 'other_organs_pancreas', 'other_organs_liver', 'other_organs_tissue'
        ]}),
        ('PreOp Data', {'fields': [
            'diagnosis', 'diagnosis_other', 'diabetes_melitus', 'alcohol_abuse', 'cardiac_arrest',
            'systolic_blood_pressure', 'diastolic_blood_pressure', 'diuresis_last_day', 'diuresis_last_day_unknown',
            'diuresis_last_hour', 'diuresis_last_hour_unknown', 'dopamine', 'dobutamine', 'nor_adrenaline',
            'vasopressine', 'other_medication_details'
        ]}),
        ('Lab Results', {'fields': [
            'last_creatinine', 'last_creatinine_unit', 'max_creatinine', 'max_creatinine_unit'
        ]}),
        ('Operation Data', {'fields': [
            'life_support_withdrawal', 'systolic_pressure_low', 'o2_saturation', 'circulatory_arrest', 'length_of_no_touch',
            'death_diagnosed', 'perfusion_started', 'systemic_flush_used', 'systemic_flush_used_other',
            'systemic_flush_volume_used', 'heparin'
        ]}),
    ]

admin.site.register(Donor, DonorAdmin)


class ProcurementResourceInline(admin.TabularInline):
    model = ProcurementResource
    exclude = ('created_on', 'created_by')
    can_delete = True


class OrganAdmin(VersionControlAdmin):
    list_display = ('__unicode__', 'location', 'transplantable', 'donor')
    ordering = ('donor__id', 'location')
    fieldsets = [
        ('Context', {'fields': [
            'donor', 'location'
        ]}),
        ('Inspection', {'fields': [
            'removal', 'renal_arteries', 'graft_damage', 'graft_damage_other', 'washout_perfusion', 'transplantable',
            'not_transplantable_reason'
        ]}),
        ('Randomisation', {'fields': [
            'preservation'
        ]}),
        ('Perfusion', {'fields': [
            'perfusion_possible', 'perfusion_not_possible_because', 'perfusion_started', 'patch_holder',
            'artificial_patch_used', 'artificial_patch_size', 'artificial_patch_number', 'oxygen_bottle_full',
            'oxygen_bottle_open', 'oxygen_bottle_changed', 'oxygen_bottle_changed_at', 'ice_container_replenished',
            'ice_container_replenished_at', 'perfusate_measurable', 'perfusate_measure', 'perfusion_machine',
            'perfusion_file'
        ]})
    ]
    inlines = [
        ProcurementResourceInline
    ]

    def save_formset(self, request, form, formset, change):
        if formset.model == ProcurementResource:
            for subform in formset:
                subform.instance.created_by = request.user
                subform.instance.created_on = timezone.now()
        formset.save()

admin.site.register(Organ, OrganAdmin)


class OrganAllocationAdmin(VersionControlAdmin):
    ordering = ('organ__pk', 'created_on')
    list_display = ('__unicode__', 'organ', 'perfusion_technician', 'transplant_hospital', 'reallocated', 'reallocation')

admin.site.register(OrganAllocation, OrganAllocationAdmin)


class RecipientAdmin(VersionControlAdmin):
    list_display = ('__unicode__', 'person', 'organ', 'allocation', 'signed_consent', 'successful_conclusion')
    ordering = ('organ__pk', 'created_on')

admin.site.register(Recipient, RecipientAdmin)
