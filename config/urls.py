#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.conf.urls import include, url
from django.contrib import admin
from django.conf.urls.i18n import i18n_patterns
from django.conf import settings

from wp4.views import error403, error404, error500
from wp4.views import dashboard_index, dashboard_changelog, dashboard_usermanual
from wp4 import urls as wp4_urls

# if settings.DEBUG:
#     print("DEBUG: config.urls loading")

urlpatterns = [
    url(r'^i18n/', include('django.conf.urls.i18n')),
]

urlpatterns += i18n_patterns(
    # url(r'^autocomplete/', include('autocomplete_light.urls')),
    url(r'^admin/doc/', include('django.contrib.admindocs.urls')),
    url(r'^admin/', include(admin.site.urls)),
    url(r'^accounts/', include('django.contrib.auth.urls')),
    url(r'^wp4/', include(wp4_urls, namespace='wp4')),

    url(r'^errors/403/$', error403, name='error_403'),
    url(r'^errors/404/$', error404, name='error_404'),
    url(r'^errors/500/$', error500, name='error_500'),

    url(r'^changelog$', dashboard_changelog, name='changelog'),
    url(r'^user-manual$', dashboard_usermanual, name='user_manual'),
    url(r'^$', dashboard_index, name='home'),
)

if settings.DEBUG:
    try:
        urlpatterns += (
          url(r'^plate/', include('django_spaghetti.urls', namespace="spaghetti")),
        )
    except ImportError:
        pass

    try:
        import debug_toolbar
        urlpatterns += (
            url(r'^__debug__/', include(debug_toolbar.urls)),
        )
    except ImportError:
        pass
