# Generated by Django 2.2.2 on 2019-06-17 16:11

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('samples', '0004_auto_20180108_1433'),
    ]

    operations = [
        migrations.AlterField(
            model_name='bloodsample',
            name='record_locked',
            field=models.BooleanField(default=False, help_text='Locked by the admin team. This can only be reversed by the System Administrator'),
        ),
        migrations.AlterField(
            model_name='event',
            name='record_locked',
            field=models.BooleanField(default=False, help_text='Locked by the admin team. This can only be reversed by the System Administrator'),
        ),
        migrations.AlterField(
            model_name='perfusatesample',
            name='record_locked',
            field=models.BooleanField(default=False, help_text='Locked by the admin team. This can only be reversed by the System Administrator'),
        ),
        migrations.AlterField(
            model_name='tissuesample',
            name='record_locked',
            field=models.BooleanField(default=False, help_text='Locked by the admin team. This can only be reversed by the System Administrator'),
        ),
        migrations.AlterField(
            model_name='urinesample',
            name='record_locked',
            field=models.BooleanField(default=False, help_text='Locked by the admin team. This can only be reversed by the System Administrator'),
        ),
    ]
