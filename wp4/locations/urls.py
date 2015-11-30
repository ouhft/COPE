#!/usr/bin/python
# coding: utf-8
from django.conf.urls import include, url

from . import views

urlpatterns = [
    url(
        regex=r'^add/$',
        view=views.HospitalCreateView.as_view(),
        name='add'
    ),
    url(
        regex=r'^(?P<pk>[0-9]+)/details/$',
        view=views.HospitalDetailView.as_view(),
        name='detail'
    ),
    url(
        regex=r'^(?P<pk>[0-9]+)/$',
        view=views.HospitalUpdateView.as_view(),
        name='update'
    ),
    url(
        regex=r'^$',
        view=views.HospitalListView.as_view(),
        name='list'
    ),
]
