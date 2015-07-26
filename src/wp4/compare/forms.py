from django.forms import ModelForm, RadioSelect, CharField, HiddenInput
from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, Submit
from crispy_forms.bootstrap import FormActions
from .models import Donor, Organ
from django.utils import timezone
from django.contrib.auth.forms import AuthenticationForm
from django.contrib.auth import REDIRECT_FIELD_NAME
from wp4.settings import LOGIN_REDIRECT_URL


class DonorForm(ModelForm):
    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True

    class Meta:
        model = Donor
        fields = [
            'retrieval_team', 
            'perfusion_technician', 
            'transplant_coordinator', 
            'call_received', 
            'retrieval_hospital',
            # 'scheduled_start',  -- Not needed for UK, identical to withdrawal 
            # 'technician_arrival', 
            # 'ice_boxes_filled', 
            'depart_perfusion_centre',
            'arrival_at_donor_hospital',

            'number', 'date_of_birth', 'age', 'date_of_admission', 'admitted_to_itu', 'date_admitted_to_itu',
            'date_of_procurement', 'gender', 'weight', 'height', 'ethnicity', 'blood_group',

            'diagnosis', 'diagnosis_other', 'diabetes_melitus', 'alcohol_abuse', 'cardiac_arrest',
            'systolic_blood_pressure', 'diastolic_blood_pressure', 'diuresis_last_day', 'diuresis_last_day_unknown',
            'diuresis_last_hour', 'diuresis_last_hour_unknown', 'dopamine', 'dobutamine', 'nor_adrenaline',
            'vasopressine', 'other_medication_details',

            'last_creatinine', 'last_creatinine_unit', 'max_creatinine', 'max_creatinine_unit',

            'life_support_withdrawal', 'systolic_pressure_low', 'o2_saturation', 'circulatory_arrest', 'length_of_no_touch',
            'death_diagnosed', 'perfusion_started', 'systemic_flush_used', 'systemic_flush_used_other',
            'heparin',

            'donor_blood_1_EDTA', 'donor_blood_1_SST', 'donor_urine_1', 'donor_urine_2',
        ]
        widgets = {
            # 'name': Textarea(attrs={'cols': 80, 'rows': 20}),
            # 'admitted_to_itu' : RadioSelect(),
        }
        localized_fields = ('__all__')
    #     https://docs.djangoproject.com/en/1.8/topics/forms/modelforms/#overriding-the-default-fields
    labels = {}
    help_texts = {}
    error_messages = {}

    def save(self, user):
        donor = super(DonorForm, self).save(commit=False)
        donor.created_by = user
        donor.created_on = timezone.now()
        donor.version += 1
        donor.save()
        return donor


class DonorStartForm(ModelForm):
    # gender = CharField(widget=RadioSelect(choices=Donor.GENDER_CHOICES))

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True

    class Meta:
        model = Donor
        fields = ['retrieval_team', 'perfusion_technician', 'age', 'gender']
        widgets = {
            # 'name': Textarea(attrs={'cols': 80, 'rows': 20}),
            # 'gender': RadioSelect(choices=Donor.GENDER_CHOICES)
        }
        localized_fields = ('__all__')

    def save(self, user):
        donor = super(DonorStartForm, self).save(commit=False)
        donor.created_by = user
        donor.created_on = timezone.now()
        donor.version = 1
        donor.save()
        return donor


class OrganForm(ModelForm):
    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True

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


class LoginForm(AuthenticationForm):
    redirect_to = CharField(
        initial=LOGIN_REDIRECT_URL,
        widget=HiddenInput()
    )

    helper = FormHelper()
    helper.layout = Layout(
        'username',
        'password',
        'redirect_to',
        FormActions(
            Submit('login', 'Login', css_class='btn-primary')
        )
    )

    class Meta:
        fields = '__all__'

    def __init__(self, *args, **kwargs):
        super(LoginForm, self).__init__(*args, **kwargs)

