# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models
import django.core.validators
import wp4.compare.validators


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0001_initial'),
    ]

    operations = [
        migrations.AlterField(
            model_name='donor',
            name='arrival_at_donor_hospital',
            field=models.DateTimeField(blank=True, null=True, verbose_name='DO11 arrival at donor hospital', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='call_received',
            field=models.DateTimeField(blank=True, null=True, verbose_name='DO05 Consultant to MTO called at', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='circulatory_arrest',
            field=models.DateTimeField(blank=True, null=True, verbose_name='DO41 end of cardiac output', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_admitted_to_itu',
            field=models.DateField(blank=True, null=True, verbose_name='DO15 when admitted to ITU', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_of_admission',
            field=models.DateField(blank=True, null=True, verbose_name='DO13 date of admission', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='date_of_procurement',
            field=models.DateField(blank=True, null=True, verbose_name='DO16 date of procurement', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='death_diagnosed',
            field=models.DateTimeField(blank=True, null=True, verbose_name='DO43 knife to skin time', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='depart_perfusion_centre',
            field=models.DateTimeField(blank=True, null=True, verbose_name='DO10 departure from base hospital at', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='ice_boxes_filled',
            field=models.DateTimeField(blank=True, null=True, verbose_name='DO09 ice boxes filled', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='life_support_withdrawal',
            field=models.DateTimeField(blank=True, null=True, verbose_name='DO38 withdrawal of life support', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='o2_saturation',
            field=models.DateTimeField(blank=True, null=True, verbose_name='DO40 O2 saturation below 80%', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='perfusion_started',
            field=models.DateTimeField(blank=True, null=True, verbose_name='DO44 start in-situ cold perfusion', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='scheduled_start',
            field=models.DateTimeField(blank=True, null=True, verbose_name='DO07 time of withdrawal therapy', validators=[wp4.compare.validators.validate_between_1900_2050]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='systolic_pressure_low',
            field=models.DateTimeField(blank=True, null=True, verbose_name='DO39 systolic arterial pressure', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='technician_arrival',
            field=models.DateTimeField(blank=True, null=True, verbose_name='DO08 arrival time of technician', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='ice_container_replenished_at',
            field=models.DateTimeField(blank=True, null=True, verbose_name='OR21 ice container replenished at', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='oxygen_bottle_changed_at',
            field=models.DateTimeField(blank=True, null=True, verbose_name='OR19 oxygen bottle changed at', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_started',
            field=models.DateTimeField(blank=True, null=True, verbose_name='OR11 machine perfusion', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='removal',
            field=models.DateTimeField(blank=True, null=True, verbose_name='OR02 time out', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='renal_arteries',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OR03 number of renal arteries', validators=[django.core.validators.MinValueValidator(0), django.core.validators.MaxValueValidator(5)]),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='arrival_at_recipient_hospital',
            field=models.DateTimeField(blank=True, null=True, verbose_name='OA08 arrival at transplant hospital', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='call_received',
            field=models.DateTimeField(blank=True, null=True, verbose_name='OA02 call received from transplant co-ordinator at', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='depart_perfusion_centre',
            field=models.DateTimeField(blank=True, null=True, verbose_name='OA07 departure from hub', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='scheduled_start',
            field=models.DateTimeField(blank=True, null=True, verbose_name='OA05 scheduled start', validators=[wp4.compare.validators.validate_between_1900_2050]),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='technician_arrival',
            field=models.DateTimeField(blank=True, null=True, verbose_name='OA06 arrival time at hub', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='transplant_hospital',
            field=models.ForeignKey(verbose_name='OA03 transplant hospital', blank=True, to='locations.Hospital', null=True),
        ),
        migrations.AlterField(
            model_name='organperson',
            name='date_of_birth',
            field=models.DateField(blank=True, null=True, verbose_name='OP02 date of birth', validators=[wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='anastomosis_started_at',
            field=models.DateTimeField(blank=True, null=True, verbose_name='RE34 start anastomosis at', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='anesthesia_started_at',
            field=models.DateTimeField(blank=True, null=True, verbose_name='RE27 start anesthesia at', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='knife_to_skin',
            field=models.DateTimeField(blank=True, null=True, verbose_name='RE18 knife to skin time', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='operation_concluded_at',
            field=models.DateTimeField(blank=True, null=True, verbose_name='RE43 operation concluded at', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='perfusion_stopped',
            field=models.DateTimeField(blank=True, null=True, verbose_name='RE20 stop machine perfusion', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='removed_from_machine_at',
            field=models.DateTimeField(blank=True, null=True, verbose_name='RE23 kidney removed from machine at', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='reperfusion_started_at',
            field=models.DateTimeField(blank=True, null=True, verbose_name='RE35 start reperfusion at', validators=[wp4.compare.validators.validate_between_1900_2050, wp4.compare.validators.validate_not_in_future]),
        ),
    ]
