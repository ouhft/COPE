#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.conf.urls import url

from . import views


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

]
