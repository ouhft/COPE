#!/usr/bin/python
# coding: utf-8
from django.conf.urls import include, url

from . import views


urlpatterns = [
    url(r'^type/(?P<type>[0-9]+)/$', views.sample_editor, name='new'),
    url(r'^(?P<pk>[0-9]+)/$', views.sample_editor, name='update'),
]
