# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models
import django.utils.timezone
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0001_initial'),
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
    ]

    operations = [
        migrations.CreateModel(
            name='BloodSample',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('version', models.PositiveIntegerField(default=0)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('record_locked', models.BooleanField(default=False)),
                ('barcode', models.CharField(max_length=20, verbose_name='BC01 barcode number', blank=True)),
                ('collected', models.NullBooleanField(verbose_name='DM01 sample collected?')),
                ('notes', models.TextField(verbose_name='DM02 notes', blank=True)),
                ('blood_type', models.PositiveSmallIntegerField(verbose_name='BS02 blood sample type', choices=[(1, 'BSc01 Blood SST'), (2, 'BSc02 Blood EDSA')])),
                ('centrifuged_at', models.DateTimeField(null=True, verbose_name='BS01 centrifuged at', blank=True)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
            options={
                'ordering': ['person', 'event__taken_at'],
                'verbose_name': 'BSm1 blood sample',
                'verbose_name_plural': 'BSm2 blood samples',
            },
        ),
        migrations.CreateModel(
            name='Event',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('version', models.PositiveIntegerField(default=0)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('record_locked', models.BooleanField(default=False)),
                ('type', models.PositiveSmallIntegerField(verbose_name='EV01 sample type', choices=[(1, 'EVc01 Blood'), (2, 'EVc02 Urine'), (3, 'EVc03 Perfusate'), (4, 'EVc04 Tissue')])),
                ('name', models.PositiveSmallIntegerField(verbose_name='EV03 sample process name', choices=[(1, 'EVc05 donor blood 1'), (2, 'EVc06 donor urine 1'), (3, 'EVc07 donor urine 2'), (4, 'EVc08 organ perfusate 1'), (5, 'EVc09 organ perfusate 2'), (6, 'EVc10 organ perfusate 3'), (7, 'EVc11 organ tissue 1'), (8, 'EVc12 recipient blood 1'), (9, 'EVc13 recipient blood 2')])),
                ('taken_at', models.DateTimeField(null=True, verbose_name='EV02 date and time taken', blank=True)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
            options={
                'ordering': ['type', 'name'],
                'verbose_name': 'EVm1 sample event',
                'verbose_name_plural': 'EVm2 sample events',
            },
        ),
        migrations.CreateModel(
            name='PerfusateSample',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('version', models.PositiveIntegerField(default=0)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('record_locked', models.BooleanField(default=False)),
                ('barcode', models.CharField(max_length=20, verbose_name='BC01 barcode number', blank=True)),
                ('collected', models.NullBooleanField(verbose_name='DM01 sample collected?')),
                ('notes', models.TextField(verbose_name='DM02 notes', blank=True)),
                ('centrifuged_at', models.DateTimeField(null=True, verbose_name='PS01 centrifuged at', blank=True)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('event', models.ForeignKey(to='samples.Event')),
                ('organ', models.ForeignKey(verbose_name='PS02 sample from', to='compare.Organ')),
            ],
            options={
                'ordering': ['organ', 'event__taken_at'],
                'verbose_name': 'PSm1 perfusate sample',
                'verbose_name_plural': 'PSm2 perfusate samples',
            },
        ),
        migrations.CreateModel(
            name='TissueSample',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('version', models.PositiveIntegerField(default=0)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('record_locked', models.BooleanField(default=False)),
                ('barcode', models.CharField(max_length=20, verbose_name='BC01 barcode number', blank=True)),
                ('collected', models.NullBooleanField(verbose_name='DM01 sample collected?')),
                ('notes', models.TextField(verbose_name='DM02 notes', blank=True)),
                ('tissue_type', models.CharField(max_length=1, verbose_name='TS02 tissue sample type', choices=[('F', 'TSc01 ReK1F'), ('R', 'TSc02 ReK1R')])),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('event', models.ForeignKey(to='samples.Event')),
                ('organ', models.ForeignKey(verbose_name='TS01 sample from', to='compare.Organ')),
            ],
            options={
                'ordering': ['organ', 'event__taken_at'],
                'verbose_name': 'TSm1 tissue sample',
                'verbose_name_plural': 'TSm2 tissue samples',
            },
        ),
        migrations.CreateModel(
            name='UrineSample',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('version', models.PositiveIntegerField(default=0)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('record_locked', models.BooleanField(default=False)),
                ('barcode', models.CharField(max_length=20, verbose_name='BC01 barcode number', blank=True)),
                ('collected', models.NullBooleanField(verbose_name='DM01 sample collected?')),
                ('notes', models.TextField(verbose_name='DM02 notes', blank=True)),
                ('centrifuged_at', models.DateTimeField(null=True, verbose_name='US01 centrifuged at', blank=True)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('event', models.ForeignKey(to='samples.Event')),
                ('person', models.ForeignKey(verbose_name='US02 sample from', to='compare.OrganPerson')),
            ],
            options={
                'ordering': ['person', 'event__taken_at'],
                'verbose_name': 'USm1 urine sample',
                'verbose_name_plural': 'USm2 urine samples',
            },
        ),
        migrations.CreateModel(
            name='Worksheet',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('version', models.PositiveIntegerField(default=0)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('record_locked', models.BooleanField(default=False)),
                ('barcode', models.CharField(max_length=20, verbose_name='BC01 barcode number', blank=True)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('person', models.ForeignKey(to='compare.OrganPerson')),
            ],
            options={
                'ordering': ['person'],
                'verbose_name': 'WSm1 worksheet',
                'verbose_name_plural': 'WSm2 worksheets',
            },
        ),
        migrations.AddField(
            model_name='event',
            name='worksheet',
            field=models.ForeignKey(to='samples.Worksheet'),
        ),
        migrations.AddField(
            model_name='bloodsample',
            name='event',
            field=models.ForeignKey(to='samples.Event'),
        ),
        migrations.AddField(
            model_name='bloodsample',
            name='person',
            field=models.ForeignKey(verbose_name='BS03 sample from', to='compare.OrganPerson'),
        ),
    ]
