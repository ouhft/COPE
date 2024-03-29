#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.conf.urls import url

from . import views


app_name = "compare"
urlpatterns = [
    # ==============================================================  DAL Functions
    url(
        regex=r'^retrieval-team-autocomplete/$',
        view=views.RetrievalTeamAutoComplete.as_view(),
        name='retrieval-team-autocomplete'
    ),
    url(
        regex=r'^adverse-organ-autocomplete/$',
        view=views.AdverseEventOrganAutoComplete.as_view(),
        name='adverse-organ-autocomplete'
    ),

    # ==============================================================  Pages
    url(
        regex=r'^procurement/(?P<pk>[0-9]+)/update/$',
        view=views.procurement_form,
        name='procurement_form'
    ),
    url(
        regex=r'^procurement/(?P<pk>[0-9]+)/$',
        view=views.procurement_view,
        name='procurement_view'
    ),
    url(
        regex=r'^procurement/$',
        view=views.procurement_list,
        name='procurement_list'
    ),

    url(
        regex=r'^transplantation/(?P<pk>[0-9]+)/$',
        view=views.transplantation_form,
        name='transplantation_detail'
    ),
    url(
        regex=r'^transplantation/$',
        view=views.transplantation_list,
        name='transplantation_list'
    ),

    url(
        regex=r'^$',
        view=views.index,
        name='index'
    )
]
