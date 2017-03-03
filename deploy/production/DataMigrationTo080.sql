/*
 Script to migrate data safely from a 0.6.4 schema to the new 0.8.0 schema
 */
/* BLOCK 1 */
DETACH DATABASE old;
DETACH DATABASE new;
ATTACH DATABASE 'file:///Users/carl/Projects/py3_cope/cope_repo/old.db.sqlite3?mode=ro' AS old;
ATTACH DATABASE 'file:///Users/carl/Projects/py3_cope/cope_repo/db.sqlite3' AS new;

SELECT * FROM old.django_site;
DELETE FROM new.django_site;
UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='new.django_site';
INSERT INTO new.django_site SELECT * FROM old.django_site;
SELECT * FROM new.django_site;

DELETE FROM new.django_content_type;
UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='new.django_content_type';
INSERT INTO new.django_content_type SELECT * FROM old.django_content_type;
INSERT INTO new.django_content_type (app_label, model) VALUES ('adverse_event','category');
UPDATE new.django_content_type SET model='event' WHERE id=29;
UPDATE new.django_content_type SET model='person', app_label='staff' WHERE id=3;
DELETE FROM new.django_content_type WHERE id IN (9,10,13);


INSERT INTO new.staff_person (
  id, password, last_login, is_superuser, username, first_name, last_name, email, is_staff, is_active, date_joined
)
  SELECT
  id, password, last_login, is_superuser, username, first_name, last_name, email, is_staff, is_active, date_joined
  FROM old.auth_user;
UPDATE new.staff_person SET is_active=0 WHERE id=105;

INSERT INTO new.compare_randomisation SELECT * FROM old.compare_randomisation;

INSERT INTO new.django_admin_log SELECT * FROM old.django_admin_log;
DELETE FROM new.django_admin_log WHERE content_type_id IN (9,10,13);

INSERT INTO new.perfusion_machine_perfusionmachine SELECT * FROM old.perfusion_machine_perfusionmachine;


/* BLOCK 2 */
INSERT INTO new.compare_organperson
(
  id
  , created_on
  , version
  , record_locked
  , live
  , number
  , date_of_birth
  , date_of_birth_unknown
  , date_of_death
  , date_of_death_unknown
  , gender
  , weight
  , height
  , ethnicity
  , blood_group
  , created_by_id
)
  SELECT
    id,
    created_on,
    version,
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
    blood_group,
    created_by_id
  FROM old.compare_organperson;




/* Can't copy donor until staff_person has been mapped to new users */
/* Only practical solution I can see here is to manually go through all old.Staffperson records, and create
user accounts for those who do not have them. Also manually update the auth_user record with any salient differences
in names or email. Can then use the user_id in old.staffperson to replace technician_ids, etc.
Also means we can do an update to pull in telephone numbers where available, as well as based_at info from linked profiles -- this has to be done as a separate update because of the need to load fixture data for locations, which
rely on user records existing first.

348 Staff Person records adjusted
auth_user records created for all valid StaffPerson records
Locations updated to display NOT PROJECT SITE
... can return to the move and merge process on MONDAY
 */



INSERT INTO new.compare_donor (
  id, created_on, version, record_locked, sequence_number, multiple_recipients
  ,not_randomised_because
  ,not_randomised_because_other
  ,procurement_form_completed
  ,admin_notes
  ,call_received
  ,call_received_unknown
  ,retrieval_hospital
  ,scheduled_start
  ,scheduled_start_unknown
  ,technician_arrival
  ,technician_arrival_unknown
  ,ice_boxes_filled
  ,ice_boxes_filled_unknown
  ,depart_perfusion_centre
  ,depart_perfusion_centre_unknown
  ,arrival_at_donor_hospital
  ,arrival_at_donor_hospital_unknown
  ,age
  ,date_of_admission
  ,date_of_admission_unknown
  ,admitted_to_itu
  ,date_admitted_to_itu
  ,date_admitted_to_itu_unknown
  ,date_of_procurement
  ,other_organs_procured
  ,other_organs_lungs
  ,other_organs_pancreas
  ,other_organs_liver
  ,other_organs_tissue
  ,diagnosis
  ,diagnosis_other
  ,diabetes_melitus
  ,alcohol_abuse
  ,cardiac_arrest
  ,systolic_blood_pressure
  ,diastolic_blood_pressure
  ,diuresis_last_day
  ,diuresis_last_day_unknown
  ,diuresis_last_hour
  ,diuresis_last_hour_unknown
  ,dopamine
  ,dobutamine
  ,nor_adrenaline
  ,vasopressine
  ,other_medication_details
  ,last_creatinine
  ,last_creatinine_unit
  ,max_creatinine
  ,max_creatinine_unit
  ,life_support_withdrawal
  ,systolic_pressure_low
  ,systolic_pressure_low_unknown
  ,o2_saturation
  ,o2_saturation_unknown
  ,circulatory_arrest
  ,circulatory_arrest_unknown
  ,length_of_no_touch
  ,death_diagnosed
  ,perfusion_started
  ,perfusion_started_unknown
  ,systemic_flush_used
  ,systemic_flush_used_other
  ,systemic_flush_volume_used
  ,heparin
  ,created_by_id
  ,person_id
  ,retrieval_team_id
  )
  SELECT
  odo.id
  ,odo.created_on
  ,odo.version
  ,odo.record_locked
  ,odo.sequence_number
  ,odo.multiple_recipients
  ,odo.not_randomised_because
  ,odo.not_randomised_because_other
  ,odo.procurement_form_completed
  ,odo.admin_notes
  ,odo.call_received
  ,odo.call_received_unknown
  ,oho.name
  ,odo.scheduled_start
  ,odo.scheduled_start_unknown
  ,odo.technician_arrival
  ,odo.technician_arrival_unknown
  ,odo.ice_boxes_filled
  ,odo.ice_boxes_filled_unknown
  ,odo.depart_perfusion_centre
  ,odo.depart_perfusion_centre_unknown
  ,odo.arrival_at_donor_hospital
  ,odo.arrival_at_donor_hospital_unknown
  ,odo.age
  ,odo.date_of_admission
  ,odo.date_of_admission_unknown
  ,odo.admitted_to_itu
  ,odo.date_admitted_to_itu
  ,odo.date_admitted_to_itu_unknown
  ,odo.date_of_procurement
  ,odo.other_organs_procured
  ,odo.other_organs_lungs
  ,odo.other_organs_pancreas
  ,odo.other_organs_liver
  ,odo.other_organs_tissue
  ,odo.diagnosis
  ,odo.diagnosis_other
  ,odo.diabetes_melitus
  ,odo.alcohol_abuse
  ,odo.cardiac_arrest
  ,odo.systolic_blood_pressure
  ,odo.diastolic_blood_pressure
  ,odo.diuresis_last_day
  ,odo.diuresis_last_day_unknown
  ,odo.diuresis_last_hour
  ,odo.diuresis_last_hour_unknown
  ,odo.dopamine
  ,odo.dobutamine
  ,odo.nor_adrenaline
  ,odo.vasopressine
  ,odo.other_medication_details
  ,odo.last_creatinine
  ,odo.last_creatinine_unit
  ,odo.max_creatinine
  ,odo.max_creatinine_unit
  ,odo.life_support_withdrawal
  ,odo.systolic_pressure_low
  ,odo.systolic_pressure_low_unknown
  ,odo.o2_saturation
  ,odo.o2_saturation_unknown
  ,odo.circulatory_arrest
  ,odo.circulatory_arrest_unknown
  ,odo.length_of_no_touch
  ,odo.death_diagnosed
  ,odo.perfusion_started
  ,odo.perfusion_started_unknown
  ,odo.systemic_flush_used
  ,odo.systemic_flush_used_other
  ,odo.systemic_flush_volume_used
  ,odo.heparin
  ,odo.created_by_id
  ,odo.person_id
  ,odo.retrieval_team_id
  FROM old.compare_donor as odo,
    old.locations_hospital as oho
  WHERE odo.retrieval_hospital_id=oho.id
;
/* NB:
  ,perfusion_technician_id << NEEDS REMAPPING
  ,transplant_coordinator_id << NEEDS REMAPPING
*/

SELECT * FROM old.compare_organperson;
INSERT INTO new.TABLE SELECT * FROM old.TABLE;
INSERT INTO new.TABLE(fieldname1, fieldname2) SELECT fieldname1, fieldname2 FROM old.TABLE;
/*

 Do not copy:
 * auth_group_permissions  -- populated from fixture 01_permissions.json
 * auth_permission         -- populated by migrations
 * auth_group              -- populated from fixture 01_permissions.json
 * django_sessions         -- data temp table for django
 * django_migrations       -- populated by migrations
 * staffperson_staffjob    -- redundant, replaced by groups
 * samples_worksheet       -- redundant, no replacement for data
 * locations_hospital      -- populated from fixture 03_hospitals.json (Issue #210)
 * compare_retrievalteam   -- populated from fixture 03_hospitals.json (Issue #210)
 * perfusion_machine_perfusionfile -- nothing in the old data

 New Tables
 * adverse_event_category   -- new table; populate from fixture 11_adverseevent_categories.json after staff_person is filled
 * staff_person_groups      -- Do nothing
 * staff_person_user_permissions -- Do nothing

 Overwrite data in:
 * django_site
 * django_content_type
   * Amend records in here as changes are made elsewhere in the migration process.
     * e.g. Amend adverse_event.adverseevent to event
   * Remove redundant types (e.g. samples.worksheet)
   * Add adverse_event.category
 * compare_randomisation - no need to bother with fixture, but does need staff_person to be filled first

 Order of processing
 - create new database `pm migrate`
 - load fixture `pm loaddata config/fixtures/01_permissions.json`
 - Execute BLOCK 1
 - load fixture `pm loaddata config/fixtures/10_hospitals.json`
 - load fixture `pm loaddata config/fixtures/11_adverseevent_categories.json`
 - Execute BLOCK 2


 TBD
 * adverse_event_event  (was adverse_event_adverseevent)
 *
 * compare_organ
 * compare_organallocation
 * compare_organperson
 * compare_procurementresource
 * compare_recipient
 * followups_followup1y
 * followups_followup3m
 * followups_followup6m
 * followups_followupinitial
 * health_economics_qualityoflife
 * health_economics_resourcehospital
 * health_economics_resourcelog
 * health_economics_resourcerehabilitation
 * health_economics_resourcevisit
 *
 *
 * reversion_revision
 * reversion_version
 * samples_bloodsample
 * samples_event
 * samples_perfusatesample
 * samples_tissuesample
 * samples_urinesample
 * sqlite_sequence
 * staff_person


 For Issue #211 - Remove link to Hospital from Donor
 * Map locations.hospital.name to donor.retrieval_hospital on copy

 */