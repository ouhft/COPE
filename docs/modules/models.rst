
Models
======

This is source documentation for the WP4 data models. These are Python/Django classes mapped to the database by Django's ORM, and as such follow the usual Django conventions on model design. There are a few other conventions to note, general ones are noted here, class specific notes are inline below.

Compare is the central app to this system (presently), and the Core models (such as VersionControlModel) will appear all over the rest of the apps. This means common fields such as `version`, `created_on`, `created_by`, and `record_locked` will feature repeatedly on many of the models below.

Items that appear in CAPITAL LETTERS are frequently the constants used in the system. Whilst many of them follow a clear naming convention (e.g. BLOOD_xxx), some are still in their orignal format (e.g. MALE, FEMALE, CAUCASIAN). Most are also part of groups, which means you'll see a topicname_CHOICES constant that links certain constants - typically for the benefit of dropdown lists and other static content (e.g. BLOOD_GROUP_CHOICES).

Where model attributes use a choice list (e.g. from a constant), there are helper methods to translate the stored value into a display string. These take the form of OBJECT.get_ATTRIBUTE_display() methods (e.g. OrganPerson.get_gender_display() )




Compare
-------
Core defines the following common CONSTANTS used widely through the system

.. automodule:: wp4.compare.models.core
.. automodule:: wp4.compare.models.donor
.. automodule:: wp4.compare.models.organ
.. automodule:: wp4.compare.models.transplantation

Adverse Event
-------------
.. automodule:: wp4.adverse_event.models

Follow Ups
----------
.. automodule:: wp4.followups.models

Health Economics
----------------
.. automodule:: wp4.health_economics.models

Locations
---------
.. automodule:: wp4.locations.models

Perfusion Machine
-----------------
.. automodule:: wp4.perfusion_machine.models

Samples
-------
.. automodule:: wp4.samples.models

Staff Person
------------
.. automodule:: wp4.staff_person.models

Theme
-----
No Models in theme app