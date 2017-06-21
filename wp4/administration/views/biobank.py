#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from io import BytesIO

from django.core.exceptions import PermissionDenied
from django.contrib import messages
from django.contrib.auth.decorators import login_required
from django.http import HttpResponseRedirect
from django.shortcuts import render, reverse
from django.utils import timezone

from wp4.samples.models import WP7Record, BloodSample, UrineSample, TissueSample, PerfusateSample
from wp4.samples.forms import WP7FileForm
from wp4.samples.utils import WP7Workbook, load_wp7_xlsx


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


@login_required
def wp7_file_form(request):
    """
    Process a file being uploaded from the WP7 database
    :param request:
    :return: Page response
    """
    if request.method == 'POST':
        form = WP7FileForm(request.POST, request.FILES)
        if form.is_valid():

            file_in_memory = request.FILES['file'].read()
            # Load the xlsx file with the raw data
            workbook = WP7Workbook()
            if not workbook.load_xlsx(filename=BytesIO(file_in_memory)):
                raise Exception("xlsx file failed to load")

            total_rows, created_count, update_count = load_wp7_xlsx(workbook)

            messages.success(
                request=request,
                message="<strong>WP7 Data loaded</strong>:" +
                        "{0} rows processed, resulting in {1} created and {2} updated records".\
                        format(total_rows, created_count, update_count)
            )
        else:
            messages.error(request, "Error with file upload form: {0}".format(form.errors))
    else:
        messages.error(request, "No form was submitted correctly")
    # messages.info(request, "Hello from wp7_file_form")
    return HttpResponseRedirect(reverse('wp4:administration:index'))

