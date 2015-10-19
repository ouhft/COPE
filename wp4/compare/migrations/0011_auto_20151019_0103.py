# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models
import django.core.validators


class Migration(migrations.Migration):

    dependencies = [
        ('staff_person', '0001_initial'),
        ('compare', '0010_auto_20151018_1856'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='recipient',
            name='transplant_coordinator',
        ),
        migrations.AddField(
            model_name='recipient',
            name='signed_consent',
            field=models.NullBooleanField(default=None, verbose_name='RE13 informed consent given'),
        ),
        migrations.AddField(
            model_name='recipient',
            name='single_kidney_transplant',
            field=models.NullBooleanField(default=None, verbose_name='RE14 receiving one kidney'),
        ),
        migrations.AddField(
            model_name='recipient',
            name='theatre_contact',
            field=models.ForeignKey(related_name='recipient_transplant_coordinator_set', verbose_name='RE04 name of the theatre contact', blank=True, to='staff_person.StaffPerson', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='admitted_to_itu',
            field=models.BooleanField(default=False, verbose_name='DO14 admitted to ITU'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='age',
            field=models.PositiveSmallIntegerField(verbose_name='DO12 age', validators=[django.core.validators.MinValueValidator(50), django.core.validators.MaxValueValidator(99)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='alcohol_abuse',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO25 alcohol abuse', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='blood_group',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OP07 blood group', choices=[(1, 'O'), (2, 'A'), (3, 'B'), (4, 'AB'), (5, 'OPc06 Unknown')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='cardiac_arrest',
            field=models.NullBooleanField(verbose_name='DO26 cardiac arrest'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='circulatory_arrest',
            field=models.DateTimeField(null=True, verbose_name='DO41 end of cardiac output', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_admitted_to_itu',
            field=models.DateField(null=True, verbose_name='DO15 when admitted to ITU', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_of_admission',
            field=models.DateField(null=True, verbose_name='DO13 date of admission', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_of_birth',
            field=models.DateField(null=True, verbose_name='OP02 date of birth', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_of_procurement',
            field=models.DateField(null=True, verbose_name='DO16 date of procurement', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='death_diagnosed',
            field=models.DateTimeField(null=True, verbose_name='DO43 knife to skin time', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diabetes_melitus',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO24 diabetes mellitus', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diagnosis',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO22 diagnosis', choices=[(1, 'DOc01 Cerebrovascular Accident'), (2, 'DOc02 Hypoxia'), (3, 'DOc03 Trauma'), (4, 'DOc04 Other')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diagnosis_other',
            field=models.CharField(max_length=250, verbose_name='DO23 other diagnosis', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diastolic_blood_pressure',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO28 last diastolic blood pressure', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(200)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diuresis_last_day',
            field=models.PositiveSmallIntegerField(null=True, verbose_name='DO29 diuresis last day (ml)', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diuresis_last_hour',
            field=models.PositiveSmallIntegerField(null=True, verbose_name='DO30 diuresis last hour (ml)', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='dobutamine',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO32 dobutamine', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_blood_1_EDTA',
            field=models.OneToOneField(related_name='donor_blood_1', null=True, blank=True, to='samples.Sample', verbose_name='DO49 db 1.1 edta'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_blood_1_SST',
            field=models.OneToOneField(related_name='donor_blood_2', null=True, blank=True, to='samples.Sample', verbose_name='DO50 db 1.2 sst'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_urine_1',
            field=models.OneToOneField(related_name='donor_urine_1', null=True, blank=True, to='samples.Sample', verbose_name='DO51 du 1'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_urine_2',
            field=models.OneToOneField(related_name='donor_urine_2', null=True, blank=True, to='samples.Sample', verbose_name='DO52 du 2'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='dopamine',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO31 dopamine', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='ethnicity',
            field=models.IntegerField(blank=True, null=True, verbose_name='OP06 ethnicity', choices=[(1, 'OPc03 Caucasian'), (2, 'OPc04 Black'), (3, 'OPc05 Other')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='gender',
            field=models.CharField(default='M', max_length=1, verbose_name='OP03 gender', choices=[('M', 'OPc01 Male'), ('F', 'OPc02 Female')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='height',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OP05 Height (cm)', validators=[django.core.validators.MinValueValidator(100), django.core.validators.MaxValueValidator(250)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='heparin',
            field=models.NullBooleanField(verbose_name='DO48 heparin'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='last_creatinine',
            field=models.FloatField(blank=True, null=True, verbose_name='DO36 last creatinine', validators=[django.core.validators.MinValueValidator(0.0)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='length_of_no_touch',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO42 length of no touch period (minutes)', validators=[django.core.validators.MinValueValidator(1), django.core.validators.MaxValueValidator(60)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='life_support_withdrawal',
            field=models.DateTimeField(null=True, verbose_name='DO38 withdrawal of life support', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='max_creatinine',
            field=models.FloatField(null=True, verbose_name='DO37 max creatinine', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='multiple_recipients',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO02 Multiple recipients', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='nor_adrenaline',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO33 nor adrenaline', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='number',
            field=models.CharField(max_length=20, verbose_name='OP01 NHSBT Number', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='o2_saturation',
            field=models.DateTimeField(null=True, verbose_name='DO40 O2 saturation below 80%', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='other_medication_details',
            field=models.CharField(max_length=250, verbose_name='DO35 other medication', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='other_organs_liver',
            field=models.BooleanField(default=False, verbose_name='DO20 liver'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='other_organs_lungs',
            field=models.BooleanField(default=False, verbose_name='DO18 lungs'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='other_organs_pancreas',
            field=models.BooleanField(default=False, verbose_name='DO19 pancreas'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='other_organs_procured',
            field=models.BooleanField(default=False, verbose_name='DO17 other organs procured'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='other_organs_tissue',
            field=models.BooleanField(default=False, verbose_name='DO21 tissue'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='perfusion_started',
            field=models.DateTimeField(null=True, verbose_name='DO44 start in-situ cold perfusion', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='sequence_number',
            field=models.PositiveSmallIntegerField(default=0, verbose_name='DO01 sequence number'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systemic_flush_used',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO45 systemic (aortic) flush solution used', choices=[(3, 'HTK'), (2, "Marshall's"), (1, 'UW'), (4, 'DOc04 Other')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systemic_flush_used_other',
            field=models.CharField(max_length=250, verbose_name='DO46 systemic flush used', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systemic_flush_volume_used',
            field=models.PositiveSmallIntegerField(null=True, verbose_name='DO47 aortic - volume (ml)', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systolic_blood_pressure',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO27 last systolic blood pressure', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(200)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systolic_pressure_low',
            field=models.DateTimeField(null=True, verbose_name='DO39 systolic arterial pressure', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='technician_arrival',
            field=models.DateTimeField(null=True, verbose_name='DO08 arrival time of technician', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='vasopressine',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO34 vasopressine', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='weight',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OP04 Weight (kg)', validators=[django.core.validators.MinValueValidator(20), django.core.validators.MaxValueValidator(200)]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='artificial_patch_number',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OR15 number of patches', validators=[django.core.validators.MinValueValidator(1), django.core.validators.MaxValueValidator(2)]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='artificial_patch_size',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OR14 artificial patch size', choices=[(1, 'ORc12 Small'), (2, 'ORc13 Large')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='artificial_patch_used',
            field=models.NullBooleanField(verbose_name='OR13 artificial patch used'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='graft_damage',
            field=models.PositiveSmallIntegerField(default=5, verbose_name='OR04 renal graft damage', choices=[(5, 'ORc01 None'), (1, 'ORc02 Arterial Damage'), (2, 'ORc03 Venous Damage'), (3, 'ORc04 Ureteral Damage'), (4, 'ORc05 Parenchymal Damage'), (6, 'ORc06 Other Damage')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='graft_damage_other',
            field=models.CharField(max_length=250, verbose_name='OR05 other damage done', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='ice_container_replenished',
            field=models.NullBooleanField(verbose_name='OR20 ice container replenished'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='ice_container_replenished_at',
            field=models.DateTimeField(null=True, verbose_name='OR21 ice container replenished at', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='location',
            field=models.CharField(max_length=1, verbose_name='OR01 kidney location', choices=[('L', 'OR01 Left'), ('R', 'OR02 Right')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='not_transplantable_reason',
            field=models.CharField(max_length=250, verbose_name='OR08 not transplantable because', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_changed',
            field=models.NullBooleanField(verbose_name='OR18 oxygen bottle changed'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_changed_at',
            field=models.DateTimeField(null=True, verbose_name='OR19 oxygen bottle changed at', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_full',
            field=models.NullBooleanField(verbose_name='OR16 is oxygen bottle full'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_open',
            field=models.NullBooleanField(verbose_name='OR17 oxygen bottle opened'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='patch_holder',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OR12 used patch holder', choices=[(1, 'ORc12 Small'), (2, 'ORc13 Large'), (3, 'ORc14 Double Artery')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_1',
            field=models.ForeignKey(related_name='kidney_perfusate_1', verbose_name='OR26 p1', blank=True, to='samples.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_2',
            field=models.ForeignKey(related_name='kidney_perfusate_2', verbose_name='OR27 p2', blank=True, to='samples.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_3',
            field=models.ForeignKey(related_name='kidney_perfusate_3', verbose_name='OR28 p3', blank=True, to='samples.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_measurable',
            field=models.NullBooleanField(verbose_name='OR22 perfusate measurable'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_measure',
            field=models.FloatField(null=True, verbose_name='OR23 value pO2', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_file',
            field=models.ForeignKey(verbose_name='OR25 machine file', blank=True, to='compare.PerfusionFile', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_machine',
            field=models.ForeignKey(verbose_name='OR24 perfusion machine', blank=True, to='compare.PerfusionMachine', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_not_possible_because',
            field=models.CharField(max_length=250, verbose_name='OR10 not possible because', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_possible',
            field=models.NullBooleanField(verbose_name='OR09 machine perfusion possible?'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_started',
            field=models.DateTimeField(null=True, verbose_name='OR11 machine perfusion', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='preservation',
            field=models.PositiveSmallIntegerField(default=9, choices=[(9, 'ORc11 Not Set'), (0, 'HMP'), (1, 'HMP O2')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='removal',
            field=models.DateTimeField(null=True, verbose_name='OR02 time out', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='renal_arteries',
            field=models.PositiveSmallIntegerField(null=True, verbose_name='OR03 number of renal arteries', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='transplantable',
            field=models.NullBooleanField(verbose_name='OR07 is transplantable'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='washout_perfusion',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OR06 perfusion characteristics', choices=[(1, 'ORc07 Homogenous'), (2, 'ORc08 Patchy'), (3, 'ORc09 Blue'), (9, 'ORc10 Unknown')]),
        ),
        migrations.AlterField(
            model_name='procurementresource',
            name='expiry_date',
            field=models.DateField(null=True, verbose_name='PR04 expiry date', blank=True),
        ),
        migrations.AlterField(
            model_name='procurementresource',
            name='lot_number',
            field=models.CharField(max_length=50, verbose_name='PR03 lot number', blank=True),
        ),
        migrations.AlterField(
            model_name='procurementresource',
            name='organ',
            field=models.ForeignKey(verbose_name='PR01 related kidney', to='compare.Organ'),
        ),
        migrations.AlterField(
            model_name='procurementresource',
            name='type',
            field=models.CharField(max_length=5, verbose_name='PR02 resource used', choices=[('D', 'PRc01 Disposables'), ('C-SM', 'PRc02 Extra cannula small (3mm)'), ('C-LG', 'PRc03 Extra cannula large (5mm)'), ('PH-SM', 'PRc04 Extra patch holder small'), ('PH-LG', 'PRc05 Extra patch holder large'), ('DB-C', 'PRc06 Extra double cannula set'), ('P', 'PRc07 Perfusate solution')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='arrival_at_recipient_hospital',
            field=models.DateTimeField(null=True, verbose_name='RE08 arrival at transplant hospital', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='blood_group',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OP07 blood group', choices=[(1, 'O'), (2, 'A'), (3, 'B'), (4, 'AB'), (5, 'OPc06 Unknown')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='call_received',
            field=models.DateTimeField(null=True, verbose_name='RE02 call received from transplant co-ordinator at', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='date_of_birth',
            field=models.DateField(null=True, verbose_name='OP02 date of birth', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='depart_perfusion_centre',
            field=models.DateTimeField(null=True, verbose_name='RE07 departure from hub', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='ethnicity',
            field=models.IntegerField(blank=True, null=True, verbose_name='OP06 ethnicity', choices=[(1, 'OPc03 Caucasian'), (2, 'OPc04 Black'), (3, 'OPc05 Other')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='gender',
            field=models.CharField(default='M', max_length=1, verbose_name='OP03 gender', choices=[('M', 'OPc01 Male'), ('F', 'OPc02 Female')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='height',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OP05 Height (cm)', validators=[django.core.validators.MinValueValidator(100), django.core.validators.MaxValueValidator(250)]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='journey_remarks',
            field=models.TextField(verbose_name='RE09 journey notes', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='number',
            field=models.CharField(max_length=20, verbose_name='OP01 NHSBT Number', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='perfusion_technician',
            field=models.ForeignKey(related_name='recipient_perfusion_technician_set', verbose_name='RO01 name of transplant technician', blank=True, to='staff_person.StaffPerson', null=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='reallocated',
            field=models.NullBooleanField(default=None, verbose_name='RE10 reallocated'),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='reallocation_reason',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='RE11 reason for re-allocation', choices=[(1, 'REc01 Positive crossmatch'), (2, 'REc02 Unknown'), (3, 'REc03 Other')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='reallocation_reason_other',
            field=models.CharField(max_length=250, verbose_name='RE12 other reason', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='removed_from_machine_at',
            field=models.DateTimeField(null=True, verbose_name='kidney removed from machine at', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='renal_disease',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='RE13 renal disease', choices=[(1, 'REc04 Glomerular diseases'), (2, 'REc05 Polycystic kidneys'), (3, 'REc06 Uncertain etiology'), (4, 'REc07 Tubular and interstitial diseases'), (5, 'REc08 Retransplant graft failure'), (6, 'REc09 diabetic nephropathyes'), (7, 'REc10 hypertensive nephropathyes'), (8, 'REc11 congenital rare disorders'), (9, 'REc12 renovascular and other diseases'), (10, 'REc13 neoplasms'), (11, 'REc14 other')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='renal_disease_other',
            field=models.CharField(max_length=250, verbose_name='RE14 other renal disease', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='scheduled_start',
            field=models.DateTimeField(null=True, verbose_name='RE05 scheduled start', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='technician_arrival',
            field=models.DateTimeField(null=True, verbose_name='RE06 arrival time at hub', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='transplant_hospital',
            field=models.ForeignKey(verbose_name='RE03 transplant hospital', blank=True, to='locations.Hospital', null=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='transplant_side',
            field=models.CharField(blank=True, max_length=1, verbose_name='transplant side', choices=[('L', 'OR01 Left'), ('R', 'OR02 Right')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='weight',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OP04 Weight (kg)', validators=[django.core.validators.MinValueValidator(20), django.core.validators.MaxValueValidator(200)]),
        ),
        migrations.AlterField(
            model_name='retrievalteam',
            name='centre_code',
            field=models.PositiveSmallIntegerField(verbose_name='RT01 centre code', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(99)]),
        ),
    ]
