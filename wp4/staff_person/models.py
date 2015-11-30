#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.db import models
from django.contrib.auth.models import User
from django.core.urlresolvers import reverse
from django.core.validators import RegexValidator
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _, ungettext_lazy as __

from ..locations.models import Hospital


# TODO: Stop this being implemented twice (see also compare.models). Here to stop an import error with circular import
class VersionControlModel(models.Model):
    version = models.PositiveIntegerField(default=0)
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)
    record_locked = models.BooleanField(default=False)

    # TODO: Add save method here that aborts saving if record_locked is already true
    # TODO: Add version control via django-reversion

    class Meta:
        abstract = True

    def save(self, force_insert=False, force_update=False, using=None,
             update_fields=None):
        self.created_on = timezone.now()
        self.version += 1
        return super(VersionControlModel, self).save(force_insert, force_update, using, update_fields)


class StaffJob(models.Model):
    # pk values for StaffJob taken from fixtures/persons.json
    PERFUSION_TECHNICIAN = 1
    TRANSPLANT_COORDINATOR = 2
    RESEARCH_NURSE = 3
    NATIONAL_COORDINATOR = 4
    CENTRAL_COORDINATOR = 5
    STATISTICIAN = 6
    SYSTEMS_ADMINISTRATOR = 7
    LOCAL_INVESTIGATOR = 8
    OTHER_PROJECT_MEMBER = 9
    BIOBANK_COORDINATOR = 10

    description = models.CharField(max_length=100)
    # TODO: Work out how to get localised values from this

    def __unicode__(self):
        return self.description


# Create your models here.
class StaffPerson(VersionControlModel):
    phone_regex = RegexValidator(
        regex=r'^\+?1?\d{9,15}$',
        message="Phone number must be entered in the format: '+999999999'. Up to 15 digits allowed."
    )

    user = models.OneToOneField(User, verbose_name=_("PE14 related user account"), blank=True, null=True,
                                related_name="profile")
    first_names = models.CharField(verbose_name=_("PE10 first names"), max_length=50)
    last_names = models.CharField(verbose_name=_("PE11 last names"), max_length=50)
    jobs = models.ManyToManyField(StaffJob, verbose_name=_("PE12 jobs"))
    telephone = models.CharField(verbose_name=_("PE13 telephone number"), validators=[phone_regex],
                                 max_length=15, blank=True)
    email = models.EmailField(verbose_name=_("PE15 email"), blank=True)
    based_at = models.ForeignKey(Hospital, blank=True, null=True)

    def full_name(self):
        return self.first_names + ' ' + self.last_names
    full_name.short_description = 'Name'

    def has_job(self, acceptable_jobs):
        jobs_list = [x.id for x in self.jobs.all()]
        if type(acceptable_jobs) is list or type(acceptable_jobs) is tuple:
            answer = [x for x in jobs_list if x in acceptable_jobs]
            return True if len(answer) > 0 else False
        elif isinstance(acceptable_jobs, (int, long)):
            return acceptable_jobs in jobs_list
        else:
            raise TypeError("acceptable jobs is an invalid type")

    def __unicode__(self):
        return self.full_name()  # + ' : ' + self.get_job_display()  TODO: List jobs?

    class Meta:
        verbose_name = _('PEm1 person')
        verbose_name_plural = _('PEm2 people')

    def get_absolute_url(self):
        return reverse("staff_person:detail", kwargs={"pk": self.pk})
