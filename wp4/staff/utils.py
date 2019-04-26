#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.core.validators import validate_email, ValidationError

from .models import Person

JACQUES_PIREENE = 109  # Staff.Person at v0.8.0
INA_JOCHMANS = 419  # Staff.Person at v0.9.3 -- See #337 about original account being overwritten by SM
SARAH_MERTENS = 6  # Staff.Person at v0.8.0
ALLY_BRADLEY = 3  # Staff.Person at v0.8.0
AUKJE_BRAT = 78  # Staff.Person at v0.8.0
BHUMIKA_PATEL = 97  # Staff.Person at v0.8.0


def get_emails_from_ids(list_of_ids=[]):
    email_list = []
    # print("DEBUG: get_emails_from_ids called with list_of_ids={0}".format(list_of_ids))
    for sp_id in list_of_ids:
        try:
            person = Person.objects.get(pk=sp_id)
            try:
                validate_email(person.email)
                email_list.append(person.email)
            except ValidationError as m:
                print("DEBUG: get_emails_from_ids ValidationError={0}".format(m))
        except Person.DoesNotExist:
            continue
    return email_list


def generate_username(current_person):
    """
    Take the last name, and initials of first names, remove spaces and return as lowercase
    
    :param current_person: Person object requiring a new username
    :return string: username
    """
    last_name = ''.join(current_person.last_name.replace('-','').split())
    # print("DEBUG: generate_username({0}): last_name={1}".format(current_person.last_name, last_name))
    first_name = "".join(item[0] for item in current_person.first_name.split())
    username = "{0}{1}".format(first_name, last_name).lower()

    # Now test for collisions, until there are none
    i = 1
    while Person.objects.filter(username=username).exclude(pk=current_person.id).count() > 0:
        username = "{0}{1}".format(first_name, last_name).lower() + str(i)
        i += 1

    # print("DEBUG: generate_username({0}): username={1}".format(current_person.last_name, username))
    return username
