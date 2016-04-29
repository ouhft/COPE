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
echo " "
if [ "$PULL" = "Already up-to-date." ]; then
    echo "No further actions are required"
else
    RETOUCH_CRON=$(chmod 755 deploy/webfaction/deploy-cm13.sh)
    find . -name '*.pyc' -delete
    source $HOME/.virtualenvs/wp4_20150514/bin/activate
    pip install -U -r requirements/webfaction.txt
    echo " "
    CHECK=$(python2.7 manage.py check)
    if [[ $CHECK == *"System check identified no issues (0 silenced)."* ]]; then
        COLLECT_FILES=$(python2.7 manage.py collectstatic --noinput)
        echo $COLLECT_FILES
        echo " "
        COMPILE_MESSAGES=$(python2.7 manage.py compilemessages)
        echo $COMPILE_MESSAGES
        echo " "
        MIGRATE=$(python2.7 manage.py migrate)
        echo "Migration completed : $MIGRATE"
        APACHE=$(../apache2/bin/restart)
        echo "Server restart called : $APACHE"
    else
        echo "System check FAILED : $CHECK"
    fi
    deactivate
fi
