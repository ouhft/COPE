#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.test import TestCase
from django.conf import settings
from django.core.urlresolvers import resolve
from django.utils import translation

from wp4.views import dashboard_index, error404, error403, error500, wp4_index
from wp4.samples.views import sample_home, sample_form
from wp4.locations.views import HospitalListView, HospitalDetailView, HospitalUpdateView, HospitalCreateView
from ..views import procurement_list, transplantation_list, index, procurement_form, transplantation_form


class CompareURLsTestCase(TestCase):
    def test_four_languages(self):
        # Test default language matches the settings
        self.assertEqual(translation.get_language(), settings.LANGUAGE_CODE)
        # Set language specifics and test that translation is picking up the changes
        translation.activate('en-gb')
        with translation.override('fr-be'):
            self.assertEqual(translation.get_language(), 'fr-be')
        self.assertEqual(translation.get_language(), 'en-gb')
        with translation.override('nl-nl'):
            self.assertEqual(translation.get_language(), 'nl-nl')
        self.assertEqual(translation.get_language(), 'en-gb')
        # Reset and test
        translation.deactivate()
        self.assertEqual(translation.get_language(), settings.LANGUAGE_CODE)

    def test_root_url_uses_index_view(self):
        """
        Test that the root of the site resolves to the
        correct view function
        """
        translation.activate('en-GB')
        root = resolve('/en-gb/')
        self.assertEqual(root.func, dashboard_index)

    def test_error_urls_use_error_views(self):
        error_url = resolve('/en-gb/errors/404/')
        self.assertEqual(error_url.func, error404)
        error_url = resolve('/en-gb/errors/403/')
        self.assertEqual(error_url.func, error403)
        error_url = resolve('/en-gb/errors/500/')
        self.assertEqual(error_url.func, error500)

    def test_wp4_urls_use_correct_views(self):
        url = resolve('/en-gb/wp4/')
        self.assertEqual(url.func, wp4_index)

    def test_compare_urls_use_correct_views(self):
        url = resolve('/en-gb/wp4/compare/')
        self.assertEqual(url.func, index)
        url = resolve('/en-gb/wp4/compare/procurement/')
        self.assertEqual(url.func, procurement_list)
        url = resolve('/en-gb/wp4/compare/transplantation/')
        self.assertEqual(url.func, transplantation_list)
        url = resolve('/en-gb/wp4/compare/procurement/1/')
        self.assertEqual(url.func, procurement_form)
        url = resolve('/en-gb/wp4/compare/transplantation/1/')
        self.assertEqual(url.func, transplantation_form)

    def test_sample_urls_use_correct_views(self):
        url = resolve('/en-gb/wp4/sample/')
        self.assertEqual(url.func, sample_home)
        url = resolve('/en-gb/wp4/sample/1/')
        self.assertEqual(url.func, sample_form)

    def test_location_urls_use_correct_views(self):
        url = resolve('/en-gb/wp4/location/')
        # import pdb;pdb.set_trace()
        self.assertEqual(url.func.__name__, 'HospitalListView')
        url = resolve('/en-gb/wp4/location/1/')
        self.assertEqual(url.func.__name__, 'HospitalUpdateView')
        url = resolve('/en-gb/wp4/location/1/details/')
        self.assertEqual(url.func.__name__, 'HospitalDetailView')
        url = resolve('/en-gb/wp4/location/add/')
        self.assertEqual(url.func.__name__, 'HospitalCreateView')
