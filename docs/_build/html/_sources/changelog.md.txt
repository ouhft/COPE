# Changelog

This is a list of changes made to the website and application for each release.

## v0.8.2 (16 Jun 2017) - Bugfix

## v0.8.1 (31 Mar 2017) - Bugfix
* Issue #262: 
  - Fixes the incorrect date math for `activate_followups_in_window()` 
  - Added the start of a Follow Ups report to help debug this issue
  - Basic Admin updates for Samples app
* Issue #265:
  - New template tag produced to allow progamatic changes to querystring values
  - Template tag used in paginator
  - Sort Columns extracted as an include, to use the `query_transform` tag and simplified calling
  - Modified the main listing templates that had manually implemented sorting
* Issue #251:
  - Cleaned up Administration index page, added some permission checking for displaying of links
  - Refactored the Administration App views into separate files for clarity
  - Checked permissions match between view and index display
  - Fixed broken link in SAE Sites template (the origin for this issue)
* Issue #252 - Fixed ISE from bad reference
* Issue #258 - Add a Key Description to the Combined Pairs report. Added new DMC reports:
  - Adverse Events
  - Serious Events
  - Graft Failures
* Issue #273 - Fixed broken link in S/AE Alert emails
* Issue #260 - ISE: SAE - String index out of range, which was due to missing or invalid email addresses in the data.
* DMC Reports:
  - Added DMC Permanent Impairment Report
  - Added permissions checking for DMC reports
  - Fixed default url for Adverse Events to Detail, rather than update
  - Added a link back to edit the event from Detail
  - Adverse events, new calculation method and totals
  - Serious Adverse events, new calculation method and totals
  - Added categories to Adverse Event report
  - Added categories to Serious Adverse Event report
  - OPEN report versions added for SAE, AE, and Graft Failure reports. Death and Impairment left to do.
  - Index listing permissions refined to allow for Open reports to be viewed by Administrators that are blinded to randomisation
* Enhanced Completed Pairs to show what happens to Non-allocated Eligible Kidneys, and highlighting of pairs without CC data

## v0.8.0 (21 Mar 2017) - People and Permissions

### New Features
* Documentation for COPE DB now published on each server. Production version at [https://cope.nds.ox.ac.uk/docs/]() (and same subdirectory on Test/Staging)
* New Staff tool that allows for easier and effective management of users by the Co-ordinators. Adds extra roles for National and Central Investigators.
* Samples have had their data entry forms overhauled, with Worksheets removed as a concept (and data linkage). There's now a display version for each type of sample as well as the data entry form (Issue [#130](https://github.com/AllyBradley/COPE/issues/130), [#26](https://github.com/AllyBradley/COPE/issues/26), & [#241](https://github.com/AllyBradley/COPE/issues/241))
* Follow Ups are now produced based on a schedule, rather than previous data entry actions (Issue [#140](https://github.com/AllyBradley/COPE/issues/140)). However, a complete Recipient record is needed before this can happen, which means completing most of the Transplant Form.
  - Initial Follow Up to be generated on creation of Recipient record
  - 3M Follow Up to be generated 70 days after Randomisation Date
  - 6M Follow Up to be generated 150 days after Randomisation Date
  - 12M Follow Up to be generated 300 days after Randomisation Date

### Main Changes
* Banner added on Test Site to announce THIS IS THE TEST SYSTEM due to user confusion and errors
* Added extra fields to S/AE reporting (Issue [#170](https://github.com/AllyBradley/COPE/issues/170))
* Amended S/AE report for Statistician (Issue [#188](https://github.com/AllyBradley/COPE/issues/188))
* New reports added for DMC and Co-ordinators (e.g. Completed Pairs, Donor Flowchart, DMC Death Summaries, DMC Secondary Outcomes, etc)
* Removed validation check for a Perfusion Technician to be entered on the Transplant Form (Issue [#192](https://github.com/AllyBradley/COPE/issues/192))
* Implementation of a "soft-delete" function for Administrators (the introduction of LiveField on core models)
* Removed "Worksheets" from Samples as these were not being effectively used and causing data conflicts (Issue [#129](https://github.com/AllyBradley/COPE/issues/129))
* Password change no longer requires admin access (Issue [#160](https://github.com/AllyBradley/COPE/issues/160))
* New permissions added to support Read Only or Geographically bounded access to data (Issue [#167](https://github.com/AllyBradley/COPE/issues/167))
* Cold Storage validation modified (Issue [#146](https://github.com/AllyBradley/COPE/issues/146)) to partly address problems with the data entered so far, however there are outstanding concerns over this topic
* Date of Death as recored by an S/AE form has been moved from being specific to each S/AE record/form and correctly reassigned to the relevant Recipient record. This means that any further S/AE forms will all show and reference the same DoD. (Issue [#163](https://github.com/AllyBradley/COPE/issues/163))
* S/AE Emails send on saving (Issue [#155](https://github.com/AllyBradley/COPE/issues/155))
* The 0.8.0 Release introduced Database Breaking changes, and so there is no automatic migration path from earlier versions to this release. New database setup scripts were generated, and lots of manual effort has been put in to port (and clean) the data between releases.
* Added Categorisation to S/AE events (Issue [#190](https://github.com/AllyBradley/COPE/issues/190))
* Changed the need to link Donor to a specific hospital, and allow freetext records (Issue [#211](https://github.com/AllyBradley/COPE/issues/211))
* Removed Transplant Technicians' ability to add-new/edit Allocation Hospitals (Issue [#212](https://github.com/AllyBradley/COPE/issues/212))
* Restricted Allocation locations on Transplant form to Project Sites + "other" (Issue [#210](https://github.com/AllyBradley/COPE/issues/210))
* Removed duplicate Staff records introduced through a combination of human error and process logic (Issue [#180](https://github.com/AllyBradley/COPE/issues/180))
* Trial ID autocomplete added for S/AE Forms (Issue [#157](https://github.com/AllyBradley/COPE/issues/157))
* Administrators can now edit closed Procurement and Transplant forms (Issue [#168](https://github.com/AllyBradley/COPE/issues/168))

### Minor or Background Changes

* Various library updates for security improvements
* Typos corrected (Issues [#87](https://github.com/AllyBradley/COPE/issues/87) & [#86](https://github.com/AllyBradley/COPE/issues/86))
* Documentation updates
* Re-enabled ReVersion-Compare after updates to library
* GitIgnore tweaked to account for database copies in use during development
* New Groups fixture added (Issue [#208](https://github.com/AllyBradley/COPE/issues/208))
* Moved Admin reports and functions into their own Administration app (from Compare app)
* Added the Staff App (Issue [#208](https://github.com/AllyBradley/COPE/issues/208))
* Changed the core Auth model to use the new Staff model, and the subsequent data migrations and cleanup needed to enable this
* StaffJob profiles have been replaced with Auth.Groups (Issue [#167](https://github.com/AllyBradley/COPE/issues/167))
* Modified the database backup script to compress on backup due to space constraints
* Added script to send an email on server boot and shutdown (Issue [#132](https://github.com/AllyBradley/COPE/issues/132))
* Configuration to support logrotation added (Issue [#135](https://github.com/AllyBradley/COPE/issues/135)) 
* Fixed a logrotate permissions bug (Issue [#138](https://github.com/AllyBradley/COPE/issues/138))
* Added `Organ.was_cold_stored` as per Issue [#166](https://github.com/AllyBradley/COPE/issues/166)
* Changed hardcoded email references to use Staff record details (Issue [#239](https://github.com/AllyBradley/COPE/issues/239))
* Refactoring including: `adverse_event.AdverseEvent` renamed to `adverse_event.event`, `OrganPerson` renamed to `Patient`, and `VersionControlMixin` rebuilt as `AuditControlModelBase`
* Added limited ordering to the S/AE, HE-QoL, and FU listings
* Added `Trial_ID` as a database field, and changed the way the IDs are generated so as to populate this
* Added `allocated_by` to Randomisation to help clear up who performed the randomisation (Issue [#223](https://github.com/AllyBradley/COPE/issues/223))
* Fixed select Perfusion Technician (Theatre contact, etc) listing missing names for display bug (Issue [#226](https://github.com/AllyBradley/COPE/issues/226))
* `created_by` removed from base model class as mostly unused and creating surplus data linkage redundancies
* Basic admin supports triggering a password change for `Staff.Person` records (Issue [#229](https://github.com/AllyBradley/COPE/issues/229))
* Quality of Life model form now loads the DateTime picker tool (Issue [#109](https://github.com/AllyBradley/COPE/issues/109))
* Fixed Donor not proceeding - appearing without cause bug (Issue [#221](https://github.com/AllyBradley/COPE/issues/221))
* Linked fixed on repository ReadMe (Issue [#171](https://github.com/AllyBradley/COPE/issues/171))



## v0.6.4 (14 Oct 2016) - Bugfix
* Security updates to core libraries
* Added Admin report for SAE Summary (Issue [#126](https://github.com/AllyBradley/COPE/issues/126))
* Added Admin report for Transplantation Sites (Issue [#125](https://github.com/AllyBradley/COPE/issues/125))
* Added Admin report for Procurement Pairs (Issue [#123](https://github.com/AllyBradley/COPE/issues/123))

## v0.6.3 (3 Aug 2016) - Bugfix
* Fixed an error after randomisation bug (Issue [#120](https://github.com/AllyBradley/COPE/issues/120))
* Increased loading speed for Samples (Issue [#77](https://github.com/AllyBradley/COPE/issues/77))
* Added Admin link to cases (Issue [#117](https://github.com/AllyBradley/COPE/issues/117))

## v0.6.2 (5 July 2016) - Bugfix
* Labelling change for Transplantation form (Issue [#58](https://github.com/AllyBradley/COPE/issues/58))

## v0.6.1 (1 July 2016) - Bugfix
* Resolved Internal Server Errors due to an issue with date_of_birth (Issue [#116](https://github.com/AllyBradley/COPE/issues/116))

## v0.6.0 (21 June 2016) - The Follow Up edition
### New Features
* Follow Up forms added for Initial, 3 Month, 6 Month, and 1 Year data collection based on the current paper forms (Issue #79)
* Health Economics forms added for Quality of Life data collection (Issue #97)
* Initial reports added for DMC/Statistical analysis (Issues #90, #105, & #110)

### Main changes
* Removed validation check for Perfusion Stopped on the Procurement form at close (Issue #104)
* Initial work done to change several sections where slow performance was affecting usability. More remains to be done on this (Issues #82, & #77)
* User Manual updated to reflect new sections and previous updates

### Minor (and background) changes
* Samples label correction for Perfusate (Issues #39)
* Admin rights restored along with UI shortcuts (Issue #91)
* Ported to Python 3.x from 2.x, for improved support with unicode and csv libraries (Issue #111)
* Various library updates for security improvements


## v0.5.1 (1 May 2016) - Bugfix 
* Internal documentation comments were appearing in the UI. These have been modifed or hidden (Issue #89)
* Online randomisation was failing, due to an incorrect validation check on an internal sequence number. (Issue #88)
* Question RE22 on the recipient form (Tape over regulator broken?) has not been saving. Incorrect field definition has been fixed. (Issue #81)

## v0.5.0 (29 Apr 2016) - Adversely Related
### New Features
* Adverse Event reporting added (Issue #80)
* Offline Randomisation Listings for Administrators. Shows available randomisation codes.
* Version Control implemented. All changes to site data are now preserved for audit and security purposes. (Issue #19)
* Changelog, and online User Manual added to the website (Issues #36 and #31)
* Transplantation Form now shows a checklist of eligibility criteria (Issue #42)
* Transplantation Form validation is now implemented using an expanded list of criteria as requested (Issue #45)

### Main changes
* Procurement form: 
 * Record information about cases that are not randomised (Issue #23)
 * Recipient randomisation display updating fixed (Issue #33)
* Transplantation form:
 * Changed the start process to require confirmation of allocation (Issue #44)
 * Added process to close form if allocation set to a non-trial location (Issue #41)
 * Ineligible cases are now shown a warning message to confirm answers, before closing the case on the subsequent save and confirm
 * General notes field added on form closing (Issue #46)
 * List of closed cases added for Administrators
* Expanded list of countries to include all of Europe (Locations app), rather than just the trial countries (Issue #25)
* Follow Up forms:
 * Urine Creatinine has been removed (Issue #49)
 * "Are they dead" question added to each stage of the forms (Issue #40)
* Security improvments made with:
 * Password rules implemented
 * Access controls tightened on all areas
 * Templates and menus now only show links for accessible (to you) areas
* Site navigation improved with: 
 * Breadcrumbs for all areas
 * Missing pages created for sections
 * Menu links updated
* Server emails have been enabled, allowing password reset requests to be done by all users, and sets up the option for having automated alerts in the future

### Minor (and background) changes
* Person's weight now recorded as a float, rather than an int (Issue #22)
* Fixed a middleware issue that caused problems with the debug language setting on the Test server (Issue #54)
* Help text added to the Is transplantable question (Issue #30)
* Fixed a bug which stopped hospitals with an apostrophe in their name from being selected (Issue #55)
* Date of Death is now back as a necessary OrganPerson attribute; This had implications for various date calculation methods, and save methods (such as saving when Donor death diagnosed is set). This was prompted again by Issue #40
* Age vs DoB validation message now shows the range of valid options to the user on error
* Template change to display version number from package details rather than hardcoded
* StaffPerson and FollowUp apps now use the common VersionControlModel class as their base
* Admin forms updated for new models and changes
* Improved the configuration setup to allow for easier deployment to multiple environments (location.env)
* Fixed a server error related to configuration for deployment on test server (wsgi.py)
* Split the compare:forms into their own package due to size growth
* Updated the core VersionControlModel class to handle saving for inherited classes
* New managers added to the Organ class to handle common queries
* Added `not_allocated_reason` to replace the redundant `allocated` attribute
* Two permission checking utilities added to determine if member of a list of groups, or has a specifed Job
* Many models updated to replace method calls with more appropriate property flags, and subsequent related corrections
* Organ now has an `explain_is_allocated` method that attempts to work out the allocated status of itself, and put it into words
* Foreign Key Widget now shows error messages when appropriate (previously they were not displayed)
* Developer documentation updated and expanded
* Repeated updates of third party libraries incorporated
 * Major version change to AutocompleteLight library (2 to 3) meant various changes, and several improvements to the functionality and appearance of type ahead select fields
* Server patching and updating done frequently

## v0.4.6 (2 Mar 2016) - Bugfix
* Changes made to allow use of local timezones in the website rather than just UST/GMT (Issue #34)
* Validation rules for calculating DoB corrected (Issue #35)

## v0.4.5 (18 Jan 2016) - Initial release
* First production release of the database, with focus on Procurement, Samples, Transplant, Locations, People, and related.
* User Documentation created (User Manual) and distributed by PDF
* Basic validation in place for the two forms

