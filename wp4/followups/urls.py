#!/usr/bin/python
# coding: utf-8
from django.conf.urls import url

from . import views


urlpatterns = [
    url(
        regex=r'^$',
        view=views.index,
        name='index'
    ),

    url(
        regex=r'^initial/add/$',
        view=views.FollowUpInitialCreateView.as_view(),
        name='initial_add'
    ),
    url(
        regex=r'^initial/(?P<pk>[0-9]+)/details/$',
        view=views.FollowUpInitialDetailView.as_view(),
        name='initial_detail'
    ),
    url(
        regex=r'^initial/(?P<pk>[0-9]+)/$',
        view=views.FollowUpInitialUpdateView.as_view(),
        name='initial_update'
    ),
    url(
        regex=r'^initial/$',
        view=views.FollowUpInitialListView.as_view(),
        name='initial_list'
    ),

    url(
        regex=r'^month3/add/$',
        view=views.FollowUp3MCreateView.as_view(),
        name='month3_add'
    ),
    url(
        regex=r'^month3/(?P<pk>[0-9]+)/details/$',
        view=views.FollowUp3MDetailView.as_view(),
        name='month3_detail'
    ),
    url(
        regex=r'^month3/(?P<pk>[0-9]+)/$',
        view=views.FollowUp3MUpdateView.as_view(),
        name='month3_update'
    ),
    url(
        regex=r'^month3/$',
        view=views.FollowUp3MListView.as_view(),
        name='month3_list'
    ),

    url(
        regex=r'^month6/add/$',
        view=views.FollowUp6MCreateView.as_view(),
        name='month6_add'
    ),
    url(
        regex=r'^month6/(?P<pk>[0-9]+)/details/$',
        view=views.FollowUp6MDetailView.as_view(),
        name='month6_detail'
    ),
    url(
        regex=r'^month6/(?P<pk>[0-9]+)/$',
        view=views.FollowUp6MUpdateView.as_view(),
        name='month6_update'
    ),
    url(
        regex=r'^month6/$',
        view=views.FollowUp6MListView.as_view(),
        name='month6_list'
    ),

    url(
        regex=r'^final/add/$',
        view=views.FollowUp1YCreateView.as_view(),
        name='final_add'
    ),
    url(
        regex=r'^final/(?P<pk>[0-9]+)/details/$',
        view=views.FollowUp1YDetailView.as_view(),
        name='final_detail'
    ),
    url(
        regex=r'^final/(?P<pk>[0-9]+)/$',
        view=views.FollowUp1YUpdateView.as_view(),
        name='final_update'
    ),
    url(
        regex=r'^final/$',
        view=views.FollowUp1YListView.as_view(),
        name='final_list'
    ),
]
