# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
from django.conf import settings
import django.core.validators


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0007_auto_20150803_1411'),
    ]

    operations = [
        migrations.AlterModelOptions(
            name='donor',
            options={'ordering': ['sequence_number'], 'verbose_name': 'DOm1 donor', 'verbose_name_plural': 'DOm2 donors'},
        ),
        migrations.AlterModelOptions(
            name='hospital',
            options={'ordering': ['centre_code'], 'verbose_name': 'HOm1 hospital', 'verbose_name_plural': 'HOm2 hospitals'},
        ),
        migrations.AlterModelOptions(
            name='organ',
            options={'verbose_name': 'ORm1 organ', 'verbose_name_plural': 'ORm2 organs'},
        ),
        migrations.AlterModelOptions(
            name='organsoffered',
            options={'verbose_name': 'OOm1 organ offered', 'verbose_name_plural': 'OOm2 organs offered'},
        ),
        migrations.AlterModelOptions(
            name='perfusionfile',
            options={'verbose_name': 'PFm1 perfusion machine file', 'verbose_name_plural': 'PFm2 perfusion machine files'},
        ),
        migrations.AlterModelOptions(
            name='perfusionmachine',
            options={'verbose_name': 'PMm1 perfusion machine', 'verbose_name_plural': 'PMm2 perfusion machines'},
        ),
        migrations.AlterModelOptions(
            name='person',
            options={'verbose_name': 'PEm1 person', 'verbose_name_plural': 'PEm2 people'},
        ),
        migrations.AlterModelOptions(
            name='procurementresource',
            options={'verbose_name': 'PRm1 procurement resource', 'verbose_name_plural': 'PRm2 procurement resources'},
        ),
        migrations.AlterModelOptions(
            name='retrievalteam',
            options={'verbose_name': 'RTm1 retrieval team', 'verbose_name_plural': 'RTm2 retrieval teams'},
        ),
        migrations.AlterModelOptions(
            name='sample',
            options={'ordering': ['taken_at'], 'verbose_name': 'SAm1 sample', 'verbose_name_plural': 'SAm2 samples'},
        ),
        migrations.AlterField(
            model_name='donor',
            name='admitted_to_itu',
            field=models.BooleanField(default=False, verbose_name='DO34 admitted to ITU'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='age',
            field=models.PositiveSmallIntegerField(verbose_name='DO31 age', validators=[django.core.validators.MinValueValidator(50), django.core.validators.MaxValueValidator(99)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='alcohol_abuse',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO57 alcohol abuse', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='arrival_at_donor_hospital',
            field=models.DateTimeField(null=True, verbose_name='DO11 arrival at donor hospital', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='blood_group',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO42 blood group', choices=[(1, b'O'), (2, b'A'), (3, b'B'), (4, b'AB'), (5, 'DO29 Unknown')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='call_received',
            field=models.DateTimeField(null=True, verbose_name='DO05 Consultant to MTO called at', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='cardiac_arrest',
            field=models.NullBooleanField(verbose_name='DO58 cardiac arrest'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='circulatory_arrest',
            field=models.DateTimeField(null=True, verbose_name='DO83 end of cardiac output', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_admitted_to_itu',
            field=models.DateField(null=True, verbose_name='DO35 when admitted to ITU', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_of_admission',
            field=models.DateField(null=True, verbose_name='DO33 date of admission into hospital', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_of_birth',
            field=models.DateField(null=True, verbose_name='DO32 date of birth', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_of_procurement',
            field=models.DateField(null=True, verbose_name='DO36 date of procurement', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='death_diagnosed',
            field=models.DateTimeField(null=True, verbose_name='DO85 knife to skin time', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='depart_perfusion_centre',
            field=models.DateTimeField(null=True, verbose_name='DO10 departure from base hospital at', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diabetes_melitus',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO56 diabetes mellitus', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diagnosis',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO54 diagnosis', choices=[(1, 'DO50 Cerebrivascular Accident'), (2, 'DO51 Hypoxia'), (3, 'DO52 Trauma'), (4, 'DO53 Other')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diagnosis_other',
            field=models.CharField(max_length=250, null=True, verbose_name='DO55 other diagnosis', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diastolic_blood_pressure',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO60 last diastolic blood pressure', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(200)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diuresis_last_day',
            field=models.PositiveSmallIntegerField(null=True, verbose_name='DO61 diuresis last day (ml)', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diuresis_last_hour',
            field=models.PositiveSmallIntegerField(null=True, verbose_name='DO62 diuresis last hour (ml)', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='dobutamine',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO64 dobutamine', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_blood_1_EDTA',
            field=models.ForeignKey(related_name='donor_blood_1_EDTA_set', verbose_name='DO91 db 1.1 edta', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_blood_1_SST',
            field=models.ForeignKey(related_name='donor_blood_1_SST_set', verbose_name='DO92 db 1.2 sst', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_urine_1',
            field=models.ForeignKey(related_name='donor_urine_1_set', verbose_name='DO93 du 1', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_urine_2',
            field=models.ForeignKey(related_name='donor_urine_2_set', verbose_name='DO94 du 2', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='dopamine',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO63 dopamine', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='ethnicity',
            field=models.IntegerField(blank=True, null=True, verbose_name='DO41 ethnicity', choices=[(1, 'DO22 Caucasian'), (2, 'DO23 Black'), (3, 'DO24 Other')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='gender',
            field=models.CharField(default=b'M', max_length=1, verbose_name='DO37 gender', choices=[(b'M', 'DO20 Male'), (b'F', 'DO21 Female')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='height',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO40 Height (cm)', validators=[django.core.validators.MinValueValidator(100), django.core.validators.MaxValueValidator(250)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='heparin',
            field=models.NullBooleanField(verbose_name='DO90 heparin'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='ice_boxes_filled',
            field=models.DateTimeField(null=True, verbose_name='DO09 ice boxes filled', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='last_creatinine',
            field=models.FloatField(blank=True, null=True, verbose_name='DO70 last creatinine', validators=[django.core.validators.MinValueValidator(0.0)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='length_of_no_touch',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO84 length of no touch period (minutes)', validators=[django.core.validators.MinValueValidator(1), django.core.validators.MaxValueValidator(60)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='life_support_withdrawal',
            field=models.DateTimeField(null=True, verbose_name='DO80 withdrawal of life support', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='max_creatinine',
            field=models.FloatField(null=True, verbose_name='DO72 max creatinine', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='nor_adrenaline',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO65 nor adrenaline', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='number',
            field=models.CharField(max_length=20, verbose_name='DO30 NHSBT Number'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='o2_saturation',
            field=models.DateTimeField(null=True, verbose_name='DO82 O2 saturation below 80%', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='other_medication_details',
            field=models.CharField(max_length=250, null=True, verbose_name='DO67 other medication', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='perfusion_started',
            field=models.DateTimeField(null=True, verbose_name='DO86 start in-situ cold perfusion', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='perfusion_technician',
            field=models.ForeignKey(related_name='perfusion_technician_set', verbose_name='DO03 name of transplant technician', to='compare.Person'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='retrieval_hospital',
            field=models.ForeignKey(verbose_name='DO06 donor hospital', blank=True, to='compare.Hospital', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='retrieval_team',
            field=models.ForeignKey(verbose_name='DO01 retrieval team', to='compare.RetrievalTeam'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='scheduled_start',
            field=models.DateTimeField(null=True, verbose_name='DO07 time of withdrawal therapy', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='sequence_number',
            field=models.PositiveSmallIntegerField(default=0, verbose_name='DO02 sequence number'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systemic_flush_used',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO87 systemic (aortic) flush solution used', choices=[(3, b'HTK'), (2, b"Marshall's"), (1, b'UW'), (4, b'Other')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systemic_flush_used_other',
            field=models.CharField(max_length=250, null=True, verbose_name='DO88 systemic flush used', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systemic_flush_volume_used',
            field=models.PositiveSmallIntegerField(null=True, verbose_name='DO89 aortic - volume (ml)', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systolic_blood_pressure',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO59 last systolic blood pressure', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(200)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systolic_pressure_low',
            field=models.DateTimeField(null=True, verbose_name='DO81 systolic arterial pressure', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='technician_arrival',
            field=models.DateTimeField(null=True, verbose_name='DO08 arrival time of technician at hub', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='transplant_coordinator',
            field=models.ForeignKey(related_name='transplant_coordinator_set', verbose_name='DO04 name of the SN-OD', blank=True, to='compare.Person', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='vasopressine',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO66 vasopressine', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='weight',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO39 Weight (kg)', validators=[django.core.validators.MinValueValidator(20), django.core.validators.MaxValueValidator(200)]),
        ),
        migrations.AlterField(
            model_name='hospital',
            name='centre_code',
            field=models.PositiveSmallIntegerField(verbose_name='HO02 centre code', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(99)]),
        ),
        migrations.AlterField(
            model_name='hospital',
            name='country',
            field=models.CharField(max_length=50, verbose_name='HO03 country'),
        ),
        migrations.AlterField(
            model_name='hospital',
            name='name',
            field=models.CharField(max_length=100, verbose_name='HO01 hospital name'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='artificial_patch_number',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OR41 number of patches', validators=[django.core.validators.MinValueValidator(1), django.core.validators.MaxValueValidator(2)]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='artificial_patch_size',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OR40 artificial patch size', choices=[(1, 'OR33 Small'), (2, 'OR34 Large')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='artificial_patch_used',
            field=models.NullBooleanField(verbose_name='OR39 artificial patch used'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='graft_damage',
            field=models.PositiveSmallIntegerField(default=5, verbose_name='OR23 renal graft damage', choices=[(1, 'OR10 Arterial Damage'), (2, 'OR11 Venous Damage'), (3, 'OR12 Ureteral Damage'), (4, 'OR13 Parenchymal Damage'), (6, 'OR14 Other Damage'), (5, 'OR15 None')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='graft_damage_other',
            field=models.CharField(max_length=250, null=True, verbose_name='OR24 other damage done', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='ice_container_replenished',
            field=models.NullBooleanField(verbose_name='OR46 ice container replenished'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='ice_container_replenished_at',
            field=models.DateTimeField(null=True, verbose_name='OR47 ice container replenished at', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='location',
            field=models.CharField(max_length=1, verbose_name='OR03 kidney location', choices=[(b'L', 'OR01 Left'), (b'R', 'OR02 Right')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='not_transplantable_reason',
            field=models.CharField(max_length=250, null=True, verbose_name='OR27 not transplantable because', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_changed',
            field=models.NullBooleanField(verbose_name='OR44 oxygen bottle changed'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_changed_at',
            field=models.DateTimeField(null=True, verbose_name='OR45 oxygen bottle changed at', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_full',
            field=models.NullBooleanField(verbose_name='OR42 is oxygen bottle full'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_open',
            field=models.NullBooleanField(verbose_name='OR43 oxygen bottle opened'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='patch_holder',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OR38 used patch holder', choices=[(1, 'OR30 Small'), (2, 'OR31 Large'), (3, 'OR32 Double Artery')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_1',
            field=models.ForeignKey(related_name='perfusate_1_set', verbose_name='OR60 p1', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_2',
            field=models.ForeignKey(related_name='perfusate_2_set', verbose_name='OR60 p2', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_measurable',
            field=models.NullBooleanField(verbose_name='OR48 perfusate measurable'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_measure',
            field=models.FloatField(null=True, verbose_name='OR49 value pO2', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_file',
            field=models.ForeignKey(verbose_name='OR51 machine file', blank=True, to='compare.PerfusionFile', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_machine',
            field=models.ForeignKey(verbose_name='OR50 perfusion machine', blank=True, to='compare.PerfusionMachine', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_not_possible_because',
            field=models.CharField(max_length=250, null=True, verbose_name='OR36 not possible because', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_possible',
            field=models.NullBooleanField(verbose_name='OR35 machine perfusion possible?'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_started',
            field=models.DateTimeField(null=True, verbose_name='OR37 machine perfusion', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='removal',
            field=models.DateTimeField(null=True, verbose_name='OR21 time out', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='renal_arteries',
            field=models.PositiveSmallIntegerField(null=True, verbose_name='OR22 number of renal arteries', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='transplantable',
            field=models.NullBooleanField(verbose_name='OR26 is transplantable'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='washout_perfusion',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OR25 perfusion characteristics', choices=[(1, 'OR16 Homogenous'), (2, 'OR17 Patchy'), (3, 'OR18 Blue'), (9, 'OR19 Unknown')]),
        ),
        migrations.AlterField(
            model_name='organsoffered',
            name='organ',
            field=models.PositiveSmallIntegerField(verbose_name='OO05 organ name', choices=[(1, 'OO01 Liver'), (2, 'OO02 Lung'), (3, 'OO03 Pancreas'), (4, 'OO04 Tissue')]),
        ),
        migrations.AlterField(
            model_name='perfusionfile',
            name='machine',
            field=models.ForeignKey(verbose_name='PF01 perfusion machine', to='compare.PerfusionMachine'),
        ),
        migrations.AlterField(
            model_name='perfusionmachine',
            name='machine_reference_number',
            field=models.CharField(max_length=50, verbose_name='PM02 machine reference number'),
        ),
        migrations.AlterField(
            model_name='perfusionmachine',
            name='machine_serial_number',
            field=models.CharField(max_length=50, verbose_name='PM01 machine serial number'),
        ),
        migrations.AlterField(
            model_name='person',
            name='first_names',
            field=models.CharField(max_length=50, verbose_name='PE10 first names'),
        ),
        migrations.AlterField(
            model_name='person',
            name='job',
            field=models.CharField(max_length=2, verbose_name='PE12 job', choices=[(b'PT', 'PE01 Perfusion Technician'), (b'TC', 'PE02 Transplant Co-ordinator'), (b'RN', 'PE03 Research Nurse / Follow-up'), (b'NC', 'PE04 National Co-ordinator'), (b'CC', 'PE05 Central Co-ordinator'), (b'BC', 'PE06 Biobank Co-ordinator'), (b'S', 'PE07 Statistician'), (b'SA', 'PE08 Sys-admin')]),
        ),
        migrations.AlterField(
            model_name='person',
            name='last_names',
            field=models.CharField(max_length=50, verbose_name='PE11 last names'),
        ),
        migrations.AlterField(
            model_name='person',
            name='telephone',
            field=models.CharField(max_length=20, verbose_name='PE13 telephone number'),
        ),
        migrations.AlterField(
            model_name='person',
            name='user',
            field=models.OneToOneField(related_name='profile', null=True, blank=True, to=settings.AUTH_USER_MODEL, verbose_name='PE14 related user account'),
        ),
        migrations.AlterField(
            model_name='procurementresource',
            name='expiry_date',
            field=models.DateField(verbose_name='PR13 expiry date'),
        ),
        migrations.AlterField(
            model_name='procurementresource',
            name='lot_number',
            field=models.CharField(max_length=50, verbose_name='PR12 lot number'),
        ),
        migrations.AlterField(
            model_name='procurementresource',
            name='organ',
            field=models.ForeignKey(verbose_name='PR10 related kidney', to='compare.Organ'),
        ),
        migrations.AlterField(
            model_name='procurementresource',
            name='type',
            field=models.CharField(max_length=5, verbose_name='PR11 resource used', choices=[(b'D', 'PR01 Disposables'), (b'C-SM', 'PR02 Extra cannula small (3mm)'), (b'C-LG', 'PR03 Extra cannula large (5mm)'), (b'PH-SM', 'PR04 Extra patch holder small'), (b'PH-LG', 'PR05 Extra patch holder large'), (b'DB-C', 'PR06 Extra double cannula set'), (b'P', 'PR07 Perfusate solution')]),
        ),
        migrations.AlterField(
            model_name='retrievalteam',
            name='based_at',
            field=models.ForeignKey(verbose_name='RT02 base hospital', to='compare.Hospital'),
        ),
        migrations.AlterField(
            model_name='retrievalteam',
            name='name',
            field=models.CharField(max_length=100, verbose_name='RT01 team name'),
        ),
        migrations.AlterField(
            model_name='sample',
            name='barcode',
            field=models.CharField(max_length=20, verbose_name='SA01 barcode number'),
        ),
        migrations.AlterField(
            model_name='sample',
            name='centrifugation',
            field=models.DateTimeField(null=True, verbose_name='SA03 centrifugation', blank=True),
        ),
        migrations.AlterField(
            model_name='sample',
            name='comment',
            field=models.CharField(max_length=2000, null=True, verbose_name='SA04 comment', blank=True),
        ),
        migrations.AlterField(
            model_name='sample',
            name='taken_at',
            field=models.DateTimeField(verbose_name='SA02 date and time taken'),
        ),
    ]
