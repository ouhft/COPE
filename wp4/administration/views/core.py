#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.core.exceptions import PermissionDenied
from django.shortcuts import render


# Admin Home
def index(request):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    return render(request, 'administration/index.html', {})
