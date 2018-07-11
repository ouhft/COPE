#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.core.exceptions import PermissionDenied
from django.contrib.auth.decorators import login_required
from django.http import HttpResponseNotFound, Http404
from django.shortcuts import get_object_or_404, render

from wp4.compare.models import Donor, Organ, Recipient
from wp4.compare.utils import get_donor_id_from_trial_id
from wp4.locations.models import Hospital


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


@login_required
def transplant_per_centre(request):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = Hospital.objects.\
        prefetch_related('organallocation_set__recipient__person').\
        prefetch_related('organallocation_set__organ').\
        filter(organallocation__reallocated=False).\
        distinct()

    summary = {
        "hospital_count": listing.count(),
        "allocation_count": 0,
        "recipient_count": 0
    }

    data = []

    for hospital in listing:
        # data_block = {
        #     "name": hospital.name,
        #     "country": hospital.get_country_display(),
        #     "allocation_count": hospital.organallocation_set.count(),
        #     "allocations": []
        # }
        for allocation in hospital.organallocation_set.all():
            # allocation_block = {
            #     "trial_id": allocation.organ.trial_id,
            #     "organ_id": allocation.organ.id,
            #     "recipient_id": None,
            #     "recipient_number": "",
            #     "recipient_gender": None,
            #     "recipient_age": None,
            #     "recipient_consent": None,
            #     "recipient_single_kidney": None,
            #     "recipient_knife_to_skin": None
            # }
            summary["allocation_count"] += 1
            try:
                if allocation.recipient is not None:
                # allocation_block["recipient_id"] = allocation.recipient.id
                # allocation_block["recipient_number"] = allocation.recipient.person.number
                # allocation_block["recipient_gender"] = allocation.recipient.person.get_gender_display()
                # allocation_block["recipient_age"] = allocation.recipient.age_from_dob
                # allocation_block["recipient_consent"] = allocation.recipient.signed_consent
                # allocation_block["recipient_single_kidney"] = allocation.recipient.single_kidney_transplant
                # allocation_block["recipient_knife_to_skin"] = allocation.recipient.knife_to_skin
                    summary["recipient_count"] += 1
            except Recipient.DoesNotExist:
                pass
        #     data_block["allocations"].append(allocation_block)
        # data.append(data_block)

    return render(
        request,
        'administration/completeness/transplant_per_centre.html',
        {
            # 'data': data,
            'listing': listing,
            'summary': summary
        }
    )


@login_required
def donor_summary_by_donor_id(request, donor_pk):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    donor = get_object_or_404(Donor, pk=int(donor_pk))

    return render(
        request,
        'administration/completeness/donor-summary.html',
        {
            'donor': donor
        }
    )


@login_required
def donor_summary_by_trial_id(request, trial_id):
    try:
        donor_pk = get_donor_id_from_trial_id(trial_id)
        return donor_summary_by_donor_id(request, donor_pk)
    except TypeError:
        raise Http404
        # return HttpResponseNotFound("Trial ID Not Found")


@login_required
def donor_summary_listing(request):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    donors = Donor.all_objects.all().order_by('trial_id')

    return render(
        request,
        'administration/completeness/donor-summary-listing.html',
        {
            'donors': donors
        }
    )


@login_required
def consent_summary_listing(request):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    recipients = Recipient.objects.exclude(signed_consent=True).order_by('organ__trial_id')

    summary = {
        "non_consent": 0,
        "unknown": 0
    }

    for recipient in recipients:
        if recipient.signed_consent is None:
            summary["unknown"] += 1
        else:
            summary["non_consent"] += 1

    return render(
        request,
        'administration/completeness/consent-summary-listing.html',
        {
            'summary': summary,
            'listing': recipients
        }
    )
