"""
WSGI config for wp4 project.

It exposes the WSGI callable as a module-level variable named ``application``.

For more information on this file, see
https://docs.djangoproject.com/en/1.8/howto/deployment/wsgi/
"""

import os

from django.core.wsgi import get_wsgi_application

# Determine location and therefore which settings to consult
with open('location.env', 'r') as location_file:
    environment_string = location_file.read().replace('\n', '')

os.environ.setdefault("DJANGO_SETTINGS_MODULE", "config.settings." + environment_string)

application = get_wsgi_application()
