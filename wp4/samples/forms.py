#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django import forms
from django.conf import settings
from django.contrib.contenttypes.models import ContentType
from django.forms.models import inlineformset_factory
# from django.utils import timezone
from django.utils.translation import ugettext_lazy as _


from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, Field, Div, HTML

from ..theme.layout import FieldWithFollowup, DateTimeField, FormPanel
from .models import Event, UrineSample, BloodSample, TissueSample, PerfusateSample, WP7Record
from .utils import number_as_str

# Common CONSTANTS
NO_YES_CHOICES = (
    (False, _("FF01 No")),
    (True, _("FF02 Yes")))

YES_NO_CHOICES = (
    (True, _("FF02 Yes")),
    (False, _("FF01 No")))


class SampleWP7SaveMixin(forms.ModelForm):
    def save(self, commit=True):
        """
        Checks if the barcode has been changed, and if so, after the save, attempt to run a relink function
        to the WP7 data that exists. Remember that unlinking may be needed too

        :param commit:
        :return:
        """
        # print("DEBUG: SampleWP7SaveMixin.save(): form changed={0}, changes={1}".format(
        #     self.has_changed(), self.changed_data
        # ))

        self.instance = super(SampleWP7SaveMixin, self).save(commit=commit)

        if self.has_changed() and 'barcode' in self.changed_data:
            # Unlink anything linked to this record, as a change implies this will be wrong
            old_records = WP7Record.objects.filter(
                object_id=self.instance.id,
                content_type=ContentType.objects.get_for_model(self.instance)
            )
            for record in old_records:
                # print("DEBUG: SampleWP7SaveMixin.save(): record {0} unlinked".format(record))
                record.content_object = None
                record.save()

            # Now link (if barcode exists)
            try:
                wp7_record = WP7Record.objects.get(barcode__exact=self.instance.barcode)

                # print("DEBUG: SampleWP7SaveMixin.save(): wp7_record={0}".format(wp7_record))
                if wp7_record is not None:
                    wp7_record.content_object = self.instance
                    wp7_record.save()
            except WP7Record.DoesNotExist:
                pass


class EventForm(forms.ModelForm):
    def __init__(self, *args, **kwargs):
        super(EventForm, self).__init__(*args, **kwargs)
        self.render_required_fields = True
        self.fields['type'].widget = forms.HiddenInput()
        self.fields['name'].widget = forms.HiddenInput()
        self.fields['taken_at'].input_formats = settings.DATETIME_INPUT_FORMATS

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            Div(Field('name', template="bootstrap3/layout/read-only.html"), css_class="col-md-6"),
            Div(DateTimeField('taken_at'), css_class="col-md-6"),
            'type',
        )

    class Meta:
        model = Event
        fields = ('type', 'name', 'taken_at')


class BloodSampleForm(SampleWP7SaveMixin):
    layout_collected = Layout(
        'barcode',
        DateTimeField('centrifuged_at'),
    )

    def __init__(self, *args, **kwargs):
        super(BloodSampleForm, self).__init__(*args, **kwargs)
        self.render_required_fields = True
        self.fields['event'].widget = forms.HiddenInput()
        self.fields['blood_type'].widget = forms.HiddenInput()
        self.fields['person'].widget = forms.HiddenInput()
        self.fields['collected'].choices = NO_YES_CHOICES
        self.fields['centrifuged_at'].input_formats = settings.DATETIME_INPUT_FORMATS

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            'event',
            'person',
            Div(
                Field('blood_type', template="bootstrap3/layout/read-only.html"),
                css_class="col-md-4"
            ),
            Div(
                FieldWithFollowup(
                    Field('collected', template="bootstrap3/layout/radioselect-buttons.html"),
                    self.layout_collected
                ),
                css_class="col-md-4"
            ),
            Div(
                'notes',
                css_class="col-md-4"
            )
        )

    class Meta:
        model = BloodSample
        fields = ('event', 'collected', 'barcode', 'person', 'blood_type', 'centrifuged_at', 'notes')
        localized_fields = "__all__"

BloodSampleFormSet = inlineformset_factory(
    Event, BloodSample, form=BloodSampleForm, extra=0, can_delete=False
)


class UrineSampleForm(SampleWP7SaveMixin):
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

    class Meta:
        model = UrineSample
        fields = ('event', 'collected', 'barcode', 'person', 'centrifuged_at', 'notes')
        localized_fields = "__all__"


UrineSampleFormSet = inlineformset_factory(
    Event, UrineSample, form=UrineSampleForm, extra=0, can_delete=False)


class PerfusateSampleForm(SampleWP7SaveMixin):
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

    class Meta:
        model = PerfusateSample
        fields = ('collected', 'barcode', 'organ', 'event', 'centrifuged_at', 'notes')
        localized_fields = "__all__"


PerfusateSampleFormSet = inlineformset_factory(
    Event, PerfusateSample, form=PerfusateSampleForm, extra=0, can_delete=False)


class TissueSampleForm(SampleWP7SaveMixin):
    layout_collected = Layout(
        'barcode',
    )

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        'event',
        'organ',
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

    class Meta:
        model = TissueSample
        fields = ('collected', 'barcode', 'organ', 'event', 'tissue_type', 'notes')
        localized_fields = "__all__"


TissueSampleFormSet = inlineformset_factory(
    Event, TissueSample, form=TissueSampleForm, extra=0, can_delete=False)


class WP7RecordForm(forms.ModelForm):
    class Meta:
        model = WP7Record
        fields = [
            'barcode',
            'box_number',
            'position_in_box'
        ]

        def clean_barcode(self):
            barcode = self.cleaned_data['barcode']
            return number_as_str(barcode)

        def clean_position_in_box(self):
            position_in_box = self.cleaned_data['position_in_box']
            return number_as_str(position_in_box)


class WP7FileForm(forms.Form):
    """
    Handle the uploading of data from the WP7 database
    """
    file = forms.FileField(label="WP7 Excel File")

    def __init__(self, *args, **kwargs):
        super(WP7FileForm, self).__init__(*args, **kwargs)
        self.fields['file'].widget.attrs.update({'class': 'form-control'})

