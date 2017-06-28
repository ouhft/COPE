#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

"""
Local settings

- Run in Debug mode
- Use console backend for emails
- Add Django Debug Toolbar
- Add django-extensions as app
- Add django-spaghetti-and-meatballs as app
"""

from django.utils.translation import ugettext_lazy as _
from .common import *  # noqa

print("DEBUG: Loading settings from development")

# DEBUG
# ------------------------------------------------------------------------------
CRISPY_FAIL_SILENTLY = not DEBUG
# Debug language for seeing labels rather than translations
LANGUAGES = LANGUAGES + [
    ('en-db', _('SET04 Debug Language')),
]

# Mail settings
# ------------------------------------------------------------------------------
EMAIL_HOST = 'localhost'
EMAIL_PORT = 25
EMAIL_BACKEND = env('DJANGO_EMAIL_BACKEND',
                    default='django.core.mail.backends.console.EmailBackend')

# CACHING
# ------------------------------------------------------------------------------
CACHES = {
    'default': {
        'BACKEND': 'django.core.cache.backends.locmem.LocMemCache',
        'LOCATION': ''
    }
}

# django-debug-toolbar
# ------------------------------------------------------------------------------
MIDDLEWARE_CLASSES = [
    'debug_toolbar.middleware.DebugToolbarMiddleware',
] + MIDDLEWARE_CLASSES
INSTALLED_APPS += [
    'debug_toolbar',
    'template_profiler_panel'
    # Disabled due to django 1.11 creating template timings that caused this profiler to explode with super
    # huge page sizes and load times - https://github.com/jazzband/django-debug-toolbar/issues/910
]

INTERNAL_IPS = ('127.0.0.1',)

DEBUG_TOOLBAR_CONFIG = {
    'DISABLE_PANELS': [
        'debug_toolbar.panels.redirects.RedirectsPanel',
        'debug_toolbar.panels.templates.TemplatesPanel',
    ],
    'SHOW_TEMPLATE_CONTEXT': False,
}
DEBUG_TOOLBAR_PANELS = [
    'debug_toolbar.panels.versions.VersionsPanel',
    'debug_toolbar.panels.timer.TimerPanel',
    'debug_toolbar.panels.settings.SettingsPanel',
    'debug_toolbar.panels.headers.HeadersPanel',
    'debug_toolbar.panels.request.RequestPanel',
    'debug_toolbar.panels.sql.SQLPanel',
    'debug_toolbar.panels.staticfiles.StaticFilesPanel',
    'debug_toolbar.panels.templates.TemplatesPanel',
    'debug_toolbar.panels.cache.CachePanel',
    'debug_toolbar.panels.signals.SignalsPanel',
    'debug_toolbar.panels.logging.LoggingPanel',
    'debug_toolbar.panels.redirects.RedirectsPanel',
    # 'debug_toolbar.panels.profiling.ProfilingPanel',
    'template_profiler_panel.panels.template.TemplateProfilerPanel'
]

# django-extensions
# ------------------------------------------------------------------------------
INSTALLED_APPS += ('django_extensions',)

# django-spaghetti-and-meatballs
# ------------------------------------------------------------------------------
INSTALLED_APPS += ('django_spaghetti',)

# https://django-spaghetti-and-meatballs.readthedocs.io/en/latest/customising.html
SPAGHETTI_SAUCE = {
    'apps': [
        'administration',
        'adverse_event',
        'compare',
        'followups',
        'health_economics',
        'locations',
        'samples',
        'staff',
        'theme'
    ],
    'show_fields': False,
    'exclude': {}
    # 'exclude': {'auth': ['user']}
}

# TESTING
# ------------------------------------------------------------------------------
TEST_RUNNER = 'django.test.runner.DiscoverRunner'

# Your local stuff: Below this line define 3rd party library settings
