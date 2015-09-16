#!/usr/bin/python
# coding: utf-8
from django import forms
from django.utils import timezone

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, Field

from ..theme.layout import DateTimeField, DATETIME_INPUT_FORMATS
from .models import Sample


class SampleForm(forms.ModelForm):
    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        Field('barcode', template="bootstrap3/layout/read-only.html"),
        Field('type', template="bootstrap3/layout/read-only.html"),
        DateTimeField('taken_at'),
        DateTimeField('centrifugation'),
        'comment'
    )

    class Meta:
        model = Sample
        exclude = ['created_by', 'created_on']
        localized_fields = "__all__"

    def __init__(self, *args, **kwargs):
        super(SampleForm, self).__init__(*args, **kwargs)
        self.fields['barcode'].widget = forms.HiddenInput()
        self.fields['type'].widget = forms.HiddenInput()
        self.fields['taken_at'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['centrifugation'].input_formats = DATETIME_INPUT_FORMATS

    def save(self, user):
        sample = super(SampleForm, self).save(commit=False)
        sample.created_by = user
        sample.created_on = timezone.now()
        barcode_string = "undefined"
        if sample.type in (Sample.DONOR_BLOOD_1, Sample.DONOR_BLOOD_2, Sample.DONOR_URINE_1, Sample.DONOR_URINE_2):
            barcode_string = "%s:%s" % (sample.linked_to().trial_id(), sample.get_type_display())
        if sample.type in (Sample.KIDNEY_PERFUSATE_1, Sample.KIDNEY_PERFUSATE_2, Sample.KIDNEY_PERFUSATE_3):
            barcode_string = "%s:%s" % (sample.linked_to().donor.trial_id(), sample.get_type_display())
        # TODO: Naming for Recipient Samples
        sample.barcode = barcode_string
        sample.save()
        return sample
