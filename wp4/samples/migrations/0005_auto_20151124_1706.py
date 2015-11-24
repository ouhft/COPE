# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('samples', '0004_auto_20151120_2026'),
    ]

    operations = [
        migrations.AlterModelOptions(
            name='event',
            options={'ordering': ['type', 'name'], 'verbose_name': 'EVm1 sample event', 'verbose_name_plural': 'EVm2 sample events'},
        ),
        migrations.AlterField(
            model_name='bloodsample',
            name='barcode',
            field=models.CharField(max_length=20, verbose_name='BC01 barcode number', blank=True),
        ),
        migrations.AlterField(
            model_name='bloodsample',
            name='collected',
            field=models.NullBooleanField(verbose_name='DM01 sample collected?'),
        ),
        migrations.AlterField(
            model_name='bloodsample',
            name='notes',
            field=models.TextField(verbose_name='DM02 notes', blank=True),
        ),
        migrations.AlterField(
            model_name='bloodsample',
            name='person',
            field=models.ForeignKey(verbose_name='BS03 sample from', to='compare.OrganPerson'),
        ),
        migrations.AlterField(
            model_name='perfusatesample',
            name='barcode',
            field=models.CharField(max_length=20, verbose_name='BC01 barcode number', blank=True),
        ),
        migrations.AlterField(
            model_name='perfusatesample',
            name='collected',
            field=models.NullBooleanField(verbose_name='DM01 sample collected?'),
        ),
        migrations.AlterField(
            model_name='perfusatesample',
            name='notes',
            field=models.TextField(verbose_name='DM02 notes', blank=True),
        ),
        migrations.AlterField(
            model_name='perfusatesample',
            name='organ',
            field=models.ForeignKey(verbose_name='PS02 sample from', to='compare.Organ'),
        ),
        migrations.AlterField(
            model_name='tissuesample',
            name='barcode',
            field=models.CharField(max_length=20, verbose_name='BC01 barcode number', blank=True),
        ),
        migrations.AlterField(
            model_name='tissuesample',
            name='collected',
            field=models.NullBooleanField(verbose_name='DM01 sample collected?'),
        ),
        migrations.AlterField(
            model_name='tissuesample',
            name='notes',
            field=models.TextField(verbose_name='DM02 notes', blank=True),
        ),
        migrations.AlterField(
            model_name='tissuesample',
            name='organ',
            field=models.ForeignKey(verbose_name='TS01 sample from', to='compare.Organ'),
        ),
        migrations.AlterField(
            model_name='tissuesample',
            name='tissue_type',
            field=models.CharField(max_length=1, verbose_name='TS02 tissue sample type', choices=[('F', 'TSc01 ReK1F'), ('R', 'TSc02 ReK1R')]),
        ),
        migrations.AlterField(
            model_name='urinesample',
            name='barcode',
            field=models.CharField(max_length=20, verbose_name='BC01 barcode number', blank=True),
        ),
        migrations.AlterField(
            model_name='urinesample',
            name='collected',
            field=models.NullBooleanField(verbose_name='DM01 sample collected?'),
        ),
        migrations.AlterField(
            model_name='urinesample',
            name='notes',
            field=models.TextField(verbose_name='DM02 notes', blank=True),
        ),
        migrations.AlterField(
            model_name='urinesample',
            name='person',
            field=models.ForeignKey(verbose_name='US02 sample from', to='compare.OrganPerson'),
        ),
        migrations.AlterField(
            model_name='worksheet',
            name='barcode',
            field=models.CharField(max_length=20, verbose_name='BC01 barcode number', blank=True),
        ),
    ]
