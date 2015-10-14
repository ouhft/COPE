# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0008_auto_20151005_1321'),
    ]

    operations = [
        migrations.AlterField(
            model_name='procurementresource',
            name='expiry_date',
            field=models.DateField(null=True, verbose_name='PR13 expiry date', blank=True),
        ),
        migrations.AlterField(
            model_name='procurementresource',
            name='lot_number',
            field=models.CharField(max_length=50, verbose_name='PR12 lot number', blank=True),
        ),
    ]
