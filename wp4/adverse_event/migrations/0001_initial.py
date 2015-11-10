# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models
import django.utils.timezone
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0001_initial'),
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
        ('staff_person', '0001_initial'),
    ]

    operations = [
        migrations.CreateModel(
            name='AdverseEvent',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('version', models.PositiveIntegerField(default=0)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('record_locked', models.BooleanField(default=False)),
                ('sequence_number', models.PositiveSmallIntegerField(default=0, verbose_name='AE01 sequence number')),
                ('onset_at_date', models.DateField(verbose_name='AE02 onset date')),
                ('resolution_at_date', models.DateField(null=True, verbose_name='AE03 resolution date', blank=True)),
                ('device_related', models.BooleanField(default=False, verbose_name='AE05 device related')),
                ('description', models.CharField(default=b'', max_length=1000, verbose_name='AE06 description')),
                ('action', models.CharField(default=b'', max_length=1000, verbose_name='AE07 action')),
                ('outcome', models.CharField(default=b'', max_length=1000, verbose_name='AE08 outcome')),
                ('contact', models.ForeignKey(verbose_name='AE09 primary contact', blank=True, to='staff_person.StaffPerson', null=True)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('organ', models.ForeignKey(verbose_name='AE04', to='compare.Organ')),
            ],
            options={
                'ordering': ['sequence_number'],
                'verbose_name': 'AEm1 adverse event',
                'verbose_name_plural': 'AEm2 adverse events',
            },
        ),
        migrations.CreateModel(
            name='AlternativeGrading',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('label', models.CharField(max_length=10)),
                ('description', models.CharField(max_length=300)),
            ],
        ),
        migrations.CreateModel(
            name='ClavienDindoGrading',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('label', models.CharField(max_length=10)),
                ('description', models.CharField(max_length=300)),
            ],
        ),
        migrations.AlterOrderWithRespectTo(
            name='adverseevent',
            order_with_respect_to='organ',
        ),
    ]
