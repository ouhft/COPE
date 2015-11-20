# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models
import django.utils.timezone
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0002_auto_20151116_1301'),
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
    ]

    operations = [
        migrations.CreateModel(
            name='BloodSample',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('barcode', models.CharField(max_length=20, verbose_name='SA01 barcode number')),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('collected', models.NullBooleanField(verbose_name='WM01 sample collected?')),
                ('notes', models.TextField(verbose_name='WM02 notes', blank=True)),
                ('centrifuged_at', models.DateTimeField(null=True, verbose_name='SA02 centrifuged at', blank=True)),
                ('blood_type', models.PositiveSmallIntegerField(verbose_name='BS02 blood sample type', choices=[(1, 'BSc01 Blood SST'), (2, 'BSc02 Blood EDSA')])),
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
                ('type', models.PositiveSmallIntegerField(verbose_name='EV01 sample type', choices=[(1, 'EVc01 Blood'), (2, 'EVc02 Urine'), (3, 'EVc03 Perfusate'), (4, 'EVc04 Tissue')])),
                ('taken_at', models.DateTimeField(verbose_name='EV02 date and time taken')),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
            options={
                'ordering': ['taken_at'],
                'verbose_name': 'EVm1 sample event',
                'verbose_name_plural': 'EVm2 sample events',
            },
        ),
        migrations.CreateModel(
            name='PerfusateSample',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('barcode', models.CharField(max_length=20, verbose_name='SA01 barcode number')),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('collected', models.NullBooleanField(verbose_name='WM01 sample collected?')),
                ('notes', models.TextField(verbose_name='WM02 notes', blank=True)),
                ('centrifuged_at', models.DateTimeField(null=True, verbose_name='SA02 centrifuged at', blank=True)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('event', models.ForeignKey(to='samples.Event')),
                ('organ', models.ForeignKey(to='compare.Organ')),
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
                ('barcode', models.CharField(max_length=20, verbose_name='SA01 barcode number')),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('collected', models.NullBooleanField(verbose_name='WM01 sample collected?')),
                ('notes', models.TextField(verbose_name='WM02 notes', blank=True)),
                ('tissue_type', models.CharField(max_length=1, choices=[(b'F', 'TSc01 ReK1F'), (b'R', 'TSc02 ReK1R')])),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('event', models.ForeignKey(to='samples.Event')),
                ('organ', models.ForeignKey(to='compare.Organ')),
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
                ('barcode', models.CharField(max_length=20, verbose_name='SA01 barcode number')),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('collected', models.NullBooleanField(verbose_name='WM01 sample collected?')),
                ('notes', models.TextField(verbose_name='WM02 notes', blank=True)),
                ('centrifuged_at', models.DateTimeField(null=True, verbose_name='SA02 centrifuged at', blank=True)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('event', models.ForeignKey(to='samples.Event')),
                ('person', models.ForeignKey(to='compare.OrganPerson')),
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
                ('barcode', models.CharField(max_length=20, verbose_name='SA01 barcode number')),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
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
            field=models.ForeignKey(to='compare.OrganPerson'),
        ),
    ]
