# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
import django.core.validators


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0001_initial'),
    ]

    operations = [
        migrations.AlterField(
            model_name='donor',
            name='admitted_to_itu',
            field=models.NullBooleanField(verbose_name=b'admitted to ITU?'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='arrival_at_donor_hospital',
            field=models.DateTimeField(null=True, verbose_name=b'arrival at donor hospital', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='blood_group',
            field=models.PositiveSmallIntegerField(blank=True, null=True, choices=[(1, b'O'), (2, b'A'), (3, b'B'), (4, b'AB'), (5, b'Unknown')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='call_received',
            field=models.DateTimeField(null=True, verbose_name=b'transplant co-ordinator received call at', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_admitted_to_itu',
            field=models.DateField(null=True, verbose_name=b'when admitted to ITU', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_of_admission',
            field=models.DateField(null=True, verbose_name=b'date of admission into hospital', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_of_procurement',
            field=models.DateField(null=True, verbose_name=b'date of procurement', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='depart_perfusion_centre',
            field=models.DateTimeField(null=True, verbose_name=b'departure from hub at', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diagnosis',
            field=models.PositiveSmallIntegerField(blank=True, null=True, choices=[(1, b'Cerebrivascular Accident'), (2, b'Hypoxia'), (3, b'Trauma'), (4, b'Other')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diagnosis_other',
            field=models.CharField(max_length=250, null=True, blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diastolic_blood_pressure',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name=b'Last Diastolic Blood Pressure (Before switch off)', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(200)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='ethnicity',
            field=models.IntegerField(blank=True, null=True, choices=[(1, b'Caucasian'), (2, b'Black'), (3, b'Other')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='gender',
            field=models.CharField(blank=True, max_length=1, null=True, choices=[(b'M', b'Male'), (b'F', b'Female')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='height',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name=b'Height (cm)', validators=[django.core.validators.MinValueValidator(100), django.core.validators.MaxValueValidator(250)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='hypotensive',
            field=models.NullBooleanField(verbose_name=b'hypotensive'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='ice_boxes_filled',
            field=models.DateTimeField(null=True, verbose_name=b'ice boxes filled with sufficient amount of ice (for kidney assist)', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='retrieval_hospital',
            field=models.ForeignKey(blank=True, to='compare.Hospital', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='scheduled_start',
            field=models.DateTimeField(null=True, verbose_name=b'scheduled time of withdrawal therapy', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systolic_blood_pressure',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name=b'Last Systolic Blood Pressure (Before switch off)', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(200)]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='technician_arrival',
            field=models.DateTimeField(null=True, verbose_name=b'arrival time of technician at hub', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='weight',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name=b'Weight (kg)', validators=[django.core.validators.MinValueValidator(20), django.core.validators.MaxValueValidator(200)]),
        ),
        migrations.AlterField(
            model_name='sample',
            name='centrifugation',
            field=models.DateTimeField(null=True, blank=True),
        ),
        migrations.AlterField(
            model_name='sample',
            name='comment',
            field=models.CharField(max_length=2000, null=True, blank=True),
        ),
    ]
