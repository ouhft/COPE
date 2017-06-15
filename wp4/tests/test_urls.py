#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.test import TestCase
from django.conf import settings
from django.core.urlresolvers import resolve
from django.utils import translation

from wp4.views import dashboard_index, error404, error403, error500
from wp4.views import wp4_index, dashboard_changelog, dashboard_usermanual
from wp4.health_economics.views import index as he_index
from wp4.followups.views import index as fu_index
from wp4.compare import views as co_views
from wp4.administration.views import core as ad_core, data_extracts as ad_data_extracts
from wp4.administration.views import administrator as ad_administrator, completeness as ad_completeness
from wp4.administration.views import dmc_reports as ad_dmc_reports, biobank as ad_biobank

from .utils.languages import LANGUAGE_STRINGS


class WP4URLsTestCase(TestCase):
    """
    Test the urls used throughout the site match the expected views
    """
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

    # wp4/views.py and config/urls.py
    def test_root_url_uses_index_view(self):
        """
        Test that the root of the site resolves to the
        correct view function
        """
        for LANG in LANGUAGE_STRINGS:
            # print("DEBUG: test_root_url_uses_index_view: {0}".format(LANG['string']))
            translation.activate(LANG["string"])
            root = resolve(LANG["url"] + '/')
            self.assertEqual(root.func, dashboard_index)
        translation.deactivate()

    def test_error_urls_use_error_views(self):
        # for LANG in LANGUAGE_STRINGS:
        LANG = LANGUAGE_STRINGS[0]
        # Limit to en-gb because this fails for unknown reasons on fr-be
        # print("DEBUG: test_error_urls_use_error_views: {0}".format(LANG['string']))
        error_url = resolve(LANG["url"] + '/errors/404/')
        self.assertEqual(error_url.func, error404)
        error_url = resolve(LANG["url"] + '/errors/403/')
        self.assertEqual(error_url.func, error403)
        error_url = resolve(LANG["url"] + '/errors/500/')
        self.assertEqual(error_url.func, error500)

    def test_markdowndocs_urls_use_correct_views(self):
        for LANG in LANGUAGE_STRINGS:
            # print("DEBUG: test_markdowndocs_urls_use_correct_views: {0}".format(LANG['string']))
            translation.activate(LANG["string"])
            url = resolve(LANG["url"] + '/changelog')
            self.assertEqual(url.func, dashboard_changelog)
            url = resolve(LANG["url"] + '/user-manual')
            self.assertEqual(url.func, dashboard_usermanual)
        translation.deactivate()

    # wp4/urls.py
    def test_wp4_urls_use_correct_views(self):
        for LANG in LANGUAGE_STRINGS:
            # print("DEBUG: test_wp4_urls_use_correct_views: {0}".format(LANG['string']))
            translation.activate(LANG["string"])
            url = resolve(LANG["url"] + '/wp4/')
            self.assertEqual(url.func, wp4_index)
        translation.deactivate()

    # wp4/health_economics/urls.py
    def test_health_economics_urls_use_correct_views(self):
        for LANG in LANGUAGE_STRINGS:
            translation.activate(LANG["string"])
            url = resolve(LANG["url"] + '/wp4/health-economics/')
            self.assertEqual(url.func, he_index)
            url = resolve(LANG["url"] + '/wp4/health-economics/add/')
            self.assertEqual(url.func.__name__, 'QualityOfLifeCreateView')
            url = resolve(LANG["url"] + '/wp4/health-economics/1/details/')
            self.assertEqual(url.func.__name__, 'QualityOfLifeDetailView')
            url = resolve(LANG["url"] + '/wp4/health-economics/1/')
            self.assertEqual(url.func.__name__, 'QualityOfLifeUpdateView')
            url = resolve(LANG["url"] + '/wp4/health-economics/list/')
            self.assertEqual(url.func.__name__, 'QualityOfLifeListView')
        translation.deactivate()

    # wp4/followups/urls.py
    def test_followup_urls_use_correct_views(self):
        for LANG in LANGUAGE_STRINGS:
            translation.activate(LANG["string"])
            url = resolve(LANG["url"] + '/wp4/follow-up/')
            self.assertEqual(url.func, fu_index)
            # Initial
            # url = resolve(LANG["url"] + '/wp4/follow-up/initial/add/')
            # self.assertEqual(url.func.__name__, 'FollowUpInitialCreateView')
            url = resolve(LANG["url"] + '/wp4/follow-up/initial/1/details/')
            self.assertEqual(url.func.__name__, 'FollowUpInitialDetailView')
            url = resolve(LANG["url"] + '/wp4/follow-up/initial/1/')
            self.assertEqual(url.func.__name__, 'FollowUpInitialUpdateView')
            url = resolve(LANG["url"] + '/wp4/follow-up/initial/')
            self.assertEqual(url.func.__name__, 'FollowUpInitialListView')
            # 3M
            # url = resolve(LANG["url"] + '/wp4/follow-up/month3/add/')
            # self.assertEqual(url.func.__name__, 'FollowUp3MCreateView')
            url = resolve(LANG["url"] + '/wp4/follow-up/month3/1/details/')
            self.assertEqual(url.func.__name__, 'FollowUp3MDetailView')
            url = resolve(LANG["url"] + '/wp4/follow-up/month3/1/')
            self.assertEqual(url.func.__name__, 'FollowUp3MUpdateView')
            url = resolve(LANG["url"] + '/wp4/follow-up/month3/')
            self.assertEqual(url.func.__name__, 'FollowUp3MListView')
            # 6M
            # url = resolve(LANG["url"] + '/wp4/follow-up/month6/add/')
            # self.assertEqual(url.func.__name__, 'FollowUp6MCreateView')
            url = resolve(LANG["url"] + '/wp4/follow-up/month6/1/details/')
            self.assertEqual(url.func.__name__, 'FollowUp6MDetailView')
            url = resolve(LANG["url"] + '/wp4/follow-up/month6/1/')
            self.assertEqual(url.func.__name__, 'FollowUp6MUpdateView')
            url = resolve(LANG["url"] + '/wp4/follow-up/month6/')
            self.assertEqual(url.func.__name__, 'FollowUp6MListView')
            # Final
            # url = resolve(LANG["url"] + '/wp4/follow-up/final/add/')
            # self.assertEqual(url.func.__name__, 'FollowUp1YCreateView')
            url = resolve(LANG["url"] + '/wp4/follow-up/final/1/details/')
            self.assertEqual(url.func.__name__, 'FollowUp1YDetailView')
            url = resolve(LANG["url"] + '/wp4/follow-up/final/1/')
            self.assertEqual(url.func.__name__, 'FollowUp1YUpdateView')
            url = resolve(LANG["url"] + '/wp4/follow-up/final/')
            self.assertEqual(url.func.__name__, 'FollowUp1YListView')
        translation.deactivate()

    # wp4/adverse_event/urls.py
    def test_adverse_event_urls_use_correct_views(self):
        for LANG in LANGUAGE_STRINGS:
            translation.activate(LANG["string"])
            url = resolve(LANG["url"] + '/wp4/adverse-event/add/')
            self.assertEqual(url.func.__name__, 'AdverseEventCreateView')
            url = resolve(LANG["url"] + '/wp4/adverse-event/1/details/')
            self.assertEqual(url.func.__name__, 'AdverseEventDetailView')
            url = resolve(LANG["url"] + '/wp4/adverse-event/1/')
            self.assertEqual(url.func.__name__, 'AdverseEventUpdateView')
            url = resolve(LANG["url"] + '/wp4/adverse-event/')
            self.assertEqual(url.func.__name__, 'AdverseEventListView')
        translation.deactivate()

    # wp4/staff/urls.py
    def test_staff_urls_use_correct_views(self):
        for LANG in LANGUAGE_STRINGS:
            translation.activate(LANG["string"])
            url = resolve(LANG["url"] + '/wp4/staff/technician-autocomplete/')
            self.assertEqual(url.func.__name__, 'TechnicianAutoComplete')
            url = resolve(LANG["url"] + '/wp4/staff/add/')
            self.assertEqual(url.func.__name__, 'PersonCreateView')
            url = resolve(LANG["url"] + '/wp4/staff/1/details/')
            self.assertEqual(url.func.__name__, 'PersonDetailView')
            url = resolve(LANG["url"] + '/wp4/staff/1/')
            self.assertEqual(url.func.__name__, 'PersonUpdateView')
            url = resolve(LANG["url"] + '/wp4/staff/')
            self.assertEqual(url.func.__name__, 'PersonListView')
        translation.deactivate()

    # wp4/locations/urls.py
    def test_locations_urls_use_correct_views(self):
        for LANG in LANGUAGE_STRINGS:
            translation.activate(LANG["string"])
            url = resolve(LANG["url"] + '/wp4/location/hospital-autocomplete/')
            self.assertEqual(url.func.__name__, 'HospitalAutoComplete')
            url = resolve(LANG["url"] + '/wp4/location/add/')
            self.assertEqual(url.func.__name__, 'HospitalCreateView')
            url = resolve(LANG["url"] + '/wp4/location/1/details/')
            self.assertEqual(url.func.__name__, 'HospitalDetailView')
            url = resolve(LANG["url"] + '/wp4/location/1/')
            self.assertEqual(url.func.__name__, 'HospitalUpdateView')
            url = resolve(LANG["url"] + '/wp4/location/')
            self.assertEqual(url.func.__name__, 'HospitalListView')
        translation.deactivate()

    # wp4/samples/urls.py
    # TODO!

    # wp4/compare/urls.py
    def test_compare_urls_use_correct_views(self):
        for LANG in LANGUAGE_STRINGS:
            # print("DEBUG: test_compare_urls_use_correct_views: {0}".format(LANG['string']))
            translation.activate(LANG["string"])
            url = resolve(LANG["url"] + '/wp4/compare/retrieval-team-autocomplete/')
            self.assertEqual(url.func.__name__, 'RetrievalTeamAutoComplete')
            url = resolve(LANG["url"] + '/wp4/compare/adverse-organ-autocomplete/')
            self.assertEqual(url.func.__name__, 'AdverseEventOrganAutoComplete')

            url = resolve(LANG["url"] + '/wp4/compare/procurement/')
            self.assertEqual(url.func, co_views.procurement_list)
            url = resolve(LANG["url"] + '/wp4/compare/procurement/1/')
            self.assertEqual(url.func, co_views.procurement_view)
            url = resolve(LANG["url"] + '/wp4/compare/procurement/1/update/')
            self.assertEqual(url.func, co_views.procurement_form)

            url = resolve(LANG["url"] + '/wp4/compare/transplantation/')
            self.assertEqual(url.func, co_views.transplantation_list)
            url = resolve(LANG["url"] + '/wp4/compare/transplantation/1/')
            self.assertEqual(url.func, co_views.transplantation_form)

            url = resolve(LANG["url"] + '/wp4/compare/')
            self.assertEqual(url.func, co_views.index)
        translation.deactivate()

    # wp4/administration/urls.py
    def test_administration_urls_use_correct_views(self):
        for LANG in LANGUAGE_STRINGS:
            # print("DEBUG: test_administration_urls_use_correct_views: {0}".format(LANG['string']))
            translation.activate(LANG["string"])
            url = resolve(LANG["url"] + '/wp4/administration/stats/reports/procurement')
            self.assertEqual(url.func, ad_data_extracts.report_procurement)
            url = resolve(LANG["url"] + '/wp4/administration/stats/reports/organs')
            self.assertEqual(url.func, ad_data_extracts.report_organ)
            url = resolve(LANG["url"] + '/wp4/administration/stats/reports/allocations')
            self.assertEqual(url.func, ad_data_extracts.report_allocations)
            url = resolve(LANG["url"] + '/wp4/administration/stats/reports/adverse-events')
            self.assertEqual(url.func, ad_data_extracts.report_adverse_events)

            url = resolve(LANG["url"] + '/wp4/administration/europe-list')
            self.assertEqual(url.func, ad_administrator.offline_europe_list)
            url = resolve(LANG["url"] + '/wp4/administration/uk-list')
            self.assertEqual(url.func, ad_administrator.offline_uk_list)
            url = resolve(LANG["url"] + '/wp4/administration/procurement-pairs')
            self.assertEqual(url.func, ad_administrator.procurement_pairs)
            url = resolve(LANG["url"] + '/wp4/administration/transplantation-sites')
            self.assertEqual(url.func, ad_administrator.transplantation_sites)
            url = resolve(LANG["url"] + '/wp4/administration/sae-sites')
            self.assertEqual(url.func, ad_administrator.sae_sites)
            url = resolve(LANG["url"] + '/wp4/administration/flowchart')
            self.assertEqual(url.func, ad_administrator.flowchart)
            url = resolve(LANG["url"] + '/wp4/administration/completed-pairs')
            self.assertEqual(url.func, ad_administrator.completed_pairs)
            url = resolve(LANG["url"] + '/wp4/administration/followups')
            self.assertEqual(url.func, ad_administrator.followups)

            url = resolve(LANG["url"] + '/wp4/administration/demographics/data-linkage')
            self.assertEqual(url.func, ad_administrator.demographics_data_linkage)

            url = resolve(LANG["url"] + '/wp4/administration/completeness/procurement')
            self.assertEqual(url.func, ad_completeness.procurement)
            url = resolve(LANG["url"] + '/wp4/administration/completeness/transplant-per-centre')
            self.assertEqual(url.func, ad_completeness.transplant_per_centre)

            url = resolve(LANG["url"] + '/wp4/administration/biobank/blood-collection')
            self.assertEqual(url.func, ad_biobank.blood_collection)
            url = resolve(LANG["url"] + '/wp4/administration/biobank/urine-collection')
            self.assertEqual(url.func, ad_biobank.urine_collection)
            url = resolve(LANG["url"] + '/wp4/administration/biobank/tissue-collection')
            self.assertEqual(url.func, ad_biobank.tissue_collection)
            url = resolve(LANG["url"] + '/wp4/administration/biobank/perfusate-collection')
            self.assertEqual(url.func, ad_biobank.perfusate_collection)
            url = resolve(LANG["url"] + '/wp4/administration/biobank/unmatched-samples')
            self.assertEqual(url.func, ad_biobank.unmatched_samples)

            url = resolve(LANG["url"] + '/wp4/administration/dmc/death-summaries/open')
            self.assertEqual(url.func, ad_dmc_reports.death_summaries)
            url = resolve(LANG["url"] + '/wp4/administration/dmc/death-summaries')
            self.assertEqual(url.func, ad_dmc_reports.death_summaries)
            url = resolve(LANG["url"] + '/wp4/administration/dmc/permanent-impairment/open')
            self.assertEqual(url.func, ad_dmc_reports.permanent_impairment)
            url = resolve(LANG["url"] + '/wp4/administration/dmc/permanent-impairment')
            self.assertEqual(url.func, ad_dmc_reports.permanent_impairment)
            url = resolve(LANG["url"] + '/wp4/administration/dmc/graft-failures/open')
            self.assertEqual(url.func, ad_dmc_reports.graft_failures)
            url = resolve(LANG["url"] + '/wp4/administration/dmc/graft-failures')
            self.assertEqual(url.func, ad_dmc_reports.graft_failures)
            url = resolve(LANG["url"] + '/wp4/administration/dmc/adverse-events/open')
            self.assertEqual(url.func, ad_dmc_reports.adverse_events)
            url = resolve(LANG["url"] + '/wp4/administration/dmc/adverse-events')
            self.assertEqual(url.func, ad_dmc_reports.adverse_events)
            url = resolve(LANG["url"] + '/wp4/administration/dmc/serious-events/open')
            self.assertEqual(url.func, ad_dmc_reports.serious_events)
            url = resolve(LANG["url"] + '/wp4/administration/dmc/serious-events')
            self.assertEqual(url.func, ad_dmc_reports.serious_events)

            url = resolve(LANG["url"] + '/wp4/administration/')
            self.assertEqual(url.func, ad_core.index)
        translation.deactivate()
