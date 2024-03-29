# Generated by Django 2.0.4 on 2018-04-03 10:23

from django.db import migrations


class Migration(migrations.Migration):

    dependencies = [
        ('compare', '0007_organ_excluded_from_analysis_because'),
    ]

    operations = [
        migrations.AlterModelOptions(
            name='organ',
            options={'base_manager_name': 'objects', 'default_manager_name': 'objects', 'ordering': ['id'], 'permissions': (('view_organ', 'Can only view the data'), ('restrict_to_national', 'Can only use data from the same location country'), ('restrict_to_local', 'Can only use data from a specific location')), 'verbose_name': 'ORm1 organ', 'verbose_name_plural': 'ORm2 organs'},
        ),
        migrations.AlterModelOptions(
            name='procurementresource',
            options={'ordering': ['id'], 'permissions': (('view_procurementresource', 'Can only view the data'),), 'verbose_name': 'PRm1 procurement resource', 'verbose_name_plural': 'PRm2 procurement resources'},
        ),
    ]
