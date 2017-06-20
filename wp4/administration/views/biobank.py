#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.core.exceptions import PermissionDenied
from django.contrib.auth.decorators import login_required
from django.shortcuts import render
from django.utils import timezone

from wp4.samples.models import WP7Record, BloodSample, UrineSample, TissueSample, PerfusateSample


@login_required
def blood_collection(request):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = BloodSample.objects.all().order_by('collected')

    summary = {}

    return render(
        request,
        'administration/biobank/blood_collection.html',
        {
            'listing': listing,
            'summary': summary
        }
    )


@login_required
def urine_collection(request):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = UrineSample.objects.all().order_by('collected')

    summary = {}

    return render(
        request,
        'administration/biobank/urine_collection.html',
        {
            'listing': listing,
            'summary': summary
        }
    )


@login_required
def tissue_collection(request):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = TissueSample.objects.all().order_by('collected')

    summary = {
        "total": 0,
        "collected": {
            "unknown": 0,
            "no": 0,
            "yes": 0
        },
        "matching": {
            "collected_matched": 0,
            "collected_unmatched": 0,
        }
    }

    for sample in listing:
        summary["total"] += 1
        if sample.collected is not None:
            if sample.collected is True:
                summary["collected"]["yes"] += 1
                if sample.wp7_location.count() > 0:
                    summary["matching"]["collected_matched"] += 1
                else:
                    summary["matching"]["collected_unmatched"] += 1
            else:
                summary["collected"]["no"] += 1
        else:
            summary["collected"]["unknown"] += 1

    return render(
        request,
        'administration/biobank/tissue_collection.html',
        {
            'listing': listing,
            'summary': summary
        }
    )


@login_required
def perfusate_collection(request):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = PerfusateSample.objects.all().order_by('collected')

    summary = {}

    return render(
        request,
        'administration/biobank/perfusate_collection.html',
        {
            'listing': listing,
            'summary': summary
        }
    )


@login_required
def unmatched_samples(request):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = WP7Record.objects.filter(content_type__isnull=True)

    summary = {}

    return render(
        request,
        'administration/biobank/unmatched_samples.html',
        {
            'listing': listing,
            'summary': summary
        }
    )


@login_required
def paired_biopsies(request):
    """
    A list of RNA Later biopsies, organised by Donor, for Maria K's analysis
    
    :param request: 
    :return: 
    """
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = {}

    summary = {}

    return render(
        request,
        'administration/biobank/unmatched_samples.html',
        {
            'listing': listing,
            'summary': summary
        }
    )
