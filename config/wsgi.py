"""
WSGI config for wp4 project.

It exposes the WSGI callable as a module-level variable named ``application``.

For more information on this file, see
https://docs.djangoproject.com/en/1.8/howto/deployment/wsgi/
"""

import os
import sys

from django.core.wsgi import get_wsgi_application

LOCATION_FILE = 'location.env'

# Determine location and therefore which settings to consult
if os.getcwd() == '/home/cm13':
    LOCATION_FILE = '/home/cm13/webapps/wp4_django/COPE/' + LOCATION_FILE

with open(LOCATION_FILE, 'r') as location_file:
    environment_string = location_file.read().replace('\n', '')

os.environ.setdefault("DJANGO_SETTINGS_MODULE", "config.settings." + environment_string)

application = get_wsgi_application()
