#!/usr/bin/python
# coding: utf-8
from django import forms
from django.conf import settings
from django.utils import timezone
from django.contrib.auth.forms import AuthenticationForm
from django.utils.translation import ugettext_lazy as _, ungettext_lazy as __

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, Submit, Div, HTML, Field
from crispy_forms.bootstrap import FormActions, StrictButton
from autocomplete_light import ModelChoiceField

from ..theme.layout import InlineFields, FieldWithFollowup, YesNoFieldWithAlternativeFollowups, FieldWithNotKnown, \
    DateTimeField, DateField, FormPanel, DATETIME_INPUT_FORMATS, DATE_INPUT_FORMATS
from .models import Donor, Organ, Recipient, ProcurementResource
from .models import YES_NO_UNKNOWN_CHOICES, LOCATION_CHOICES


NO_YES_CHOICES = (
    (False, _("FF01 No")),
    (True, _("FF02 Yes"))
)

YES_NO_CHOICES = (
    (True, _("FF02 Yes")),
    (False, _("FF01 No"))
)


class DonorForm(forms.ModelForm):
    layout_1 = Layout(
        Field('retrieval_team', template="bootstrap3/layout/read-only.html"),
        'sequence_number',  # TODO: Work out how to hide this field if not admin
        Field('perfusion_technician', template="bootstrap3/layout/read-only.html"),
        'transplant_coordinator',
        DateTimeField('call_received'),
        'retrieval_hospital',
        DateTimeField('scheduled_start'),  # -- Not needed for UK, identical to withdrawal
        DateTimeField('technician_arrival'),  # -- Not needed for UK, identical to withdrawal
        DateTimeField('ice_boxes_filled'),  # -- Not needed for UK, identical to withdrawal
        DateTimeField('depart_perfusion_centre'),
        DateTimeField('arrival_at_donor_hospital'),
    )
    layout_other_organs = Layout(
        Field('other_organs_lungs', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('other_organs_pancreas', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('other_organs_liver', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('other_organs_tissue', template="bootstrap3/layout/radioselect-buttons.html"),
    )
    layout_2 = Layout(
        Field('number', placeholder="___ ___ ____"),
        FieldWithNotKnown(DateField('date_of_birth', notknown=True), 'date_of_birth_unknown'),
        # DateField('date_of_birth'),
        'age',
        DateField('date_of_admission'),
        FieldWithFollowup(
            Field('admitted_to_itu', template="bootstrap3/layout/radioselect-buttons.html"),
            DateField('date_admitted_to_itu')
        ),
        DateField('date_of_procurement'),
        Field('gender', template="bootstrap3/layout/read-only.html"),
        # Field('gender', template="bootstrap3/layout/radioselect-buttons.html"),
        'weight', 'height',
        Field('ethnicity', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('blood_group', template="bootstrap3/layout/radioselect-buttons.html"),
        FieldWithFollowup(
            Field('other_organs_procured', template="bootstrap3/layout/radioselect-buttons.html"),
            layout_other_organs
        )
    )
    layout_3 = Layout(
        FieldWithFollowup('diagnosis', 'diagnosis_other'),
        Field('diabetes_melitus', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('alcohol_abuse', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('cardiac_arrest', template="bootstrap3/layout/radioselect-buttons.html"),
        'systolic_blood_pressure', 'diastolic_blood_pressure',
        FieldWithNotKnown('diuresis_last_day', 'diuresis_last_day_unknown'),
        FieldWithNotKnown('diuresis_last_hour', 'diuresis_last_hour_unknown'),
        Field('dopamine', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('dobutamine', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('nor_adrenaline', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('vasopressine', template="bootstrap3/layout/radioselect-buttons.html"),
        'other_medication_details',
    )
    layout_4 = Layout(
        InlineFields('last_creatinine', 'last_creatinine_unit'),
        InlineFields('max_creatinine', 'max_creatinine_unit')
    )
    layout_6 = Layout(
        DateTimeField('life_support_withdrawal'),
        DateTimeField('systolic_pressure_low'),
        DateTimeField('o2_saturation'),
        DateTimeField('circulatory_arrest'),
        'length_of_no_touch',
        DateTimeField('death_diagnosed'),
        DateTimeField('perfusion_started'),
        FieldWithFollowup(
            Field('systemic_flush_used', template="bootstrap3/layout/radioselect-buttons.html"),
            'systemic_flush_used_other'
        ),
        'systemic_flush_volume_used',
        Field('heparin', template="bootstrap3/layout/radioselect-buttons.html"),
    )
    layout_5 = Layout(
        InlineFields(
            Field('donor_blood_1_EDTA', template="bootstrap3/layout/read-only.html"),
            StrictButton('<i class="glyphicon glyphicon-edit"></i>', css_class='btn-default', data_toggle="modal",
                         data_target="#myModal", title="Add/Edit Sample", css_id="button_db1"),
            label=_('DO91 db 1.1 edta')
        ),
        InlineFields(
            Field('donor_blood_1_SST', template="bootstrap3/layout/read-only.html"),
            StrictButton('<i class="glyphicon glyphicon-edit"></i>', css_class='btn-default', data_toggle="modal",
                         data_target="#myModal", title="Add/Edit Sample", css_id="button_db2"),
            label=_('DO92 db 1.2 sst')
        ),
        InlineFields(
            Field('donor_urine_1', template="bootstrap3/layout/read-only.html"),
            StrictButton('<i class="glyphicon glyphicon-edit"></i>', css_class='btn-default', data_toggle="modal",
                         data_target="#myModal", title="Add/Edit Sample", css_id="button_du1"),
            label=_('DO93 du 1')
        ),
        InlineFields(
            Field('donor_urine_2', template="bootstrap3/layout/read-only.html"),
            StrictButton('<i class="glyphicon glyphicon-edit"></i>', css_class='btn-default', data_toggle="modal",
                         data_target="#myModal", title="Add/Edit Sample", css_id="button_du2"),
            label=_('DO94 du 2')
        ),
    )

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Div(
        Div(
            FormPanel("Procedure Data", layout_1),
            FormPanel("Donor Preop Data", layout_3),
            # FormPanel("Sampling Data", layout_5),
            css_class="col-md-6", style="margin-top: 10px;"
        ),
        Div(
            FormPanel("Donor Details", layout_2),
            FormPanel("Lab Results", layout_4),
            FormPanel("Donor Procedure", layout_6),
            css_class="col-md-6", style="margin-top: 10px;"
        ),
        css_class='row'
    )

    retrieval_hospital = ModelChoiceField('HospitalAutoComplete')  # TODO: Add the label and text from model

    def __init__(self, *args, **kwargs):
        super(DonorForm, self).__init__(*args, **kwargs)
        self.fields['retrieval_team'].widget = forms.HiddenInput()
        self.fields['sequence_number'].widget = forms.HiddenInput()
        self.fields['multiple_recipients'].choices = NO_YES_CHOICES
        self.fields['perfusion_technician'].widget = forms.HiddenInput()
        self.fields['call_received'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['scheduled_start'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['technician_arrival'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['ice_boxes_filled'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['depart_perfusion_centre'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['arrival_at_donor_hospital'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['number'].required = False
        self.fields['date_of_birth'].input_formats = DATE_INPUT_FORMATS
        self.fields['date_of_admission'].input_formats = DATE_INPUT_FORMATS
        self.fields['admitted_to_itu'].choices = NO_YES_CHOICES
        self.fields['date_admitted_to_itu'].input_formats = DATE_INPUT_FORMATS
        self.fields['date_of_procurement'].input_formats = DATE_INPUT_FORMATS
        # self.fields['gender'].choices = Donor.GENDER_CHOICES
        self.fields['gender'].widget = forms.HiddenInput()
        self.fields['ethnicity'].choices = Donor.ETHNICITY_CHOICES
        self.fields['blood_group'].choices = Donor.BLOOD_GROUP_CHOICES
        self.fields['other_organs_procured'].choices = NO_YES_CHOICES
        self.fields['other_organs_lungs'].choices = NO_YES_CHOICES
        self.fields['other_organs_pancreas'].choices = NO_YES_CHOICES
        self.fields['other_organs_liver'].choices = NO_YES_CHOICES
        self.fields['other_organs_tissue'].choices = NO_YES_CHOICES
        self.fields['diabetes_melitus'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['alcohol_abuse'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['cardiac_arrest'].choices = NO_YES_CHOICES
        self.fields['dopamine'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['dobutamine'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['nor_adrenaline'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['vasopressine'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['life_support_withdrawal'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['systolic_pressure_low'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['o2_saturation'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['circulatory_arrest'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['death_diagnosed'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['perfusion_started'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['systemic_flush_used'].choices = Donor.FLUSH_SOLUTION_CHOICES
        self.fields['heparin'].choices = NO_YES_CHOICES
        # self.fields[''].
        self.fields['donor_blood_1_EDTA'].widget = forms.HiddenInput()
        self.fields['donor_blood_1_SST'].widget = forms.HiddenInput()
        self.fields['donor_urine_1'].widget = forms.HiddenInput()
        self.fields['donor_urine_2'].widget = forms.HiddenInput()

    class Meta:
        model = Donor
        exclude = ['created_by', 'version', 'created_on']
        localized_fields = "__all__"

    def save(self, user):
        donor = super(DonorForm, self).save(commit=False)
        donor.created_by = user
        donor.created_on = timezone.now()
        donor.version += 1
        donor.save()
        return donor


class DonorStartForm(forms.ModelForm):
    perfusion_technician = ModelChoiceField('TechnicianAutoComplete')  # TODO: Add the label and text from model

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        Div('retrieval_team', 'age', css_class="col-md-6"),
        Div('perfusion_technician', Field('gender', template="bootstrap3/layout/radioselect-buttons.html"),
            css_class="col-md-6"),
    )

    def __init__(self, *args, **kwargs):
        super(DonorStartForm, self).__init__(*args, **kwargs)

    class Meta:
        model = Donor
        fields = ['retrieval_team', 'perfusion_technician', 'age', 'gender']
        localized_fields = '__all__'

    def save(self, user):
        donor = super(DonorStartForm, self).save(commit=False)
        donor.created_by = user
        donor.created_on = timezone.now()
        donor.version = 1
        donor.save()
        return donor


class OrganForm(forms.ModelForm):
    layout_1 = Layout(
        'donor',
        Field('location', template="bootstrap3/layout/read-only.html"),
        Field('preservation', template="bootstrap3/layout/read-only.html"),
    )

    layout_2 = Layout(
        'perfusate_1', 'perfusate_2',
    )

    layout_3 = Layout(
        FieldWithFollowup(
            Field('transplantable', template="bootstrap3/layout/radioselect-buttons.html"),
            'not_transplantable_reason'
        ),
        DateTimeField('removal'),
        'renal_arteries',
        FieldWithFollowup(
            'graft_damage',
            'graft_damage_other'
        ),
        Field('washout_perfusion', template="bootstrap3/layout/radioselect-buttons.html"),
    )

    layout_artificial_patches = Layout(
        Field('artificial_patch_size', template="bootstrap3/layout/radioselect-buttons.html"),
        'artificial_patch_number',

    )

    layout_perfusion_possible = Layout(
        'perfusion_machine',
        DateTimeField('perfusion_started'),
        Field('patch_holder', template="bootstrap3/layout/radioselect-buttons.html"),
        FieldWithFollowup(

            Field('artificial_patch_used', template="bootstrap3/layout/radioselect-buttons.html"),
            layout_artificial_patches
        ),
        Field('oxygen_bottle_full', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('oxygen_bottle_open', template="bootstrap3/layout/radioselect-buttons.html"),
        FieldWithFollowup(
            Field('oxygen_bottle_changed', template="bootstrap3/layout/radioselect-buttons.html"),
            DateTimeField('oxygen_bottle_changed_at')
        ),
        FieldWithFollowup(
            Field('ice_container_replenished', template="bootstrap3/layout/radioselect-buttons.html"),
            DateTimeField('ice_container_replenished_at')
        ),
        FieldWithFollowup(
            Field('perfusate_measurable', template="bootstrap3/layout/radioselect-buttons.html"),
            'perfusate_measure'
        )
    )

    layout_4 = Layout(
        YesNoFieldWithAlternativeFollowups(
            'perfusion_possible',
            'perfusion_not_possible_because',
            layout_perfusion_possible
        ),
    )

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Div(
        Div(
            FormPanel("Inspection", layout_3),
            FormPanel("Sampling Data", layout_2),
            FormPanel("Preset Data", layout_1),
            css_class="col-md-6", style="margin-top: 10px;"
        ),
        Div(
            FormPanel("Perfusion Data", layout_4),
            css_class="col-md-6", style="margin-top: 10px;"
        ),
        css_class='row'
    )

    def __init__(self, *args, **kwargs):
        super(OrganForm, self).__init__(*args, **kwargs)
        self.fields['donor'].widget = forms.HiddenInput()
        self.fields['location'].widget = forms.HiddenInput()
        self.fields['preservation'].widget = forms.HiddenInput()
        self.fields['removal'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['washout_perfusion'].choices = Organ.WASHOUT_PERFUSION_CHOICES
        self.fields['transplantable'].choices = YES_NO_CHOICES
        self.fields['perfusion_possible'].choices = YES_NO_CHOICES
        self.fields['perfusion_started'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['patch_holder'].choices = Organ.PATCH_HOLDER_CHOICES
        self.fields['artificial_patch_used'].choices = NO_YES_CHOICES
        self.fields['artificial_patch_size'].choices = Organ.ARTIFICIAL_PATCH_CHOICES
        self.fields['oxygen_bottle_full'].choices = NO_YES_CHOICES
        self.fields['oxygen_bottle_open'].choices = NO_YES_CHOICES
        self.fields['oxygen_bottle_changed'].choices = NO_YES_CHOICES
        self.fields['oxygen_bottle_changed_at'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['ice_container_replenished'].choices = NO_YES_CHOICES
        self.fields['ice_container_replenished_at'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['perfusate_measurable'].choices = NO_YES_CHOICES

    class Meta:
        model = Organ
        exclude = ['created_by', 'version', 'created_on', 'perfusion_file']
        localized_fields = "__all__"

    def save(self, user):
        organ = super(OrganForm, self).save(commit=False)
        organ.created_by = user
        organ.created_on = timezone.now()
        organ.version += 1
        organ.save()
        return organ


class ProcurementResourceForm(forms.ModelForm):
    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Div(
        Div(Div('type', style="padding: 0 5px;"), css_class="col-md-4"),
        Div(Div('lot_number', style="padding: 0 5px;"), css_class="col-md-3"),
        Div(Div(DateField('expiry_date'), style="padding: 0 5px;"), css_class="col-md-3"),
        Div(Div('DELETE', style="padding: 0 5px;"), css_class="col-md-2"),
        css_class="row"
    )

    def __init__(self, *args, **kwargs):
        super(ProcurementResourceForm, self).__init__(*args, **kwargs)
        self.fields['organ'].widget = forms.HiddenInput()
        self.fields['expiry_date'].input_formats = DATE_INPUT_FORMATS
        self.fields['created_by'].widget = forms.HiddenInput()

    class Meta:
        model = ProcurementResource
        exclude = ('created_on',)
        localized_fields = "__all__"


ProcurementResourceInlineFormSet = forms.models.inlineformset_factory(
    Organ,
    ProcurementResource,
    form=ProcurementResourceForm,
    max_num=len(ProcurementResource.TYPE_CHOICES),
    extra=1,
    can_delete=True
)


class LoginForm(AuthenticationForm):
    next = forms.CharField(
        initial=settings.LOGIN_REDIRECT_URL,
        widget=forms.HiddenInput()
    )

    helper = FormHelper()
    helper.layout = Layout(
        'username',
        'password',
        'next',
        FormActions(
            Submit('login', 'Login', css_class='btn-primary')
        )
    )

    class Meta:
        fields = '__all__'

    def __init__(self, *args, **kwargs):
        super(LoginForm, self).__init__(*args, **kwargs)


class RecipientForm(forms.ModelForm):
    perfusion_technician = ModelChoiceField('TechnicianAutoComplete')  # TODO: Add the label and text from model
    transplant_hospital = ModelChoiceField('HospitalAutoComplete')  # TODO: Add the label and text from model
    transplant_coordinator = ModelChoiceField(
        'TransplantCoordinatorAutoComplete')  # TODO: Add the label and text from model

    layout_reallocation = Layout(
        FieldWithFollowup(
            Field('reallocation_reason', template="bootstrap3/layout/radioselect-buttons.html"),
            'reallocation_reason_other',
        ),
        'reallocation_recipient'
    )

    layout_allocation = Layout(
        'perfusion_technician',
        DateTimeField('call_received'),
        'transplant_hospital',
        'transplant_coordinator',
        DateTimeField('scheduled_start'),
        DateTimeField('technician_arrival'),
        DateTimeField('depart_perfusion_centre'),
        DateTimeField('arrival_at_recipient_hospital'),
        'journey_remarks',
        FieldWithFollowup(
            Field('reallocated', template="bootstrap3/layout/radioselect-buttons.html"),
            layout_reallocation
        )

    )
    layout_recipient = Layout(
        Field('number', placeholder="___ ___ ____"),
        FieldWithNotKnown(DateField('date_of_birth', notknown=True), 'date_of_birth_unknown', label="Flibble"),
        Field('gender', template="bootstrap3/layout/radioselect-buttons.html"),
        'weight',
        'height',
        Field('ethnicity', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('blood_group', template="bootstrap3/layout/radioselect-buttons.html"),
        FieldWithFollowup('renal_disease', 'renal_disease_other'),
        'pre_transplant_diuresis'
    )
    layout_perioperative_transplantable = Layout(
        DateTimeField('anesthesia_started_at'),
        DateTimeField('knife_to_skin'),
        Field('incision', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('transplant_side', template="bootstrap3/layout/radioselect-buttons.html"),
        FieldWithFollowup(
            'arterial_problems',
            'arterial_problems_other'),
        FieldWithFollowup(
            'venous_problems',
            'venous_problems_other'),
        DateTimeField('anastomosis_started_at'),
        DateTimeField('reperfusion_started_at'),
        Field('mannitol_used', template="bootstrap3/layout/radioselect-buttons.html"),
        FieldWithFollowup(
            Field('other_diurectics', template="bootstrap3/layout/radioselect-buttons.html"),
            'other_diurectics_details'),
        'systolic_blood_pressure',
        'cvp',
        Field('intra_operative_diuresis', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('successful_conclusion', template="bootstrap3/layout/radioselect-buttons.html"),
        DateTimeField('operation_concluded_at')
    )
    layout_perioperative = Layout(
        'perfusate_measure',
        DateTimeField('perfusion_stopped'),
        Field('organ_cold_stored', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('tape_broken', template="bootstrap3/layout/radioselect-buttons.html"),
        DateTimeField('removed_from_machine_at'),
        Field('oxygen_full_and_open', template="bootstrap3/layout/radioselect-buttons.html"),

        YesNoFieldWithAlternativeFollowups(
            Field('organ_untransplantable', template="bootstrap3/layout/radioselect-buttons.html"),
            'organ_untransplantable_reason',
            layout_perioperative_transplantable
        )
    )
    layout_samples = Layout()
    layout_cleaning = Layout(
        Field('probe_cleaned', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('ice_removed', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('oxygen_flow_stopped', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('oxygen_bottle_removed', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('box_cleaned', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('batteries_charged', template="bootstrap3/layout/radioselect-buttons.html"),
        'cleaning_log'
    )

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Div(
        Div(
            FormPanel("Allocation Data", layout_allocation),
            FormPanel("Recipient Details", layout_recipient),
            FormPanel("Sampling Data", layout_samples),
            css_class="col-md-6", style="margin-top: 10px;"
        ),
        Div(
            FormPanel("Peri-Operative Data", layout_perioperative),
            FormPanel("Cleaning Log", layout_cleaning),
            css_class="col-md-6", style="margin-top: 10px;"
        ),
        css_class='row'
    )

    def __init__(self, *args, **kwargs):
        super(RecipientForm, self).__init__(*args, **kwargs)
        self.fields['organ'].widget = forms.HiddenInput()
        self.fields['perfusion_technician'].required = False
        self.fields['call_received'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['transplant_hospital'].required = False
        self.fields['transplant_coordinator'].required = False
        self.fields['scheduled_start'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['technician_arrival'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['depart_perfusion_centre'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['arrival_at_recipient_hospital'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['reallocated'].choices = NO_YES_CHOICES
        self.fields['reallocation_reason'].choices = Recipient.REALLOCATION_CHOICES
        self.fields['renal_disease'].choices = Recipient.RENAL_DISEASE_CHOICES
        self.fields['date_of_birth'].input_formats = DATE_INPUT_FORMATS
        self.fields['gender'].widget = forms.HiddenInput()
        self.fields['ethnicity'].choices = Recipient.ETHNICITY_CHOICES
        self.fields['blood_group'].choices = Recipient.BLOOD_GROUP_CHOICES

        self.fields['knife_to_skin'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['perfusion_stopped'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['organ_cold_stored'].choices = NO_YES_CHOICES
        self.fields['tape_broken'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['removed_from_machine_at'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['oxygen_full_and_open'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['organ_untransplantable'].choices = NO_YES_CHOICES
        self.fields['anesthesia_started_at'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['incision'].choices = Recipient.INCISION_CHOICES
        self.fields['transplant_side'].choices = LOCATION_CHOICES
        self.fields['transplant_side'].required = False
        self.fields['arterial_problems'].choices = Recipient.ARTERIAL_PROBLEM_CHOICES
        self.fields['venous_problems'].choices = Recipient.VENOUS_PROBLEM_CHOICES
        self.fields['anastomosis_started_at'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['reperfusion_started_at'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['mannitol_used'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['other_diurectics'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['intra_operative_diuresis'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['successful_conclusion'].choices = NO_YES_CHOICES
        self.fields['operation_concluded_at'].input_formats = DATETIME_INPUT_FORMATS

        self.fields['probe_cleaned'].choices = NO_YES_CHOICES
        self.fields['ice_removed'].choices = NO_YES_CHOICES
        self.fields['oxygen_flow_stopped'].choices = NO_YES_CHOICES
        self.fields['oxygen_bottle_removed'].choices = NO_YES_CHOICES
        self.fields['box_cleaned'].choices = NO_YES_CHOICES
        self.fields['batteries_charged'].choices = NO_YES_CHOICES

    class Meta:
        model = Recipient
        exclude = ['created_by', 'version', 'created_on', 'record_locked']
        localized_fields = "__all__"

    def save(self, user):
        recipient = super(RecipientForm, self).save(commit=False)
        recipient.created_by = user
        recipient.created_on = timezone.now()
        recipient.version += 1
        recipient.save()
        return recipient
