#!/bin/bash

NAME="cope_app"                                   # Name of the application
APP_ROOT=/sites/cope                              # Application root path
DJANGODIR=$APP_ROOT/cope_repo                     # Django project directory
SOCKFILE=$APP_ROOT/var/run/wsgi.socket            # we will communicte using this unix socket
USER=cope-app-user                                # the user to run as
GROUP=worker                                      # the group to run as
NUM_WORKERS=3                                     # how many worker processes should Gunicorn spawn
DJANGO_SETTINGS_MODULE=config.settings.production # which settings file should Django use
DJANGO_WSGI_MODULE=config.wsgi                    # WSGI module name
PID_FILE=$APP_ROOT/var/run/gunicorn.pid

echo "Starting $NAME as `whoami`"

# Activate the virtual environment
cd $DJANGODIR
source ../bin/activate
# export DJANGO_SETTINGS_MODULE=$DJANGO_SETTINGS_MODULE
# export PYTHONPATH=$DJANGODIR:$PYTHONPATH

# Create the run directory if it doesn't exist
RUNDIR=$(dirname $SOCKFILE)
test -d $RUNDIR || mkdir -p $RUNDIR

# Start your Django Unicorn
# Programs meant to be run under supervisor should not daemonize themselves (do not use --daemon)
exec ../bin/gunicorn ${DJANGO_WSGI_MODULE}:application \
  --name $NAME \
  --workers $NUM_WORKERS \
  --bind=unix:$SOCKFILE \
  --log-level=debug \
  --log-file=- \
  --pid=$PID_FILE
#  --user=$USER --group=$GROUP \
