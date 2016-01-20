__author__ = 'carl'

import time
import os
from django import template
from django.conf import settings
import wp4

register = template.Library()


@register.simple_tag
def active(request, pattern):
    path = request.path
    if path == pattern:
        return 'active'
    return ''


@register.simple_tag
def version_number():
    return wp4.__version__


# From http://stackoverflow.com/a/23584696/3687274
@register.simple_tag
def version_date():
    path = settings.ROOT_DIR + '.git'
    return time.strftime('%b %Y', time.gmtime(os.path.getmtime(path.__str__())))


@register.simple_tag
def copyright_date_string():
    return "2015-" + time.strftime('%Y', time.gmtime())
