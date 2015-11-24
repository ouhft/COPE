# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0002_auto_20151116_1301'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='organperson',
            name='date_of_death',
        ),
        migrations.RemoveField(
            model_name='organperson',
            name='date_of_death_unknown',
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='arrival_at_recipient_hospital',
            field=models.DateTimeField(null=True, verbose_name='OA08 arrival at transplant hospital', blank=True),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='call_received',
            field=models.DateTimeField(null=True, verbose_name='OA02 call received from transplant co-ordinator at', blank=True),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='depart_perfusion_centre',
            field=models.DateTimeField(null=True, verbose_name='OA07 departure from hub', blank=True),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='journey_remarks',
            field=models.TextField(verbose_name='OA09 journey notes', blank=True),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='perfusion_technician',
            field=models.ForeignKey(related_name='recipient_perfusion_technician_set', verbose_name='OA01 name of transplant technician', blank=True, to='staff_person.StaffPerson', null=True),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='reallocated',
            field=models.NullBooleanField(default=None, verbose_name='OA10 reallocated'),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='reallocation_reason',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='OA11 reason for re-allocation', choices=[(1, 'OAc01 Positive crossmatch'), (2, 'OAc02 Unknown'), (3, 'OAc03 Other')]),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='reallocation_reason_other',
            field=models.CharField(max_length=250, verbose_name='OA12 other reason', blank=True),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='scheduled_start',
            field=models.DateTimeField(null=True, verbose_name='OA05 scheduled start', blank=True),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='technician_arrival',
            field=models.DateTimeField(null=True, verbose_name='OA06 arrival time at hub', blank=True),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='theatre_contact',
            field=models.ForeignKey(related_name='recipient_transplant_coordinator_set', verbose_name='OA04 name of the theatre contact', blank=True, to='staff_person.StaffPerson', null=True),
        ),
        migrations.AlterField(
            model_name='organallocation',
            name='transplant_hospital',
            field=models.ForeignKey(verbose_name='OA03 transplant hospital', blank=True, to='locations.Hospital', null=True),
        ),
    ]
