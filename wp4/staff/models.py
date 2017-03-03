#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib.auth.models import AbstractUser
from django.core.urlresolvers import reverse
from django.core.validators import RegexValidator
from django.db import models
from django.utils.translation import ugettext_lazy as _

from wp4.locations.models import Hospital


class Person(AbstractUser):
    """
    Replacement for original StaffPerson class which was linked to User as a one-to-one via "profile" attribute.

    This represents any person that is a member of staff (medical, administrative, academic, etc) and therefore not
    a patient (see compare.OrganPerson for that). Not all people with records here will be active users of the system

    Rather than base off of VersionControlMixin, we'll rely on Django-Reversion to capture changes to records, and thus
    the audit trail (so no created_by, created_on fields).
    """

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

    phone_regex = RegexValidator(
        regex=r'^\+?1?\d{9,15}$',
        message="Phone number must be entered in the format: '+999999999'. Up to 15 digits allowed."
    )

    telephone = models.CharField(
        verbose_name=_("SP01 telephone number"),
        validators=[phone_regex],
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

    class Meta:
        verbose_name = _('SPm1 person')
        verbose_name_plural = _('SPm2 people')

    def get_absolute_url(self):
        return reverse("wp4:staff:detail", kwargs={"pk": self.pk})
