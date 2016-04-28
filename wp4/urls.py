#!/usr/bin/python
# coding: utf-8
from django.conf.urls import include, url

# from .perfusion_machine import urls as perfusionmachine_urls
from .followups import urls as followup_urls
from .adverse_event import urls as adverseevent_urls
from .staff_person import urls as staffperson_urls
from .locations import urls as locations_urls
from .samples import urls as samples_urls
from .compare import urls as compare_urls
from .views import wp4_index
from .views import administrator_index, administrator_uk_list, administrator_europe_list, administrator_datalist

urlpatterns = [
    # url(r'^followup/', include(followup_urls, namespace="followup")),
    url(r'^adverse-event/', include(adverseevent_urls, namespace="adverse_event")),
    url(r'^person/', include(staffperson_urls, namespace="staff_person")),
    url(r'^location/', include(locations_urls, namespace="locations")),
    url(r'^sample/', include(samples_urls, namespace="samples")),
    url(r'^compare/', include(compare_urls, namespace="compare")),
    url(
        regex=r'^administrator/data-list$',
        view=administrator_datalist,
        name='admin_data_list'
    ),
    url(
        regex=r'^administrator/europe-list$',
        view=administrator_europe_list,
        name='admin_europe_list'
    ),
    url(
        regex=r'^administrator/uk-list$',
        view=administrator_uk_list,
        name='admin_uk_list'
    ),
    url(
        regex=r'^administrator/$',
        view=administrator_index,
        name='admin_index'
    ),
    url(
        regex=r'^$',
        view=wp4_index,
        name='index'
    )
]
