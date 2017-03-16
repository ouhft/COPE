#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

import csv
import pytz
import datetime

# from django.contrib import messages
from django.db.models import Q
from django.http import HttpResponse
from django.shortcuts import render
from django.utils import timezone

from wp4.compare.models import Randomisation, PAPER_UNITED_KINGDOM, PAPER_EUROPE
from wp4.compare.models import Donor, Organ, OrganAllocation
from wp4.compare.models import RetrievalTeam
from wp4.compare.models import PRESERVATION_HMP, PRESERVATION_HMPO2, PRESERVATION_NOT_SET
from wp4.locations.models import Hospital
from wp4.staff.models import Person
from wp4.adverse_event.models import Event

from wp4.utils import group_required


# Statisticians' Reports
@group_required(Person.STATISTICIAN, Person.CENTRAL_COORDINATOR)
def report_procurement(request):
    # Create the HttpResponse object with the appropriate CSV header.
    response = HttpResponse(content_type='text/csv')
    response['Content-Disposition'] = 'attachment; filename="wp4_report_procurement.csv"'

    writer = csv.writer(response)
    writer.writerow([
        "donor.trial_id",
        "donor.person.date_of_birth_unknown",
        "donor.person.date_of_birth",
        "donor.person.gender",
        "donor.person.weight",
        "donor.person.height",
        "donor.person.ethnicity",
        "donor.person.get_ethnicity_display",
        "donor.person.blood_group",
        "donor.person.get_blood_group_display",
        "donor.age",
        "donor.date_of_procurement",
        "donor.retrieval_team",
        "donor.retrieval_hospital",
        "donor.multiple_recipients",
        "donor.life_support_withdrawal",
        "donor.death_diagnosed",
        "donor.perfusion_started_unknown",
        "donor.perfusion_started",
        "donor.systemic_flush_used",
        "donor.get_systemic_flush_used_display",
        "donor.diagnosis",
        "donor.get_diagnosis_display",
        "donor.diabetes_melitus",
        "donor.get_diabetes_melitus_display",
        "donor.alcohol_abuse",
        "donor.get_alcohol_abuse_display",
        "donor.diuresis_last_day",
        "donor.last_creatinine",
    ])

    donors = Donor.objects.filter(randomisation__isnull=False)
    for donor in donors:
        result_row = []

        result_row.append(donor.trial_id)
        result_row.append(donor.person.date_of_birth_unknown)
        try:
            result_row.append(donor.person.date_of_birth.strftime("%d-%m-%Y"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.person.gender)
        result_row.append(donor.person.weight)
        result_row.append(donor.person.height)
        result_row.append(donor.person.ethnicity)
        result_row.append(donor.person.get_ethnicity_display())
        result_row.append(donor.person.blood_group)
        result_row.append(donor.person.get_blood_group_display())
        result_row.append(donor.age)
        try:
            result_row.append(donor.date_of_procurement.strftime("%d-%m-%Y"))
        except AttributeError:
            result_row.append("")
        try:
            result_row.append(str(donor.retrieval_team))
        except AttributeError:
            result_row.append("")
        try:
            result_row.append(str(donor.retrieval_hospital))
        except AttributeError:
            result_row.append("")
        result_row.append("Y" if donor.multiple_recipients else "N")
        try:
            result_row.append(donor.life_support_withdrawal.strftime("%d-%m-%Y %H:%M"))
        except AttributeError:
            result_row.append("")
        try:
            result_row.append(donor.death_diagnosed.strftime("%d-%m-%Y %H:%M"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.perfusion_started_unknown)
        try:
            result_row.append(donor.perfusion_started.strftime("%d-%m-%Y %H:%M"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.systemic_flush_used)
        result_row.append(donor.get_systemic_flush_used_display())
        result_row.append(donor.diagnosis)
        result_row.append(donor.get_diagnosis_display())
        result_row.append(donor.diabetes_melitus)
        result_row.append(donor.get_diabetes_melitus_display())
        result_row.append(donor.alcohol_abuse)
        result_row.append(donor.get_alcohol_abuse_display())
        result_row.append(donor.diuresis_last_day)
        result_row.append(donor.last_creatinine)

        writer.writerow(result_row)

    return response


@group_required(Person.STATISTICIAN, Person.CENTRAL_COORDINATOR)
def report_organ(request):
    # Create the HttpResponse object with the appropriate CSV header.
    response = HttpResponse(content_type='text/csv')
    response['Content-Disposition'] = 'attachment; filename="wp4_report_organ.csv"'

    writer = csv.writer(response)
    writer.writerow([
        "organ.trial_id",
        "organ.transplantable",
        "organ.not_transplantable_reason",
        "organ.removal",
        "organ.renal_arteries",
        "organ.graft_damage",
        "organ.get_graft_damage_display",
        "organ.washout_perfusion",
        "organ.get_washout_perfusion_display",
        "organ.perfusion_possible",
        "organ.perfusion_not_possible_because",
        "organ.perfusion_machine",
        "organ.perfusion_started",
        "organ.preservation",
        "organ.get_preservation_display",
        "organ.recipient.person.date_of_birth",
        "organ.recipient.person.gender",
        "organ.recipient.person.weight",
        "organ.recipient.person.height",
        "organ.recipient.person.ethnicity",
        "organ.recipient.person.get_ethnicity_display",
        "organ.recipient.person.blood_group",
        "organ.recipient.person.get_blood_group_display",
        "organ.recipient.renal_disease",
        "organ.recipient.get_renal_disease_display",
        "organ.recipient.pre_transplant_diuresis",
        "organ.recipient.perfusion_stopped",
        "organ.recipient.organ_cold_stored",
        "organ.recipient.removed_from_machine_at",
        "organ.recipient.organ_untransplantable",
        "organ.recipient.organ_untransplantable_reason",
        "organ.recipient.knife_to_skin",
        "organ.recipient.incision",
        "organ.recipient.get_incision_display",
        "organ.recipient.transplant_side",
        "organ.recipient.get_transplant_side_display",
        "organ.recipient.arterial_problems",
        "organ.recipient.get_arterial_problems_display",
        "organ.recipient.venous_problems",
        "organ.recipient.get_venous_problems_display",
        "organ.recipient.anastomosis_started_at",
        "organ.recipient.reperfusion_started_at",
        "organ.recipient.successful_conclusion",
        "organ.recipient.operation_concluded_at",
    ])

    organs = Organ.objects.filter(recipient__isnull=False)
    for organ in organs:
        result_row = []

        result_row.append(organ.trial_id)
        result_row.append(organ.transplantable)
        result_row.append(organ.not_transplantable_reason)
        try:
            result_row.append(organ.removal.strftime("%d-%m-%Y"))
        except AttributeError:
            result_row.append("")
        result_row.append(organ.renal_arteries)
        result_row.append(organ.graft_damage)
        result_row.append(organ.get_graft_damage_display())
        result_row.append(organ.washout_perfusion)
        result_row.append(organ.get_washout_perfusion_display())
        result_row.append(organ.perfusion_possible)
        result_row.append(organ.perfusion_not_possible_because)
        try:
            result_row.append(str(organ.perfusion_machine))
        except AttributeError:
            result_row.append("")
        try:
            result_row.append(organ.perfusion_started.strftime("%d-%m-%Y %H:%M"))
        except AttributeError:
            result_row.append("")
        result_row.append(organ.preservation)
        result_row.append(organ.get_preservation_display())

        try:
            result_row.append(organ.recipient.person.date_of_birth.strftime("%d-%m-%Y"))
        except AttributeError:
            result_row.append("")
        result_row.append(organ.recipient.person.gender)
        result_row.append(organ.recipient.person.weight)
        result_row.append(organ.recipient.person.height)
        result_row.append(organ.recipient.person.ethnicity)
        result_row.append(organ.recipient.person.get_ethnicity_display())
        result_row.append(organ.recipient.person.blood_group)
        result_row.append(organ.recipient.person.get_blood_group_display())
        result_row.append(organ.recipient.renal_disease)
        result_row.append(organ.recipient.get_renal_disease_display())
        result_row.append(organ.recipient.pre_transplant_diuresis)
        try:
            result_row.append(organ.recipient.perfusion_stopped.strftime("%d-%m-%Y %H:%M"))
        except AttributeError:
            result_row.append("")
        result_row.append(organ.recipient.organ_cold_stored)
        try:
            result_row.append(organ.recipient.removed_from_machine_at.strftime("%d-%m-%Y %H:%M"))
        except AttributeError:
            result_row.append("")
        result_row.append(organ.recipient.organ_untransplantable)
        result_row.append(organ.recipient.organ_untransplantable_reason)
        try:
            result_row.append(organ.recipient.knife_to_skin.strftime("%d-%m-%Y %H:%M"))
        except AttributeError:
            result_row.append("")
        result_row.append(organ.recipient.incision)
        result_row.append(organ.recipient.get_incision_display())
        result_row.append(organ.recipient.transplant_side)
        result_row.append(organ.recipient.get_transplant_side_display())
        result_row.append(organ.recipient.arterial_problems)
        result_row.append(organ.recipient.get_arterial_problems_display())
        result_row.append(organ.recipient.venous_problems)
        result_row.append(organ.recipient.get_venous_problems_display())
        try:
            result_row.append(organ.recipient.anastomosis_started_at.strftime("%d-%m-%Y %H:%M"))
        except AttributeError:
            result_row.append("")
        try:
            result_row.append(organ.recipient.reperfusion_started_at.strftime("%d-%m-%Y %H:%M"))
        except AttributeError:
            result_row.append("")
        result_row.append(organ.recipient.successful_conclusion)
        try:
            result_row.append(organ.recipient.operation_concluded_at.strftime("%d-%m-%Y %H:%M"))
        except AttributeError:
            result_row.append("")

        writer.writerow(result_row)

    return response


@group_required(Person.STATISTICIAN, Person.CENTRAL_COORDINATOR)
def report_allocations(request):
    # Create the HttpResponse object with the appropriate CSV header.
    response = HttpResponse(content_type='text/csv')
    response['Content-Disposition'] = 'attachment; filename="wp4_report_allocation.csv"'

    writer = csv.writer(response)
    writer.writerow([
        "allocation.organ.trial_id",
        "allocation.reallocated",
        "allocation.transplant_hospital",
        "allocation.reallocation_reason",
        "allocation.get_reallocation_reason_display"
    ])

    allocations = OrganAllocation.objects.filter(organ__recipient__isnull=False)
    for allocation in allocations:
        result_row = []
        result_row.append(allocation.organ.trial_id)
        result_row.append(allocation.reallocated)
        try:
            result_row.append(str(allocation.transplant_hospital))
        except AttributeError:
            result_row.append("")
        result_row.append(allocation.reallocation_reason)
        result_row.append(allocation.get_reallocation_reason_display())

        writer.writerow(result_row)

    return response


@group_required(Person.STATISTICIAN, Person.CENTRAL_COORDINATOR)
def report_adverse_events(request):
    # Create the HttpResponse object with the appropriate CSV header.
    response = HttpResponse(content_type='text/csv')
    response['Content-Disposition'] = 'attachment; filename="wp4_report_adverseevents.csv"'

    writer = csv.writer(response)
    writer.writerow([
        "adverseevent.id",
        "adverseevent.organ.trial_id",
        "adverseevent.onset_at_date",
        "adverseevent.event_ongoing",
        "adverseevent.description",
        "adverseevent.action",
        "adverseevent.outcome",
        "adverseevent.alive_query_1",
        "adverseevent.alive_query_2",
        "adverseevent.alive_query_3",
        "adverseevent.alive_query_4",
        "adverseevent.alive_query_5",
        "adverseevent.alive_query_6",
        "adverseevent.alive_query_7",
        "adverseevent.alive_query_8",
        "adverseevent.alive_query_9",
        "adverseevent.rehospitalisation",
        "adverseevent.date_of_admission",
        "adverseevent.date_of_discharge",
        "adverseevent.admitted_to_itu",
        "adverseevent.dialysis_needed",
        "adverseevent.surgery_required",
        "adverseevent.death",
        "adverseevent.date_of_death",
        "adverseevent.treatment_related",
        "adverseevent.get_treatment_related_display",
        "adverseevent.cause_of_death_1",
        "adverseevent.cause_of_death_2",
        "adverseevent.cause_of_death_3",
        "adverseevent.cause_of_death_4",
        "adverseevent.cause_of_death_5",
        "adverseevent.cause_of_death_6",
        "adverseevent.cause_of_death_comment",
    ])

    events = Event.objects.all()
    for event in events:
        result_row = []

        result_row.append(event.id)
        result_row.append(event.organ.trial_id)
        try:
            result_row.append(event.onset_at_date.strftime("%d-%m-%Y"))
        except AttributeError:
            result_row.append("")
        result_row.append(event.event_ongoing)
        result_row.append(event.description)
        result_row.append(event.action)
        result_row.append(event.outcome)
        result_row.append(event.alive_query_1)
        result_row.append(event.alive_query_2)
        result_row.append(event.alive_query_3)
        result_row.append(event.alive_query_4)
        result_row.append(event.alive_query_5)
        result_row.append(event.alive_query_6)
        result_row.append(event.alive_query_7)
        result_row.append(event.alive_query_8)
        result_row.append(event.alive_query_9)
        result_row.append(event.rehospitalisation)
        try:
            result_row.append(event.date_of_admission.strftime("%d-%m-%Y"))
        except AttributeError:
            result_row.append("")
        try:
            result_row.append(event.date_of_discharge.strftime("%d-%m-%Y"))
        except AttributeError:
            result_row.append("")
        result_row.append(event.admitted_to_itu)
        result_row.append(event.dialysis_needed)
        result_row.append(event.surgery_required)
        result_row.append(event.death)
        try:
            result_row.append(event.date_of_death.strftime("%d-%m-%Y"))
        except AttributeError:
            result_row.append("")
        result_row.append(event.treatment_related)
        result_row.append(event.get_treatment_related_display())
        result_row.append(event.cause_of_death_1)
        result_row.append(event.cause_of_death_2)
        result_row.append(event.cause_of_death_3)
        result_row.append(event.cause_of_death_4)
        result_row.append(event.cause_of_death_5)
        result_row.append(event.cause_of_death_6)
        result_row.append(event.cause_of_death_comment)

        writer.writerow(result_row)

    return response


# Administrator Reports
@group_required(Person.CENTRAL_COORDINATOR)
def administrator_uk_list(request):
    randomisation_listing = Randomisation.objects.filter(list_code=PAPER_UNITED_KINGDOM)
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
def administrator_europe_list(request):
    randomisation_listing = Randomisation.objects.filter(list_code=PAPER_EUROPE)
    return render(
        request,
        'administration/offline_list.html',
        {
            'listing': randomisation_listing,
            'location': "Europe",
            'timestamp': timezone.now()
        }
    )


@group_required(Person.CENTRAL_COORDINATOR)
def administrator_procurement_pairs(request):

    listing = Donor.objects.\
        filter(randomisation__isnull=False).\
        order_by('retrieval_team__centre_code', 'randomisation__allocated_on')
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


@group_required(Person.CENTRAL_COORDINATOR)
def administrator_transplantation_sites(request):

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


@group_required(Person.CENTRAL_COORDINATOR)
def administrator_sae_sites(request):

    listing = Event.objects.all().\
        order_by('organ__recipient__allocation__transplant_hospital', 'created_on')

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


@group_required(Person.CENTRAL_COORDINATOR)
def flowchart(request):
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
        summary["donors"]["eligibility"][donor.is_eligible if not donor.is_eligible == -1 else "not_randomised"] += 1
        if donor.procurement_form_completed is True:
            summary["donors"]["p_forms_completed"] += 1

        if donor.created_on > summary["dates"]["latest_p_form"]:
            summary["dates"]["latest_p_form"] = donor.created_on

        if donor.is_randomised:
            summary["kidneys"]["total"] += 2
        if donor.is_eligible > 0:
            if donor.left_kidney.transplantable is True:
                summary["kidneys"]["transplantable"]["left"] += 1
                summary["recipients"]["t_forms_theoretical"] += 1
                if donor.left_kidney.final_allocation is not None:
                    summary["recipients"]["t_forms_started"] += 1
                    if donor.left_kidney.final_allocation.created_on > summary["dates"]["latest_t_form"]:
                        summary["dates"]["latest_t_form"] = donor.left_kidney.final_allocation.created_on

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
                    if donor.right_kidney.final_allocation.created_on > summary["dates"]["latest_t_form"]:
                        summary["dates"]["latest_t_form"] = donor.right_kidney.final_allocation.created_on

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

@group_required(Person.CENTRAL_COORDINATOR)
def completed_pairs(request):
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
        "donors" : {
            "total": 0
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
            "total_to_non_project_sites": 0
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
            "on_time": {
                "without_cc": 0,
                "with_cc": 0
            },
            "outside_window": {
                "early": 0,
                "overdue": 0
            }
        }
    }

    for donor in listing:
        summary["donors"]["total"] += 1
        previous_organ_eligible = False

        for organ in (donor.left_kidney, donor.right_kidney):
            summary["organs"]["total"] += 1
            summary["organs"]["total_eligible"] += 1 if organ.preservation != 9 and donor.multiple_recipients and organ.transplantable else 0
            summary["organs"]["total_randomised"] += 1 if organ.preservation != 9 else 0
            summary["organs"]["total_singleorgan"] += 1 if donor.multiple_recipients else 0
            summary["organs"]["total_transplantable"] += 1 if organ.transplantable else 0

            if organ.final_allocation and organ.preservation != 9 and donor.multiple_recipients and organ.transplantable:
                summary["allocations"]["total"] += 1
                if organ.final_allocation.transplant_hospital:
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
                                    previous_organ_eligible = True if not previous_organ_eligible else False
                                    if organ.followup_done_within_followup_final_window():
                                        if organ.followup_1y.creatinine_clearance:
                                            summary["finals"]["on_time"]["with_cc"] += 1
                                        else:
                                            summary["finals"]["on_time"]["without_cc"] += 1
                                    # Too Many problems with RelatedObjectDoesNotExist: Organ has no followup_1y
                                    # elif organ.followup_1y and (organ.followup_1y.start_date > organ.followup_final_completed_by or timezone.now().date() > organ.followup_final_completed_by):
                                    #     summary["finals"]["outside_window"]["early"] += 1
                                    # else:
                                    #     summary["finals"]["outside_window"]["overdue"] += 1
                    else:
                        summary["allocations"]["total_to_non_project_sites"] += 1
                else:
                    summary["allocations"]["total_to_unknown_hospital"] += 1

    return render(
        request,
        'administration/completed_pairs.html',
        {
            'listing': listing,
            'summary': summary
        }
    )

@group_required(Person.CENTRAL_COORDINATOR)
def dmc_secondary_outcomes(request):
    """
    Produce a series of matrix tables that list Graft Failure against Death for each of the Follow Up periods

    Notes:
    - Wait until Date of Death is recorded against Recipient correctly, and not only on the S/AE records
    - Provide some context about how many followups have occurred?

    :param request:
    :return:
    """
    listing = Event.objects.filter(Q('') | Q()).select_related().order_by('organ__id', 'created_on')

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


@group_required(Person.CENTRAL_COORDINATOR)
def dmc_death_summaries(request):
    """
    Produce a list of S/AE records where death is recorded, separated into preservation groups

    Notes:
    - This will produce multiple "deaths" per trial where there are multiple S/AE events that lead to death

    :param request:
    :return:
    """
    listing = Event.objects.filter(date_of_death__isnull=False).select_related('organ__recipient').\
        order_by('organ__id', 'created_on')

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


# Admin Home
@group_required(Person.CENTRAL_COORDINATOR, Person.STATISTICIAN)
def administrator_index(request):
    return render(request, 'administration/index.html', {})
