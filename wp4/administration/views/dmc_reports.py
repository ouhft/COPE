#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.core.exceptions import PermissionDenied
from django.db.models import Q
from django.shortcuts import render

from wp4.compare.models import PRESERVATION_HMP, PRESERVATION_HMPO2
from wp4.adverse_event.models import Event


def secondary_outcomes(request):
    """
    Produce a series of matrix tables that list Graft Failure against Death for each of the Follow Up periods

    Notes:
    - Wait until Date of Death is recorded against Recipient correctly, and not only on the S/AE records
    - Provide some context about how many followups have occurred?

    :param request:
    :return:
    """
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = Event.objects.filter(Q('') | Q()).select_related().order_by('organ__id')

    report_period = {
        "graft_failure": {
            "death": {
                PRESERVATION_HMP: 0,
                PRESERVATION_HMPO2: 0
            },
            "alive": {
                PRESERVATION_HMP: 0,
                PRESERVATION_HMPO2: 0
            }
        },
        "graft_ok": {
            "death": {
                PRESERVATION_HMP: 0,
                PRESERVATION_HMPO2: 0
            },
            "alive": {
                PRESERVATION_HMP: 0,
                PRESERVATION_HMPO2: 0
            }
        },
    }
    report_initial = {
        "full_count": 0,
        "breakdown": {}
    }
    report_3m = {
        "full_count": 0,
        "breakdown": {}
    }
    report_6m = {
        "full_count": 0,
        "breakdown": {}
    }
    report_1y = {
        "full_count": 0,
        "breakdown": {}
    }

    for event in listing:
        continue

    return render(
        request,
        'administration/dmc_secondary_outcomes.html',
        {
            'listing': listing,
        }
    )


def death_summaries(request):
    """
    Produce a list of S/AE records where death is recorded, separated into preservation groups

    Notes:
    - This will produce multiple "deaths" per trial where there are multiple S/AE events that lead to death

    :param request:
    :return:
    """
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = Event.objects.filter(date_of_death__isnull=False).select_related('organ__recipient').\
        order_by('organ__id')

    data = {
        PRESERVATION_HMP: [],
        PRESERVATION_HMPO2: []
    }

    def new_record(trial_id=None, centre_name=None, date_of_transplantation=None):
        return {
            "trial_id": trial_id,
            "centre_name": centre_name,
            "date_of_transplant": date_of_transplantation,
            "events": []
        }

    record = None
    previous_organ = None
    for event in listing:
        if previous_organ != event.organ:
            if record is not None:
                data[previous_organ.preservation].append(record)

            try:
                # event.organ.donor.retrieval_team.based_at.full_description,  <--- Doesn't make sense to be this
                transplant_centre = event.organ.final_allocation.transplant_hospital
            except AttributeError:
                transplant_centre = "Not Allocated"
            try:
                knife_to_skin_date = event.organ.safe_recipient.knife_to_skin
            except AttributeError:
                knife_to_skin_date = "No Recipient"
            record = new_record(
                event.organ.trial_id,
                transplant_centre,
                knife_to_skin_date
            )
        record["events"].append(event)
        previous_organ = event.organ
    data[previous_organ.preservation].append(record)  # Get the last event set

    return render(
        request,
        'administration/dmc_death_summaries.html',
        {
            'listing': data,
        }
    )
