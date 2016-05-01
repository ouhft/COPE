# -*- coding: utf-8 -*-
# Generated by Django 1.9 on 2016-01-20 16:29
from __future__ import unicode_literals

import django.core.validators
from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0005_auto_20160114_0944'),
    ]

    operations = [
        migrations.AlterField(
            model_name='organperson',
            name='weight',
            field=models.DecimalField(blank=True, decimal_places=1, max_digits=4, null=True, validators=[django.core.validators.MinValueValidator(20.0), django.core.validators.MaxValueValidator(200.0)], verbose_name='OP04 Weight (kg)'),
        ),
    ]