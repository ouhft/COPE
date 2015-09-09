from django.contrib import admin
from django.utils import timezone

# Register your models here.
from .models import StaffJob, StaffPerson, Hospital, RetrievalTeam, Sample, Donor, PerfusionMachine, Recipient, AdverseEvent
from .models import PerfusionFile, Organ, ProcurementResource


class VersionControlAdmin(admin.ModelAdmin):
    exclude = ('version', 'created_on', 'created_by', 'record_locked')

    def save_model(self, request, obj, form, change):
        # TODO: this will have to respect the version control work later...
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.version += 1
        obj.save()


class HospitalAdmin(admin.ModelAdmin):
    exclude = ('created_on', 'created_by')

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()

admin.site.register(Hospital, HospitalAdmin)


class StaffPersonAdmin(VersionControlAdmin):
    pass

admin.site.register(StaffPerson, StaffPersonAdmin)


class RetrievalTeamAdmin(admin.ModelAdmin):
    exclude = ('created_on', 'created_by')

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()

admin.site.register(RetrievalTeam, RetrievalTeamAdmin)


class SampleAdmin(admin.ModelAdmin):
    exclude = ('created_on', 'created_by')

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()

admin.site.register(Sample, SampleAdmin)


class DonorAdmin(VersionControlAdmin):
    fieldsets = [
        ('Case information', {'fields': ['sequence_number', 'multiple_recipients']}),
        ('Trial Procedure', {'fields': [
            'retrieval_team', 'perfusion_technician', 'transplant_coordinator', 'call_received', 'retrieval_hospital',
            'scheduled_start', 'technician_arrival', 'ice_boxes_filled', 'depart_perfusion_centre',
            'arrival_at_donor_hospital'
        ]}),
        ('Donor Details', {'fields': [
            'number', 'date_of_birth', 'age', 'date_of_admission', 'admitted_to_itu', 'date_admitted_to_itu',
            'date_of_procurement', 'gender', 'weight', 'height', 'ethnicity', 'blood_group',
            'other_organs_procured','other_organs_lungs','other_organs_pancreas', 'other_organs_liver', 'other_organs_tissue'
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
        ('Donor Samples', {'fields': [
            'donor_blood_1_EDTA', 'donor_blood_1_SST', 'donor_urine_1', 'donor_urine_2'
        ]})
    ]

admin.site.register(Donor, DonorAdmin)


class PerfusionMachineAdmin(admin.ModelAdmin):
    exclude = ('created_on', 'created_by')

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()

admin.site.register(PerfusionMachine, PerfusionMachineAdmin)


class ProcurementResourceInline(admin.TabularInline):
    model = ProcurementResource
    extra = 2
    exclude = ('created_on', 'created_by')


class OrganAdmin(VersionControlAdmin):
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
        ]}),
        ('Organ Samples', {'fields': [
            'perfusate_1', 'perfusate_2', 'perfusate_3'
        ]})
    ]
    inlines = [ProcurementResourceInline]

    def save_formset(self, request, form, formset, change):
        if formset.model == ProcurementResource:
            instances = formset.save(commit=False)
            for instance in instances:
                instance.created_by = request.user
                instance.created_on = timezone.now()
                instance.save()
        else:
            formset.save()

admin.site.register(Organ, OrganAdmin)


class RecipientAdmin(VersionControlAdmin):
    pass

admin.site.register(Recipient, RecipientAdmin)


class AdverseEventAdmin(VersionControlAdmin):
    pass

admin.site.register(AdverseEvent, AdverseEventAdmin)
