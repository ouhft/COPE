#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.db import models
from django.utils.translation import ugettext_lazy as _

from wp4.compare.models.core import BaseModelMixin


class PerfusionMachine(BaseModelMixin):
    """
    Helper class for device accountability. Tracks actual Perfusion Machines used in the trial.
    """
    machine_serial_number = models.CharField(verbose_name=_('PM01 machine serial number'), max_length=50)
    machine_reference_number = models.CharField(verbose_name=_('PM02 machine reference number'), max_length=50)

    def __str__(self):
        return 's/n: ' + self.machine_serial_number

    class Meta:
        verbose_name = _('PMm1 perfusion machine')
        verbose_name_plural = _('PMm2 perfusion machines')


class PerfusionFile(BaseModelMixin):
    """
    Early plans talked about collecting the data file from each machine after each transplant to provide
    extra data, however this has been shelved as a concept presently. This class was the foundation of
    supporting that activity
    """
    machine = models.ForeignKey(PerfusionMachine, verbose_name=_('PF01 perfusion machine'))
    file = models.FileField(blank=True, upload_to='perfusion_files')

    def __str__(self):
        return 'ID: %s' % self.id

    class Meta:
        verbose_name = _('PFm1 perfusion machine file')
        verbose_name_plural = _('PFm2 perfusion machine files')
