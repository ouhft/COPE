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

    listing = UrineSample.objects.all()

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

    listing = TissueSample.objects.all()

    summary = {}

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

    listing = PerfusateSample.objects.all()

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
