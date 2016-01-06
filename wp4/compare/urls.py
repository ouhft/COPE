from django.conf.urls import url

from . import views

urlpatterns = [
    url(
        regex=r'^procurement/$',
        view=views.procurement_list,
        name='procurement_list'
    ),
    url(
        regex=r'^procurement/(?P<pk>[0-9]+)/$',
        view=views.procurement_form,
        name='procurement_detail'
    ),

    url(
        regex=r'^transplantation/$',
        view=views.transplantation_list,
        name='transplantation_list'
    ),
    url(
        regex=r'^transplantation/(?P<pk>[0-9]+)/$',
        view=views.transplantation_form,
        name='transplantation_detail'
    ),
    url(
        regex=r'^$',
        view=views.index,
        name='index'
    )
]
