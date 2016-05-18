#!/usr/bin/python
# coding: utf-8
from django.conf.urls import url

from . import views


urlpatterns = [
    url(r'^(?P<pk>[0-9]+)/$', views.sample_form, name='update'),
    # url(r'^$', views.sample_home, name='home'),
    url(
        regex=r'^$',
        view=views.WorksheetListView.as_view(),
        name='home'
    ),
]
