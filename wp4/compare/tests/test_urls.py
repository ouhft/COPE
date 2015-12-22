from django.test import TestCase
from django.conf import settings
from django.core.urlresolvers import resolve
from django.utils import translation

from wp4.views import dashboard_index
from ..views import procurement_list


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
        root = resolve('/')
        self.assertEqual(root.func, dashboard_index)
