# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0003_auto_20150914_1426'),
    ]

    operations = [
        migrations.AddField(
            model_name='recipient',
            name='arterial_problems_other',
            field=models.CharField(max_length=250, verbose_name='arterial problems other', blank=True),
        ),
        migrations.AddField(
            model_name='recipient',
            name='venous_problems_other',
            field=models.CharField(max_length=250, verbose_name='venous problems other', blank=True),
        ),
    ]
