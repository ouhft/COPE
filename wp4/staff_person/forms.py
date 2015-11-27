#!/usr/bin/python
# coding: utf-8
from django import forms
from django.forms.models import inlineformset_factory

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, Field, Div
from crispy_forms.bootstrap import InlineCheckboxes

# from ..theme.layout import FieldWithFollowup, DateTimeField, FormPanel
from .models import StaffJob, StaffPerson


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

    def __init__(self, *args, **kwargs):
        super(StaffPersonForm, self).__init__(*args, **kwargs)
        self.render_required_fields = True
        # self.fields['jobs'].widget = forms.HiddenInput()


    class Meta:
        model = StaffPerson
        fields = ('user', 'first_names', 'last_names', 'telephone', 'email', 'based_at', 'jobs')

#
# class StaffJobForm(forms.ModelForm):
#     helper = FormHelper()
#     helper.form_tag = False
#     helper.html5_required = True
#     helper.layout = Layout(
#         'description',
#     )
#
#     # def __init__(self, *args, **kwargs):
#     #     super(StaffJobForm, self).__init__(*args, **kwargs)
#
#     class Meta:
#         model = StaffJob
#         fields = ('description',)
#
#
# StaffJobFormset = inlineformset_factory(
#     StaffPerson, StaffJob., form=StaffJobForm, min_num=1, validate_min=True
# )
