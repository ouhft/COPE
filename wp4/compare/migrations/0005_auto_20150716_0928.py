# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
import django.core.validators


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0004_auto_20150703_1654'),
    ]

    operations = [
        migrations.AlterModelOptions(
            name='donor',
            options={'ordering': ['sequence_number'], 'verbose_name': 'donor', 'verbose_name_plural': 'donors'},
        ),
        migrations.AlterModelOptions(
            name='hospital',
            options={'ordering': ['centre_code'], 'verbose_name': 'hospital', 'verbose_name_plural': 'hospitals'},
        ),
        migrations.AlterModelOptions(
            name='organ',
            options={'verbose_name': 'organ', 'verbose_name_plural': 'organs'},
        ),
        migrations.AlterModelOptions(
            name='organsoffered',
            options={'verbose_name': 'organ offered', 'verbose_name_plural': 'organs offered'},
        ),
        migrations.AlterModelOptions(
            name='perfusionfile',
            options={'verbose_name': 'perfusion machine file', 'verbose_name_plural': 'perfusion machine files'},
        ),
        migrations.AlterModelOptions(
            name='perfusionmachine',
            options={'verbose_name': 'perfusion machine', 'verbose_name_plural': 'perfusion machines'},
        ),
        migrations.AlterModelOptions(
            name='person',
            options={'verbose_name': 'person', 'verbose_name_plural': 'people'},
        ),
        migrations.AlterModelOptions(
            name='procurementresource',
            options={'verbose_name': 'procurement resource', 'verbose_name_plural': 'procurement resources'},
        ),
        migrations.AlterModelOptions(
            name='retrievalteam',
            options={'verbose_name': 'retrieval team', 'verbose_name_plural': 'retrieval teams'},
        ),
        migrations.AlterModelOptions(
            name='sample',
            options={'ordering': ['taken_at'], 'verbose_name': 'sample', 'verbose_name_plural': 'samples'},
        ),
        migrations.AlterField(
            model_name='donor',
            name='admitted_to_itu',
            field=models.NullBooleanField(verbose_name='admitted to ITU'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='age',
            field=models.PositiveSmallIntegerField(verbose_name='age', validators=[django.core.validators.MinValueValidator(50), django.core.validators.MaxValueValidator(99)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='alcohol_abuse',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='alcohol abuse', choices=[(0, 'No'), (1, 'Yes'), (2, 'Unknown')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='blood_group',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='blood group', choices=[(1, b'O'), (2, b'A'), (3, b'B'), (4, b'AB'), (5, 'Unknown')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='cardiac_arrest',
            field=models.NullBooleanField(verbose_name='cardiac arrest'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='circulatory_arrest',
            field=models.DateTimeField(null=True, verbose_name='end of cardiac output', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_of_birth',
            field=models.DateField(null=True, verbose_name='date of birth', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diabetes_melitus',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='diabetes mellitus', choices=[(0, 'No'), (1, 'Yes'), (2, 'Unknown')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diagnosis',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='diagnosis', choices=[(1, b'Cerebrivascular Accident'), (2, b'Hypoxia'), (3, b'Trauma'), (4, b'Other')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diagnosis_other',
            field=models.CharField(max_length=250, null=True, verbose_name='other diagnosis', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diastolic_blood_pressure',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='last diastolic blood pressure', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(200)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diuresis_last_day',
            field=models.PositiveSmallIntegerField(null=True, verbose_name='diuresis last day (ml)', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diuresis_last_hour',
            field=models.PositiveSmallIntegerField(null=True, verbose_name='diuresis last hour (ml)', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='dobutamine',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='dobutamine', choices=[(0, 'No'), (1, 'Yes'), (2, 'Unknown')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_blood_1_EDTA',
            field=models.ForeignKey(related_name='donor_blood_1_EDTA_set', verbose_name='db 1.1 edta', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_blood_1_SST',
            field=models.ForeignKey(related_name='donor_blood_1_SST_set', verbose_name='db 1.2 sst', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_urine_1',
            field=models.ForeignKey(related_name='donor_urine_1_set', verbose_name='du 1', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_urine_2',
            field=models.ForeignKey(related_name='donor_urine_2_set', verbose_name='du 2', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='dopamine',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='dopamine', choices=[(0, 'No'), (1, 'Yes'), (2, 'Unknown')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='ethnicity',
            field=models.IntegerField(blank=True, null=True, verbose_name='ethnicity', choices=[(1, 'Caucasian'), (2, 'Black'), (3, 'Other')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='gender',
            field=models.CharField(max_length=1, verbose_name='gender', choices=[(b'M', 'Male'), (b'F', 'Female')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='last_creatinine',
            field=models.FloatField(null=True, verbose_name='last creatinine', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='max_creatinine',
            field=models.FloatField(null=True, verbose_name='max creatinine', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='nor_adrenaline',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='nor adrenaline', choices=[(0, 'No'), (1, 'Yes'), (2, 'Unknown')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='other_medication_details',
            field=models.CharField(max_length=250, null=True, verbose_name='other medication', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='retrieval_team',
            field=models.ForeignKey(verbose_name='retrieval team', to='compare.RetrievalTeam'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='sequence_number',
            field=models.PositiveSmallIntegerField(default=0, verbose_name='sequence number'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systemic_flush_used_other',
            field=models.CharField(max_length=250, null=True, verbose_name='systemic flush used', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systolic_blood_pressure',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='last systolic blood pressure', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(200)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systolic_pressure_low',
            field=models.DateTimeField(null=True, verbose_name='systolic arterial pressure', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='vasopressine',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='vasopressine', choices=[(0, 'No'), (1, 'Yes'), (2, 'Unknown')]),
        ),
        migrations.AlterField(
            model_name='hospital',
            name='centre_code',
            field=models.PositiveSmallIntegerField(verbose_name='centre code', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(99)]),
        ),
        migrations.AlterField(
            model_name='hospital',
            name='country',
            field=models.CharField(max_length=50, verbose_name='country'),
        ),
        migrations.AlterField(
            model_name='hospital',
            name='name',
            field=models.CharField(max_length=100, verbose_name='hospital name'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='artificial_patch_number',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='number of patches', validators=[django.core.validators.MinValueValidator(1), django.core.validators.MaxValueValidator(2)]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='artificial_patch_size',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='artificial patch size', choices=[(1, 'Small'), (2, 'Large')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='artificial_patch_used',
            field=models.NullBooleanField(verbose_name='artificial patch used'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='graft_damage',
            field=models.PositiveSmallIntegerField(default=5, verbose_name='renal graft damage', choices=[(1, 'Arterial Damage'), (2, 'Venous Damage'), (3, 'Ureteral Damage'), (4, 'Parenchymal Damage'), (6, 'Other Damage'), (5, 'None')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='graft_damage_other',
            field=models.CharField(max_length=250, null=True, verbose_name='other damage done', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='ice_container_replenished',
            field=models.NullBooleanField(verbose_name='ice container replenished'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='ice_container_replenished_at',
            field=models.DateTimeField(null=True, verbose_name='ice container replenished at', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='location',
            field=models.CharField(max_length=1, verbose_name='kidney location', choices=[(b'L', 'Left'), (b'R', 'Right')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='not_transplantable_reason',
            field=models.CharField(max_length=250, null=True, verbose_name='not transplantable because', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_changed',
            field=models.NullBooleanField(verbose_name='oxygen bottle changed'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_changed_at',
            field=models.DateTimeField(null=True, verbose_name='oxygen bottle changed at', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_full',
            field=models.NullBooleanField(verbose_name='is oxygen bottle full'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_open',
            field=models.NullBooleanField(verbose_name='oxygen bottle opened'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='patch_holder',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='used patch holder', choices=[(1, 'Small'), (2, 'Large'), (3, 'Double Artery')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_1',
            field=models.ForeignKey(related_name='perfusate_1_set', verbose_name='p1', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_2',
            field=models.ForeignKey(related_name='perfusate_2_set', verbose_name='p2', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_measurable',
            field=models.NullBooleanField(verbose_name='perfusate measurable'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_measure',
            field=models.FloatField(null=True, verbose_name='value pO2', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_file',
            field=models.ForeignKey(verbose_name='machine file', blank=True, to='compare.PerfusionFile', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_machine',
            field=models.ForeignKey(verbose_name='perfusion machine', blank=True, to='compare.PerfusionMachine', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_not_possible_because',
            field=models.CharField(max_length=250, null=True, verbose_name='not possible because', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='renal_arteries',
            field=models.PositiveSmallIntegerField(null=True, verbose_name='number of renal arteries', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='transplantable',
            field=models.NullBooleanField(verbose_name='is transplantable'),
        ),
        migrations.AlterField(
            model_name='organsoffered',
            name='organ',
            field=models.PositiveSmallIntegerField(verbose_name='organ name', choices=[(1, 'Liver'), (2, 'Lung'), (3, 'Pancreas'), (4, 'Tissue')]),
        ),
        migrations.AlterField(
            model_name='perfusionfile',
            name='machine',
            field=models.ForeignKey(verbose_name='perfusion machine', to='compare.PerfusionMachine'),
        ),
        migrations.AlterField(
            model_name='perfusionmachine',
            name='machine_reference_number',
            field=models.CharField(max_length=50, verbose_name='machine reference number'),
        ),
        migrations.AlterField(
            model_name='perfusionmachine',
            name='machine_serial_number',
            field=models.CharField(max_length=50, verbose_name='machine serial number'),
        ),
        migrations.AlterField(
            model_name='procurementresource',
            name='expiry_date',
            field=models.DateField(verbose_name='expiry date'),
        ),
        migrations.AlterField(
            model_name='procurementresource',
            name='lot_number',
            field=models.CharField(max_length=50, verbose_name='lot number'),
        ),
        migrations.AlterField(
            model_name='procurementresource',
            name='organ',
            field=models.ForeignKey(verbose_name='related kidney', to='compare.Organ'),
        ),
        migrations.AlterField(
            model_name='procurementresource',
            name='type',
            field=models.CharField(max_length=5, verbose_name='resource used', choices=[(b'D', 'Disposables'), (b'C-SM', 'Extra cannula small (3mm)'), (b'C-LG', 'Extra cannula large (5mm)'), (b'PH-SM', 'Extra patch holder small'), (b'PH-LG', 'Extra patch holder large'), (b'DB-C', 'Extra double cannula set'), (b'P', 'Perfusate solution')]),
        ),
        migrations.AlterField(
            model_name='retrievalteam',
            name='based_at',
            field=models.ForeignKey(verbose_name='base hospital', to='compare.Hospital'),
        ),
        migrations.AlterField(
            model_name='retrievalteam',
            name='name',
            field=models.CharField(max_length=100, verbose_name='team name'),
        ),
        migrations.AlterField(
            model_name='sample',
            name='barcode',
            field=models.CharField(max_length=20, verbose_name='barcode number'),
        ),
        migrations.AlterField(
            model_name='sample',
            name='centrifugation',
            field=models.DateTimeField(null=True, verbose_name='centrifugation', blank=True),
        ),
        migrations.AlterField(
            model_name='sample',
            name='comment',
            field=models.CharField(max_length=2000, null=True, verbose_name='comment', blank=True),
        ),
        migrations.AlterField(
            model_name='sample',
            name='taken_at',
            field=models.DateTimeField(verbose_name='date and time taken'),
        ),
    ]
