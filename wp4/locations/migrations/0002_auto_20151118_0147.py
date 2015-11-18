# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('locations', '0001_initial'),
    ]

    operations = [
        migrations.AlterField(
            model_name='hospital',
            name='country',
            field=models.PositiveSmallIntegerField(verbose_name='HO02 country', choices=[(1, 'MM10 United Kingdom'), (4, 'MM11 Belgium'), (5, 'MM12 Netherlands')]),
        ),
        migrations.AlterField(
            model_name='hospital',
            name='is_active',
            field=models.BooleanField(default=True, verbose_name='HO03 is active'),
        ),
        migrations.AlterField(
            model_name='hospital',
            name='is_project_site',
            field=models.BooleanField(default=False, verbose_name='HO04 is project site'),
        ),
    ]
