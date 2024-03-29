# -*- coding: utf-8 -*-
# Generated by Django 1.10.6 on 2017-03-17 01:36
from __future__ import unicode_literals

import django.core.validators
from django.db import migrations, models
import django.db.models.deletion
import django.utils.timezone
import livefield.fields
import wp4.compare.models.core
import wp4.compare.validators


class Migration(migrations.Migration):

    initial = True

    dependencies = [
        ('locations', '0001_initial'),
    ]

    operations = [
        migrations.CreateModel(
            name='Donor',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('record_locked', models.BooleanField(default=False, help_text='locked by the admin team')),
                ('live', livefield.fields.LiveField(default=True)),
                ('sequence_number', models.PositiveSmallIntegerField(default=0, help_text='Internal value for tracking trial ID sequence number. Value of 1-99', validators=[django.core.validators.MaxValueValidator(99)])),
                ('multiple_recipients', models.PositiveSmallIntegerField(blank=True, choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')], null=True, verbose_name='DO02 Multiple recipients')),
                ('not_randomised_because', models.PositiveSmallIntegerField(choices=[(0, 'DOc15 Not Applicable'), (1, 'DOc10 Donor not proceeding'), (2, 'DOc11 One or more kidneys allocated to non-trial location'), (3, 'DOc12 Kidneys not allocated'), (4, 'DOc13 Kidneys not transplanable'), (5, 'DOc14 Other')], default=0, verbose_name='DO51 Why was this not randomised?')),
                ('not_randomised_because_other', models.CharField(blank=True, max_length=250, verbose_name='DO52 More details')),
                ('procurement_form_completed', models.BooleanField(default=False, help_text='Select Yes when you believe the form is complete and you have no more data to enter', verbose_name='DO99 Form complete')),
                ('admin_notes', models.TextField(blank=True, verbose_name='DO50 Admin notes')),
                ('trial_id', models.CharField(blank=True, max_length=10, verbose_name='DO99 donor id')),
                ('call_received', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='DO05 Consultant to MTO called at')),
                ('call_received_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('retrieval_hospital', models.CharField(blank=True, max_length=100, verbose_name='DO06 donor hospital')),
                ('scheduled_start', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050', null=True, validators=[wp4.compare.validators.validate_between_1900_2050], verbose_name='DO07 time of withdrawal therapy')),
                ('scheduled_start_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('technician_arrival', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='DO08 arrival time of technician')),
                ('technician_arrival_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('ice_boxes_filled', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='DO09 ice boxes filled')),
                ('ice_boxes_filled_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('depart_perfusion_centre', models.DateTimeField(blank=True, null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='DO10 departure from base hospital at')),
                ('depart_perfusion_centre_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('arrival_at_donor_hospital', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='DO11 arrival at donor hospital')),
                ('arrival_at_donor_hospital_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('age', models.PositiveSmallIntegerField(help_text='Age must be in the range 50-99', validators=[django.core.validators.MinValueValidator(50), django.core.validators.MaxValueValidator(99)], verbose_name='DO12 age')),
                ('date_of_admission', models.DateField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='DO13 date of admission')),
                ('date_of_admission_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('admitted_to_itu', models.BooleanField(default=False, verbose_name='DO14 admitted to ITU')),
                ('date_admitted_to_itu', models.DateField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='DO15 when admitted to ITU')),
                ('date_admitted_to_itu_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('date_of_procurement', models.DateField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='DO16 date of procurement')),
                ('other_organs_procured', models.BooleanField(default=False, verbose_name='DO17 other organs procured')),
                ('other_organs_lungs', models.BooleanField(default=False, verbose_name='DO18 lungs')),
                ('other_organs_pancreas', models.BooleanField(default=False, verbose_name='DO19 pancreas')),
                ('other_organs_liver', models.BooleanField(default=False, verbose_name='DO20 liver')),
                ('other_organs_tissue', models.BooleanField(default=False, verbose_name='DO21 tissue')),
                ('diagnosis', models.PositiveSmallIntegerField(blank=True, choices=[(1, 'DOc01 Cerebrovascular Accident'), (2, 'DOc02 Hypoxia'), (3, 'DOc03 Trauma'), (4, 'DOc04 Other')], null=True, verbose_name='DO22 diagnosis')),
                ('diagnosis_other', models.CharField(blank=True, max_length=250, verbose_name='DO23 other diagnosis')),
                ('diabetes_melitus', models.PositiveSmallIntegerField(blank=True, choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')], null=True, verbose_name='DO24 diabetes mellitus')),
                ('alcohol_abuse', models.PositiveSmallIntegerField(blank=True, choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')], null=True, verbose_name='DO25 alcohol abuse')),
                ('cardiac_arrest', models.NullBooleanField(verbose_name='DO26 cardiac arrest')),
                ('systolic_blood_pressure', models.PositiveSmallIntegerField(blank=True, help_text='Value must be in the range 10-200', null=True, validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(200)], verbose_name='DO27 last systolic blood pressure')),
                ('diastolic_blood_pressure', models.PositiveSmallIntegerField(blank=True, help_text='Value must be in the range 10-200', null=True, validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(200)], verbose_name='DO28 last diastolic blood pressure')),
                ('diuresis_last_day', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO29 diuresis last day (ml)')),
                ('diuresis_last_day_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('diuresis_last_hour', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO30 diuresis last hour (ml)')),
                ('diuresis_last_hour_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('dopamine', models.PositiveSmallIntegerField(blank=True, choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')], null=True, verbose_name='DO31 dopamine')),
                ('dobutamine', models.PositiveSmallIntegerField(blank=True, choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')], null=True, verbose_name='DO32 dobutamine')),
                ('nor_adrenaline', models.PositiveSmallIntegerField(blank=True, choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')], null=True, verbose_name='DO33 nor adrenaline')),
                ('vasopressine', models.PositiveSmallIntegerField(blank=True, choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')], null=True, verbose_name='DO34 vasopressine')),
                ('other_medication_details', models.CharField(blank=True, max_length=250, verbose_name='DO35 other medication')),
                ('last_creatinine', models.FloatField(blank=True, null=True, validators=[django.core.validators.MinValueValidator(0.0)], verbose_name='DO36 last creatinine')),
                ('last_creatinine_unit', models.PositiveSmallIntegerField(choices=[(1, 'mg/dl'), (2, 'umol/L')], default=1)),
                ('max_creatinine', models.FloatField(blank=True, null=True, verbose_name='DO37 max creatinine')),
                ('max_creatinine_unit', models.PositiveSmallIntegerField(choices=[(1, 'mg/dl'), (2, 'umol/L')], default=1)),
                ('life_support_withdrawal', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='DO38 withdrawal of life support')),
                ('systolic_pressure_low', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='DO39 systolic arterial pressure')),
                ('systolic_pressure_low_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('o2_saturation', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='DO40 O2 saturation below 80%')),
                ('o2_saturation_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('circulatory_arrest', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='DO41 end of cardiac output')),
                ('circulatory_arrest_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('length_of_no_touch', models.PositiveSmallIntegerField(blank=True, help_text='Value must be in the range 1-60 minutes', null=True, validators=[django.core.validators.MinValueValidator(1), django.core.validators.MaxValueValidator(60)], verbose_name='DO42 length of no touch period (minutes)')),
                ('death_diagnosed', models.DateTimeField(blank=True, help_text='DO43h This also counts as Date of Death for donor. Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='DO43 knife to skin time')),
                ('perfusion_started', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='DO44 start in-situ cold perfusion')),
                ('perfusion_started_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('systemic_flush_used', models.PositiveSmallIntegerField(blank=True, choices=[(3, 'HTK'), (2, "Marshall's"), (1, 'UW'), (4, 'DOc04 Other')], null=True, verbose_name='DO45 systemic (aortic) flush solution used')),
                ('systemic_flush_used_other', models.CharField(blank=True, max_length=250, verbose_name='DO46 systemic flush used')),
                ('systemic_flush_volume_used', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO47 aortic - volume (ml)')),
                ('heparin', models.NullBooleanField(verbose_name='DO48 heparin')),
            ],
            options={
                'verbose_name': 'DOm1 donor',
                'verbose_name_plural': 'DOm2 donors',
                'permissions': (('view_donor', 'Can only view the data'), ('restrict_to_national', 'Can only use data from the same location country'), ('restrict_to_local', 'Can only use data from a specific location')),
            },
        ),
        migrations.CreateModel(
            name='Organ',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('record_locked', models.BooleanField(default=False, help_text='locked by the admin team')),
                ('live', livefield.fields.LiveField(default=True)),
                ('location', models.CharField(choices=[('L', 'ORc01 Left'), ('R', 'ORc02 Right')], max_length=1, verbose_name='OR01 kidney location')),
                ('not_allocated_reason', models.CharField(blank=True, max_length=250, verbose_name='OR31 not transplantable because')),
                ('admin_notes', models.TextField(blank=True, verbose_name='OR50 Admin notes')),
                ('transplantation_notes', models.TextField(blank=True, verbose_name='OR51 Transplantation notes')),
                ('transplantation_form_completed', models.BooleanField(default=False, help_text='Select Yes when you believe the form is complete and you have no more data to enter', verbose_name='OR99 Form complete')),
                ('trial_id', models.CharField(blank=True, max_length=10, verbose_name='OR99 organ id')),
                ('removal', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='OR02 time out')),
                ('renal_arteries', models.PositiveSmallIntegerField(blank=True, help_text='Number of arteries must be in range 0-5', null=True, validators=[django.core.validators.MinValueValidator(0), django.core.validators.MaxValueValidator(5)], verbose_name='OR03 number of renal arteries')),
                ('graft_damage', models.PositiveSmallIntegerField(choices=[(5, 'ORc01 None'), (1, 'ORc02 Arterial Damage'), (2, 'ORc03 Venous Damage'), (3, 'ORc04 Ureteral Damage'), (4, 'ORc05 Parenchymal Damage'), (6, 'ORc06 Other Damage')], default=5, verbose_name='OR04 renal graft damage')),
                ('graft_damage_other', models.CharField(blank=True, max_length=250, verbose_name='OR05 other damage done')),
                ('washout_perfusion', models.PositiveSmallIntegerField(blank=True, choices=[(1, 'ORc07 Homogenous'), (2, 'ORc08 Patchy'), (3, 'ORc09 Blue'), (9, 'ORc10 Unknown')], null=True, verbose_name='OR06 perfusion characteristics')),
                ('transplantable', models.NullBooleanField(help_text='OR07h This answer can be amended after randomisation and saving of the form if necessary', verbose_name='OR07 is transplantable')),
                ('not_transplantable_reason', models.CharField(blank=True, max_length=250, verbose_name='OR08 not transplantable because')),
                ('preservation', models.PositiveSmallIntegerField(choices=[(9, 'ORc11 Not Set'), (0, 'HMP'), (1, 'HMP O2')], default=9)),
                ('perfusion_possible', models.NullBooleanField(verbose_name='OR09 machine perfusion possible?')),
                ('perfusion_not_possible_because', models.CharField(blank=True, max_length=250, verbose_name='OR10 not possible because')),
                ('perfusion_started', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='OR11 machine perfusion')),
                ('patch_holder', models.PositiveSmallIntegerField(blank=True, choices=[(1, 'ORc12 Small'), (2, 'ORc13 Large'), (3, 'ORc14 Double Artery')], null=True, verbose_name='OR12 used patch holder')),
                ('artificial_patch_used', models.NullBooleanField(verbose_name='OR13 artificial patch used')),
                ('artificial_patch_size', models.PositiveSmallIntegerField(blank=True, choices=[(1, 'ORc12 Small'), (2, 'ORc13 Large')], null=True, verbose_name='OR14 artificial patch size')),
                ('artificial_patch_number', models.PositiveSmallIntegerField(blank=True, null=True, validators=[django.core.validators.MinValueValidator(1), django.core.validators.MaxValueValidator(2)], verbose_name='OR15 number of patches')),
                ('oxygen_bottle_full', models.NullBooleanField(verbose_name='OR16 is oxygen bottle full')),
                ('oxygen_bottle_open', models.NullBooleanField(verbose_name='OR17 oxygen bottle opened')),
                ('oxygen_bottle_changed', models.NullBooleanField(verbose_name='OR18 oxygen bottle changed')),
                ('oxygen_bottle_changed_at', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='OR19 oxygen bottle changed at')),
                ('oxygen_bottle_changed_at_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('ice_container_replenished', models.NullBooleanField(verbose_name='OR20 ice container replenished')),
                ('ice_container_replenished_at', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='OR21 ice container replenished at')),
                ('ice_container_replenished_at_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('perfusate_measurable', models.NullBooleanField(verbose_name='OR22 perfusate measurable')),
                ('perfusate_measure', models.FloatField(blank=True, null=True, verbose_name='OR23 value pO2')),
            ],
            options={
                'verbose_name': 'ORm1 organ',
                'verbose_name_plural': 'ORm2 organs',
                'permissions': (('view_organ', 'Can only view the data'), ('restrict_to_national', 'Can only use data from the same location country'), ('restrict_to_local', 'Can only use data from a specific location')),
                'base_manager_name': 'objects',
                'default_manager_name': 'objects',
            },
        ),
        migrations.CreateModel(
            name='OrganAllocation',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('record_locked', models.BooleanField(default=False, help_text='locked by the admin team')),
                ('live', livefield.fields.LiveField(default=True)),
                ('call_received', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='OA02 call received from transplant co-ordinator at')),
                ('call_received_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('scheduled_start', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050', null=True, validators=[wp4.compare.validators.validate_between_1900_2050], verbose_name='OA05 scheduled start')),
                ('scheduled_start_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('technician_arrival', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='OA06 arrival time at hub')),
                ('technician_arrival_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('depart_perfusion_centre', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='OA07 departure from hub')),
                ('depart_perfusion_centre_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('arrival_at_recipient_hospital', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='OA08 arrival at transplant hospital')),
                ('arrival_at_recipient_hospital_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('journey_remarks', models.TextField(blank=True, verbose_name='OA09 journey notes')),
                ('reallocated', models.NullBooleanField(default=None, verbose_name='OA10 reallocated')),
                ('reallocation_reason', models.PositiveSmallIntegerField(blank=True, choices=[(1, 'OAc01 Positive crossmatch'), (2, 'OAc02 Unknown'), (3, 'OAc03 Other')], null=True, verbose_name='OA11 reason for re-allocation')),
                ('reallocation_reason_other', models.CharField(blank=True, max_length=250, verbose_name='OA12 other reason')),
            ],
            options={
                'verbose_name': 'OAm1 organ allocation',
                'verbose_name_plural': 'OAm2 organ allocations',
                'permissions': (('view_organallocation', 'Can only view the data'), ('restrict_to_national', 'Can only use data from the same location country'), ('restrict_to_local', 'Can only use data from a specific location')),
                'get_latest_by': 'pk',
            },
        ),
        migrations.CreateModel(
            name='Patient',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('record_locked', models.BooleanField(default=False, help_text='locked by the admin team')),
                ('live', livefield.fields.LiveField(default=True)),
                ('number', models.CharField(blank=True, max_length=20, verbose_name='OP01 NHSBT Number')),
                ('date_of_birth', models.DateField(blank=True, help_text='Date can not be in the future', null=True, validators=[wp4.compare.validators.validate_not_in_future], verbose_name='OP02 date of birth')),
                ('date_of_birth_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('date_of_death', models.DateField(blank=True, help_text='Date can not be in the future', null=True, validators=[wp4.compare.validators.validate_not_in_future], verbose_name='OP08 date of death')),
                ('date_of_death_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('gender', models.CharField(choices=[('M', 'OPc01 Male'), ('F', 'OPc02 Female')], default='M', max_length=1, verbose_name='OP03 gender')),
                ('weight', models.DecimalField(blank=True, decimal_places=1, help_text='Answer must be in range 20.0-200.0kg', max_digits=4, null=True, validators=[django.core.validators.MinValueValidator(20.0), django.core.validators.MaxValueValidator(200.0)], verbose_name='OP04 Weight (kg)')),
                ('height', models.PositiveSmallIntegerField(blank=True, help_text='Answer must be in range 100-250cm', null=True, validators=[django.core.validators.MinValueValidator(100), django.core.validators.MaxValueValidator(250)], verbose_name='OP05 Height (cm)')),
                ('ethnicity', models.IntegerField(blank=True, choices=[(1, 'OPc03 Caucasian'), (2, 'OPc04 Black'), (3, 'OPc05 Other')], null=True, verbose_name='OP06 ethnicity')),
                ('blood_group', models.PositiveSmallIntegerField(blank=True, choices=[(1, 'O'), (2, 'A'), (3, 'B'), (4, 'AB'), (5, 'OPc06 Unknown')], null=True, verbose_name='OP07 blood group')),
            ],
            options={
                'verbose_name': 'OPm1 trial person',
                'verbose_name_plural': 'OPm2 organ people',
                'ordering': ['number'],
            },
        ),
        migrations.CreateModel(
            name='ProcurementResource',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('record_locked', models.BooleanField(default=False, help_text='locked by the admin team')),
                ('live', livefield.fields.LiveField(default=True)),
                ('type', models.CharField(choices=[('D', 'PRc01 Disposables'), ('C-SM', 'PRc02 Extra cannula small (3mm)'), ('C-LG', 'PRc03 Extra cannula large (5mm)'), ('PH-SM', 'PRc04 Extra patch holder small'), ('PH-LG', 'PRc05 Extra patch holder large'), ('DB-C', 'PRc06 Extra double cannula set'), ('P', 'PRc07 Perfusate solution')], max_length=5, verbose_name='PR02 resource used')),
                ('lot_number', models.CharField(blank=True, max_length=50, verbose_name='PR03 lot number')),
                ('expiry_date', models.DateField(blank=True, null=True, verbose_name='PR04 expiry date')),
                ('expiry_date_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
            ],
            options={
                'verbose_name': 'PRm1 procurement resource',
                'verbose_name_plural': 'PRm2 procurement resources',
                'permissions': (('view_procurementresource', 'Can only view the data'),),
            },
        ),
        migrations.CreateModel(
            name='Randomisation',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('list_code', models.PositiveSmallIntegerField(choices=[(1, 'RNc01 UK Live list'), (2, 'RNc02 Europe Live list'), (4, 'RNc03 UK Offline list'), (3, 'RNc04 Europe Offline list')], verbose_name='RA01 list code')),
                ('result', models.BooleanField(default=wp4.compare.models.core.random_5050, verbose_name='RA02 result')),
                ('allocated_on', models.DateTimeField(default=django.utils.timezone.now, verbose_name='RA03 allocated on')),
            ],
            options={
                'permissions': (('hide_randomisation', 'User should be kept ignorant of this data'),),
            },
        ),
        migrations.CreateModel(
            name='Recipient',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('record_locked', models.BooleanField(default=False, help_text='locked by the admin team')),
                ('live', livefield.fields.LiveField(default=True)),
                ('signed_consent', models.NullBooleanField(default=None, verbose_name='RE13 informed consent given')),
                ('single_kidney_transplant', models.NullBooleanField(default=None, verbose_name='RE14 receiving one kidney')),
                ('renal_disease', models.PositiveSmallIntegerField(blank=True, choices=[(1, 'REc04 Glomerular diseases'), (2, 'REc05 Polycystic kidneys'), (3, 'REc06 Uncertain etiology'), (4, 'REc07 Tubular and interstitial diseases'), (5, 'REc08 Retransplant graft failure'), (6, 'REc09 diabetic nephropathyes'), (7, 'REc10 hypertensive nephropathyes'), (8, 'REc11 congenital rare disorders'), (9, 'REc12 renovascular and other diseases'), (10, 'REc13 neoplasms'), (11, 'REc14 other')], null=True, verbose_name='RE15 renal disease')),
                ('renal_disease_other', models.CharField(blank=True, max_length=250, verbose_name='RE16 other renal disease')),
                ('pre_transplant_diuresis', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='RE17 diuresis (ml/24hr)')),
                ('knife_to_skin', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='RE18 knife to skin time')),
                ('perfusate_measure', models.FloatField(blank=True, null=True, verbose_name='RE19 pO2 perfusate')),
                ('perfusion_stopped', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='RE20 stop machine perfusion')),
                ('organ_cold_stored', models.BooleanField(default=False, verbose_name='RE21 kidney was cold stored?')),
                ('tape_broken', models.PositiveSmallIntegerField(blank=True, choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')], null=True, verbose_name='RE22 tape over regulator broken')),
                ('removed_from_machine_at', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='RE23 kidney removed from machine at')),
                ('oxygen_full_and_open', models.PositiveSmallIntegerField(blank=True, choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')], null=True, verbose_name='RE24 oxygen full and open')),
                ('organ_untransplantable', models.NullBooleanField(help_text='REh25 Either answer means further questions will open below', verbose_name='RE25 kidney discarded')),
                ('organ_untransplantable_reason', models.CharField(blank=True, max_length=250, verbose_name='RE26 untransplantable because')),
                ('anesthesia_started_at', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='RE27 start anesthesia at')),
                ('incision', models.PositiveSmallIntegerField(blank=True, choices=[(1, 'REc15 midline laparotomy'), (2, 'REc16 hockey stick'), (3, 'REc17 unknown')], null=True, verbose_name='RE28 incision')),
                ('transplant_side', models.CharField(blank=True, choices=[('L', 'ORc01 Left'), ('R', 'ORc02 Right')], max_length=1, verbose_name='RE29 transplant side')),
                ('arterial_problems', models.PositiveSmallIntegerField(blank=True, choices=[(1, 'REc18 None'), (2, 'REc19 ligated polar artery'), (3, 'REc20 reconstructed polar artery'), (4, 'REc21 repaired intima dissection'), (5, 'REc22 other')], null=True, verbose_name='RE30 arterial problems')),
                ('arterial_problems_other', models.CharField(blank=True, max_length=250, verbose_name='RE31 arterial problems other')),
                ('venous_problems', models.PositiveSmallIntegerField(blank=True, choices=[(1, 'REc23 none'), (2, 'REc24 laceration'), (3, 'REc25 elongation plasty'), (4, 'REc26 other')], null=True, verbose_name='RE32 venous problems')),
                ('venous_problems_other', models.CharField(blank=True, max_length=250, verbose_name='RE33 venous problems other')),
                ('anastomosis_started_at', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='RE34 start anastomosis at')),
                ('anastomosis_started_at_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('reperfusion_started_at', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='RE35 start reperfusion at')),
                ('reperfusion_started_at_unknown', models.BooleanField(default=False, help_text='Internal unknown flag')),
                ('mannitol_used', models.PositiveSmallIntegerField(blank=True, choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')], null=True, verbose_name='RE36 mannitol used')),
                ('other_diurectics', models.PositiveSmallIntegerField(blank=True, choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')], null=True, verbose_name='RE37 other diurectics used')),
                ('other_diurectics_details', models.CharField(blank=True, max_length=250, verbose_name='RE38 other diurectics detail')),
                ('systolic_blood_pressure', models.PositiveSmallIntegerField(blank=True, help_text='Value must be in range 10-200', null=True, validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(200)], verbose_name='RE39 systolic blood pressure at reperfusion')),
                ('cvp', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='RE40 cvp at reperfusion')),
                ('intra_operative_diuresis', models.PositiveSmallIntegerField(blank=True, choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')], null=True, verbose_name='RE41 intra-operative diuresis')),
                ('successful_conclusion', models.BooleanField(default=False, verbose_name='RE42 successful conclusion')),
                ('operation_concluded_at', models.DateTimeField(blank=True, help_text='Date must be fall within 1900-2050, and not be in the future', null=True, validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future], verbose_name='RE43 operation concluded at')),
                ('probe_cleaned', models.NullBooleanField(verbose_name='RE44 temperature and flow probe cleaned')),
                ('ice_removed', models.NullBooleanField(verbose_name='RE45 ice and water removed')),
                ('oxygen_flow_stopped', models.NullBooleanField(verbose_name='RE46 oxygen flow stopped')),
                ('oxygen_bottle_removed', models.NullBooleanField(verbose_name='RE47 oxygen bottle removed')),
                ('box_cleaned', models.NullBooleanField(verbose_name='RE48 box kidney assist cleaned')),
                ('batteries_charged', models.NullBooleanField(verbose_name='RE49 batteries charged')),
                ('cleaning_log', models.TextField(blank=True, verbose_name='RE50 cleaning log notes')),
                ('allocation', models.OneToOneField(help_text='Internal link to OrganAllocation', on_delete=django.db.models.deletion.CASCADE, to='compare.OrganAllocation')),
                ('organ', models.OneToOneField(help_text='Internal link to Organ', on_delete=django.db.models.deletion.CASCADE, to='compare.Organ')),
                ('person', models.OneToOneField(help_text='Internal link to Patient', on_delete=django.db.models.deletion.CASCADE, to='compare.Patient')),
            ],
            options={
                'verbose_name': 'REm1 recipient',
                'verbose_name_plural': 'REm2 recipients',
                'permissions': (('view_recipient', 'Can only view the data'), ('restrict_to_national', 'Can only use data from the same location country'), ('restrict_to_local', 'Can only use data from a specific location')),
                'get_latest_by': 'pk',
            },
        ),
        migrations.CreateModel(
            name='RetrievalTeam',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('centre_code', models.PositiveSmallIntegerField(help_text='Value must be in the range 10-99', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(99)], verbose_name='RT01 centre code')),
                ('based_at', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='locations.Hospital', verbose_name='RT02 base hospital')),
            ],
            options={
                'verbose_name': 'RTm1 retrieval team',
                'verbose_name_plural': 'RTm2 retrieval teams',
                'ordering': ['centre_code'],
                'permissions': (('view_retrievalteam', 'Can only view the data'), ('restrict_to_national', 'Can only use data from the same location country'), ('restrict_to_local', 'Can only use data from a specific location')),
            },
        ),
    ]
