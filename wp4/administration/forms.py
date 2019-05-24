#!/usr/bin/python
# coding: utf-8
from __future__ import unicode_literals

from django import forms
from django.utils.translation import ugettext_lazy as _

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, Div, HTML, Field

from wp4.compare.models.donor import Organ
from wp4.compare.models import YES_NO_NA_CHOICES
from wp4.compare.forms.core import YES_NO_CHOICES


class OrganAdminForm(forms.ModelForm):
    def __init__(self, *args, **kwargs):
        super(OrganAdminForm, self).__init__(*args, **kwargs)
        # self.render_required_fields = True
        self.fields['included_for_analysis'].choices = YES_NO_CHOICES
        self.fields['intention_to_treat'].choices = YES_NO_NA_CHOICES
        self.fields['actual_treatment_received'].choices = Organ.ATR_CHOICES

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            Field('included_for_analysis', template="bootstrap3/layout/radioselect-buttons.html"),
            'excluded_from_analysis_because',
            Field('intention_to_treat', template="bootstrap3/layout/radioselect-buttons.html"),
            Field('actual_treatment_received', template="bootstrap3/layout/radioselect-buttons.html"),
        )

    class Meta:
        model = Organ
        fields = (
            'included_for_analysis', 'excluded_from_analysis_because', 'intention_to_treat',
            'actual_treatment_received'
        )
