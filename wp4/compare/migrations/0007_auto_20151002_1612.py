# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0006_recipient_cleaning_log'),
    ]

    operations = [
        migrations.AlterField(
            model_name='organ',
            name='preservation',
            field=models.PositiveSmallIntegerField(default=9, choices=[(9, 'Not Set'), (0, 'HMP'), (1, 'HMP O2')]),
        ),
    ]
