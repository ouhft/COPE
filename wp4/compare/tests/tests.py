from django.test import LiveServerTestCase
from selenium import webdriver


class ProcurementTestCase(LiveServerTestCase):
    def setUp(self):
        self.browser = webdriver.Firefox()
        self.browser.implicitly_wait(2)

    def tearDown(self):
        self.browser.quit()

    def test_start_new_case(self):
        """
        Test that a user can start a new procurement case
        :return:
        """
        # Blah
        home_page = self.browser.get(self.live_server_url + '/')
        brand_element = self.browser.find_element_by_css_selector('.navbar-brand')
        self.assertEqual('COPE DB Online Trials System', brand_element.text)

        self.fail('Incomplete Test')
