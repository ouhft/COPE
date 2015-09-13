# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
import django.utils.timezone
from django.conf import settings
import django.core.validators


class Migration(migrations.Migration):

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
        ('compare', '0011_auto_20150816_2028'),
    ]

    operations = [
        migrations.CreateModel(
            name='Recipient',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('version', models.PositiveIntegerField(default=0)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('number', models.CharField(max_length=20, verbose_name='DO30 NHSBT Number')),
                ('date_of_birth', models.DateField(null=True, verbose_name='DO32 date of birth', blank=True)),
                ('gender', models.CharField(default=b'M', max_length=1, verbose_name='DO37 gender', choices=[(b'M', 'DO20 Male'), (b'F', 'DO21 Female')])),
                ('weight', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO39 Weight (kg)', validators=[django.core.validators.MinValueValidator(20), django.core.validators.MaxValueValidator(200)])),
                ('height', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO40 Height (cm)', validators=[django.core.validators.MinValueValidator(100), django.core.validators.MaxValueValidator(250)])),
                ('ethnicity', models.IntegerField(blank=True, null=True, verbose_name='DO41 ethnicity', choices=[(1, 'DO22 Caucasian'), (2, 'DO23 Black'), (3, 'DO24 Other')])),
                ('blood_group', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO42 blood group', choices=[(1, b'O'), (2, b'A'), (3, b'B'), (4, b'AB'), (5, 'DO29 Unknown')])),
                ('call_received', models.DateTimeField(null=True, verbose_name='DO05 Consultant to MTO called at', blank=True)),
                ('scheduled_start', models.DateTimeField(null=True, verbose_name='DO07 time of withdrawal therapy', blank=True)),
                ('technician_arrival', models.DateTimeField(null=True, verbose_name='DO08 arrival time of technician at hub', blank=True)),
                ('depart_perfusion_centre', models.DateTimeField(null=True, verbose_name='DO10 departure from base hospital at', blank=True)),
                ('arrival_at_donor_hospital', models.DateTimeField(null=True, verbose_name='DO11 arrival at donor hospital', blank=True)),
                ('reallocated', models.BooleanField(default=False, verbose_name='reallocated')),
                ('reallocation_reason', models.PositiveSmallIntegerField(verbose_name='reason for re-allocation', choices=[(1, 'RE01 Positive crossmatch'), (2, 'RE02 Unknown'), (3, 'RE03 Other')])),
                ('reallocation_reason_other', models.CharField(max_length=250, null=True, verbose_name='other reason', blank=True)),
                ('renal_disease', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO54 renal disease', choices=[(1, 'Glomerular diseases'), (2, 'Polycystic kidneys'), (3, 'Uncertain etiology'), (4, 'Tubular and interstitial diseases'), (5, 'Retransplant graft failure'), (6, 'diabetic nephropathyes'), (7, 'hypertensive nephropathyes'), (8, 'congenital rare disorders'), (9, 'renovascular and other diseases'), (10, 'neoplasms'), (11, 'other')])),
                ('renal_disease_other', models.CharField(max_length=250, null=True, verbose_name='DO55 other renal disease', blank=True)),
                ('pre_transplant_diuresis', models.PositiveSmallIntegerField(null=True, verbose_name='DO61 diuresis (ml/24hr)', blank=True)),
                ('knife_to_skin', models.DateTimeField(null=True, verbose_name='DO85 knife to skin time', blank=True)),
                ('perfusate_measure', models.FloatField(null=True, verbose_name='pO2 perfusate', blank=True)),
                ('perfusion_stopped', models.DateTimeField(null=True, verbose_name='stop machine perfusion', blank=True)),
                ('organ_cold_stored', models.BooleanField(default=False, verbose_name='kidney was cold stored?')),
                ('tape_broken', models.NullBooleanField(verbose_name='tape over regulator broken')),
                ('removed_from_machine_at', models.DateTimeField(null=True, verbose_name='kidney removed from matchine at', blank=True)),
                ('oxygen_full_and_open', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='oxygen full and open', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')])),
                ('organ_untransplantable', models.NullBooleanField(verbose_name='kidney discarded')),
                ('organ_untransplantable_reason', models.CharField(max_length=250, null=True, verbose_name='untransplantable because', blank=True)),
                ('anesthesia_started_at', models.DateTimeField(null=True, verbose_name='start anesthesia at', blank=True)),
                ('incision', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='incision', choices=[(1, 'midline laparotomy'), (2, 'hockey stick'), (3, 'unknown')])),
                ('transplant_side', models.CharField(max_length=1, verbose_name='transplant side', choices=[(b'L', 'OR01 Left'), (b'R', 'OR02 Right')])),
                ('arterial_problems', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='arterial problems', choices=[(1, 'None'), (2, 'ligated polar artery'), (3, 'reconstructed polar artery'), (4, 'repaired intima dissection'), (5, 'other')])),
                ('venous_problems', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='venous problems', choices=[(1, 'none'), (2, 'laceration'), (3, 'elongation plasty'), (4, 'other')])),
                ('anastomosis_started_at', models.DateTimeField(null=True, verbose_name='start anastomosis at', blank=True)),
                ('reperfusion_started_at', models.DateTimeField(null=True, verbose_name='start reperfusion at', blank=True)),
                ('mannitol_used', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='mannitol used', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')])),
                ('other_diurectics', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='other diurectics used', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')])),
                ('other_diurectics_details', models.CharField(max_length=250, null=True, verbose_name='other diurectics detail', blank=True)),
                ('systolic_blood_pressure', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='systolic blood pressure at reperfusion', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(200)])),
                ('cvp', models.PositiveSmallIntegerField(null=True, verbose_name='cvp at reperfusion', blank=True)),
                ('intra_operative_diuresis', models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='intra-operative diuresis', choices=[(2, 'MM03 Unknown'), (0, 'MM01 No'), (1, 'MM02 Yes')])),
                ('probe_cleaned', models.NullBooleanField(verbose_name='temperature and flow probe cleaned')),
                ('ice_removed', models.NullBooleanField(verbose_name='ice and water removed')),
                ('oxygen_flow_stopped', models.NullBooleanField(verbose_name='oxygen flow stopped')),
                ('oxygen_bottle_removed', models.NullBooleanField(verbose_name='oxygen bottle removed')),
                ('box_cleaned', models.NullBooleanField(verbose_name='box kidney assist cleaned')),
                ('batteries_charged', models.NullBooleanField(verbose_name='batteries charged')),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('organ', models.ForeignKey(to='compare.Organ')),
            ],
            options={
                'verbose_name': 'REm1 recipient',
                'verbose_name_plural': 'REm2 recipients',
            },
        ),
        migrations.CreateModel(
            name='StaffPerson',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('version', models.PositiveIntegerField(default=0)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('first_names', models.CharField(max_length=50, verbose_name='PE10 first names')),
                ('last_names', models.CharField(max_length=50, verbose_name='PE11 last names')),
                ('job', models.CharField(max_length=2, verbose_name='PE12 job', choices=[(b'PT', 'PE01 Perfusion Technician'), (b'TC', 'PE02 Transplant Co-ordinator'), (b'RN', 'PE03 Research Nurse / Follow-up'), (b'NC', 'PE04 National Co-ordinator'), (b'CC', 'PE05 Central Co-ordinator'), (b'BC', 'PE06 Biobank Co-ordinator'), (b'S', 'PE07 Statistician'), (b'SA', 'PE08 Sys-admin')])),
                ('telephone', models.CharField(max_length=20, verbose_name='PE13 telephone number')),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
                ('user', models.OneToOneField(related_name='profile', null=True, blank=True, to=settings.AUTH_USER_MODEL, verbose_name='PE14 related user account')),
            ],
            options={
                'verbose_name': 'PEm1 person',
                'verbose_name_plural': 'PEm2 people',
            },
        ),
        migrations.RemoveField(
            model_name='person',
            name='created_by',
        ),
        migrations.RemoveField(
            model_name='person',
            name='user',
        ),
        migrations.AlterModelOptions(
            name='hospital',
            options={'ordering': ['country', 'name'], 'verbose_name': 'HOm1 hospital', 'verbose_name_plural': 'HOm2 hospitals'},
        ),
        migrations.AlterModelOptions(
            name='retrievalteam',
            options={'ordering': ['centre_code'], 'verbose_name': 'RTm1 retrieval team', 'verbose_name_plural': 'RTm2 retrieval teams'},
        ),
        migrations.RemoveField(
            model_name='hospital',
            name='centre_code',
        ),
        migrations.AddField(
            model_name='retrievalteam',
            name='centre_code',
            field=models.PositiveSmallIntegerField(default=0, verbose_name='HO02 centre code', validators=[django.core.validators.MinValueValidator(10), django.core.validators.MaxValueValidator(99)]),
            preserve_default=False,
        ),
        migrations.AlterField(
            model_name='donor',
            name='diagnosis',
            field=models.PositiveSmallIntegerField(blank=True, null=True, verbose_name='DO54 diagnosis', choices=[(1, 'DO50 Cerebrovascular Accident'), (2, 'DO51 Hypoxia'), (3, 'DO52 Trauma'), (4, 'DO53 Other')]),
        ),
        migrations.AlterField(
            model_name='donor',
            name='perfusion_technician',
            field=models.ForeignKey(related_name='donor_perfusion_technician_set', verbose_name='DO03 name of transplant technician', to='compare.StaffPerson'),
        ),
        migrations.AlterField(
            model_name='donor',
            name='transplant_coordinator',
            field=models.ForeignKey(related_name='donor_transplant_coordinator_set', verbose_name='DO04 name of the SN-OD', blank=True, to='compare.StaffPerson', null=True),
        ),
        migrations.AlterField(
            model_name='hospital',
            name='country',
            field=models.PositiveSmallIntegerField(verbose_name='HO03 country', choices=[(1, 'MM10 United Kingdom'), (4, 'MM11 Belgium'), (5, 'MM12 Netherlands')]),
        ),
        migrations.AlterOrderWithRespectTo(
            name='retrievalteam',
            order_with_respect_to=None,
        ),
        migrations.DeleteModel(
            name='Person',
        ),
        migrations.AddField(
            model_name='recipient',
            name='perfusion_technician',
            field=models.ForeignKey(related_name='recipient_perfusion_technician_set', verbose_name='DO03 name of transplant technician', to='compare.StaffPerson'),
        ),
        migrations.AddField(
            model_name='recipient',
            name='reallocation_recipient',
            field=models.OneToOneField(default=None, to='compare.Recipient'),
        ),
        migrations.AddField(
            model_name='recipient',
            name='transplant_coordinator',
            field=models.ForeignKey(related_name='recipient_transplant_coordinator_set', verbose_name='DO04 name of the SN-OD', blank=True, to='compare.StaffPerson', null=True),
        ),
        migrations.AddField(
            model_name='recipient',
            name='transplant_hospital',
            field=models.ForeignKey(verbose_name='DO06 donor hospital', blank=True, to='compare.Hospital', null=True),
        ),
        migrations.RemoveField(
            model_name='retrievalteam',
            name='name',
        ),
    ]
