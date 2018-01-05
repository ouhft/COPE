#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.conf.urls import include, url

from . import views

app_name = "staff"
urlpatterns = [
    url(
        regex=r'^technician-autocomplete/$',
        view=views.TechnicianAutoComplete.as_view(),
        name='technician-autocomplete'
    ),

    url(
        regex=r'^add/$',
        view=views.PersonCreateView.as_view(),
        name='add'
    ),
    url(
        regex=r'^(?P<pk>[0-9]+)/details/$',
        view=views.PersonDetailView.as_view(),
        name='detail'
    ),
    url(
        regex=r'^(?P<pk>[0-9]+)/$',
        view=views.PersonUpdateView.as_view(),
        name='update'
    ),
    url(
        regex=r'^$',
        view=views.PersonListView.as_view(),
        name='list'
    ),
]
