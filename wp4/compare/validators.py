#!/usr/bin/python
# coding: utf-8
import datetime

from django.core.validators import ValidationError
from django.utils import timezone


def validate_between_1900_2050(date):
    if date.year < 1900 or date.year > 2050:
        raise ValidationError(u'%s is not between 1900 and 2050' % date)


def validate_not_in_future(date):
    if isinstance(date, datetime.datetime):
        if date > timezone.now():
            raise ValidationError(u'%s is in the future' % date)
    else:
        if date > timezone.now().date():
            raise ValidationError(u'%s is in the future' % date)


def validate_not_in_past(date):
    if isinstance(date, datetime.datetime):
        if date < timezone.now():
            raise ValidationError(u'%s is in the past' % date)
    else:
        if date < timezone.now().date():
            raise ValidationError(u'%s is in the past' % date)