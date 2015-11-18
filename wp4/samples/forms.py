#!/usr/bin/python
# coding: utf-8
from django.forms import ModelForm
from django.forms.models import inlineformset_factory
# from django import forms
# from django.utils import timezone

# from crispy_forms.helper import FormHelper
# from crispy_forms.layout import Layout, Field

# from ..theme.layout import DateTimeField, DATETIME_INPUT_FORMATS
from .models import Event, Worksheet, UrineSample, BloodSample, TissueSample, PerfusateSample, Deviation


class WorksheetForm(ModelForm):

    def __init__(self, *args, **kwargs):
        super(WorksheetForm, self).__init__(*args, **kwargs)

    class Meta:
        model = Worksheet
        fields = ('barcode', 'person')

UrineSampleWorksheetFormSet = inlineformset_factory(
    Worksheet, UrineSample, fields=('barcode', 'person', 'event', 'centrifuged_at'))
BloodSampleWorksheetFormSet = inlineformset_factory(
    Worksheet, BloodSample, fields=('barcode', 'person', 'event', 'blood_type', 'centrifuged_at'))
PerfusateSampleWorksheetFormSet = inlineformset_factory(
    Worksheet, PerfusateSample, fields=('barcode', 'organ', 'event', 'centrifuged_at'))
TissueSampleWorksheetFormSet = inlineformset_factory(
    Worksheet, TissueSample, fields=('barcode', 'organ', 'event', 'tissue_type'))
DeviationFormSet = inlineformset_factory(
    Worksheet, Deviation, fields=('worksheet', 'description', 'occurred_at'))


class EventForm(ModelForm):
    class Meta:
        model = Event
        fields = ('type', 'taken_at')

UrineSampleEventFormSet = inlineformset_factory(
    Event, UrineSample, fields=('barcode', 'person', 'event', 'centrifuged_at'))
BloodSampleEventFormSet = inlineformset_factory(
    Event, BloodSample, fields=('barcode', 'person', 'event', 'blood_type', 'centrifuged_at'))
PerfusateSampleEventFormSet = inlineformset_factory(
    Event, PerfusateSample, fields=('barcode', 'organ', 'event', 'centrifuged_at'))
TissueSampleEventFormSet = inlineformset_factory(
    Event, TissueSample, fields=('barcode', 'organ', 'event', 'tissue_type'))






# class SampleForm(forms.ModelForm):
#     helper = FormHelper()
#     helper.form_tag = False
#     helper.html5_required = True
#     helper.layout = Layout(
#         Field('barcode', template="bootstrap3/layout/read-only.html"),
#         Field('type', template="bootstrap3/layout/read-only.html"),
#         DateTimeField('taken_at'),
#         DateTimeField('centrifugation'),
#         'comment'
#     )
#
#     class Meta:
#         model = Sample
#         exclude = ['created_by', 'created_on']
#         localized_fields = "__all__"
#
#     def __init__(self, *args, **kwargs):
#         super(SampleForm, self).__init__(*args, **kwargs)
#         self.fields['barcode'].widget = forms.HiddenInput()
#         self.fields['type'].widget = forms.HiddenInput()
#         self.fields['taken_at'].input_formats = DATETIME_INPUT_FORMATS
#         self.fields['centrifugation'].input_formats = DATETIME_INPUT_FORMATS
#
#     def save(self, user):
#         sample = super(SampleForm, self).save(commit=False)
#         sample.created_by = user
#         sample.created_on = timezone.now()
#         barcode_string = "undefined"
#         if sample.type in (Sample.DONOR_BLOOD_1, Sample.DONOR_BLOOD_2, Sample.DONOR_URINE_1, Sample.DONOR_URINE_2):
#             barcode_string = "%s:%s" % (sample.linked_to().trial_id(), sample.get_type_display())
#         if sample.type in (Sample.KIDNEY_PERFUSATE_1, Sample.KIDNEY_PERFUSATE_2, Sample.KIDNEY_PERFUSATE_3):
#             barcode_string = "%s:%s" % (sample.linked_to().donor.trial_id(), sample.get_type_display())
#         # TODO: Naming for Recipient Samples
#         sample.barcode = barcode_string
#         sample.save()
#         return sample
