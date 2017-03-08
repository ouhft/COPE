#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.conf.urls import url

from . import views


urlpatterns = [
    url(r'^(?P<pk>[0-9]+)/$', views.sample_form, name='update'),
    url(
        regex=r'^$',
        view=views.EventListView.as_view(),
        name='home'
    ),
]
