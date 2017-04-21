#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.core.exceptions import PermissionDenied
from django.contrib.auth.decorators import login_required
from django.shortcuts import render

from wp4.compare.models import Donor


# Completeness Reports
@login_required
def procurement(request):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = Donor.objects.all().\
        select_related('_left_kidney').\
        select_related('_right_kidney').\
        order_by('trial_id')

    summary = {}

    return render(
        request,
        'administration/completeness/procurement.html',
        {
            'listing': listing,
            'summary': summary
        }
    )
