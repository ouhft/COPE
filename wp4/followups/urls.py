#!/usr/bin/python
# coding: utf-8
from django.conf.urls import url

from . import views


urlpatterns = [
    url(r'^$', views.FollowUpList.as_view(), name='index'),

    url(r'^initial/$', views.FollowUpInitialList.as_view(), name='initial_list'),
    url(r'^initial/(?P<pk>[0-9]+)/$', views.FollowUpInitialDetail.as_view(), name='initial_detail'),
    url(r'^initial/form/$', views.FollowUpInitialAdd.as_view(), name='initial_add'),
    url(r'^initial/form/(?P<pk>[0-9]+)/$', views.FollowUpInitialUpdate.as_view(), name='initial_update'),
]
