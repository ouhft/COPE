#!/usr/bin/python
# coding: utf-8
from django import forms
from django.conf import settings
from django.utils.translation import ugettext_lazy as _

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, HTML, Field
from autocomplete_light.fields import ModelChoiceField

from wp4.theme.layout import FieldWithFollowup, FieldWithNotKnown
from wp4.theme.layout import DateField, FormPanel
from ..models import OrganPerson, Donor, Randomisation, Organ
from ..models import PAPER_EUROPE, PAPER_UNITED_KINGDOM


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
            label=OrganPerson._meta.get_field("date_of_birth").verbose_name.title()
        ),
        Field('gender', template="bootstrap3/layout/radioselect-buttons.html"),
        'weight',
        'height',
        Field('ethnicity', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('blood_group', template="bootstrap3/layout/radioselect-buttons.html")
    )

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        HTML("<div class=\"col-md-4\" style=\"margin-top: 10px\">"),
        FormPanel("Patient Description", layout_person)
    )

    def __init__(self, *args, **kwargs):
        super(OrganPersonForm, self).__init__(*args, **kwargs)
        self.fields['number'].required = False
        self.fields['date_of_birth'].input_formats = settings.DATE_INPUT_FORMATS
        self.fields['gender'].choices = OrganPerson.GENDER_CHOICES
        self.fields['ethnicity'].choices = OrganPerson.ETHNICITY_CHOICES
        self.fields['blood_group'].choices = OrganPerson.BLOOD_GROUP_CHOICES

    class Meta:
        model = OrganPerson
        fields = [
            'number', 'date_of_birth', 'date_of_birth_unknown',
            'gender', 'weight', 'height', 'ethnicity', 'blood_group'
        ]
        localized_fields = "__all__"

    def save(self, user=None, *args, **kwargs):
        person = super(OrganPersonForm, self).save(commit=False)
        if kwargs.get("commit", True):
            if user is None:
                raise Exception("No user specified when saving OrganPerson Form")
            person.save(created_by=user)
        return person


class DonorStartForm(forms.ModelForm):
    perfusion_technician = ModelChoiceField('TechnicianAutoComplete')
    gender = forms.CharField(max_length=1, min_length=1)
    online = forms.BooleanField(initial=True, label=_("DSF02 Online Randomisation?"))
    randomisation = forms.ModelChoiceField(
        Randomisation.objects.filter(
            donor__isnull=True,
            list_code__in=[PAPER_EUROPE, PAPER_UNITED_KINGDOM]
        ),
        required=False,
        empty_label=_("DSF03 Not Applicable"),
        label=_("DSF01 Offline Case ID")
    )

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        'retrieval_team',
        'perfusion_technician',
        'age',
        Field('gender', template="bootstrap3/layout/radioselect-buttons.html"),
        FieldWithFollowup(
            Field('online', template="bootstrap3/layout/radioselect-buttons.html"),
            'randomisation'
        )
    )

    def __init__(self, *args, **kwargs):
        super(DonorStartForm, self).__init__(*args, **kwargs)
        self.fields['perfusion_technician'].label = Donor._meta.get_field("perfusion_technician").verbose_name.title()
        self.fields['gender'].label = OrganPerson._meta.get_field("gender").verbose_name.title()
        self.fields['gender'].choices = OrganPerson.GENDER_CHOICES
        self.fields['online'].required = False
        self.fields['online'].choices = YES_NO_CHOICES

    class Meta:
        model = Donor
        fields = ['retrieval_team', 'perfusion_technician', 'age', 'gender']
        localized_fields = '__all__'

    def save(self, user=None, *args, **kwargs):
        donor = super(DonorStartForm, self).save(commit=False)
        if kwargs.get("commit", True):
            if user is None:
                raise Exception("No user specified when saving DonorStartForm")
            donor.save(created_by=user)
        return donor

    def clean(self):
        cleaned_data = super(DonorStartForm, self).clean()
        online = cleaned_data.get("online")
        if not online:
            randomisation = cleaned_data.get("randomisation")
            retrieval_team = cleaned_data.get("retrieval_team")
            if not randomisation:
                self.add_error('randomisation', forms.ValidationError("Please select an Offline Case ID"))
            elif randomisation.list_code != retrieval_team.get_randomisation_list(False):
                self.add_error(
                    'randomisation',
                    forms.ValidationError("Please select an Offline Case ID for the same region as the Retrieval team")
                )
        return cleaned_data


class AllocationStartForm(forms.Form):
    organ = forms.ModelChoiceField(
        Organ.allocatable_objects.all(),
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

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        'organ',
        # This counter doesn't work because it is only accurate at compile time
        # HTML("<p> " + str(Organ.allocatable_objects.count()) + " available organs</p>"),
        FieldWithFollowup(
            Field('allocated', template="bootstrap3/layout/radioselect-buttons.html"),
            'not_allocated_reason'
        )
    )

    def __init__(self, *args, **kwargs):
        super(AllocationStartForm, self).__init__(*args, **kwargs)
        self.fields['allocated'].choices = YES_NO_CHOICES

    class Meta:
        localized_fields = '__all__'

    def clean(self):
        cleaned_data = super(AllocationStartForm, self).clean()
        allocated = cleaned_data.get("allocated")
        reason = cleaned_data.get("not_allocated_reason")
        if not allocated and not reason:
            self.add_error('not_allocated_reason', forms.ValidationError("Please enter an explanation"))
        return cleaned_data
