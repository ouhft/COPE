# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0015_auto_20150820_2254'),
    ]

    operations = [
        migrations.AlterField(
            model_name='recipient',
            name='reallocation_recipient',
            field=models.OneToOneField(null=True, default=None, blank=True, to='compare.Recipient'),
        ),
    ]
