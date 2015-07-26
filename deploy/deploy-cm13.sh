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


cd $HOME/webapps/wp4_django
date
cd COPE
PULL=$(git pull)
echo "Repository Updated : $PULL"
if [ "$PULL" = "Already up-to-date." ]; then
    echo No further actions are required
else
    cd src/wp4/
    # TODO: Pip update after activate if requirements.txt found in git pull output
    source $HOME/.virtualenvs/wp4_20150514/bin/activate
    CHECK=$(python2.7 manage.py check)
    if [ "$CHECK" = "System check identified no issues (0 silenced)." ]; then
        MIGRATE=$(python2.7 manage.py migrate)
        echo "Migration completed : $MIGRATE"
        APACHE=$(../../../apache2/bin/restart)
        echo $APACHE
    else
        echo "System check failed : $CHECK"
    fi
    deactivate
fi
