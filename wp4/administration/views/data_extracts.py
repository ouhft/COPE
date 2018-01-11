#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

import csv

from django.http import HttpResponse

from wp4.compare.models import Donor, Organ, OrganAllocation
from wp4.staff.models import Person
from wp4.adverse_event.models import Event, Category
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
        "donor.get_last_creatinine_unit_display"
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
        result_row.append(donor.get_last_creatinine_unit_display())

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

    events = Event.objects.all()
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
        try:
            result_row.append(event.organ.final_allocation.transplant_hospital.name)
        except AttributeError:
            result_row.append("")

        writer.writerow(result_row)

    return response


"""
A simplified output of the main data in a flat table for the statician to interpret. Issue #314. Organ centric listing
with non-normalised additions of donor, allocation, recipient, and followups.
"""
@group_required(Person.STATISTICIAN, Person.CENTRAL_COORDINATOR)
def report_data_flattened(request):
    # Create the HttpResponse object with the appropriate CSV header.
    response = HttpResponse(content_type='text/csv')
    response['Content-Disposition'] = 'attachment; filename="wp4_report_adverseevents.csv"'

    writer = csv.writer(response)
    writer.writerow([
        # ORGAN
        "organ.trial_id",
        "organ.get_location_display",
        "organ.not_transplantable_reason",
        "organ.admin_notes",
        "organ.transplantation_notes",
        "organ.transplantation_form_completed",
        "organ.paper_form_was_the_source",
        "organ.included_for_analysis",
        "organ.removal",
        "organ.renal_arteries",  # 10
        "organ.get_graft_damage_display",
        "organ.graft_damage_other",
        "organ.washout_perfusion",
        "organ.get_washout_perfusion_display",
        "organ.transplantable",
        "organ.not_transplantable_reason",
        "organ.get_preservation_display",
        "organ.perfusion_possible",
        "organ.perfusion_not_possible_because",
        "organ.perfusion_started",  # 20
        "organ.get_patch_holder_display",
        "organ.artificial_patch_used",
        "organ.get_artificial_patch_size_display",
        "organ.artificial_patch_number",
        "organ.oxygen_bottle_full",
        "organ.oxygen_bottle_open",
        "organ.oxygen_bottle_changed",
        "organ.oxygen_bottle_changed_at",
        "organ.oxygen_bottle_changed_at_unknown",
        "organ.ice_container_replenished",  # 30
        "organ.ice_container_replenished_at",
        "organ.ice_container_replenished_at_unknown",
        "organ.perfusate_measurable",
        "organ.perfusate_measure",
        "organ.perfusion_machine",

        # DONOR
        "organ.donor.person.number",
        "organ.donor.person.date_of_birth",
        "organ.donor.person.date_of_birth_unknown",
        "organ.donor.person.date_of_death",
        "organ.donor.person.date_of_death_unknown",
        "organ.donor.person.get_gender_display",
        "organ.donor.person.weight",
        "organ.donor.person.height",
        "organ.donor.person.get_ethnicity_display",
        "organ.donor.person.get_blood_group_display",  # 10
        "organ.donor.sequence_number",
        "organ.donor.multiple_recipients",
        "organ.donor.get_not_randomised_because_display",
        "organ.donor.not_randomised_because_other",
        "organ.donor.procurement_form_completed",
        "organ.donor.admin_notes",
        "organ.donor.retrieval_team",
        "organ.donor.perfusion_technician",
        "organ.donor.transplant_coordinator",
        "organ.donor.call_received",  # 20
        "organ.donor.call_received_unknown",
        "organ.donor.retrieval_hospital",
        "organ.donor.scheduled_start",
        "organ.donor.scheduled_start_unknown",
        "organ.donor.technician_arrival",
        "organ.donor.technician_arrival_unknown",
        "organ.donor.ice_boxes_filled",
        "organ.donor.ice_boxes_filled_unknown",
        "organ.donor.depart_perfusion_centre",
        "organ.donor.depart_perfusion_centre_unknown",  # 30
        "organ.donor.arrival_at_donor_hospital",
        "organ.donor.arrival_at_donor_hospital_unknown",
        "organ.donor.age",
        "organ.donor.date_of_admission",
        "organ.donor.date_of_admission_unknown",
        "organ.donor.admitted_to_itu",
        "organ.donor.date_admitted_to_itu",
        "organ.donor.date_admitted_to_itu_unknown",
        "organ.donor.date_of_procurement",
        "organ.donor.other_organs_procured",  # 40
        "organ.donor.other_organs_lungs",
        "organ.donor.other_organs_pancreas",
        "organ.donor.other_organs_liver",
        "organ.donor.other_organs_tissue",
        "organ.donor.get_diagnosis_display",
        "organ.donor.diagnosis_other",
        "organ.donor.get_diabetes_melitus_display",
        "organ.donor.get_alcohol_abuse_display",
        "organ.donor.cardiac_arrest",
        "organ.donor.systolic_blood_pressure",  # 50
        "organ.donor.diastolic_blood_pressure",
        "organ.donor.diuresis_last_day",
        "organ.donor.diuresis_last_day_unknown",
        "organ.donor.diuresis_last_hour",
        "organ.donor.diuresis_last_hour_unknown",
        "organ.donor.dopamine",
        "organ.donor.dobutamine",
        "organ.donor.nor_adrenaline",
        "organ.donor.vasopressine",
        "organ.donor.other_medication_details",  # 60
        "organ.donor.last_creatinine",
        "organ.donor.get_last_creatinine_unit_display",
        "organ.donor.max_creatinine",
        "organ.donor.max_creatinine_unit",
        "organ.donor.life_support_withdrawal",
        "organ.donor.systolic_pressure_low",
        "organ.donor.systolic_pressure_low_unknown",
        "organ.donor.o2_saturation",
        "organ.donor.o2_saturation_unknown",
        "organ.donor.circulatory_arrest",  # 70
        "organ.donor.circulatory_arrest_unknown",
        "organ.donor.length_of_no_touch",
        "organ.donor.death_diagnosed",
        "organ.donor.perfusion_started",
        "organ.donor.perfusion_started_unknown",
        "organ.donor.get_systemic_flush_used_display",
        "organ.donor.systemic_flush_used",
        "organ.donor.systemic_flush_used_other",
        "organ.donor.systemic_flush_volume_used",
        "organ.donor.heparin",  # 80

        # ALLOCATION
        "organ.allocation.perfusion_technician",
        "organ.allocation.call_received",
        "organ.allocation.call_received_unknown",
        "organ.allocation.transplant_hospital",
        "organ.allocation.theatre_contact",
        "organ.allocation.scheduled_start",
        "organ.allocation.scheduled_start_unknown",
        "organ.allocation.technician_arrival",
        "organ.allocation.technician_arrival_unknown",
        "organ.allocation.depart_perfusion_centre",  # 10
        "organ.allocation.depart_perfusion_centre_unknown",
        "organ.allocation.arrival_at_recipient_hospital",
        "organ.allocation.arrival_at_recipient_hospital_unknown",
        "organ.allocation.journey_remarks",
        "organ.allocation.reallocated",
        "organ.allocation.get_reallocation_reason_display",
        "organ.allocation.reallocation_reason_other",

        # RECIPIENT
        "organ.recipient.person.number",
        "organ.recipient.person.date_of_birth",
        "organ.recipient.person.date_of_birth_unknown",
        "organ.recipient.person.date_of_death",
        "organ.recipient.person.date_of_death_unknown",
        "organ.recipient.person.get_gender_display",
        "organ.recipient.person.weight",
        "organ.recipient.person.height",
        "organ.recipient.person.get_ethnicity_display",
        "organ.recipient.person.get_blood_group_display",  # 10
        "organ.recipient.signed_consent",
        "organ.recipient.single_kidney_transplant",
        "organ.recipient.get_renal_disease_display",
        "organ.recipient.renal_disease_other",
        "organ.recipient.pre_transplant_diuresis",
        "organ.recipient.knife_to_skin",
        "organ.recipient.perfusate_measure",
        "organ.recipient.perfusion_stopped",
        "organ.recipient.organ_cold_stored",
        "organ.recipient.tape_broken",  # 20
        "organ.recipient.removed_from_machine_at",
        "organ.recipient.oxygen_full_and_open",
        "organ.recipient.organ_untransplantable",
        "organ.recipient.organ_untransplantable_reason",
        "organ.recipient.anesthesia_started_at",
        "organ.recipient.get_incision_display",
        "organ.recipient.get_transplant_side_display",
        "organ.recipient.get_arterial_problems_display",
        "organ.recipient.get_venous_problems_display",
        "organ.recipient.anastomosis_started_at",  # 30
        "organ.recipient.anastomosis_started_at_unknown",
        "organ.recipient.reperfusion_started_at",
        "organ.recipient.reperfusion_started_at_unknown",
        "organ.recipient.mannitol_used",
        "organ.recipient.other_diurectics",
        "organ.recipient.other_diurectics_details",
        "organ.recipient.systolic_blood_pressure",
        "organ.recipient.cvp",
        "organ.recipient.intra_operative_diuresis",
        "organ.recipient.successful_conclusion",  # 40
        "organ.recipient.operation_concluded_at",
        "organ.recipient.probe_cleaned",
        "organ.recipient.ice_removed",
        "organ.recipient.oxygen_flow_stopped",
        "organ.recipient.oxygen_bottle_removed",
        "organ.recipient.box_cleaned",
        "organ.recipient.batteries_charged",
        "organ.recipient.cleaning_log",

        # FOLLOW UP 1
        "organ.followup_initial.start_date",
        "organ.followup_initial.notes",
        "organ.followup_initial.paper_form_was_the_source",
        "organ.followup_initial.graft_failure",
        "organ.followup_initial.graft_failure_date",
        "organ.followup_initial.get_graft_failure_type_display",
        "organ.followup_initial.graft_failure_type_other",
        "organ.followup_initial.graft_removal",
        "organ.followup_initial.graft_removal_date",
        "organ.followup_initial.get_dialysis_type_display",  # 10
        "organ.followup_initial.immunosuppression_1",
        "organ.followup_initial.immunosuppression_2",
        "organ.followup_initial.immunosuppression_3",
        "organ.followup_initial.immunosuppression_4",
        "organ.followup_initial.immunosuppression_5",
        "organ.followup_initial.immunosuppression_6",
        "organ.followup_initial.immunosuppression_7",
        "organ.followup_initial.immunosuppression_other",
        "organ.followup_initial.rejection",
        "organ.followup_initial.rejection_prednisolone",  # 20
        "organ.followup_initial.rejection_drug",
        "organ.followup_initial.rejection_drug_other",
        "organ.followup_initial.rejection_biopsy",
        "organ.followup_initial.calcineurin",
        "organ.followup_initial.get_serum_creatinine_unit_display",
        "organ.followup_initial.serum_creatinine_1",
        "organ.followup_initial.serum_creatinine_2",
        "organ.followup_initial.serum_creatinine_3",
        "organ.followup_initial.serum_creatinine_4",
        "organ.followup_initial.serum_creatinine_5",  # 30
        "organ.followup_initial.serum_creatinine_6",
        "organ.followup_initial.serum_creatinine_7",
        "organ.followup_initial.dialysis_requirement_1",
        "organ.followup_initial.dialysis_requirement_2",
        "organ.followup_initial.dialysis_requirement_3",
        "organ.followup_initial.dialysis_requirement_4",
        "organ.followup_initial.dialysis_requirement_5",
        "organ.followup_initial.dialysis_requirement_6",
        "organ.followup_initial.dialysis_requirement_7",
        "organ.followup_initial.get_dialysis_cause_display",  # 40
        "organ.followup_initial.dialysis_cause_other",
        "organ.followup_initial.hla_mismatch_a",
        "organ.followup_initial.hla_mismatch_b",
        "organ.followup_initial.hla_mismatch_dr",
        "organ.followup_initial.get_induction_therapy_display",
        "organ.followup_initial.discharge_date",

        # FOLLOW UP 2
        "organ.followup_3m.start_date",
        "organ.followup_3m.notes",
        "organ.followup_3m.paper_form_was_the_source",
        "organ.followup_3m.graft_failure",
        "organ.followup_3m.graft_failure_date",
        "organ.followup_3m.get_graft_failure_type_display",
        "organ.followup_3m.graft_failure_type_other",
        "organ.followup_3m.graft_removal",
        "organ.followup_3m.graft_removal_date",
        "organ.followup_3m.get_dialysis_type_display",  # 10
        "organ.followup_3m.immunosuppression_1",
        "organ.followup_3m.immunosuppression_2",
        "organ.followup_3m.immunosuppression_3",
        "organ.followup_3m.immunosuppression_4",
        "organ.followup_3m.immunosuppression_5",
        "organ.followup_3m.immunosuppression_6",
        "organ.followup_3m.immunosuppression_7",
        "organ.followup_3m.immunosuppression_other",
        "organ.followup_3m.rejection",
        "organ.followup_3m.rejection_prednisolone",  # 20
        "organ.followup_3m.rejection_drug",
        "organ.followup_3m.rejection_drug_other",
        "organ.followup_3m.rejection_biopsy",
        "organ.followup_3m.calcineurin",
        "organ.followup_3m.get_serum_creatinine_unit_display",
        "organ.followup_3m.serum_creatinine",
        "organ.followup_3m.creatinine_clearance",
        "organ.followup_3m.currently_on_dialysis",
        "organ.followup_3m.dialysis_date",
        "organ.followup_3m.number_of_dialysis_sessions",  # 30
        "organ.followup_3m.rejection_periods",
        "organ.followup_3m.graft_complications",
        "organ.followup_3m.quality_of_life.date_recorded",
        "organ.followup_3m.quality_of_life.qol_mobility",
        "organ.followup_3m.quality_of_life.qol_selfcare",
        "organ.followup_3m.quality_of_life.qol_usual_activities",
        "organ.followup_3m.quality_of_life.qol_pain",
        "organ.followup_3m.quality_of_life.qol_anxiety",
        "organ.followup_3m.quality_of_life.vas_score",

        # FOLLOW UP 3
        "organ.followup_6m.start_date",
        "organ.followup_6m.notes",
        "organ.followup_6m.paper_form_was_the_source",
        "organ.followup_6m.graft_failure",
        "organ.followup_6m.graft_failure_date",
        "organ.followup_6m.get_graft_failure_type_display",
        "organ.followup_6m.graft_failure_type_other",
        "organ.followup_6m.graft_removal",
        "organ.followup_6m.graft_removal_date",
        "organ.followup_6m.get_dialysis_type_display",  # 10
        "organ.followup_6m.immunosuppression_1",
        "organ.followup_6m.immunosuppression_2",
        "organ.followup_6m.immunosuppression_3",
        "organ.followup_6m.immunosuppression_4",
        "organ.followup_6m.immunosuppression_5",
        "organ.followup_6m.immunosuppression_6",
        "organ.followup_6m.immunosuppression_7",
        "organ.followup_6m.immunosuppression_other",
        "organ.followup_6m.rejection",
        "organ.followup_6m.rejection_prednisolone",  # 20
        "organ.followup_6m.rejection_drug",
        "organ.followup_6m.rejection_drug_other",
        "organ.followup_6m.rejection_biopsy",
        "organ.followup_6m.calcineurin",
        "organ.followup_6m.get_serum_creatinine_unit_display",
        "organ.followup_6m.serum_creatinine",
        "organ.followup_6m.creatinine_clearance",
        "organ.followup_6m.currently_on_dialysis",
        "organ.followup_6m.dialysis_date",
        "organ.followup_6m.number_of_dialysis_sessions",  # 30
        "organ.followup_6m.rejection_periods",
        "organ.followup_6m.graft_complications",

        # FOLLOW UP 4
        "organ.followup_1y.start_date",
        "organ.followup_1y.notes",
        "organ.followup_1y.paper_form_was_the_source",
        "organ.followup_1y.graft_failure",
        "organ.followup_1y.graft_failure_date",
        "organ.followup_1y.get_graft_failure_type_display",
        "organ.followup_1y.graft_failure_type_other",
        "organ.followup_1y.graft_removal",
        "organ.followup_1y.graft_removal_date",
        "organ.followup_1y.get_dialysis_type_display",  # 10
        "organ.followup_1y.immunosuppression_1",
        "organ.followup_1y.immunosuppression_2",
        "organ.followup_1y.immunosuppression_3",
        "organ.followup_1y.immunosuppression_4",
        "organ.followup_1y.immunosuppression_5",
        "organ.followup_1y.immunosuppression_6",
        "organ.followup_1y.immunosuppression_7",
        "organ.followup_1y.immunosuppression_other",
        "organ.followup_1y.rejection",
        "organ.followup_1y.rejection_prednisolone",  # 20
        "organ.followup_1y.rejection_drug",
        "organ.followup_1y.rejection_drug_other",
        "organ.followup_1y.rejection_biopsy",
        "organ.followup_1y.calcineurin",
        "organ.followup_1y.get_serum_creatinine_unit_display",
        "organ.followup_1y.serum_creatinine",
        "organ.followup_1y.creatinine_clearance",
        "organ.followup_1y.currently_on_dialysis",
        "organ.followup_1y.dialysis_date",
        "organ.followup_1y.number_of_dialysis_sessions",  # 30
        "organ.followup_1y.rejection_periods",
        "organ.followup_1y.graft_complications",
        "organ.followup_1y.quality_of_life.date_recorded",
        "organ.followup_1y.quality_of_life.qol_mobility",
        "organ.followup_1y.quality_of_life.qol_selfcare",
        "organ.followup_1y.quality_of_life.qol_usual_activities",
        "organ.followup_1y.quality_of_life.qol_pain",
        "organ.followup_1y.quality_of_life.qol_anxiety",
        "organ.followup_1y.quality_of_life.vas_score",

        # Extras: S/AE Numbers
        "S/AE Total Count",
        "S/AE IDs with serious_eligible_1 set",
        "S/AE IDs with death set",
        "S/AE IDs with Permanent Graph Failure category"

    ])

    pgf_category_object = Category.objects.get(id=8)  # Hardcoded reference to PGF Category
    for organ in Organ.objects.all():
        result_row = []

        # ORGAN
        result_row.append(organ.trial_id)
        result_row.append(organ.get_location_display())
        result_row.append(organ.not_transplantable_reason)
        result_row.append(organ.admin_notes)
        result_row.append(organ.transplantation_notes)
        result_row.append(organ.transplantation_form_completed)
        result_row.append(organ.paper_form_was_the_source)
        result_row.append(organ.included_for_analysis)
        try:
            result_row.append(organ.removal.strftime("%d-%m-%Y %H:%M %Z"))
        except AttributeError:
            result_row.append("")
        result_row.append(organ.renal_arteries)  # 10
        result_row.append(organ.get_graft_damage_display())
        result_row.append(organ.graft_damage_other)
        result_row.append(organ.washout_perfusion)
        result_row.append(organ.get_washout_perfusion_display())
        result_row.append(organ.transplantable)
        result_row.append(organ.not_transplantable_reason)
        result_row.append(organ.get_preservation_display())
        result_row.append(organ.perfusion_possible)
        result_row.append(organ.perfusion_not_possible_because)
        try:
            result_row.append(organ.perfusion_started.strftime("%d-%m-%Y %H:%M %Z"))  # 20
        except AttributeError:
            result_row.append("")
        result_row.append(organ.get_patch_holder_display())
        result_row.append(organ.artificial_patch_used)
        result_row.append(organ.get_artificial_patch_size_display())
        result_row.append(organ.artificial_patch_number)
        result_row.append(organ.oxygen_bottle_full)
        result_row.append(organ.oxygen_bottle_open)
        result_row.append(organ.oxygen_bottle_changed)
        try:
            result_row.append(organ.oxygen_bottle_changed_at.strftime("%d-%m-%Y %H:%M %Z"))
        except AttributeError:
            result_row.append("")
        result_row.append(organ.oxygen_bottle_changed_at_unknown)
        result_row.append(organ.ice_container_replenished)  # 30
        try:
            result_row.append(organ.ice_container_replenished_at.strftime("%d-%m-%Y %H:%M %Z"))
        except AttributeError:
            result_row.append("")
        result_row.append(organ.ice_container_replenished_at_unknown)
        result_row.append(organ.perfusate_measurable)
        result_row.append(organ.perfusate_measure)
        try:
            result_row.append(str(organ.perfusion_machine))
        except AttributeError:
            result_row.append("")

        # DONOR
        donor = organ.donor
        result_row.append(donor.person.number)
        try:
            result_row.append(donor.person.date_of_birth.strftime("%d-%m-%Y"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.person.date_of_birth_unknown)
        try:
            result_row.append(donor.person.date_of_death.strftime("%d-%m-%Y"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.person.date_of_death_unknown)
        result_row.append(donor.person.get_gender_display())
        result_row.append(donor.person.weight)
        result_row.append(donor.person.height)
        result_row.append(donor.person.get_ethnicity_display())
        result_row.append(donor.person.get_blood_group_display())  # 10
        result_row.append(donor.sequence_number)
        result_row.append("Y" if donor.multiple_recipients else "N")
        result_row.append(donor.get_not_randomised_because_display())
        result_row.append(donor.not_randomised_because_other)
        result_row.append(donor.procurement_form_completed)
        result_row.append(donor.admin_notes)
        try:
            result_row.append(str(donor.retrieval_team))
        except AttributeError:
            result_row.append("")
        try:
            result_row.append(str(donor.perfusion_technician))
        except AttributeError:
            result_row.append("")
        try:
            result_row.append(str(donor.transplant_coordinator))
        except AttributeError:
            result_row.append("")
        try:
            result_row.append(donor.call_received.strftime("%d-%m-%Y %H:%M %Z"))  # 20
        except AttributeError:
            result_row.append("")
        result_row.append(donor.call_received_unknown)
        result_row.append(str(donor.retrieval_hospital))
        try:
            result_row.append(donor.scheduled_start.strftime("%d-%m-%Y %H:%M %Z"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.scheduled_start_unknown)
        try:
            result_row.append(donor.technician_arrival.strftime("%d-%m-%Y %H:%M %Z"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.technician_arrival_unknown)
        try:
            result_row.append(donor.ice_boxes_filled.strftime("%d-%m-%Y %H:%M %Z"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.ice_boxes_filled_unknown)
        try:
            result_row.append(donor.depart_perfusion_centre.strftime("%d-%m-%Y %H:%M %Z"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.depart_perfusion_centre_unknown)  # 30
        try:
            result_row.append(donor.arrival_at_donor_hospital.strftime("%d-%m-%Y %H:%M %Z"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.arrival_at_donor_hospital_unknown)
        result_row.append(donor.age)
        try:
            result_row.append(donor.date_of_admission.strftime("%d-%m-%Y"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.date_of_admission_unknown)
        result_row.append(donor.admitted_to_itu)
        try:
            result_row.append(donor.date_admitted_to_itu.strftime("%d-%m-%Y"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.date_admitted_to_itu_unknown)
        try:
            result_row.append(donor.date_of_procurement.strftime("%d-%m-%Y"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.other_organs_procured)  # 40
        result_row.append(donor.other_organs_lungs)
        result_row.append(donor.other_organs_pancreas)
        result_row.append(donor.other_organs_liver)
        result_row.append(donor.other_organs_tissue)
        result_row.append(donor.get_diagnosis_display())
        result_row.append(donor.diagnosis_other)
        result_row.append(donor.get_diabetes_melitus_display())
        result_row.append(donor.get_alcohol_abuse_display())
        result_row.append(donor.cardiac_arrest)
        result_row.append(donor.systolic_blood_pressure)  # 50
        result_row.append(donor.diastolic_blood_pressure)
        result_row.append(donor.diuresis_last_day)
        result_row.append(donor.diuresis_last_day_unknown)
        result_row.append(donor.diuresis_last_hour)
        result_row.append(donor.diuresis_last_hour_unknown)
        result_row.append(donor.dopamine)
        result_row.append(donor.dobutamine)
        result_row.append(donor.nor_adrenaline)
        result_row.append(donor.vasopressine)
        result_row.append(donor.other_medication_details)  # 60
        result_row.append(donor.last_creatinine)
        result_row.append(donor.get_last_creatinine_unit_display())
        result_row.append(donor.max_creatinine)
        result_row.append(donor.max_creatinine_unit)
        try:
            result_row.append(donor.life_support_withdrawal.strftime("%d-%m-%Y %H:%M %Z"))
        except AttributeError:
            result_row.append("")
        try:
            result_row.append(donor.systolic_pressure_low.strftime("%d-%m-%Y %H:%M %Z"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.systolic_pressure_low_unknown)
        try:
            result_row.append(donor.o2_saturation.strftime("%d-%m-%Y %H:%M %Z"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.o2_saturation_unknown)
        try:
            result_row.append(donor.circulatory_arrest.strftime("%d-%m-%Y %H:%M %Z"))  # 70
        except AttributeError:
            result_row.append("")
        result_row.append(donor.circulatory_arrest_unknown)
        result_row.append(donor.length_of_no_touch)
        try:
            result_row.append(donor.death_diagnosed.strftime("%d-%m-%Y %H:%M %Z"))
        except AttributeError:
            result_row.append("")
        try:
            result_row.append(donor.perfusion_started.strftime("%d-%m-%Y %H:%M %Z"))
        except AttributeError:
            result_row.append("")
        result_row.append(donor.perfusion_started_unknown)
        result_row.append(donor.get_systemic_flush_used_display())
        result_row.append(donor.systemic_flush_used)
        result_row.append(donor.systemic_flush_used_other)
        result_row.append(donor.systemic_flush_volume_used)
        result_row.append(donor.heparin)  # 80

        # ALLOCATION
        allocation = organ.final_allocation
        if allocation is None:
            for x in range(17):
                result_row.append("No Allocation")
        else:
            try:
                result_row.append(str(allocation.perfusion_technician))
            except AttributeError:
                result_row.append("")
            try:
                result_row.append(allocation.call_received.strftime("%d-%m-%Y %H:%M %Z"))
            except AttributeError:
                result_row.append("")
            result_row.append(allocation.call_received_unknown)
            try:
                result_row.append(str(allocation.transplant_hospital))
            except AttributeError:
                result_row.append("")
            try:
                result_row.append(str(allocation.theatre_contact))
            except AttributeError:
                result_row.append("")
            try:
                result_row.append(allocation.scheduled_start.strftime("%d-%m-%Y %H:%M %Z"))
            except AttributeError:
                result_row.append("")
            result_row.append(allocation.scheduled_start_unknown)
            try:
                result_row.append(allocation.technician_arrival.strftime("%d-%m-%Y %H:%M %Z"))
            except AttributeError:
                result_row.append("")
            result_row.append(allocation.technician_arrival_unknown)
            try:
                result_row.append(allocation.depart_perfusion_centre.strftime("%d-%m-%Y %H:%M %Z"))  # 10
            except AttributeError:
                result_row.append("")
            result_row.append(allocation.depart_perfusion_centre_unknown)
            try:
                result_row.append(allocation.arrival_at_recipient_hospital.strftime("%d-%m-%Y %H:%M %Z"))
            except AttributeError:
                result_row.append("")
            result_row.append(allocation.arrival_at_recipient_hospital_unknown)
            result_row.append(allocation.journey_remarks)
            result_row.append(allocation.reallocated)
            result_row.append(allocation.get_reallocation_reason_display())
            result_row.append(allocation.reallocation_reason_other)

        # RECIPIENT
        recipient = organ.safe_recipient
        if recipient is None:
            # If no recipient, blank out all the recipient columns
            for x in range(48):
                result_row.append("No Recipient")
        else:
            result_row.append(recipient.person.number)
            try:
                result_row.append(recipient.person.date_of_birth.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(recipient.person.date_of_birth_unknown)
            try:
                result_row.append(recipient.person.date_of_death.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(recipient.person.date_of_death_unknown)
            result_row.append(recipient.person.get_gender_display())
            result_row.append(recipient.person.weight)
            result_row.append(recipient.person.height)
            result_row.append(recipient.person.get_ethnicity_display())
            result_row.append(recipient.person.get_blood_group_display())  # 10
            result_row.append(recipient.signed_consent)
            result_row.append(recipient.single_kidney_transplant)
            result_row.append(recipient.get_renal_disease_display())
            result_row.append(recipient.renal_disease_other)
            result_row.append(recipient.pre_transplant_diuresis)
            try:
                result_row.append(recipient.knife_to_skin.strftime("%d-%m-%Y %H:%M %Z"))
            except AttributeError:
                result_row.append("")
            result_row.append(recipient.perfusate_measure)
            try:
                result_row.append(recipient.perfusion_stopped.strftime("%d-%m-%Y %H:%M %Z"))
            except AttributeError:
                result_row.append("")
            result_row.append(recipient.organ_cold_stored)
            result_row.append(recipient.tape_broken)  # 20
            try:
                result_row.append(recipient.removed_from_machine_at.strftime("%d-%m-%Y %H:%M %Z"))
            except AttributeError:
                result_row.append("")
            result_row.append(recipient.oxygen_full_and_open)
            result_row.append(recipient.organ_untransplantable)
            result_row.append(recipient.organ_untransplantable_reason)
            try:
                result_row.append(recipient.anesthesia_started_at.strftime("%d-%m-%Y %H:%M %Z"))
            except AttributeError:
                result_row.append("")
            result_row.append(recipient.get_incision_display())
            result_row.append(recipient.get_transplant_side_display())
            result_row.append(recipient.get_arterial_problems_display())
            result_row.append(recipient.get_venous_problems_display())
            try:
                result_row.append(recipient.anastomosis_started_at.strftime("%d-%m-%Y %H:%M %Z"))  # 30
            except AttributeError:
                result_row.append("")
            result_row.append(recipient.anastomosis_started_at_unknown)
            try:
                result_row.append(recipient.reperfusion_started_at.strftime("%d-%m-%Y %H:%M %Z"))
            except AttributeError:
                result_row.append("")
            result_row.append(recipient.reperfusion_started_at_unknown)
            result_row.append(recipient.mannitol_used)
            result_row.append(recipient.other_diurectics)
            result_row.append(recipient.other_diurectics_details)
            result_row.append(recipient.systolic_blood_pressure)
            result_row.append(recipient.cvp)
            result_row.append(recipient.intra_operative_diuresis)
            result_row.append(recipient.successful_conclusion)  # 40
            try:
                result_row.append(recipient.operation_concluded_at.strftime("%d-%m-%Y %H:%M %Z"))
            except AttributeError:
                result_row.append("")
            result_row.append(recipient.probe_cleaned)
            result_row.append(recipient.ice_removed)
            result_row.append(recipient.oxygen_flow_stopped)
            result_row.append(recipient.oxygen_bottle_removed)
            result_row.append(recipient.box_cleaned)
            result_row.append(recipient.batteries_charged)
            result_row.append(recipient.cleaning_log)

        # FOLLOW UP 1
        try:
            followup = organ.followup_initial
            try:
                result_row.append(followup.start_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.notes)
            result_row.append(followup.paper_form_was_the_source)
            result_row.append(followup.graft_failure)
            try:
                result_row.append(followup.graft_failure_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.get_graft_failure_type_display())
            result_row.append(followup.graft_failure_type_other)
            result_row.append(followup.graft_removal)
            try:
                result_row.append(followup.graft_removal_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.get_dialysis_type_display())  # 10
            result_row.append(followup.immunosuppression_1)
            result_row.append(followup.immunosuppression_2)
            result_row.append(followup.immunosuppression_3)
            result_row.append(followup.immunosuppression_4)
            result_row.append(followup.immunosuppression_5)
            result_row.append(followup.immunosuppression_6)
            result_row.append(followup.immunosuppression_7)
            result_row.append(followup.immunosuppression_other)
            result_row.append(followup.rejection)
            result_row.append(followup.rejection_prednisolone)  # 20
            result_row.append(followup.rejection_drug)
            result_row.append(followup.rejection_drug_other)
            result_row.append(followup.rejection_biopsy)
            result_row.append(followup.calcineurin)  # End Base
            result_row.append(followup.get_serum_creatinine_unit_display())
            result_row.append(followup.serum_creatinine_1)
            result_row.append(followup.serum_creatinine_2)
            result_row.append(followup.serum_creatinine_3)
            result_row.append(followup.serum_creatinine_4)
            result_row.append(followup.serum_creatinine_5)  # 30
            result_row.append(followup.serum_creatinine_6)
            result_row.append(followup.serum_creatinine_7)
            result_row.append(followup.dialysis_requirement_1)
            result_row.append(followup.dialysis_requirement_2)
            result_row.append(followup.dialysis_requirement_3)
            result_row.append(followup.dialysis_requirement_4)
            result_row.append(followup.dialysis_requirement_5)
            result_row.append(followup.dialysis_requirement_6)
            result_row.append(followup.dialysis_requirement_7)
            result_row.append(followup.get_dialysis_cause_display())  # 40
            result_row.append(followup.dialysis_cause_other)
            result_row.append(followup.hla_mismatch_a)
            result_row.append(followup.hla_mismatch_b)
            result_row.append(followup.hla_mismatch_dr)
            result_row.append(followup.get_induction_therapy_display())
            try:
                result_row.append(followup.discharge_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
        except AttributeError:
            # If no followup, blank out all the followup columns
            for x in range(46):
                result_row.append("No FU 1")

        # FOLLOW UP 2
        try:
            followup = organ.followup_3m
            try:
                result_row.append(followup.start_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.notes)
            result_row.append(followup.paper_form_was_the_source)
            result_row.append(followup.graft_failure)
            try:
                result_row.append(followup.graft_failure_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.get_graft_failure_type_display())
            result_row.append(followup.graft_failure_type_other)
            result_row.append(followup.graft_removal)
            try:
                result_row.append(followup.graft_removal_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.get_dialysis_type_display())  # 10
            result_row.append(followup.immunosuppression_1)
            result_row.append(followup.immunosuppression_2)
            result_row.append(followup.immunosuppression_3)
            result_row.append(followup.immunosuppression_4)
            result_row.append(followup.immunosuppression_5)
            result_row.append(followup.immunosuppression_6)
            result_row.append(followup.immunosuppression_7)
            result_row.append(followup.immunosuppression_other)
            result_row.append(followup.rejection)
            result_row.append(followup.rejection_prednisolone)  # 20
            result_row.append(followup.rejection_drug)
            result_row.append(followup.rejection_drug_other)
            result_row.append(followup.rejection_biopsy)
            result_row.append(followup.calcineurin)  # End Base
            result_row.append(followup.get_serum_creatinine_unit_display())
            result_row.append(followup.serum_creatinine)
            result_row.append(followup.creatinine_clearance)
            result_row.append(followup.currently_on_dialysis)
            try:
                result_row.append(followup.dialysis_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.number_of_dialysis_sessions)
            result_row.append(followup.rejection_periods)
            result_row.append(followup.graft_complications)
            # QoL
            try:
                result_row.append(followup.quality_of_life.date_recorded.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.quality_of_life.qol_mobility)
            result_row.append(followup.quality_of_life.qol_selfcare)
            result_row.append(followup.quality_of_life.qol_usual_activities)
            result_row.append(followup.quality_of_life.qol_pain)
            result_row.append(followup.quality_of_life.qol_anxiety)
            result_row.append(followup.quality_of_life.vas_score)

        except AttributeError:
            # If no followup, blank out all the followup columns
            for x in range(39):
                result_row.append("No FU 2")

        # FOLLOW UP 3
        try:
            followup = organ.followup_6m
            try:
                result_row.append(followup.start_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.notes)
            result_row.append(followup.paper_form_was_the_source)
            result_row.append(followup.graft_failure)
            try:
                result_row.append(followup.graft_failure_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.get_graft_failure_type_display())
            result_row.append(followup.graft_failure_type_other)
            result_row.append(followup.graft_removal)
            try:
                result_row.append(followup.graft_removal_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.get_dialysis_type_display())  # 10
            result_row.append(followup.immunosuppression_1)
            result_row.append(followup.immunosuppression_2)
            result_row.append(followup.immunosuppression_3)
            result_row.append(followup.immunosuppression_4)
            result_row.append(followup.immunosuppression_5)
            result_row.append(followup.immunosuppression_6)
            result_row.append(followup.immunosuppression_7)
            result_row.append(followup.immunosuppression_other)
            result_row.append(followup.rejection)
            result_row.append(followup.rejection_prednisolone)  # 20
            result_row.append(followup.rejection_drug)
            result_row.append(followup.rejection_drug_other)
            result_row.append(followup.rejection_biopsy)
            result_row.append(followup.calcineurin)  # End Base
            result_row.append(followup.get_serum_creatinine_unit_display())
            result_row.append(followup.serum_creatinine)
            result_row.append(followup.creatinine_clearance)
            result_row.append(followup.currently_on_dialysis)
            try:
                result_row.append(followup.dialysis_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.number_of_dialysis_sessions)  # 30
            result_row.append(followup.rejection_periods)
            result_row.append(followup.graft_complications)

        except AttributeError:
            # If no followup, blank out all the followup columns
            for x in range(32):  # TODO: Correct the column count
                result_row.append("No FU 3")

        # FOLLOW UP 4
        try:
            followup = organ.followup_1y
            try:
                result_row.append(followup.start_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.notes)
            result_row.append(followup.paper_form_was_the_source)
            result_row.append(followup.graft_failure)
            try:
                result_row.append(followup.graft_failure_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.get_graft_failure_type_display())
            result_row.append(followup.graft_failure_type_other)
            result_row.append(followup.graft_removal)
            try:
                result_row.append(followup.graft_removal_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.get_dialysis_type_display())  # 10
            result_row.append(followup.immunosuppression_1)
            result_row.append(followup.immunosuppression_2)
            result_row.append(followup.immunosuppression_3)
            result_row.append(followup.immunosuppression_4)
            result_row.append(followup.immunosuppression_5)
            result_row.append(followup.immunosuppression_6)
            result_row.append(followup.immunosuppression_7)
            result_row.append(followup.immunosuppression_other)
            result_row.append(followup.rejection)
            result_row.append(followup.rejection_prednisolone)  # 20
            result_row.append(followup.rejection_drug)
            result_row.append(followup.rejection_drug_other)
            result_row.append(followup.rejection_biopsy)
            result_row.append(followup.calcineurin)  # End Base
            result_row.append(followup.get_serum_creatinine_unit_display())
            result_row.append(followup.serum_creatinine)
            result_row.append(followup.creatinine_clearance)
            result_row.append(followup.currently_on_dialysis)
            try:
                result_row.append(followup.dialysis_date.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.number_of_dialysis_sessions)  # 30
            result_row.append(followup.rejection_periods)
            result_row.append(followup.graft_complications)
            # QoL
            try:
                result_row.append(followup.quality_of_life.date_recorded.strftime("%d-%m-%Y"))
            except AttributeError:
                result_row.append("")
            result_row.append(followup.quality_of_life.qol_mobility)
            result_row.append(followup.quality_of_life.qol_selfcare)
            result_row.append(followup.quality_of_life.qol_usual_activities)
            result_row.append(followup.quality_of_life.qol_pain)
            result_row.append(followup.quality_of_life.qol_anxiety)
            result_row.append(followup.quality_of_life.vas_score)

        except AttributeError:
            # If no followup, blank out all the followup columns
            for x in range(39):
                result_row.append("No FU 4")

        total_count = 0
        serious_eligible_1_ids = []
        death_ids = []
        pgf_category_ids = []


        for event in organ.event_set.all():
            total_count += 1
            if event.serious_eligible_1:
                serious_eligible_1_ids.append(event.id)
            if event.death:
                death_ids.append(event.id)
            if pgf_category_object in event.categories.all():
                pgf_category_ids.append(event.id)

        result_row.append(total_count)
        result_row.append(serious_eligible_1_ids)
        result_row.append(death_ids)
        result_row.append(pgf_category_ids)

        writer.writerow(result_row)

    return response
