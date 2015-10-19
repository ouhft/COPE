# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('followups', '0002_auto_20150916_2136'),
    ]

    operations = [
        migrations.AlterField(
            model_name='followup1y',
            name='currently_on_dialysis',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='currently on dialysis', choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')]),
        ),
        migrations.AlterField(
            model_name='followup3m',
            name='currently_on_dialysis',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='currently on dialysis', choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')]),
        ),
        migrations.AlterField(
            model_name='followup6m',
            name='currently_on_dialysis',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='currently on dialysis', choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')]),
        ),
    ]
