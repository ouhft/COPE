# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
import django.utils.timezone
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
        ('compare', '0017_auto_20150821_1710'),
    ]

    operations = [
        migrations.CreateModel(
            name='AdverseEvent',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('version', models.PositiveIntegerField(default=0)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('record_locked', models.BooleanField(default=False)),
                ('sequence_number', models.PositiveSmallIntegerField(default=0, verbose_name='AE02 sequence number')),
                ('onset_at_date', models.DateField()),
                ('onset_at_time', models.TimeField()),
                ('resolution_at_date', models.DateField()),
                ('resolution_at_time', models.TimeField()),
                ('grade_first_30_days_d', models.BooleanField(help_text=b'If the patients suffers from a complication at the time of discharge, the suffix  \xe2\x80\x9cd\xe2\x80\x9d (for \xe2\x80\x98disability\xe2\x80\x99) is added to the respective grade of complication. This label indicates the need for a follow-up to fully evaluate the complication.')),
                ('date_of_death', models.DateField()),
                ('treatment_related', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')])),
                ('cause_of_death_comment', models.CharField(max_length=500)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
            options={
                'abstract': False,
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
        migrations.CreateModel(
            name='StaffJob',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('description', models.CharField(max_length=100)),
            ],
        ),
        migrations.RemoveField(
            model_name='staffperson',
            name='job',
        ),
        migrations.AddField(
            model_name='donor',
            name='record_locked',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='hospital',
            name='is_project_site',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='hospital',
            name='project_contact',
            field=models.ForeignKey(blank=True, to='compare.StaffPerson', null=True),
        ),
        migrations.AddField(
            model_name='organ',
            name='record_locked',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='recipient',
            name='record_locked',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='staffperson',
            name='email',
            field=models.EmailField(max_length=254, null=True, verbose_name='PE15 email', blank=True),
        ),
        migrations.AddField(
            model_name='staffperson',
            name='record_locked',
            field=models.BooleanField(default=False),
        ),
        migrations.AlterField(
            model_name='staffperson',
            name='telephone',
            field=models.CharField(max_length=20, null=True, verbose_name='PE13 telephone number', blank=True),
        ),
        migrations.AddField(
            model_name='adverseevent',
            name='grade_first_30_days',
            field=models.ForeignKey(to='compare.ClavienDindoGrading'),
        ),
        migrations.AddField(
            model_name='adverseevent',
            name='grade_post_30_days',
            field=models.ForeignKey(to='compare.AlternativeGrading'),
        ),
        migrations.AddField(
            model_name='adverseevent',
            name='recipient',
            field=models.ForeignKey(to='compare.Recipient'),
        ),
        migrations.AddField(
            model_name='adverseevent',
            name='reporting_site',
            field=models.ForeignKey(to='compare.Hospital'),
        ),
        migrations.AddField(
            model_name='staffperson',
            name='jobs',
            field=models.ManyToManyField(to='compare.StaffJob', verbose_name='PE12 jobs'),
        ),
    ]
