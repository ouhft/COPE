# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0008_auto_20150811_1155'),
    ]

    operations = [
        migrations.AddField(
            model_name='donor',
            name='multiple_recipients',
            field=models.NullBooleanField(default=None, verbose_name=''),
        ),
    ]
