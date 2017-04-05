#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django import forms
from django.conf import settings

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, Field, Div

from wp4.theme.layout import DateField
from wp4.compare.models.transplantation import Recipient

from .models import QualityOfLife


class QualityOfLifeForm(forms.ModelForm):
    QOL_CHOICES = (
        (1, 1),
        (2, 2),
        (3, 3),
        (4, 4),
        (5, 5),
    )

    def __init__(self, data=None, *args, **kwargs):
        super(QualityOfLifeForm, self).__init__(data=data, *args, **kwargs)
        self.render_required_fields = True
        self.fields['recipient'].choices = Recipient.objects.\
            select_related('organ').values_list('id', 'organ__trial_id')
        self.fields['date_recorded'].input_formats = settings.DATE_INPUT_FORMATS
        self.fields['qol_mobility'].choices = self.QOL_CHOICES
        self.fields['qol_selfcare'].choices = self.QOL_CHOICES
        self.fields['qol_usual_activities'].choices = self.QOL_CHOICES
        self.fields['qol_pain'].choices = self.QOL_CHOICES
        self.fields['qol_anxiety'].choices = self.QOL_CHOICES

        self.helper = FormHelper(self)
        self.helper.form_tag = False
        self.helper.html5_required = True
        self.helper.layout = Layout(
            Div(
                Div(
                    'recipient',
                    Field('qol_mobility', template="bootstrap3/layout/radioselect-buttons.html"),
                    Field('qol_selfcare', template="bootstrap3/layout/radioselect-buttons.html"),
                    Field('qol_usual_activities', template="bootstrap3/layout/radioselect-buttons.html"),
                    css_class="col-md-6"
                ),
                Div(
                    DateField('date_recorded'),
                    Field('qol_pain', template="bootstrap3/layout/radioselect-buttons.html"),
                    Field('qol_anxiety', template="bootstrap3/layout/radioselect-buttons.html"),
                    'vas_score',
                    css_class="col-md-6"
                ),
                css_class="row",
            ),
        )

    class Meta:
        model = QualityOfLife
        fields = [
            'recipient',
            'date_recorded',
            'qol_mobility',
            'qol_selfcare',
            'qol_usual_activities',
            'qol_pain',
            'qol_anxiety',
            'vas_score',
        ]
        localized_fields = "__all__"

