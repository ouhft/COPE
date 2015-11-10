# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models
import django.utils.timezone
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        ('locations', '0001_initial'),
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
    ]

    operations = [
        migrations.CreateModel(
            name='StaffJob',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('description', models.CharField(max_length=100)),
            ],
        ),
        migrations.CreateModel(
            name='StaffPerson',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('version', models.PositiveIntegerField(default=0)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('record_locked', models.BooleanField(default=False)),
                ('first_names', models.CharField(max_length=50, verbose_name='PE10 first names')),
                ('last_names', models.CharField(max_length=50, verbose_name='PE11 last names')),
                ('telephone', models.CharField(max_length=20, verbose_name='PE13 telephone number', blank=True)),
                ('email', models.EmailField(max_length=254, verbose_name='PE15 email', blank=True)),
                ('based_at', models.ForeignKey(blank=True, to='locations.Hospital', null=True)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('jobs', models.ManyToManyField(to='staff_person.StaffJob', verbose_name='PE12 jobs')),
                ('user', models.OneToOneField(related_name='profile', null=True, blank=True, to=settings.AUTH_USER_MODEL, verbose_name='PE14 related user account')),
            ],
            options={
                'verbose_name': 'PEm1 person',
                'verbose_name_plural': 'PEm2 people',
            },
        ),
    ]
