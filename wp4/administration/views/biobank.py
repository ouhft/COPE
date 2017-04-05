#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.core.exceptions import PermissionDenied
from django.contrib.auth.decorators import login_required
from django.shortcuts import render
from django.utils import timezone

from wp4.samples.models import WP7Record, BloodSample, UrineSample, TissueSample, PerfusateSample


@login_required
def sample_collection(request):
    """
    Return data that outlines what sample data has been collected, matched against data from the WP7 database

    :param request: 
    :return: 
    """
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing_bloods = BloodSample.objects.all()
    listing_urine = UrineSample.objects.all()
    listing_tissue = TissueSample.objects.all()
    listing_perfusate = PerfusateSample.objects.all()
    unmatched = WP7Record.objects.filter(content_type__isnull=True)
    summary = {}

    return render(
        request,
        'administration/sample_collection.html',
        {
            'listing_bloods': listing_bloods,
            'listing_urine': listing_urine,
            'listing_tissue': listing_tissue,
            'listing_perfusate': listing_perfusate,
            'unmatched': unmatched,
            'summary': summary
        }
    )

