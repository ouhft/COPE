# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0004_auto_20150914_1623'),
    ]

    operations = [
        migrations.AddField(
            model_name='recipient',
            name='operation_concluded_at',
            field=models.DateTimeField(null=True, verbose_name='operation concluded at', blank=True),
        ),
        migrations.AddField(
            model_name='recipient',
            name='successful_conclusion',
            field=models.BooleanField(default=False, verbose_name='successful conclusion'),
        ),
    ]
