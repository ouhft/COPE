# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0006_auto_20151130_1623'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='perfusionfile',
            name='created_by',
        ),
        migrations.RemoveField(
            model_name='perfusionfile',
            name='machine',
        ),
        migrations.RemoveField(
            model_name='perfusionmachine',
            name='created_by',
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_file',
            field=models.ForeignKey(verbose_name='OR25 machine file', blank=True, to='perfusion_machine.PerfusionFile', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_machine',
            field=models.ForeignKey(verbose_name='OR24 perfusion machine', blank=True, to='perfusion_machine.PerfusionMachine', null=True),
        ),
        migrations.DeleteModel(
            name='PerfusionFile',
        ),
        migrations.DeleteModel(
            name='PerfusionMachine',
        ),
    ]
