#!/usr/bin/python
# coding: utf-8
from django import forms
from django.conf import settings
from django.utils import timezone

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, Div, HTML, Field

from wp4.compare.models import YES_NO_UNKNOWN_CHOICES
from wp4.compare.forms.core import NO_YES_CHOICES, YES_NO_CHOICES
from wp4.theme.layout import DateTimeField, DateField, FormPanel, ForeignKeyModal, FieldWithFollowup

from .models import AdverseEvent


class AdverseEventForm(forms.ModelForm):
    def __init__(self, *args, **kwargs):
        super(AdverseEventForm, self).__init__(*args, **kwargs)
        self.fields['serious_eligible_1'].choices = NO_YES_CHOICES
        self.fields['serious_eligible_2'].choices = NO_YES_CHOICES
        self.fields['serious_eligible_3'].choices = NO_YES_CHOICES
        self.fields['serious_eligible_4'].choices = NO_YES_CHOICES
        self.fields['serious_eligible_5'].choices = NO_YES_CHOICES
        self.fields['serious_eligible_6'].choices = NO_YES_CHOICES
        self.fields['onset_at_date'].input_formats = settings.DATE_INPUT_FORMATS
        self.fields['event_ongoing'].choices = NO_YES_CHOICES
        self.fields['description'].widget = forms.Textarea()
        self.fields['action'].widget = forms.Textarea()
        self.fields['outcome'].widget = forms.Textarea()
        self.fields['alive_query_1'].choices = NO_YES_CHOICES
        self.fields['alive_query_2'].choices = NO_YES_CHOICES
        self.fields['alive_query_3'].choices = NO_YES_CHOICES
        self.fields['alive_query_4'].choices = NO_YES_CHOICES
        self.fields['alive_query_5'].choices = NO_YES_CHOICES
        self.fields['alive_query_6'].choices = NO_YES_CHOICES
        self.fields['alive_query_7'].choices = NO_YES_CHOICES
        self.fields['alive_query_8'].choices = NO_YES_CHOICES
        self.fields['alive_query_9'].choices = NO_YES_CHOICES
        self.fields['rehospitalisation'].choices = NO_YES_CHOICES
        self.fields['date_of_admission'].input_formats = settings.DATE_INPUT_FORMATS
        self.fields['date_of_discharge'].input_formats = settings.DATE_INPUT_FORMATS
        self.fields['admitted_to_itu'].choices = NO_YES_CHOICES
        self.fields['dialysis_needed'].choices = NO_YES_CHOICES
        self.fields['biopsy_taken'].choices = NO_YES_CHOICES
        self.fields['surgery_required'].choices = NO_YES_CHOICES
        self.fields['death'].choices = NO_YES_CHOICES
        self.fields['date_of_death'].input_formats = settings.DATE_INPUT_FORMATS
        self.fields['treatment_related'].choices = YES_NO_UNKNOWN_CHOICES

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            FormPanel("Part 1", Layout(
                'organ',
                Div(
                    Div(
                        Field('serious_eligible_1', template="bootstrap3/layout/radioselect-buttons.html"),
                        Field('serious_eligible_2', template="bootstrap3/layout/radioselect-buttons.html"),
                        Field('serious_eligible_3', template="bootstrap3/layout/radioselect-buttons.html"),
                        css_class="col-md-6", style="margin-top: 10px;"
                    ),
                    Div(
                        Field('serious_eligible_4', template="bootstrap3/layout/radioselect-buttons.html"),
                        Field('serious_eligible_5', template="bootstrap3/layout/radioselect-buttons.html"),
                        Field('serious_eligible_6', template="bootstrap3/layout/radioselect-buttons.html"),
                        css_class="col-md-6", style="margin-top: 10px;"
                    ),
                    css_class="row"
                )
            )),
            FormPanel("Part 2", Layout(
                Div(
                    Div(
                        DateField('onset_at_date'),
                        css_class="col-md-6", style="margin-top: 10px;"
                    ),
                    Div(
                        Field('event_ongoing', template="bootstrap3/layout/radioselect-buttons.html"),
                        css_class="col-md-6", style="margin-top: 10px;"
                    ),
                    css_class="row"
                ),
                'description',
                'action',
                'outcome',

                Div(
                    HTML("<p>Q5: if alive?</p>"),
                    Div(
                        Field('alive_query_1', template="bootstrap3/layout/radioselect-buttons.html"),
                        Field('alive_query_2', template="bootstrap3/layout/radioselect-buttons.html"),
                        Field('alive_query_3', template="bootstrap3/layout/radioselect-buttons.html"),
                        Field('alive_query_4', template="bootstrap3/layout/radioselect-buttons.html"),
                        Field('alive_query_5', template="bootstrap3/layout/radioselect-buttons.html"),
                        css_class="col-md-6", style="margin-top: 10px;"
                    ),
                    Div(
                        Field('alive_query_6', template="bootstrap3/layout/radioselect-buttons.html"),
                        Field('alive_query_7', template="bootstrap3/layout/radioselect-buttons.html"),
                        Field('alive_query_8', template="bootstrap3/layout/radioselect-buttons.html"),
                        Field('alive_query_9', template="bootstrap3/layout/radioselect-buttons.html"),
                        css_class="col-md-6", style="margin-top: 10px;"
                    ),
                    css_class="row"
                ),

                FieldWithFollowup(
                    Field('rehospitalisation', template="bootstrap3/layout/radioselect-buttons.html"),
                    Layout(
                        DateField('date_of_admission'),
                        DateField('date_of_discharge'),
                        Field('admitted_to_itu', template="bootstrap3/layout/radioselect-buttons.html"),
                        Field('dialysis_needed', template="bootstrap3/layout/radioselect-buttons.html"),
                        Field('biopsy_taken', template="bootstrap3/layout/radioselect-buttons.html"),
                        Field('surgery_required', template="bootstrap3/layout/radioselect-buttons.html"),
                        'rehospitalisation_comments',
                    )
                ),

                FieldWithFollowup(
                    Field('death', template="bootstrap3/layout/radioselect-buttons.html"),
                    Layout(
                        DateField('date_of_death'),
                        Field('treatment_related', template="bootstrap3/layout/radioselect-buttons.html"),
                        HTML("<p>Cause of death, tick all that apply</p>"),
                        Div(
                            'cause_of_death_1',
                            'cause_of_death_2',
                            'cause_of_death_3',
                            css_class="col-md-6", style="margin-top: 10px;"
                        ),
                        Div(
                            'cause_of_death_4',
                            'cause_of_death_5',
                            'cause_of_death_6',
                            'cause_of_death_comment',
                            css_class="col-md-6", style="margin-top: 10px;"
                        ),
                    )
                ),
            )),
            ForeignKeyModal('contact'),
        )

    class Meta:
        model = AdverseEvent
        fields = [
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
            'date_of_death',
            'treatment_related',
            'cause_of_death_1',
            'cause_of_death_2',
            'cause_of_death_3',
            'cause_of_death_4',
            'cause_of_death_5',
            'cause_of_death_6',
            'cause_of_death_comment',
            'contact'
        ]
        localized_fields = "__all__"
