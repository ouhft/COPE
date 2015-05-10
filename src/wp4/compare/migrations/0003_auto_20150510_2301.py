# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0002_auto_20150510_2127'),
    ]

    operations = [
        migrations.AlterField(
            model_name='donor',
            name='alcohol_abuse',
            field=models.PositiveSmallIntegerField(blank=True, null=True, choices=[(0, b'No'), (1, b'Yes'), (2, b'Unknown')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='arrival_at_donor_hospital',
            field=models.DateTimeField(verbose_name=b'arrival at donor hospital'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='call_received',
            field=models.DateTimeField(verbose_name=b'transplant co-ordinator received call at'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='cardiac_arrest',
            field=models.NullBooleanField(verbose_name=b'Cardiac Arrest (During ITU stay, prior to Retrieval Procedure)'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_of_admission',
            field=models.DateField(verbose_name=b'date of admission into hospital'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='depart_perfusion_centre',
            field=models.DateTimeField(verbose_name=b'departure from hub at'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diabetes_melitus',
            field=models.PositiveSmallIntegerField(blank=True, null=True, choices=[(0, b'No'), (1, b'Yes'), (2, b'Unknown')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diuresis_last_day',
            field=models.PositiveSmallIntegerField(null=True, verbose_name=b'Diuresis last day (ml)', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diuresis_last_day_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diuresis_last_hour',
            field=models.PositiveSmallIntegerField(null=True, verbose_name=b'Diuresis last hour (ml', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diuresis_last_hour_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AlterField(
            model_name='donor',
            name='dobutamine',
            field=models.PositiveSmallIntegerField(blank=True, null=True, choices=[(0, b'No'), (1, b'Yes'), (2, b'Unknown')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_blood_1_EDTA',
            field=models.ForeignKey(related_name='donor_blood_1_EDTA_set', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_blood_1_SST',
            field=models.ForeignKey(related_name='donor_blood_1_SST_set', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_urine_1',
            field=models.ForeignKey(related_name='donor_urine_1_set', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_urine_2',
            field=models.ForeignKey(related_name='donor_urine_2_set', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='dopamine',
            field=models.PositiveSmallIntegerField(blank=True, null=True, choices=[(0, b'No'), (1, b'Yes'), (2, b'Unknown')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='ice_boxes_filled',
            field=models.DateTimeField(verbose_name=b'ice boxes filled with sufficient amount of ice (for kidney assist)'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='nor_adrenaline',
            field=models.PositiveSmallIntegerField(blank=True, null=True, choices=[(0, b'No'), (1, b'Yes'), (2, b'Unknown')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='other_medication_details',
            field=models.CharField(max_length=250, null=True, blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='perfusion_technician',
            field=models.ForeignKey(related_name='perfusion_technician_set', verbose_name=b'name of transplant technician', to='compare.Person'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='scheduled_start',
            field=models.DateTimeField(verbose_name=b'scheduled time of withdrawal therapy'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='technician_arrival',
            field=models.DateTimeField(verbose_name=b'arrival time of technician at hub'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='transplant_coordinator',
            field=models.ForeignKey(related_name='transplant_coordinator_set', verbose_name=b'name of transplant co-ordinator', blank=True, to='compare.Person', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='vasopressine',
            field=models.PositiveSmallIntegerField(blank=True, null=True, choices=[(0, b'No'), (1, b'Yes'), (2, b'Unknown')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='version',
            field=models.PositiveIntegerField(default=0),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_1',
            field=models.ForeignKey(related_name='perfusate_1_set', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_2',
            field=models.ForeignKey(related_name='perfusate_2_set', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_file',
            field=models.ForeignKey(blank=True, to='compare.PerfusionFile', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='version',
            field=models.PositiveIntegerField(default=0),
        ),
        migrations.AlterField(
            model_name='person',
            name='user',
            field=models.OneToOneField(related_name='people_set', null=True, blank=True, to=settings.AUTH_USER_MODEL),
        ),
        migrations.AlterField(
            model_name='person',
            name='version',
            field=models.PositiveIntegerField(default=0),
        ),
        migrations.AlterField(
            model_name='sample',
            name='comment',
            field=models.CharField(max_length=2000, null=True),
        ),
    ]
