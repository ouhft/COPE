# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0016_auto_20150820_2255'),
    ]

    operations = [
        migrations.AlterModelOptions(
            name='recipient',
            options={'ordering': ['sequence_number'], 'get_latest_by': 'created_on', 'verbose_name': 'REm1 recipient', 'verbose_name_plural': 'REm2 recipients'},
        ),
        migrations.RemoveField(
            model_name='recipient',
            name='sequence_number',
        ),
    ]
