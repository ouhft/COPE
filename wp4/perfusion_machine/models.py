#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.db import models
from django.utils.translation import ugettext_lazy as _


class Machine(models.Model):
    """
    Helper class for device accountability. Tracks actual Perfusion Machines used in the trial.

    So trivial as to be readable by all, and limited by nothing significant.
    """
    machine_serial_number = models.CharField(verbose_name=_('PM01 machine serial number'), max_length=50)
    machine_reference_number = models.CharField(verbose_name=_('PM02 machine reference number'), max_length=50)

    def __str__(self):
        return 's/n: {0}'.format(self.machine_serial_number)

    class Meta:
        verbose_name = _('PMm1 perfusion machine')
        verbose_name_plural = _('PMm2 perfusion machines')
