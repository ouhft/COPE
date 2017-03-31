#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals
from itertools import chain

from django.core.exceptions import PermissionDenied
from django.db.models import Q
from django.shortcuts import render

from wp4.compare.models import PRESERVATION_HMP, PRESERVATION_HMPO2, Organ, Recipient
from wp4.adverse_event.models import Event, Category
from wp4.followups.models import FollowUpInitial, FollowUp3M, FollowUp6M, FollowUp1Y


def adverse_events(request):
    """
    Produce a count of non-serious adverse events per recipient, per preservation arm, grouped by count of events per 
    recipient
    
    Secondly, produce a summary of number of events, per category, per preservation arm
    
    :param request: 
    :return: 
    """
    current_person = request.user
    if not current_person.is_administrator or \
            (current_person.has_perm('compare.hide_randomisation') and not current_person.is_superuser):
        raise PermissionDenied

    listing_hmp = Organ.objects.\
        prefetch_related("event_set", "event_set__categories").\
        filter(preservation=False).\
        order_by('trial_id')
    listing_hmp02 = Organ.objects.\
        prefetch_related("event_set", "event_set__categories").\
        filter(preservation=True).\
        order_by('trial_id')

    summary = {
        'organ': {
            0: {'hmp_02': 0, 'hmp': 0},
            1: {'hmp_02': 0, 'hmp': 0},
            2: {'hmp_02': 0, 'hmp': 0},
            3: {'hmp_02': 0, 'hmp': 0},
            4: {'hmp_02': 0, 'hmp': 0},
            5: {'hmp_02': 0, 'hmp': 0},
            'more': {'hmp_02': 0, 'hmp': 0},
        },
        'category': {},
        'totals': {
            'category': {'hmp_02': 0, 'hmp': 0, 'overall': 0},
            'organ': {'hmp_02': 0, 'hmp': 0, 'overall': 0},
            'events': {'hmp_02': 0, 'hmp': 0, 'overall': 0},
        }
    }
    for category in Category.objects.all():
        summary['category'][category.id] = {
            'description': category.description,
            'hmp_02': 0,
            'hmp': 0
        }


    # This will be an inefficient way of doing a group by and count, but that's because of the categories and later
    # processing
    for organ in list(chain(listing_hmp, listing_hmp02)):
        organ_events = organ.non_serious_events_only()
        count = len(organ_events)
        if len(organ_events) > 0:
            summary['totals']['organ']['hmp_02' if organ.preservation else 'hmp'] += 1

            for event in organ_events:
                for category in event.categories.all():
                    summary['category'][category.id]['hmp_02' if organ.preservation else 'hmp'] += 1
                    summary['totals']['category']['hmp_02' if organ.preservation else 'hmp'] += 1
                    summary['totals']['category']['overall'] += 1

        summary['totals']['events']['hmp_02' if organ.preservation else 'hmp'] += count
        summary['organ'][count if count < 5 else 'more']['hmp_02' if organ.preservation else 'hmp'] += 1

    print("DEBUG: adverse_events(): Summary={0}".format(summary))
    return render(
        request,
        'administration/dmc_adverse_events.html',
        {
            'listing_hmp': listing_hmp,
            'listing_hmp02': listing_hmp02,
            'summary': summary
        }
    )


def serious_events(request):
    """
    Produce a count of serious adverse events per recipient, per preservation arm, grouped by count of events per 
    recipient
    
    :param request: 
    :return: 
    """
    current_person = request.user
    if not current_person.is_administrator or \
            (current_person.has_perm('compare.hide_randomisation') and not current_person.is_superuser):
        raise PermissionDenied

    listing_hmp = Organ.objects.\
        prefetch_related("event_set", "event_set__categories").\
        filter(preservation=False).\
        order_by('trial_id')
    listing_hmp02 = Organ.objects.\
        prefetch_related("event_set", "event_set__categories").\
        filter(preservation=True).\
        order_by('trial_id')

    summary = {
        'organ': {
            0: {'hmp_02': 0, 'hmp': 0},
            1: {'hmp_02': 0, 'hmp': 0},
            2: {'hmp_02': 0, 'hmp': 0},
            3: {'hmp_02': 0, 'hmp': 0},
            4: {'hmp_02': 0, 'hmp': 0},
            5: {'hmp_02': 0, 'hmp': 0},
            'more': {'hmp_02': 0, 'hmp': 0},
        },
        'category': {},
        'totals': {
            'category': {'hmp_02': 0, 'hmp': 0, 'overall': 0},
            'organ': {'hmp_02': 0, 'hmp': 0, 'overall': 0},
            'events': {'hmp_02': 0, 'hmp': 0, 'overall': 0},
        }
    }
    for category in Category.objects.all():
        summary['category'][category.id] = {
            'description': category.description,
            'hmp_02': 0,
            'hmp': 0
        }

    # This will be an inefficient way of doing a group by and count, but that's because of the categories and later
    # processing
    for organ in list(chain(listing_hmp, listing_hmp02)):
        organ_events = organ.serious_events_only()
        count = len(organ_events)
        if len(organ_events) > 0:
            summary['totals']['organ']['hmp_02' if organ.preservation else 'hmp'] += 1

            for event in organ_events:
                for category in event.categories.all():
                    summary['category'][category.id]['hmp_02' if organ.preservation else 'hmp'] += 1
                    summary['totals']['category']['hmp_02' if organ.preservation else 'hmp'] += 1
                    summary['totals']['category']['overall'] += 1

        summary['totals']['events']['hmp_02' if organ.preservation else 'hmp'] += count
        summary['organ'][count if count < 5 else 'more']['hmp_02' if organ.preservation else 'hmp'] += 1

    print("DEBUG: serious_events(): Summary={0}".format(summary))
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
    if not current_person.is_administrator or \
            (current_person.has_perm('compare.hide_randomisation') and not current_person.is_superuser):
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
            'listing_initial': listing_initial.filter(graft_failure=True),
            'listing_month3': listing_month3.filter(graft_failure=True),
            'listing_month6': listing_month6.filter(graft_failure=True),
            'listing_year1': listing_year1.filter(graft_failure=True),
            'summary': summary
        }
    )


def death_summaries(request):
    """
    Produce a list of S/AE records where death is recorded, separated into preservation groups. 
    Additionally, count the number of Recipients with a DoD (including unknowns) and display them per preservation

    Notes:
    - This will produce multiple "deaths" per trial where there are multiple S/AE events that lead to death

    :param request:
    :return:
    """
    current_person = request.user
    if not current_person.is_administrator or \
            (current_person.has_perm('compare.hide_randomisation') and not current_person.is_superuser):
        raise PermissionDenied

    listing_recipient = Recipient.objects.filter(
        Q(person__date_of_death__isnull=False) |
        Q(person__date_of_death_unknown=True)
    )

    listing_hmp_organs = Organ.objects.\
        filter(recipient__isnull=False, event__death=True, preservation=False).\
        distinct().\
        prefetch_related("event_set")

    listing_hmp02_organs = Organ.objects.\
        filter(recipient__isnull=False, event__death=True, preservation=True).\
        distinct().\
        prefetch_related("event_set")

    summary = {
        'hmp_02': 0,
        'hmp': 0,
    }

    for recipient in listing_recipient:
        if recipient.organ.preservation:
            summary['hmp_02'] += 1
        else:
            summary['hmp'] += 1

    return render(
        request,
        'administration/dmc_death_summaries.html',
        {
            'listing_hmp_organs': listing_hmp_organs,
            'listing_hmp02_organs': listing_hmp02_organs,
            'summary': summary
        }
    )


def permanent_impairment(request):
    """
    Produce a list of S/AE records where permanent impairment is recorded, separated into preservation groups. 
    Additionally, count the number of Recipients with one or more S/AEs and display them per preservation
    
    Note: There are 2 Questions on the S/AE form that we have to check that ask the same thing!??
    They are stored as: serious_eligible_3 & alive_query_7

    :param request:
    :return:
    """
    current_person = request.user
    if not current_person.is_administrator or \
            (current_person.has_perm('compare.hide_randomisation') and not current_person.is_superuser):
        raise PermissionDenied

    listing_hmp_organs = Organ.objects.\
        filter(recipient__isnull=False, preservation=False).\
        filter(Q(event__serious_eligible_3=True) | Q(event__alive_query_7=True)).\
        distinct().\
        prefetch_related("event_set")

    listing_hmp02_organs = Organ.objects.\
        filter(recipient__isnull=False, preservation=True).\
        filter(Q(event__serious_eligible_3=True) | Q(event__alive_query_7=True)).\
        distinct().\
        prefetch_related("event_set")

    summary = {
        'hmp_02': 0,
        'hmp': 0,
    }

    for organ in list(chain(listing_hmp_organs, listing_hmp02_organs)):
        if organ.preservation:
            summary['hmp_02'] += 1
        else:
            summary['hmp'] += 1

    return render(
        request,
        'administration/dmc_permanent_impairment.html',
        {
            'listing_hmp_organs': listing_hmp_organs,
            'listing_hmp02_organs': listing_hmp02_organs,
            'summary': summary
        }
    )

