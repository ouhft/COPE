#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.conf.urls import include, url

from .views import report_procurement, report_organ, report_allocations, report_adverse_events
from .views import administrator_index, administrator_uk_list, administrator_europe_list
from .views import administrator_procurement_pairs, administrator_transplantation_sites, administrator_sae_sites
from .views import flowchart
from .views import dmc_secondary_outcomes, dmc_death_summaries

urlpatterns = [
    # Statisticians' Reports
    url(
        regex=r'^stats/reports/procurement$',
        view=report_procurement,
        name='stats_p'
    ),
    url(
        regex=r'^stats/reports/organs$',
        view=report_organ,
        name='stats_o'
    ),
    url(
        regex=r'^stats/reports/allocations$',
        view=report_allocations,
        name='stats_a'
    ),
    url(
        regex=r'^stats/reports/adverse-events$',
        view=report_adverse_events,
        name='stats_ae'
    ),

    # Administrator Reports
    url(
        regex=r'^procurement-pairs$',
        view=administrator_procurement_pairs,
        name='procurement_pairs'
    ),
    url(
        regex=r'^transplantation-sites$',
        view=administrator_transplantation_sites,
        name='transplantation_sites'
    ),
    url(
        regex=r'^sae-sites$',
        view=administrator_sae_sites,
        name='sae_sites'
    ),
    url(
        regex=r'^europe-list$',
        view=administrator_europe_list,
        name='europe_list'
    ),

    url(
        regex=r'^uk-list$',
        view=administrator_uk_list,
        name='uk_list'
    ),
    url(
        regex=r'^flowchart',
        view=flowchart,
        name='flowchart'
    ),

    # DMC Reports
    url(
        regex=r'^dmc/secondary-outcomes',
        view=dmc_secondary_outcomes,
        name='dmc_secondary_outcomes'
    ),
    url(
        regex=r'^dmc/death-summaries',
        view=dmc_death_summaries,
        name='dmc_death_summaries'
    ),

    # Admin tools


    # Admin Home
    url(
        regex=r'^$',
        view=administrator_index,
        name='index'
    ),
]
