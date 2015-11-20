#!/usr/bin/python
# coding: utf-8
from django import forms
from django.forms.models import inlineformset_factory
# from django import forms
# from django.utils import timezone
from django.utils.translation import ugettext_lazy as _

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, Field

from ..theme.layout import FieldWithFollowup, DateTimeField, DateField, DATETIME_INPUT_FORMATS, DATE_INPUT_FORMATS
from .models import Event, Worksheet, UrineSample, BloodSample, TissueSample, PerfusateSample

# Common CONSTANTS
NO_YES_CHOICES = (
    (False, _("FF01 No")),
    (True, _("FF02 Yes")))

YES_NO_CHOICES = (
    (True, _("FF02 Yes")),
    (False, _("FF01 No")))

#
# class WorksheetForm(forms.ModelForm):
#     helper = FormHelper()
#     helper.form_tag = False
#     helper.html5_required = True
#     helper.layout = Layout(
#         'barcode',
#         'person')
#
#     def __init__(self, *args, **kwargs):
#         super(WorksheetForm, self).__init__(*args, **kwargs)
#
#     class Meta:
#         model = Worksheet
#         fields = ('barcode', 'person')
#
#
# class UrineSampleForm(forms.ModelForm):
#     helper = FormHelper()
#     helper.form_tag = False
#     helper.html5_required = True
#     helper.layout = Layout(
#         'worksheet',
#         'collected',
#         'barcode',
#         'person',
#         'event',
#         DateField('centrifuged_at'),
#         'created_by',
#         'notes')
#
#     def __init__(self, *args, **kwargs):
#         super(UrineSampleForm, self).__init__(*args, **kwargs)
#         self.render_required_fields = True
#         self.fields['worksheet'].widget = forms.HiddenInput()
#         self.fields['centrifuged_at'].input_formats = DATE_INPUT_FORMATS
#         self.fields['created_by'].widget = forms.HiddenInput()
#
#     class Meta:
#         model = UrineSample
#         fields = ('worksheet', 'collected', 'barcode', 'person', 'event', 'centrifuged_at', 'notes', 'created_by')
#         localized_fields = "__all__"
#
# UrineSampleWorksheetFormSet = inlineformset_factory(
#     Worksheet, UrineSample, form=UrineSampleForm, extra=1, can_delete=True)
#
#
# class BloodSampleForm(forms.ModelForm):
#     helper = FormHelper()
#     helper.form_tag = False
#     helper.html5_required = True
#     helper.layout = Layout(
#         'worksheet',
#         'collected',
#         'barcode',
#         'person',
#         'event',
#         Field('blood_type', template="bootstrap3/layout/radioselect-buttons.html"),
#         DateField('centrifuged_at'),
#         'created_by',
#         'notes')
#
#     def __init__(self, *args, **kwargs):
#         super(BloodSampleForm, self).__init__(*args, **kwargs)
#         self.render_required_fields = True
#         self.fields['worksheet'].widget = forms.HiddenInput()
#         self.fields['centrifuged_at'].input_formats = DATE_INPUT_FORMATS
#         self.fields['blood_type'].input_formats = BloodSample.SAMPLE_CHOICES
#         self.fields['created_by'].widget = forms.HiddenInput()
#
#     class Meta:
#         model = BloodSample
#         fields = ('worksheet', 'collected', 'barcode', 'person', 'event', 'blood_type', 'centrifuged_at', 'notes', 'created_by')
#         localized_fields = "__all__"
#
# BloodSampleWorksheetFormSet = inlineformset_factory(
#     Worksheet, BloodSample, form=BloodSampleForm, extra=2, can_delete=True)
#
#
# class PerfusateSampleForm(forms.ModelForm):
#     helper = FormHelper()
#     helper.form_tag = False
#     helper.html5_required = True
#     helper.layout = Layout(
#         'worksheet',
#         'collected',
#         'barcode',
#         'organ',
#         'event',
#         DateField('centrifuged_at'),
#         'created_by',
#         'notes')
#
#     def __init__(self, *args, **kwargs):
#         super(PerfusateSampleForm, self).__init__(*args, **kwargs)
#         self.render_required_fields = True
#         self.fields['worksheet'].widget = forms.HiddenInput()
#         self.fields['centrifuged_at'].input_formats = DATE_INPUT_FORMATS
#         self.fields['created_by'].widget = forms.HiddenInput()
#
#     class Meta:
#         model = PerfusateSample
#         fields = ('worksheet', 'collected', 'barcode', 'organ', 'event', 'centrifuged_at', 'notes', 'created_by')
#         localized_fields = "__all__"
#
# PerfusateSampleWorksheetFormSet = inlineformset_factory(
#     Worksheet, PerfusateSample, form=PerfusateSampleForm, extra=1, can_delete=True)
#
#
# class TissueSampleForm(forms.ModelForm):
#     layout_collected = Layout(
#         'barcode',
#         'event',
#         Field('tissue_type', template="bootstrap3/layout/radioselect-buttons.html"),
#     )
#
#     helper = FormHelper()
#     helper.form_tag = False
#     helper.html5_required = True
#     helper.layout = Layout(
#         'organ',
#         FieldWithFollowup(
#             Field('collected', template="bootstrap3/layout/radioselect-buttons.html"),
#             layout_collected
#         ),
#         'notes',
#         'worksheet',
#         'created_by',)
#
#     def __init__(self, *args, **kwargs):
#         super(TissueSampleForm, self).__init__(*args, **kwargs)
#         self.render_required_fields = True
#         self.fields['collected'].choices = NO_YES_CHOICES
#         self.fields['worksheet'].widget = forms.HiddenInput()
#         self.fields['tissue_type'].input_formats = TissueSample.SAMPLE_CHOICES
#         self.fields['created_by'].widget = forms.HiddenInput()
#
#     class Meta:
#         model = TissueSample
#         fields = ('worksheet', 'collected', 'barcode', 'organ', 'event', 'tissue_type', 'notes', 'created_by')
#         localized_fields = "__all__"
#
# TissueSampleWorksheetFormSet = inlineformset_factory(
#     Worksheet, TissueSample, form=TissueSampleForm, extra=2, can_delete=True)
#
#
# class EventForm(forms.ModelForm):
#     helper = FormHelper()
#     helper.form_tag = False
#     helper.html5_required = True
#     helper.layout = Layout(
#         Field('type', template="bootstrap3/layout/radioselect-buttons.html"),
#         DateField('taken_at'),)
#
#     def __init__(self, *args, **kwargs):
#         super(EventForm, self).__init__(*args, **kwargs)
#         self.fields['type'].choices = Event.TYPE_CHOICES
#         self.fields['taken_at'].input_formats = DATE_INPUT_FORMATS
#
#     class Meta:
#         model = Event
#         fields = ('type', 'taken_at')
#
# UrineSampleEventFormSet = inlineformset_factory(
#     Event, UrineSample, form=UrineSampleForm, extra=1, can_delete=True)
# BloodSampleEventFormSet = inlineformset_factory(
#     Event, BloodSample, form=BloodSampleForm, extra=2, can_delete=True)
# PerfusateSampleEventFormSet = inlineformset_factory(
#     Event, PerfusateSample, form=PerfusateSampleForm, extra=1, can_delete=True)
# TissueSampleEventFormSet = inlineformset_factory(
#     Event, TissueSample, form=TissueSampleForm, extra=2, can_delete=True)
