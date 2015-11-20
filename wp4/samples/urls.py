#!/usr/bin/python
# coding: utf-8
from django.conf.urls import url

from . import views


urlpatterns = [
    # url(regex=r"^event/$", view=views.EventListView.as_view(), name="event_list"),
    # url(regex=r"^event/(?P<pk>\d+)/$", view=views.EventDetailView.as_view(), name="event_detail"),
    # url(regex=r"^event/(?P<pk>\d+)/add$", view=views.EventCreateView.as_view(), name="event_create"),

    # url(regex=r"^worksheet/$", view=views.WorksheetListView.as_view(), name="worksheet_list"),
    # url(regex=r"^worksheet/(?P<pk>\d+)/$", view=views.WorksheetDetailView.as_view(), name="worksheet_detail"),


    url(r'^$', views.sample_home, name='home'),

    # url(r'^type/(?P<type>[0-9]+)/$', views.sample_editor, name='new'),
    # url(r'^(?P<pk>[0-9]+)/$', views.sample_editor, name='update'),
]
