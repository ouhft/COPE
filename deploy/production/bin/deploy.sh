#!/bin/bash
PATH=/usr/local/bin:/bin:/usr/bin:/usr/local/sbin:/usr/sbin:/sbin
WORKON_HOME=/sites/.virtualenvs
VIRTUALENVWRAPPER_PYTHON=/usr/bin/python2.7
VIRTUALENVWRAPPER_SCRIPT=/usr/local/bin/virtualenvwrapper.sh
VIRTUALENVWRAPPER_PROJECT_FILENAME=.project
VIRTUALENVWRAPPER_HOOK_DIR=/sites/.virtualenvs
PIP_VIRTUALENV_BASE=/sites/.virtualenvs
VIRTUALENVWRAPPER_TMPDIR=/sites/.virtualenvs/tmp
PIP_RESPECT_VIRTUALENV=true
HOME=/sites
PYTHON=python2.7

echo " "
echo "=================================================================================="
echo " "
date
cd $HOME/cope/cope-repo
PULL=$(git pull --all)
echo "Repository Updated : $PULL"
echo " "
if [ "$PULL" = "Already up-to-date." ]; then
    echo "No further actions are required"
else
    git checkout 0.5.0
    RETOUCH_CRON=$(chmod 755 deploy/production/bin/deploy.sh)
    find . -name '*.pyc' -delete
    source $HOME/.virtualenvs/cope/bin/activate
    pip install -U -r requirements/production.txt
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
