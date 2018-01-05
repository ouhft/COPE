#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.conf.urls import url

from . import views


app_name = "samples"
urlpatterns = [
    url(
        regex=r'^$',
        view=views.index,
        name='index'
    ),

    # ======================  Events
    url(
        regex=r'^event/(?P<pk>[0-9]+)/details/$$',
        view=views.EventDetailView.as_view(),
        name='event_detail'
    ),
    url(
        regex=r'^event/(?P<pk>[0-9]+)/$$',
        view=views.EventUpdateView.as_view(),
        name='event_update'
    ),
    url(
        regex=r'^event/$',
        view=views.EventListView.as_view(),
        name='event_list'
    ),

    # ======================  Donors

    url(
        regex=r'^donor/(?P<pk>[0-9]+)/details/$$',
        view=views.DonorSamplesDetailView.as_view(),
        name='donor_detail'
    ),

    url(
        regex=r'^donor/$',
        view=views.DonorSamplesListView.as_view(),
        name='donor_list'
    ),

    # ======================  Organs

    url(
        regex=r'^organ/(?P<pk>[0-9]+)/details/$$',
        view=views.OrganSamplesDetailView.as_view(),
        name='organ_detail'
    ),

    url(
        regex=r'^organ/$',
        view=views.OrganSamplesListView.as_view(),
        name='organ_list'
    ),

    # ======================  Recipients

    url(
        regex=r'^recipient/(?P<pk>[0-9]+)/details/$$',
        view=views.RecipientSamplesDetailView.as_view(),
        name='recipient_detail'
    ),

    url(
        regex=r'^recipient/$',
        view=views.RecipientSamplesListView.as_view(),
        name='recipient_list'
    ),
]
