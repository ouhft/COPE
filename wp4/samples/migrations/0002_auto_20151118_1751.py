# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('samples', '0001_initial'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='deviation',
            name='created_by',
        ),
        migrations.RemoveField(
            model_name='deviation',
            name='worksheet',
        ),
        migrations.AlterModelOptions(
            name='bloodsample',
            options={'ordering': ['person', 'event__taken_at'], 'verbose_name': 'BSm1 blood sample', 'verbose_name_plural': 'BSm2 blood samples'},
        ),
        migrations.AlterModelOptions(
            name='event',
            options={'ordering': ['taken_at'], 'verbose_name': 'EVm1 sample event', 'verbose_name_plural': 'EVm2 sample events'},
        ),
        migrations.AlterModelOptions(
            name='perfusatesample',
            options={'ordering': ['organ', 'event__taken_at'], 'verbose_name': 'PSm1 perfusate sample', 'verbose_name_plural': 'PSm2 perfusate samples'},
        ),
        migrations.AlterModelOptions(
            name='tissuesample',
            options={'ordering': ['organ', 'event__taken_at'], 'verbose_name': 'TSm1 tissue sample', 'verbose_name_plural': 'TSm2 tissue samples'},
        ),
        migrations.AlterModelOptions(
            name='urinesample',
            options={'ordering': ['person', 'event__taken_at'], 'verbose_name': 'USm1 urine sample', 'verbose_name_plural': 'USm2 urine samples'},
        ),
        migrations.AlterModelOptions(
            name='worksheet',
            options={'ordering': ['person'], 'verbose_name': 'WSm1 worksheet', 'verbose_name_plural': 'WSm2 worksheets'},
        ),
        migrations.AddField(
            model_name='bloodsample',
            name='collected',
            field=models.NullBooleanField(verbose_name='WM01 sample collected?'),
        ),
        migrations.AddField(
            model_name='bloodsample',
            name='notes',
            field=models.TextField(verbose_name='WM02 notes', blank=True),
        ),
        migrations.AddField(
            model_name='perfusatesample',
            name='collected',
            field=models.NullBooleanField(verbose_name='WM01 sample collected?'),
        ),
        migrations.AddField(
            model_name='perfusatesample',
            name='notes',
            field=models.TextField(verbose_name='WM02 notes', blank=True),
        ),
        migrations.AddField(
            model_name='tissuesample',
            name='collected',
            field=models.NullBooleanField(verbose_name='WM01 sample collected?'),
        ),
        migrations.AddField(
            model_name='tissuesample',
            name='notes',
            field=models.TextField(verbose_name='WM02 notes', blank=True),
        ),
        migrations.AddField(
            model_name='urinesample',
            name='collected',
            field=models.NullBooleanField(verbose_name='WM01 sample collected?'),
        ),
        migrations.AddField(
            model_name='urinesample',
            name='notes',
            field=models.TextField(verbose_name='WM02 notes', blank=True),
        ),
        migrations.AlterField(
            model_name='event',
            name='type',
            field=models.PositiveSmallIntegerField(verbose_name='EV01 sample type', choices=[(1, 'EVc01 Blood'), (2, 'EVc02 Urine'), (3, 'EVc03 Perfusate'), (4, 'EVc04 Tissue')]),
        ),
        migrations.DeleteModel(
            name='Deviation',
        ),
    ]
