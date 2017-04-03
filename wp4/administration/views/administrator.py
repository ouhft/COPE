#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

import pytz

from django.core.exceptions import PermissionDenied
from django.contrib.auth.decorators import login_required
from django.shortcuts import render
from django.utils import timezone

from wp4.compare.models import Randomisation
from wp4.compare.models import Donor, Organ
from wp4.compare.models import RetrievalTeam
from wp4.compare.models import PRESERVATION_HMP, PRESERVATION_HMPO2, PRESERVATION_NOT_SET, LEFT, RIGHT
from wp4.locations.models import Hospital
from wp4.staff.models import Person
from wp4.adverse_event.models import Event

from wp4.utils import group_required
from wp4.followups.models import FollowUp1Y


# Administrator Reports
@group_required(Person.CENTRAL_COORDINATOR)
def offline_uk_list(request):
    randomisation_listing = Randomisation.objects.filter(list_code=Randomisation.PAPER_UNITED_KINGDOM)
    return render(
        request,
        'administration/offline_list.html',
        {
            'listing': randomisation_listing,
            'location': "United Kingdom",
            'timestamp': timezone.now()
        }
    )


@group_required(Person.CENTRAL_COORDINATOR)
def offline_europe_list(request):
    randomisation_listing = Randomisation.objects.filter(list_code=Randomisation.PAPER_EUROPE)
    return render(
        request,
        'administration/offline_list.html',
        {
            'listing': randomisation_listing,
            'location': "Europe",
            'timestamp': timezone.now()
        }
    )


@login_required
def procurement_pairs(request):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = Donor.objects.filter(randomisation__isnull=False).order_by('trial_id')
    centres = dict()
    for centre in RetrievalTeam.objects.all():
        centres[centre.centre_code] = {
            "code": centre.centre_code,
            "name": centre.based_at.__str__(),
            "count": 0
        }
    summary = {
        "full_count": 0,
        "centres": centres
    }
    for donor in listing:
        summary["full_count"] += 1
        summary["centres"][donor.retrieval_team.centre_code]["count"] += 1
    return render(
        request,
        'administration/procurement_pairs.html',
        {
            'listing': listing,
            'summary': summary
        }
    )


@login_required
def transplantation_sites(request):
    """
    Contains randomisation data, so hide from "Investigators" and similar
    :param request: 
    :return: 
    """
    current_person = request.user
    if not current_person.is_administrator or \
            (current_person.has_perm('compare.hide_randomisation') and not current_person.is_superuser):
        raise PermissionDenied

    listing = Organ.objects.\
        filter(recipient__isnull=False).\
        order_by('recipient__allocation__transplant_hospital')
    centres = dict()
    for centre in Hospital.objects.filter(is_project_site=True).filter(is_active=True):
        centres[centre.id] = {
            "name": centre.name,
            "full_count": 0,
            "discarded": 0
        }
    summary = {
        "full_count": 0,
        "centres": centres,
        "preservations": {
            # Preservation counts: discarded, total
            PRESERVATION_HMP: [0, 0],
            PRESERVATION_HMPO2: [0, 0],
            PRESERVATION_NOT_SET: [0, 0]
        }
    }
    for organ in listing:
        summary["full_count"] += 1
        summary["preservations"][organ.preservation][1] += 1  # total count
        if organ.recipient.organ_untransplantable:
            summary["preservations"][organ.preservation][0] += 1  # discard count
        try:
            summary["centres"][organ.recipient.allocation.transplant_hospital.id]["full_count"] += 1
            if organ.recipient.organ_untransplantable:
                summary["centres"][organ.recipient.allocation.transplant_hospital.id]["discarded"] += 1
        except AttributeError:
            print("DEBUG: Investigate organ {0} for missing data".format(organ.id))
            pass
        except KeyError:
            print("DEBUG: Investigate organ {0} for allocation to a non project site".format(organ.id))
            pass
    return render(
        request,
        'administration/transplantation_sites.html',
        {
            'listing': listing,
            'summary': summary
        }
    )


@login_required
def sae_sites(request):
    """
    Contains randomisation data, so hide from "Investigators" and similar
    :param request: 
    :return: 
    """
    current_person = request.user
    if not current_person.is_administrator or \
            (current_person.has_perm('compare.hide_randomisation') and not current_person.is_superuser):
        raise PermissionDenied

    listing = Event.objects.all().\
        order_by('organ__recipient__allocation__transplant_hospital')

    centres = dict()
    for centre in Hospital.objects.filter(is_active=True):
        centres[centre.id] = {
            "name": centre.name,
            "event_count": 0,
        }
    summary = {
        "full_count": 0,
        "centres": centres,
        "preservations": {
            # Preservation counts
            PRESERVATION_HMP: 0,
            PRESERVATION_HMPO2: 0,
            PRESERVATION_NOT_SET: 0
        }
    }

    for event in listing:
        summary["full_count"] += 1
        summary["preservations"][event.organ.preservation] += 1  # total count
        try:
            summary["centres"][event.organ.recipient.allocation.transplant_hospital.id]["event_count"] += 1
        except AttributeError:
            print("DEBUG: Investigate organ {0} for missing data".format(event.organ.id))
            pass
        except KeyError:
            print("DEBUG: Investigate organ {0} for allocation to a non project site".format(event.organ.id))
            pass

    return render(
        request,
        'administration/sae_sites.html',
        {
            'listing': listing,
            'summary': summary
        }
    )


@login_required
def flowchart(request):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = Donor.objects.all().\
        select_related('_left_kidney').\
        select_related('_right_kidney').\
        order_by('id')

    summary = {
        "donors" : {
            "total": 0,
            "eligibility": {
                # Eligibilty counts
                0: 0,
                1: 0,
                2: 0,
                "not_randomised": 0
            },
            "p_forms_completed": 0
        },
        "kidneys": {
            "total": 0,
            "transplantable": {
                "total": 0,
                "left": 0,
                "right": 0
            },
            "allocated": {
                "total": 0,
                "left": 0,
                "right": 0
            },
        },
        "recipients": {
            "total": 0,
            "left": 0,
            "right": 0,
            "t_forms_theoretical": 0,
            "t_forms_started": 0,
            "t_forms_completed": 0
        },
        "dates": {
            "today": timezone.now(),
            "latest_p_form": timezone.datetime(day=1, month=1, year=2012, tzinfo=pytz.UTC),
            "latest_t_form": timezone.datetime(day=1, month=1, year=2012, tzinfo=pytz.UTC),
        }
    }

    for donor in listing:
        summary["donors"]["total"] += 1
        summary["donors"]["eligibility"][donor.count_of_eligible_organs if donor.count_of_eligible_organs >= 0 else "not_randomised"] += 1
        if donor.procurement_form_completed is True:
            summary["donors"]["p_forms_completed"] += 1

        # if donor.created_on > summary["dates"]["latest_p_form"]:
        #     summary["dates"]["latest_p_form"] = donor.created_on

        if donor.is_randomised:
            summary["kidneys"]["total"] += 2
        if donor.count_of_eligible_organs > 0:
            if donor.left_kidney.transplantable is True:
                summary["kidneys"]["transplantable"]["left"] += 1
                summary["recipients"]["t_forms_theoretical"] += 1
                if donor.left_kidney.final_allocation is not None:
                    summary["recipients"]["t_forms_started"] += 1
                    # if donor.left_kidney.final_allocation.created_on > summary["dates"]["latest_t_form"]:
                    #     summary["dates"]["latest_t_form"] = donor.left_kidney.final_allocation.created_on

                if donor.left_kidney.is_allocated:
                    summary["kidneys"]["allocated"]["left"] += 1
                    if donor.left_kidney.safe_recipient is not None:
                        summary["recipients"]["left"] += 1
                    if donor.left_kidney.transplantation_form_completed is True:
                        summary["recipients"]["t_forms_completed"] += 1

            if donor.right_kidney.transplantable is True:
                summary["kidneys"]["transplantable"]["right"] += 1
                summary["recipients"]["t_forms_theoretical"] += 1
                if donor.right_kidney.final_allocation is not None:
                    summary["recipients"]["t_forms_started"] += 1
                    # if donor.right_kidney.final_allocation.created_on > summary["dates"]["latest_t_form"]:
                    #     summary["dates"]["latest_t_form"] = donor.right_kidney.final_allocation.created_on

                if donor.right_kidney.is_allocated:
                    summary["kidneys"]["allocated"]["right"] += 1
                    if donor.right_kidney.safe_recipient is not None:
                        summary["recipients"]["right"] += 1
                    if donor.right_kidney.transplantation_form_completed is True:
                        summary["recipients"]["t_forms_completed"] += 1

            summary["kidneys"]["transplantable"]["total"] = \
                summary["kidneys"]["transplantable"]["left"] + summary["kidneys"]["transplantable"]["right"]
            summary["kidneys"]["allocated"]["total"] = \
                summary["kidneys"]["allocated"]["left"] + summary["kidneys"]["allocated"]["right"]
            summary["recipients"]["total"] = \
                summary["recipients"]["left"] + summary["recipients"]["right"]

    return render(
        request,
        'administration/flowchart.html',
        {
            'listing': listing,
            'summary': summary
        }
    )


@login_required
def completed_pairs(request):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = Donor.objects.all().\
        select_related('_left_kidney').\
        select_related('_right_kidney').\
        order_by('id')

    # Donors,
    # Organs,
    # Organ.knifetoskin (if missing, date of procedure, or anastmosis time, or reperfusion time) to get a time of transplant
    # If a pair: has recipient, is consented, allocated to project site, is a single organ transplant, has a T-Form
    # Determine Follow up windows
    # Looking for 12month follow up: check for completed as: creatinine clearance entered

    summary = {
        "donors": {
            "total": 0,
            "listing": {}
        },
        "organs": {
            "total": 0,
            "total_eligible": 0,
            "total_randomised": 0,
            "total_singleorgan": 0,
            "total_transplantable": 0,
        },
        "allocations": {
            "total": 0,
            "total_to_unknown_hospital": 0,
            "total_to_project_sites": 0,
            "total_to_non_project_sites": 0,
            "not_allocated": {
                'total': 0,
                'listing': []
            }
        },
        "recipients": {
            "total": 0,
            "operated_on": {
                "total": 0,
                "consented": 0,
                "single_organ": 0,
                "eligible": 0,
            }
        },
        "eligible_pairs": {
            "total": 0,
            "singles": 0,
        },
        "finals": {
            "early": {
                "without_cc": 0,
                "with_cc": 0
            },
            "on_time": {
                "without_cc": 0,
                "with_cc": 0
            },
            "late": {
                "without_cc": 0,
                "with_cc": 0
            },
            'total': 0
        }
    }

    today = timezone.now().date()
    for donor in listing:
        summary["donors"]["total"] += 1
        summary["donors"]["listing"][donor.id] = {
            'eligible_left': False,
            'eligible_right': False,
            'complete_left': False,
            'complete_right': False,
        }
        previous_organ_eligible = False

        for organ in (donor.left_kidney, donor.right_kidney):
            organ_eligible = organ.preservation != PRESERVATION_NOT_SET and donor.multiple_recipients and organ.transplantable
            summary["organs"]["total"] += 1
            summary["organs"]["total_eligible"] += 1 if organ_eligible else 0
            summary["organs"]["total_randomised"] += 1 if organ.preservation != PRESERVATION_NOT_SET else 0
            summary["organs"]["total_singleorgan"] += 1 if donor.multiple_recipients else 0
            summary["organs"]["total_transplantable"] += 1 if organ.transplantable else 0

            if organ.final_allocation and organ_eligible:
                summary["allocations"]["total"] += 1
                if organ.final_allocation.transplant_hospital and organ.final_allocation.reallocated is False:
                    if organ.final_allocation.transplant_hospital.is_project_site:
                        summary["allocations"]["total_to_project_sites"] += 1

                        if organ.safe_recipient:
                            recipient = organ.safe_recipient
                            summary["recipients"]["total"] += 1

                            if recipient.knife_to_skin or recipient.anastomosis_started_at or recipient.reperfusion_started_at or recipient.operation_concluded_at:
                                summary["recipients"]["operated_on"]["total"] += 1
                                summary["recipients"]["operated_on"]["consented"] += 1 if recipient.signed_consent else 0
                                summary["recipients"]["operated_on"]["single_organ"] += 1 if recipient.single_kidney_transplant else 0

                                if recipient.signed_consent and recipient.single_kidney_transplant:
                                    summary["recipients"]["operated_on"]["eligible"] += 1

                                    # Pair analysis
                                    summary["eligible_pairs"]["total"] += 1 if previous_organ_eligible else 0

                                    if organ.location == LEFT:
                                        summary["donors"]["listing"][donor.id]['eligible_left'] = True
                                    if organ.location == RIGHT:
                                        summary["donors"]["listing"][donor.id]['eligible_right'] = True

                                    summary["eligible_pairs"]["singles"] += 1 if previous_organ_eligible else -1  # Will result in a negative number of singles
                                    previous_organ_eligible = True if not previous_organ_eligible else False
                                    try:
                                        if organ.followup_1y:
                                            summary['finals']['total'] += 1
                                            if organ.followup_1y.start_date is not None:
                                                if organ.followup_1y.start_date < organ.followup_final_begin_by:
                                                    if organ.followup_1y.creatinine_clearance:
                                                        summary["finals"]["early"]["with_cc"] += 1
                                                    else:
                                                        summary["finals"]["early"]["without_cc"] += 1

                                                elif organ.followup_1y.start_date > organ.followup_final_completed_by:
                                                    if organ.followup_1y.creatinine_clearance:
                                                        summary["finals"]["late"]["with_cc"] += 1
                                                    else:
                                                        summary["finals"]["late"]["without_cc"] += 1

                                                else:
                                                    if organ.followup_1y.creatinine_clearance:
                                                        summary["finals"]["on_time"]["with_cc"] += 1

                                                        if organ.location == LEFT:
                                                            summary["donors"]["listing"][donor.id]['complete_left'] = True
                                                        if organ.location == RIGHT:
                                                            summary["donors"]["listing"][donor.id]['complete_right'] = True
                                                    else:
                                                        summary["finals"]["on_time"]["without_cc"] += 1

                                            else:
                                                if today < organ.followup_final_begin_by:
                                                    if organ.followup_1y.creatinine_clearance:
                                                        summary["finals"]["early"]["with_cc"] += 1
                                                        print("DEBUG: completed_pairs - no start date, but early with cc")
                                                    else:
                                                        summary["finals"]["early"]["without_cc"] += 1

                                                elif today > organ.followup_final_completed_by:
                                                    if organ.followup_1y.creatinine_clearance:
                                                        summary["finals"]["late"]["with_cc"] += 1
                                                        print("DEBUG: completed_pairs - no start date, but late with cc")
                                                    else:
                                                        summary["finals"]["late"]["without_cc"] += 1

                                                else:
                                                    if organ.followup_1y.creatinine_clearance:
                                                        summary["finals"]["on_time"]["with_cc"] += 1
                                                        print("DEBUG: completed_pairs - no start date, but on time with cc")
                                                    else:
                                                        summary["finals"]["on_time"]["without_cc"] += 1
                                    except FollowUp1Y.DoesNotExist:
                                        pass

                    else:
                        summary["allocations"]["total_to_non_project_sites"] += 1
                else:
                    summary["allocations"]["total_to_unknown_hospital"] += 1
            elif organ_eligible:
                summary["allocations"]["not_allocated"]["total"] += 1
                summary["allocations"]["not_allocated"]["listing"].append(organ)

    summary["eligible_pairs"]["singles"] *= -1  # Reverse the sign for this value

    return render(
        request,
        'administration/completed_pairs.html',
        {
            'listing': listing,
            'summary': summary
        }
    )


@login_required
def followups(request):
    current_person = request.user
    if not current_person.is_administrator:
        raise PermissionDenied

    listing = Organ.objects.all().order_by('trial_id')
    summary = {}

    return render(
        request,
        'administration/followups.html',
        {
            'listing': listing,
            'summary': summary
        }
    )
