# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0002_auto_20150625_2157'),
    ]

    operations = [
        migrations.AlterField(
            model_name='organ',
            name='artificial_patch_used',
            field=models.NullBooleanField(),
        ),
        migrations.AlterField(
            model_name='organ',
            name='ice_container_replenished',
            field=models.NullBooleanField(),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_changed',
            field=models.NullBooleanField(),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_full',
            field=models.NullBooleanField(),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_open',
            field=models.NullBooleanField(),
        ),
        migrations.AlterField(
            model_name='organ',
            name='patch_holder',
            field=models.PositiveSmallIntegerField(blank=True, null=True, choices=[(1, b'Small'), (2, b'Large'), (3, b'Double Artery')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_measurable',
            field=models.NullBooleanField(),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_machine',
            field=models.ForeignKey(blank=True, to='compare.PerfusionMachine', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_possible',
            field=models.NullBooleanField(),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_started',
            field=models.DateTimeField(null=True, blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='preservation',
            field=models.PositiveSmallIntegerField(blank=True, null=True, choices=[(0, b'HMP'), (1, b'HMP O2')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='removal',
            field=models.DateTimeField(null=True, blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='renal_arteries',
            field=models.PositiveSmallIntegerField(null=True, blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='transplantable',
            field=models.NullBooleanField(),
        ),
        migrations.AlterField(
            model_name='organ',
            name='washout_perfusion',
            field=models.PositiveSmallIntegerField(blank=True, null=True, choices=[(1, b'Homogenous'), (2, b'Patchy'), (3, b'Blue'), (9, b'Unknown')]),
        ),
    ]
