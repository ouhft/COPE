# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('followups', '0001_initial'),
    ]

    operations = [
        migrations.AlterField(
            model_name='followup1y',
            name='organ',
            field=models.OneToOneField(related_name='followup_1y', to='compare.Organ'),
        ),
        migrations.AlterField(
            model_name='followup3m',
            name='organ',
            field=models.OneToOneField(related_name='followup_3m', to='compare.Organ'),
        ),
        migrations.AlterField(
            model_name='followup6m',
            name='organ',
            field=models.OneToOneField(related_name='followup_6m', to='compare.Organ'),
        ),
        migrations.AlterField(
            model_name='followupinitial',
            name='organ',
            field=models.OneToOneField(related_name='followup_initial', to='compare.Organ'),
        ),
    ]
