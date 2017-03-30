#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals
from itertools import chain

from django.core.exceptions import PermissionDenied
from django.db.models import Q
from django.shortcuts import render

from wp4.compare.models import PRESERVATION_HMP, PRESERVATION_HMPO2, Organ
from wp4.adverse_event.models import Event
from wp4.followups.models import FollowUpInitial, FollowUp3M, FollowUp6M, FollowUp1Y


def adverse_events(request):
    """
    Produce a count of adverse events per recipient, per preservation arm, grouped by count of events per recipient
    :param request: 
    :return: 
    """
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = Organ.objects.all()

    summary = {
        0: {
            'hmp_02': 0,
            'hmp': 0
        },
        1: {
            'hmp_02': 0,
            'hmp': 0
        },
        2: {
            'hmp_02': 0,
            'hmp': 0
        },
        3: {
            'hmp_02': 0,
            'hmp': 0
        },
        4: {
            'hmp_02': 0,
            'hmp': 0
        },
        5: {
            'hmp_02': 0,
            'hmp': 0
        },
        'more': {
            'hmp_02': 0,
            'hmp': 0
        },
    }

    # This will be an inefficient way of doing a group by and count, but that's because of the categories and later
    # processing
    for organ in listing:
        count = 0
        for event in organ.event_set.all():
            if not event.is_serious:
                count += 1
        summary[count if count < 5 else 'more']['hmp_02' if organ.preservation else 'hmp'] += 1

    print("DEBUG: adverse_events(): Summary={0}".format(summary))
    return render(
        request,
        'administration/dmc_adverse_events.html',
        {
            'listing': listing,
            'summary': summary
        }
    )


def serious_events(request):
    """
    Produce a count of serious adverse events per recipient, per preservation arm, grouped by count of events per recipient
    :param request: 
    :return: 
    """
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing_hmp = Organ.objects.filter(preservation=False).order_by('trial_id')
    listing_hmp02 = Organ.objects.filter(preservation=True).order_by('trial_id')

    summary = {
        0: {
            'hmp_02': 0,
            'hmp': 0
        },
        1: {
            'hmp_02': 0,
            'hmp': 0
        },
        2: {
            'hmp_02': 0,
            'hmp': 0
        },
        3: {
            'hmp_02': 0,
            'hmp': 0
        },
        4: {
            'hmp_02': 0,
            'hmp': 0
        },
        5: {
            'hmp_02': 0,
            'hmp': 0
        },
        'more': {
            'hmp_02': 0,
            'hmp': 0
        },
    }

    # This will be an inefficient way of doing a group by and count, but that's because of the categories and later
    # processing
    sae_total = 0
    ae_total = 0
    organ_total = 0
    for organ in list(chain(listing_hmp, listing_hmp02)):
        organ_total += 1
        count = 0
        for event in organ.event_set.all():
            if event.is_serious:
                sae_total += 1
                count += 1
            else:
                ae_total += 1
        summary[count if count < 5 else 'more']['hmp_02' if organ.preservation else 'hmp'] += 1

    print("DEBUG: serious_events(): Summary={0}".format(summary))
    print("DEBUG: serious_events(): {0} Organs with {1} Serious & {2} Adverse".format(organ_total, sae_total, ae_total))
    return render(
        request,
        'administration/dmc_serious_events.html',
        {
            'listing_hmp': listing_hmp,
            'listing_hmp02': listing_hmp02,
            'summary': summary
        }
    )


def graft_failures(request):
    """
    Produces a table of graft failures per time point and preservation
    
    :param request: 
    :return: 
    """
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing_initial = FollowUpInitial.objects.filter(start_date__isnull=False)
    listing_month3 = FollowUp3M.objects.filter(start_date__isnull=False)
    listing_month6 = FollowUp6M.objects.filter(start_date__isnull=False)
    listing_year1 = FollowUp1Y.objects.filter(start_date__isnull=False)

    summary = {
        'initial': {
            'total': 0,
            'hmp_02': {
                'Y': 0,
                'N': 0,
                'U': 0
            },
            'hmp': {
                'Y': 0,
                'N': 0,
                'U': 0
            },

        },
        'month3': {
            'total': 0,
            'hmp_02': {
                'Y': 0,
                'N': 0,
                'U': 0
            },
            'hmp': {
                'Y': 0,
                'N': 0,
                'U': 0
            },

        },
        'month6': {
            'total': 0,
            'hmp_02': {
                'Y': 0,
                'N': 0,
                'U': 0
            },
            'hmp': {
                'Y': 0,
                'N': 0,
                'U': 0
            },

        },
        'year1': {
            'total': 0,
            'hmp_02': {
                'Y': 0,
                'N': 0,
                'U': 0
            },
            'hmp': {
                'Y': 0,
                'N': 0,
                'U': 0
            },

        },
    }

    for record in listing_initial:
        summary['initial']['total'] += 1
        if record.organ.preservation:
            if record.graft_failure is None:
                summary['initial']['hmp_02']['U'] += 1
            elif record.graft_failure:
                summary['initial']['hmp_02']['Y'] += 1
            else:
                summary['initial']['hmp_02']['N'] += 1
        else:
            if record.graft_failure is None:
                summary['initial']['hmp']['U'] += 1
            elif record.graft_failure:
                summary['initial']['hmp']['Y'] += 1
            else:
                summary['initial']['hmp']['N'] += 1

    for record in listing_month3:
        summary['month3']['total'] += 1
        if record.organ.preservation:
            if record.graft_failure is None:
                summary['month3']['hmp_02']['U'] += 1
            elif record.graft_failure:
                summary['month3']['hmp_02']['Y'] += 1
            else:
                summary['month3']['hmp_02']['N'] += 1
        else:
            if record.graft_failure is None:
                summary['month3']['hmp']['U'] += 1
            elif record.graft_failure:
                summary['month3']['hmp']['Y'] += 1
            else:
                summary['month3']['hmp']['N'] += 1

    for record in listing_month6:
        summary['month6']['total'] += 1
        if record.organ.preservation:
            if record.graft_failure is None:
                summary['month6']['hmp_02']['U'] += 1
            elif record.graft_failure:
                summary['month6']['hmp_02']['Y'] += 1
            else:
                summary['month6']['hmp_02']['N'] += 1
        else:
            if record.graft_failure is None:
                summary['month6']['hmp']['U'] += 1
            elif record.graft_failure:
                summary['month6']['hmp']['Y'] += 1
            else:
                summary['month6']['hmp']['N'] += 1

    for record in listing_year1:
        summary['year1']['total'] += 1
        if record.organ.preservation:
            if record.graft_failure is None:
                summary['year1']['hmp_02']['U'] += 1
            elif record.graft_failure:
                summary['year1']['hmp_02']['Y'] += 1
            else:
                summary['year1']['hmp_02']['N'] += 1
        else:
            if record.graft_failure is None:
                summary['year1']['hmp']['U'] += 1
            elif record.graft_failure:
                summary['year1']['hmp']['Y'] += 1
            else:
                summary['year1']['hmp']['N'] += 1

    return render(
        request,
        'administration/dmc_graft_failures.html',
        {
            'listing_initial': listing_initial,
            'listing_month3': listing_month3,
            'listing_month6': listing_month6,
            'listing_year1': listing_year1,
            'summary': summary
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

    listing = Event.objects.filter(organ__recipient__person__date_of_death__isnull=False).select_related('organ__recipient').\
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
