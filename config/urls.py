from django.conf.urls import include, url
from django.contrib import admin
from django.conf.urls.i18n import i18n_patterns
from django.conf import settings


# if settings.DEBUG:
#     print("DEBUG: config.urls loading")

urlpatterns = [
    url(r'^i18n/', include('django.conf.urls.i18n')),
]

urlpatterns += i18n_patterns(
    url(r'^autocomplete/', include('autocomplete_light.urls')),
    url(r'^admin/', include(admin.site.urls)),
    url(r'^accounts/', include('django.contrib.auth.urls')),
    url(r'^followup/', include('wp4.followups.urls', namespace="followup")),
    url(r'^adverse-event/', include('wp4.adverse_event.urls', namespace="adverse_event")),
    url(r'^person/', include('wp4.staff_person.urls', namespace="staff_person")),
    url(r'^location/', include('wp4.locations.urls', namespace="locations")),
    url(r'^sample/', include('wp4.samples.urls', namespace="samples")),
    url(r'^', include('wp4.compare.urls', namespace="compare")),
)

if settings.DEBUG:
    try:
        urlpatterns += (
          url(r'^plate/', include('django_spaghetti.urls', namespace="spaghetti")),
        )
    except ImportError:
        pass
#     print("DEBUG: config.urls loaded")
