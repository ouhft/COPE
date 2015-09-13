# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0005_auto_20150716_0928'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='donor',
            name='hypotensive',
        ),
        migrations.AlterField(
            model_name='donor',
            name='gender',
            field=models.CharField(default=b'M', max_length=1, verbose_name='gender', choices=[(b'M', 'Male'), (b'F', 'Female')]),
        ),
    ]
