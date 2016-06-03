#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.conf.urls import include, url

from . import views

urlpatterns = [
    url(
        regex=r'^technician-autocomplete/$',
        view=views.TechnicianAutoComplete.as_view(),
        name='technician-autocomplete'
    ),

    url(
        regex=r'^add/$',
        view=views.StaffPersonCreateView.as_view(),
        name='add'
    ),
    url(
        regex=r'^(?P<pk>[0-9]+)/details/$',
        view=views.StaffPersonDetailView.as_view(),
        name='detail'
    ),
    url(
        regex=r'^(?P<pk>[0-9]+)/$',
        view=views.StaffPersonUpdateView.as_view(),
        name='update'
    ),
    url(
        regex=r'^$',
        view=views.StaffPersonListView.as_view(),
        name='list'
    ),
]
