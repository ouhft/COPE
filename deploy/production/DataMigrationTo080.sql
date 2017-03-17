/*
 Script to migrate data safely from a 0.6.4 schema to the new 0.8.0 schema

 #######################################################################################################################
 ################################################  Order of processing  ################################################
 #######################################################################################################################
 - create new database `pm migrate`
 - load fixture `pm loaddata config/fixtures/05_hospitals.json`
 - load fixture `pm loaddata config/fixtures/06_adverseevent_categories.json`
 - EXECUTE BLOCK 1
 - Make the Trial IDs via Python script in shell:
     ```
    from wp4.compare.models.donor import Donor
    for donor in Donor.objects.all():
        donor.trial_id = donor.make_trial_id()
        donor.save()
        donor.left_kidney.trial_id = donor.left_kidney.make_trial_id()
        donor.left_kidney.save()
        donor.right_kidney.trial_id = donor.right_kidney.make_trial_id()
        donor.right_kidney.save()

     ```
 */


/* =============================================  BLOCK 1  =============================================  */
DETACH DATABASE old;
DETACH DATABASE new;
-- ATTACH DATABASE 'file:///Users/carl/Projects/py3_cope/cope_repo/old.db.sqlite3?mode=ro' AS old;
-- ATTACH DATABASE 'file:///Users/carl/Projects/py3_cope/cope_repo/db.sqlite3' AS new;
ATTACH DATABASE 'file:///Volumes/ExtSSD/Users/carl/Projects/py3_cope/cope_repo/old.db.sqlite3?mode=ro' AS old;
ATTACH DATABASE 'file:///Volumes/ExtSSD/Users/carl/Projects/py3_cope/cope_repo//db.sqlite3' AS new;

DELETE FROM new.django_site;

UPDATE SQLITE_SEQUENCE
SET SEQ = 0
WHERE NAME = 'new.django_site';

INSERT INTO new.django_site (id, name, domain)
  SELECT
    id,
    name,
    domain
  FROM old.django_site;

-- Bring in all the staff ------------------------------------------------------------------------
INSERT INTO new.staff_person (
  id,
  password,
  last_login,
  is_superuser,
  username,
  first_name,
  last_name,
  email,
  is_staff,
  is_active,
  date_joined,
  telephone,
  based_at_id
)
  SELECT
    oau.id,
    oau.password,
    oau.last_login,
    oau.is_superuser,
    oau.username,
    ifnull(osp.first_names, oau.first_name),
    ifnull(osp.last_names, oau.last_name),
    ifnull(osp.email, oau.email),
    oau.is_staff,
    oau.is_active,
    oau.date_joined,
    osp.telephone,
    osp.based_at_id
  FROM old.auth_user AS oau
    LEFT OUTER JOIN old.staff_person_staffperson AS osp
      ON osp.user_id = oau.id;

-- Add these users to their relevant groups based on their Staff_Person jobs
INSERT INTO new.staff_person_groups (
  person_id,
  group_id
)
  SELECT
    sp.user_id,
    spj.staffjob_id
  FROM old.staff_person_staffperson_jobs AS spj,
    old.staff_person_staffperson AS sp
  WHERE sp.id = spj.staffperson_id
        AND sp.user_id IS NOT NULL;

-- Copy the Randomisations. NB, allocated_on, and allocated_by will need manual cleaning up
INSERT INTO new.compare_randomisation (id, list_code, result, allocated_on, allocated_by_id, donor_id)
  SELECT
    id,
    list_code,
    result,
    allocated_on,
    1,
    donor_id
  FROM old.compare_randomisation;

-- Copy the admin log over, but purge the ones related to deleted content types
INSERT INTO new.django_admin_log (
  id, object_id, object_repr, action_flag, change_message, content_type_id, user_id, action_time)
  SELECT
    id,
    object_id,
    object_repr,
    action_flag,
    change_message,
    content_type_id,
    user_id,
    action_time
  FROM old.django_admin_log
  WHERE NOT content_type_id IN (9, 10, 13);

-- Copy the Perfusion machine data across, noting the change in table name
INSERT INTO new.perfusion_machine_machine (id, machine_serial_number, machine_reference_number)
  SELECT
    id,
    machine_serial_number,
    machine_reference_number
  FROM old.perfusion_machine_perfusionmachine;

/*
 Changes to account for:
 - table renamed to compare_patient
 - live - added as part of AuditControlModelBase, defaults to 1
 */
INSERT INTO new.compare_patient (
id,
record_locked,
live,
number,
date_of_birth,
date_of_birth_unknown,
date_of_death,
date_of_death_unknown,
gender,
weight,
height,
ethnicity,
blood_group
)
  SELECT
    id,
    record_locked,
    1,
    number,
    date_of_birth,
    date_of_birth_unknown,
    date_of_death,
    date_of_death_unknown,
    gender,
    weight,
    height,
    ethnicity,
    blood_group
  FROM old.compare_organperson;

/*
 Changes to account for:
 - live - added as part of AuditControlModelBase, defaults to 1
 - perfusion_technician - was Staff_Person.StaffPerson, now Staff.Person, which replaces Auth.User
 - transplant_coordinator - was Staff_Person.StaffPerson, now Staff.Person, which replaces Auth.User
 - retrieval_hospital - is a new charfield that replaces retrieval_hospital(_id) which was a Location.Hospital
 */
INSERT INTO new.compare_donor (
id,
record_locked,
live,
sequence_number,
multiple_recipients,
not_randomised_because,
not_randomised_because_other,
procurement_form_completed,
admin_notes,
trial_id,
call_received,
call_received_unknown,
retrieval_hospital,
scheduled_start,
scheduled_start_unknown,
technician_arrival,
technician_arrival_unknown,
ice_boxes_filled,
ice_boxes_filled_unknown,
depart_perfusion_centre,
depart_perfusion_centre_unknown,
arrival_at_donor_hospital,
arrival_at_donor_hospital_unknown,
age,
date_of_admission,
date_of_admission_unknown,
admitted_to_itu,
date_admitted_to_itu,
date_admitted_to_itu_unknown,
date_of_procurement,
other_organs_procured,
other_organs_lungs,
other_organs_pancreas,
other_organs_liver,
other_organs_tissue,
diagnosis,
diagnosis_other,
diabetes_melitus,
alcohol_abuse,
cardiac_arrest,
systolic_blood_pressure,
diastolic_blood_pressure,
diuresis_last_day,
diuresis_last_day_unknown,
diuresis_last_hour,
diuresis_last_hour_unknown,
dopamine,
dobutamine,
nor_adrenaline,
vasopressine,
other_medication_details,
last_creatinine,
last_creatinine_unit,
max_creatinine,
max_creatinine_unit,
life_support_withdrawal,
systolic_pressure_low,
systolic_pressure_low_unknown,
o2_saturation,
o2_saturation_unknown,
circulatory_arrest,
circulatory_arrest_unknown,
length_of_no_touch,
death_diagnosed,
perfusion_started,
perfusion_started_unknown,
systemic_flush_used,
systemic_flush_used_other,
systemic_flush_volume_used,
heparin,
_left_kidney_id,
_right_kidney_id,
perfusion_technician_id,
person_id,
retrieval_team_id,
transplant_coordinator_id,
_order
)
  SELECT
    odo.id,
    odo.record_locked,
    1,
    odo.sequence_number,
    odo.multiple_recipients,
    odo.not_randomised_because,
    odo.not_randomised_because_other,
    odo.procurement_form_completed,
    odo.admin_notes,
    '',
    odo.call_received,
    odo.call_received_unknown,
    ifnull(oho.name, ''),
    odo.scheduled_start,
    odo.scheduled_start_unknown,
    odo.technician_arrival,
    odo.technician_arrival_unknown,
    odo.ice_boxes_filled,
    odo.ice_boxes_filled_unknown,
    odo.depart_perfusion_centre,
    odo.depart_perfusion_centre_unknown,
    odo.arrival_at_donor_hospital,
    odo.arrival_at_donor_hospital_unknown,
    odo.age,
    odo.date_of_admission,
    odo.date_of_admission_unknown,
    odo.admitted_to_itu,
    odo.date_admitted_to_itu,
    odo.date_admitted_to_itu_unknown,
    odo.date_of_procurement,
    odo.other_organs_procured,
    odo.other_organs_lungs,
    odo.other_organs_pancreas,
    odo.other_organs_liver,
    odo.other_organs_tissue,
    odo.diagnosis,
    odo.diagnosis_other,
    odo.diabetes_melitus,
    odo.alcohol_abuse,
    odo.cardiac_arrest,
    odo.systolic_blood_pressure,
    odo.diastolic_blood_pressure,
    odo.diuresis_last_day,
    odo.diuresis_last_day_unknown,
    odo.diuresis_last_hour,
    odo.diuresis_last_hour_unknown,
    odo.dopamine,
    odo.dobutamine,
    odo.nor_adrenaline,
    odo.vasopressine,
    odo.other_medication_details,
    odo.last_creatinine,
    odo.last_creatinine_unit,
    odo.max_creatinine,
    odo.max_creatinine_unit,
    odo.life_support_withdrawal,
    odo.systolic_pressure_low,
    odo.systolic_pressure_low_unknown,
    odo.o2_saturation,
    odo.o2_saturation_unknown,
    odo.circulatory_arrest,
    odo.circulatory_arrest_unknown,
    odo.length_of_no_touch,
    odo.death_diagnosed,
    odo.perfusion_started,
    odo.perfusion_started_unknown,
    odo.systemic_flush_used,
    odo.systemic_flush_used_other,
    odo.systemic_flush_volume_used,
    odo.heparin,
    odo._left_kidney_id,
    odo._right_kidney_id,
    techid_sp.user_id,
    odo.person_id,
    odo.retrieval_team_id,
    coorid_sp.user_id,
    odo._order
  FROM old.compare_donor AS odo,
    old.staff_person_staffperson AS techid_sp
    LEFT OUTER JOIN old.locations_hospital AS oho
      ON odo.retrieval_hospital_id = oho.id
    LEFT OUTER JOIN old.staff_person_staffperson AS coorid_sp
      ON odo.transplant_coordinator_id = coorid_sp.id
  WHERE odo.perfusion_technician_id = techid_sp.id;

/*
 Changes to account for:
 - live - added as part of AuditControlModelBase, defaults to 1
 - perfusion_file_id removed
 */
INSERT INTO new.compare_organ
(
id,
record_locked,
live,
location,
not_allocated_reason,
admin_notes,
transplantation_notes,
transplantation_form_completed,
trial_id,
removal,
renal_arteries,
graft_damage,
graft_damage_other,
washout_perfusion,
transplantable,
not_transplantable_reason,
preservation,
perfusion_possible,
perfusion_not_possible_because,
perfusion_started,
patch_holder,
artificial_patch_used,
artificial_patch_size,
artificial_patch_number,
oxygen_bottle_full,
oxygen_bottle_open,
oxygen_bottle_changed,
oxygen_bottle_changed_at,
oxygen_bottle_changed_at_unknown,
ice_container_replenished,
ice_container_replenished_at,
ice_container_replenished_at_unknown,
perfusate_measurable,
perfusate_measure,
donor_id,
perfusion_machine_id
)
  SELECT
    oco.id,
    oco.record_locked,
    1,
    oco.location,
    oco.not_allocated_reason,
    oco.admin_notes,
    oco.transplantation_notes,
    oco.transplantation_form_completed,
    '',
    oco.removal,
    oco.renal_arteries,
    oco.graft_damage,
    oco.graft_damage_other,
    oco.washout_perfusion,
    oco.transplantable,
    oco.not_transplantable_reason,
    oco.preservation,
    oco.perfusion_possible,
    oco.perfusion_not_possible_because,
    oco.perfusion_started,
    oco.patch_holder,
    oco.artificial_patch_used,
    oco.artificial_patch_size,
    oco.artificial_patch_number,
    oco.oxygen_bottle_full,
    oco.oxygen_bottle_open,
    oco.oxygen_bottle_changed,
    oco.oxygen_bottle_changed_at,
    oco.oxygen_bottle_changed_at_unknown,
    oco.ice_container_replenished,
    oco.ice_container_replenished_at,
    oco.ice_container_replenished_at_unknown,
    oco.perfusate_measurable,
    oco.perfusate_measure,
    oco.donor_id,
    oco.perfusion_machine_id
  FROM old.compare_organ AS oco;

-- Copy procurement resource
INSERT INTO new.compare_procurementresource (
  id,
record_locked,
live,
type,
lot_number,
expiry_date,
expiry_date_unknown,
organ_id
)
  SELECT
    id,
    0,
    1,
    type,
    ifnull(lot_number, ''),
    expiry_date,
    expiry_date_unknown,
    organ_id
  FROM old.compare_procurementresource;

/*
 Changes to account for:
 - live - added as part of AuditControlModelBase, defaults to 1
 - perfusion_technician - was Staff_Person.StaffPerson, now Staff.Person, which replaces Auth.User
 - theatre_contact - was Staff_Person.StaffPerson, now Staff.Person, which replaces Auth.User
 */

INSERT INTO new.compare_organallocation (
id,
record_locked,
live,
call_received,
call_received_unknown,
scheduled_start,
scheduled_start_unknown,
technician_arrival,
technician_arrival_unknown,
depart_perfusion_centre,
depart_perfusion_centre_unknown,
arrival_at_recipient_hospital,
arrival_at_recipient_hospital_unknown,
journey_remarks,
reallocated,
reallocation_reason,
reallocation_reason_other,
organ_id,
perfusion_technician_id,
theatre_contact_id,
transplant_hospital_id,
_order,
reallocation_id
)
  SELECT
    compare_organallocation.id,
    compare_organallocation.record_locked,
    1,
    compare_organallocation.call_received,
    compare_organallocation.call_received_unknown,
    compare_organallocation.scheduled_start,
    compare_organallocation.scheduled_start_unknown,
    compare_organallocation.technician_arrival,
    compare_organallocation.technician_arrival_unknown,
    compare_organallocation.depart_perfusion_centre,
    compare_organallocation.depart_perfusion_centre_unknown,
    compare_organallocation.arrival_at_recipient_hospital,
    compare_organallocation.arrival_at_recipient_hospital_unknown,
    compare_organallocation.journey_remarks,
    compare_organallocation.reallocated,
    compare_organallocation.reallocation_reason,
    compare_organallocation.reallocation_reason_other,
    compare_organallocation.organ_id,
    pt_sp.user_id,
    tc_sp.user_id,
    compare_organallocation.transplant_hospital_id,
    compare_organallocation._order,
    compare_organallocation.reallocation_id
  FROM old.compare_organallocation AS compare_organallocation
    LEFT OUTER JOIN old.staff_person_staffperson AS tc_sp
      ON compare_organallocation.theatre_contact_id = tc_sp.id
    LEFT OUTER JOIN old.staff_person_staffperson AS pt_sp
      ON compare_organallocation.perfusion_technician_id = pt_sp.id;
/* Map old austria other, to new other/austria as per Issue #210 */
UPDATE new.compare_organallocation
SET transplant_hospital_id = 14
WHERE transplant_hospital_id = 35;
UPDATE new.compare_organallocation
SET transplant_hospital_id = 18
WHERE transplant_hospital_id = 37;
UPDATE new.compare_organallocation
SET transplant_hospital_id = 24
WHERE transplant_hospital_id = 57;
UPDATE new.compare_organallocation
SET transplant_hospital_id = 13
WHERE transplant_hospital_id = 58;
UPDATE new.compare_organallocation
SET transplant_hospital_id = 23
WHERE transplant_hospital_id = 63;
UPDATE new.compare_organallocation
SET transplant_hospital_id = 26
WHERE transplant_hospital_id = 68;

/*
 Changes to account for:
 - live - added as part of AuditControlModelBase, defaults to 1
 */
INSERT INTO new.compare_recipient (
id,
record_locked,
live,
signed_consent,
single_kidney_transplant,
renal_disease,
renal_disease_other,
pre_transplant_diuresis,
knife_to_skin,
perfusate_measure,
perfusion_stopped,
organ_cold_stored,
tape_broken,
removed_from_machine_at,
oxygen_full_and_open,
organ_untransplantable,
organ_untransplantable_reason,
anesthesia_started_at,
incision,
transplant_side,
arterial_problems,
arterial_problems_other,
venous_problems,
venous_problems_other,
anastomosis_started_at,
anastomosis_started_at_unknown,
reperfusion_started_at,
reperfusion_started_at_unknown,
mannitol_used,
other_diurectics,
other_diurectics_details,
systolic_blood_pressure,
cvp,
intra_operative_diuresis,
successful_conclusion,
operation_concluded_at,
probe_cleaned,
ice_removed,
oxygen_flow_stopped,
oxygen_bottle_removed,
box_cleaned,
batteries_charged,
cleaning_log,
allocation_id,
organ_id,
person_id,
_order
)
  SELECT
    ocr.id,
    ocr.record_locked,
    1,
    ocr.signed_consent,
    ocr.single_kidney_transplant,
    ocr.renal_disease,
    ocr.renal_disease_other,
    ocr.pre_transplant_diuresis,
    ocr.knife_to_skin,
    ocr.perfusate_measure,
    ocr.perfusion_stopped,
    ocr.organ_cold_stored,
    ocr.tape_broken,
    ocr.removed_from_machine_at,
    ocr.oxygen_full_and_open,
    ocr.organ_untransplantable,
    ocr.organ_untransplantable_reason,
    ocr.anesthesia_started_at,
    ocr.incision,
    ocr.transplant_side,
    ocr.arterial_problems,
    ocr.arterial_problems_other,
    ocr.venous_problems,
    ocr.venous_problems_other,
    ocr.anastomosis_started_at,
    ocr.anastomosis_started_at_unknown,
    ocr.reperfusion_started_at,
    ocr.reperfusion_started_at_unknown,
    ocr.mannitol_used,
    ocr.other_diurectics,
    ocr.other_diurectics_details,
    ocr.systolic_blood_pressure,
    ocr.cvp,
    ocr.intra_operative_diuresis,
    ocr.successful_conclusion,
    ocr.operation_concluded_at,
    ocr.probe_cleaned,
    ocr.ice_removed,
    ocr.oxygen_flow_stopped,
    ocr.oxygen_bottle_removed,
    ocr.box_cleaned,
    ocr.batteries_charged,
    ocr.cleaning_log,
    ocr.allocation_id,
    ocr.organ_id,
    ocr.person_id,
    ocr._order
  FROM old.compare_recipient AS ocr;

/*
 adverse_event_event  (was adverse_event_adverseevent)
 Changes to account for:
 - live - added as part of AuditControlModelBase, defaults to 1
 - date_of_death - removed from old, no long part of new event
 - contact - was Staff_Person.StaffPerson, now Staff.Person, which replaces Auth.User
 - Where contact is missing (19 or so examples) set it to Ina Jochmans (Staff id=108) as PI
 */
INSERT INTO new.adverse_event_event (
id,
record_locked,
live,
serious_eligible_1,
serious_eligible_2,
serious_eligible_3,
serious_eligible_4,
serious_eligible_5,
serious_eligible_6,
onset_at_date,
event_ongoing,
description,
action,
outcome,
alive_query_1,
alive_query_2,
alive_query_3,
alive_query_4,
alive_query_5,
alive_query_6,
alive_query_7,
alive_query_8,
alive_query_9,
rehospitalisation,
date_of_admission,
date_of_discharge,
admitted_to_itu,
dialysis_needed,
biopsy_taken,
surgery_required,
rehospitalisation_comments,
death,
treatment_related,
cause_of_death_1,
cause_of_death_2,
cause_of_death_3,
cause_of_death_4,
cause_of_death_5,
cause_of_death_6,
cause_of_death_comment,
contact_id,
organ_id,
_order
)
  SELECT
    adverse_event_event.id,
    adverse_event_event.record_locked,
    1,
    adverse_event_event.serious_eligible_1,
    adverse_event_event.serious_eligible_2,
    adverse_event_event.serious_eligible_3,
    adverse_event_event.serious_eligible_4,
    adverse_event_event.serious_eligible_5,
    adverse_event_event.serious_eligible_6,
    adverse_event_event.onset_at_date,
    adverse_event_event.event_ongoing,
    adverse_event_event.description,
    adverse_event_event.action,
    adverse_event_event.outcome,
    adverse_event_event.alive_query_1,
    adverse_event_event.alive_query_2,
    adverse_event_event.alive_query_3,
    adverse_event_event.alive_query_4,
    adverse_event_event.alive_query_5,
    adverse_event_event.alive_query_6,
    adverse_event_event.alive_query_7,
    adverse_event_event.alive_query_8,
    adverse_event_event.alive_query_9,
    adverse_event_event.rehospitalisation,
    adverse_event_event.date_of_admission,
    adverse_event_event.date_of_discharge,
    adverse_event_event.admitted_to_itu,
    adverse_event_event.dialysis_needed,
    adverse_event_event.biopsy_taken,
    adverse_event_event.surgery_required,
    adverse_event_event.rehospitalisation_comments,
    adverse_event_event.death,
    adverse_event_event.treatment_related,
    adverse_event_event.cause_of_death_1,
    adverse_event_event.cause_of_death_2,
    adverse_event_event.cause_of_death_3,
    adverse_event_event.cause_of_death_4,
    adverse_event_event.cause_of_death_5,
    adverse_event_event.cause_of_death_6,
    adverse_event_event.cause_of_death_comment,
    ifnull(contact_sp.user_id, 108),
    adverse_event_event.organ_id,
    adverse_event_event._order
  FROM old.adverse_event_adverseevent AS adverse_event_event
    LEFT OUTER JOIN old.staff_person_staffperson AS contact_sp
      ON adverse_event_event.contact_id = contact_sp.id;

/*
 TABLE: followups_followup1y
 Changes to account for:
 - live - added as part of AuditControlModelBase, defaults to 1
 */
INSERT INTO new.followups_followup1y (
id,
record_locked,
live,
start_date,
notes,
graft_failure,
graft_failure_date,
graft_failure_type,
graft_failure_type_other,
graft_removal,
graft_removal_date,
dialysis_type,
immunosuppression_1,
immunosuppression_2,
immunosuppression_3,
immunosuppression_4,
immunosuppression_5,
immunosuppression_6,
immunosuppression_7,
immunosuppression_other,
rejection,
rejection_prednisolone,
rejection_drug,
rejection_drug_other,
rejection_biopsy,
calcineurin,
serum_creatinine_unit,
serum_creatinine,
creatinine_clearance,
currently_on_dialysis,
dialysis_date,
number_of_dialysis_sessions,
rejection_periods,
graft_complications,
organ_id,
quality_of_life_id
)
  SELECT
    followups_followup1y.id,
    followups_followup1y.record_locked,
    1,
    followups_followup1y.start_date,
    followups_followup1y.notes,
    followups_followup1y.graft_failure,
    followups_followup1y.graft_failure_date,
    followups_followup1y.graft_failure_type,
    followups_followup1y.graft_failure_type_other,
    followups_followup1y.graft_removal,
    followups_followup1y.graft_removal_date,
    followups_followup1y.dialysis_type,
    followups_followup1y.immunosuppression_1,
    followups_followup1y.immunosuppression_2,
    followups_followup1y.immunosuppression_3,
    followups_followup1y.immunosuppression_4,
    followups_followup1y.immunosuppression_5,
    followups_followup1y.immunosuppression_6,
    followups_followup1y.immunosuppression_7,
    followups_followup1y.immunosuppression_other,
    followups_followup1y.rejection,
    followups_followup1y.rejection_prednisolone,
    followups_followup1y.rejection_drug,
    followups_followup1y.rejection_drug_other,
    followups_followup1y.rejection_biopsy,
    followups_followup1y.calcineurin,
    followups_followup1y.serum_creatinine_unit,
    followups_followup1y.serum_creatinine,
    followups_followup1y.creatinine_clearance,
    followups_followup1y.currently_on_dialysis,
    followups_followup1y.dialysis_date,
    followups_followup1y.number_of_dialysis_sessions,
    followups_followup1y.rejection_periods,
    followups_followup1y.graft_complications,
    followups_followup1y.organ_id,
    followups_followup1y.quality_of_life_id
  FROM old.followups_followup1y AS followups_followup1y;

/*
 TABLE: followups_followup3m
 Changes to account for:
 - live - added as part of AuditControlModelBase, defaults to 1
 */
INSERT INTO new.followups_followup3m (
id,
record_locked,
live,
start_date,
notes,
graft_failure,
graft_failure_date,
graft_failure_type,
graft_failure_type_other,
graft_removal,
graft_removal_date,
dialysis_type,
immunosuppression_1,
immunosuppression_2,
immunosuppression_3,
immunosuppression_4,
immunosuppression_5,
immunosuppression_6,
immunosuppression_7,
immunosuppression_other,
rejection,
rejection_prednisolone,
rejection_drug,
rejection_drug_other,
rejection_biopsy,
calcineurin,
serum_creatinine_unit,
serum_creatinine,
creatinine_clearance,
currently_on_dialysis,
dialysis_date,
number_of_dialysis_sessions,
rejection_periods,
graft_complications,
organ_id,
quality_of_life_id
)
  SELECT
    followups_followup3m.id,
    followups_followup3m.record_locked,
    1,
    followups_followup3m.start_date,
    followups_followup3m.notes,
    followups_followup3m.graft_failure,
    followups_followup3m.graft_failure_date,
    followups_followup3m.graft_failure_type,
    followups_followup3m.graft_failure_type_other,
    followups_followup3m.graft_removal,
    followups_followup3m.graft_removal_date,
    followups_followup3m.dialysis_type,
    followups_followup3m.immunosuppression_1,
    followups_followup3m.immunosuppression_2,
    followups_followup3m.immunosuppression_3,
    followups_followup3m.immunosuppression_4,
    followups_followup3m.immunosuppression_5,
    followups_followup3m.immunosuppression_6,
    followups_followup3m.immunosuppression_7,
    followups_followup3m.immunosuppression_other,
    followups_followup3m.rejection,
    followups_followup3m.rejection_prednisolone,
    followups_followup3m.rejection_drug,
    followups_followup3m.rejection_drug_other,
    followups_followup3m.rejection_biopsy,
    followups_followup3m.calcineurin,
    followups_followup3m.serum_creatinine_unit,
    followups_followup3m.serum_creatinine,
    followups_followup3m.creatinine_clearance,
    followups_followup3m.currently_on_dialysis,
    followups_followup3m.dialysis_date,
    followups_followup3m.number_of_dialysis_sessions,
    followups_followup3m.rejection_periods,
    followups_followup3m.graft_complications,
    followups_followup3m.organ_id,
    followups_followup3m.quality_of_life_id
  FROM old.followups_followup3m AS followups_followup3m;

/*
 TABLE: followups_followup6m
 Changes to account for:
 - live - added as part of AuditControlModelBase, defaults to 1
 */
INSERT INTO new.followups_followup6m (
id,
record_locked,
live,
start_date,
notes,
graft_failure,
graft_failure_date,
graft_failure_type,
graft_failure_type_other,
graft_removal,
graft_removal_date,
dialysis_type,
immunosuppression_1,
immunosuppression_2,
immunosuppression_3,
immunosuppression_4,
immunosuppression_5,
immunosuppression_6,
immunosuppression_7,
immunosuppression_other,
rejection,
rejection_prednisolone,
rejection_drug,
rejection_drug_other,
rejection_biopsy,
calcineurin,
serum_creatinine_unit,
serum_creatinine,
creatinine_clearance,
currently_on_dialysis,
dialysis_date,
number_of_dialysis_sessions,
rejection_periods,
graft_complications,
organ_id
)
  SELECT
    followups_followup6m.id,
    followups_followup6m.record_locked,
    1,
    followups_followup6m.start_date,
    followups_followup6m.notes,
    followups_followup6m.graft_failure,
    followups_followup6m.graft_failure_date,
    followups_followup6m.graft_failure_type,
    followups_followup6m.graft_failure_type_other,
    followups_followup6m.graft_removal,
    followups_followup6m.graft_removal_date,
    followups_followup6m.dialysis_type,
    followups_followup6m.immunosuppression_1,
    followups_followup6m.immunosuppression_2,
    followups_followup6m.immunosuppression_3,
    followups_followup6m.immunosuppression_4,
    followups_followup6m.immunosuppression_5,
    followups_followup6m.immunosuppression_6,
    followups_followup6m.immunosuppression_7,
    followups_followup6m.immunosuppression_other,
    followups_followup6m.rejection,
    followups_followup6m.rejection_prednisolone,
    followups_followup6m.rejection_drug,
    followups_followup6m.rejection_drug_other,
    followups_followup6m.rejection_biopsy,
    followups_followup6m.calcineurin,
    followups_followup6m.serum_creatinine_unit,
    followups_followup6m.serum_creatinine,
    followups_followup6m.creatinine_clearance,
    followups_followup6m.currently_on_dialysis,
    followups_followup6m.dialysis_date,
    followups_followup6m.number_of_dialysis_sessions,
    followups_followup6m.rejection_periods,
    followups_followup6m.graft_complications,
    followups_followup6m.organ_id
  FROM old.followups_followup6m AS followups_followup6m;

/*
 TABLE: followups_followupinitial
 Changes to account for:
 - live - added as part of AuditControlModelBase, defaults to 1
 */
INSERT INTO new.followups_followupinitial (
id,
record_locked,
live,
start_date,
notes,
graft_failure,
graft_failure_date,
graft_failure_type,
graft_failure_type_other,
graft_removal,
graft_removal_date,
dialysis_type,
immunosuppression_1,
immunosuppression_2,
immunosuppression_3,
immunosuppression_4,
immunosuppression_5,
immunosuppression_6,
immunosuppression_7,
immunosuppression_other,
rejection,
rejection_prednisolone,
rejection_drug,
rejection_drug_other,
rejection_biopsy,
calcineurin,
serum_creatinine_unit,
serum_creatinine_1,
serum_creatinine_2,
serum_creatinine_3,
serum_creatinine_4,
serum_creatinine_5,
serum_creatinine_6,
serum_creatinine_7,
dialysis_requirement_1,
dialysis_requirement_2,
dialysis_requirement_3,
dialysis_requirement_4,
dialysis_requirement_5,
dialysis_requirement_6,
dialysis_requirement_7,
dialysis_cause,
dialysis_cause_other,
hla_mismatch_a,
hla_mismatch_b,
hla_mismatch_dr,
induction_therapy,
discharge_date,
organ_id
)
  SELECT
    followups_followupinitial.id,
    followups_followupinitial.record_locked,
    1,
    followups_followupinitial.start_date,
    followups_followupinitial.notes,
    followups_followupinitial.graft_failure,
    followups_followupinitial.graft_failure_date,
    followups_followupinitial.graft_failure_type,
    followups_followupinitial.graft_failure_type_other,
    followups_followupinitial.graft_removal,
    followups_followupinitial.graft_removal_date,
    followups_followupinitial.dialysis_type,
    followups_followupinitial.immunosuppression_1,
    followups_followupinitial.immunosuppression_2,
    followups_followupinitial.immunosuppression_3,
    followups_followupinitial.immunosuppression_4,
    followups_followupinitial.immunosuppression_5,
    followups_followupinitial.immunosuppression_6,
    followups_followupinitial.immunosuppression_7,
    followups_followupinitial.immunosuppression_other,
    followups_followupinitial.rejection,
    followups_followupinitial.rejection_prednisolone,
    followups_followupinitial.rejection_drug,
    followups_followupinitial.rejection_drug_other,
    followups_followupinitial.rejection_biopsy,
    followups_followupinitial.calcineurin,
    followups_followupinitial.serum_creatinine_unit,
    followups_followupinitial.serum_creatinine_1,
    followups_followupinitial.serum_creatinine_2,
    followups_followupinitial.serum_creatinine_3,
    followups_followupinitial.serum_creatinine_4,
    followups_followupinitial.serum_creatinine_5,
    followups_followupinitial.serum_creatinine_6,
    followups_followupinitial.serum_creatinine_7,
    followups_followupinitial.dialysis_requirement_1,
    followups_followupinitial.dialysis_requirement_2,
    followups_followupinitial.dialysis_requirement_3,
    followups_followupinitial.dialysis_requirement_4,
    followups_followupinitial.dialysis_requirement_5,
    followups_followupinitial.dialysis_requirement_6,
    followups_followupinitial.dialysis_requirement_7,
    followups_followupinitial.dialysis_cause,
    followups_followupinitial.dialysis_cause_other,
    followups_followupinitial.hla_mismatch_a,
    followups_followupinitial.hla_mismatch_b,
    followups_followupinitial.hla_mismatch_dr,
    followups_followupinitial.induction_therapy,
    followups_followupinitial.discharge_date,
    followups_followupinitial.organ_id
  FROM old.followups_followupinitial AS followups_followupinitial;

/*
 TABLE: health_economics_resourcelog
 Note: Appears to be empty!
 Changes to account for:
 - live - added as part of AuditControlModelBase, defaults to 1
 */

INSERT INTO new.health_economics_resourcelog
(
id,
record_locked,
live,
date_given,
date_returned,
notes,
recipient_id
)
  SELECT
    id,
    record_locked,
    1,
    date_given,
    date_returned,
    notes,
    recipient_id
  FROM old.health_economics_resourcelog;

-- HE - Admissions
INSERT INTO new.health_economics_resourcehospitaladmission (
id,
record_locked,
live,
reason,
had_surgery,
days_in_itu,
days_in_hospital,
log_id
)
  SELECT
    id,
    0,
    1,
    reason,
    had_surgery,
    days_in_itu,
    days_in_hospital,
    log_id
  FROM old.health_economics_resourcehospitaladmission;

-- HE - Rehabilitation
INSERT INTO new.health_economics_resourcerehabilitation (
id,
record_locked,
live,
reason,
days_in_hospital,
log_id
)
  SELECT
    id,
    0,
    1,
    reason,
    days_in_hospital,
    log_id
  FROM old.health_economics_resourcerehabilitation;

-- HE - Visits
INSERT INTO new.health_economics_resourcevisit (
id,
record_locked,
live,
type,
log_id
)
  SELECT
    id,
    0,
    1,
    type,
    log_id
  FROM old.health_economics_resourcevisit;

/*
 TABLE: health_economics_qualityoflife
 Changes to account for:
 - live - added as part of AuditControlModelBase, defaults to 1
 */
INSERT INTO new.health_economics_qualityoflife (
id,
record_locked,
live,
date_recorded,
qol_mobility,
qol_selfcare,
qol_usual_activities,
qol_pain,
qol_anxiety,
vas_score,
recipient_id
)
  SELECT
    health_economics_qualityoflife.id,
    health_economics_qualityoflife.record_locked,
    1,
    health_economics_qualityoflife.date_recorded,
    health_economics_qualityoflife.qol_mobility,
    health_economics_qualityoflife.qol_selfcare,
    health_economics_qualityoflife.qol_usual_activities,
    health_economics_qualityoflife.qol_pain,
    health_economics_qualityoflife.qol_anxiety,
    health_economics_qualityoflife.vas_score,
    health_economics_qualityoflife.recipient_id
  FROM old.health_economics_qualityoflife AS health_economics_qualityoflife;

/*
 TABLE: samples_event
 Changes to account for:
 - live - added as part of AuditControlModelBase, defaults to 1
 - worksheet - removed from old model, not in new model
 */
INSERT INTO new.samples_event (
id,
record_locked,
live,
type,
name,
taken_at
)
  SELECT
    id,
    record_locked,
    1,
    type,
    name,
    taken_at
  FROM old.samples_event;

/*
 TABLE: samples_bloodsample
 Changes to account for:
 - live - added as part of AuditControlModelBase, defaults to 1
 */
INSERT INTO new.samples_bloodsample
(
id,
record_locked,
live,
barcode,
collected,
notes,
blood_type,
centrifuged_at,
event_id,
person_id
)
  SELECT
    id,
    record_locked,
    1,
    barcode,
    collected,
    notes,
    blood_type,
    centrifuged_at,
    event_id,
    person_id
  FROM old.samples_bloodsample;

/*
 TABLE: samples_perfusatesample
 Changes to account for:
 - live - added as part of AuditControlModelBase, defaults to 1
 */
INSERT INTO new.samples_perfusatesample (
id,
record_locked,
live,
barcode,
collected,
notes,
centrifuged_at,
event_id,
organ_id
)
  SELECT
    id,
    record_locked,
    1,
    barcode,
    collected,
    notes,
    centrifuged_at,
    event_id,
    organ_id
  FROM old.samples_perfusatesample;

/*
 TABLE: samples_tissuesample
 Changes to account for:
 - live - added as part of AuditControlModelBase, defaults to 1
 */
INSERT INTO new.samples_tissuesample (
id,
record_locked,
live,
barcode,
collected,
notes,
tissue_type,
event_id,
organ_id
)
  SELECT
    id,
    record_locked,
    1,
    barcode,
    collected,
    notes,
    tissue_type,
    event_id,
    organ_id
  FROM old.samples_tissuesample;

-- And urine samples
INSERT INTO new.samples_urinesample (
id,
record_locked,
live,
barcode,
collected,
notes,
centrifuged_at,
event_id,
person_id
)
  SELECT
    id,
    record_locked,
    1,
    barcode,
    collected,
    notes,
    centrifuged_at,
    event_id,
    person_id
  FROM old.samples_urinesample;

/*
 TABLE: reversion_revision
 No changes to note, all references should be consistent
 */
INSERT INTO new.reversion_revision (
  id,
  date_created,
  comment,
  user_id
)
  SELECT
    id,
    date_created,
    comment,
    user_id
  FROM old.reversion_revision;

/*
 TABLE: reversion_version
 No changes to note, all references should be consistent as content_type_ids have not changed.
 */
INSERT INTO new.reversion_version (
  id,
  object_id,
  format,
  serialized_data,
  object_repr,
  content_type_id,
  revision_id,
  db
)
  SELECT
    id,
    object_id,
    format,
    serialized_data,
    object_repr,
    content_type_id,
    revision_id,
    db
  FROM old.reversion_version;




/*
 #######################################################################################################################
 #######################################################################################################################
 #######################################################################################################################

UTILS

SELECT count(id)
FROM old.reversion_version;

SELECT *
FROM old.adverse_event_adverseevent
WHERE contact_id IS NULL;

SELECT *
FROM old.staff_person_staffperson;
INSERT INTO new.TABLE (fieldname1, fieldname2)
  SELECT
    fieldname1,
    fieldname2
  FROM old.TABLE;

SELECT
  lh.id,
  lh.name,
  lh.country
FROM old.locations_hospital AS lh,
  old.compare_organallocation AS oa
WHERE oa.transplant_hospital_id = lh.id
GROUP BY lh.id;


 NB: Be explicit in naming the fields and ordering for all inserts and selects, otherwise some fields are out of order.

 Do not copy:
 * auth_group_permissions  -- populated from migrations
 * auth_permission         -- populated from migrations
 * auth_group              -- populated from migrations
 * django_sessions         -- data temp table for django
 * django_migrations       -- populated by migrations
 * staffperson_staffjob    -- redundant, replaced by groups
 * samples_worksheet       -- redundant, no replacement for data
 * locations_hospital      -- populated from fixture 05_hospitals.json (Issue #210)
 * compare_retrievalteam   -- populated from fixture 05_hospitals.json (Issue #210)
 * perfusion_machine_perfusionfile -- redundant, no replacement for data (was empty)
 * sqlite_sequence

 New Tables
 * adverse_event_category   -- new table; populate from fixture 06_adverseevent_categories.json after staff_person is filled
 * staff_person_groups      -- Do nothing
 * staff_person_user_permissions -- Do nothing

 Overwrite data in:
 * django_site
 * django_content_type      -- populated from migrations. NB: Need to fix reversion references!
 * compare_randomisation    -- no need to bother with fixture, but does need staff_person to be filled first

 For Issue #211 - Remove link to Hospital from Donor
 * Map locations.hospital.name to donor.retrieval_hospital on copy

*/