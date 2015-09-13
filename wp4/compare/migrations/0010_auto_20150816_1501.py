# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0009_donor_multiple_recipients'),
    ]

    operations = [
        migrations.AddField(
            model_name='organ',
            name='perfusate_3',
            field=models.ForeignKey(related_name='kidney_perfusate_3', verbose_name='OR61 p3', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AddField(
            model_name='sample',
            name='type',
            field=models.PositiveSmallIntegerField(default=1, verbose_name='SA05 sample type', choices=[(1, 'SA10 Donor blood 1'), (2, 'SA11 Donor blood 2'), (3, 'SA12 Donor urine 1'), (4, 'SA13 Donor urine 2'), (5, 'SA14 Kidney perfusate 1'), (6, 'SA15 Kidney perfusate 1'), (7, 'SA16 Kidney perfusate 1'), (8, 'SA17 Recipient blood 1'), (9, 'SA18 Recipient blood 1'), (10, 'SA19 Kidney tissue 1')]),
            preserve_default=False,
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_blood_1_EDTA',
            field=models.OneToOneField(related_name='donor_blood_1', null=True, blank=True, to='compare.Sample', verbose_name='DO91 db 1.1 edta'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_blood_1_SST',
            field=models.OneToOneField(related_name='donor_blood_2', null=True, blank=True, to='compare.Sample', verbose_name='DO92 db 1.2 sst'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_urine_1',
            field=models.OneToOneField(related_name='donor_urine_1', null=True, blank=True, to='compare.Sample', verbose_name='DO93 du 1'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='donor_urine_2',
            field=models.OneToOneField(related_name='donor_urine_2', null=True, blank=True, to='compare.Sample', verbose_name='DO94 du 2'),
        ),
        migrations.AlterField(
            model_name='organ',
            name='graft_damage',
            field=models.PositiveSmallIntegerField(default=5, verbose_name='OR23 renal graft damage', choices=[(5, 'OR15 None'), (1, 'OR10 Arterial Damage'), (2, 'OR11 Venous Damage'), (3, 'OR12 Ureteral Damage'), (4, 'OR13 Parenchymal Damage'), (6, 'OR14 Other Damage')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_1',
            field=models.ForeignKey(related_name='kidney_perfusate_1', verbose_name='OR60 p1', blank=True, to='compare.Sample', null=True),
        ),
        migrations.AlterField(
            model_name='organ',
            name='perfusate_2',
            field=models.ForeignKey(related_name='kidney_perfusate_2', verbose_name='OR60 p2', blank=True, to='compare.Sample', null=True),
        ),
    ]
