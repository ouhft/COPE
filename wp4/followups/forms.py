#!/usr/bin/python
# coding: utf-8
from django import forms
from django.utils import timezone

from crispy_forms.helper import FormHelper
from crispy_forms.bootstrap import FormActions
from crispy_forms.layout import Layout, Div, Submit, Button

from ..theme.layout import DateTimeField, DATETIME_INPUT_FORMATS, DATE_INPUT_FORMATS, FormPanel
from .models import FollowUpInitial, FollowUp3M, FollowUp6M, FollowUp1Y


class FollowUpInitialForm(forms.ModelForm):
    layout_graft = Layout(
        'start_date',
        'graft_failure',
        'graft_failure_type',
        'graft_failure_type_other',
        'graft_failure_date',
        'graft_removal',
        'graft_removal_date',
    )
    layout_serum = Layout(
        'serum_creatinine_1',
        'serum_creatinine_1_unit',
        'serum_creatinine_2',
        'serum_creatinine_2_unit',
        'serum_creatinine_3',
        'serum_creatinine_3_unit',
        'serum_creatinine_4',
        'serum_creatinine_4_unit',
        'serum_creatinine_5',
        'serum_creatinine_5_unit',
        'serum_creatinine_6',
        'serum_creatinine_6_unit',
        'serum_creatinine_7',
        'serum_creatinine_7_unit',
    )
    layout_dialysis = Layout(
        'dialysis_requirement_1',
        'dialysis_requirement_2',
        'dialysis_requirement_3',
        'dialysis_requirement_4',
        'dialysis_requirement_5',
        'dialysis_requirement_6',
        'dialysis_requirement_7',
        'dialysis_type',
        'dialysis_cause',
        'dialysis_cause_other',
    )
    layout_hla = Layout(
        'hla_mismatch_a',
        'hla_mismatch_b',
        'hla_mismatch_dr',
        'induction_therapy',
        'immunosuppression',
        'immunosuppression_other',
    )
    layout_rejection = Layout(
        'rejection',
        'rejection_prednisolone',
        'rejection_drug',
        'rejection_drug_other',
        'rejection_biopsy',
        'calcineurin',
        'discharge_date',
    )
    layout_notes = Layout(
        'notes'
    )

    helper = FormHelper()
    # helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        Div(
            Div(
                FormPanel("1", layout_graft),
                FormPanel("4", layout_hla),
                css_class="col-md-4", style="margin-top: 10px;"
            ),
            Div(
                FormPanel("2", layout_serum),
                FormPanel("5", layout_rejection),
                css_class="col-md-4", style="margin-top: 10px;"
            ),
            Div(
                FormPanel("3", layout_dialysis),
                css_class="col-md-4", style="margin-top: 10px;"
            ),
            css_class='row'
        ),

        FormPanel("6", layout_notes),
        FormActions(
            'organ',
            Submit('save', 'Save changes'),
            Button('cancel', 'Cancel')
        )
    )

    class Meta:
        model = FollowUpInitial
        exclude = ['created_by', 'version', 'created_on', 'record_locked']
        localized_fields = "__all__"

    def __init__(self, *args, **kwargs):
        super(FollowUpInitialForm, self).__init__(*args, **kwargs)
        self.fields['organ'].widget = forms.HiddenInput()
        self.fields['start_date'].input_formats = DATE_INPUT_FORMATS

    def save(self, user):
        instance = super(FollowUpInitialForm, self).save(commit=False)
        instance.created_by = user
        instance.created_on = timezone.now()
        instance.save()
        return instance
