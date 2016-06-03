#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django import forms
from django.conf import settings

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, Div, HTML, Field

from wp4.compare.models import YES_NO_UNKNOWN_CHOICES
from wp4.compare.forms.core import NO_YES_CHOICES
from wp4.theme.layout import DateField, FieldWithFollowup, ForeignKeyModal

from .models import FollowUpInitial, FollowUp3M, FollowUp6M, FollowUp1Y, FollowUpBase


class FollowUpInitialStartForm(forms.ModelForm):
    def __init__(self, *args, **kwargs):
        super(FollowUpInitialStartForm, self).__init__(*args, **kwargs)

        self.fields['organ'].queryset = self.fields['organ'].queryset.filter(
            transplantation_form_completed=True,
            recipient__isnull=False,
            followup_initial__isnull=True
        )

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            'organ',  # FI01
        )

    class Meta:
        model = FollowUpInitial
        fields = [
            'organ',  # FI01
        ]


class FollowUpInitialForm(forms.ModelForm):
    def __init__(self, *args, **kwargs):
        super(FollowUpInitialForm, self).__init__(*args, **kwargs)

        self.fields['start_date'].input_formats = settings.DATE_INPUT_FORMATS
        self.fields['graft_failure_date'].input_formats = settings.DATE_INPUT_FORMATS
        self.fields['graft_removal_date'].input_formats = settings.DATE_INPUT_FORMATS
        self.fields['discharge_date'].input_formats = settings.DATE_INPUT_FORMATS

        self.fields['graft_failure'].choices = NO_YES_CHOICES
        self.fields['graft_removal'].choices = NO_YES_CHOICES
        self.fields['rejection'].choices = NO_YES_CHOICES
        self.fields['rejection_prednisolone'].choices = NO_YES_CHOICES
        self.fields['rejection_drug'].choices = NO_YES_CHOICES
        self.fields['rejection_biopsy'].choices = NO_YES_CHOICES
        self.fields['calcineurin'].choices = NO_YES_CHOICES
        self.fields['dialysis_requirement_1'].choices = NO_YES_CHOICES
        self.fields['dialysis_requirement_2'].choices = NO_YES_CHOICES
        self.fields['dialysis_requirement_3'].choices = NO_YES_CHOICES
        self.fields['dialysis_requirement_4'].choices = NO_YES_CHOICES
        self.fields['dialysis_requirement_5'].choices = NO_YES_CHOICES
        self.fields['dialysis_requirement_6'].choices = NO_YES_CHOICES
        self.fields['dialysis_requirement_7'].choices = NO_YES_CHOICES

        self.fields['serum_creatinine_unit'].choices = FollowUpBase.UNIT_CHOICES
        self.fields['dialysis_type'].choices = FollowUpInitial.DIALYSIS_TYPE_CHOICES
        self.fields['induction_therapy'].choices = FollowUpInitial.INDUCTION_CHOICES

        self.fields['organ'].widget = forms.HiddenInput()
        self.fields['notes'].widget = forms.Textarea()

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            Div(
                Div(
                    'organ',  # FI01
                    FieldWithFollowup(
                        Field('graft_failure', template="bootstrap3/layout/radioselect-buttons.html"),  # FB10
                        Layout(
                            DateField('graft_failure_date'),  # FB11
                            'graft_failure_type',  # FB12
                            'graft_failure_type_other',  # FB13
                        )
                    ),
                    css_class="col-md-6"
                ),
                Div(
                    DateField('start_date'),  # FB01
                    FieldWithFollowup(
                        Field('graft_removal', template="bootstrap3/layout/radioselect-buttons.html"),  # FB14
                        DateField('graft_removal_date'),  # FB15
                    ),
                    css_class="col-md-6"
                ),
                css_class="row"
            ),

            Div(
                Div(
                    Field('serum_creatinine_unit', template="bootstrap3/layout/radioselect-buttons.html"),  # FI02
                    'serum_creatinine_1',  # FI03
                    css_class="col-md-3"
                ),
                Div(
                    'serum_creatinine_2',  # FI04
                    'serum_creatinine_3',  # FI05
                    css_class="col-md-3"
                ),
                Div(
                    'serum_creatinine_4',  # FI06
                    'serum_creatinine_5',  # FI07
                    css_class="col-md-3"
                ),
                Div(
                    'serum_creatinine_6',  # FI08
                    'serum_creatinine_7',  # FI09
                    css_class="col-md-3"
                ),
                css_class="row"
            ),
            Div(
                Div(
                    Field('dialysis_requirement_1', template="bootstrap3/layout/radioselect-buttons.html"),  # FI10
                    Field('dialysis_requirement_2', template="bootstrap3/layout/radioselect-buttons.html"),  # FI11
                    css_class="col-md-3"
                ),
                Div(
                    Field('dialysis_requirement_3', template="bootstrap3/layout/radioselect-buttons.html"),  # FI12
                    Field('dialysis_requirement_4', template="bootstrap3/layout/radioselect-buttons.html"),  # FI13
                    css_class="col-md-3"
                ),
                Div(
                    Field('dialysis_requirement_5', template="bootstrap3/layout/radioselect-buttons.html"),  # FI14
                    Field('dialysis_requirement_6', template="bootstrap3/layout/radioselect-buttons.html"),  # FI15
                    css_class="col-md-3"
                ),
                Div(
                    Field('dialysis_requirement_7', template="bootstrap3/layout/radioselect-buttons.html"),  # FI16
                    Field('dialysis_type', template="bootstrap3/layout/radioselect-buttons.html"),  # FB16
                    css_class="col-md-3"
                ),
                css_class="row"
            ),

            Div(
                Div(
                    'dialysis_cause',  # FI20
                    'dialysis_cause_other',  # FI21
                    'hla_mismatch_a',  # FI22
                    'hla_mismatch_b',  # FI23
                    'hla_mismatch_dr',  # FI24
                    css_class="col-md-4"
                ),
                Div(
                    Field('induction_therapy', template="bootstrap3/layout/radioselect-buttons.html"),  # FI25
                    HTML("<h4>Post Tx Immunosuppresion</h4>"),
                    'immunosuppression_1',  # FB30
                    'immunosuppression_2',  # FB31
                    'immunosuppression_3',  # FB32
                    'immunosuppression_4',  # FB33
                    'immunosuppression_5',  # FB34
                    'immunosuppression_6',  # FB35
                    'immunosuppression_7',  # FB36
                    'immunosuppression_other',  # FB37
                    css_class="col-md-4"
                ),
                Div(
                    FieldWithFollowup(
                        Field('rejection', template="bootstrap3/layout/radioselect-buttons.html"),  # FB19
                        Layout(
                            Field('rejection_prednisolone', template="bootstrap3/layout/radioselect-buttons.html"),
                            # FB20
                            Field('rejection_drug', template="bootstrap3/layout/radioselect-buttons.html"),  # FB21
                            'rejection_drug_other',  # FB22
                            Field('rejection_biopsy', template="bootstrap3/layout/radioselect-buttons.html"),  # FB23
                        )
                    ),
                    css_class="col-md-4"
                ),
                css_class="row"
            ),
            Field('calcineurin', template="bootstrap3/layout/radioselect-buttons.html"),  # FB24
            DateField('discharge_date'),  # FI26
            'notes',  # FB03
        )

    class Meta:
        model = FollowUpInitial
        fields = [
            'organ',  # FI01
            'start_date',  # FB01
            'graft_failure',  # FB10
            'graft_failure_date',  # FB11
            'graft_failure_type',  # FB12
            'graft_failure_type_other',  # FB13
            'graft_removal',  # FB14
            'graft_removal_date',  # FB15
            'serum_creatinine_unit',  # FI02
            'serum_creatinine_1',  # FI03
            'serum_creatinine_2',  # FI04
            'serum_creatinine_3',  # FI05
            'serum_creatinine_4',  # FI06
            'serum_creatinine_5',  # FI07
            'serum_creatinine_6',  # FI08
            'serum_creatinine_7',  # FI09
            'dialysis_requirement_1',  # FI10
            'dialysis_requirement_2',  # FI11
            'dialysis_requirement_3',  # FI12
            'dialysis_requirement_4',  # FI13
            'dialysis_requirement_5',  # FI14
            'dialysis_requirement_6',  # FI15
            'dialysis_requirement_7',  # FI16
            'dialysis_type',  # FB16
            'dialysis_cause',  # FI20
            'dialysis_cause_other',  # FI21
            'hla_mismatch_a',  # FI22
            'hla_mismatch_b',  # FI23
            'hla_mismatch_dr',  # FI24
            'induction_therapy',  # FI25
            'immunosuppression_1',  # FB30
            'immunosuppression_2',  # FB31
            'immunosuppression_3',  # FB32
            'immunosuppression_4',  # FB33
            'immunosuppression_5',  # FB34
            'immunosuppression_6',  # FB35
            'immunosuppression_7',  # FB36
            'immunosuppression_other',  # FB37
            'rejection',  # FB19
            'rejection_prednisolone',  # FB20
            'rejection_drug',  # FB21
            'rejection_drug_other',  # FB22
            'rejection_biopsy',  # FB23
            'calcineurin',  # FB24
            'discharge_date',  # FI26
            'notes',  # FB03
        ]
        localized_fields = "__all__"


class FollowUp3MStartForm(forms.ModelForm):
    def __init__(self, *args, **kwargs):
        super(FollowUp3MStartForm, self).__init__(*args, **kwargs)

        self.fields['organ'].queryset = self.fields['organ'].queryset.filter(
            # transplantation_form_completed=True,   <-- Redundant filter
            followup_initial__isnull=False,
            followup_3m__isnull=True
        )

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            'organ',  # F301
        )

    class Meta:
        model = FollowUp3M
        fields = [
            'organ',  # F301
        ]


class FollowUp3MForm(forms.ModelForm):
    def __init__(self, *args, **kwargs):
        super(FollowUp3MForm, self).__init__(*args, **kwargs)

        self.fields['start_date'].input_formats = settings.DATE_INPUT_FORMATS  # FB01
        self.fields['graft_failure_date'].input_formats = settings.DATE_INPUT_FORMATS  # FB11
        self.fields['graft_removal_date'].input_formats = settings.DATE_INPUT_FORMATS  # FB15
        self.fields['dialysis_date'].input_formats = settings.DATE_INPUT_FORMATS  # F304

        self.fields['currently_on_dialysis'].choices = YES_NO_UNKNOWN_CHOICES  # F303
        self.fields['graft_failure'].choices = NO_YES_CHOICES  # FB10
        self.fields['graft_removal'].choices = NO_YES_CHOICES  # FB14
        self.fields['rejection'].choices = NO_YES_CHOICES  # FB19
        self.fields['rejection_prednisolone'].choices = NO_YES_CHOICES  # FB20
        self.fields['rejection_drug'].choices = NO_YES_CHOICES  # FB21
        self.fields['rejection_biopsy'].choices = NO_YES_CHOICES  # FB23
        self.fields['calcineurin'].choices = NO_YES_CHOICES  # FB24

        self.fields['serum_creatinine_unit'].choices = FollowUpBase.UNIT_CHOICES  # F310
        self.fields['dialysis_type'].choices = FollowUpInitial.DIALYSIS_TYPE_CHOICES  # FB16

        self.fields['graft_complications'].widget = forms.Textarea()  # F307
        self.fields['notes'].widget = forms.Textarea()  # FB03

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            Div(
                Div(
                    'organ',  # FI01
                    FieldWithFollowup(
                        Field('graft_failure', template="bootstrap3/layout/radioselect-buttons.html"),  # FB10
                        Layout(
                            DateField('graft_failure_date'),  # FB11
                            'graft_failure_type',  # FB12
                            'graft_failure_type_other',  # FB13
                        )
                    ),
                    css_class="col-md-6"
                ),
                Div(
                    DateField('start_date'),  # FB01
                    FieldWithFollowup(
                        Field('graft_removal', template="bootstrap3/layout/radioselect-buttons.html"),  # FB14
                        DateField('graft_removal_date'),  # FB15
                    ),
                    css_class="col-md-6"
                ),
                css_class="row"
            ),

            Div(
                Div(
                    Field('serum_creatinine_unit', template="bootstrap3/layout/radioselect-buttons.html"),  # FI02
                    'serum_creatinine',  # F311
                    'creatinine_clearance',  # F302
                    css_class="col-md-4"
                ),
                Div(
                    FieldWithFollowup(
                        Field('currently_on_dialysis', template="bootstrap3/layout/radioselect-buttons.html"),  # F303
                        Field('dialysis_type', template="bootstrap3/layout/radioselect-buttons.html"),  # FB16
                    ),
                    css_class="col-md-4"
                ),
                Div(
                    DateField('dialysis_date'),  # F304
                    'number_of_dialysis_sessions',  # F305
                    css_class="col-md-4"
                ),
                css_class="row"
            ),

            Div(
                Div(
                    HTML("<h4>Post Tx Immunosuppresion</h4>"),
                    'immunosuppression_1',  # FB30
                    'immunosuppression_2',  # FB31
                    'immunosuppression_3',  # FB32
                    'immunosuppression_4',  # FB33
                    'immunosuppression_5',  # FB34
                    'immunosuppression_6',  # FB35
                    'immunosuppression_7',  # FB36
                    'immunosuppression_other',  # FB37
                    css_class="col-md-4"
                ),
                Div(
                    FieldWithFollowup(
                        Field('rejection', template="bootstrap3/layout/radioselect-buttons.html"),  # FB19
                        Layout(
                            'rejection_periods',  # F306
                            Field('rejection_prednisolone', template="bootstrap3/layout/radioselect-buttons.html"),
                            # FB20
                            Field('rejection_drug', template="bootstrap3/layout/radioselect-buttons.html"),  # FB21
                            'rejection_drug_other',  # FB22
                            Field('rejection_biopsy', template="bootstrap3/layout/radioselect-buttons.html"),  # FB23
                        )
                    ),
                    css_class="col-md-4"
                ),
                Div(
                    Field('calcineurin', template="bootstrap3/layout/radioselect-buttons.html"),  # FB24
                    'graft_complications',  # F307
                    'notes',  # FB03
                    css_class="col-md-4"
                ),
                css_class="row"
            ),
            ForeignKeyModal('quality_of_life', no_search=True),  # F308
        )

    class Meta:
        model = FollowUp3M
        fields = [
            'organ',  # F301
            'start_date',  # FB01
            'graft_failure',  # FB10
            'graft_failure_date',  # FB11
            'graft_failure_type',  # FB12
            'graft_failure_type_other',  # FB13
            'graft_removal',  # FB14
            'graft_removal_date',  # FB15
            'serum_creatinine_unit',  # F310
            'serum_creatinine',  # F311
            'creatinine_clearance',  # F302
            'currently_on_dialysis',  # F303
            'dialysis_type',  # FB16
            'dialysis_date',  # F304
            'number_of_dialysis_sessions',  # F305
            'immunosuppression_1',  # FB30
            'immunosuppression_2',  # FB31
            'immunosuppression_3',  # FB32
            'immunosuppression_4',  # FB33
            'immunosuppression_5',  # FB34
            'immunosuppression_6',  # FB35
            'immunosuppression_7',  # FB36
            'immunosuppression_other',  # FB37
            'rejection',  # FB19
            'rejection_periods',  # F306
            'rejection_prednisolone',  # FB20
            'rejection_drug',  # FB21
            'rejection_drug_other',  # FB22
            'rejection_biopsy',  # FB23
            'calcineurin',  # FB24
            'graft_complications',  # F307
            'quality_of_life',  # F308
            'notes',  # FB03
        ]
        localized_fields = "__all__"


class FollowUp6MStartForm(forms.ModelForm):
    def __init__(self, *args, **kwargs):
        super(FollowUp6MStartForm, self).__init__(*args, **kwargs)

        self.fields['organ'].queryset = self.fields['organ'].queryset.filter(
            # transplantation_form_completed=True,   <-- Redundant filters
            # followup_initial__isnull=False,
            followup_3m__isnull=False,
            followup_6m__isnull=True
        )

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            'organ',  # F601
        )

    class Meta:
        model = FollowUp6M
        fields = [
            'organ',  # F601
        ]


class FollowUp6MForm(forms.ModelForm):
    def __init__(self, *args, **kwargs):
        super(FollowUp6MForm, self).__init__(*args, **kwargs)

        self.fields['start_date'].input_formats = settings.DATE_INPUT_FORMATS  # FB01
        self.fields['graft_failure_date'].input_formats = settings.DATE_INPUT_FORMATS  # FB11
        self.fields['graft_removal_date'].input_formats = settings.DATE_INPUT_FORMATS  # FB15
        self.fields['dialysis_date'].input_formats = settings.DATE_INPUT_FORMATS  # F604

        self.fields['currently_on_dialysis'].choices = YES_NO_UNKNOWN_CHOICES  # F603
        self.fields['graft_failure'].choices = NO_YES_CHOICES  # FB10
        self.fields['graft_removal'].choices = NO_YES_CHOICES  # FB14
        self.fields['rejection'].choices = NO_YES_CHOICES  # FB19
        self.fields['rejection_prednisolone'].choices = NO_YES_CHOICES  # FB20
        self.fields['rejection_drug'].choices = NO_YES_CHOICES  # FB21
        self.fields['rejection_biopsy'].choices = NO_YES_CHOICES  # FB23
        self.fields['calcineurin'].choices = NO_YES_CHOICES  # FB24

        self.fields['serum_creatinine_unit'].choices = FollowUpBase.UNIT_CHOICES  # F610
        self.fields['dialysis_type'].choices = FollowUpInitial.DIALYSIS_TYPE_CHOICES  # FB16

        self.fields['graft_complications'].widget = forms.Textarea()  # F607
        self.fields['notes'].widget = forms.Textarea()  # FB03

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            Div(
                Div(
                    'organ',  # F601
                    FieldWithFollowup(
                        Field('graft_failure', template="bootstrap3/layout/radioselect-buttons.html"),  # FB10
                        Layout(
                            DateField('graft_failure_date'),  # FB11
                            'graft_failure_type',  # FB12
                            'graft_failure_type_other',  # FB13
                        )
                    ),
                    css_class="col-md-6"
                ),
                Div(
                    DateField('start_date'),  # FB01
                    FieldWithFollowup(
                        Field('graft_removal', template="bootstrap3/layout/radioselect-buttons.html"),  # FB14
                        DateField('graft_removal_date'),  # FB15
                    ),
                    css_class="col-md-6"
                ),
                css_class="row"
            ),

            Div(
                Div(
                    Field('serum_creatinine_unit', template="bootstrap3/layout/radioselect-buttons.html"),  # F602
                    'serum_creatinine',  # F611
                    'creatinine_clearance',  # F602
                    css_class="col-md-4"
                ),
                Div(
                    FieldWithFollowup(
                        Field('currently_on_dialysis', template="bootstrap3/layout/radioselect-buttons.html"),  # F603
                        Field('dialysis_type', template="bootstrap3/layout/radioselect-buttons.html"),  # FB16
                    ),
                    css_class="col-md-4"
                ),
                Div(
                    DateField('dialysis_date'),  # F604
                    'number_of_dialysis_sessions',  # F605
                    css_class="col-md-4"
                ),
                css_class="row"
            ),

            Div(
                Div(
                    HTML("<h4>Post Tx Immunosuppresion</h4>"),
                    'immunosuppression_1',  # FB30
                    'immunosuppression_2',  # FB31
                    'immunosuppression_3',  # FB32
                    'immunosuppression_4',  # FB33
                    'immunosuppression_5',  # FB34
                    'immunosuppression_6',  # FB35
                    'immunosuppression_7',  # FB36
                    'immunosuppression_other',  # FB37
                    css_class="col-md-4"
                ),
                Div(
                    FieldWithFollowup(
                        Field('rejection', template="bootstrap3/layout/radioselect-buttons.html"),  # FB19
                        Layout(
                            'rejection_periods',  # F606
                            Field('rejection_prednisolone', template="bootstrap3/layout/radioselect-buttons.html"),
                            # FB20
                            Field('rejection_drug', template="bootstrap3/layout/radioselect-buttons.html"),  # FB21
                            'rejection_drug_other',  # FB22
                            Field('rejection_biopsy', template="bootstrap3/layout/radioselect-buttons.html"),  # FB23
                        )
                    ),
                    css_class="col-md-4"
                ),
                Div(
                    Field('calcineurin', template="bootstrap3/layout/radioselect-buttons.html"),  # FB24
                    'graft_complications',  # F607
                    'notes',  # FB03
                    css_class="col-md-4"
                ),
                css_class="row"
            ),
        )

    class Meta:
        model = FollowUp6M
        fields = [
            'organ',  # F601
            'start_date',  # FB01
            'graft_failure',  # FB10
            'graft_failure_date',  # FB11
            'graft_failure_type',  # FB12
            'graft_failure_type_other',  # FB13
            'graft_removal',  # FB14
            'graft_removal_date',  # FB15
            'serum_creatinine_unit',  # F610
            'serum_creatinine',  # F611
            'creatinine_clearance',  # F602
            'currently_on_dialysis',  # F603
            'dialysis_type',  # FB16
            'dialysis_date',  # F604
            'number_of_dialysis_sessions',  # F605
            'immunosuppression_1',  # FB30
            'immunosuppression_2',  # FB31
            'immunosuppression_3',  # FB32
            'immunosuppression_4',  # FB33
            'immunosuppression_5',  # FB34
            'immunosuppression_6',  # FB35
            'immunosuppression_7',  # FB36
            'immunosuppression_other',  # FB37
            'rejection',  # FB19
            'rejection_periods',  # F606
            'rejection_prednisolone',  # FB20
            'rejection_drug',  # FB21
            'rejection_drug_other',  # FB22
            'rejection_biopsy',  # FB23
            'calcineurin',  # FB24
            'graft_complications',  # F607
            'notes',  # FB03
        ]
        localized_fields = "__all__"


class FollowUp1YStartForm(forms.ModelForm):
    def __init__(self, *args, **kwargs):
        super(FollowUp1YStartForm, self).__init__(*args, **kwargs)

        self.fields['organ'].queryset = self.fields['organ'].queryset.filter(
            # transplantation_form_completed=True,   <-- Redundant filters
            # followup_initial__isnull=False,
            # followup_3m__isnull=False,
            followup_6m__isnull=False,
            followup_1y__isnull=True
        )

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            'organ',  # FY01
        )

    class Meta:
        model = FollowUp1Y
        fields = [
            'organ',  # FY01
        ]


class FollowUp1YForm(forms.ModelForm):
    def __init__(self, *args, **kwargs):
        super(FollowUp1YForm, self).__init__(*args, **kwargs)

        self.fields['start_date'].input_formats = settings.DATE_INPUT_FORMATS  # FB01
        self.fields['graft_failure_date'].input_formats = settings.DATE_INPUT_FORMATS  # FB11
        self.fields['graft_removal_date'].input_formats = settings.DATE_INPUT_FORMATS  # FB15
        self.fields['dialysis_date'].input_formats = settings.DATE_INPUT_FORMATS  # FY04

        self.fields['currently_on_dialysis'].choices = YES_NO_UNKNOWN_CHOICES  # FY03
        self.fields['graft_failure'].choices = NO_YES_CHOICES  # FB10
        self.fields['graft_removal'].choices = NO_YES_CHOICES  # FB14
        self.fields['rejection'].choices = NO_YES_CHOICES  # FB19
        self.fields['rejection_prednisolone'].choices = NO_YES_CHOICES  # FB20
        self.fields['rejection_drug'].choices = NO_YES_CHOICES  # FB21
        self.fields['rejection_biopsy'].choices = NO_YES_CHOICES  # FB23
        self.fields['calcineurin'].choices = NO_YES_CHOICES  # FB24

        self.fields['serum_creatinine_unit'].choices = FollowUpBase.UNIT_CHOICES  # FY10
        self.fields['dialysis_type'].choices = FollowUpInitial.DIALYSIS_TYPE_CHOICES  # FB16

        self.fields['graft_complications'].widget = forms.Textarea()  # FY07
        self.fields['notes'].widget = forms.Textarea()  # FB03

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            Div(
                Div(
                    'organ',  # FY01
                    FieldWithFollowup(
                        Field('graft_failure', template="bootstrap3/layout/radioselect-buttons.html"),  # FB10
                        Layout(
                            DateField('graft_failure_date'),  # FB11
                            'graft_failure_type',  # FB12
                            'graft_failure_type_other',  # FB13
                        )
                    ),
                    css_class="col-md-6"
                ),
                Div(
                    DateField('start_date'),  # FB01
                    FieldWithFollowup(
                        Field('graft_removal', template="bootstrap3/layout/radioselect-buttons.html"),  # FB14
                        DateField('graft_removal_date'),  # FB15
                    ),
                    css_class="col-md-6"
                ),
                css_class="row"
            ),

            Div(
                Div(
                    Field('serum_creatinine_unit', template="bootstrap3/layout/radioselect-buttons.html"),  # FY10
                    'serum_creatinine',  # FY11
                    'creatinine_clearance',  # FY02
                    css_class="col-md-4"
                ),
                Div(
                    FieldWithFollowup(
                        Field('currently_on_dialysis', template="bootstrap3/layout/radioselect-buttons.html"),  # FY03
                        Field('dialysis_type', template="bootstrap3/layout/radioselect-buttons.html"),  # FB16
                    ),
                    css_class="col-md-4"
                ),
                Div(
                    DateField('dialysis_date'),  # FY04
                    'number_of_dialysis_sessions',  # FY05
                    css_class="col-md-4"
                ),
                css_class="row"
            ),

            Div(
                Div(
                    HTML("<h4>Post Tx Immunosuppresion</h4>"),
                    'immunosuppression_1',  # FB30
                    'immunosuppression_2',  # FB31
                    'immunosuppression_3',  # FB32
                    'immunosuppression_4',  # FB33
                    'immunosuppression_5',  # FB34
                    'immunosuppression_6',  # FB35
                    'immunosuppression_7',  # FB36
                    'immunosuppression_other',  # FB37
                    css_class="col-md-4"
                ),
                Div(
                    FieldWithFollowup(
                        Field('rejection', template="bootstrap3/layout/radioselect-buttons.html"),  # FB19
                        Layout(
                            'rejection_periods',  # FY06
                            Field('rejection_prednisolone', template="bootstrap3/layout/radioselect-buttons.html"),
                            # FB20
                            Field('rejection_drug', template="bootstrap3/layout/radioselect-buttons.html"),  # FB21
                            'rejection_drug_other',  # FB22
                            Field('rejection_biopsy', template="bootstrap3/layout/radioselect-buttons.html"),  # FB23
                        )
                    ),
                    css_class="col-md-4"
                ),
                Div(
                    Field('calcineurin', template="bootstrap3/layout/radioselect-buttons.html"),  # FB24
                    'graft_complications',  # FY07
                    'notes',  # FB03
                    css_class="col-md-4"
                ),
                css_class="row"
            ),
            ForeignKeyModal('quality_of_life'),  # FY08
        )

    class Meta:
        model = FollowUp1Y
        fields = [
            'organ',  # FY01
            'start_date',  # FB01
            'graft_failure',  # FB10
            'graft_failure_date',  # FB11
            'graft_failure_type',  # FB12
            'graft_failure_type_other',  # FB13
            'graft_removal',  # FB14
            'graft_removal_date',  # FB15
            'serum_creatinine_unit',  # FY10
            'serum_creatinine',  # FY11
            'creatinine_clearance',  # FY02
            'currently_on_dialysis',  # FY03
            'dialysis_type',  # FB16
            'dialysis_date',  # FY04
            'number_of_dialysis_sessions',  # FY05
            'immunosuppression_1',  # FB30
            'immunosuppression_2',  # FB31
            'immunosuppression_3',  # FB32
            'immunosuppression_4',  # FB33
            'immunosuppression_5',  # FB34
            'immunosuppression_6',  # FB35
            'immunosuppression_7',  # FB36
            'immunosuppression_other',  # FB37
            'rejection',  # FB19
            'rejection_periods',  # FY06
            'rejection_prednisolone',  # FB20
            'rejection_drug',  # FB21
            'rejection_drug_other',  # FB22
            'rejection_biopsy',  # FB23
            'calcineurin',  # FB24
            'graft_complications',  # FY07
            'quality_of_life',  # FY08
            'notes',  # FB03
        ]
        localized_fields = "__all__"
