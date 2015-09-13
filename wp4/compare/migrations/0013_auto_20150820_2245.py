# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0012_auto_20150820_2122'),
    ]

    operations = [
        migrations.AlterModelOptions(
            name='recipient',
            options={'ordering': ['sequence_number'], 'get_latest_by': 'sequence_number', 'verbose_name': 'REm1 recipient', 'verbose_name_plural': 'REm2 recipients'},
        ),
        migrations.AddField(
            model_name='recipient',
            name='sequence_number',
            field=models.PositiveSmallIntegerField(default=0),
        ),
        migrations.AlterOrderWithRespectTo(
            name='recipient',
            order_with_respect_to='organ',
        ),
    ]
