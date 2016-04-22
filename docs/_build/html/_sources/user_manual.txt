# COPE DB User Manual

Welcome to the COPE DB Online Trials System. This guide provides an overview of what to expect as a user.

* Written by: *Carl Marshall*
* For DB version: *0.4.5*
* Last updated: *18th Jan 2016*

## Overview
By visiting the system at [https://cope.nds.ox.ac.uk/]() you should see a page similar to the one below:

![Screenshot of the home page](static/docs/user_manual/screen_index.png)

A few key areas are:

* **The Navbar** - in blue, at the top of the screen 
* **Page title** - largest text just under the Navbar
* **Page trail** - Otherwise called Breadcrumbs, this should help you locate where in the system you are presently viewing
* **Footer** - At the bottom of the page, under a pale grey line are links to the supporting organisations and information about the current system version
* **Page Content** - is everything between the Footer and the Page Trail

The above image is of the Home page, when logged out. To access most sections of the system you will need a username and password (supplied by the COPE Admin team). 

> **Please ensure you keep your username and password details secure, and do not share them with anyone else**. All system actions are recorded and logged against the relevant user account for auditing purposes and you will be held to account for any actions taken with your user account.

### Logins and Passwords

Clicking on the Login button on the NavBar or any link that goes to a secure page will take you to the *Login page*, where you will be asked for your username and password. If you have forgotten your password, you an click on the `Lost Password?` link under the username text input, and you will be taken to a *Password Reset page*. Enter your registered email address (if you're not sure which one that is, please contact the COPE Admin team) to have a password reset link emailed to you. Once you have that email, clicking on the link will take you back to the website and prompt you to enter a new password twice.

Once you are logged into the system, the Navbar will update to something similar to the image below:

![Navbar with user menu on display](static/docs/user_manual/screen_index_loggedin.png)

In the upper right corner of the page, on the Navbar you should now see some new links and menus. Working from the right hand edge to the left, we have:

* **Logout** - the "power on/off" icon is to securely log you out of the system. You should use this anytime you leave your computer or the website.
* **Locale** - the "globe" icon is to allow you to select your current location and will update the system to use terminology appropriate to your country
* **User** - the "person outline" icon is the User menu and shows some basic information about your account, and a link to Change Password
* **WP4 Menu** - The text "WP4: Compare" has a small menu linking to the various sections of the website, we'll cover more of this in a moment.
* **Home** - will take you back to the system home page from earlier (though still logged in).

### Form Widgets / Inputs

There are a few conventions to be aware of that are used to help you collect accurate data easily as possible.

#### Questions with followups
Some questions will have additional questions related to the answer given; You should be aware of these causing the form to add questions (sliding into view) directly below the question you've just answered. 

#### Date & Time questions
These can be recognised by the grey "calendar" icon to the right of the text entry, and placeholder text similar to `DD-MM-YYYY` (for dates) or `DD-MM-YYYY HH:MM` (for dates and times). You can type dates into these fields directly, or you can click on the icon to the right to get a calendar popup widget displayed, such as:

![Examples of the Date and Time picker](static/docs/user_manual/screen_procurement_form_datetime.png)

In the above image we can see 5 examples.

* **A:** Is the default display, allowing you to select the date from the month shown at the top of the small window. Clicking on a number will change the date text to reflect that choice.
* **B:** At the top of the window in A, we see the Month and Year (January 2016). Clicking on either of the arrows to each side of that will move forward or backwards in time in monthly increments. Clicking on the text itself will take you to image B, which shows you the month in the displayed year. Clicking on a month will take you to the days in that particular month to select from.
* **C:** Clicking again on the date at the top of B will take you to a list of years within that decade. Clicking on the text at the top again will take you to a decade selection list shown in image C. Clicking on the options will drill down till you get back to the days again.
* **D:** At the bottom of the date selectors, when a time is also required, there will be a clock icon displayed. Clicking on that will result in the time wheels being displayed in example D. Using the arrows will advance or decrease the hours and minutes (left and right numbers, respectively).
* **E:** Clicking on one of the time numbers in D will result in something like example E (for minutes) allowing you to get closer to your intended time with fewer clicks. Finally, clicking on the calendar icon at the top of the time selectors will return you to the date picking options.

You will need to click off of the calendar area to hide the display and leave just the selected date behind.

#### Not Known

There are questions that we know will result in some headscratching or examples of where local processes do not collect something we are asking for. In these cases, we want you to mark these special fields with Not Known. Some have the answer as Unknown in the list, and you can select that when appropriate. Others will need you to identify them by the Circle with diagonal bar across it (see the date time example images above), to the left of the input fields, and to click on that icon. It will disable the data entry for that question and mark it as Not Known.

To change your mind, click it again, and the field will be re-enabled for use.

#### People and Locations

There are a range of questions where we want to know about either people, or locations (i.e. hospitals). In these cases we may not know about them before you start data entry, so we will want you to add some extra information to help complete the record.

Apart from the question label as the first clue, you can identify these fields by the greyed out appearance (so that you can't type directly into them), and the magnifying glass icon to the right hand side. Clicking on the magnifying glass will bring up another window with the currently available option in.

![Manage Staff popup showing existing staff](static/docs/user_manual/screen_procurement_form_manage_staff_list.png)

Using the example of "Name of the SN-OD", this is the list of people known to the system so far. If the correct answer is one of these people, clicking on them will close the window and put the answer in the form. If the answer is someone else, then clicking on Add Person will let us create a new staff member.

![Manage Staff popup showing add new person form](static/docs/user_manual/screen_procurement_form_manage_staff_form.png)

To complete this form (shown above) we need to know their first name and last names as a minimum. However, since this is to aide contacting people in case of followups, we really want to know more, like where they are based (pick an existing location - if one doesn't exist, you'll have to create it via another question first), their telephone number, and an email address.

Whilst it is possible to edit the contact information of existing people, please do so with care, as this information may be critical for other cases also.

![](static/docs/user_manual/screen_procurement_form_manage_location_list1.png)

When dealing with either locations or people, the list may grow to be larger than the available screenspace. In which case it will fill from top to bottom as shown above (with Manage Hospitals), and you can scroll the screen to find the correct entry, or to the bottom to find the Add Hospital Option.

![](static/docs/user_manual/screen_procurement_form_manage_location_list2.png)

Adding a new location is a little easier than a person, and only requires two bits of information at this stage: Name of the location, and the country it is based in.

![](static/docs/user_manual/screen_procurement_form_manage_location_form.png)

As with both forms, clicking Save and Use will save the data (making it available for other cases) and use the result to populate the form field.

#### Searchable / Typeahead fields

There are a limited number of fields where you can find an answer by typing part of the answer into a field, and then selecting from the list of matching results that appears. You do need to click on a valid result for this field to be completed, simply typing the answer is not sufficient. If you can't find the answer you're looking for, please contact COPE Admin. If you want to change the answer, you will see a small grey circle with a cross in next to the answer - click that and it will return to being an empty field to type and search again in.

#### Saving data

**Save early, and often.** The forms are all setup so that you can enter incomplete data and still save the results, allowing you to come back to your data entry again and again. However, if you make a mistake during entry that the system can detect, you will need to correct that error before it will save any of your recent changes.

Upon saving you should see a message such as:

![](static/docs/user_manual/screen_procurement_form_save_success.png)

If you have made a mistake, then you will see errors highlighted in a variety of ways (dependant upon the type of mistake).

![](static/docs/user_manual/screen_procurement_form_save_failed.png)

There will be the general error message at the top of the page content area (in this example counting one error).

![](static/docs/user_manual/screen_procurement_form_save_failed_dob.png)

When the answer relates to a specific field, it may be highlighted directly with advice given below it (such as in the Date of Birth example above).

![](static/docs/user_manual/screen_procurement_form_save_failed_extrainfo.png)

And it is possible to have multiple mistakes highlighted on save, in which case you may see an error count in the tabs affected, as well as help messages above the forms.

Try and correct all the errors, and then save again. It is possible to have recurring (though perhaps different) errors, so don't stop correcting until you see the green successful save message.

## WP4: Compare

The system is currently focussed on the WP4 trial. There are two key sections: Procurement and Transplantation

### Procurement

![Procurement screenhot showing an emtpy listing](static/docs/user_manual/screen_procurement_empty.png)

When you first start and click on Procurement (*Procurement Files* on the *Home* page, or `WP4:Compare -> Procurement` in the Navbar) you are likely to see an empty screen similar to above. On the left would be a list of cases you are currently working on (Open Cases). On the right is a small form to start off a New Case.

#### New Cases
To start a new case (i.e. to collect Donor and Organ details during retrieval) you need the following information as a minimum:

* **Name of the Retrieval Team**
* **Name of the MTO/Transplant Technician** - This should default to your name if you are registered as a Perfusion Technician on the system
* **Age of the Donor** - remember 50 years old or more
* **Gender of the Donor**

Additionally, to allow for all eventualities, we need to account for when things go wrong with the data recording. We anticipate the following scenarios:

1. Data is entered as it is collected, and saved frequently. This presumes you have internet connectivity on whichever device you are using for data collection.
2. If you have connectivity issues, then the expectation is that you will take notes on the procedure (perhaps using the backup Paper form as a template) for you to enter into the system when connectivity returns. However, if you need to Randomise and can't connect to the system, you need to contact your nearest co-ordinator.

The co-ordinator will do one of two things: 

* Assuming they can access the system, they can enter the basic information needed to randomise based on feedback from you. The case should then appear on your list of Open Cases when you next get online access and you can resume data entry.
* If the system is offline (such as for maintenance), then they will have access to a small list of values to allow them to randomise the case offline. Upon consulting this list, they will give you an Offline Case ID (three digit number), which you (or they, presuming they have your notes) can use upon regaining access to the system to link up with the correct randomisation record.

**In short:** Work *Online* throughout. If no connection, contact co-ordinator. Use paper notes to record details that you can't get into the system immediately, but resume online once you can.

Once you have entered the basic information, you can click `Start new case` to move onto the main data entry form

#### Existing cases

If you have a case currently on the go as a Technician, you should see it displayed in your Open Cases listing. Clicking on the Case ID link (in blue in the left hand column) will let you resume editing the case.

#### Editing a Case

In this example, we have started a case as *Example Technician 1*, with the retrieval team from *Royal London Hospital, UK*. The donor is *Female*, and *54 years old*. We have confirmed that we are working *Online* presently.

![Screenshot of Procurement form for Case DO026](static/docs/user_manual/screen_procurement_form_case26_1.png)

Four key areas to point out in this initial screen view of the case:

* **Page Title** - states Procurement (as previous page), but also shows the Case identifier. Until a case is randomised, it has a system assigned ID for reference purposes. In this case, the indentifier is *DO026*
* **Eligibility Criteria** - Under the Page trail is the list of eligibility criteria for this trial. We trust you to only start recording cases that are *DCDIII*, but we will confirm things like age (50+), whether the kidneys have been deemed transplantable, and if there are two separate recipients. More on this below.
* **Action Bar** - attached to the bottom of the screen is a light grey bar with (currently) on button on it at the right hand side. Be default that button will say `Save`, however, it will change depending on the state of the form (again, see details below).
* **Data collection areas** - In the main page content area under the eligibility criteria, there are a range of tabs and data entry fields. This is where the majority of the information and interaction will happen.

Of the data collection areas, there are four tabs containing sets of questions:

* **Left Kidney** - all about one of the two organs we're transplanting
* **Donor** - core information about the donor of the organ(s)
* **Right Kidney** - the other organ
* **Samples** - information specific to the samples being taken. Varies depending on your region (for example, UK Donor's don't record any sample information)

**You can enter data in any order you wish, and you can save the form at any time to ensure data is not lost.** Answers are partially validated on each save, and you need to clear all errors to complete a save, otherwise changes will be lost. When data entry for the case is ready to complete and be signed off, there will be extra checks made on the data to help identify any recurring issues with data quality.

##### Donor data

On the donor tab we have 6 subsections, and an extra question related to data entry completion. These areas are:

* Patient Description - generic information about the donor as a patient
* Donor Details - extra data related to donors specifically
* Procedure Data - records of the technician's actions and logistics
* Donor Pre-op Data - much like it says, information from prior to extraction
* Donor Procedure - data from during the extraction
* Lab Results - local records of creatinine

##### Organ data

The Left and Right Kidney tabs are essentially identical (apart from their colouring), and can be summarised as thus:

* Inspection - recording information related to the condition of the kidney. Note, it is possible to change your answers as more information comes to light (see randomisation notes below for example)
* Preset Data - this is data set in the system, and notably, displays the results of the randomisation
* Perfusion Data - details relating to how the machine and setup works out
* Resources Used - this is an audit of materials and consumables used for this organ. Please complete this regardless of the outcome of the kidney

**Samples** will be covered later in this manual.

#### When all is said and done

If you feel you have completed all the information you can gather for the case, the last step is to select Form Completed -> Yes, and then click on the updated Save and Close button on the Action Bar.

![](static/docs/user_manual/screen_procurement_form_save_and_close.png)

Closing the form (assuming no errors are detected) will lock the form from further editing by non-Admin users. If you do discover further information or errors, please contact the COPE Admin team.

### Randomisation

A key function of this system is to tell you how to set up the transport of the kidneys. The checklist at the top of the Procurement form will give you a summary of the criteria that need to be passed. Clicking on the small circles with arrows in next to one of the last three items will highlight the relevant question in the form for you to answer.

For example, clicking on `Left Kidney transplantable`, will result in...

![](static/docs/user_manual/screen_procurement_form_randomise_leftkidney.png)

... the Left Kidney tab being selected, and the Transplantable? question being highlighted with orange pulses. Similarly, clicking on `Two separate recipients` will result in...

![](static/docs/user_manual/screen_procurement_form_randomise_recipients.png)

... the Donor tab being selected, and the Multiple Recipients question at the bottom of the Procedure Data group being highlighted with orange pulses.

Selecting the relevant answers on these questions will result in the checklist row turning green (and the thumb down icon on the right turning into a thumb up), and if all five thumbs are up, the Save button on the Action Bar will change to Save and Randomise.

![](static/docs/user_manual/screen_procurement_form_save_and_randomise.png)

**Note:** It is still possible for the save to be stalled by errors in the form, so please check the feedback messages after pressing the button!

After a successful save, you should now see messsages and changes such as the following:

![](static/docs/user_manual/screen_procurement_form_save_and_randomise_success.png)

* Form has been successfully saved
* This case has been randomised! Preservation results: Left=HMP and Right=HMP O2
* The checklist has collapsed down to a light blue bar - you can click on it to expand the list again
* On the Action Bar, the Save button has returned to Save only
* The Trial ID has been updated in the Page Title (WP421001 in this example)
* On the Kidney tabs, the Preset Data field for Preservation will have been updated with the randomisation results

#### If the facts change...

Understandably during the course of the procedure, and early positive inspection leading to a timely randomisation (allowing the Technician to set up the machines), can be overriden by later events. Back table inspection may find serious damage or other issues with the kidney, and the surgeon can now deem the organs to not be transplantable.

We want you to record these changes of data as they happen. Even if you've done a randomisation, you can subsequently declare the organ not transplantable (remember to give a reason in the followup question), and then save the form again. Similarly with the recipient data - it may be unknown at the start when you need to randomise, but later it turns out that there are not two separate recipients. Again, save the changes and update the relevant notes.

### Transplantation

![](static/docs/user_manual/screen_transplantation_empty.png)

Following procurement of a kidney, the next stage of recording is picked up by the Transplant Technician assigned to the Recipient location. Organs that have been procured, but not yet assigned to recipients will appear on the right side of the Tranplantation listing screen (see above), and can be identified by their Trial ID. Cases that you are currently working on will appear in the listing on the left hand side of the Page Content area.

Let's look at the next stage of the Left Kidney that was procured earlier in this manual. Clicking on case WP421001L will allow us to allocate the kidney to a location.

![](static/docs/user_manual/screen_transplantation_allocation_1.png)

Again, if you are a Technician, you will see your name appear as the answer to Name of TT Attending under Allocation Round 1. This can be changed for another technician by following the instructions for a Typeahead Field.

The key activity here is to track the allocation for the organ, and any subsequent changes to that allocation. Underneath the Allocation Round 1 box you can see Recipient with a note directing you to answer the question about re-allocation (right hand column of the Allocation set). 

Answering Yes and clicking Save will result in another Allocation Round appearing, as shown below:

![](static/docs/user_manual/screen_transplantation_allocation_2.png)

Once there are no more re-allocations, clicking save will result in the Recipient Form appearing for completion.

![](static/docs/user_manual/screen_transplantation_allocation_3.png)

This fairly small form contains sections on:

* Patient Description - same questions as for Donor:Patient Description
* Recipient Details - additional questions about the Recipient
* Peri-Operative data - reporting on what happens for the implantation
* Cleaning Log - the todo list for finishing off

![](static/docs/user_manual/screen_transplantation_recipient.png)

### Sample Collection and Recording

At the end of the Transplantation:Recipient form (seen above) you can see a section marked Samples, and similarly, on the Transplant:Samples tab:

![](static/docs/user_manual/screen_procurement_form_samples.png)

Following the warning advice to save before clicking, clicking on the `Goto Samples Worksheet` button will take you into the Samples data collection system.

We anticipate that various people will be involved in recording sample data, and those users may not be the technicians involved in the procurement of the sample - for example, Perfusate 1 and 2 from the Donor Procurement proceedure are likely to be recorded by the recieving technician, and consequently, those samples are logged on the Recipient Worksheet.

![](static/docs/user_manual/screen_samples_worksheet.png)

The Samples Worksheet is a fairly long form with not a lot of questions being asked. Depending on whether this is a Donor or Recipient Worksheet, there will be a range of sections covering Perfusate, Urine, Blood, and Biopsy data. The first thing to record is the Barcode number for the Worksheet that will accompany the samples back to the lab.

![](static/docs/user_manual/screen_samples_perfusate.png)

Each type of sample has a slightly different collection of data. Urine and Perfusate, for example, are single events that result in one sample, which needs to be processed in a centrifuge.

The Notes field on each sample is to record any Deviations from procedure so that our Biobank project team can follow up accordingly.

![](static/docs/user_manual/screen_samples_blood.png)

Blood and Biopsy sections result in two Samples from just one collection event, and again, will ask the relevant sub-questions.

Remeber to keep Saving as often as you can, and that partial information will be stored (assuming there are no errors detected).

## Work in Progress

This system remains a work in progress as new features are added and any issues that arise are corrected. Please expect this manual to change frequently to reflect this work, and ensure that any downloaded copies are kept up to date.

Please direct any queries to the COPE Admin Team.

**END**