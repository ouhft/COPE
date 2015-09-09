# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0020_auto_20150908_1303'),
    ]

    operations = [
        migrations.AddField(
            model_name='donor',
            name='date_of_birth_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='recipient',
            name='date_of_birth_unknown',
            field=models.BooleanField(default=False),
        ),
    ]
