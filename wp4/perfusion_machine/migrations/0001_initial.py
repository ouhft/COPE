# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models
import django.utils.timezone
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
    ]

    operations = [
        migrations.CreateModel(
            name='PerfusionFile',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('file', models.FileField(upload_to=b'perfusion_files', blank=True)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
            options={
                'verbose_name': 'PFm1 perfusion machine file',
                'verbose_name_plural': 'PFm2 perfusion machine files',
            },
        ),
        migrations.CreateModel(
            name='PerfusionMachine',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('machine_serial_number', models.CharField(max_length=50, verbose_name='PM01 machine serial number')),
                ('machine_reference_number', models.CharField(max_length=50, verbose_name='PM02 machine reference number')),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
            options={
                'verbose_name': 'PMm1 perfusion machine',
                'verbose_name_plural': 'PMm2 perfusion machines',
            },
        ),
        migrations.AddField(
            model_name='perfusionfile',
            name='machine',
            field=models.ForeignKey(verbose_name='PF01 perfusion machine', to='perfusion_machine.PerfusionMachine'),
        ),
    ]
