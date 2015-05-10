# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
import datetime
from django.utils.timezone import utc


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0001_initial'),
    ]

    operations = [
        migrations.AlterModelOptions(
            name='organsoffered',
            options={'verbose_name_plural': 'organs offered'},
        ),
        migrations.AlterModelOptions(
            name='person',
            options={'verbose_name_plural': 'people'},
        ),
        migrations.RemoveField(
            model_name='donor',
            name='life_support_withdrawl',
        ),
        migrations.AddField(
            model_name='donor',
            name='life_support_withdrawal',
            field=models.DateTimeField(default=datetime.datetime(2015, 5, 10, 21, 27, 5, 844703, tzinfo=utc), verbose_name=b'withdrawal of life support'),
            preserve_default=False,
        ),
        migrations.AddField(
            model_name='person',
            name='telephone',
            field=models.CharField(default='01234567890', max_length=20),
            preserve_default=False,
        ),
        migrations.AlterField(
            model_name='donor',
            name='circulatory_arrest',
            field=models.DateTimeField(verbose_name=b'end of cardiac output (=start of no touch period)'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='death_diagnosed',
            field=models.DateTimeField(verbose_name=b'diagnosis of death'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diuresis_last_day',
            field=models.PositiveSmallIntegerField(verbose_name=b'Diuresis last day (ml)'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diuresis_last_hour',
            field=models.PositiveSmallIntegerField(verbose_name=b'Diuresis last hour (ml'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='heparin',
            field=models.NullBooleanField(verbose_name=b'heparin (administered to donor/in flush solution)'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='length_of_no_touch',
            field=models.PositiveSmallIntegerField(verbose_name=b'length of no touch period (minutes)'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='perfusion_started',
            field=models.DateTimeField(verbose_name=b'start in-situ cold perfusion'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systemic_flush_used',
            field=models.PositiveSmallIntegerField(verbose_name=b'systemic (aortic) flush solution used', choices=[(3, b'HTK'), (2, b"Marshall's"), (1, b'UW'), (4, b'Other')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systolic_pressure_low',
            field=models.DateTimeField(verbose_name=b'systolic arterial pressure < 50 mm Hg (inadequate organ perfusion)'),
        ),
        migrations.AlterField(
            model_name='organsoffered',
            name='organ',
            field=models.PositiveSmallIntegerField(choices=[(1, b'Liver'), (2, b'Lung'), (3, b'Pancreas'), (4, b'Tissue')]),
        ),
    ]
