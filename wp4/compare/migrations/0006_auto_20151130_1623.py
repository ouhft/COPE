# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0005_randomisation'),
    ]

    operations = [
        migrations.AddField(
            model_name='donor',
            name='form_completed',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='recipient',
            name='form_completed',
            field=models.BooleanField(default=False),
        ),
    ]
