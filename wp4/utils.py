#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib.auth.decorators import user_passes_test


# Based on https://djangosnippets.org/snippets/1703/
def group_required(*group_ids):
    """Requires user membership in at least one of the groups passed in."""
    def in_groups(u):
        if u.is_authenticated():
            if u.is_superuser or bool(u.groups.filter(id__in=group_ids)):
                return True
        return False
    return user_passes_test(in_groups)
