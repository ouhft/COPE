from django.contrib import admin
from django.utils import timezone

# Register your models here.
from .models import Person, Hospital, RetrievalTeam, Sample, Donor, OrgansOffered, PerfusionMachine
from .models import PerfusionFile, Organ, ProcurementResource

class VersionControlAdmin(admin.ModelAdmin):
    def save_model(self, request, obj, form, change):
        # TODO: this will have to respect the version control work later...
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.version += 1
        obj.save()


class PersonAdmin(VersionControlAdmin):
    fields = ['first_names', 'last_names', 'job', 'telephone', 'user']


class HospitalAdmin(admin.ModelAdmin):
    fields = ['centre_code', 'name', 'country', 'is_active']

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()


class RetrievalTeamAdmin(admin.ModelAdmin):
    fields = ['name', 'based_at']

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()


class SampleAdmin(admin.ModelAdmin):
    fields = ['barcode', 'taken_at', 'centrifugation', 'comment']

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()


class OrgansOfferedInline(admin.TabularInline):
    model = OrgansOffered
    extra = 1
    fields = ['donor', 'organ']


class DonorAdmin(VersionControlAdmin):
    fieldsets = [
        ('Trial Procedure', {'fields': [
            'retrieval_team', 'perfusion_technician', 'transplant_coordinator', 'call_received', 'retrieval_hospital',
            'scheduled_start', 'technician_arrival', 'ice_boxes_filled', 'depart_perfusion_centre',
            'arrival_at_donor_hospital'
        ]}),
        ('Donor Details', {'fields': [
            'number', 'date_of_birth', 'age', 'date_of_admission', 'admitted_to_itu', 'date_admitted_to_itu',
            'date_of_procurement', 'gender', 'weight', 'height', 'ethnicity', 'blood_group'
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
        ('Donor Procedure', {'fields': [
            'life_support_withdrawal', 'systolic_pressure_low', 'circulatory_arrest', 'length_of_no_touch',
            'death_diagnosed', 'perfusion_started', 'systemic_flush_used', 'systemic_flush_used_other',
            'heparin'
        ]}),
        ('Donor Samples', {'fields': [
            'donor_blood_1_EDTA', 'donor_blood_1_SST', 'donor_urine_1', 'donor_urine_2'
        ]})
    ]
    inlines = [OrgansOfferedInline]

    def save_formset(self, request, form, formset, change):
        if formset.model == OrgansOffered:
            instances = formset.save(commit=False)
            for instance in instances:
                instance.created_by = request.user
                instance.created_on = timezone.now()
                instance.save()
        else:
            formset.save()


class PerfusionMachineAdmin(admin.ModelAdmin):
    fields = ['machine_serial_number', 'machine_reference_number']

    def save_model(self, request, obj, form, change):
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.save()


class ProcurementResourceInline(admin.TabularInline):
    model = ProcurementResource
    extra = 2
    fields = ['organ', 'type', 'lot_number', 'expiry_date']


class OrganAdmin(VersionControlAdmin):
    fieldsets = [
        ('Context', {'fields': [
            'donor', 'location'
        ]}),
        ('Inspection', {'fields': [
            'removal', 'renal_arteries', 'graft_damage', 'washout_perfusion', 'transplantable',
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
            'perfusate_1', 'perfusate_2'
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



admin.site.register(Person, PersonAdmin)
admin.site.register(Hospital, HospitalAdmin)
admin.site.register(RetrievalTeam, RetrievalTeamAdmin)
admin.site.register(Sample, SampleAdmin)
admin.site.register(Donor, DonorAdmin)
admin.site.register(PerfusionMachine, PerfusionMachineAdmin)
admin.site.register(Organ, OrganAdmin)
