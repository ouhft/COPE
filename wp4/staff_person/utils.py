#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from .models import StaffPerson

# Constants based on data from persons.json fixture
JACQUES_PIREENE = 3
INA_JOCHMANS = 4
SARAH_MERTENS = 38
ALLY_BRADLEY = 37


def get_emails_from_ids(list_of_ids=[]):
    email_list = []
    for sp_id in list_of_ids:
        try:
            person = StaffPerson.objects.get(pk=sp_id)
            if person.email is not None:
                email_list += [person.email, ]
        except StaffPerson.DoesNotExist:
            continue
    return email_list
