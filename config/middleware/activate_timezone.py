#!/usr/bin/python
# coding: utf-8
import re
import pytz

from django.conf import settings
from django.core.exceptions import MiddlewareNotUsed
from django.utils import timezone


# From https://bitbucket.org/carljm/django-localeurl/src/ff81b368577b748d7ce17aff8e76223b71b0527d/localeurl/utils.py?at=default&fileviewer=file-view-default
def _strip_path(path):
    """
    Separates the locale prefix from the rest of the path. If the path does not
    begin with a locale it is returned without change.
    """
    supported_locales = dict(
        (code.lower(), name) for code, name in settings.LANGUAGES)
    locales_re = '|'.join(
        sorted(supported_locales.keys(), key=lambda i: len(i), reverse=True))
    path_re = re.compile(r'^/(?P<locale>%s)(?P<path>.*)$' % locales_re, re.I)

    check = path_re.match(path)
    if check:
        path_info = check.group('path') or '/'
        if path_info.startswith('/'):
            return check.group('locale'), path_info
    return '', path


class TimezoneMiddleware(object):
    def __init__(self):
        if not settings.USE_I18N:
            raise MiddlewareNotUsed("TimezoneMiddleware not appropriate")

    # http://stackoverflow.com/questions/18322262/how-to-setup-custom-middleware-in-django
    # https://docs.djangoproject.com/en/1.9/topics/i18n/timezones/
    def process_request(self, request):
        tzname = request.session.get('django_timezone')
        if tzname is None:
            # Determine the tz from the url localisation
            locale, path = _strip_path(request.path_info)
            if locale != '':
                country = locale.split('-')[1]  # Take the country code only
                tzname = pytz.country_timezones[country][0]  # in cases of where there are more than one

        # print("DEBUG: TimezoneMiddleware:tzname=%s" % tzname)
        # print("DEBUG: settings.TIME_ZONE = %s" % settings.TIME_ZONE)
        if tzname:
            timezone.activate(pytz.timezone(tzname))
        else:
            timezone.deactivate()
