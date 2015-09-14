# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0001_initial'),
    ]

    operations = [
        migrations.AddField(
            model_name='recipient',
            name='journey_remarks',
            field=models.TextField(verbose_name='journey notes', blank=True),
        ),
    ]
