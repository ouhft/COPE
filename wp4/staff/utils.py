#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from .models import Person

JACQUES_PIREENE = 109  # Staff.Person at v0.8.0
INA_JOCHMANS = 108  # Staff.Person at v0.8.0
SARAH_MERTENS = 6  # Staff.Person at v0.8.0
ALLY_BRADLEY = 3  # Staff.Person at v0.8.0


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


def generate_username(current_person):
    """
    Take the last name, and initials of first names, remove spaces and return as lowercase
    :param current_person: Person object requiring a new username
    :return string: username
    """
    last_name = ''.join(current_person.last_name.split())
    first_name = "".join(item[0] for item in current_person.first_name.split())
    username = "{0}{1}".format(first_name, last_name).lower()

    # Now test for collisions, until there are none
    i = 1
    while Person.objects.filter(username=username).exclude(pk=current_person.id).count() > 0:
        username += str(i)
        i += 1

    return username
