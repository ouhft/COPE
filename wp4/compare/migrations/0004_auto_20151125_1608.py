# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0003_auto_20151124_1706'),
    ]

    operations = [
        migrations.AddField(
            model_name='donor',
            name='arrival_at_donor_hospital_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='donor',
            name='call_received_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='donor',
            name='circulatory_arrest_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='donor',
            name='date_admitted_to_itu_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='donor',
            name='date_of_admission_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='donor',
            name='depart_perfusion_centre_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='donor',
            name='ice_boxes_filled_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='donor',
            name='o2_saturation_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='donor',
            name='perfusion_started_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='donor',
            name='scheduled_start_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='donor',
            name='systolic_pressure_low_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='donor',
            name='technician_arrival_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='organ',
            name='ice_container_replenished_at_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='organ',
            name='oxygen_bottle_changed_at_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='organallocation',
            name='arrival_at_recipient_hospital_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='organallocation',
            name='call_received_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='organallocation',
            name='depart_perfusion_centre_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='organallocation',
            name='scheduled_start_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='organallocation',
            name='technician_arrival_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='procurementresource',
            name='expiry_date_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='recipient',
            name='anastomosis_started_at_unknown',
            field=models.BooleanField(default=False),
        ),
        migrations.AddField(
            model_name='recipient',
            name='reperfusion_started_at_unknown',
            field=models.BooleanField(default=False),
        ),
    ]
