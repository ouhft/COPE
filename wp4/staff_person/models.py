#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.db import models
from django.contrib.auth.models import User
from django.core.urlresolvers import reverse
from django.core.validators import RegexValidator
from django.utils import six
from django.utils.translation import ugettext_lazy as _

from wp4.locations.models import Hospital
from wp4.compare.models import VersionControlMixin


class StaffJob(models.Model):
    """
    Helper class for the range of Job/roles that are defined in the project protocol. These aren't
    integrated with the Django permissions system, though there is potential to do that as a refactor.

    This data is taken from a preset fixture list (persons.json), with constants only defined for
    key jobs used within the system.
    """
    PERFUSION_TECHNICIAN = 1  #: Constant for StaffJob
    TRANSPLANT_COORDINATOR = 2  #: Constant for StaffJob
    RESEARCH_NURSE = 3  #: Constant for StaffJob
    NATIONAL_COORDINATOR = 4  #: Constant for StaffJob
    CENTRAL_COORDINATOR = 5  #: Constant for StaffJob
    STATISTICIAN = 6  #: Constant for StaffJob
    SYSTEMS_ADMINISTRATOR = 7  #: Constant for StaffJob
    LOCAL_INVESTIGATOR = 8  #: Constant for StaffJob
    OTHER_PROJECT_MEMBER = 9  #: Constant for StaffJob
    BIOBANK_COORDINATOR = 10  #: Constant for StaffJob
    CHIEF_INVESTIGATOR = 11  #: Constant for StaffJob
    PRINCIPLE_INVESTIGATOR = 12  #: Constant for StaffJob
    CENTRAL_INVESTIGATOR = 13  #: Constant for StaffJob
    NATIONAL_INVESTIGATOR = 14  #: Constant for StaffJob
    THEATRE_CONTACT = 15  #: Constant for StaffJob

    description = models.CharField(max_length=100, help_text="Job Label")

    def __str__(self):
        return self.description


class StaffPerson(VersionControlMixin):
    """
    Base person record that links with the Django User record to add further metadata relevant to the
    project management. Doesn't extend User to avoid complications of doing so.
    """
    phone_regex = RegexValidator(
        regex=r'^\+?1?\d{9,15}$',
        message="Phone number must be entered in the format: '+999999999'. Up to 15 digits allowed."
    )

    user = models.OneToOneField(
        User,
        verbose_name=_("PE14 related user account"),
        blank=True, null=True,
        related_name="profile"
    )
    first_names = models.CharField(verbose_name=_("PE10 first names"), max_length=50)
    last_names = models.CharField(verbose_name=_("PE11 last names"), max_length=50)
    jobs = models.ManyToManyField(StaffJob, verbose_name=_("PE12 jobs"))
    telephone = models.CharField(
        verbose_name=_("PE13 telephone number"),
        validators=[phone_regex],
        max_length=15,
        blank=True
    )  #: Contents validated against phone_regex ``r'^\+?1?\d{9,15}$'``
    email = models.EmailField(verbose_name=_("PE15 email"), blank=True, unique=True)
    based_at = models.ForeignKey(
        Hospital,
        blank=True, null=True,
        help_text="Link to a primary hospital location for the member of staff"
    )

    @property
    def full_name(self):
        return self.first_names + ' ' + self.last_names
    full_name.short_description = 'Name'

    def has_job(self, acceptable_jobs):
        """
        Dual purpose function. Will comfirm or deny this person has one or more of the jobs specified
        if a list of jobs is is submitted, otherwise will confirm if a specified job is in this person's
        list of jobs

        :param acceptable_jobs: list, tuple, int, or long, representing StaffJob IDs
        :return: bool related to whether given input is in person's jobs list
        :rtype: bool
        """
        jobs_list = [x.id for x in self.jobs.all()]
        if type(acceptable_jobs) is list or type(acceptable_jobs) is tuple:
            answer = [x for x in jobs_list if x in acceptable_jobs]
            return True if len(answer) > 0 else False
        elif isinstance(acceptable_jobs, six.integer_types):
            return acceptable_jobs in jobs_list
        else:
            raise TypeError("acceptable jobs is an invalid type")

    def __str__(self):
        return self.full_name

    class Meta:
        unique_together = ('first_names', 'last_names')
        verbose_name = _('PEm1 person')
        verbose_name_plural = _('PEm2 people')

    def get_absolute_url(self):
        return reverse("wp4:staff_person:detail", kwargs={"pk": self.pk})
