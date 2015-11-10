#!/usr/bin/python
# coding: utf-8
from django.conf.urls import url

from . import views


urlpatterns = [
    url(r'$', views.sample_home, name='home'),
    url(r'^type/(?P<type>[0-9]+)/$', views.sample_editor, name='new'),
    url(r'^(?P<pk>[0-9]+)/$', views.sample_editor, name='update'),
]
