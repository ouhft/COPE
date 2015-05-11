# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
import django.utils.timezone
from django.conf import settings
import django.core.validators


class Migration(migrations.Migration):

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
    ]

    operations = [
        migrations.CreateModel(
            name='Donor',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('version', models.PositiveIntegerField(default=0)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('sequence_number', models.PositiveSmallIntegerField(default=0)),
                ('call_received', models.DateTimeField(verbose_name=b'transplant co-ordinator received call at')),
                ('scheduled_start', models.DateTimeField(verbose_name=b'scheduled time of withdrawal therapy')),
                ('technician_arrival', models.DateTimeField(verbose_name=b'arrival time of technician at hub')),
                ('ice_boxes_filled', models.DateTimeField(verbose_name=b'ice boxes filled with sufficient amount of ice (for kidney assist)')),
                ('depart_perfusion_centre', models.DateTimeField(verbose_name=b'departure from hub at')),
                ('arrival_at_donor_hospital', models.DateTimeField(verbose_name=b'arrival at donor hospital')),
                ('number', models.CharField(max_length=20, verbose_name=b'ET Donor number/ NHSBT Number')),
                ('age', models.PositiveSmallIntegerField(validators=[django.core.validators.MinValueValidator(50), django.core.validators.MaxValueValidator(99)])),
                ('date_of_birth', models.DateField(null=True, blank=True)),
                ('date_of_admission', models.DateField(verbose_name=b'date of admission into hospital')),
                ('admitted_to_itu', models.BooleanField()),
                ('date_admitted_to_itu', models.DateField()),
                ('date_of_procurement', models.DateField()),
                ('gender', models.CharField(max_length=1, choices=[(b'M', b'Male'), (b'F', b'Female')])),
                ('weight', models.PositiveSmallIntegerField(verbose_name=b'Weight (kg)', validators=[django.core.validators.MinValueValidator(20), django.core.validators.MaxValueValidator(200)])),
                ('height', models.PositiveSmallIntegerField(verbose_name=b'Height (cm)', validators=[django.core.validators.MinValueValidator(100), django.core.validators.MaxValueValidator(250)])),
                ('ethnicity', models.IntegerField(choices=[(1, b'Caucasian'), (2, b'Black'), (3, b'Other')])),
                ('blood_group', models.PositiveSmallIntegerField(choices=[(1, b'O'), (2, b'A'), (3, b'B'), (4, b'AB'), (5, b'Unknown')])),
                ('diagnosis', models.PositiveSmallIntegerField(choices=[(1, b'Cerebrivascular Accident'), (2, b'Hypoxia'), (3, b'Trauma'), (4, b'Other')])),
                ('diagnosis_other', models.CharField(max_length=250, blank=True)),
                ('diabetes_melitus', models.PositiveSmallIntegerField(blank=True, null=True, choices=[(0, b'No'), (1, b'Yes'), (2, b'Unknown')])),
                ('alcohol_abuse', models.PositiveSmallIntegerField(blank=True, null=True, choices=[(0, b'No'), (1, b'Yes'), (2, b'Unknown')])),
                ('cardiac_arrest', models.NullBooleanField(verbose_name=b'Cardiac Arrest (During ITU stay, prior to Retrieval Procedure)')),
                ('systolic_blood_pressure', models.PositiveSmallIntegerField(verbose_name=b'Last Systolic Blood Pressure (Before switch off)', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(200)])),
                ('diastolic_blood_pressure', models.PositiveSmallIntegerField(verbose_name=b'Last Diastolic Blood Pressure (Before switch off)', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(200)])),
                ('hypotensive', models.NullBooleanField()),
                ('diuresis_last_day', models.PositiveSmallIntegerField(null=True, verbose_name=b'Diuresis last day (ml)', blank=True)),
                ('diuresis_last_day_unknown', models.BooleanField(default=False)),
                ('diuresis_last_hour', models.PositiveSmallIntegerField(null=True, verbose_name=b'Diuresis last hour (ml', blank=True)),
                ('diuresis_last_hour_unknown', models.BooleanField(default=False)),
                ('dopamine', models.PositiveSmallIntegerField(blank=True, null=True, choices=[(0, b'No'), (1, b'Yes'), (2, b'Unknown')])),
                ('dobutamine', models.PositiveSmallIntegerField(blank=True, null=True, choices=[(0, b'No'), (1, b'Yes'), (2, b'Unknown')])),
                ('nor_adrenaline', models.PositiveSmallIntegerField(blank=True, null=True, choices=[(0, b'No'), (1, b'Yes'), (2, b'Unknown')])),
                ('vasopressine', models.PositiveSmallIntegerField(blank=True, null=True, choices=[(0, b'No'), (1, b'Yes'), (2, b'Unknown')])),
                ('other_medication_details', models.CharField(max_length=250, null=True, blank=True)),
                ('last_creatinine', models.FloatField(null=True, blank=True)),
                ('last_creatinine_unit', models.PositiveSmallIntegerField(default=1, choices=[(1, b'mg/dl'), (2, b'umol/L')])),
                ('max_creatinine', models.FloatField(null=True, blank=True)),
                ('max_creatinine_unit', models.PositiveSmallIntegerField(default=1, choices=[(1, b'mg/dl'), (2, b'umol/L')])),
                ('life_support_withdrawal', models.DateTimeField(null=True, verbose_name=b'withdrawal of life support', blank=True)),
                ('systolic_pressure_low', models.DateTimeField(null=True, verbose_name=b'systolic arterial pressure < 50 mm Hg (inadequate organ perfusion)', blank=True)),
                ('circulatory_arrest', models.DateTimeField(null=True, verbose_name=b'end of cardiac output (=start of no touch period)', blank=True)),
                ('length_of_no_touch', models.PositiveSmallIntegerField(null=True, verbose_name=b'length of no touch period (minutes)', blank=True)),
                ('death_diagnosed', models.DateTimeField(null=True, verbose_name=b'diagnosis of death', blank=True)),
                ('perfusion_started', models.DateTimeField(null=True, verbose_name=b'start in-situ cold perfusion', blank=True)),
                ('systemic_flush_used', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name=b'systemic (aortic) flush solution used', choices=[(3, b'HTK'), (2, b"Marshall's"), (1, b'UW'), (4, b'Other')])),
                ('systemic_flush_used_other', models.CharField(max_length=250, null=True, blank=True)),
                ('heparin', models.NullBooleanField(verbose_name=b'heparin (administered to donor/in flush solution)')),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
            options={
                'ordering': ['sequence_number'],
            },
        ),
        migrations.CreateModel(
            name='Hospital',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('name', models.CharField(max_length=100)),
                ('centre_code', models.PositiveSmallIntegerField(validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(99)])),
                ('country', models.CharField(max_length=50)),
                ('is_active', models.BooleanField(default=True)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
            options={
                'ordering': ['centre_code'],
            },
        ),
        migrations.CreateModel(
            name='Organ',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('version', models.PositiveIntegerField(default=0)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('location', models.CharField(max_length=1, choices=[(b'L', b'Left'), (b'R', b'Right')])),
                ('removal', models.DateTimeField()),
                ('renal_arteries', models.PositiveSmallIntegerField()),
                ('graft_damage', models.PositiveSmallIntegerField(default=5, choices=[(1, b'Arterial Damage'), (2, b'Venous Damage'), (3, b'Ureteral Damage'), (4, b'Parenchymal Damage'), (5, b'None')])),
                ('washout_perfusion', models.PositiveSmallIntegerField(choices=[(1, b'Homogenous'), (2, b'Patchy'), (3, b'Blue'), (9, b'Unknown')])),
                ('transplantable', models.BooleanField()),
                ('not_transplantable_reason', models.CharField(max_length=250, null=True, blank=True)),
                ('preservation', models.PositiveSmallIntegerField(choices=[(0, b'HMP'), (1, b'HMP O2')])),
                ('perfusion_possible', models.BooleanField()),
                ('perfusion_not_possible_because', models.CharField(max_length=250, null=True, blank=True)),
                ('perfusion_started', models.DateTimeField()),
                ('patch_holder', models.PositiveSmallIntegerField(choices=[(1, b'Small'), (2, b'Large'), (3, b'Double Artery')])),
                ('artificial_patch_used', models.BooleanField()),
                ('artificial_patch_size', models.PositiveSmallIntegerField(blank=True, null=True, choices=[(1, b'Small'), (2, b'Large')])),
                ('artificial_patch_number', models.PositiveSmallIntegerField(blank=True, null=True, validators=[django.core.validators.MinValueValidator(1), django.core.validators.MaxValueValidator(2)])),
                ('oxygen_bottle_full', models.BooleanField()),
                ('oxygen_bottle_open', models.BooleanField()),
                ('oxygen_bottle_changed', models.BooleanField()),
                ('oxygen_bottle_changed_at', models.DateTimeField(null=True, blank=True)),
                ('ice_container_replenished', models.BooleanField()),
                ('ice_container_replenished_at', models.DateTimeField(null=True, blank=True)),
                ('perfusate_measurable', models.BooleanField()),
                ('perfusate_measure', models.FloatField(null=True, blank=True)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('donor', models.ForeignKey(to='compare.Donor')),
            ],
            options={
                'abstract': False,
            },
        ),
        migrations.CreateModel(
            name='OrgansOffered',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('organ', models.PositiveSmallIntegerField(choices=[(1, b'Liver'), (2, b'Lung'), (3, b'Pancreas'), (4, b'Tissue')])),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('donor', models.ForeignKey(to='compare.Donor')),
            ],
            options={
                'verbose_name_plural': 'organs offered',
            },
        ),
        migrations.CreateModel(
            name='PerfusionFile',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('file', models.FileField(upload_to=b'')),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
        ),
        migrations.CreateModel(
            name='PerfusionMachine',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('machine_serial_number', models.CharField(max_length=50)),
                ('machine_reference_number', models.CharField(max_length=50)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
        ),
        migrations.CreateModel(
            name='Person',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('version', models.PositiveIntegerField(default=0)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('first_names', models.CharField(max_length=50)),
                ('last_names', models.CharField(max_length=50)),
                ('job', models.CharField(max_length=2, choices=[(b'PT', b'Perfusion Technician'), (b'TC', b'Transplant Co-ordinator'), (b'RN', b'Research Nurse / Follow-up'), (b'NC', b'National Co-ordinator'), (b'CC', b'Central Co-ordinator'), (b'BC', b'Biobank Co-ordinator'), (b'S', b'Statistician'), (b'SA', b'Sys-admin')])),
                ('telephone', models.CharField(max_length=20)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('user', models.OneToOneField(related_name='people_set', null=True, blank=True, to=settings.AUTH_USER_MODEL)),
            ],
            options={
                'verbose_name_plural': 'people',
            },
        ),
        migrations.CreateModel(
            name='ProcurementResource',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('type', models.CharField(max_length=5, choices=[(b'D', b'Disposables'), (b'C-SM', b'Extra cannula small (3mm)'), (b'C-LG', b'Extra cannula large (5mm)'), (b'PH-SM', b'Extra patch holder small'), (b'PH-LG', b'Extra patch holder large'), (b'DB-C', b'Extra double cannula set'), (b'P', b'Perfusate solution')])),
                ('lot_number', models.CharField(max_length=50)),
                ('expiry_date', models.DateField()),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('organ', models.ForeignKey(to='compare.Organ')),
            ],
        ),
        migrations.CreateModel(
            name='RetrievalTeam',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('name', models.CharField(max_length=100)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('based_at', models.ForeignKey(to='compare.Hospital')),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
        ),
        migrations.CreateModel(
            name='Sample',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('barcode', models.CharField(max_length=20)),
                ('taken_at', models.DateTimeField()),
                ('centrifugation', models.DateTimeField(null=True)),
                ('comment', models.CharField(max_length=2000, null=True)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
        ),
        migrations.AddField(
            model_name='perfusionfile',
            name='machine',
            field=models.ForeignKey(to='compare.PerfusionMachine'),
        ),
        migrations.AddField(
            model_name='organ',
            name='perfusate_1',
            field=models.ForeignKey(related_name='perfusate_1_set', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AddField(
            model_name='organ',
            name='perfusate_2',
            field=models.ForeignKey(related_name='perfusate_2_set', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AddField(
            model_name='organ',
            name='perfusion_file',
            field=models.ForeignKey(blank=True, to='compare.PerfusionFile', null=True),
        ),
        migrations.AddField(
            model_name='organ',
            name='perfusion_machine',
            field=models.ForeignKey(to='compare.PerfusionMachine'),
        ),
        migrations.AddField(
            model_name='donor',
            name='donor_blood_1_EDTA',
            field=models.ForeignKey(related_name='donor_blood_1_EDTA_set', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AddField(
            model_name='donor',
            name='donor_blood_1_SST',
            field=models.ForeignKey(related_name='donor_blood_1_SST_set', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AddField(
            model_name='donor',
            name='donor_urine_1',
            field=models.ForeignKey(related_name='donor_urine_1_set', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AddField(
            model_name='donor',
            name='donor_urine_2',
            field=models.ForeignKey(related_name='donor_urine_2_set', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AddField(
            model_name='donor',
            name='perfusion_technician',
            field=models.ForeignKey(related_name='perfusion_technician_set', verbose_name=b'name of transplant technician', to='compare.Person'),
        ),
        migrations.AddField(
            model_name='donor',
            name='retrieval_hospital',
            field=models.ForeignKey(to='compare.Hospital'),
        ),
        migrations.AddField(
            model_name='donor',
            name='retrieval_team',
            field=models.ForeignKey(to='compare.RetrievalTeam'),
        ),
        migrations.AddField(
            model_name='donor',
            name='transplant_coordinator',
            field=models.ForeignKey(related_name='transplant_coordinator_set', verbose_name=b'name of transplant co-ordinator', blank=True, to='compare.Person', null=True),
        ),
        migrations.AlterOrderWithRespectTo(
            name='retrievalteam',
            order_with_respect_to='based_at',
        ),
        migrations.AlterOrderWithRespectTo(
            name='donor',
            order_with_respect_to='retrieval_team',
        ),
    ]
