#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib.auth.models import AbstractUser
from django.core.urlresolvers import reverse
from django.core.validators import RegexValidator
from django.db import models
from django.conf import settings
from django.utils.functional import cached_property
from django.utils.translation import ugettext_lazy as _


class Person(AbstractUser):
    """
    Replacement for original StaffPerson class which was linked to User as a one-to-one via "profile" attribute.

    This represents any person that is a member of staff (medical, administrative, academic, etc) and therefore not
    a patient (see compare.Patient for that). Not all people with records here will be active users of the system

    We'll rely on Django-Reversion to capture changes to records, and thus the audit trail.
    """
    from wp4.locations.models import Hospital

    # Constants to help reference specific staff groups (auth_groups, defined by fixtures)
    PERFUSION_TECHNICIAN = 1  #: Constant for Group ID
    TRANSPLANT_COORDINATOR = 2  #: Constant for Group ID
    RESEARCH_NURSE = 3  #: Constant for Group ID
    NATIONAL_COORDINATOR = 4  #: Constant for Group ID
    CENTRAL_COORDINATOR = 5  #: Constant for Group ID
    STATISTICIAN = 6  #: Constant for Group ID
    SYSTEMS_ADMINISTRATOR = 7  #: Constant for Group ID
    LOCAL_INVESTIGATOR = 8  #: Constant for Group ID
    OTHER_PROJECT_MEMBER = 9  #: Constant for Group ID
    BIOBANK_COORDINATOR = 10  #: Constant for Group ID
    CHIEF_INVESTIGATOR = 11  #: Constant for Group ID
    PRINCIPLE_INVESTIGATOR = 12  #: Constant for Group ID
    CENTRAL_INVESTIGATOR = 13  #: Constant for Group ID
    NATIONAL_INVESTIGATOR = 14  #: Constant for Group ID
    THEATRE_CONTACT = 15  #: Constant for Group ID

    _phone_regex = RegexValidator(
        regex=r'^\+?1?\d{9,15}$',
        message="Phone number must be entered in the format: '+999999999'. Up to 15 digits allowed."
    )
    _my_groups = None

    telephone = models.CharField(
        verbose_name=_("SP01 telephone number"),
        validators=[_phone_regex],
        max_length=15,
        blank=True,
        null=True
    )  #: Contents validated against phone_regex ``r'^\+?1?\d{9,15}$'``
    based_at = models.ForeignKey(
        Hospital,
        verbose_name=_("SP02 location"),
        blank=True,
        null=True,
        help_text="Link to a primary hospital location for the member of staff"
    )

    def has_group(self, group_ids=[]):
        if self._my_groups is None:
            self._my_groups = [g.id for g in self.groups.all()]
        # print("DEBUG: I am {2}, and my_group_ids={0} and group_ids={1} ({3})".format(
        #     my_group_ids, group_ids, self.get_full_name(), type(group_ids))
        # )
        if type(group_ids) in (list, tuple):
            for group in group_ids:
                if group in self._my_groups:
                    return True
        else:
            if group_ids in self._my_groups:
                return True
        # print("DEBUG: has_group returning False")
        return False

    def _is_administrator(self):
        """
        Checks for membership of an admin group
        :return:
        """
        administrator_groups = (self.NATIONAL_COORDINATOR, self.CENTRAL_COORDINATOR, self.SYSTEMS_ADMINISTRATOR,
                                self.BIOBANK_COORDINATOR, self.CHIEF_INVESTIGATOR, self.PRINCIPLE_INVESTIGATOR,
                                self.NATIONAL_INVESTIGATOR, self.CENTRAL_INVESTIGATOR,)
        if self.has_group(administrator_groups):
            return True
        elif self.has_group(self.CENTRAL_INVESTIGATOR) and settings.DEBUG is True:
            # Add a pass through for Ina when on the test server as per Issue #249
            return True
        return False

    is_administrator = cached_property(_is_administrator, name='is_administrator')

    class Meta:
        verbose_name = _('SPm1 person')
        verbose_name_plural = _('SPm2 people')
        permissions = (
            ("single_person", "Can only view their own person record"),
        )

    def get_absolute_url(self):
        return reverse("wp4:staff:detail", kwargs={"pk": self.pk})

    def __str__(self):
        return self.get_full_name()
