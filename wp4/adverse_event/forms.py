#!/usr/bin/python
# coding: utf-8
from django import forms
from django.utils import timezone

from crispy_forms.helper import FormHelper
from crispy_forms.layout import Div

from .models import AdverseEvent


class AdverseEventForm(forms.ModelForm):
    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Div()

    def __init__(self, *args, **kwargs):
        super(AdverseEventForm, self).__init__(*args, **kwargs)

    class Meta:
        model = AdverseEvent
        exclude = ['created_by', 'version', 'created_on', 'record_locked']
        localized_fields = "__all__"

    def save(self, user):
        recipient = super(AdverseEventForm, self).save(commit=False)
        recipient.created_by = user
        recipient.created_on = timezone.now()
        recipient.version += 1
        recipient.save()
        return recipient
