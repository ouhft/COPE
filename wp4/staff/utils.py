#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from .models import Person

# Constants based on data from persons.json fixture
#  TODO: Update these to match the new staff.Person references!
JACQUES_PIREENE = 3
INA_JOCHMANS = 4
SARAH_MERTENS = 38
ALLY_BRADLEY = 37


def get_emails_from_ids(list_of_ids=[]):
    email_list = []
    for sp_id in list_of_ids:
        try:
            person = Person.objects.get(pk=sp_id)
            if person.email is not None:
                email_list += [person.email, ]
        except Person.DoesNotExist:
            continue
    return email_list


def generate_username(first, last):
    """
    Take the last name, and initials of first names, remove spacea and return as lowercase
    :param first: first names as a string
    :param last: last names as a string
    :return string: username
    """
    last_name = ''.join(last.split())
    first_name = "".join(item[0] for item in first.split())
    return "{0}{1}".format(first_name, last_name).lower()
