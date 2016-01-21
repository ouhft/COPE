#!/usr/bin/python
# coding: utf-8
from django.test import LiveServerTestCase
from django.contrib.auth.models import User
from django.core.urlresolvers import reverse

from selenium import webdriver
from selenium.webdriver.support.ui import Select

from wp4.locations.models import Hospital, UNITED_KINGDOM, BELGIUM, NETHERLANDS
from wp4.staff_person.models import StaffJob, StaffPerson
from ..models import RetrievalTeam, Randomisation

class CoreDataMixin(object):
    @classmethod
    def setUpClass(cls):
        super(CoreDataMixin, cls).setUpClass()
        # Create the core user accounts
        cls.user_admin = User.objects.create_superuser('admin', 'admin@test.com', 'adminpass')
        cls.user_coordinator = User.objects.create_user('coord', 'coord@test.com', 'coordpass')
        cls.user_technician = User.objects.create_user('tech', 'tech@test.com', 'techpass')
        # Setup the JobRoles
        cls.staffjob_technician = StaffJob.objects.create(description="Perfusion Technician")     # 1
        cls.staffjob_tcoord = StaffJob.objects.create(description="Transplant Co-ordinator")      # 2
        cls.staffjob_ncoord = StaffJob.objects.create(description="National Co-ordinator")        # 4
        cls.staffjob_sysadmin = StaffJob.objects.create(description="Sys-admin")                  # 7
        cls.staffjob_other = StaffJob.objects.create(description="Other Project Member")          # 9
        cls.staffjob_theatre = StaffJob.objects.create(description="Transplant Theatre Contact")  # 15
        # Setup core locations
        cls.locations_hospital_gb = Hospital.objects.create(
            name="Churchill Hospital, Oxford",
            country=UNITED_KINGDOM,
            is_active=True,
            is_project_site=True,
            created_by=cls.user_admin
        )
        cls.locations_hospital_be = Hospital.objects.create(
            name="Universitaire Ziekenhuizen Leuven",
            country=BELGIUM,
            is_active=True,
            is_project_site=True,
            created_by=cls.user_admin
        )
        cls.locations_hospital_nl = Hospital.objects.create(
            name="Universitair Medisch Centrum Groningen",
            country=NETHERLANDS,
            is_active=True,
            is_project_site=True,
            created_by=cls.user_admin
        )
        # Create core retrieval teams
        cls.compare_retrievalteam_gb = RetrievalTeam.objects.create(
            centre_code="15",
            based_at=cls.locations_hospital_gb,
            created_by=cls.user_admin
        )
        cls.compare_retrievalteam_be = RetrievalTeam.objects.create(
            centre_code="41",
            based_at=cls.locations_hospital_be,
            created_by=cls.user_admin
        )
        cls.compare_retrievalteam_nl = RetrievalTeam.objects.create(
            centre_code="51",
            based_at=cls.locations_hospital_nl,
            created_by=cls.user_admin
        )
        # Setup the core people
        cls.staffperson_admin = StaffPerson.objects.create(
            first_names="System",
            last_names="Admin",
            based_at=cls.locations_hospital_gb,
            email="admin@test.com",
            user=cls.user_admin,
            created_by=cls.user_admin
        )
        cls.staffperson_admin.jobs.add(cls.staffjob_sysadmin)
        cls.staffperson_admin.save()
        cls.staffperson_coord = StaffPerson.objects.create(
            first_names="National",
            last_names="Coordinator",
            based_at=cls.locations_hospital_gb,
            email="coord@test.com",
            user=cls.user_coordinator,
            created_by=cls.user_admin
        )
        cls.staffperson_coord.jobs.add(cls.staffjob_ncoord)
        cls.staffperson_coord.save()
        cls.staffperson_technician = StaffPerson.objects.create(
            first_names="Transplant",
            last_names="Technician",
            based_at=cls.locations_hospital_gb,
            email="tech@test.com",
            user=cls.user_technician,
            created_by=cls.user_admin
        )
        cls.staffperson_technician.jobs.add(cls.staffjob_technician)
        cls.staffperson_technician.save()
        # Create randomisation records
        cls.compare_random_01 = Randomisation.objects.create(country=UNITED_KINGDOM, result=True)
        cls.compare_random_02 = Randomisation.objects.create(country=UNITED_KINGDOM, result=False)
        cls.compare_random_03 = Randomisation.objects.create(country=UNITED_KINGDOM, result=False)
        cls.compare_random_04 = Randomisation.objects.create(country=UNITED_KINGDOM, result=True)
        cls.compare_random_05 = Randomisation.objects.create(country=UNITED_KINGDOM, result=True)
        cls.compare_random_06 = Randomisation.objects.create(country=UNITED_KINGDOM, result=True)
        cls.compare_random_07 = Randomisation.objects.create(country=UNITED_KINGDOM, result=False)
        cls.compare_random_08 = Randomisation.objects.create(country=UNITED_KINGDOM, result=False)
        cls.compare_random_09 = Randomisation.objects.create(country=UNITED_KINGDOM, result=True)
        cls.compare_random_10 = Randomisation.objects.create(country=UNITED_KINGDOM, result=False)


class ProcurementTestCase(CoreDataMixin, LiveServerTestCase):
    def setUp(self):
        self.browser = webdriver.Firefox()
        self.browser.implicitly_wait(2)

    def tearDown(self):
        self.browser.quit()

    def test_start_new_case(self):
        """
        Test that a tech user can start a new procurement case
        :return:
        """
        # Visit the website
        home_page = self.browser.get(self.live_server_url + '/')
        brand_element = self.browser.find_element_by_css_selector('.navbar-brand')
        self.assertEqual('COPE DB Online Trials System', brand_element.text)

        # Need to login, so do that via the login form
        navbar = self.browser.find_element_by_id("navbar")
        login_button = navbar.find_element_by_class_name("navbar-btn")
        self.assertEqual(login_button.text, "Login")
        self.assertEqual(login_button.get_attribute('href'), self.live_server_url + reverse('login'))
        login_button.click()
        self.assertEqual(self.browser.current_url, self.live_server_url + reverse('login'))

        # complete the login form
        username_input = self.browser.find_element_by_css_selector('input[name=username')
        self.assertEqual(username_input.get_attribute('placeholder'), 'Username')
        username_input.send_keys('tech')
        password_input = self.browser.find_element_by_css_selector('input[name=password')
        self.assertEqual(password_input.get_attribute('placeholder'), 'Password')
        password_input.send_keys('techpass')
        password_input.submit()

        # We are now back to the home page, so confirm we're signed in
        self.assertEqual(self.browser.current_url, self.live_server_url + reverse('home'))
        aboutme_menu = self.browser.find_element_by_id('navbar-menu-aboutme')
        aboutme_menu.click()
        menu_items = aboutme_menu.find_elements_by_tag_name('li')
        self.assertEqual(len(menu_items), 6)
        self.assertEqual(menu_items[0].get_attribute("class"), "dropdown-header")
        self.assertEqual(menu_items[0].text, "About me")
        self.assertEqual(menu_items[1].text, "Name: Transplant Technician")

        # Now select Procurement from the WP4 menu
        wp4_menu = self.browser.find_element_by_id('navbar-menu-wp4')
        wp4_menu.click()
        menu_items = wp4_menu.find_elements_by_tag_name('li')
        self.assertEqual(len(menu_items), 5)
        procurement_item = menu_items[0].find_element_by_tag_name('a')
        self.assertEqual(procurement_item.text, "Procurement")
        self.assertEqual(procurement_item.get_attribute('href'), self.live_server_url + reverse('wp4:compare:procurement_list'))
        procurement_item.click()

        # Check we are now on the Procurement listing screen
        page_header = self.browser.find_element_by_css_selector('.page-header')
        self.assertEqual('Procurement', page_header.text)
        sub_headers = self.browser.find_elements_by_tag_name('h2')
        self.assertEqual('Open Cases (0 cases)', sub_headers[0].text)
        self.assertEqual('New Case', sub_headers[1].text)

        # Fill in the new case form
        new_case_form = self.browser.find_element_by_id('new-case-form')

        retrieval_team_input = Select(new_case_form.find_element_by_css_selector('select[name=donor-retrieval_team'))
        retrieval_team_input.select_by_value("1")  # Oxford, Churchill
        self.assertEqual(retrieval_team_input.all_selected_options[0].text, '(15) Churchill Hospital, Oxford, United Kingdon')

        # Name of MTO is a typeahead field, and we're a technician, so check we are selected
        mto_text = self.browser.find_element_by_id('id_donor-perfusion_technician-deck')
        self.assertContains('Transplant Technician', mto_text.text)

        # password_input.send_keys('techpass')
        # password_input.submit()

        # import pdb;pdb.set_trace()
        self.fail('Incomplete Test')
