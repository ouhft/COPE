# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0009_auto_20151014_2104'),
    ]

    operations = [
        migrations.AlterField(
            model_name='recipient',
            name='reallocated',
            field=models.NullBooleanField(default=None, verbose_name='reallocated'),
        ),
    ]
