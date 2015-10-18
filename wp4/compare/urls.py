from django.conf.urls import url

from . import views

urlpatterns = [
    url(r'^procurement/$', views.procurement_list, name='procurement_list'),
    url(r'^procurement/(?P<pk>[0-9]+)/$', views.procurement_form, name='procurement_detail'),

    url(r'^transplantation/$', views.transplantation_list, name='transplantation_list'),
    url(r'^transplantation/(?P<pk>[0-9]+)/$', views.transplantation_form, name='transplantation_detail'),

    url(r'^errors/403$', views.error403, name='error_403'),
    url(r'^errors/404$', views.error404, name='error_404'),
    url(r'^errors/500$', views.error500, name='error_500'),

    url(r'^$', views.dashboard_index, name='home'),
]
