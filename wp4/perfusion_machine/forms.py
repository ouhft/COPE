#!/usr/bin/python
# coding: utf-8
from django import forms

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout

from .models import PerfusionFile, PerfusionMachine


class PerfusionFileForm(forms.ModelForm):
    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        'machine',
        'file',
    )

    def __init__(self, data=None, *args, **kwargs):
        super(PerfusionFileForm, self).__init__(data=data, *args, **kwargs)
        self.render_required_fields = True

    class Meta:
        model = PerfusionFile
        fields = ('machine', 'file')
