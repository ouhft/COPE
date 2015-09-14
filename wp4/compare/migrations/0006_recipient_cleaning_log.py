# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0005_auto_20150914_1750'),
    ]

    operations = [
        migrations.AddField(
            model_name='recipient',
            name='cleaning_log',
            field=models.TextField(verbose_name='cleaning log notes', blank=True),
        ),
    ]
