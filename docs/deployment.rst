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
