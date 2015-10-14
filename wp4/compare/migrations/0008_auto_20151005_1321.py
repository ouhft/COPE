# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0007_auto_20151002_1612'),
    ]

    operations = [
        migrations.AlterField(
            model_name='donor',
            name='multiple_recipients',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO01 Multiple recipients', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')]),
        ),
    ]
