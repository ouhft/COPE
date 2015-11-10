# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models
import django.utils.timezone
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
        ('compare', '0001_initial'),
    ]

    operations = [
        migrations.CreateModel(
            name='BloodSample',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('barcode', models.CharField(max_length=20, verbose_name='SA01 barcode number')),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('centrifuged_at', models.DateTimeField(null=True, verbose_name='SA02 centrifuged at', blank=True)),
                ('blood_type', models.PositiveSmallIntegerField(verbose_name='BS02 blood sample type', choices=[(1, 'BSc01 Blood SST'), (2, 'BSc02 Blood EDSA')])),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
            options={
                'abstract': False,
            },
        ),
        migrations.CreateModel(
            name='Deviation',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('description', models.TextField()),
                ('occurred_at', models.DateTimeField()),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
        ),
        migrations.CreateModel(
            name='Event',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('type', models.PositiveSmallIntegerField(verbose_name='EV01 event type', choices=[(1, 'EVc01 Blood'), (2, 'EVc02 Urine'), (3, 'EVc03 Perfusate'), (4, 'EVc04 Tissue')])),
                ('taken_at', models.DateTimeField(verbose_name='EV02 date and time taken')),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
        ),
        migrations.CreateModel(
            name='PerfusateSample',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('barcode', models.CharField(max_length=20, verbose_name='SA01 barcode number')),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('centrifuged_at', models.DateTimeField(null=True, verbose_name='SA02 centrifuged at', blank=True)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('event', models.ForeignKey(to='samples.Event')),
                ('organ', models.ForeignKey(to='compare.Organ')),
            ],
            options={
                'abstract': False,
            },
        ),
        migrations.CreateModel(
            name='TissueSample',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('barcode', models.CharField(max_length=20, verbose_name='SA01 barcode number')),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('tissue_type', models.CharField(max_length=1, choices=[(b'F', 'TSc01 ReK1F'), (b'R', 'TSc02 ReK1R')])),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('event', models.ForeignKey(to='samples.Event')),
                ('organ', models.ForeignKey(to='compare.Organ')),
            ],
            options={
                'abstract': False,
            },
        ),
        migrations.CreateModel(
            name='UrineSample',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('barcode', models.CharField(max_length=20, verbose_name='SA01 barcode number')),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('centrifuged_at', models.DateTimeField(null=True, verbose_name='SA02 centrifuged at', blank=True)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('event', models.ForeignKey(to='samples.Event')),
                ('person', models.ForeignKey(to='compare.OrganPerson')),
            ],
            options={
                'abstract': False,
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
                'abstract': False,
            },
        ),
        migrations.AddField(
            model_name='urinesample',
            name='worksheet',
            field=models.ForeignKey(blank=True, to='samples.Worksheet', null=True),
        ),
        migrations.AddField(
            model_name='tissuesample',
            name='worksheet',
            field=models.ForeignKey(blank=True, to='samples.Worksheet', null=True),
        ),
        migrations.AddField(
            model_name='perfusatesample',
            name='worksheet',
            field=models.ForeignKey(blank=True, to='samples.Worksheet', null=True),
        ),
        migrations.AddField(
            model_name='deviation',
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
        migrations.AddField(
            model_name='bloodsample',
            name='worksheet',
            field=models.ForeignKey(blank=True, to='samples.Worksheet', null=True),
        ),
    ]
