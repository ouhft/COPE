from django.conf.urls import include, url
from django.contrib.auth.views import login as auth_login

from . import views
from .forms import LoginForm

urlpatterns = [
    # url(r'teams/$', views.RetrievalTeamIndexView.as_view(), name='teams_index'),
    # url(r'teams/(?P<pk>[0-9]+)/$', views.RetrievalTeamDetailView.as_view(), name='teams_detail'),
    # url(r'teams/(?P<pk>[0-9]+)/results/$', views.RetrievalTeamResultsView.as_view(), name='teams_results'),
    # url(r'teams/(?P<question_id>[0-9]+)/vote/$', views.vote, name='vote'),

    # TODO: Rename all names to use underscores as in two-scoops


    # Override the login view with our own Crispy enabled login form
    # url(r'^accounts/login/$', auth_login, kwargs={'authentication_form': LoginForm}, name='login'),

    url(r'^procurement/$', views.procurement_form_blank, name='procurement-blank'),
    url(r'^procurement/(?P<pk>[0-9]+)/$', views.procurement_form, name='procurement-detail'),

    url(r'^transplantation/$', views.transplantation_form_list, name='transplantation'),
    url(r'^transplantation/new/(?P<pk>[0-9]+)/$', views.transplantation_form_new, name='transplantation-new'),
    url(r'^transplantation/(?P<pk>[0-9]+)/$', views.transplantation_form, name='transplantation-detail'),


    url(r'^errors/403$', views.error403, name='error-403'),
    url(r'^errors/404$', views.error404, name='error-404'),
    url(r'^errors/500$', views.error500, name='error-500'),

    url(r'^$', views.dashboard_index, name='home'),
]
