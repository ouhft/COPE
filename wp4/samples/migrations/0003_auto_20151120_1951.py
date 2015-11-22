# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('samples', '0002_auto_20151120_1328'),
    ]

    operations = [
        migrations.AddField(
            model_name='event',
            name='name',
            field=models.PositiveSmallIntegerField(default=1, verbose_name='EV03 sample process name', choices=[(1, 'EVc05 donor blood 1'), (2, 'EVc06 donor urine 1'), (3, 'EVc07 donor urine 2'), (4, 'EVc08 organ perfusate 1'), (5, 'EVc09 organ perfusate 2'), (6, 'EVc10 organ perfusate 3'), (7, 'EVc11 organ tissue 1'), (8, 'EVc12 recipient blood 1'), (9, 'EVc13 recipient blood 2')]),
            preserve_default=False,
        ),
        migrations.AlterField(
            model_name='bloodsample',
            name='barcode',
            field=models.CharField(max_length=20, verbose_name='SA01 barcode number', blank=True),
        ),
        migrations.AlterField(
            model_name='bloodsample',
            name='centrifuged_at',
            field=models.DateTimeField(null=True, verbose_name='BS01 centrifuged at', blank=True),
        ),
        migrations.AlterField(
            model_name='perfusatesample',
            name='barcode',
            field=models.CharField(max_length=20, verbose_name='SA01 barcode number', blank=True),
        ),
        migrations.AlterField(
            model_name='perfusatesample',
            name='centrifuged_at',
            field=models.DateTimeField(null=True, verbose_name='PS01 centrifuged at', blank=True),
        ),
        migrations.AlterField(
            model_name='tissuesample',
            name='barcode',
            field=models.CharField(max_length=20, verbose_name='SA01 barcode number', blank=True),
        ),
        migrations.AlterField(
            model_name='urinesample',
            name='barcode',
            field=models.CharField(max_length=20, verbose_name='SA01 barcode number', blank=True),
        ),
        migrations.AlterField(
            model_name='urinesample',
            name='centrifuged_at',
            field=models.DateTimeField(null=True, verbose_name='US01 centrifuged at', blank=True),
        ),
        migrations.AlterField(
            model_name='worksheet',
            name='barcode',
            field=models.CharField(max_length=20, verbose_name='SA01 barcode number', blank=True),
        ),
    ]
