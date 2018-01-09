/* Import the csv data into the database via commandline */
.mode csv temp_paper_record_import
.import /Users/carl/Documents/NDS/COPE/PaperAudit/NL_BE_cleaned_combined.csv temp_paper_record_import


/* Clean/normalise the data */
UPDATE temp_paper_record_import
SET t_form = LOWER(t_form),
  fu_1 = LOWER(fu_1),
  fu_2 = LOWER(fu_2),
  fu_3 = LOWER(fu_3),
  fu_4 = LOWER(fu_4),
  trial_id = UPPER(trial_id);

/* Data clean up in existing data to allow for merging here */
UPDATE compare_donor SET trial_id = 'WP451025', sequence_number = '25' WHERE id = 137;
UPDATE compare_organ SET trial_id = 'WP451025L' WHERE id = 273;
UPDATE compare_organ SET trial_id = 'WP451025R' WHERE id = 274;

/* Need to have the organ_id for easy mapping with FU and TForms , so add column and add in reference */
ALTER TABLE temp_paper_record_import ADD organ_id INT NULL;

UPDATE temp_paper_record_import
SET organ_id = (
  SELECT compare_organ.id
  FROM compare_organ
  WHERE compare_organ.trial_id = temp_paper_record_import.trial_id
)
WHERE EXISTS(SELECT * FROM compare_organ WHERE compare_organ.trial_id = temp_paper_record_import.trial_id);



/*
######  Update T-Forms from paper record import  ######
x = paper form, or True;
database = database entry, or False;
austria = redundant data, so treat as database;
missing = semi redundant data, but we treat this as blank, and thus None.
 */
UPDATE compare_organ
SET paper_form_was_the_source = (
  Select CASE t_form
      WHEN 'x' THEN 1
      WHEN 'database' THEN 0
      WHEN 'austria' THEN 0
      ELSE NULL
    END
  FROM temp_paper_record_import
  WHERE temp_paper_record_import.organ_id = compare_organ.id
)
WHERE EXISTS(SELECT * FROM temp_paper_record_import WHERE temp_paper_record_import.organ_id = compare_organ.id);



/*
######  Update FU Forms from paper record import  ######
x = paper form, or True;
database = database entry, or False;
half = redundant data, so treat as database;
missing = semi redundant data, but we treat this as blank, and thus None.
n/a = semi redundant data, but we treat this as blank, and thus None.
death = semi redundant data, but we treat this as blank, and thus None.
patient died = semi redundant data, but we treat this as blank, and thus None.
not due = semi redundant data, but we treat this as blank, and thus None.
blank = no answer, so None
*/
UPDATE followups_followupinitial
SET paper_form_was_the_source = (
  Select CASE fu_1
      WHEN 'x' THEN 1
      WHEN 'database' THEN 0
      WHEN 'half' THEN 0
      ELSE NULL
    END
  FROM temp_paper_record_import
  WHERE temp_paper_record_import.organ_id = followups_followupinitial.organ_id
)
WHERE EXISTS(
    SELECT *
    FROM temp_paper_record_import
    WHERE temp_paper_record_import.organ_id = followups_followupinitial.organ_id
);

UPDATE followups_followup3m
SET paper_form_was_the_source = (
  Select CASE fu_2
      WHEN 'x' THEN 1
      WHEN 'database' THEN 0
      WHEN 'half' THEN 0
      ELSE NULL
    END
  FROM temp_paper_record_import
  WHERE temp_paper_record_import.organ_id = followups_followup3m.organ_id
)
WHERE EXISTS(
    SELECT *
    FROM temp_paper_record_import
    WHERE temp_paper_record_import.organ_id = followups_followup3m.organ_id
);

UPDATE followups_followup6m
SET paper_form_was_the_source = (
  Select CASE fu_3
      WHEN 'x' THEN 1
      WHEN 'database' THEN 0
      WHEN 'half' THEN 0
      ELSE NULL
    END
  FROM temp_paper_record_import
  WHERE temp_paper_record_import.organ_id = followups_followup6m.organ_id
)
WHERE EXISTS(
    SELECT *
    FROM temp_paper_record_import
    WHERE temp_paper_record_import.organ_id = followups_followup6m.organ_id
);

UPDATE followups_followup1y
SET paper_form_was_the_source = (
  Select CASE fu_4
      WHEN 'x' THEN 1
      WHEN 'database' THEN 0
      WHEN 'half' THEN 0
      ELSE NULL
    END
  FROM temp_paper_record_import
  WHERE temp_paper_record_import.organ_id = followups_followup1y.organ_id
)
WHERE EXISTS(
    SELECT *
    FROM temp_paper_record_import
    WHERE temp_paper_record_import.organ_id = followups_followup1y.organ_id
);
