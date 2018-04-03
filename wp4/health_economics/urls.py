#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.conf.urls import url

from . import views

app_name = "health-economics"
urlpatterns = [
    url(
        regex=r'^$',
        view=views.index,
        name='index'
    ),

    url(
        regex=r'^add/$',
        view=views.QualityOfLifeCreateView.as_view(),
        name='add'
    ),
    url(
        regex=r'^(?P<pk>[0-9]+)/details/$',
        view=views.QualityOfLifeDetailView.as_view(),
        name='detail'
    ),
    url(
        regex=r'^(?P<pk>[0-9]+)/$',
        view=views.QualityOfLifeUpdateView.as_view(),
        name='update'
    ),
    url(
        regex=r'^list/baseline/$',
        view=views.QualityOfLifeBaselineListView.as_view(),
        name='list-baseline'
    ),
    url(
        regex=r'^list/month3/$',
        view=views.QualityOfLifeMonth3ListView.as_view(),
        name='list-month3'
    ),
    url(
        regex=r'^list/final/$',
        view=views.QualityOfLifeFinalListView.as_view(),
        name='list-final'
    ),
    url(
        regex=r'^list/$',
        view=views.QualityOfLifeListView.as_view(),
        name='list'
    ),
]