__author__ = 'carl'

import time
import os
import markdown

from django import template
from django.conf import settings
from django.contrib.auth.models import Group

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


# http://www.abidibo.net/blog/2014/05/22/check-if-user-belongs-group-django-templates/
@register.filter(name='has_group')
def has_group(user, group_name):
    group = Group.objects.get(name=group_name)
    return True if group in user.groups.all() else False


# Based on wp4/utils.py:job_required()
@register.filter(name='has_job')
def has_job(user, job_id):
    if user.is_superuser or bool(user.profile.has_job(job_id)):
        return True
    return False


# http://stackoverflow.com/questions/2894365/use-variable-as-dictionary-key-in-django-template
@register.filter
def keyvalue(t_dict, key):
    """
    {{dictionary|keyvalue:key_variable}}
    """
    return t_dict[key]


@register.filter
def selected_choice_text(field):
    """
    Takes a given crispy form multichoice field, and returns the selected item as text
    """
    for key, value in field.field.choices:
        if key == field.value():
            return value
    return ""
