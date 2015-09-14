#!/usr/bin/python
# coding: utf-8
from django.conf.urls import include, url

from . import views

urlpatterns = [
    url(r'person/$', views.StaffPersonListlView.as_view(), name='list'),
    url(r'person/(?P<pk>[0-9]+)/details/$', views.StaffPersonDetailView.as_view(), name='detail'),
    # url(r'person/(?P<pk>[0-9]+)/results/$', views.PersonResultsView.as_view(), name='person_results'),
    url(r'person/add/$', views.StaffPersonCreateView.as_view(), name='add'),
    url(r'person/(?P<pk>[0-9]+)/$', views.StaffPersonCreateView.as_view(), name='update'),
    # url(r'person/(?P<pk>[0-9]+)/delete/$', views.PersonDelete.as_view(), name='person_delete'),
]
