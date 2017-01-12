#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

import csv

# from django.contrib import messages
from django.http import Http404, HttpResponse
from django.shortcuts import render
from django.conf import settings
from django.contrib.auth.decorators import login_required
from django.core.exceptions import PermissionDenied
from django.utils import timezone

from wp4.compare.models import Randomisation, PAPER_UNITED_KINGDOM, PAPER_EUROPE
from wp4.compare.models import Donor, Organ, OrganAllocation
from wp4.compare.models import RetrievalTeam
from wp4.compare.models import PRESERVATION_HMP, PRESERVATION_HMPO2, PRESERVATION_NOT_SET
from wp4.locations.models import Hospital
from wp4.staff_person.models import StaffJob
from wp4.adverse_event.models import AdverseEvent

from .utils import job_required


# Some forced errors to allow for testing the Error Page Templates
def error404(request):
    raise Http404("This is a page holder")  # This message is only for debug view


def error403(request):
    raise PermissionDenied


def error500(request):
    1 / 0


# Legitimate pages
def dashboard_index(request):
    return render(request, 'dashboard/index.html', {})


@login_required
def dashboard_changelog(request):
    file_path = settings.ROOT_DIR + 'docs/source/changelog.md'
    with open(str(file_path), 'r') as markdown_file:
        document = markdown_file.read()
        return render(request, 'dashboard/changelog.html', {'document': document})


def dashboard_usermanual(request):
    file_path = settings.ROOT_DIR + 'docs/source/user_manual.md'
    with open(str(file_path), 'r') as markdown_file:
        document = markdown_file.read()
        return render(request, 'dashboard/user_manual.html', {'document': document})


@login_required
def wp4_index(request):
    return render(request, 'dashboard/wp4_index.html', {})


@job_required(StaffJob.CENTRAL_COORDINATOR, StaffJob.STATISTICIAN)
def administrator_index(request):
    return render(request, 'dashboard/administrator_index.html', {})


@job_required(StaffJob.CENTRAL_COORDINATOR)
def administrator_uk_list(request):
    randomisation_listing = Randomisation.objects.filter(list_code=PAPER_UNITED_KINGDOM)
    return render(
        request,
        'dashboard/administrator_offline_list.html',
        {
            'listing': randomisation_listing,
            'location': "United Kingdom",
            'timestamp': timezone.now()
        }
    )


@job_required(StaffJob.CENTRAL_COORDINATOR)
def administrator_europe_list(request):
    randomisation_listing = Randomisation.objects.filter(list_code=PAPER_EUROPE)
    return render(
        request,
        'dashboard/administrator_offline_list.html',
        {
            'listing': randomisation_listing,
            'location': "Europe",
            'timestamp': timezone.now()
        }
    )


@job_required(StaffJob.CENTRAL_COORDINATOR)
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
        'dashboard/administrator_procurement_pairs.html',
        {
            'listing': listing,
            'summary': summary
        }
    )


@job_required(StaffJob.CENTRAL_COORDINATOR)
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
        'dashboard/administrator_transplantation_sites.html',
        {
            'listing': listing,
            'summary': summary
        }
    )


@job_required(StaffJob.CENTRAL_COORDINATOR)
def administrator_sae_sites(request):

    listing = AdverseEvent.objects.all().\
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
        'dashboard/administrator_sae_sites.html',
        {
            'listing': listing,
            'summary': summary
        }
    )


@job_required(StaffJob.CENTRAL_COORDINATOR)
def administrator_existence_checks(request):

    listing = Organ.objects.all().order_by('id')

    summary = {
        "full_count": 0,
        "preservations": {
            # Preservation counts
            PRESERVATION_HMP: 0,
            PRESERVATION_HMPO2: 0,
            PRESERVATION_NOT_SET: 0
        }
    }

    for organ in listing:
        summary["full_count"] += 1
        summary["preservations"][organ.preservation] += 1  # total count

    return render(
        request,
        'dashboard/administrator_existence_checks.html',
        {
            'listing': listing,
            'summary': summary
        }
    )

# @login_required
# def administrator_datalist(request):
#     organs = Organ.objects.all()
#     return render(
#         request,
#         'dashboard/administrator_data_list.html',
#         {
#             'listing': organs,
#             # 'location': "Europe",
#             # 'timestamp': timezone.now()
#         }
#     )


@job_required(StaffJob.STATISTICIAN, StaffJob.CENTRAL_COORDINATOR)
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


@job_required(StaffJob.STATISTICIAN, StaffJob.CENTRAL_COORDINATOR)
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


@job_required(StaffJob.STATISTICIAN, StaffJob.CENTRAL_COORDINATOR)
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


@job_required(StaffJob.STATISTICIAN, StaffJob.CENTRAL_COORDINATOR)
def report_adverse_events(request):
    # Create the HttpResponse object with the appropriate CSV header.
    response = HttpResponse(content_type='text/csv')
    response['Content-Disposition'] = 'attachment; filename="wp4_report_adverseevents.csv"'

    writer = csv.writer(response)
    writer.writerow([
        "adverseevent.id",
        "adverseevent.organ.trial_id",
        "adverseevent.organ.preservation",
        "adverseevent.organ.get_preservation_display",
        "adverseevent.serious_eligible_1",
        "adverseevent.serious_eligible_2",
        "adverseevent.serious_eligible_3",
        "adverseevent.serious_eligible_4",
        "adverseevent.serious_eligible_5",
        "adverseevent.serious_eligible_6",
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
        "adverseevent.organ.final_allocation.transplant_hospital.name"
    ])

    events = AdverseEvent.objects.all()
    for event in events:
        result_row = []
        result_row.append(event.id)
        result_row.append(event.organ.trial_id)
        result_row.append(event.organ.preservation)
        result_row.append(event.organ.get_preservation_display())
        result_row.append(event.serious_eligible_1)
        result_row.append(event.serious_eligible_2)
        result_row.append(event.serious_eligible_3)
        result_row.append(event.serious_eligible_4)
        result_row.append(event.serious_eligible_5)
        result_row.append(event.serious_eligible_6)
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
        result_row.append(event.organ.final_allocation.transplant_hospital.name)

        writer.writerow(result_row)

    return response
