from django.forms import ModelForm
from .models import Donor, Organ
from django.utils import timezone


class DonorForm(ModelForm):
    class Meta:
        model = Donor
        fields = [
            'retrieval_team', 'perfusion_technician', 'transplant_coordinator', 'call_received', 'retrieval_hospital',
            'scheduled_start', 'technician_arrival', 'ice_boxes_filled', 'depart_perfusion_centre',
            'arrival_at_donor_hospital',

            'number', 'date_of_birth', 'age', 'date_of_admission', 'admitted_to_itu', 'date_admitted_to_itu',
            'date_of_procurement', 'gender', 'weight', 'height', 'ethnicity', 'blood_group',

            'diagnosis', 'diagnosis_other', 'diabetes_melitus', 'alcohol_abuse', 'cardiac_arrest',
            'systolic_blood_pressure', 'diastolic_blood_pressure', 'diuresis_last_day', 'diuresis_last_day_unknown',
            'diuresis_last_hour', 'diuresis_last_hour_unknown', 'dopamine', 'dobutamine', 'nor_adrenaline',
            'vasopressine', 'other_medication_details',

            'last_creatinine', 'last_creatinine_unit', 'max_creatinine', 'max_creatinine_unit',

            'life_support_withdrawal', 'systolic_pressure_low', 'circulatory_arrest', 'length_of_no_touch',
            'death_diagnosed', 'perfusion_started', 'systemic_flush_used', 'systemic_flush_used_other',
            'heparin',

            'donor_blood_1_EDTA', 'donor_blood_1_SST', 'donor_urine_1', 'donor_urine_2',
        ]

    def save(self, user):
        donor = super(DonorForm, self).save(commit=False)
        donor.created_by = user
        donor.created_on = timezone.now()
        donor.version += 1
        donor.save()
        return donor


class DonorStartForm(ModelForm):
    class Meta:
        model = Donor
        fields = ['retrieval_team', 'perfusion_technician', 'age', 'number']

    def save(self, user):
        donor = super(DonorStartForm, self).save(commit=False)
        donor.created_by = user
        donor.created_on = timezone.now()
        donor.version = 1
        donor.save()
        return donor


class OrganForm(ModelForm):
    class Meta:
        model = Organ
        fields = [
            'donor', 'location',

            'removal', 'renal_arteries', 'graft_damage', 'washout_perfusion', 'transplantable',
            'not_transplantable_reason',

            'preservation',

            'perfusion_possible', 'perfusion_not_possible_because', 'perfusion_started', 'patch_holder',
            'artificial_patch_used', 'artificial_patch_size', 'artificial_patch_number', 'oxygen_bottle_full',
            'oxygen_bottle_open', 'oxygen_bottle_changed', 'oxygen_bottle_changed_at', 'ice_container_replenished',
            'ice_container_replenished_at', 'perfusate_measurable', 'perfusate_measure', 'perfusion_machine',
            'perfusion_file',

            'perfusate_1', 'perfusate_2',
    ]

    def save(self, user):
        organ = super(OrganForm, self).save(commit=False)
        organ.created_by = user
        organ.created_on = timezone.now()
        organ.version += 1
        organ.save()
        return organ
