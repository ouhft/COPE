from django.conf.urls import include, url
from django.contrib import admin
from django.conf.urls.i18n import i18n_patterns
# from django.conf import settings


# if settings.DEBUG:
#     print("DEBUG: config.urls loading")

urlpatterns = [
    url(r'^i18n/', include('django.conf.urls.i18n')),
]

urlpatterns += i18n_patterns(
    url(r'^', include('wp4.compare.urls', namespace="compare")),
    url(r'^autocomplete/', include('autocomplete_light.urls')),
    url(r'^admin/', include(admin.site.urls)),
    url(r'^accounts/', include('django.contrib.auth.urls')),
)

# if settings.DEBUG:
#     print("DEBUG: config.urls loaded")
