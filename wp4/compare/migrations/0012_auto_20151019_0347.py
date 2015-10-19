# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models
import django.core.validators


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0011_auto_20151019_0103'),
    ]

    operations = [
        migrations.AlterField(
            model_name='donor',
            name='alcohol_abuse',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO25 alcohol abuse', choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='diabetes_melitus',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO24 diabetes mellitus', choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='dobutamine',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO32 dobutamine', choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='dopamine',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO31 dopamine', choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='multiple_recipients',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO02 Multiple recipients', choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='nor_adrenaline',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO33 nor adrenaline', choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='sequence_number',
            field=models.PositiveSmallIntegerField(default=0),
        ),
        migrations.AlterField(
            model_name='donor',
            name='vasopressine',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO34 vasopressine', choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')]),
        ),
        migrations.AlterField(
            model_name='organ',
            name='location',
            field=models.CharField(max_length=1, verbose_name='OR01 kidney location', choices=[('L', 'ORc01 Left'), ('R', 'ORc02 Right')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='anastomosis_started_at',
            field=models.DateTimeField(null=True, verbose_name='RE34 start anastomosis at', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='anesthesia_started_at',
            field=models.DateTimeField(null=True, verbose_name='RE27 start anesthesia at', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='arterial_problems',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='RE30 arterial problems', choices=[(1, 'REc18 None'), (2, 'REc19 ligated polar artery'), (3, 'REc20 reconstructed polar artery'), (4, 'REc21 repaired intima dissection'), (5, 'REc22 other')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='arterial_problems_other',
            field=models.CharField(max_length=250, verbose_name='RE31 arterial problems other', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='batteries_charged',
            field=models.NullBooleanField(verbose_name='RE49 batteries charged'),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='box_cleaned',
            field=models.NullBooleanField(verbose_name='RE48 box kidney assist cleaned'),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='cleaning_log',
            field=models.TextField(verbose_name='RE50 cleaning log notes', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='cvp',
            field=models.PositiveSmallIntegerField(null=True, verbose_name='RE40 cvp at reperfusion', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='ice_removed',
            field=models.NullBooleanField(verbose_name='RE45 ice and water removed'),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='incision',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='RE28 incision', choices=[(1, 'REc15 midline laparotomy'), (2, 'REc16 hockey stick'), (3, 'REc17 unknown')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='intra_operative_diuresis',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='RE41 intra-operative diuresis', choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='knife_to_skin',
            field=models.DateTimeField(null=True, verbose_name='RE18 knife to skin time', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='mannitol_used',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='RE36 mannitol used', choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='operation_concluded_at',
            field=models.DateTimeField(null=True, verbose_name='RE43 operation concluded at', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='organ_cold_stored',
            field=models.BooleanField(default=False, verbose_name='RE21 kidney was cold stored?'),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='organ_untransplantable',
            field=models.NullBooleanField(verbose_name='RE25 kidney discarded'),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='organ_untransplantable_reason',
            field=models.CharField(max_length=250, verbose_name='RE26 untransplantable because', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='other_diurectics',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='RE37 other diurectics used', choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='other_diurectics_details',
            field=models.CharField(max_length=250, verbose_name='RE38 other diurectics detail', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='oxygen_bottle_removed',
            field=models.NullBooleanField(verbose_name='RE47 oxygen bottle removed'),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='oxygen_flow_stopped',
            field=models.NullBooleanField(verbose_name='RE46 oxygen flow stopped'),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='oxygen_full_and_open',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='RE24 oxygen full and open', choices=[(2, 'MMc03 Unknown'), (0, 'MMc01 No'), (1, 'MMc02 Yes')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='perfusate_measure',
            field=models.FloatField(null=True, verbose_name='RE19 pO2 perfusate', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='perfusion_stopped',
            field=models.DateTimeField(null=True, verbose_name='RE20 stop machine perfusion', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='pre_transplant_diuresis',
            field=models.PositiveSmallIntegerField(null=True, verbose_name='RE17 diuresis (ml/24hr)', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='probe_cleaned',
            field=models.NullBooleanField(verbose_name='RE44 temperature and flow probe cleaned'),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='removed_from_machine_at',
            field=models.DateTimeField(null=True, verbose_name='RE23 kidney removed from machine at', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='renal_disease',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='RE15 renal disease', choices=[(1, 'REc04 Glomerular diseases'), (2, 'REc05 Polycystic kidneys'), (3, 'REc06 Uncertain etiology'), (4, 'REc07 Tubular and interstitial diseases'), (5, 'REc08 Retransplant graft failure'), (6, 'REc09 diabetic nephropathyes'), (7, 'REc10 hypertensive nephropathyes'), (8, 'REc11 congenital rare disorders'), (9, 'REc12 renovascular and other diseases'), (10, 'REc13 neoplasms'), (11, 'REc14 other')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='renal_disease_other',
            field=models.CharField(max_length=250, verbose_name='RE16 other renal disease', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='reperfusion_started_at',
            field=models.DateTimeField(null=True, verbose_name='RE35 start reperfusion at', blank=True),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='successful_conclusion',
            field=models.BooleanField(default=False, verbose_name='RE42 successful conclusion'),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='systolic_blood_pressure',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='RE39 systolic blood pressure at reperfusion', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(200)]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='tape_broken',
            field=models.NullBooleanField(verbose_name='RE22 tape over regulator broken'),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='transplant_side',
            field=models.CharField(blank=True, max_length=1, verbose_name='RE29 transplant side', choices=[('L', 'ORc01 Left'), ('R', 'ORc02 Right')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='venous_problems',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='RE32 venous problems', choices=[(1, 'REc23 none'), (2, 'REc24 laceration'), (3, 'REc25 elongation plasty'), (4, 'REc26 other')]),
        ),
        migrations.AlterField(
            model_name='recipient',
            name='venous_problems_other',
            field=models.CharField(max_length=250, verbose_name='RE33 venous problems other', blank=True),
        ),
    ]
