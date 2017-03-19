__author__ = 'carl'

import time
import os
import markdown
import re

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
    return instance._meta.get_field(field_name).verbose_name


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
def has_group(user, group_identifier):
    if user.is_superuser:
        return True

    if type(group_identifier) not in (int, tuple):
        group_identifier = Group.objects.get(name=group_identifier).id

    print("DEBUG: has_group(tag filter) group_identifier={0}".format(group_identifier))
    return user.has_group(group_identifier)


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

@register.simple_tag
def unflatten_attr(text):
    """
    For crispy forms, take the flat_attr contents (text) and convert it into a dictionary

    e.g.: In Code:
        `ForeignKeyModal('quality_of_life', no_search=True, false_attr="Flibble \"text\" with space", final_at=456)`
    Becomes flat_attr with value of in the raw template output:
        ` no-search="True" false-attr="Flibble &quot;text&quot; with space" final-at="456"`

    Note: hyphens need to become underscores again for key names

    :param text: the flat_attr contents
    :return dict: attributes in a dict
    """
    dict_re = re.compile(r'([\w\-]+)="([\w\s&;]+)"')
    raw_list = re.findall(dict_re, text)
    output_dict = {}
    for k, v in raw_list:
        value = v.replace('&quot;', '"')
        if value == "True":
            value = True
        elif value == "False":
            value = False

        output_dict[k.replace('-','_')] = value
    return output_dict


@register.filter
def make_js_safe(text):
    """
    Take a string like `load_datetime_for_id_bloodsamples-0-centrifuged_at` and replace the characters that make JS
    upset
    """
    return text.replace("-","_")
