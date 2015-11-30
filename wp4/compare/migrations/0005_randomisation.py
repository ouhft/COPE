# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models
import django.utils.timezone
import wp4.compare.models


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0004_auto_20151125_1608'),
    ]

    operations = [
        migrations.CreateModel(
            name='Randomisation',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('country', models.PositiveSmallIntegerField(verbose_name='RA01 country', choices=[(1, 'MM10 United Kingdom'), (4, 'MM11 Belgium'), (5, 'MM12 Netherlands')])),
                ('result', models.BooleanField(default=wp4.compare.models.random_5050, verbose_name='RA02 result')),
                ('allocated_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('donor', models.OneToOneField(null=True, default=None, blank=True, to='compare.Donor')),
            ],
        ),
    ]
