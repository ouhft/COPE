# Deployment

## Development
Local development deployment notes on this can be found in [Development](development.md)


## Test/Staging
Test server deployment is to the dev.nds server, accessible under [https://dev.nds.ox.ac.uk/cope/](). This is an Ubuntu 16.04 Server with similar setup to the Production environment. This location was moved here from external (personal) hosting in Nov 2017, having been online in its previous setup since Aug 2015.

Deployment Outline

* Create project `$ mkproject -p /usr/bin/python3 cope`
  * This will activate the new virtualenv, and drop you into `/sites/cope`
* Create project user `$ sudo useradd --system --gid worker --shell /bin/bash --home /sites/cope cope-app-user`
* Clone repository, select branch
  * `$ git clone git@github.com:ouh-churchill/cope.git cope_repo`
  * `$ cd cope_repo/`
  * `$ git checkout testing`
* Create subfolders, link virtualenv folders
  * `$ cd ..` <-- return to `/sites/cope`
  * `$ ln -s /sites/.virtualenvs/cope/lib ./lib`
  * `$ ln -s /sites/.virtualenvs/cope/bin ./bin` 
  * `$ mkdir -p var/log var/run etc/nginx/sites-available htdocs/media etc/gunicorn`
* Install requirements
  * `$ cd cope_repo/`
  * `$ pip install -r requirements/staging.txt`
* Modify local `.env` settings
  * `$ cp config/settings/.env.template config/settings/.env`
  * `$ vi config/settings/.env` -- Put in local setting values
  * `$ python manage.py check`
* Create database (port from old location)
* Copy deployment files into system folders (nginx, supervisor, etc)
* Link into the main Nginx config via sites-available



### Webfaction - OLD 

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

    ln -s cope_repo/deploy/webfaction/deploy-cm13.sh ./update_by_cron.sh
    chmod 755 COPE/deploy/webfaction/deploy-cm13.sh
    crontab -e    # To edit the cron record, and to insert...
    */2 * * * * $HOME/webapps/wp4_django/update_by_cron.sh >> $HOME/webapps/wp4_django/cron-wp4_django.log 2>&1


### Footnotes for Staging

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


Unrelated, but likely useful to remember: Command to get pip to update all installed packages:

    pip list --outdated
    pip freeze --local | grep -v '^\-e' | cut -d = -f 1  | xargs -n1 pip install -U


## Production (cope.nds)

An Ubuntu 14.04 LTS Server virtual machine has been created and hosted by MSD-IT Services. This is the unmanaged
service, which means that we are responsible for it's patching and upkeep. Initial steps so far have been to change
the user account password (u: copeuser - p: in carl's password safe). Installing ssh keys will happen soon. Access
to the server should be via ssh (only from Oxford network, including vpn) and via the VMWare web console (oxford network
only - [https://fibula.msd.ox.ac.uk/]())

Setup is going to be loosely based on the guides from [https://www.chicagodjango.com/blog/new-server-setup-part-1/]() and
[https://www.chicagodjango.com/blog/new-django-server-setup-part-2/]() as these are inline with the best practice information
in the Two Scoops of Django 1.8: Best Practices guide (see Chapter 31). More useful though is the guide from
[http://michal.karzynski.pl/blog/2013/06/09/django-nginx-gunicorn-virtualenv-supervisor/]() (which I've used in the past).

### Access

So, initially we have access via ``ssh copeuser@cope.nds.ox.ac.uk``, and using a password. Step two is to install our
SSH key (iMac@Home presently). Step one is we need to generate an ssh key for the server to register with github for
access to the repoistory. Help is at [https://help.ubuntu.com/community/SSH/OpenSSH/Keys]() . To whit:

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

### Software stack

For now I'm happy to go with Nginx (for static), Gunicorn (for application), and SQLite (for datastore). System's
version of python is 2.7.6, which is currently fine for use, so will skip installing a local instance of python. Not
requiring any particular caching installation at this time either, so in the interests of KISS, we will install as
little as possible.

#### Maintainence

Don't forget to keep things up to date with ([https://help.ubuntu.com/community/AptGet/Howto]()):

    sudo apt autoremove
    sudo apt update
    sudo apt upgrade
    sudo apt-get check
    sudo apt autoclean

    sudo shutdown -r now

#### Installation

Follow guide plus above we have:

    # Remove some default processes and packages that we don't need to reduce server exposure
    sudo apt-get remove tomcat7 tomcat7-docs tomcat7-admin tomcat7-examples default-jdk
    sudo apt-get remove postgresql postgresql-9.3 postgresql-client postgresql-client-9.3 postgresql-client-common postgresql-common postgresql-contrib postgresql-contrib-9.3 postgresql-doc postgresql-doc-9.3

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
    setting relayhost: smtp.ox.ac.uk

Current info on the Oxford smarthost is at [http://help.it.ox.ac.uk/network/smtp/relay/index]()

Grab the file server and some other useful apps::

    sudo apt-get install nginx sqlite3
    sudo apt-get install pwgen htop curl rsync nmon dnsutils \
          screen tmux ack-grep whois sshfs openssh-client \
          openssh-server libssl-dev libreadline-dev aptitude fail2ban \
          libxml2-dev libxslt-dev graphviz graphviz-dev csstidy \
          emacs23-nox vim ncurses-dev iotop nload iptraf-ng nethogs tree

Note that we got sqlite3 in place of the postgres option from the guide.

We want to stop nginx from being run at server startup though, and let Supervisor handle that later on, so we need to
disable the link for init.d ::

    sudo rm /etc/rc2.d/S19postgresql /etc/rc2.d/S92tomcat7     # CLEANUP FROM EARLIER
    sudo mv /etc/rc2.d/S20nginx /etc/rc2.d/K20nginx
    update-rc.d script defaults


### User setup

We're not using the application user from the guide here, but using the nginx defined www-data user as the application
user::

    sudo mkdir -p /sites/cope
    sudo groupadd --system worker
    sudo chown root:worker /sites/
    sudo chmod 775 /sites/
    sudo useradd --system --gid worker --shell /bin/bash --home /sites/cope cope-app-user
    sudo usermod -aG worker www-data        # This is the application_user from the nginx config
    sudo usermod -aG worker copeuser        # This is the MSD-IT created user account
    sudo sh -c 'echo "umask 002" >> /etc/profile'

Remember to logout and back in again here so that any further work done as ``copeuser`` will have the ``worker``
group rights and therefore won't need sudo all the time.


### VirtualEnvWrapper Installation

The wrapper isn't installed as part of the apt-get process, so we do this following the instructions from
[http://virtualenvwrapper.readthedocs.org/en/latest/install.html#basic-installation]():

    sudo pip install virtualenvwrapper
    vi ~/.bashrc

Appending to .bashrc ::

    # Setup VirtualEnvWrapper for this user
    export WORKON_HOME=/sites/.virtualenvs
    export PROJECT_HOME=/sites
    source /usr/local/bin/virtualenvwrapper.sh

Then returning and::
    source ~/.bashrc


### Site setup

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
    mkdir -p var/log var/run etc/nginx/sites-available htdocs/media etc/gunicorn
    ln -s /sites/.virtualenvs/cope/lib ./lib
    ln -s /sites/.virtualenvs/cope/bin ./bin

Now we have: ``/sites/{{ENVIRONMENT_ROOT}}/{{PROJECT_ROOT}}/`` as ``/sites/cope/cope_repo`` (or in terms of the online
guide we have: ``/sites/{{site_name}}/{{proj_name}}/``)

Tweak nginx core config with ``sudo vi /etc/nginx/nginx.conf`` and add ``daemon off;`` near the top few lines, then we
can link to the conf files from the repository. First to the local folder, then to the system folder. ::

    # NB: cwd is /sites/cope
    ln -s /sites/cope/cope_repo/deploy/production/etc/nginx/sites-available/cope.conf /sites/cope/etc/nginx/sites-available/cope.conf
    sudo ln -s /sites/cope/etc/nginx/sites-available/cope.conf /etc/nginx/sites-available/cope.conf
    sudo ln -s /etc/nginx/sites-available/cope.conf /etc/nginx/sites-enabled/cope.conf
    sudo rm /etc/nginx/sites-enabled/default

    sudo ln -s /sites/cope/cope_repo/deploy/production/etc/supervisor/conf.d/nginx.conf /etc/supervisor/conf.d/nginx.conf
    sudo ln -s /sites/cope/cope_repo/deploy/production/etc/supervisor/conf.d/django.conf /etc/supervisor/conf.d/django.conf

    ln -s /sites/cope/cope_repo/deploy/production/bin/gunicorn_start.sh /sites/cope/bin/gunicorn_start.sh
    chmod 775 /sites/cope/cope_repo/deploy/production/bin/gunicorn_start.sh

Now we need to get some application code install done so that things like gunicorn are actually installed::

    # NB: cwd is /sites/cope
    pip install -r cope_repo/requirements/production.txt
    cd cope-repo/
    chmod 775 manage.py
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

### SSL Certificate

Help via:

* [https://help.ubuntu.com/lts/serverguide/certificates-and-security.html]()
* [https://wiki.it.ox.ac.uk/itss/CertificateService]()

Steps for request::

    cd .ssh/
    mkdir ssl-cert
    cd ssl-cert/
    vi website.ssl.cfg

Into which we used the following::

    [ req ]
        prompt                 = no
        default_bits           = 2048
        default_md             = sha256
        distinguished_name     = dn
    [ dn ]
        C                      = GB
        ST                     = Oxfordshire
        L                      = Oxford
        O                      = University of Oxford
        OU                     = Nuffield Department of Surgical Sciences
        CN                     = cope.nds.ox.ac.uk

And then::

    openssl req -new -keyout private.pem -out cope.nds.csr -config ./website.ssl.cfg -batch -verbose -nodes

    Using configuration from ./website.ssl.cfg
    Generating a 2048 bit RSA private key
    .............................................+++
    ..............................................+++
    writing new private key to 'private.pem'
    -----

Which resulted in a private.pem and cope.nds.csr files being created. The contents of the cope.nds.csr was then submitted
to Trend via their port (see ITSS wiki link) and emailed to the ITS3 team for confirmation. One phone confirmation
from ITS3 later, and an email arrives with the certificates zipped up, and a (supposedly the equivalent) .pem file of the
bundle. However, the .pem does not validate against the csr and private.pem (see [https://www.sslshopper.com/certificate-key-matcher.html]()
or run the openssl validation checks below).

Copy the files to the server (scp), and put them into ``~/.ssh/ssl-cert/``. Run unzip on the zip bundle, to get 3 .crt files returned. This now gives us:

    -rw-rw-r-- 1 copeuser copeuser 1218 Dec 14 18:20 AffirmTrust_Commercial.crt
    -rw-r--r-- 1 copeuser copeuser 3624 Dec 17 16:21 all-certificate.zip
    -rw-rw-r-- 1 copeuser copeuser 1086 Dec 14 17:48 cope.nds.csr
    -rw-rw-r-- 1 copeuser copeuser 1642 Dec 14 18:20 cope.nds.ox.ac.uk.crt
    -rw-rw-r-- 1 copeuser copeuser 1704 Dec 14 17:48 private.pem
    -rw-r--r-- 1 copeuser copeuser 2813 Dec 17 16:21 s2cabundle.pem
    -rw-rw-r-- 1 copeuser copeuser 1630 Dec 14 18:20 Trend_Micro_S2_CA.crt
    -rw-rw-r-- 1 copeuser copeuser  465 Dec 14 17:45 website.ssl.cfg

Long story short, because we can't validate the s2cabundle.pem file, we need to create our own .crt (or .pem, same thing) file by concatinating the three supplied certificates in the correct order, thus we do:

    cat cope.nds.ox.ac.uk.crt AffirmTrust_Commercial.crt Trend_Micro_S2_CA.crt > cope.nds.crt

    # Validation can now be done to confirm these all match!
    copeuser@cope:~/.ssh/ssl-cert$ openssl rsa -noout -modulus -in private.pem | openssl md5
    (stdin)= cf0888a35d5070123c032249b4010244
    copeuser@cope:~/.ssh/ssl-cert$ openssl req -noout -modulus -in cope.nds.csr | openssl md5
    (stdin)= cf0888a35d5070123c032249b4010244
    copeuser@cope:~/.ssh/ssl-cert$ openssl x509 -noout -modulus -in cope.nds.ox.ac.uk.crt  | openssl md5
    (stdin)= cf0888a35d5070123c032249b4010244

Now we'll put a copy of this combined certificate chain, and our private key, in a folder accessible by the NGINX
process, as that is acting as the SSL proxy server (see the configuration in ``cope-repo/
deploy/production/etc/nginx/sites-available/cope.conf`` for how Nginx will use these)::

    mkdir -p /sites/etc/ngix/ssl/    # Yes, there is a typo in nginx
    cp private.pem /sites/etc/ngix/ssl/
    cp cope.nds.crt /sites/etc/ngix/ssl/

With those in place, and using the config file above, you can got to ``sudo supervisorctl`` and then
``restart all``, and lo and behold, we have service on port 443.

It's worth noting that ``SECURE_PROXY_SSL_HEADER = ('HTTP_X_FORWARDED_PROTOCOL', 'https')`` needed to be added to the production.py settings file so that Django can detect if https is being used, otherwise when you set ``DJANGO_SECURE_SSL_REDIRECT=True`` in the ``local.env`` file, you will get a redirect loop in the browser.

The string "HTTP_X_FORWARDED_PROTOCOL" is derived from ``proxy_set_header X-Forwarded-Protocol $scheme;`` in the nginx conf combined with the note from [https://docs.djangoproject.com/en/dev/ref/settings/#secure-proxy-ssl-header]() saying:

    Note that the header needs to be in the format as used by request.META – all caps 
    and likely starting with HTTP_. (Remember, Django automatically adds 'HTTP_' to the 
    start of x-header names before making the header available in request.META.)

Port 443 has been confirmed as open on the MSD firewall for cope.nds.ox.ac.uk

### Maintenance and updates

Periodically there are maintenance tasks to do, such as:

* Update the OS libraries and packages - see Maintenance under System Setup above
* Backup the DB - ``cp db.sqlite3 ../db-backup/yyyymmdd.sqlite3``

#### Update the application release
 * Activate the virtualenv ``workon cope``
 * Move into the repository directory for most tasks ``cd cope-repo``
 * Get the latest updates from the central repository ``git pull --all``
 * Checkout the relevant tagged release (i.e. not Head of Master branch) ``git checkout 0.4.6``
 * Update the application libraries ``pip install -r requirements/production.txt``
 * Check things are working so far ``python manage.py check``
 * Apply any necessary migrations ``python manage.py migrate``
 * Gather the staticfiles up ``python manage.py collectstatic``
 * Apply the locale updates `python manage.py compilemessages`
 * Check things are working so far II ``python manage.py check``
 * Start the supervisor console to reload the changes ``sudo supervisorctl``...
 * and then ``restart cope-django``
 * ...and all should be fine.

#### Troubleshooting
When things do not go to plan, we want to find out what we can. There's no debug mode on the production server, so it's into the error logs for information. Logs from supervisord are found via: ``cd /var/log/supervisor/``. Unfortunately stack traces aren't captured here (for example on a Server Error), so more work needs to be done to help the monitoring of this server.


### Migrating to Python 3 and Postgres

* Perform the usual system maintainence (apt-get update, upgrade, autoclean, etc)
* Using installed python 3.4.3
 * Needs extra: `sudo apt-get install python3.4-dev libpq-dev` for psycopg2 to install
* Installed postgres following instructions at [https://www.digitalocean.com/community/tutorials/how-to-install-and-use-postgresql-on-ubuntu-14-04](). 
 * `sudo apt-get install postgresql postgresql-contrib`
 * Results in Postgres 9.3 being installed.
 * Created user `copedb` and database `copedb`

  ```
  postgres@cope:~$ createuser --interactive
  Enter name of role to add: copedb
  Shall the new role be a superuser? (y/n) n
  Shall the new role be allowed to create databases? (y/n) n
  Shall the new role be allowed to create more new roles? (y/n) n
  postgres@cope:~$ createdb copedb
  ```

 * Set password on user. `postgres=# \password copedb`

Follow site setup again, but pointing at `/usr/bin/python3` for virtualenv, and changing the files linked below to use the new `/sites/py3_cope path`::

    mkproject -p /usr/bin/python3 py3_cope
    # NB: cwd is now /sites/py3_cope
    git clone git@github.com:AllyBradley/COPE.git cope_repo
    mkdir -p var/log var/run etc/nginx/sites-available htdocs/media etc/gunicorn
    ln -s /sites/.virtualenvs/py3_cope/lib ./lib
    ln -s /sites/.virtualenvs/py3_cope/bin ./bin
   
    ln -s /sites/py3_cope/cope_repo/deploy/production/etc/nginx/sites-available/cope.conf /sites/py3_cope/etc/nginx/sites-available/py3_cope.conf
    sudo ln -s /sites/py3_cope/etc/nginx/sites-available/py3_cope.conf /etc/nginx/sites-available/py3_cope.conf
    sudo ln -s /etc/nginx/sites-available/py3_cope.conf /etc/nginx/sites-enabled/py3_cope.conf
    sudo rm /etc/nginx/sites-enabled/cope.conf 

    sudo rm /etc/supervisor/conf.d/nginx.conf 
    sudo ln -s /sites/py3_cope/cope_repo/deploy/production/etc/supervisor/conf.d/nginx.conf /etc/supervisor/conf.d/nginx.conf
    
    sudo ln -s /sites/py3_cope/cope_repo/deploy/production/etc/supervisor/conf.d/django.conf /etc/supervisor/conf.d/py3_django.conf
    # sudo rm /etc/supervisor/conf.d/django.conf  <-- See Supervisor config steps below

    ln -s /sites/py3_cope/cope_repo/deploy/production/bin/gunicorn_start.sh /sites/py3_cope/bin/gunicorn_start.sh
    chmod 775 /sites/py3_cope/cope_repo/deploy/production/bin/gunicorn_start.sh
    
    sudo chown -R www-data:worker *
    
    cd cope_repo
    chmod 775 manage.py
    pip install -U -r requirements/production.txt

Continue the site setup (`python` will default to the environment version of 3.4.3). 

* Use the previous local.env file (`cp ../../cope/cope_repo/local.env local.env` - NB: edit the static root path!), as we need to setup first to use the existing sqlite db file, and then port it to the new postgres engine. 
* Don't forget a location.env file too (`cp ../../cope/cope_repo/location.env location.env`)
* And we need the existing data: `cp ../../cope/cope_repo/db.sqlite3 ./db.sqlite3`
* `python manage.py check`
 * Because of breaking DB changes in 0.6.0, the previous 'followups' migrations need to be cleared from the database before we migrate, otherwise we won't have the tables in.
 * `python manage.py dbshell` and then `DELETE FROM django_migrations WHERE app="followups";`
* `python manage.py migrate`
* `python manage.py collectstatic`
* `python manage.py compilemessages`

Now is a good time to reboot the server, then we can go and see about gunicorn config/starting.

#### Supervisor Config
`sudo supervisorctl` to get into the interactive shell. Then:

* `reread` should show a new app available (py3-cope-django)
* `stop cope-django` to turn off the old app
* `remove cope-django` to stop it coming back on reboot
* `add py3-cope-django` to make the new config active for use
* `start py3-cope-django`... and we should be good to go
* When we're happy everything is running we can do some cleanup:
 * `sudo rm /etc/supervisor/conf.d/django.conf` 

#### Debugging Supervisor
On the likely outcome that something doesn't work, remember the following:

* Supervisor log output goes to `cd /var/log/supervisor/`, and this require root priviledges
* Nginx default log output goes to `cd /var/log/nginx/`, and is also root only access
* Gunicorn root log folder should be empty, but is at `cd /var/log/gunicorn/`

#### Reset backup:

* Make backup dir
* Link the dbbackup.sh script to the local bin folder
 * `ln -s /sites/py3_cope/cope_repo/deploy/production/bin/dbbackup.sh /sites/py3_cope/bin/dbbackup.sh`
* Update Cron with the new path

### Migrating from LTS 14.04 to LTS 16.04.1
...did not go smoothly :-/

* ``sudo apt-get autoremove`` to clear enough space on ``/boot``
* ``sudo do-release-upgrade``
 * Will need to update ``\etc\sudoers`` via the command ``visudo`` which has to be executed as root
 * Update ``/etc/nginx/nginx.conf`` 
 * Investigate the Postgres updates
* ``sudo systemctl enable supervisor.service`` to re-enable supervisor to start on bootup
* Reboot the server manually
* ... now we diagnose why the supervisor cope script won't start up; outwardly because the virtualenv is not working.
 * Recreate the virtualenv, because the python links are broken since there has been a change from python3.4 to python3.5 



```
Configuring postgresql-common ├─────────────────────────────────────────┐  
 │                                                                                                                   │  
 │ Obsolete major version 9.3                                                                                        │  
 │                                                                                                                   │  
 │ The PostgreSQL version 9.3 is obsolete, but the server or client packages are still installed. Please install     │  
 │ the latest packages (postgresql-9.5 and postgresql-client-9.5) and upgrade the existing  clusters with            │  
 │ pg_upgradecluster (see manpage).                                                                                  │  
 │                                                                                                                   │  
 │ Please be aware that the installation of postgresql-9.5 will automatically create a default cluster 9.5/main. If  │  
 │ you want to upgrade the 9.3/main cluster, you need to remove the already existing 9.5 cluster (pg_dropcluster     │  
 │ --stop 9.5 main, see manpage for details).                                                                        │  
 │                                                                                                                   │  
 │ The old server and client packages are no longer supported. After the existing clusters are upgraded, the         │  
 │ postgresql-9.3 and postgresql-client-9.3 packages should be removed.                                              │  
 │                                                                                                                   │  
 │ Please see /usr/share/doc/postgresql-common/README.Debian.gz for details.         
 ```


### TODO

* Run the django-secure scan


### Footnotes for Production

Useful commands:

* ``sudo cut -d: -f1 /etc/passwd`` -- lists all users
* ``sudo apt-get --purge remove {{package-name}}`` -- remove an installed package and config files
* ``apt --installed list`` -- list all installed packages
* ``sudo netstat -peanut``  -- list all ports in use on system
* ``ps -auxf`` -- list all processes in a tree showing originating process
* ``git checkout -- path/file.py`` -- reset a file back to the last git version

Reference urls:

* Gunicorn - [http://docs.gunicorn.org/en/latest/index.html]()
* Supervisor - [http://supervisord.org/index.html]()
* Django Security - [https://docs.djangoproject.com/en/1.8/topics/security/]()
* NGinx Admin Guide - [https://www.nginx.com/resources/admin-guide/]()
* NGinx Getting Started Wiki - [https://www.nginx.com/resources/wiki/start/]()
* NGinx Beginners Guide - [http://nginx.org/en/docs/beginners_guide.html]()
* Ubuntu Apt-Get Guide - [https://help.ubuntu.com/community/AptGet/Howto#Search_commands]()
* Django Secure - [http://django-secure.readthedocs.org/en/latest/]()


