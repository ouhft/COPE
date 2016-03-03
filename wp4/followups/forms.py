#!/usr/bin/python
# coding: utf-8
from django import forms
from django.conf import settings
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _

from crispy_forms.helper import FormHelper
from crispy_forms.bootstrap import FormActions
from crispy_forms.layout import Layout, Div, Submit, Button, Field

from wp4.theme.layout import FormPanel, DateTimeField, DateField, InlineFields, FieldWithFollowup, \
    YesNoFieldWithAlternativeFollowups, FieldWithNotKnown
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
    recipient_alive = forms.ChoiceField(
        label=_("Is the recipient still alive?"),
        choices=NO_YES_CHOICES
    )
    on_dialysis_at_death = forms.ChoiceField(
        label=FollowUpInitial._meta.get_field("on_dialysis_at_death").verbose_name.title(),
        choices=NO_YES_CHOICES
    )

    graft_failure = forms.ChoiceField(
        label=_("FB02 graft failure"),
        choices=NO_YES_CHOICES
    )
    graft_failure_type = forms.ChoiceField(
        label=FollowUpInitial._meta.get_field("graft_failure_type").verbose_name.title(),
        choices=FollowUpInitial.FAILURE_CHOICES
    )
    graft_failure_type_other = forms.CharField(
        label=FollowUpInitial._meta.get_field("graft_failure_type_other").verbose_name.title(),
    )

    graft_removal = forms.ChoiceField(
        label=_("FB06 graft removal"),
        choices=NO_YES_CHOICES
    )

    dialysis_required = forms.ChoiceField(
        label=_("Dialysis required?"),
        choices=NO_YES_CHOICES
    )
    dialysis_type = forms.ChoiceField(
        label=FollowUpInitial._meta.get_field("dialysis_type").verbose_name.title(),
        choices=FollowUpInitial.DIALYSIS_TYPE_CHOICES
    )
    dialysis_cause = forms.ChoiceField(
        label=FollowUpInitial._meta.get_field("dialysis_cause").verbose_name.title(),
        choices=FollowUpInitial.DIALYSIS_CAUSE_CHOICES
    )
    dialysis_cause_other = forms.CharField(
        label=FollowUpInitial._meta.get_field("dialysis_cause_other").verbose_name.title(),
    )

    serum_creatinine = forms.DecimalField(
        label=_("Serum creatinine level")
    )
    serum_creatinine_unit = forms.ChoiceField(
        choices=FollowUpInitial.UNIT_CHOICES
    )

    def __init__(self, *args, **kwargs):
        super(FollowUpDayForm, self).__init__(*args, **kwargs)
        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            YesNoFieldWithAlternativeFollowups(
                Field('recipient_alive', template="bootstrap3/layout/radioselect-buttons.html"),
                Layout(
                    FieldWithFollowup(
                        Field('graft_failure', template="bootstrap3/layout/radioselect-buttons.html"),
                        Layout(
                            FieldWithFollowup(
                                'graft_failure_type',
                                'graft_failure_type_other'
                            )
                        )
                    ),
                    Field('graft_removal', template="bootstrap3/layout/radioselect-buttons.html"),
                    FieldWithFollowup(
                        Field('dialysis_required', template="bootstrap3/layout/radioselect-buttons.html"),
                        Layout(
                            Field('dialysis_type', template="bootstrap3/layout/radioselect-buttons.html"),
                            FieldWithFollowup(
                                'dialysis_cause',
                                'dialysis_cause_other'
                            ),
                        )
                    )
                ),
                Field('on_dialysis_at_death', template="bootstrap3/layout/radioselect-buttons.html")
            ),
            InlineFields('serum_creatinine', 'serum_creatinine_unit')
        )

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

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            'organ',
            FormPanel("Overall", Layout(
                'hla_mismatch_a',
                'hla_mismatch_b',
                'hla_mismatch_dr',
                Field('induction_therapy', template="bootstrap3/layout/radioselect-buttons.html"),
                FieldWithFollowup(
                    'immunosuppression',
                    'immunosuppression_other'
                ),
                FieldWithFollowup(
                    Field('rejection', template="bootstrap3/layout/radioselect-buttons.html"),
                    Layout(
                        Field('rejection_prednisolone', template="bootstrap3/layout/radioselect-buttons.html"),
                        FieldWithFollowup(
                            Field('rejection_drug', template="bootstrap3/layout/radioselect-buttons.html"),
                            'rejection_drug_other'
                        ),
                        Field('rejection_biopsy', template="bootstrap3/layout/radioselect-buttons.html"),
                    )
                ),
                Field('calcineurin', template="bootstrap3/layout/radioselect-buttons.html"),
                DateField('discharge_date')
            )),
            FormPanel("General Comments", Layout(
                'notes',
                Field('completed', template="bootstrap3/layout/radioselect-buttons.html")
            )),
        )


class FollowUp3MForm(forms.ModelForm):
    recipient_alive = forms.ChoiceField(
        label=_("Is the recipient still alive?"),
        choices=NO_YES_CHOICES
    )
    graft_failure = forms.ChoiceField(
        label=_("FB02 graft failure"),
        choices=NO_YES_CHOICES
    )
    graft_removal = forms.ChoiceField(
        label=_("FB06 graft removal"),
        choices=NO_YES_CHOICES
    )
    dialysis_required = forms.ChoiceField(
        label=_("Dialysis required?"),
        choices=NO_YES_CHOICES
    )
    date_of_death = forms.DateField(
        label=_("Date of death")
    )

    class Meta:
        model = FollowUp3M
        fields = [
            'start_date',
            'completed',
            'on_dialysis_at_death',
            'graft_failure_date',
            'graft_failure_type',
            'graft_failure_type_other',
            'graft_removal_date',
            'serum_creatinine_1',
            'serum_creatinine_1_unit',
            'last_dialysis_at',
            'dialysis_type',
            'immunosuppression',
            'immunosuppression_other',
            'rejection',
            'rejection_prednisolone',
            'rejection_drug',
            'rejection_drug_other',
            'rejection_biopsy',
            'calcineurin',
            'notes',
            # End common
            'organ',
            'creatinine_clearance',
            'currently_on_dialysis',
            'number_of_dialysis_sessions',
            'rejection_periods',
            'graft_complications',
            'qol_mobility',
            'qol_selfcare',
            'qol_usual_activities',
            'qol_pain',
            'qol_anxiety',
            'vas_score'
        ]
        localized_fields = "__all__"

    def __init__(self, *args, **kwargs):
        super(FollowUp3MForm, self).__init__(*args, **kwargs)

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            FormPanel("Overview", Layout(
                DateField('start_date'),

            )),
            YesNoFieldWithAlternativeFollowups(
                Field('recipient_alive', template="bootstrap3/layout/radioselect-buttons.html"),
                Layout(
                    FieldWithFollowup(
                        Field('graft_failure', template="bootstrap3/layout/radioselect-buttons.html"),
                        Layout(
                            FieldWithFollowup(
                                'graft_failure_type',
                                'graft_failure_type_other'
                            )
                        )
                    ),
                    Field('graft_removal', template="bootstrap3/layout/radioselect-buttons.html"),
                    FieldWithFollowup(
                        Field('dialysis_required', template="bootstrap3/layout/radioselect-buttons.html"),
                        Layout(
                            DateField('last_dialysis_at'),
                            Field('dialysis_type', template="bootstrap3/layout/radioselect-buttons.html"),
                            # FieldWithFollowup(
                            #     'dialysis_cause',
                            #     'dialysis_cause_other'
                            # ),
                        )
                    )
                ),
                Layout(
                    DateField('date_of_death'),
                    Field('on_dialysis_at_death', template="bootstrap3/layout/radioselect-buttons.html")
                )
            ),
            InlineFields('serum_creatinine_1', 'serum_creatinine_1_unit'),

            'completed',
            # 'on_dialysis_at_death',
            # 'graft_failure_date',
            # 'graft_failure_type',
            # 'graft_failure_type_other',
            # 'graft_removal_date',
            # 'serum_creatinine_1',
            # 'serum_creatinine_1_unit',
            # 'last_dialysis_at',
            # 'dialysis_type',
            'immunosuppression',
            'immunosuppression_other',
            'rejection',
            'rejection_prednisolone',
            'rejection_drug',
            'rejection_drug_other',
            'rejection_biopsy',
            'calcineurin',
            'notes',
            # End common
            'organ',
            'creatinine_clearance',
            'currently_on_dialysis',
            'number_of_dialysis_sessions',
            'rejection_periods',
            'graft_complications',
            'qol_mobility',
            'qol_selfcare',
            'qol_usual_activities',
            'qol_pain',
            'qol_anxiety',
            'vas_score'
        )

