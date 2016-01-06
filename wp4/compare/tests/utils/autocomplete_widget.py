#!/usr/bin/python
# coding: utf-8

# Utility to drive the Autocomplete light widget, similar to the functionality in selenium.webdriver.support.ui

from selenium.common.exceptions import NoSuchElementException, UnexpectedTagNameException


class AutoComplete:
    def __init__(self, webelement):
        """
        Check that the given element is an autocomplete widget, and that all components can be found.
        If not, then an UnexpectedTagNameException is thrown.
        :param webelement: span element with id ending in '-wrapper' and containing class 'autocomplete-light-widget'
        """
        if webelement.tag_name.lower() != "span":
            raise UnexpectedTagNameException(
                "AutoComplete only works on <span> elements, not on <%s>" %
                webelement.tag_name)
        if webelement.get_attribute('id').split('-')[-1].lower() != 'wrapper':
            raise UnexpectedTagNameException(
                "AutoComplete only works on <span> elements with an id ending in -wrapper"
            )
        if 'autocomplete-light-widget' not in webelement.get_attribute('class').split(' '):
            raise UnexpectedTagNameException(
                "AutoComplete only works on <span> elements with a class containing in autocomplete-light-widget"
            )

# TODO: Complete this?
