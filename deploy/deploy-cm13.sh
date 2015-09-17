#!/bin/bash
PATH=/home/cm13/bin:/usr/local/bin:/bin:/usr/bin:/usr/local/sbin:/usr/sbin:/sbin:/home/cm13/bin
WORKON_HOME=/home/cm13/.virtualenvs
VIRTUALENVWRAPPER_PYTHON=/usr/local/bin/python2.7
VIRTUALENVWRAPPER_SCRIPT=/home/cm13/bin/virtualenvwrapper.sh
VIRTUALENVWRAPPER_PROJECT_FILENAME=.project
VIRTUALENVWRAPPER_HOOK_DIR=/home/cm13/.virtualenvs
PIP_VIRTUALENV_BASE=/home/cm13/.virtualenvs
VIRTUALENVWRAPPER_TMPDIR=/home/cm13/.virtualenvs/tmp
PIP_RESPECT_VIRTUALENV=true
HOME=/home/cm13
PYTHON=python2.7

echo " "
echo "=================================================================================="
echo " "
date
cd $HOME/webapps/wp4_django/COPE
PULL=$(git pull)
echo "Repository Updated : $PULL"
if [ "$PULL" = "Already up-to-date." ]; then
    echo No further actions are required
else
    RETOUCH_CRON=$(chmod 755 COPE/deploy/deploy-cm13.sh)
    source $HOME/.virtualenvs/wp4_20150514/bin/activate
    PIP_UPDATE=$(pip install -r requirements/webfaction.txt)
    echo $PIP_UPDATE
    COLLECT_FILES=$(python2.7 manage.py collectstatic --noinput)
    echo $COLLECT_FILES
    CHECK=$(python2.7 manage.py check)
    if [ "$CHECK" = "System check identified no issues (0 silenced)." ]; then
        MIGRATE=$(python2.7 manage.py migrate)
        echo "Migration completed : $MIGRATE"
        APACHE=$(../apache2/bin/restart)
        echo $APACHE
    else
        echo "System check failed : $CHECK"
    fi
    deactivate
fi
