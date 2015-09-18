================
Home development
================







====================
Staging (Webfaction)
====================

Used an existing site setup, which needs documenting at some stage, and cleared it out as much as possible of the
previous application install (lots of pip uninstall, along with deleting a few files and folders)

Commands from the initial setup that were used (though not necessarily the correct ones as this is taken from
bash history on the server - and excludes the steps taken on the webfaction control panel)::

    cd ~/webapps/wp4_django/
    mkvirtualenv wp4_20150514
    deactivate
    touch ~/.virtualenvs/wp4_20150514/lib/python2.7/sitecustomize.py
    workon wp4_20150514
    ln -s ../../.virtualenvs/wp4_20150514/lib/ ./lib

    git clone git@github.com:AllyBradley/COPE.git
    cd COPE/
    git checkout testing
    pip install -r requirements/webfaction.txt
    vi local.env
    pm migrate
    pm collectstatic

After the virtual environment is setup, the application code was put into place, and Apache was then configured::

    cd apache2/conf/
    cp httpd.conf httpd.conf.orig
    vi httpd.conf
    ../bin/restart

Which left us with a working instance, so the application setup was started (will list the setup for v0.2.0)::

    pm createsuperuser
    pm loaddata config/fixtures/01_hospitals.json
    pm loaddata config/fixtures/02_persons.json
    pm loaddata config/fixtures/03_gradings.json
    pm loaddata config/fixtures/09_testusers.json

Create a link to the auto-deploy script and add it to the crontab::

    ln -s COPE/deploy/deploy-cm13.sh ./update_by_cron.sh
    chmod 755 COPE/deploy/deploy-cm13.sh
    crontab -e    # To edit the cron record, and to insert...
    */2 * * * * $HOME/webapps/wp4_django/update_by_cron.sh >> $HOME/webapps/wp4_django/cron-wp4_django.log 2>&1

The apache2 httpd.conf looks like this presently::

    ServerRoot "/home/cm13/webapps/wp4_django/apache2"

    LoadModule authz_core_module modules/mod_authz_core.so
    LoadModule dir_module        modules/mod_dir.so
    LoadModule env_module        modules/mod_env.so
    LoadModule log_config_module modules/mod_log_config.so
    LoadModule mime_module       modules/mod_mime.so
    LoadModule rewrite_module    modules/mod_rewrite.so
    LoadModule setenvif_module   modules/mod_setenvif.so
    LoadModule wsgi_module       modules/mod_wsgi.so
    LoadModule unixd_module      modules/mod_unixd.so

    LogFormat "%{X-Forwarded-For}i %l %u %t \"%r\" %>s %b \"%{Referer}i\" \"%{User-Agent}i\"" combined
    CustomLog /home/cm13/logs/user/access_wp4_django.log combined
    ErrorLog /home/cm13/logs/user/error_wp4_django.log

    DirectoryIndex index.py
    # DocumentRoot /home/cm13/webapps/wp4_django/htdocs

    Listen 31283
    KeepAlive Off
    SetEnvIf X-Forwarded-SSL on HTTPS=1
    ServerLimit 1
    StartServers 1
    MaxRequestWorkers 5
    MinSpareThreads 1
    MaxSpareThreads 3
    ThreadsPerChild 5

    WSGIPythonPath /home/cm13/webapps/wp4_django/lib/python2.7
    WSGIDaemonProcess wp4_django processes=2 threads=12 python-path=/home/cm13/webapps/wp4_django/lib/python2.7:/home/cm13/webapps/wp4_django/lib/python2.7/site-packages:/home/cm13/webapps/wp4_django/COPE/
    WSGIProcessGroup wp4_django
    WSGIRestrictEmbedded On
    WSGILazyInitialization On

    WSGIScriptAlias / /home/cm13/webapps/wp4_django/COPE/config/wsgi.py


Unrelated, but likely useful to remember: Command to get pip to update all installed packages::

    pip list --outdated
    pip freeze --local | grep -v '^\-e' | cut -d = -f 1  | xargs -n1 pip install -U


=====================
Production (cope.nds)
=====================
