__author__ = 'carl'

import time
import os
import markdown

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


# http://stackoverflow.com/a/14498938/3687274
@register.simple_tag
def get_verbose_field_name(instance, field_name):
    """
    Returns verbose_name for a field.
    """
    return instance._meta.get_field(field_name).verbose_name.title()


# http://www.jw.pe/blog/post/using-markdown-django-17/
# http://pythonhosted.org/Markdown/index.html
@register.filter
def markdownify(text):
    # safe_mode governs how the function handles raw HTML
    output = markdown.markdown(text, safe_mode='escape')
    # Remap the static path to the static url
    output = output.replace("src=\"static", 'src="/static')
    # Refresh the imgs to have a bootstrap responsive class
    output = output.replace("<img ", "<img class=\"img-responsive\" ")
    # print("DEBUG: output=%s" % output)
    return output
