# Development

This is a fairly typical Python + Django development. Hosting is Linux (Ubuntu); Development is UNIX (macOS). Local development system is presently (Jun 2019) macOS 10.14.5 (Mojave), and CM has this environment setup in two locations so far. 

## Maintenance
    
Periodically Homebrew needs updates, so copy these steps:

```
brew update
brew upgrade
brew doctor
```
Be aware that updates to the minor (and major) version of Python will change the symlinks from the virtualenvs, and will need recreating.

## Setup

This document mostly details how the second environment was built (to match the first - which has evolved from OS X 10.7 times). Most of the approach is based on [http://hackercodex.com/guide/python-development-environment-on-mac-osx/]() (which is the updated version of the guide used for the past 4 years)

### User credentials

As a new user account (e.g. on a new machine) we need to create an SSH key for access to things like GitHub and cope.nds Server.

    mkdir ~/.ssh
    chmod 700 ~/.ssh
    ssh-keygen -t rsa -b 4096
    # Follow onscreen prompts for name of key and empty passphrase

With a key generated, it can be added to:

* cope.nds authorised_keys
* dev.nds authorised_keys
* GitHub personal account (e.g. marshalc) keys for user with write access on the repository.

This needs to be done for GitHub at least, before you can do the project setup below


### Third party apps

The following have been used during the build and maintenance of this system:

* **IDE:** PyCharm - from [https://www.jetbrains.com/pycharm/](). I'm using the Professional 2017.2 version.
* **Utility:** CodeKit - from [http://incident57.com/codekit/](). Currently v3.3. This handles most of the UI library compilation (could be refactored to use Webkit, etc)
* **Utility:** CyberDuck - from [https://cyberduck.io](). Currently v6.2.2. This is for sftp transfer convenience.
* **Base libraries:** Homebrew - from [http://brew.sh](). More on its installation later.

#### Brew installation

Easy enough, just follow the instructions on the Homebrew website.

    ruby -e "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install)"
    # Accept suggestions, and enter password for sudo
    brew doctor
    # Recommends running the xcode install to get the 10.10 SDK which isn't implicit in the XCode 7 install
    xcode-select --install
    # This prompts a gui installer to pop up, so click to accept.

Of the various base libraries that come up for use in various projects, one that seems to be key is `readline`, so we'll install that now

    brew install readline
    # Also helpful, but not essential
    brew install bash-completion ssh-copy-id wget

### Python setup

*NB: These steps were valid back in Aug 2017, however package standards and base OS libraries have evolved since then so some details will need to be modified for a new setup now - for example, python 2 is almost non-existent in the process.*

Start by putting Homebrew's python installs into the system to avoid conflicts with the system python

    brew install python python3 gettext
    brew link gettext --force   # This is to enable the language files to be processed in Django

Have installed both python (2.7.10) and python3 (3.6) brew packages, which gives us setuptools, sqlite3 and pip.

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

### Project setup

With the base tools in place, we can now setup the development environment for the project itself. We'll create a `virtualenv` project for development, then get the latest code from the central github repository. *NB: This has been updated to reflect the change from python 2.7 to python 3 (3.7):*

    mkproject -p /usr/local/bin/python3 cope
    # NB: this sets CWD to /Users/$USER/Projects/cope
    git clone https://github.com/ouh-churchill/COPE.git cope_repo
    ln -s ~/.virtualenvs/cope/lib ./lib
    ln -s ~/.virtualenvs/cope/bin ./bin
    pip install --upgrade pip setuptools wheel
    pip install -r cope_repo/requirements/development.txt
    cd cope_repo/
    chmod 775 manage.py
    cp local.env.template local.env
    vi local.env
    vi location.env          # add the word "development" for settings to work
    pm check                 # Should return 0 errors
    pm migrate
    pm collectstatic         # NB: Should point to the htdocs folder and ask for confirmation
    pm createsuperuser       # superuser is you
    pm makemessages
    pm compilemessages
    
    # Fixture loading needs to be updated to reflect current project setup
    ## pm loaddata config/fixtures/01_hospitals.json


### PyCharm setup

Now we can head to PyCharm and make the project available, along with debug options, etc

* Open PyCharm
* Select open project
  * Point it to `$HOME/Projects/cope`
* Allow it to index files and complete loading.
* You should be prompted to add a new VCS root for cope_repo. Do this.
* In Preferences, Enable Django support for the project
* Edit Configurations (toolbar) to create a Django Server project
* You can now test by running the testserver (^R for shortcut, or see toolbar button)


## Useful things to remember

* `pip list -o` will list the installed packages that have been superseded/outdated
* Unrelated, but likely useful to remember: Command to get pip to update all installed packages

    ```
    pip list --outdated
    pip freeze --local | grep -v '^\-e' | cut -d = -f 1  | xargs -n1 pip install -U
    ```

## Documentation
Sphinx is now installed in the development branch to allow documentation to be generated. Notes on the setup of this can be found at: [https://gist.github.com/marshalc/327fc737ce0557a253c0c3d57f679292]()