#!/usr/bin/python
# coding: utf-8
from __future__ import unicode_literals

from django import forms
from django.utils.translation import ugettext_lazy as _

from wp4.staff_person.models import StaffPerson
from wp4.compare.forms.core import YES_NO_CHOICES


class MergeStaffPersonForm(forms.Form):
    person1 = forms.ModelChoiceField(
        queryset=StaffPerson.objects.all().order_by('first_names', 'last_names'),
        label=_("MSPF01 Person 1")
    )
    person2 = forms.ModelChoiceField(
        queryset=StaffPerson.objects.all().order_by('first_names', 'last_names'),
        label=_("MSPF02 Person 2")
    )
    confirm = forms.BooleanField(
        initial=False,
        label=_("MSPF03 Are you sure?"),
        required=False  # Can't be required because the answer no is the same as no answer
    )

    def __init__(self, *args, **kwargs):
        super(MergeStaffPersonForm, self).__init__(*args, **kwargs)
        self.fields['confirm'].choices = YES_NO_CHOICES

    class Meta:
        localized_fields = '__all__'
