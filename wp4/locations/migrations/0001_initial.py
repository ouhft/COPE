# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import migrations, models
import django.utils.timezone
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
    ]

    operations = [
        migrations.CreateModel(
            name='Hospital',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('name', models.CharField(max_length=100, verbose_name='HO01 hospital name')),
                ('country', models.PositiveSmallIntegerField(verbose_name='HO02 country', choices=[(1, 'MM10 United Kingdom'), (4, 'MM11 Belgium'), (5, 'MM12 Netherlands')])),
                ('is_active', models.BooleanField(default=True, verbose_name='HO03 is active')),
                ('is_project_site', models.BooleanField(default=False, verbose_name='HO04 is project site')),
                ('created_on', models.DateTimeField(default=django.utils.timezone.now)),
                ('created_by', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
            options={
                'ordering': ['country', 'name'],
                'verbose_name': 'HOm1 hospital',
                'verbose_name_plural': 'HOm2 hospitals',
            },
        ),
    ]
