#!/usr/bin/python
# coding: utf-8
from django.conf.urls import include, url

from . import views


urlpatterns = [
    url(r'^$', views.adverse_events_list, name='list'),
    url(r'^new/(?P<pk>[0-9]+)/$', views.adverse_event_form_new, name='new'),
    url(r'^(?P<pk>[0-9]+)/$', views.adverse_event_form, name='detail'),
]
