# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0002_recipient_journey_remarks'),
    ]

    operations = [
        migrations.RenameField(
            model_name='recipient',
            old_name='arrival_at_donor_hospital',
            new_name='arrival_at_recipient_hospital',
        ),
    ]
