__author__ = 'carl'

from django import template

register = template.Library()


@register.simple_tag
def active(request, pattern):
    path = request.path
    if path == pattern:
        return 'active'
    return ''
