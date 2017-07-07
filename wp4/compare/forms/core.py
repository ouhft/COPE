#!/usr/bin/python
# coding: utf-8
from __future__ import unicode_literals

from django import forms
from django.conf import settings
from django.utils.translation import ugettext_lazy as _

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, HTML, Field
from dal import autocomplete

from wp4.theme.layout import FieldWithFollowup, FieldWithNotKnown
from wp4.theme.layout import DateField, FormPanel
from ..models import Patient, Donor, Randomisation, Organ


# Common CONSTANTS
NO_YES_CHOICES = (
    (False, _("FF01 No")),
    (True, _("FF02 Yes"))
)

YES_NO_CHOICES = (
    (True, _("FF02 Yes")),
    (False, _("FF01 No"))
)


class OrganPersonForm(forms.ModelForm):
    layout_person = Layout(
        Field('number', placeholder="___ ___ ____"),
        FieldWithNotKnown(
            DateField('date_of_birth', notknown=True),
            'date_of_birth_unknown',
            label=Patient._meta.get_field("date_of_birth").verbose_name.title()
        ),
        Field('gender', template="bootstrap3/layout/radioselect-buttons.html"),
        'weight',
        'height',
        Field('ethnicity', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('blood_group', template="bootstrap3/layout/radioselect-buttons.html")
    )

    def __init__(self, *args, **kwargs):
        super(OrganPersonForm, self).__init__(*args, **kwargs)
        self.fields['number'].required = False
        self.fields['date_of_birth'].input_formats = settings.DATE_INPUT_FORMATS
        self.fields['gender'].choices = Patient.GENDER_CHOICES
        self.fields['ethnicity'].choices = Patient.ETHNICITY_CHOICES
        self.fields['blood_group'].choices = Patient.BLOOD_GROUP_CHOICES

        self.helper = FormHelper()
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            HTML("<div class=\"col-md-4\" style=\"margin-top: 10px\">"),
            FormPanel("Patient Description", self.layout_person)
        )

    class Meta:
        model = Patient
        fields = [
            'number', 'date_of_birth', 'date_of_birth_unknown',
            'gender', 'weight', 'height', 'ethnicity', 'blood_group'
        ]
        localized_fields = "__all__"


class DonorStartForm(forms.ModelForm):
    gender = forms.CharField(max_length=1, min_length=1)
    online = forms.BooleanField(initial=True, label=_("DSF02 Online Randomisation?"))
    randomisation = forms.ModelChoiceField(
        Randomisation.objects.filter(
            donor__isnull=True,
            list_code__in=[Randomisation.PAPER_EUROPE, Randomisation.PAPER_UNITED_KINGDOM]
        ),
        required=False,
        empty_label=_("DSF03 Not Applicable"),
        label=_("DSF01 Offline Case ID")
    )

    def __init__(self, *args, **kwargs):
        super(DonorStartForm, self).__init__(*args, **kwargs)
        self.fields['gender'].label = Patient._meta.get_field("gender").verbose_name.title()
        self.fields['gender'].choices = Patient.GENDER_CHOICES
        self.fields['online'].required = False
        self.fields['online'].choices = YES_NO_CHOICES

        self.helper = FormHelper()
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            'retrieval_team',
            'perfusion_technician',
            'age',
            Field('gender', template="bootstrap3/layout/radioselect-buttons.html"),
            FieldWithFollowup(
                Field('online', template="bootstrap3/layout/radioselect-buttons.html"),
                'randomisation'
            )
        )

    class Meta:
        model = Donor
        fields = ['retrieval_team', 'perfusion_technician', 'age', 'gender']
        widgets = {
            'perfusion_technician': autocomplete.ModelSelect2(url='wp4:staff:technician-autocomplete'),
            'retrieval_team': autocomplete.ModelSelect2(url='wp4:compare:retrieval-team-autocomplete')
        }
        localized_fields = '__all__'

    def clean(self):
        cleaned_data = super(DonorStartForm, self).clean()
        online = cleaned_data.get("online")
        if not online:
            randomisation = cleaned_data.get("randomisation")
            retrieval_team = cleaned_data.get("retrieval_team")
            if not randomisation:
                self.add_error('randomisation', forms.ValidationError("Please select an Offline Case ID"))
            elif randomisation.list_code != retrieval_team.get_randomisation_list(is_online=False):
                self.add_error(
                    'randomisation',
                    forms.ValidationError("Please select an Offline Case ID for the same region as the Retrieval team")
                )
        return cleaned_data


class AllocationStartForm(forms.Form):
    organ = forms.ModelChoiceField(
        queryset=Organ.allocatable_objects.order_by('trial_id'),
        # widget=autocomplete.ModelSelect2(url='wp4:compare:transplantable-organs-autocomplete'),
        label=_("ASF01 Select trial id for case")
    )
    allocated = forms.BooleanField(
        initial=True,
        label=_("ASF02 Has this been allocated to a recipient?"),
        required=False  # Can't be required because the answer no is the same as no answer
    )
    not_allocated_reason = forms.CharField(
        max_length=250,
        min_length=0,
        label=_("ASF03 Why was this not allocated?"),
        required=False
    )

    def __init__(self, *args, **kwargs):
        super(AllocationStartForm, self).__init__(*args, **kwargs)
        self.fields['allocated'].choices = YES_NO_CHOICES

        self.helper = FormHelper()
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            'organ',
            FieldWithFollowup(
                Field('allocated', template="bootstrap3/layout/radioselect-buttons.html"),
                'not_allocated_reason'
            )
        )

    class Meta:
        localized_fields = '__all__'

    def clean(self):
        cleaned_data = super(AllocationStartForm, self).clean()
        allocated = cleaned_data.get("allocated")
        reason = cleaned_data.get("not_allocated_reason")
        if not allocated and not reason:
            self.add_error('not_allocated_reason', forms.ValidationError("Please enter an explanation"))
        return cleaned_data
