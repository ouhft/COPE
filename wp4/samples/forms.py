#!/usr/bin/python
# coding: utf-8
from django import forms
from django.conf import settings
from django.forms.models import inlineformset_factory
# from django import forms
# from django.utils import timezone
from django.utils.translation import ugettext_lazy as _

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, Field, Div

from ..theme.layout import FieldWithFollowup, DateTimeField, FormPanel
from .models import Event, Worksheet, UrineSample, BloodSample, TissueSample, PerfusateSample

# Common CONSTANTS
NO_YES_CHOICES = (
    (False, _("FF01 No")),
    (True, _("FF02 Yes")))

YES_NO_CHOICES = (
    (True, _("FF02 Yes")),
    (False, _("FF01 No")))


class WorksheetForm(forms.ModelForm):
    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        Div(Field('person', template="bootstrap3/layout/read-only.html"), css_class="col-md-6"),
        Div('barcode', css_class="col-md-6"),
    )

    def __init__(self, *args, **kwargs):
        super(WorksheetForm, self).__init__(*args, **kwargs)
        self.fields['person'].widget = forms.HiddenInput()

    class Meta:
        model = Worksheet
        fields = ('barcode', 'person')


class EventForm(forms.ModelForm):
    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        Div(Field('name', template="bootstrap3/layout/read-only.html"), css_class="col-md-6"),
        Div(DateTimeField('taken_at'), css_class="col-md-6"),
        'worksheet',
        'type',
    )

    def __init__(self, *args, **kwargs):
        super(EventForm, self).__init__(*args, **kwargs)
        self.render_required_fields = True
        self.fields['worksheet'].widget = forms.HiddenInput()
        # self.fields['type'].choices = Event.TYPE_CHOICES
        self.fields['type'].widget = forms.HiddenInput()
        self.fields['name'].widget = forms.HiddenInput()
        self.fields['taken_at'].input_formats = settings.DATETIME_INPUT_FORMATS

    class Meta:
        model = Event
        fields = ('worksheet', 'type', 'name', 'taken_at')


EventFormSet = inlineformset_factory(
    Worksheet, Event, form=EventForm, extra=0, can_delete=False)


class BloodSampleForm(forms.ModelForm):
    layout_collected = Layout(
        'barcode',
        DateTimeField('centrifuged_at'),
    )
    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        'event',
        'person',
        'created_by',
        Div(
            Field('blood_type', template="bootstrap3/layout/read-only.html"),
            css_class="col-md-4"
        ),
        Div(
            FieldWithFollowup(
                Field('collected', template="bootstrap3/layout/radioselect-buttons.html"),
                layout_collected
            ),
            css_class="col-md-4"
        ),
        Div(
            'notes',
            css_class="col-md-4"
        ))

    def __init__(self, *args, **kwargs):
        super(BloodSampleForm, self).__init__(*args, **kwargs)
        self.render_required_fields = True
        self.fields['event'].widget = forms.HiddenInput()
        self.fields['blood_type'].widget = forms.HiddenInput()
        self.fields['person'].widget = forms.HiddenInput()
        self.fields['collected'].choices = NO_YES_CHOICES
        self.fields['centrifuged_at'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['created_by'].widget = forms.HiddenInput()

    class Meta:
        model = BloodSample
        fields = ('event', 'collected', 'barcode', 'person', 'blood_type', 'centrifuged_at', 'notes', 'created_by')
        localized_fields = "__all__"


BloodSampleFormSet = inlineformset_factory(
    Event, BloodSample, form=BloodSampleForm, extra=0, can_delete=False)


class UrineSampleForm(forms.ModelForm):
    layout_collected = Layout(
        'barcode',
        DateTimeField('centrifuged_at'),
    )
    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        'event',
        'person',
        'created_by',
        Div(
            FieldWithFollowup(
                Field('collected', template="bootstrap3/layout/radioselect-buttons.html"),
                layout_collected
            ),
            css_class="col-md-8"
        ),
        Div(
            'notes',
            css_class="col-md-4"
        ))

    def __init__(self, *args, **kwargs):
        super(UrineSampleForm, self).__init__(*args, **kwargs)
        self.render_required_fields = True
        self.fields['event'].widget = forms.HiddenInput()
        self.fields['person'].widget = forms.HiddenInput()
        self.fields['collected'].choices = NO_YES_CHOICES
        self.fields['centrifuged_at'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['created_by'].widget = forms.HiddenInput()

    class Meta:
        model = UrineSample
        fields = ('event', 'collected', 'barcode', 'person', 'centrifuged_at', 'notes', 'created_by')
        localized_fields = "__all__"


UrineSampleFormSet = inlineformset_factory(
    Event, UrineSample, form=UrineSampleForm, extra=0, can_delete=False)


class PerfusateSampleForm(forms.ModelForm):
    layout_collected = Layout(
        'barcode',
        DateTimeField('centrifuged_at'),
    )
    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        'event',
        'organ',
        'created_by',
        Div(
            FieldWithFollowup(
                Field('collected', template="bootstrap3/layout/radioselect-buttons.html"),
                layout_collected
            ),
            css_class="col-md-8"
        ),
        Div(
            'notes',
            css_class="col-md-4"
        ))

    def __init__(self, *args, **kwargs):
        super(PerfusateSampleForm, self).__init__(*args, **kwargs)
        self.render_required_fields = True
        self.fields['event'].widget = forms.HiddenInput()
        self.fields['organ'].widget = forms.HiddenInput()
        self.fields['collected'].choices = NO_YES_CHOICES
        self.fields['centrifuged_at'].input_formats = settings.DATETIME_INPUT_FORMATS
        self.fields['created_by'].widget = forms.HiddenInput()

    class Meta:
        model = PerfusateSample
        fields = ('collected', 'barcode', 'organ', 'event', 'centrifuged_at', 'notes', 'created_by')
        localized_fields = "__all__"


PerfusateSampleFormSet = inlineformset_factory(
    Event, PerfusateSample, form=PerfusateSampleForm, extra=0, can_delete=False)


class TissueSampleForm(forms.ModelForm):
    layout_collected = Layout(
        'barcode',
    )

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        'event',
        'organ',
        'created_by',
        Div(
            Field('tissue_type', template="bootstrap3/layout/read-only.html"),
            css_class="col-md-4"
        ),
        Div(
            FieldWithFollowup(
                Field('collected', template="bootstrap3/layout/radioselect-buttons.html"),
                layout_collected
            ),
            css_class="col-md-4"
        ),
        Div(
            'notes',
            css_class="col-md-4"
        ))

    def __init__(self, *args, **kwargs):
        super(TissueSampleForm, self).__init__(*args, **kwargs)
        self.render_required_fields = True
        self.fields['event'].widget = forms.HiddenInput()
        self.fields['organ'].widget = forms.HiddenInput()
        self.fields['collected'].choices = NO_YES_CHOICES
        self.fields['tissue_type'].widget = forms.HiddenInput()
        self.fields['created_by'].widget = forms.HiddenInput()

    class Meta:
        model = TissueSample
        fields = ('collected', 'barcode', 'organ', 'event', 'tissue_type', 'notes', 'created_by')
        localized_fields = "__all__"


TissueSampleFormSet = inlineformset_factory(
    Event, TissueSample, form=TissueSampleForm, extra=0, can_delete=False)
