# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models
import django.core.validators


class Migration(migrations.Migration):

    dependencies = [
        ('staff_person', '0001_initial'),
    ]

    operations = [
        migrations.AlterField(
            model_name='staffperson',
            name='telephone',
            field=models.CharField(blank=True, max_length=15, verbose_name='PE13 telephone number', validators=[django.core.validators.RegexValidator(regex='^\\+?1?\\d{9,15}$', message="Phone number must be entered in the format: '+999999999'. Up to 15 digits allowed.")]),
        ),
    ]
