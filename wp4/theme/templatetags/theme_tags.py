__author__ = 'carl'

import time
import os
import markdown
import re

from django import template
from django.conf import settings
from django.contrib.auth.models import Group
from django.utils.html import format_html
from django.db.models.options import FieldDoesNotExist

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


@register.simple_tag
def display_field(instance, field_name):
    """
    Takes a given model and field name and renders them for display in a definition list
    :param instance: Model instance containing the field and data
    :param field_name: Name of the model field that we want to display
    :return: HTML String for output in template
    """
    field = instance._meta.get_field(field_name)
    try:
        field_unknown = instance._meta.get_field("{0}_unknown".format(field_name))
    except FieldDoesNotExist:
        field_unknown = None
    html = format_html("<dt>{0}</dt>", field.verbose_name)
    value = None

    if field.choices is not None and len(field.choices) > 0:
        # Look for fields with preset choices first and render the selected choice
        print("DEBUG: display_field() choices={0}".format(field.choices))
        value = getattr(instance, 'get_{0}_display'.format(field_name))()
    elif field_unknown is not None:
        # If this field has a paired Unknown field, use that as well
        print("DEBUG: display_field() field_unknown={0}".format(field_unknown))
    else:
        # Otherwise assume this is a regular field with a value
        value = getattr(instance, field_name)

    print("DEBUG: display_field() value={0}".format(value))
    # Translate the values into readable strings
    if value is True:
        value = "Yes"
    elif value is False:
        value = "No"
    elif value is None or value == '':
        value = "Missing"

    #TODO: Add some date and datetime formatting cleanup - and also, dates are coming up as "Missing"

    html += format_html("<dd>{0}<dd>", value)
    return html


@register.simple_tag
def display_fields(instance, *fields):
    """
    Takes a given model and list of field names and renders them for display in a definition list
    :param instance: Model instance containing the field and data
    :param *fields: Name of the model field that we want to display
    :return: HTML String for output in template
    """
    html = format_html("<dl>")
    for field in fields:
        html += display_field(instance, field)
    return html + format_html("</dl>")


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


# http://stackoverflow.com/questions/6453652/how-to-add-the-current-query-string-to-an-url-in-a-django-template
@register.simple_tag
def query_transform(request, **kwargs):
    updated = request.GET.copy()
    for k, v in kwargs.items():
        if v is not None:
            updated[k] = v
        else:
            updated.pop(k, 0)  # Remove or return 0 - aka, delete safely this key

    return updated.urlencode()
