# -*- coding: utf-8 -*-
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
DEBUG = env.bool('DJANGO_DEBUG', default=True)
TEMPLATES[0]['OPTIONS']['debug'] = DEBUG
CRISPY_FAIL_SILENTLY = not DEBUG
# Debug language for seeing labels rather than translations
LANGUAGES = LANGUAGES + [
    ('en-db', _('SET04 Debug Language')),
]

# SECRET CONFIGURATION
# ------------------------------------------------------------------------------
# See: https://docs.djangoproject.com/en/dev/ref/settings/#secret-key
# Note: This key only used for development and testing.
SECRET_KEY = env("DJANGO_SECRET_KEY", default='poiuytrewqasdfghjkl09876543211234567890mnbvcxzzxcvbnm.lo9876cft5432')

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
MIDDLEWARE_CLASSES += ('debug_toolbar.middleware.DebugToolbarMiddleware',)
INSTALLED_APPS += ('debug_toolbar',)

INTERNAL_IPS = ('127.0.0.1',)

DEBUG_TOOLBAR_CONFIG = {
    'DISABLE_PANELS': [
        'debug_toolbar.panels.redirects.RedirectsPanel',
    ],
    'SHOW_TEMPLATE_CONTEXT': True,
}

# django-extensions
# ------------------------------------------------------------------------------
INSTALLED_APPS += ('django_extensions',)

# django-spaghetti-and-meatballs
# ------------------------------------------------------------------------------
INSTALLED_APPS += ('django_spaghetti',)

SPAGHETTI_SAUCE = {
    'apps': [
        'compare',
        'locations',
        'staff_person',
        'adverse_event',
        'samples',
        'followups',
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
