# -*- coding: utf-8 -*-
# Generated by Django 1.9.2 on 2016-02-26 20:31
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('followups', '0006_auto_20160226_1643'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='followup1y',
            name='graft_failure',
        ),
        migrations.RemoveField(
            model_name='followup1y',
            name='graft_removal',
        ),
        migrations.RemoveField(
            model_name='followup3m',
            name='graft_failure',
        ),
        migrations.RemoveField(
            model_name='followup3m',
            name='graft_removal',
        ),
        migrations.RemoveField(
            model_name='followup6m',
            name='graft_failure',
        ),
        migrations.RemoveField(
            model_name='followup6m',
            name='graft_removal',
        ),
        migrations.RemoveField(
            model_name='followupinitial',
            name='graft_failure',
        ),
        migrations.RemoveField(
            model_name='followupinitial',
            name='graft_removal',
        ),
        migrations.AddField(
            model_name='followupinitial',
            name='dialysis_requirement_1',
            field=models.NullBooleanField(verbose_name='FI09 dialysis on day 1'),
        ),
        migrations.AlterField(
            model_name='followupinitial',
            name='dialysis_requirement_2',
            field=models.NullBooleanField(verbose_name='FI10 dialysis on day 2'),
        ),
        migrations.AlterField(
            model_name='followupinitial',
            name='dialysis_requirement_3',
            field=models.NullBooleanField(verbose_name='FI11 dialysis on day 3'),
        ),
        migrations.AlterField(
            model_name='followupinitial',
            name='dialysis_requirement_4',
            field=models.NullBooleanField(verbose_name='FI12 dialysis on day 4'),
        ),
        migrations.AlterField(
            model_name='followupinitial',
            name='dialysis_requirement_5',
            field=models.NullBooleanField(verbose_name='FI13 dialysis on day 5'),
        ),
        migrations.AlterField(
            model_name='followupinitial',
            name='dialysis_requirement_6',
            field=models.NullBooleanField(verbose_name='FI14 dialysis on day 6'),
        ),
        migrations.AlterField(
            model_name='followupinitial',
            name='dialysis_requirement_7',
            field=models.NullBooleanField(verbose_name='FI15 dialysis on day 7'),
        ),
        migrations.AlterField(
            model_name='followupinitial',
            name='induction_therapy',
            field=models.PositiveSmallIntegerField(blank=True, choices=[(1, 'FIc10 IL 2'), (2, 'FIc11 ATG'), (3, 'FIc12 None')], null=True, verbose_name='FI25 Induction therapy'),
        ),
    ]