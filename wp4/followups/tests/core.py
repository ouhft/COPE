#!/usr/bin/python
# coding: utf-8
from django.test import LiveServerTestCase
from django.contrib.auth.models import User
from django.core.urlresolvers import reverse

from selenium import webdriver
from selenium.webdriver.support.ui import Select