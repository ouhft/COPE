#!/usr/bin/env python
import os
import sys

if __name__ == "__main__":
    # Determine location and therefore which settings to consult
    with open('location.env', 'r') as location_file:
        environment_string = location_file.read().replace('\n', '')

    os.environ.setdefault("DJANGO_SETTINGS_MODULE", "config.settings." + environment_string)

    from django.core.management import execute_from_command_line

    execute_from_command_line(sys.argv)
