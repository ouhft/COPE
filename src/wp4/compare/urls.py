from django.conf.urls import include, url

from . import views

urlpatterns = [
    # url(r'person/$', views.PersonIndexView.as_view(), name='person_index'),
    # url(r'person/(?P<pk>[0-9]+)/details/$', views.PersonDetailView.as_view(), name='person_detail'),
    # url(r'person/(?P<pk>[0-9]+)/results/$', views.PersonResultsView.as_view(), name='person_results'),
    # url(r'person/add/$', views.PersonCreate.as_view(), name='person_add'),
    # url(r'person/(?P<pk>[0-9]+)/$', views.PersonUpdate.as_view(), name='person_update'),
    # url(r'person/(?P<pk>[0-9]+)/delete/$', views.PersonDelete.as_view(), name='person_delete'),
    #
    # url(r'teams/$', views.RetrievalTeamIndexView.as_view(), name='teams_index'),
    # url(r'teams/(?P<pk>[0-9]+)/$', views.RetrievalTeamDetailView.as_view(), name='teams_detail'),
    # url(r'teams/(?P<pk>[0-9]+)/results/$', views.RetrievalTeamResultsView.as_view(), name='teams_results'),
    # url(r'teams/(?P<question_id>[0-9]+)/vote/$', views.vote, name='vote'),

    # Settling on using hyphens in the named urls!

    url(r'^procurement/$', views.procurement_form_blank, name='procurement-blank'),
    url(r'^procurement/(?P<pk>[0-9]+)/$', views.procurement_form, name='procurement-detail'),
    url(r'^$', views.dashboard_index, name='dashboard-index'),
]
