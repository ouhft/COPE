# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0014_auto_20150820_2252'),
    ]

    operations = [
        migrations.AlterField(
            model_name='recipient',
            name='perfusion_technician',
            field=models.ForeignKey(related_name='recipient_perfusion_technician_set', verbose_name='DO03 name of transplant technician', blank=True, to='compare.StaffPerson', null=True),
        ),
    ]
