# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
import django.core.validators


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0006_auto_20150730_2109'),
    ]

    operations = [
        migrations.AlterField(
            model_name='donor',
            name='admitted_to_itu',
            field=models.BooleanField(default=False, verbose_name='admitted to ITU'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='length_of_no_touch',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='length of no touch period (minutes)', validators=[django.core.validators.MinValueValidator(1), django.core.validators.MaxValueValidator(60)]),
        ),
    ]
