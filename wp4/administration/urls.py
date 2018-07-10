#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.conf.urls import include, url
from django.urls import path

from .views import core, data_extracts, administrator, completeness, dmc_reports, biobank

app_name = "administration"
urlpatterns = [
    # =================================================================  Data Extracts (aka: Statisticians' Reports)
    path(
        'stats/reports/data-simplified',
        view=data_extracts.report_data_flattened,
        name='stats_simple'
    ),
    path(
        'stats/reports/procurement',
        view=data_extracts.report_procurement,
        name='stats_p'
    ),
    path(
        'stats/reports/organs',
        view=data_extracts.report_organ,
        name='stats_o'
    ),
    path(
        'stats/reports/allocations',
        view=data_extracts.report_allocations,
        name='stats_a'
    ),
    path(
        'stats/reports/adverse-events',
        view=data_extracts.report_adverse_events,
        name='stats_ae'
    ),


    # =================================================================  Administrator Reports
    url(
        regex=r'^europe-list$',
        view=administrator.offline_europe_list,
        name='europe_list'
    ),
    url(
        regex=r'^uk-list$',
        view=administrator.offline_uk_list,
        name='uk_list'
    ),

    url(
        regex=r'^procurement-pairs$',
        view=administrator.procurement_pairs,
        name='procurement_pairs'
    ),
    url(
        regex=r'^transplantation-sites$',
        view=administrator.transplantation_sites,
        name='transplantation_sites'
    ),

    url(
        regex=r'^sae-sites$',
        view=administrator.sae_sites,
        name='sae_sites'
    ),
    url(
        regex=r'^flowchart',
        view=administrator.flowchart,
        name='flowchart'
    ),
    url(
        regex=r'^completed-pairs',
        view=administrator.completed_pairs,
        name='completed_pairs'
    ),
    url(
        regex=r'^followups',
        view=administrator.followups,
        name='followups'
    ),

    url(
        regex=r'^demographics/data-linkage$',
        view=administrator.demographics_data_linkage,
        name='demographics_data_linkage'
    ),


    # =================================================================  Completeness Reports
    url(
        regex=r'^completeness/procurement$',
        view=completeness.procurement,
        name='completeness_procurement'
    ),
    url(
        regex=r'^completeness/transplant-per-centre',
        view=completeness.transplant_per_centre,
        name='completeness_tpc'
    ),
    path(
        'completeness/donor-summary/<int:donor_pk>/',
        view=completeness.donor_summary,
        name='stats_simple'
    ),
    path(
        'completeness/donor-summary/<str:trial_id>/',
        view=completeness.donor_summary_by_trial_id,
        name='stats_simple'
    ),

    # =================================================================  Biobank reports
    url(
        regex=r'^biobank/blood-collection',
        view=biobank.blood_collection,
        name='biobank_blood_collection'
    ),
    url(
        regex=r'^biobank/urine-collection',
        view=biobank.urine_collection,
        name='biobank_urine_collection'
    ),
    url(
        regex=r'^biobank/tissue-collection',
        view=biobank.tissue_collection,
        name='biobank_tissue_collection'
    ),
    url(
        regex=r'^biobank/perfusate-collection',
        view=biobank.perfusate_collection,
        name='biobank_perfusate_collection'
    ),
    url(
        regex=r'^biobank/unmatched-samples',
        view=biobank.unmatched_samples,
        name='biobank_unmatched_samples'
    ),
    url(
        regex=r'^biobank/paired-biopsies',
        view=biobank.paired_biopsies,
        name='biobank_paired_biopsies'
    ),
    url(
        regex=r'biobank/wp7-upload',
        view=biobank.wp7_file_form,
        name='biobank_wp7_form'
    ),
    url(
        regex=r'biobank/wp7-download',
        view=biobank.export_for_wp7,
        name='biobank_wp7_export'
    ),

    # =================================================================  DMC Reports
    url(
        regex=r'^dmc/death-summaries/open',
        view=dmc_reports.death_summaries,
        name='dmc_death_summaries_open',
        kwargs=dict(open_report=True),
    ),
    url(
        regex=r'^dmc/death-summaries',
        view=dmc_reports.death_summaries,
        name='dmc_death_summaries'
    ),
    url(
        regex=r'^dmc/permanent-impairment/open',
        view=dmc_reports.permanent_impairment,
        name='dmc_permanent_impairment_open',
        kwargs=dict(open_report=True),
    ),
    url(
        regex=r'^dmc/permanent-impairment',
        view=dmc_reports.permanent_impairment,
        name='dmc_permanent_impairment'
    ),
    url(
        regex=r'^dmc/graft-failures/open',
        view=dmc_reports.graft_failures,
        name='dmc_graft_failures_open',
        kwargs=dict(open_report=True),
    ),
    url(
        regex=r'^dmc/graft-failures',
        view=dmc_reports.graft_failures,
        name='dmc_graft_failures'
    ),
    url(
        regex=r'^dmc/adverse-events/open',
        view=dmc_reports.adverse_events,
        name='dmc_adverse_events_open',
        kwargs=dict(open_report=True),
    ),
    url(
        regex=r'^dmc/adverse-events',
        view=dmc_reports.adverse_events,
        name='dmc_adverse_events'
    ),
    url(
        regex=r'^dmc/serious-events/open',
        view=dmc_reports.serious_events,
        name='dmc_serious_events_open',
        kwargs=dict(open_report=True),
    ),
    url(
        regex=r'^dmc/serious-events',
        view=dmc_reports.serious_events,
        name='dmc_serious_events'
    ),

    # ================================================================= Admin Home
    url(
        regex=r'^$',
        view=core.index,
        name='index'
    ),
]
