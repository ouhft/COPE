================
Home development
================

Notes on this will come later...


====================
Staging (Webfaction)
====================

Used an existing site setup, which needs documenting at some stage, and cleared it out as much as possible of the
previous application install (lots of ``pip uninstall``, along with deleting a few files and folders)

Commands from the initial setup that were used (though not necessarily the correct ones as this is taken from
bash history on the server - and excludes the steps taken on the webfaction control panel)::

    cd ~/webapps/wp4_django/
    mkvirtualenv wp4_20150514
    deactivate
    touch ~/.virtualenvs/wp4_20150514/lib/python2.7/sitecustomize.py
    workon wp4_20150514
    ln -s ../../.virtualenvs/wp4_20150514/lib/ ./lib

After the virtual environment is setup, the application code was put into place (this is from v0.2.0)::

    git clone git@github.com:AllyBradley/COPE.git
    cd COPE/
    git checkout testing
    pip install -r requirements/webfaction.txt
    vi local.env
    pm migrate
    pm collectstatic

...and Apache was then configured::

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

---------------------
Footnotes for Staging
---------------------

The apache2 ``httpd.conf`` looks like this presently (copy in ``deploy/httpd.conf.webfaction``)::

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

An Ubuntu 14.04 LTS Server virtual machine has been created and hosted by MSD-IT Services. This is the unmanaged
service, which means that we are responsible for it's patching and upkeep. Initial steps so far have been to change
the user account password (u: copeuser - p: in carl's password safe). Installing ssh keys will happen soon. Access
to the server should be via ssh (only from Oxford network, including vpn) and via the VMWare web console (oxford network
only - https://fibula.msd.ox.ac.uk/ )

Setup is going to be loosely based on the guides from https://www.chicagodjango.com/blog/new-server-setup-part-1/ and
https://www.chicagodjango.com/blog/new-django-server-setup-part-2/ as these are inline with the best practice information
in the Two Scoops of Django 1.8: Best Practices guide (see Chapter 31).

Access
------

So, initially we have access via ``ssh copeuser@cope.nds.ox.ac.uk``, and using a password. Step two is to install our
SSH key (iMac@Home presently). Step one is we need to generate an ssh key for the server to register with github for
access to the repoistory. Help is at https://help.ubuntu.com/community/SSH/OpenSSH/Keys . To whit::

    copeuser@cope:~$ mkdir ~/.ssh
    copeuser@cope:~$ chmod 700 ~/.ssh
    copeuser@cope:~$ ssh-keygen -t rsa -b 4096
    Generating public/private rsa key pair.
    Enter file in which to save the key (/home/copeuser/.ssh/id_rsa):
    Enter passphrase (empty for no passphrase):
    Enter same passphrase again:
    Your identification has been saved in /home/copeuser/.ssh/id_rsa.
    Your public key has been saved in /home/copeuser/.ssh/id_rsa.pub.
    The key fingerprint is:
    05:98:8b:94:f7:cf:7a:2d:19:61:26:52:41:d5:96:10 copeuser@cope
    The key's randomart image is:
    +--[ RSA 4096]----+
    |     . ++oE+ .   |
    |    o + ..  +    |
    |   . o +  ..     |
    |    . o o.+      |
    |       .S* .     |
    |          +      |
    |         . +     |
    |        . + .    |
    |         . .     |
    +-----------------+

This set of keys is passphrase free because of the automated usage (i.e. scripts using it to access things).

Copy own .pub key to the server (SFTP), and then::

    mv id_rsa.pub .ssh/carl_imac.id_rsa.pub
    cat carl_imac.id_rsa.pub >> authorized_keys

Also, minor edit to ``sudo vi /etc/ssh/sshd_config`` so that ``AuthorizedKeysFile      %h/.ssh/authorized_keys`` is
uncommented. Sudo usage will continue to require password confirmation, however we can now login without needing the
password from SSH.

I have added the server's pub key to my list of Github keys, so that it can log in as marshalc.

Software stack
--------------

For now I'm happy to go with Nginx (for static), Gunicorn (for application), and SQLite (for datastore). System's
version of python is 2.7.6, which is currently fine for use, so will skip installing a local instance of python. Not
requiring any particular caching installation at this time either, so in the interests of KISS, we will install as
little as possible.

**Maintainence**

Don't forget to keep things up to date with (https://help.ubuntu.com/community/AptGet/Howto )::

    sudo apt-get update
    sudo apt-get upgrade
    sudo apt-get check
    sudo apt-get autoclean

    # Remove some default processes and packages that we don't need to reduce server exposure
    sudo apt-get remove tomcat7 tomcat7-docs tomcat7-admin tomcat7-examples default-jdk
    sudo apt-get remove postgresql postgresql-9.3 postgresql-client postgresql-client-9.3 postgresql-client-common postgresql-common postgresql-contrib postgresql-contrib-9.3 postgresql-doc postgresql-doc-9.3

    sudo shutdown -r now

**Installation**

Follow guide plus above we have::

    sudo apt-get install python-pip python-virtualenv
    sudo apt-get install python python-dev python-setuptools
    sudo apt-get install build-essential    # Was redundant because the above had already got this
    sudo apt-get install git
    sudo apt-get install supervisor postfix ntp

Postfix will ask for some config, so it is set to use a mail-relay/smarthost for sending messages, and then::

    setting myhostname: cope.nds
    setting alias maps
    setting alias database
    changing /etc/mailname to cope.nds.ox.ac.uk
    setting myorigin
    setting destinations: cope.nds.ox.ac.uk, cope.nds, localhost.nds, localhost
    setting relayhost: oxmail.ox.ac.uk

Current info on the Oxford smarthost is at http://help.it.ox.ac.uk/network/smtp/relay/index

Grab the file server and some other useful apps::

    sudo apt-get install nginx
    sudo apt-get install pwgen htop curl rsync nmon dnsutils \
          screen tmux ack-grep whois sqlite3 sshfs openssh-client \
          openssh-server libssl-dev libreadline-dev aptitude fail2ban \
          libxml2-dev libxslt-dev graphviz graphviz-dev csstidy \
          emacs23-nox vim ncurses-dev iotop nload iptraf-ng nethogs

Note that we got sqlite3 in that last batch of utils (in place of the postgres option from the guide)

We want to stop nginx from being run at server startup though, and let Supervisor handle that later on, so we need to
disable the link for init.d ::

    sudo rm /etc/rc2.d/S19postgresql /etc/rc2.d/S92tomcat7     # CLEANUP FROM EARLIER
    sudo mv /etc/rc2.d/S20nginx /etc/rc2.d/K20nginx
    update-rc.d script defaults


User setup
----------

We're not using the application user from the guide here, but using the nginx defined www-data user as the application
user::

    sudo addgroup worker
    sudo usermod -aG worker copeuser
    sudo sh -c 'echo "umask 002" >> /etc/profile'
    sudo mkdir /sites
    sudo chown root:worker /sites/
    sudo chmod 775 /sites/

Remember to logout and back in again here so that any further work done as ``copeuser`` will have the ``worker``
group rights and therefore won't need sudo all the time. ::

    cd /sites/
    mkdir cope

VirtualEnvWrapper Installation
------------------------------

The wrapper isn't installed as part of the apt-get process, so we do this following the instructions from
http://virtualenvwrapper.readthedocs.org/en/latest/install.html#basic-installation . ::

    sudo pip install virtualenvwrapper
    vi ~/.bashrc

Appending::

    # Setup VirtualEnvWrapper for this user
    export WORKON_HOME=/sites/.virtualenvs
    export PROJECT_HOME=/sites
    source /usr/local/bin/virtualenvwrapper.sh

Then returning and::
    source ~/.bashrc

Site setup
----------

Create a new virtualenv project with ``mkproject cope``. Note that the ``bin/``, ``lib/`` directories are in the
``/sites/.virtualenvs/cope/`` folder ::

    copeuser@cope:~$ mkproject cope
    New python executable in cope/bin/python
    Installing setuptools, pip...done.
    virtualenvwrapper.user_scripts creating /sites/.virtualenvs/cope/bin/predeactivate
    virtualenvwrapper.user_scripts creating /sites/.virtualenvs/cope/bin/postdeactivate
    virtualenvwrapper.user_scripts creating /sites/.virtualenvs/cope/bin/preactivate
    virtualenvwrapper.user_scripts creating /sites/.virtualenvs/cope/bin/postactivate
    virtualenvwrapper.user_scripts creating /sites/.virtualenvs/cope/bin/get_env_details

    # NB: cwd is /sites/cope
    git clone git@github.com:AllyBradley/COPE.git cope_repo
    git checkout production
    mkdir -p var/log var/run etc/nginx htdocs/media etc/gunicorn
    ln -s /sites/.virtualenvs/cope/lib ./lib
    ln -s /sites/.virtualenvs/cope/bin ./bin

Now we have: ``/sites/{{ENVIRONMENT_ROOT}}/{{PROJECT_ROOT}}/`` as ``/sites/cope/cope_repo`` (or in terms of the online
guide we have: ``/sites/{{site_name}}/{{proj_name}}/``)

Tweak nginx core config with ``sudo vi /etc/nginx/nginx.conf`` and add ``daemon off;`` near the top few lines, then we
can link to the conf files from the repository. First to the local folder, then to the system folder. ::

    ln -s /sites/cope/cope_repo/deploy/production/etc/nginx/locations.conf /sites/cope/etc/nginx/locations.conf
    ln -s /sites/cope/cope_repo/deploy/production/etc/nginx/server.conf /sites/cope/etc/nginx/server.conf
    ln -s /sites/cope/etc/nginx/server.conf /etc/nginx/conf.d/cope.conf

    sudo ln -s /sites/cope/cope_repo/deploy/production/etc/supervisor/conf.d/nginx.conf /etc/supervisor/conf.d/nginx.conf
    sudo ln -s /sites/cope/cope_repo/deploy/production/etc/supervisor/conf.d/django.conf /etc/supervisor/conf.d/django.conf

    ln -s /sites/cope/cope_repo/deploy/production/etc/gunicorn/gunicorn.py /sites/cope/etc/gunicorn/gunicorn.py

Now we need to get some application code install done so that things like gunicorn are actually installed::

    pip install -r cope_repo/requirements/production.txt
    cd cope-repo/
    sudo chmod 775 manage.py
    cp local.env.template local.env
    vi local.env                           # set debug to off, and then create a secret key, and set Static and Media root to the htdocs directory
    python manage.py check                 # Should return 0 errors
    python manage.py migrate
    python manage.py collectstatic         # NB: Should point to the htdocs folder and ask for confirmation
    python manage.py createsuperuser       # superuser is 'carl'
    python manage.py loaddata config/fixtures/01_hospitals.json
    python manage.py loaddata config/fixtures/02_persons.json
    python manage.py loaddata config/fixtures/03_gradings.json

Now do a quick sweep of the files to ensure permissions are suitably set so far...::

    sudo chown -R www-data:worker /sites/cope
    sudo chmod -R g+w /sites/cope
    sudo find /sites/cope -type d -exec chmod g+s {} \;

Generally speaking, we try to give each file and directory the minimum permissions necessary. We try to abide by the
following permission guidelines.

* Python (*.py) files should have 664 set on them UNLESS a user is to directly execute the Python file from the command
  line as a script (i.e. manage.py). Executable Python scripts should be set to 775.
* Script (*.sh) files should be set to 775.
* Static files (*.html, *.css, *.jpg, *.png, etc) should be set to 664.
* Directories should be set to 2775 (set GID set).
* All other files should be set to 664 unless there's a good reason not to do so.

Restarting server with ``sudo shutdown -r now`` to test the above configurations




WIP:
* Test application runs using runserver
 * Needs to have SSL redirect disabled so that it responds on http
* Test application runs using gunicorn
* Get the supervisor gunicorn command line to work
 * issue with not being in the correct path and unable to find config.wsgi




------------------------
Footnotes for Production
------------------------

Useful commands:
* ``sudo cut -d: -f1 /etc/passwd`` -- lists all users
* ``sudo apt-get --purge remove {{package-name}}`` -- remove an installed package and config files
* ``apt --installed list`` -- list all installed packages
* ``sudo netstat -peanut``  -- list all ports in use on system
* ``ps -auxf`` -- list all processes in a tree showing originating process
