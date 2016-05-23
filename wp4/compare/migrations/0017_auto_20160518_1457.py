# -*- coding: utf-8 -*-
# Generated by Django 1.9.6 on 2016-05-18 14:57
from __future__ import unicode_literals

from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0016_auto_20160504_1343'),
    ]

    operations = [
        migrations.AddField(
            model_name='donor',
            name='_left_kidney',
            field=models.OneToOneField(default=None, null=True, on_delete=django.db.models.deletion.CASCADE, related_name='left_kidney', to='compare.Organ'),
        ),
        migrations.AddField(
            model_name='donor',
            name='_right_kidney',
            field=models.OneToOneField(default=None, null=True, on_delete=django.db.models.deletion.CASCADE, related_name='right_kidney', to='compare.Organ'),
        ),
    ]

"""
TODO: Run this against the data to create the quick look up links for existing data

from wp4.compare.models import Donor
from django.contrib.auth.models import User
creator = User.objects.get(pk=1)
for d in Donor.objects.all():
    l = d.left_kidney
    r = d.right_kidney
    # print("rl=%s" % d._right_kidney)
    d.save(created_by=creator)
"""