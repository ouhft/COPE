Local Development Setup
=======================

Base system is OS X 10.10.5 (Yosemite), and I've got this environment setup in two locations so far. This document
mostly details how the second environment was built (to match the first - which has evolved from OS X 10.7 times)

Most of my approach is based on http://hackercodex.com/guide/python-development-environment-on-mac-osx/ (which is the
updated version of the guide I've used for the past 4 years)


User credentials
----------------

As a new user account (on a new machine, but that bit is irrelevant) we need to create an SSH key for access to things
like GitHub and cope.nds Server.::

    mkdir ~/.ssh
    chmod 700 ~/.ssh
    ssh-keygen -t rsa -b 4096
    # Follow onscreen prompts for name of key and empty passphrase

With a key generated, it can be added to:
* cope.nds authorised_keys
* webfaction authorised_keys
* GitHub marshalc keys

This needs to be done for GitHub at least, before you can so the project setup below


Third party apps in use
-----------------------

* IDE: PyCharm - from http://www.jetbrains.com. I'm using the Professional 4.5.4 version.
* Utility: CodeKit - from http://incident57.com/codekit/. Currently v2.4
* Utility: CyberDuck - from . Currently v4.7.2
* Base libraries: Homebrew - from http://brew.sh. More on its installation later.


Brew installation
.................

Easy enough, just follow the instructions on the Homebrew website. ::

    ruby -e "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install)"
    # Accept suggestions, and enter password for sudo
    brew doctor
    # Recommends running the xcode install to get the 10.10 SDK which isn't implicit in the XCode 7 install
    xcode-select --install
    # This prompts a gui installer to pop up, so click to accept.

Of the various base libraries that come up for use in various projects, one that seems to be key is `readline`, so we'll
install that now::

    brew install readline
    # Also helpful, but not essential
    brew install bash-completion ssh-copy-id wget

Python setup
............

Start by putting Homebrew's python installs into the system to avoid conflicts with the system python::

    brew install python python3

Have installed both python (2.7.10) and python3 (3.5) brew packages, which gives us setuptools, sqlite3 and pip.
Have then pip installed virtualenv, and virtualenvwrapper. However, my installation of these differs from the guide::

    pip install virtualenv virtualenvwrapper
    mkdir ~/.virtualenvs ~/Projects
    vi ~/.bashrc

Creates new file and so we add::

    # From virtualenvwrapper setup guide
    export WORKON_HOME=$HOME/.virtualenvs
    export PROJECT_HOME=$HOME/Projects
    source /usr/local/bin/virtualenvwrapper.sh

    # pip should only run if there is a virtualenv currently activated
    export PIP_REQUIRE_VIRTUALENV=true

    # Add a shortcut to the 'global' pip command for when global updates need to be managed
    gpip(){
       PIP_REQUIRE_VIRTUALENV="" pip "$@"
    }

Then as part of `vi ~/.bash_profile` we add::

    # Set architecture flags
    export ARCHFLAGS="-arch x86_64"
    # Ensure user-installed binaries take precedence
    export PATH=/usr/local/bin:$PATH
    # Load .bashrc if it exists
    test -f ~/.bashrc && source ~/.bashrc

    if [ -f $(brew --prefix)/etc/bash_completion ]; then
      . $(brew --prefix)/etc/bash_completion
    fi

    # Set local aliases
    alias ll='ls -laG'
    alias pm='python manage.py'

Don't forget to `source ~/.bash_profile` at the end if you want to use this session with this setup!

Project setup
-------------

With the base tools in place, we can now setup the development environment for the project itself. We'll create a
virtualenv project for development, then get the latest code from the central github repository. ::

    mkproject cope
    # NB: this sets CWD to /Users/$USER/Projects/cope
    git clone git@github.com:AllyBradley/COPE.git cope_repo
    ln -s ~/.virtualenvs/cope/lib ./lib
    ln -s ~/.virtualenvs/cope/bin ./bin
    pip install -r cope_repo/requirements/dev_home.txt
    cd cope-repo/
    chmod 775 manage.py
    cp local.env.template local.env
    vi local.env
    pm check                 # Should return 0 errors
    pm migrate
    pm collectstatic         # NB: Should point to the htdocs folder and ask for confirmation
    pm createsuperuser       # superuser is 'carl'
    pm loaddata config/fixtures/01_hospitals.json
    pm loaddata config/fixtures/02_persons.json
    pm loaddata config/fixtures/03_gradings.json
    pm loaddata config/fixtures/09_testusers.json


PyCharm setup
-------------

Now we can head to PyCharm and make the project available, along with debug options, etc

* Open PyCharm
* Select open project
  * Point it to $HOME/Projects/cope
* Allow it to index files and complete loading.
* You should be prompted to add a new VCS root for cope_repo. Do this.
* In Preferences, Enable Django support for the project
* Edit Configurations (toolbar) to create a Django Server project
* You can now test by running the testserver (^R for shortcut, or see toolbar button)



