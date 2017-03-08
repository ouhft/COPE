#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.test import LiveServerTestCase
from django.core.urlresolvers import reverse

from selenium import webdriver
from selenium.webdriver.support.ui import Select

from wp4.staff.models import Person
# from wp4.locations.models import Hospital, UNITED_KINGDOM, BELGIUM, NETHERLANDS
# from wp4.staff_person.models import StaffJob, StaffPerson
# from ..models import RetrievalTeam, Randomisation

class CoreDataMixin(object):
    fixtures = [
        'config/fixtures/00_testdata.json',
        'config/fixtures/01_permissions.json',
        'config/fixtures/02_randomisation.json',
        'config/fixtures/10_hospitals.json',
        'config/fixtures/11_adverseevent_categories.json',
    ]

    @classmethod
    def setUpClass(cls):
        super(CoreDataMixin, cls).setUpClass()
        # Create the core user accounts
        cls.user_admin = Person.objects.get(pk=1)
        cls.user_admin.set_password('adminpass')  # Replace password from fixture
        cls.user_coordinator = Person.objects.create_user('coord', 'coord@test.com', 'coordpass')
        cls.user_technician = Person.objects.create_user('tech', 'tech@test.com', 'techpass')


# class ProcurementTestCase(CoreDataMixin, LiveServerTestCase):
#     def setUp(self):
#         self.browser = webdriver.Firefox()
#         self.browser.implicitly_wait(2)
#
#     def tearDown(self):
#         self.browser.quit()
#
#     def test_start_new_case(self):
#         """
#         Test that a tech user can start a new procurement case
#         :return:
#         """
#         # Visit the website
#         home_page = self.browser.get(self.live_server_url + '/')
#         brand_element = self.browser.find_element_by_css_selector('.navbar-brand')
#         self.assertEqual('COPE DB Online Trials System', brand_element.text)
#
#         # Need to login, so do that via the login form
#         navbar = self.browser.find_element_by_id("navbar")
#         login_button = navbar.find_element_by_class_name("navbar-btn")
#         self.assertEqual(login_button.text, "Login")
#         self.assertEqual(login_button.get_attribute('href'), self.live_server_url + reverse('login'))
#         login_button.click()
#         self.assertEqual(self.browser.current_url, self.live_server_url + reverse('login'))
#
#         # complete the login form
#         username_input = self.browser.find_element_by_css_selector('input[name=username')
#         self.assertEqual(username_input.get_attribute('placeholder'), 'Username')
#         username_input.send_keys('tech')
#         password_input = self.browser.find_element_by_css_selector('input[name=password')
#         self.assertEqual(password_input.get_attribute('placeholder'), 'Password')
#         password_input.send_keys('techpass')
#         password_input.submit()
#
#         # We are now back to the home page, so confirm we're signed in
#         self.assertEqual(self.browser.current_url, self.live_server_url + reverse('home'))
#         aboutme_menu = self.browser.find_element_by_id('navbar-menu-aboutme')
#         aboutme_menu.click()
#         menu_items = aboutme_menu.find_elements_by_tag_name('li')
#         self.assertEqual(len(menu_items), 6)
#         self.assertEqual(menu_items[0].get_attribute("class"), "dropdown-header")
#         self.assertEqual(menu_items[0].text, "About me")
#         self.assertEqual(menu_items[1].text, "Name: Transplant Technician")
#
#         # Now select Procurement from the WP4 menu
#         wp4_menu = self.browser.find_element_by_id('navbar-menu-wp4')
#         wp4_menu.click()
#         menu_items = wp4_menu.find_elements_by_tag_name('li')
#         self.assertEqual(len(menu_items), 5)
#         procurement_item = menu_items[0].find_element_by_tag_name('a')
#         self.assertEqual(procurement_item.text, "Procurement")
#         self.assertEqual(procurement_item.get_attribute('href'), self.live_server_url + reverse('wp4:compare:procurement_list'))
#         procurement_item.click()
#
#         # Check we are now on the Procurement listing screen
#         page_header = self.browser.find_element_by_css_selector('.page-header')
#         self.assertEqual('Procurement', page_header.text)
#         sub_headers = self.browser.find_elements_by_tag_name('h2')
#         self.assertEqual('Open Cases (0 cases)', sub_headers[0].text)
#         self.assertEqual('New Case', sub_headers[1].text)
#
#         # Fill in the new case form
#         new_case_form = self.browser.find_element_by_id('new-case-form')
#
#         retrieval_team_input = Select(new_case_form.find_element_by_css_selector('select[name=donor-retrieval_team'))
#         retrieval_team_input.select_by_value("1")  # Oxford, Churchill
#         self.assertEqual(retrieval_team_input.all_selected_options[0].text, '(15) Churchill Hospital, Oxford, United Kingdon')
#
#         # Name of MTO is a typeahead field, and we're a technician, so check we are selected
#         mto_text = self.browser.find_element_by_id('id_donor-perfusion_technician-deck')
#         self.assertContains('Transplant Technician', mto_text.text)
#
#         # password_input.send_keys('techpass')
#         # password_input.submit()
#
#         # import pdb;pdb.set_trace()
#         self.fail('Incomplete Test')
