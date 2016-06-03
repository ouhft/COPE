#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django import forms

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout

from .models import Hospital


class HospitalForm(forms.ModelForm):
    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        'name',
        'country',
    )

    def __init__(self, data=None, *args, **kwargs):
        super(HospitalForm, self).__init__(data=data, *args, **kwargs)
        self.render_required_fields = True

    class Meta:
        model = Hospital
        fields = ('name', 'country')
