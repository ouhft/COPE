# Generated by Django 2.0.1 on 2018-01-08 14:33

from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    dependencies = [
        ('health_economics', '0001_initial'),
    ]

    operations = [
        migrations.AlterField(
            model_name='qualityoflife',
            name='recipient',
            field=models.ForeignKey(on_delete=django.db.models.deletion.PROTECT, to='compare.Recipient'),
        ),
        migrations.AlterField(
            model_name='resourcehospitaladmission',
            name='log',
            field=models.ForeignKey(on_delete=django.db.models.deletion.PROTECT, related_name='hospitalisations', to='health_economics.ResourceLog'),
        ),
        migrations.AlterField(
            model_name='resourcelog',
            name='recipient',
            field=models.ForeignKey(on_delete=django.db.models.deletion.PROTECT, to='compare.Recipient'),
        ),
        migrations.AlterField(
            model_name='resourcerehabilitation',
            name='log',
            field=models.ForeignKey(on_delete=django.db.models.deletion.PROTECT, related_name='rehabilitations', to='health_economics.ResourceLog'),
        ),
        migrations.AlterField(
            model_name='resourcevisit',
            name='log',
            field=models.ForeignKey(on_delete=django.db.models.deletion.PROTECT, related_name='visits', to='health_economics.ResourceLog'),
        ),
    ]
