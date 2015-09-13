# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0019_auto_20150902_1452'),
    ]

    operations = [
        migrations.AlterField(
            model_name='donor',
            name='multiple_recipients',
            field=models.NullBooleanField(default=None, verbose_name='DO01 Multiple recipients'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='number',
            field=models.CharField(max_length=20, null=True, verbose_name='DO30 NHSBT Number', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='number',
            field=models.CharField(max_length=20, null=True, verbose_name='DO30 NHSBT Number', blank=True),
        ),
    ]
