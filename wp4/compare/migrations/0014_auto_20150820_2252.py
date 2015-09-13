# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0013_auto_20150820_2245'),
    ]

    operations = [
        migrations.AlterField(
            model_name='recipient',
            name='reallocation_reason',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='reason for re-allocation', choices=[(1, 'RE01 Positive crossmatch'), (2, 'RE02 Unknown'), (3, 'RE03 Other')]),
        ),
    ]
