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



 For Issue #211 - Remove link to Hospital from Donor
 * Map locations.hospital.name to donor.retrieval_hospital on copy

 */