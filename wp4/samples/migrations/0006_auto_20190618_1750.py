# Generated by Django 2.2.2 on 2019-06-18 17:50

from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    dependencies = [
        ('samples', '0005_auto_20190617_1611'),
    ]

    operations = [
        migrations.AlterModelOptions(
            name='wp7record',
            options={'verbose_name': 'WP7 Record', 'verbose_name_plural': 'WP7 Records'},
        ),
        migrations.AlterField(
            model_name='bloodsample',
            name='event',
            field=models.ForeignKey(help_text='Link to an event of type Blood', limit_choices_to={'type': 1}, on_delete=django.db.models.deletion.CASCADE, to='samples.Event'),
        ),
        migrations.AlterField(
            model_name='bloodsample',
            name='record_locked',
            field=models.BooleanField(default=False, help_text='Locked by the admin team. This can only be reversed by the System Administrator', verbose_name='ACM01 Record Locked'),
        ),
        migrations.AlterField(
            model_name='event',
            name='record_locked',
            field=models.BooleanField(default=False, help_text='Locked by the admin team. This can only be reversed by the System Administrator', verbose_name='ACM01 Record Locked'),
        ),
        migrations.AlterField(
            model_name='perfusatesample',
            name='event',
            field=models.ForeignKey(help_text='Link to an event of type Perfusate', limit_choices_to={'type': 3}, on_delete=django.db.models.deletion.CASCADE, to='samples.Event'),
        ),
        migrations.AlterField(
            model_name='perfusatesample',
            name='record_locked',
            field=models.BooleanField(default=False, help_text='Locked by the admin team. This can only be reversed by the System Administrator', verbose_name='ACM01 Record Locked'),
        ),
        migrations.AlterField(
            model_name='tissuesample',
            name='event',
            field=models.ForeignKey(help_text='Link to an event of type Tissue', limit_choices_to={'type': 4}, on_delete=django.db.models.deletion.CASCADE, to='samples.Event'),
        ),
        migrations.AlterField(
            model_name='tissuesample',
            name='record_locked',
            field=models.BooleanField(default=False, help_text='Locked by the admin team. This can only be reversed by the System Administrator', verbose_name='ACM01 Record Locked'),
        ),
        migrations.AlterField(
            model_name='urinesample',
            name='event',
            field=models.ForeignKey(help_text='Link to an event of type Urine', limit_choices_to={'type': 2}, on_delete=django.db.models.deletion.CASCADE, to='samples.Event'),
        ),
        migrations.AlterField(
            model_name='urinesample',
            name='record_locked',
            field=models.BooleanField(default=False, help_text='Locked by the admin team. This can only be reversed by the System Administrator', verbose_name='ACM01 Record Locked'),
        ),
    ]