#!/usr/bin/python
# coding: utf-8
import pytz

from django.conf import settings
from django.utils import timezone


class TimezoneMiddleware(object):
    # http://stackoverflow.com/questions/18322262/how-to-setup-custom-middleware-in-django
    # https://docs.djangoproject.com/en/1.9/topics/i18n/timezones/
    def process_request(self, request):
        tzname = request.session.get('django_timezone')
        if tzname is None:
            pass  # TODO: FIX THIS!!!
        print("DEBUG: TimezoneMiddleware:tzname=%s" % tzname)
        print("DEBUG: settings.TIME_ZONE = %s" % settings.TIME_ZONE)
        if tzname:
            timezone.activate(pytz.timezone(tzname))
        else:
            timezone.deactivate()
