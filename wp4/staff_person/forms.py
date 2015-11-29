#!/usr/bin/python
# coding: utf-8
import re

from django import forms

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout
from crispy_forms.bootstrap import InlineCheckboxes

from .models import StaffPerson


class StaffPersonForm(forms.ModelForm):
    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        'first_names',
        'last_names',
        'based_at',
        'user',
        'telephone',
        'email',
        InlineCheckboxes('jobs'),
    )

    def __init__(self, data=None, *args, **kwargs):
        # Special intercept and clean of data to account for jQuery.serialise data mangling of lists
        if data is not None:
            jquery_serialise_list_pattern = re.compile("^\[(\d+,?\s?)+\]$")
            if isinstance(data['jobs'], type(u'')) and jquery_serialise_list_pattern.match(data['jobs']):
                data = data.copy()  # make it mutable
                data['jobs'] = [int(i) for i in data['jobs'][1:-1].split(",")]
        super(StaffPersonForm, self).__init__(data=data, *args, **kwargs)
        self.render_required_fields = True

    class Meta:
        model = StaffPerson
        fields = ('user', 'first_names', 'last_names', 'telephone', 'email', 'based_at', 'jobs')
