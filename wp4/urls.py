#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.conf.urls import include, url

# from .perfusion_machine import urls as perfusionmachine_urls
from .health_economics import urls as health_economics_urls
from .followups import urls as followup_urls
from .adverse_event import urls as adverseevent_urls
from .staff import urls as person_urls
# from .staff_person import urls as staffperson_urls
from .locations import urls as locations_urls
from .samples import urls as samples_urls
from .compare import urls as compare_urls
from .administration import urls as administration_urls
from .views import wp4_index


app_name = "wp4"
urlpatterns = [
    url(r'^health-economics/', include(health_economics_urls)),
    url(r'^follow-up/', include(followup_urls)),
    url(r'^adverse-event/', include(adverseevent_urls)),
    url(r'^staff/', include(person_urls)),
    url(r'^location/', include(locations_urls)),
    url(r'^sample/', include(samples_urls)),
    url(r'^compare/', include(compare_urls)),
    url(r'^administration/', include(administration_urls)),

    url(
        regex=r'^$',
        view=wp4_index,
        name='index'
    )
]
