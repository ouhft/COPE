#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.test import TestCase, RequestFactory
from django.test.client import Client
from django.urls import reverse

from wp4.staff.models import Person
from wp4 import views as wp4_views
from wp4.compare import views as c_views
from wp4.samples import views as s_views
from wp4.followups import views as f_views
from wp4.health_economics import views as he_views

from .tests import CoreDataMixin

BASE_URL_UK = "/en-gb"
BASE_URL_WP4 = BASE_URL_UK + "/wp4"
BASE_URL_COMPARE = BASE_URL_WP4 + "/compare"


def test_pattern(this, url, template, view):
    """
    Test that a given url and view returns a 200 response and uses the expected template
    :return:
    """
    request = this.factory.get(url)
    request.user = this.user  # Fake being logged in
    with this.assertTemplateUsed(template):
        response = view(request)
        this.assertEqual(response.status_code, 200)

def test_anti_pattern(this, url, template, view):
    """
    Test that a given url and view returns a 404 response and does not use the expected template
    :return:
    """
    request = this.factory.get(url)
    request.user = this.user  # Fake being logged in
    with this.assertTemplateNotUsed(template):
        response = view(request)
        this.assertEqual(response.status_code, 404)


class CompareViewsTestCase(CoreDataMixin, TestCase):
    BASE_URL = "/en-gb/wp4"

    def setUp(self):
        self.factory = RequestFactory()
        self.client = Client()
        self.user = Person.objects.get(pk=1)  # Admin user - has access all areas

    def test_login(self):
        """
        Test that we can login as a user for protected views
        :return:
        """
        self.client.login(username='tech', password='techpass')
        response = self.client.get(reverse('login'))
        self.assertEqual(response.status_code, 200)

    def test_root_views_basic(self):
        """
        Test urls from first level of lang/
        """
        test_pattern(self, BASE_URL_UK + '/', 'dashboard/index.html', wp4_views.dashboard_index)
        test_pattern(self, BASE_URL_UK + '/changelog', 'dashboard/changelog.html', wp4_views.dashboard_changelog)
        test_pattern(self, BASE_URL_UK + '/user-manual', 'dashboard/user_manual.html', wp4_views.dashboard_usermanual)

    def test_wp4_views_basic(self):
        """
        Test urls from second level of lang/wp4
        """
        test_pattern(self, BASE_URL_WP4 + '/', 'dashboard/wp4_index.html', wp4_views.wp4_index)

    def test_compare_view_basic(self):
        """
        Test urls from third level of lang/wp4/compare
        """
        test_pattern(self, BASE_URL_COMPARE + '/', 'compare/index.html', c_views.index)
        test_pattern(
            self,
            BASE_URL_COMPARE + '/procurement/',
            'compare/procurement_list.html',
            c_views.procurement_list
        )
        test_pattern(
            self,
            BASE_URL_COMPARE + '/transplant/',
            'compare/transplantation_list.html',
            c_views.transplantation_list
        )

    def test_samples_view_basic(self):
        """
        Test urls from third level of lang/wp4/samples
        """
        test_pattern(self, BASE_URL_WP4 + '/samples/', 'samples/index.html', s_views.index)

    def test_followups_view_basic(self):
        """
        Test urls from third level of lang/wp4/follow-up
        """
        test_pattern(self, BASE_URL_WP4 + '/follow-up/', 'followups/index.html', f_views.index)
        # test_pattern(
        #     self,
        #     BASE_URL_WP4 + '/follow-up/initial/',
        #     'followups/followupinitial_list.html',
        #     f_views.FollowUpInitialListView.as_view()
        # )
        # test_pattern(
        #     self,
        #     BASE_URL_WP4 + '/follow-up/month3/',
        #     'followups/followup3m_list.html',
        #     f_views.FollowUp3MListView.as_view()
        # )
        # test_pattern(
        #     self,
        #     BASE_URL_WP4 + '/follow-up/month6/',
        #     'followups/followup6m_list.html',
        #     f_views.FollowUp6MListView.as_view()
        # )
        # test_pattern(
        #     self,
        #     BASE_URL_WP4 + '/follow-up/final/',
        #     'followups/followup1y_list.html',
        #     f_views.FollowUp1YListView.as_view()
        # )
        # CAN'T DO TEMPLATE TEST ON GCBV - see https://github.com/AllyBradley/COPE/wiki/Testing-GCBV-doesn't-work-as-you'd-expect

    def test_health_economics_view_basic(self):
        """
        Test urls from third level of lang/wp4/health-economics
        """
        test_pattern(self, BASE_URL_WP4 + '/health-economics/', 'health_economics/index.html', he_views.index)

    def test_adverse_event_view_basic(self):
        """
        Test urls from third level of lang/wp4/adverse-event
        """
        # request = self.factory.get(BASE_URL_WP4 + '/adverse-event/')
        # request.user = self.user  # Fake being logged in
        # response = ae_views.AdverseEventListView.as_view()(request)
        # print("test_adverse_event_view_basic: {0} - {1} - {2}".format(
        #     response.template_name, response.is_rendered, response.rendered_content))
        #
        # test_anti_pattern(
        #     self,
        #     BASE_URL_WP4 + '/adverse-event/',
        #     'adverse_event/event_list.html',
        #     ae_views.AdverseEventListView.as_view()
        # )
        # CAN'T DO TEMPLATE TEST ON GCBV - see https://github.com/AllyBradley/COPE/wiki/Testing-GCBV-doesn't-work-as-you'd-expect
        pass
