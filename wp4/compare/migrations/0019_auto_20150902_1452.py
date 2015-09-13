# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0018_auto_20150826_2223'),
    ]

    operations = [
        migrations.AlterModelOptions(
            name='adverseevent',
            options={'ordering': ['sequence_number'], 'verbose_name': 'AEm1 adverse event', 'verbose_name_plural': 'AEm2 adverse events'},
        ),
        migrations.RemoveField(
            model_name='hospital',
            name='project_contact',
        ),
        migrations.AddField(
            model_name='adverseevent',
            name='action',
            field=models.CharField(default=b'', max_length=1000, verbose_name='AE07 action'),
        ),
        migrations.AddField(
            model_name='adverseevent',
            name='contact',
            field=models.ForeignKey(verbose_name='AE09 primary contact', blank=True, to='compare.StaffPerson', null=True),
        ),
        migrations.AddField(
            model_name='adverseevent',
            name='description',
            field=models.CharField(default=b'', max_length=1000, verbose_name='AE06 description'),
        ),
        migrations.AddField(
            model_name='adverseevent',
            name='device_related',
            field=models.BooleanField(default=False, verbose_name='AE05 device related'),
        ),
        migrations.AddField(
            model_name='adverseevent',
            name='organ',
            field=models.ForeignKey(default=None, verbose_name='AE04', to='compare.Organ'),
            preserve_default=False,
        ),
        migrations.AddField(
            model_name='adverseevent',
            name='outcome',
            field=models.CharField(default=b'', max_length=1000, verbose_name='AE08 outcome'),
        ),
        migrations.AddField(
            model_name='staffperson',
            name='based_at',
            field=models.ForeignKey(blank=True, to='compare.Hospital', null=True),
        ),
        migrations.AlterField(
            model_name='adverseevent',
            name='onset_at_date',
            field=models.DateField(verbose_name='AE02 onset date'),
        ),
        migrations.AlterField(
            model_name='adverseevent',
            name='resolution_at_date',
            field=models.DateField(null=True, verbose_name='AE03 resolution date', blank=True),
        ),
        migrations.AlterField(
            model_name='adverseevent',
            name='sequence_number',
            field=models.PositiveSmallIntegerField(default=0, verbose_name='AE01 sequence number'),
        ),
        migrations.AlterOrderWithRespectTo(
            name='adverseevent',
            order_with_respect_to='organ',
        ),
        migrations.RemoveField(
            model_name='adverseevent',
            name='cause_of_death_comment',
        ),
        migrations.RemoveField(
            model_name='adverseevent',
            name='date_of_death',
        ),
        migrations.RemoveField(
            model_name='adverseevent',
            name='grade_first_30_days',
        ),
        migrations.RemoveField(
            model_name='adverseevent',
            name='grade_first_30_days_d',
        ),
        migrations.RemoveField(
            model_name='adverseevent',
            name='grade_post_30_days',
        ),
        migrations.RemoveField(
            model_name='adverseevent',
            name='onset_at_time',
        ),
        migrations.RemoveField(
            model_name='adverseevent',
            name='recipient',
        ),
        migrations.RemoveField(
            model_name='adverseevent',
            name='reporting_site',
        ),
        migrations.RemoveField(
            model_name='adverseevent',
            name='resolution_at_time',
        ),
        migrations.RemoveField(
            model_name='adverseevent',
            name='treatment_related',
        ),
    ]
