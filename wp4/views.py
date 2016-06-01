#!/usr/bin/python
# coding: utf-8
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
from wp4.staff_person.models import StaffJob
from wp4.adverse_event.models import AdverseEvent

from .utils import group_required, job_required


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
    file_path = settings.ROOT_DIR + 'docs/changelog.md'
    with open(str(file_path), 'r') as markdown_file:
        document = markdown_file.read()
        return render(request, 'dashboard/changelog.html', {'document': document})


def dashboard_usermanual(request):
    file_path = settings.ROOT_DIR + 'docs/user_manual.md'
    with open(str(file_path), 'r') as markdown_file:
        document = markdown_file.read()
        return render(request, 'dashboard/user_manual.html', {'document': document})


@login_required
def wp4_index(request):
    return render(request, 'dashboard/wp4_index.html', {})


# @group_required('Central Co-ordinators')
# @login_required
@job_required(StaffJob.CENTRAL_COORDINATOR, StaffJob.STATISTICIAN)
def administrator_index(request):
    return render(request, 'dashboard/administrator_index.html', {})


@group_required('Central Co-ordinators')
@login_required
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


@group_required('Central Co-ordinators')
@login_required
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

@login_required
def administrator_datalist(request):
    organs = Organ.objects.all()
    return render(
        request,
        'dashboard/administrator_data_list.html',
        {
            'listing': organs,
            # 'location': "Europe",
            # 'timestamp': timezone.now()
        }
    )


@job_required(StaffJob.STATISTICIAN, StaffJob.CENTRAL_COORDINATOR)
def report_procurement(request):
    # Create the HttpResponse object with the appropriate CSV header.
    response = HttpResponse(content_type='text/csv')
    response['Content-Disposition'] = 'attachment; filename="wp4_report_procurement.csv"'

    writer = csv.writer(response)
    writer.writerow([
        u"donor.trial_id",
        u"donor.person.date_of_birth_unknown",
        u"donor.person.date_of_birth",
        u"donor.person.gender",
        u"donor.person.weight",
        u"donor.person.height",
        u"donor.person.ethnicity",
        u"donor.person.get_ethnicity_display",
        u"donor.person.blood_group",
        u"donor.person.get_blood_group_display",
        u"donor.age",
        u"donor.date_of_procurement",
        u"donor.retrieval_team",
        u"donor.retrieval_hospital",
        u"donor.multiple_recipients",
        u"donor.life_support_withdrawal",
        u"donor.death_diagnosed",
        u"donor.perfusion_started_unknown",
        u"donor.perfusion_started",
        u"donor.systemic_flush_used",
        u"donor.get_systemic_flush_used_display",
        u"donor.diagnosis",
        u"donor.get_diagnosis_display",
        u"donor.diabetes_melitus",
        u"donor.get_diabetes_melitus_display",
        u"donor.alcohol_abuse",
        u"donor.get_alcohol_abuse_display",
        u"donor.diuresis_last_day",
        u"donor.last_creatinine",
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
            result_row.append(donor.retrieval_team.__unicode__())
        except AttributeError:
            result_row.append("")
        try:
            result_row.append(donor.retrieval_hospital.__unicode__())
        except AttributeError:
            result_row.append("")
        result_row.append(u"Y" if donor.multiple_recipients else u"N")
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
        u"organ.trial_id",
        u"organ.transplantable",
        u"organ.not_transplantable_reason",
        u"organ.removal",
        u"organ.renal_arteries",
        u"organ.graft_damage",
        u"organ.get_graft_damage_display",
        u"organ.washout_perfusion",
        u"organ.get_washout_perfusion_display",
        u"organ.perfusion_possible",
        u"organ.perfusion_not_possible_because",
        u"organ.perfusion_machine",
        u"organ.perfusion_started",
        u"organ.preservation",
        u"organ.get_preservation_display",
        u"organ.recipient.person.date_of_birth",
        u"organ.recipient.person.gender",
        u"organ.recipient.person.weight",
        u"organ.recipient.person.height",
        u"organ.recipient.person.ethnicity",
        u"organ.recipient.person.get_ethnicity_display",
        u"organ.recipient.person.blood_group",
        u"organ.recipient.person.get_blood_group_display",
        u"organ.recipient.renal_disease",
        u"organ.recipient.get_renal_disease_display",
        u"organ.recipient.pre_transplant_diuresis",
        u"organ.recipient.perfusion_stopped",
        u"organ.recipient.organ_cold_stored",
        u"organ.recipient.removed_from_machine_at",
        u"organ.recipient.organ_untransplantable",
        u"organ.recipient.organ_untransplantable_reason",
        u"organ.recipient.knife_to_skin",
        u"organ.recipient.incision",
        u"organ.recipient.get_incision_display",
        u"organ.recipient.transplant_side",
        u"organ.recipient.get_transplant_side_display",
        u"organ.recipient.arterial_problems",
        u"organ.recipient.get_arterial_problems_display",
        u"organ.recipient.venous_problems",
        u"organ.recipient.get_venous_problems_display",
        u"organ.recipient.anastomosis_started_at",
        u"organ.recipient.reperfusion_started_at",
        u"organ.recipient.successful_conclusion",
        u"organ.recipient.operation_concluded_at",
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
            result_row.append(organ.perfusion_machine.__unicode__())
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
        u"allocation.organ.trial_id",
        u"allocation.reallocated",
        u"allocation.transplant_hospital",
        u"allocation.reallocation_reason",
        u"allocation.get_reallocation_reason_display"
    ])

    allocations = OrganAllocation.objects.filter(organ__recipient__isnull=False)
    for allocation in allocations:
        result_row = []
        result_row.append(allocation.organ.trial_id)
        result_row.append(allocation.reallocated)
        try:
            result_row.append(allocation.transplant_hospital.__unicode__())
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
        u"adverseevent.id",
        u"adverseevent.organ.trial_id",
        u"adverseevent.onset_at_date",
        u"adverseevent.event_ongoing",
        u"adverseevent.description",
        u"adverseevent.action",
        u"adverseevent.outcome",
        u"adverseevent.alive_query_1",
        u"adverseevent.alive_query_2",
        u"adverseevent.alive_query_3",
        u"adverseevent.alive_query_4",
        u"adverseevent.alive_query_5",
        u"adverseevent.alive_query_6",
        u"adverseevent.alive_query_7",
        u"adverseevent.alive_query_8",
        u"adverseevent.alive_query_9",
        u"adverseevent.rehospitalisation",
        u"adverseevent.date_of_admission",
        u"adverseevent.date_of_discharge",
        u"adverseevent.admitted_to_itu",
        u"adverseevent.dialysis_needed",
        u"adverseevent.surgery_required",
        u"adverseevent.death",
        u"adverseevent.date_of_death",
        u"adverseevent.treatment_related",
        u"adverseevent.get_treatment_related_display",
        u"adverseevent.cause_of_death_1",
        u"adverseevent.cause_of_death_2",
        u"adverseevent.cause_of_death_3",
        u"adverseevent.cause_of_death_4",
        u"adverseevent.cause_of_death_5",
        u"adverseevent.cause_of_death_6",
        u"adverseevent.cause_of_death_comment",
    ])

    events = AdverseEvent.objects.all()
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
