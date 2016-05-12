#!/usr/bin/python
# coding: utf-8
from django.conf.urls import url

from . import views


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
        regex=r'^list/$',
        view=views.QualityOfLifeListView.as_view(),
        name='list'
    ),
]