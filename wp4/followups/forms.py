#!/usr/bin/python
# coding: utf-8
from django import forms
from django.conf import settings
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _

from crispy_forms.helper import FormHelper
from crispy_forms.bootstrap import FormActions
from crispy_forms.layout import Layout, Div, Submit, Button, Field

from wp4.theme.layout import FormPanel, DateTimeField, DateField, InlineFields, FieldWithFollowup, YesNoFieldWithAlternativeFollowups, FieldWithNotKnown
from wp4.compare.models import YES_NO_UNKNOWN_CHOICES
from .models import FollowUpInitial, FollowUp3M, FollowUp6M, FollowUp1Y

# Common CONSTANTS
NO_YES_CHOICES = (
    (False, _("FF01 No")),
    (True, _("FF02 Yes"))
)

YES_NO_CHOICES = (
    (True, _("FF02 Yes")),
    (False, _("FF01 No"))
)


class FollowUpDayForm(forms.Form):
    recipient_alive = forms.Select(choices=NO_YES_CHOICES)
    on_dialysis_at_death = forms.Select(choices=NO_YES_CHOICES)

    graft_failure = forms.Select(choices=NO_YES_CHOICES)
    graft_failure_type = forms.Select(choices=FollowUpInitial.FAILURE_CHOICES)
    graft_failure_type_other = forms.CharField()

    graft_removal = forms.Select(choices=NO_YES_CHOICES)

    dialysis_required = forms.Select(choices=NO_YES_CHOICES)
    dialysis_type = forms.Select(choices=FollowUpInitial.DIALYSIS_TYPE_CHOICES)
    dialysis_cause = forms.Select(choices=FollowUpInitial.DIALYSIS_CAUSE_CHOICES)
    dialysis_cause_other = forms.CharField()

    serum_creatinine = forms.DecimalField()
    serum_creatinine_unit = forms.Select(
        choices=FollowUpInitial.UNIT_CHOICES
    )

    layout_graft_failure = Layout(
        FieldWithFollowup(
            'graft_failure_type',
            'graft_failure_type_other'
        ),
    )

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        'recipient_alive',
        'on_dialysis_at_death',
        FieldWithFollowup(
            Field('graft_failure', template="bootstrap3/layout/radioselect-buttons.html"),
            layout_graft_failure
        ),
        Field('graft_removal', template="bootstrap3/layout/radioselect-buttons.html"),
        'dialysis_required',
        Field('dialysis_type', template="bootstrap3/layout/radioselect-buttons.html"),
        FieldWithFollowup(
            'dialysis_cause',
            'dialysis_cause_other'
        ),
        'serum_creatinine',
        'serum_creatinine_unit',
    )

    def __init__(self, *args, **kwargs):
        super(FollowUpDayForm, self).__init__(*args, **kwargs)

    class Meta:
        localized_fields = "__all__"


FollowUpDayInlineFormSet = forms.models.formset_factory(
    form=FollowUpDayForm,
    min_num=1,
    validate_min=True,
    max_num=7,
    validate_max=True,
    extra=0,
    can_delete=False
)


class FollowUpInitialForm(forms.ModelForm):
    layout_hla = Layout(
        'hla_mismatch_a',
        'hla_mismatch_b',
        'hla_mismatch_dr',
        Field('induction_therapy', template="bootstrap3/layout/radioselect-buttons.html"),
        FieldWithFollowup(
            'immunosuppression',
            'immunosuppression_other'
        )
    )
    layout_rejection_yes = Layout(
        Field('rejection_prednisolone', template="bootstrap3/layout/radioselect-buttons.html"),
        FieldWithFollowup(
            Field('rejection_drug', template="bootstrap3/layout/radioselect-buttons.html"),
            'rejection_drug_other'
        ),
        Field('rejection_biopsy', template="bootstrap3/layout/radioselect-buttons.html"),
    )
    layout_rejection = Layout(
        FieldWithFollowup(
            Field('rejection', template="bootstrap3/layout/radioselect-buttons.html"),
            layout_rejection_yes
        ),
        Field('calcineurin', template="bootstrap3/layout/radioselect-buttons.html"),
        DateField('discharge_date')
    )
    layout_notes = Layout(
        'notes',
        Field('completed', template="bootstrap3/layout/radioselect-buttons.html")
    )

    helper = FormHelper()
    # helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        Div(
            Div(
                # FormPanel("Graft Update", layout_graft),
                FormPanel("HLA", layout_hla),
                css_class="col-md-4", style="margin-top: 10px;"
            ),
            # Div(
            #     # FormPanel("Creatinine", layout_serum),
            #     css_class="col-md-4", style="margin-top: 10px;"
            # ),
            # Div(
            #     FormPanel("Dialysis", layout_dialysis),
            #     css_class="col-md-4", style="margin-top: 10px;"
            # ),
            css_class='row'
        ),

        Div(
            Div(
                FormPanel("Rejection", layout_rejection),
                css_class="col-md-4", style="margin-top: 10px;"
            ),
            Div(
                FormPanel("General Comments", layout_notes),
                css_class="col-md-8", style="margin-top: 10px;"
            ),
            css_class='row'
        ),
        'organ',
    )

    class Meta:
        model = FollowUpInitial
        exclude = ['created_by', 'version', 'created_on', 'record_locked']
        localized_fields = "__all__"

    def __init__(self, *args, **kwargs):
        super(FollowUpInitialForm, self).__init__(*args, **kwargs)
        self.fields['organ'].widget = forms.HiddenInput()
        self.fields['start_date'].input_formats = settings.DATE_INPUT_FORMATS
        # self.fields['graft_failure'].choices = NO_YES_CHOICES
        # self.fields['graft_removal'].choices = NO_YES_CHOICES
        # self.fields['dialysis_type'].choices = FollowUpInitial.DIALYSIS_TYPE_CHOICES
        self.fields['induction_therapy'].choices = FollowUpInitial.INDUCTION_CHOICES
        self.fields['rejection'].choices = NO_YES_CHOICES
        self.fields['rejection_prednisolone'].choices = NO_YES_CHOICES
        self.fields['rejection_drug'].choices = NO_YES_CHOICES
        self.fields['rejection_biopsy'].choices = NO_YES_CHOICES
        self.fields['calcineurin'].choices = NO_YES_CHOICES

        self.fields['completed'].choices = NO_YES_CHOICES

    def save(self, user):
        instance = super(FollowUpInitialForm, self).save(commit=False)
        instance.save(created_by=user)
        return instance
