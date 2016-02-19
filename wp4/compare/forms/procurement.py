#!/usr/bin/python
# coding: utf-8
from django import forms
from django.conf import settings
from django.utils.translation import ugettext_lazy as _

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, Div, HTML, Field

from wp4.theme.layout import InlineFields, FieldWithFollowup, YesNoFieldWithAlternativeFollowups
from wp4.theme.layout import FieldWithNotKnown, ForeignKeyModal
from wp4.theme.layout import DateTimeField, DateField, FormPanel
from ..models import Donor, Organ, ProcurementResource
from ..models import YES_NO_UNKNOWN_CHOICES
from .core import NO_YES_CHOICES, YES_NO_CHOICES


class DonorForm(forms.ModelForm):
    layout_procedure = Layout(
        Field('retrieval_team', template="bootstrap3/layout/read-only.html"),
        'sequence_number',
        Field('perfusion_technician', template="bootstrap3/layout/read-only.html"),
        ForeignKeyModal('transplant_coordinator'),
        FieldWithNotKnown(
            DateTimeField('call_received', notknown=True),
            'call_received_unknown',
            label=Donor._meta.get_field("call_received").verbose_name.title()
        ),
        ForeignKeyModal('retrieval_hospital'),
        FieldWithNotKnown(
            DateTimeField('scheduled_start', notknown=True),
            'scheduled_start_unknown',
            label=Donor._meta.get_field("scheduled_start").verbose_name.title()
        ),
        FieldWithNotKnown(
            DateTimeField('technician_arrival', notknown=True),
            'technician_arrival_unknown',
            label=Donor._meta.get_field("technician_arrival").verbose_name.title()
        ),
        FieldWithNotKnown(
            DateTimeField('ice_boxes_filled', notknown=True),
            'ice_boxes_filled_unknown',
            label=Donor._meta.get_field("ice_boxes_filled").verbose_name.title()
        ),
        FieldWithNotKnown(
            DateTimeField('depart_perfusion_centre', notknown=True),
            'depart_perfusion_centre_unknown',
            label=Donor._meta.get_field("depart_perfusion_centre").verbose_name.title()
        ),
        FieldWithNotKnown(
            DateTimeField('arrival_at_donor_hospital', notknown=True),
            'arrival_at_donor_hospital_unknown',
            label=Donor._meta.get_field("arrival_at_donor_hospital").verbose_name.title()
        ),
        Field('multiple_recipients', template="bootstrap3/layout/radioselect-buttons.html")
    )
    layout_other_organs = Layout(
        Field('other_organs_lungs', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('other_organs_pancreas', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('other_organs_liver', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('other_organs_tissue', template="bootstrap3/layout/radioselect-buttons.html")
    )
    layout_donor_details = Layout(
        'age',
        FieldWithNotKnown(
            DateField('date_of_admission', notknown=True),
            'date_of_admission_unknown',
            label=Donor._meta.get_field("date_of_admission").verbose_name.title()
        ),
        FieldWithFollowup(
            Field('admitted_to_itu', template="bootstrap3/layout/radioselect-buttons.html"),
            FieldWithNotKnown(
                DateField('date_admitted_to_itu', notknown=True),
                'date_admitted_to_itu_unknown',
                label=Donor._meta.get_field("date_admitted_to_itu").verbose_name.title()
            ),
        ),
        DateField('date_of_procurement'),
        FieldWithFollowup(
            Field('other_organs_procured', template="bootstrap3/layout/radioselect-buttons.html"),
            layout_other_organs
        )
    )
    layout_preop = Layout(
        FieldWithFollowup('diagnosis', 'diagnosis_other'),
        Field('diabetes_melitus', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('alcohol_abuse', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('cardiac_arrest', template="bootstrap3/layout/radioselect-buttons.html"),
        'systolic_blood_pressure',
        'diastolic_blood_pressure',
        FieldWithNotKnown('diuresis_last_day', 'diuresis_last_day_unknown'),
        FieldWithNotKnown('diuresis_last_hour', 'diuresis_last_hour_unknown'),
        Field('dopamine', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('dobutamine', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('nor_adrenaline', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('vasopressine', template="bootstrap3/layout/radioselect-buttons.html"),
        'other_medication_details'
    )
    layout_labresults = Layout(
        InlineFields('last_creatinine', 'last_creatinine_unit'),
        InlineFields('max_creatinine', 'max_creatinine_unit')
    )
    layout_donor_procedure = Layout(
        DateTimeField('life_support_withdrawal'),
        FieldWithNotKnown(
            DateTimeField('systolic_pressure_low', notknown=True),
            'systolic_pressure_low_unknown',
            label=Donor._meta.get_field("systolic_pressure_low").verbose_name.title()
        ),
        FieldWithNotKnown(
            DateTimeField('o2_saturation', notknown=True),
            'o2_saturation_unknown',
            label=Donor._meta.get_field("o2_saturation").verbose_name.title()
        ),
        FieldWithNotKnown(
            DateTimeField('circulatory_arrest', notknown=True),
            'circulatory_arrest_unknown',
            label=Donor._meta.get_field("circulatory_arrest").verbose_name.title()
        ),
        'length_of_no_touch',
        DateTimeField('death_diagnosed'),
        FieldWithNotKnown(
            DateTimeField('perfusion_started', notknown=True),
            'perfusion_started_unknown',
            label=Donor._meta.get_field("perfusion_started").verbose_name.title()
        ),
        FieldWithFollowup(
            Field('systemic_flush_used', template="bootstrap3/layout/radioselect-buttons.html"),
            'systemic_flush_used_other'
        ),
        'systemic_flush_volume_used',
        Field('heparin', template="bootstrap3/layout/radioselect-buttons.html")
    )
    layout_almost_complete = Layout(
        'not_randomised_because',
        'not_randomised_because_other',
        HTML(
            "<p class=\"text-danger\">Once all errors have been cleared, clicking Save And Close " +
            "below will result in this form being closed and locked. No further edits will be possible " +
            "without contacting the admin team.</p>"
        )
    )
    layout_complete = Layout(
        FieldWithFollowup(
            Field('procurement_form_completed', template="bootstrap3/layout/radioselect-buttons.html"),
            layout_almost_complete
        )
    )

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        FormPanel("Donor Details", layout_donor_details),
        HTML("</div>"),
        Div(
            FormPanel("Procedure Data", layout_procedure),
            FormPanel("Donor Procedure", layout_donor_procedure),
            css_class="col-md-4", style="margin-top: 10px;"
        ),
        Div(
            FormPanel("Donor Preop Data", layout_preop),
            FormPanel("Lab Results", layout_labresults),
            FormPanel("Complete Submission", layout_complete, panel_status="danger"),
            css_class="col-md-4", style="margin-top: 10px;"
        ),
        'person',
    )

    def __init__(self, *args, **kwargs):
        super(DonorForm, self).__init__(*args, **kwargs)
        self.fields['person'].widget = forms.HiddenInput()
        self.fields['retrieval_team'].widget = forms.HiddenInput()
        self.fields['sequence_number'].widget = forms.HiddenInput()
        self.fields['perfusion_technician'].widget = forms.HiddenInput()
        self.fields['call_received'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['scheduled_start'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['technician_arrival'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['ice_boxes_filled'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['depart_perfusion_centre'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['arrival_at_donor_hospital'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['multiple_recipients'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['date_of_admission'].input_formats = settings.DATE_INPUT_FORMATS
        self.fields['admitted_to_itu'].choices = NO_YES_CHOICES
        self.fields['date_admitted_to_itu'].input_formats = settings.DATE_INPUT_FORMATS
        self.fields['date_of_procurement'].input_formats = settings.DATE_INPUT_FORMATS
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
        self.fields['life_support_withdrawal'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['systolic_pressure_low'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['o2_saturation'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['circulatory_arrest'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['death_diagnosed'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['perfusion_started'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['systemic_flush_used'].choices = Donor.SOLUTION_CHOICES
        self.fields['heparin'].choices = NO_YES_CHOICES
        self.fields['procurement_form_completed'].choices = NO_YES_CHOICES

    class Meta:
        model = Donor
        fields = [
            'person', 'sequence_number', 'multiple_recipients', 'retrieval_team', 'perfusion_technician',
            'transplant_coordinator', 'call_received', 'call_received_unknown', 'retrieval_hospital',
            'scheduled_start', 'scheduled_start_unknown', 'technician_arrival',
            'technician_arrival_unknown',
            'ice_boxes_filled', 'ice_boxes_filled_unknown', 'depart_perfusion_centre',
            'depart_perfusion_centre_unknown', 'arrival_at_donor_hospital',
            'arrival_at_donor_hospital_unknown', 'age',
            'date_of_admission', 'date_of_admission_unknown', 'admitted_to_itu', 'date_admitted_to_itu',
            'date_admitted_to_itu_unknown', 'date_of_procurement',
            'other_organs_procured', 'other_organs_lungs', 'other_organs_pancreas', 'other_organs_liver',
            'other_organs_tissue', 'diagnosis', 'diagnosis_other', 'diabetes_melitus', 'alcohol_abuse',
            'cardiac_arrest', 'systolic_blood_pressure', 'diastolic_blood_pressure', 'diuresis_last_day',
            'diuresis_last_day_unknown', 'diuresis_last_hour', 'diuresis_last_hour_unknown', 'dopamine',
            'dobutamine', 'nor_adrenaline', 'vasopressine', 'other_medication_details', 'last_creatinine',
            'last_creatinine_unit', 'max_creatinine', 'max_creatinine_unit', 'life_support_withdrawal',
            'systolic_pressure_low', 'systolic_pressure_low_unknown', 'o2_saturation',
            'o2_saturation_unknown', 'circulatory_arrest', 'circulatory_arrest_unknown',
            'length_of_no_touch',
            'death_diagnosed',
            'perfusion_started', 'perfusion_started_unknown',
            'systemic_flush_used', 'systemic_flush_used_other',
            'systemic_flush_volume_used', 'heparin',
            'procurement_form_completed', 'not_randomised_because', 'not_randomised_because_other'
        ]
        localized_fields = "__all__"

    def save(self, user=None, *args, **kwargs):
        donor = super(DonorForm, self).save(commit=False)
        if kwargs.get("commit", True):
            if user is None:
                raise Exception("Missing user record when saving DonorForm")
            donor.save(created_by=user)
        return donor

    def clean(self):
        cleaned_data = super(DonorForm, self).clean()
        form_completed = cleaned_data.get("procurement_form_completed")
        if form_completed:
            not_randomised_because = cleaned_data.get("not_randomised_because")
            if not_randomised_because == 0 and not self.instance.is_randomised:
                self.add_error(
                    'not_randomised_because',
                    forms.ValidationError(_("DFv01 Please enter a valid reason for this case to not be randomised"))
                )

            not_randomised_because_other = cleaned_data.get("not_randomised_because_other")
            if (not_randomised_because == 2 or not_randomised_because == 5) and not_randomised_because_other == '':
                self.add_error(
                    'not_randomised_because_other',
                    forms.ValidationError(_("DFv02 Please add additional information"))
                )

            retrieval_hospital = cleaned_data.get("retrieval_hospital")
            if not retrieval_hospital:
                self.add_error('retrieval_hospital', forms.ValidationError(_("DFv03 Missing retrieval hospital")))

        if self.errors:
            cleaned_data["procurement_form_completed"] = False

        return cleaned_data


class OrganForm(forms.ModelForm):
    layout_system_data = Layout(
        'donor',
        Field('location', template="bootstrap3/layout/read-only.html"),
        Field('preservation', template="bootstrap3/layout/read-only.html")
    )
    layout_inspection = Layout(
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
        Field('washout_perfusion', template="bootstrap3/layout/radioselect-buttons.html")
    )
    layout_artificial_patches = Layout(
        Field('artificial_patch_size', template="bootstrap3/layout/radioselect-buttons.html"),
        'artificial_patch_number'
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
        FieldWithFollowup(
            Field('oxygen_bottle_open', template="bootstrap3/layout/radioselect-buttons.html"),
            HTML(
                "<p class=\"text-warning\"><i class=\"glyphicon glyphicon-warning-sign\"></i> The " +
                "bottle should be open!</p>"
            )
        ),
        FieldWithFollowup(
            Field('oxygen_bottle_changed', template="bootstrap3/layout/radioselect-buttons.html"),
            FieldWithNotKnown(
                DateTimeField('oxygen_bottle_changed_at', notknown=True),
                'oxygen_bottle_changed_at_unknown',
                label=Organ._meta.get_field("oxygen_bottle_changed_at").verbose_name.title()
            ),
        ),
        FieldWithFollowup(
            Field('ice_container_replenished', template="bootstrap3/layout/radioselect-buttons.html"),
            FieldWithNotKnown(
                DateTimeField('ice_container_replenished_at', notknown=True),
                'ice_container_replenished_at_unknown',
                label=Organ._meta.get_field("ice_container_replenished_at").verbose_name.title()
            ),
        ),
        FieldWithFollowup(
            Field('perfusate_measurable', template="bootstrap3/layout/radioselect-buttons.html"),
            'perfusate_measure'
        )
    )
    layout_perfusion_not_possible = Layout(
        'perfusion_not_possible_because',
        HTML(
            "<p class=\"text-warning\"><i class=\"glyphicon glyphicon-warning-sign\"></i> Please " +
            "remember to enter resources used</p>"
        )
    )
    layout_perfusion = Layout(
        YesNoFieldWithAlternativeFollowups(
            'perfusion_possible',
            layout_perfusion_not_possible,
            layout_perfusion_possible
        )
    )

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        Div(
            FormPanel("Inspection", layout_inspection),
            FormPanel("Preset Data", layout_system_data),
            css_class="col-md-4",
            style="margin-top: 10px;"
        ),
        Div(
            FormPanel("Perfusion Data", layout_perfusion),
            css_class="col-md-4",
            style="margin-top: 10px;"
        )
    )

    def __init__(self, *args, **kwargs):
        super(OrganForm, self).__init__(*args, **kwargs)
        self.fields['donor'].widget = forms.HiddenInput()
        self.fields['location'].widget = forms.HiddenInput()
        self.fields['preservation'].widget = forms.HiddenInput()
        self.fields['removal'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['washout_perfusion'].choices = Organ.WASHOUT_PERFUSION_CHOICES
        self.fields['transplantable'].choices = YES_NO_CHOICES
        self.fields['perfusion_possible'].choices = YES_NO_CHOICES
        self.fields['perfusion_started'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['patch_holder'].choices = Organ.PATCH_HOLDER_CHOICES
        self.fields['artificial_patch_used'].choices = NO_YES_CHOICES
        self.fields['artificial_patch_size'].choices = Organ.ARTIFICIAL_PATCH_CHOICES
        self.fields['oxygen_bottle_full'].choices = NO_YES_CHOICES
        self.fields['oxygen_bottle_open'].choices = YES_NO_CHOICES
        self.fields['oxygen_bottle_changed'].choices = NO_YES_CHOICES
        self.fields['oxygen_bottle_changed_at'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['ice_container_replenished'].choices = NO_YES_CHOICES
        self.fields['ice_container_replenished_at'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['perfusate_measurable'].choices = NO_YES_CHOICES

    class Meta:
        model = Organ
        fields = [
            'donor', 'location', 'removal', 'renal_arteries', 'graft_damage', 'graft_damage_other',
            'washout_perfusion', 'transplantable', 'not_transplantable_reason', 'preservation',
            'perfusion_possible', 'perfusion_not_possible_because', 'perfusion_started', 'patch_holder',
            'artificial_patch_used', 'artificial_patch_size', 'artificial_patch_number',
            'oxygen_bottle_full', 'oxygen_bottle_open', 'oxygen_bottle_changed',
            'oxygen_bottle_changed_at',
            'oxygen_bottle_changed_at_unknown', 'ice_container_replenished',
            'ice_container_replenished_at',
            'ice_container_replenished_at_unknown', 'perfusate_measurable', 'perfusate_measure',
            'perfusion_machine'
        ]
        localized_fields = "__all__"

    def save(self, user=None, *args, **kwargs):
        organ = super(OrganForm, self).save(commit=False)
        if kwargs.get("commit", True):
            if user is None:
                raise Exception("Missing user record when saving OrganForm")
            organ.save(created_by=user)
        return organ


class ProcurementResourceForm(forms.ModelForm):
    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        'organ',
        Field('type', template="bootstrap3/layout/read-only.html"),
        'lot_number',
        FieldWithNotKnown(
            DateField('expiry_date', notknown=True),
            'expiry_date_unknown',
            label=ProcurementResource._meta.get_field("expiry_date").verbose_name.title()
        ),
        'created_by'
    )

    def __init__(self, *args, **kwargs):
        super(ProcurementResourceForm, self).__init__(*args, **kwargs)
        self.render_required_fields = True
        self.fields['organ'].widget = forms.HiddenInput()
        self.fields['type'].widget = forms.HiddenInput()
        self.fields['type'].choices = ProcurementResource.TYPE_CHOICES
        self.fields['expiry_date'].input_formats = settings.DATE_INPUT_FORMATS
        self.fields['created_by'].widget = forms.HiddenInput()

    class Meta:
        model = ProcurementResource
        fields = ('organ', 'type', 'lot_number', 'expiry_date', 'expiry_date_unknown', 'created_by')
        localized_fields = "__all__"


ProcurementResourceLeftInlineFormSet = forms.models.inlineformset_factory(
    Organ,
    ProcurementResource,
    form=ProcurementResourceForm,
    min_num=len(ProcurementResource.TYPE_CHOICES),
    validate_min=True,
    max_num=len(ProcurementResource.TYPE_CHOICES),
    validate_max=True,
    extra=0,
    can_delete=False
)
ProcurementResourceRightInlineFormSet = forms.models.inlineformset_factory(
    Organ,
    ProcurementResource,
    form=ProcurementResourceForm,
    min_num=len(ProcurementResource.TYPE_CHOICES),
    validate_min=True,
    max_num=len(ProcurementResource.TYPE_CHOICES),
    validate_max=True,
    extra=0,
    can_delete=False
)
