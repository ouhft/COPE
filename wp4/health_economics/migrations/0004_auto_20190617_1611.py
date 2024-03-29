# Generated by Django 2.2.2 on 2019-06-17 16:11

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('health_economics', '0003_auto_20190222_1903'),
    ]

    operations = [
        migrations.AlterField(
            model_name='qualityoflife',
            name='record_locked',
            field=models.BooleanField(default=False, help_text='Locked by the admin team. This can only be reversed by the System Administrator'),
        ),
        migrations.AlterField(
            model_name='resourcehospitaladmission',
            name='record_locked',
            field=models.BooleanField(default=False, help_text='Locked by the admin team. This can only be reversed by the System Administrator'),
        ),
        migrations.AlterField(
            model_name='resourcelog',
            name='record_locked',
            field=models.BooleanField(default=False, help_text='Locked by the admin team. This can only be reversed by the System Administrator'),
        ),
        migrations.AlterField(
            model_name='resourcerehabilitation',
            name='record_locked',
            field=models.BooleanField(default=False, help_text='Locked by the admin team. This can only be reversed by the System Administrator'),
        ),
        migrations.AlterField(
            model_name='resourcevisit',
            name='record_locked',
            field=models.BooleanField(default=False, help_text='Locked by the admin team. This can only be reversed by the System Administrator'),
        ),
    ]
