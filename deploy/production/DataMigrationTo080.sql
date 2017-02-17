/*
 Script to migrate data safely from a 0.6.4 schema to the new 0.8.0 schema
 */
ATTACH DATABASE 'file:///Users/carl/Projects/py3_cope/cope_repo/old.db.sqlite3?mode=ro' AS old;
ATTACH DATABASE 'file:///Users/carl/Projects/py3_cope/cope_repo/db.sqlite3' AS new;

SELECT
  *
FROM new.django_content_type
;
SELECT
  *
FROM old.django_site
;
SELECT
  *,
  count(id) as c
FROM old.auth_user
GROUP BY first_name, last_name
HAVING c > 1
;
SELECT
  *
FROM old.auth_user
WHERE first_name = 'Fleur'

/*
 Order of processing

 Do not copy:
 * auth_group_permissions  -- populated from fixture 01_permissions.json
 * auth_permission         -- populated by migrations
 * auth_group              -- populated from fixture 01_permissions.json
 * django_sessions         -- data temp table for django
 * django_migrations       -- populated by migrations
 * staffperson_staffjob    -- redundant, replaced by groups
 * samples_worksheet       -- redundant, no replacement for data
 * locations_hospital      -- populated from fixture 03_hospitals.json (Issue #210)

 Overwrite data in:
 * django_site
 * django_content_type
   * Amend records in here as changes are made elsewhere in the migration process.
   * Add adverse_event.category record
   * Remove redundant types (e.g. samples.worksheet)
   * Amend adverse_event.adverseevent to event
 * compare_randomisation - no need to bother with fixture, but does need staff_person to be filled first


 New Table
 * adverse_event_category   -- new table; populate from fixture 11_adverseevent_categories.json after staff_person is filled
 * staff_person_groups      -- Do nothing
 * staff_person_user_permissions


 TBD
 * adverse_event_event  (was adverse_event_adverseevent)
 * compare_donor
 * compare_organ
 * compare_organallocation
 * compare_organperson
 * compare_procurementresource
 * compare_recipient
 * compare_retrievalteam
 * django_admin_log
 * followups_followup1y
 * followups_followup3m
 * followups_followup6m
 * followups_followupinitial
 * health_economics_qualityoflife
 * health_economics_resourcehospital
 * health_economics_resourcelog
 * health_economics_resourcerehabilitation
 * health_economics_resourcevisit
 * perfusion_machine_perfusionfile
 * perfusion_machine_perfusionmachine
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