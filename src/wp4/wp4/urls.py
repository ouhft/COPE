from django.conf.urls import include, url
from django.contrib import admin

# From auth/urls.py
# from django.contrib.auth import views
# auth_urlpatterns = [
#     url(r'^login/$', views.login, name='login'),
#     url(r'^logout/$', views.logout, name='logout'),
#     url(r'^password_change/$', views.password_change, name='password_change'),
#     url(r'^password_change/done/$', views.password_change_done, name='password_change_done'),
#     url(r'^password_reset/$', views.password_reset, name='password_reset'),
#     url(r'^password_reset/done/$', views.password_reset_done, name='password_reset_done'),
#     url(r'^reset/(?P<uidb64>[0-9A-Za-z_\-]+)/(?P<token>[0-9A-Za-z]{1,13}-[0-9A-Za-z]{1,20})/$',
#         views.password_reset_confirm, name='password_reset_confirm'),
#     url(r'^reset/done/$', views.password_reset_complete, name='password_reset_complete'),
# ]
#

urlpatterns = [
    url(r'^admin/', include(admin.site.urls)),
    url(r'^accounts/', include('django.contrib.auth.urls', namespace="auth")),
    url(r'^', include('compare.urls', namespace="compare")),
]



