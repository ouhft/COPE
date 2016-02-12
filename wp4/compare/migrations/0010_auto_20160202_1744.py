# -*- coding: utf-8 -*-
# Generated by Django 1.9.1 on 2016-02-02 17:44
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0009_organ_allocated'),
    ]

    operations = [
        migrations.AddField(
            model_name='organ',
            name='not_allocated_reason',
            field=models.CharField(blank=True, max_length=250, verbose_name='OR31 not transplantable because'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='allocated',
            field=models.NullBooleanField(default=None, verbose_name='OR30 Has this kidney been allocated?'),
        ),
    ]
