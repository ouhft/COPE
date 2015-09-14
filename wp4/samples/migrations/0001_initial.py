# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
import django.utils.timezone
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
    ]

    operations = [
        migrations.CreateModel(
            name='Sample',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('type', models.PositiveSmallIntegerField(verbose_name='SA05 sample type', choices=[(1, 'SA10 Donor blood 1'), (2, 'SA11 Donor blood 2'), (3, 'SA12 Donor urine 1'), (4, 'SA13 Donor urine 2'), (5, 'SA14 Kidney perfusate 1'), (6, 'SA15 Kidney perfusate 1'), (7, 'SA16 Kidney perfusate 1'), (8, 'SA17 Recipient blood 1'), (9, 'SA18 Recipient blood 1'), (10, 'SA19 Kidney tissue 1')])),
                ('barcode', models.CharField(max_length=20, verbose_name='SA01 barcode number')),
                ('taken_at', models.DateTimeField(verbose_name='SA02 date and time taken')),
                ('centrifugation', models.DateTimeField(null=True, verbose_name='SA03 centrifugation', blank=True)),
                ('comment', models.CharField(max_length=2000, verbose_name='SA04 comment', blank=True)),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
            options={
                'ordering': ['taken_at'],
                'verbose_name': 'SAm1 sample',
                'verbose_name_plural': 'SAm2 samples',
            },
        ),
    ]
