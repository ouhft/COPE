# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0003_auto_20150625_2256'),
    ]

    operations = [
        migrations.AddField(
            model_name='donor',
            name='o2_saturation',
            field=models.DateTimeField(null=True, verbose_name=b'O2 saturation below 80%', blank=True),
        ),
        migrations.AddField(
            model_name='donor',
            name='systemic_flush_volume_used',
            field=models.PositiveSmallIntegerField(null=True, verbose_name=b'aortic - volume (ml)', blank=True),
        ),
        migrations.AddField(
            model_name='organ',
            name='graft_damage_other',
            field=models.CharField(max_length=250, null=True, blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='call_received',
            field=models.DateTimeField(null=True, verbose_name=b'Consultant to MTO called at', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='death_diagnosed',
            field=models.DateTimeField(null=True, verbose_name=b'knife to skin time', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='depart_perfusion_centre',
            field=models.DateTimeField(null=True, verbose_name=b'departure from base hospital at', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diuresis_last_hour',
            field=models.PositiveSmallIntegerField(null=True, verbose_name=b'Diuresis last hour (ml)', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='gender',
            field=models.CharField(default='M', max_length=1, choices=[(b'M', b'Male'), (b'F', b'Female')]),
            preserve_default=False,
        ),
        migrations.AlterField(
            model_name='donor',
            name='number',
            field=models.CharField(max_length=20, verbose_name=b'NHSBT Number'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='retrieval_hospital',
            field=models.ForeignKey(verbose_name=b'donor hospital', blank=True, to='compare.Hospital', null=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='scheduled_start',
            field=models.DateTimeField(null=True, verbose_name=b'time of withdrawal therapy', blank=True),
        ),
        migrations.AlterField(
            model_name='donor',
            name='transplant_coordinator',
            field=models.ForeignKey(related_name='transplant_coordinator_set', verbose_name=b'name of the SN-OD', blank=True, to='compare.Person', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='graft_damage',
            field=models.PositiveSmallIntegerField(default=5, choices=[(1, b'Arterial Damage'), (2, b'Venous Damage'), (3, b'Ureteral Damage'), (4, b'Parenchymal Damage'), (6, b'Other Damage'), (5, b'None')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_measurable',
            field=models.NullBooleanField(verbose_name=b'logistically possible to measure pO2 perfusate (use blood gas analyser)'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_possible',
            field=models.NullBooleanField(verbose_name=b'machine perfusion possible?'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusion_started',
            field=models.DateTimeField(null=True, verbose_name=b'machine perfusion', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='removal',
            field=models.DateTimeField(null=True, verbose_name=b'time out', blank=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='washout_perfusion',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name=b'perfusion characteristics', choices=[(1, b'Homogenous'), (2, b'Patchy'), (3, b'Blue'), (9, b'Unknown')]),
        ),
        migrations.AlterField(
            model_name='person',
            name='first_names',
            field=models.CharField(max_length=50, verbose_name='first names'),
        ),
        migrations.AlterField(
            model_name='person',
            name='job',
            field=models.CharField(max_length=2, verbose_name='job', choices=[(b'PT', 'Perfusion Technician'), (b'TC', 'Transplant Co-ordinator'), (b'RN', 'Research Nurse / Follow-up'), (b'NC', 'National Co-ordinator'), (b'CC', 'Central Co-ordinator'), (b'BC', 'Biobank Co-ordinator'), (b'S', 'Statistician'), (b'SA', 'Sys-admin')]),
        ),
        migrations.AlterField(
            model_name='person',
            name='last_names',
            field=models.CharField(max_length=50, verbose_name='last names'),
        ),
        migrations.AlterField(
            model_name='person',
            name='telephone',
            field=models.CharField(max_length=20, verbose_name='telephone number'),
        ),
        migrations.AlterField(
            model_name='person',
            name='user',
            field=models.OneToOneField(related_name='people_set', null=True, blank=True, to=settings.AUTH_USER_MODEL, verbose_name='related user account'),
        ),
    ]
