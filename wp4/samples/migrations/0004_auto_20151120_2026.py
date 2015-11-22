# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('samples', '0003_auto_20151120_1951'),
    ]

    operations = [
        migrations.AddField(
            model_name='bloodsample',
            name='record_locked',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='bloodsample',
            name='version',
            field=models.PositiveIntegerField(default=0),
        ),
        migrations.AddField(
            model_name='event',
            name='record_locked',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='event',
            name='version',
            field=models.PositiveIntegerField(default=0),
        ),
        migrations.AddField(
            model_name='perfusatesample',
            name='record_locked',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='perfusatesample',
            name='version',
            field=models.PositiveIntegerField(default=0),
        ),
        migrations.AddField(
            model_name='tissuesample',
            name='record_locked',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='tissuesample',
            name='version',
            field=models.PositiveIntegerField(default=0),
        ),
        migrations.AddField(
            model_name='urinesample',
            name='record_locked',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='urinesample',
            name='version',
            field=models.PositiveIntegerField(default=0),
        ),
        migrations.AddField(
            model_name='worksheet',
            name='record_locked',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='worksheet',
            name='version',
            field=models.PositiveIntegerField(default=0),
        ),
    ]
