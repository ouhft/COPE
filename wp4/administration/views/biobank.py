#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from io import BytesIO
import csv

from django.core.exceptions import PermissionDenied
from django.contrib import messages
from django.contrib.auth.decorators import login_required
from django.http import HttpResponseRedirect, HttpResponse
from django.shortcuts import render, reverse
from django.utils import timezone

from wp4.samples.models import WP7Record, BloodSample, UrineSample, TissueSample, PerfusateSample
from wp4.samples.forms import WP7FileForm
from wp4.samples.utils import WP7Workbook, load_wp7_xlsx
from wp4.staff.models import Person
from wp4.utils import group_required


@login_required
def blood_collection(request):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = BloodSample.objects.all().order_by('collected')

    summary = {
        "total": {"sst": 0, "edsa": 0},
        "collected": {
            "unknown": {"sst": 0, "edsa": 0},
            "no": {"sst": 0, "edsa": 0},
            "yes": {"sst": 0, "edsa": 0}
        },
        "matching": {
            "collected_matched": {"sst": 0, "edsa": 0},
            "collected_unmatched": {"sst": 0, "edsa": 0},
        }
    }

    for sample in listing:
        if sample.blood_type == BloodSample.SAMPLE_SST:
            summary["total"]["sst"] += 1
            if sample.collected is not None:
                if sample.collected is True:
                    summary["collected"]["yes"]["sst"] += 1
                    if sample.wp7_location.count() > 0:
                        summary["matching"]["collected_matched"]["sst"] += 1
                    else:
                        summary["matching"]["collected_unmatched"]["sst"] += 1
                else:
                    summary["collected"]["no"]["sst"] += 1
            else:
                summary["collected"]["unknown"]["sst"] += 1
        else:
            summary["total"]["edsa"] += 1
            if sample.collected is not None:
                if sample.collected is True:
                    summary["collected"]["yes"]["edsa"] += 1
                    if sample.wp7_location.count() > 0:
                        summary["matching"]["collected_matched"]["edsa"] += 1
                    else:
                        summary["matching"]["collected_unmatched"]["edsa"] += 1
                else:
                    summary["collected"]["no"]["edsa"] += 1
            else:
                summary["collected"]["unknown"]["edsa"] += 1

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
        "total": {"rna": 0, "formalin": 0},
        "collected": {
            "unknown": {"rna": 0, "formalin": 0},
            "no": {"rna": 0, "formalin": 0},
            "yes": {"rna": 0, "formalin": 0}
        },
        "matching": {
            "collected_matched": {"rna": 0, "formalin": 0},
            "collected_unmatched": {"rna": 0, "formalin": 0},
        }
    }

    for sample in listing:
        if sample.tissue_type == TissueSample.SAMPLE_R:
            summary["total"]["rna"] += 1
            if sample.collected is not None:
                if sample.collected is True:
                    summary["collected"]["yes"]["rna"] += 1
                    if sample.wp7_location.count() > 0:
                        summary["matching"]["collected_matched"]["rna"] += 1
                    else:
                        summary["matching"]["collected_unmatched"]["rna"] += 1
                else:
                    summary["collected"]["no"]["rna"] += 1
            else:
                summary["collected"]["unknown"]["rna"] += 1
        else:
            summary["total"]["formalin"] += 1
            if sample.collected is not None:
                if sample.collected is True:
                    summary["collected"]["yes"]["formalin"] += 1
                    if sample.wp7_location.count() > 0:
                        summary["matching"]["collected_matched"]["formalin"] += 1
                    else:
                        summary["matching"]["collected_unmatched"]["formalin"] += 1
                else:
                    summary["collected"]["no"]["formalin"] += 1
            else:
                summary["collected"]["unknown"]["formalin"] += 1

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

            total_rows, created_count, linked_count = load_wp7_xlsx(workbook)

            messages.success(
                request=request,
                message="<strong>WP7 Data loaded</strong>:" +
                        "{0} rows processed, resulting in {1} records added with {2} linked".\
                        format(total_rows, created_count, linked_count)
            )
        else:
            messages.error(request, "Error with file upload form: {0}".format(form.errors))
    else:
        messages.error(request, "No form was submitted correctly")
    # messages.info(request, "Hello from wp7_file_form")
    return HttpResponseRedirect(reverse('wp4:administration:index'))


@group_required(Person.BIOBANK_COORDINATOR, Person.CENTRAL_COORDINATOR)
def export_for_wp7(request):
    """
    Produce a minimalist dataset for populating information in the WP7 DB

    - Sample process – type (blood, urine, perfusate, biopsy) STATIC
    - Sample sub-type (sample code i.e. DB1, DU1, P1, etc.) STATIC
    - Trial ID - STATIC
    - Barcode – VARIABLE
    - Date and time of collection - STATIC

    This is essentially four downloads rolled into one - the four sample types need iterating over slightly
    differently to get the same information out of them.

    :param request:
    :return HttpResponse: csv text file
    """
    # Create the HttpResponse object with the appropriate CSV header.
    response = HttpResponse(content_type='text/csv')
    response['Content-Disposition'] = 'attachment; filename="wp4_data_extract_for_wp7.csv"'

    writer = csv.writer(response)
    writer.writerow([
        "sample.event.get_type_display",
        "sample.event.get_name_display",
        "sample.get_xxxx_type_display",
        "sample.person|organ.trial_id",
        "sample.barcode",
        "sample.event.taken_at",
    ])

    bloods = BloodSample.objects.filter(collected=True)
    for sample in bloods:
        result_row = []

        result_row.append(sample.event.get_type_display())
        result_row.append(sample.event.get_name_display())
        result_row.append(sample.get_blood_type_display())
        result_row.append(sample.person.trial_id)
        result_row.append(sample.barcode)
        try:
            result_row.append(sample.event.taken_at.strftime("%d-%m-%Y %H:%M"))
        except AttributeError:
            result_row.append("")

        writer.writerow(result_row)

    tissues = TissueSample.objects.filter(collected=True)
    for sample in tissues:
        result_row = []

        result_row.append(sample.event.get_type_display())
        result_row.append(sample.event.get_name_display())
        result_row.append(sample.get_tissue_type_display())
        result_row.append(sample.organ.trial_id)
        result_row.append(sample.barcode)
        try:
            result_row.append(sample.event.taken_at.strftime("%d-%m-%Y %H:%M"))
        except AttributeError:
            result_row.append("")

        writer.writerow(result_row)

    perfusates = PerfusateSample.objects.filter(collected=True)
    for sample in perfusates:
        result_row = []

        result_row.append(sample.event.get_type_display())
        result_row.append(sample.event.get_name_display())
        result_row.append("")
        result_row.append(sample.organ.trial_id)
        result_row.append(sample.barcode)
        try:
            result_row.append(sample.event.taken_at.strftime("%d-%m-%Y %H:%M"))
        except AttributeError:
            result_row.append("")

        writer.writerow(result_row)

    urines = UrineSample.objects.filter(collected=True)
    for sample in urines:
        result_row = []

        result_row.append(sample.event.get_type_display())
        result_row.append(sample.event.get_name_display())
        result_row.append("")
        result_row.append(sample.person.trial_id)
        result_row.append(sample.barcode)
        try:
            result_row.append(sample.event.taken_at.strftime("%d-%m-%Y %H:%M"))
        except AttributeError:
            result_row.append("")

        writer.writerow(result_row)

    return response
