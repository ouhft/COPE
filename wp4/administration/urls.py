#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.conf.urls import include, url

from .views import core, data_extracts, administrator, dmc_reports

urlpatterns = [
    # Data Extracts (aka: Statisticians' Reports)
    url(
        regex=r'^stats/reports/procurement$',
        view=data_extracts.report_procurement,
        name='stats_p'
    ),
    url(
        regex=r'^stats/reports/organs$',
        view=data_extracts.report_organ,
        name='stats_o'
    ),
    url(
        regex=r'^stats/reports/allocations$',
        view=data_extracts.report_allocations,
        name='stats_a'
    ),
    url(
        regex=r'^stats/reports/adverse-events$',
        view=data_extracts.report_adverse_events,
        name='stats_ae'
    ),


    # Administrator Reports
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

    # DMC Reports
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

    # Admin tools


    # Admin Home
    url(
        regex=r'^$',
        view=core.index,
        name='index'
    ),
]
