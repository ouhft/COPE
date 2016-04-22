#!/usr/bin/python
# coding: utf-8

from django.contrib.auth.models import User
from django.db import models
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _


class CreatedByModelMixin(models.Model):
    """
    Helper Meta model in the same vein as VersionControlModel, but simpler. Needs further work to refine
    the idea and to apply it more widely

    TODO: Move this to Core, and account for setting created_by in the save method
    """
    created_on = models.DateTimeField(default=timezone.now)
    created_by = models.ForeignKey(User)

    class Meta:
        abstract = True

    def save(self, force_insert=False, force_update=False, using=None,
             update_fields=None):
        self.created_on = timezone.now()
        return super(CreatedByModelMixin, self).save(force_insert, force_update, using, update_fields)


class PerfusionMachine(CreatedByModelMixin):
    """
    Helper class for device accountability. Tracks actual Perfusion Machines used in the trial.
    """
    machine_serial_number = models.CharField(verbose_name=_('PM01 machine serial number'), max_length=50)
    machine_reference_number = models.CharField(verbose_name=_('PM02 machine reference number'), max_length=50)

    def __unicode__(self):
        return 's/n: ' + self.machine_serial_number

    class Meta:
        verbose_name = _('PMm1 perfusion machine')
        verbose_name_plural = _('PMm2 perfusion machines')


class PerfusionFile(CreatedByModelMixin):
    """
    Early plans talked about collecting the data file from each machine after each transplant to provide
    extra data, however this has been shelved as a concept presently. This class was the foundation of
    supporting that activity
    """
    machine = models.ForeignKey(PerfusionMachine, verbose_name=_('PF01 perfusion machine'))
    file = models.FileField(blank=True, upload_to='perfusion_files')

    def __unicode__(self):
        return 'ID: %s' % self.id

    class Meta:
        verbose_name = _('PFm1 perfusion machine file')
        verbose_name_plural = _('PFm2 perfusion machine files')
