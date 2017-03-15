#!/usr/bin/python
# coding: utf-8
from __future__ import unicode_literals

from django import forms
from django.conf import settings
from django.utils.translation import ugettext_lazy as _

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, Div, HTML, Field
from dal import autocomplete

from wp4.theme.layout import FieldWithFollowup, YesNoFieldWithAlternativeFollowups, FieldWithNotKnown, ForeignKeyModal
from wp4.theme.layout import DateTimeField, FormPanel
from ..models import OrganAllocation, Recipient, Organ
from ..models import YES_NO_UNKNOWN_CHOICES, LOCATION_CHOICES
from .core import NO_YES_CHOICES


class AllocationForm(forms.ModelForm):
    layout_reallocation = Layout(
        FieldWithFollowup(
            Field('reallocation_reason', template="bootstrap3/layout/radioselect-buttons.html"),
            'reallocation_reason_other'
        ),
        'reallocation'
    )

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.include_media = False  # We will add the media ourselves in the footer
    helper.layout = Layout(
        'organ',
        Div(
            Div(
                'perfusion_technician',
                FieldWithNotKnown(
                    DateTimeField('call_received', notknown=True),
                    'call_received_unknown',
                    label=OrganAllocation._meta.get_field("call_received").verbose_name.title()
                ),
                'transplant_hospital',
                ForeignKeyModal('theatre_contact'),
                FieldWithNotKnown(
                    DateTimeField('scheduled_start', notknown=True),
                    'scheduled_start_unknown',
                    label=OrganAllocation._meta.get_field("scheduled_start").verbose_name.title()
                ),
                FieldWithNotKnown(
                    DateTimeField('technician_arrival', notknown=True),
                    'technician_arrival_unknown',
                    label=OrganAllocation._meta.get_field("technician_arrival").verbose_name.title()
                ),
                style="padding-right:0.5em"
            ),
            css_class="col-md-4"
        ),
        Div(
            Div(
                FieldWithNotKnown(
                    DateTimeField('depart_perfusion_centre', notknown=True),
                    'depart_perfusion_centre_unknown',
                    label=OrganAllocation._meta.get_field("depart_perfusion_centre").verbose_name.title()
                ),
                FieldWithNotKnown(
                    DateTimeField('arrival_at_recipient_hospital', notknown=True),
                    'arrival_at_recipient_hospital_unknown',
                    label=OrganAllocation._meta.get_field("arrival_at_recipient_hospital").verbose_name.title()
                ),
                'journey_remarks',
                style="padding-right:0.5em"
            ),
            css_class="col-md-4"
        ),
        Div(
            FieldWithFollowup(
                Field('reallocated', template="bootstrap3/layout/radioselect-buttons.html"),
                layout_reallocation
            ),
            css_class="col-md-4"
        )
    )

    def __init__(self, *args, **kwargs):
        super(AllocationForm, self).__init__(*args, **kwargs)
        self.fields['organ'].widget = forms.HiddenInput()
        self.fields['perfusion_technician'].required = False
        self.fields['call_received'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['scheduled_start'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['technician_arrival'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['depart_perfusion_centre'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['arrival_at_recipient_hospital'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['reallocated'].choices = NO_YES_CHOICES
        self.fields['reallocation_reason'].choices = OrganAllocation.REALLOCATION_CHOICES
        self.fields['reallocation'].widget = forms.HiddenInput()

    class Meta:
        model = OrganAllocation
        fields = [
            'organ', 'perfusion_technician', 'call_received', 'call_received_unknown',
            'transplant_hospital', 'theatre_contact',
            'scheduled_start', 'scheduled_start_unknown', 'technician_arrival',
            'technician_arrival_unknown', 'depart_perfusion_centre', 'depart_perfusion_centre_unknown',
            'arrival_at_recipient_hospital', 'arrival_at_recipient_hospital_unknown',
            'journey_remarks', 'reallocated', 'reallocation_reason', 'reallocation_reason_other',
            'reallocation'
        ]
        widgets = {
            'perfusion_technician': autocomplete.ModelSelect2(url='wp4:staff:technician-autocomplete')
        }
        localized_fields = "__all__"

    def save(self, user=None, *args, **kwargs):
        allocation_instance = super(AllocationForm, self).save(commit=False)
        if kwargs.get("commit", True):
            if user is None:
                raise Exception("Missing user record when saving AllocationForm")
            allocation_instance.save(created_by=user)
        return allocation_instance


AllocationFormSet = forms.modelformset_factory(
    OrganAllocation,
    form=AllocationForm,
    min_num=1,
    validate_min=True,
    can_delete=False,
    extra=0
)


class RecipientForm(forms.ModelForm):
    def __init__(self, *args, **kwargs):
        super(RecipientForm, self).__init__(*args, **kwargs)
        self.fields['person'].widget = forms.HiddenInput()
        self.fields['organ'].widget = forms.HiddenInput()
        self.fields['allocation'].widget = forms.HiddenInput()

        self.fields['signed_consent'].choices = NO_YES_CHOICES
        self.fields['single_kidney_transplant'].choices = NO_YES_CHOICES
        self.fields['renal_disease'].choices = Recipient.RENAL_DISEASE_CHOICES
        self.fields['knife_to_skin'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['perfusion_stopped'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['organ_cold_stored'].choices = NO_YES_CHOICES
        self.fields['tape_broken'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['removed_from_machine_at'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['oxygen_full_and_open'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['organ_untransplantable'].choices = NO_YES_CHOICES
        self.fields['anesthesia_started_at'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['incision'].choices = Recipient.INCISION_CHOICES
        self.fields['transplant_side'].choices = LOCATION_CHOICES
        self.fields['transplant_side'].required = False
        self.fields['arterial_problems'].choices = Recipient.ARTERIAL_PROBLEM_CHOICES
        self.fields['venous_problems'].choices = Recipient.VENOUS_PROBLEM_CHOICES
        self.fields['anastomosis_started_at'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['reperfusion_started_at'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['mannitol_used'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['other_diurectics'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['intra_operative_diuresis'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['successful_conclusion'].choices = NO_YES_CHOICES
        self.fields['operation_concluded_at'].input_formats = settings.DATETIME_INPUT_FORMATS

        self.fields['probe_cleaned'].choices = NO_YES_CHOICES
        self.fields['ice_removed'].choices = NO_YES_CHOICES
        self.fields['oxygen_flow_stopped'].choices = NO_YES_CHOICES
        self.fields['oxygen_bottle_removed'].choices = NO_YES_CHOICES
        self.fields['box_cleaned'].choices = NO_YES_CHOICES
        self.fields['batteries_charged'].choices = NO_YES_CHOICES

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            FormPanel("Recipient Details", Layout(
                Field('single_kidney_transplant', template="bootstrap3/layout/radioselect-buttons.html"),
                Field('signed_consent', template="bootstrap3/layout/radioselect-buttons.html"),
                FieldWithFollowup(
                    'renal_disease',
                    'renal_disease_other'
                ),
                'pre_transplant_diuresis'
            )),
            HTML("</div>"),
            Div(
                FormPanel("Peri-Operative Data", Layout(
                    'perfusate_measure',
                    DateTimeField('perfusion_stopped'),
                    Field('organ_cold_stored', template="bootstrap3/layout/radioselect-buttons.html"),
                    Field('tape_broken', template="bootstrap3/layout/radioselect-buttons.html"),
                    # 'tape_broken',
                    DateTimeField('removed_from_machine_at'),
                    Field('oxygen_full_and_open', template="bootstrap3/layout/radioselect-buttons.html"),

                    YesNoFieldWithAlternativeFollowups(
                        Field('organ_untransplantable', template="bootstrap3/layout/radioselect-buttons.html"),
                        'organ_untransplantable_reason',
                        Layout(
                            DateTimeField('anesthesia_started_at'),
                            DateTimeField('knife_to_skin'),
                            Field('incision', template="bootstrap3/layout/radioselect-buttons.html"),
                            Field('transplant_side', template="bootstrap3/layout/radioselect-buttons.html"),
                            FieldWithFollowup(
                                'arterial_problems',
                                'arterial_problems_other'
                            ),
                            FieldWithFollowup(
                                'venous_problems',
                                'venous_problems_other'
                            ),
                            FieldWithNotKnown(
                                DateTimeField('anastomosis_started_at', notknown=True),
                                'anastomosis_started_at_unknown',
                                label=Recipient._meta.get_field("anastomosis_started_at").verbose_name.title()
                            ),
                            FieldWithNotKnown(
                                DateTimeField('reperfusion_started_at', notknown=True),
                                'reperfusion_started_at_unknown',
                                label=Recipient._meta.get_field("reperfusion_started_at").verbose_name.title()
                            ),
                            Field('mannitol_used', template="bootstrap3/layout/radioselect-buttons.html"),
                            FieldWithFollowup(
                                Field('other_diurectics', template="bootstrap3/layout/radioselect-buttons.html"),
                                'other_diurectics_details'
                            ),
                            'systolic_blood_pressure',
                            'cvp',
                            Field('intra_operative_diuresis', template="bootstrap3/layout/radioselect-buttons.html"),
                            Field('successful_conclusion', template="bootstrap3/layout/radioselect-buttons.html"),
                            DateTimeField('operation_concluded_at')
                        )
                    )
                )),
                css_class="col-md-4", style="margin-top: 10px;"
            ),
            Div(
                FormPanel("Cleaning Log", Layout(
                    Field('probe_cleaned', template="bootstrap3/layout/radioselect-buttons.html"),
                    Field('ice_removed', template="bootstrap3/layout/radioselect-buttons.html"),
                    Field('oxygen_flow_stopped', template="bootstrap3/layout/radioselect-buttons.html"),
                    Field('oxygen_bottle_removed', template="bootstrap3/layout/radioselect-buttons.html"),
                    Field('box_cleaned', template="bootstrap3/layout/radioselect-buttons.html"),
                    Field('batteries_charged', template="bootstrap3/layout/radioselect-buttons.html"),
                    'cleaning_log'
                )),
                css_class="col-md-4", style="margin-top: 10px;"
            ),
            'person',
            'organ',
            'allocation'
        )

    class Meta:
        model = Recipient
        fields = [
            'person',
            'organ',
            'allocation',
            'signed_consent',
            'single_kidney_transplant',
            'renal_disease',
            'renal_disease_other',
            'pre_transplant_diuresis',
            'knife_to_skin',
            'perfusate_measure',
            'perfusion_stopped',
            'organ_cold_stored',
            'tape_broken',
            'removed_from_machine_at',
            'oxygen_full_and_open',
            'organ_untransplantable',
            'organ_untransplantable_reason',
            'anesthesia_started_at',
            'incision',
            'transplant_side',
            'arterial_problems',
            'arterial_problems_other',
            'venous_problems',
            'venous_problems_other',
            'anastomosis_started_at',
            'anastomosis_started_at_unknown',
            'reperfusion_started_at',
            'reperfusion_started_at_unknown',
            'mannitol_used',
            'other_diurectics',
            'other_diurectics_details',
            'systolic_blood_pressure',
            'cvp',
            'intra_operative_diuresis',
            'successful_conclusion',
            'operation_concluded_at',
            'probe_cleaned',
            'ice_removed',
            'oxygen_flow_stopped',
            'oxygen_bottle_removed',
            'box_cleaned',
            'batteries_charged',
            'cleaning_log',
        ]
        localized_fields = "__all__"

    def clean(self):
        cleaned_data = super(RecipientForm, self).clean()
        organ_untransplantable = cleaned_data.get("organ_untransplantable")
        organ_untransplantable_reason = cleaned_data.get("organ_untransplantable_reason")
        if organ_untransplantable is True and organ_untransplantable_reason == "":
            self.add_error(
                'organ_untransplantable_reason',
                forms.ValidationError(_("RFv01 Please enter a reason why this was untransplantable"))
            )

        if self.instance.organ.transplantation_form_completed is True:
            # Do the final validation checks
            if self.instance.organ.perfusion_started is not None:
                if cleaned_data.get("perfusion_stopped") == "":
                    self.add_error(
                        "perfusion_stopped",
                        forms.ValidationError(_("RFv02 Please enter time perfusion was stopped"))
                    )
                if cleaned_data.get("removed_from_machine_at") == "":
                    self.add_error(
                        "removed_from_machine_at",
                        forms.ValidationError(_("RFv03 Please enter time kidney was removed from machine"))
                    )
            if cleaned_data.get("anesthesia_started_at") == "":
                self.add_error(
                    "anesthesia_started_at",
                    forms.ValidationError(_("RFv04 Please enter time anesthesia was started"))
                )
            if cleaned_data.get("anastomosis_started_at") == "":
                self.add_error(
                    "anastomosis_started_at",
                    forms.ValidationError(_("RFv05 Please enter time anastomosis was started"))
                )
            if cleaned_data.get("reperfusion_started_at") == "":
                self.add_error(
                    "reperfusion_started_at",
                    forms.ValidationError(_("RFv06 Please enter time reperfusion was started"))
                )
        return cleaned_data


class TransplantOrganForm(forms.ModelForm):
    class Meta:
        model = Organ
        fields = [
            'transplantation_notes',
            'transplantation_form_completed',
        ]

    def __init__(self, *args, **kwargs):
        super(TransplantOrganForm, self).__init__(*args, **kwargs)
        self.fields['transplantation_form_completed'].choices = NO_YES_CHOICES

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            FormPanel("Complete Submission", Layout(
                'transplantation_notes',
                FieldWithFollowup(
                    Field('transplantation_form_completed', template="bootstrap3/layout/radioselect-buttons.html"),
                    HTML(
                        "<p class=\"text-danger\">Once all errors have been cleared, clicking Save below will " +
                        "result in this form being closed and locked. No further edits will be possible " +
                        "without contacting the admin team.</p>"
                    )
                )
            ), panel_status="danger"),  # , panel_hidden=True
        )
