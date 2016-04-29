#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib.auth.models import User
from django.core.urlresolvers import reverse
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _
from bdateutil import relativedelta
from wp4.compare.models import VersionControlModel, Organ, YES_NO_UNKNOWN_CHOICES
from wp4.health_economics.models import QualityOfLife

