#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.test import TestCase, RequestFactory
from django.test.client import Client
from django.contrib.auth.models import User
from django.core.urlresolvers import reverse

from ..views import index, procurement_list, transplantation_list
from .tests import CoreDataMixin

LANGUAGE_URL_GB = '/en-gb'
PACKAGE_URL = '/wp4'
APP_URL = '/compare'
BASE_URL = LANGUAGE_URL_GB + PACKAGE_URL + APP_URL


class CompareViewsTestCase(CoreDataMixin, TestCase):
    def setUp(self):
        self.factory = RequestFactory()
        self.client = Client()
        self.user = User.objects.get_by_natural_key("tech")

    def test_login(self):
        """
        Test that we can login as a user for protected views
        :return:
        """
        self.client.login(username='tech', password='techpass')
        response = self.client.get(reverse('login'))
        self.assertEqual(response.status_code, 200)

    def test_index_view_basic(self):
        """
        Test that index view returns a 200 response and uses
        the correct template
        """
        request = self.factory.get(BASE_URL + '/')
        request.user = self.user  # Fake being logged in
        with self.assertTemplateUsed('compare/index.html'):
            response = index(request)
            self.assertEqual(response.status_code, 200)

    def test_procurement_list_view_basic(self):
        """
        Test that procurement_list view returns a 200 response and uses
        the correct template
        """
        request = self.factory.get(BASE_URL + '/Procurement/')
        request.user = self.user  # Fake being logged in
        with self.assertTemplateUsed('compare/procurement_list.html'):
            response = procurement_list(request)
            self.assertEqual(response.status_code, 200)

    def test_transplant_list_view_basic(self):
        """
        Test that transplantation_list view returns a 200 response and uses
        the correct template
        """
        request = self.factory.get(BASE_URL + '/Transplant/')
        request.user = self.user  # Fake being logged in
        with self.assertTemplateUsed('compare/transplantation_list.html'):
            response = transplantation_list(request)
            self.assertEqual(response.status_code, 200)
