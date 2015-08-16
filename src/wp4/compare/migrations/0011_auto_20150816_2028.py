# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0010_auto_20150816_1501'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='organsoffered',
            name='created_by',
        ),
        migrations.RemoveField(
            model_name='organsoffered',
            name='donor',
        ),
        migrations.AddField(
            model_name='donor',
            name='other_organs_liver',
            field=models.BooleanField(default=False, verbose_name='DO46 liver'),
        ),
        migrations.AddField(
            model_name='donor',
            name='other_organs_lungs',
            field=models.BooleanField(default=False, verbose_name='DO44 lungs'),
        ),
        migrations.AddField(
            model_name='donor',
            name='other_organs_pancreas',
            field=models.BooleanField(default=False, verbose_name='DO45 pancreas'),
        ),
        migrations.AddField(
            model_name='donor',
            name='other_organs_procured',
            field=models.BooleanField(default=False, verbose_name='DO43 other organs procured'),
        ),
        migrations.AddField(
            model_name='donor',
            name='other_organs_tissue',
            field=models.BooleanField(default=False, verbose_name='DO47 tissue'),
        ),
        migrations.DeleteModel(
            name='OrgansOffered',
        ),
    ]
