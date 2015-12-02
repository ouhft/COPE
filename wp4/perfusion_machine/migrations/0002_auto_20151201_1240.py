# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('perfusion_machine', '0001_initial'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='perfusionfile',
            name='created_by',
        ),
        migrations.RemoveField(
            model_name='perfusionfile',
            name='created_on',
        ),
        migrations.RemoveField(
            model_name='perfusionmachine',
            name='created_by',
        ),
        migrations.RemoveField(
            model_name='perfusionmachine',
            name='created_on',
        ),
    ]
