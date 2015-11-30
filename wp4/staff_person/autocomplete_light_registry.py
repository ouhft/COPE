#!/usr/bin/python
# coding: utf-8
import autocomplete_light.shortcuts as al

from .models import StaffPerson, StaffJob

al.register(
    StaffPerson,
    name='TechnicianAutoComplete',
    choices=StaffPerson.objects.filter(jobs__in=[StaffJob.PERFUSION_TECHNICIAN]),
    search_fields=['first_names', 'last_names']
)

#
# class TransplantCoordinatorAutoComplete(al.AutocompleteModelBase):
#     choices = StaffPerson.objects.filter(jobs__in=[StaffJob.TRANSPLANT_COORDINATOR])
#     search_fields = ['first_names', 'last_names']
#     attrs = {
#         # This will set the input placeholder attribute:
#         'placeholder': "Please type in the their name",
#         # This will set the yourlabs.Autocomplete.minimumCharacters
#         # options, the naming conversion is handled by jQuery
#         'data-autocomplete-minimum-characters': 1,
#     }
#     widget_attrs = {
#         'data-widget-maximum-values': 1,
#         'data-widget-bootstrap': 'snod-widget',
#     }
#
#     def autocomplete_html(self):
#         html = super(TransplantCoordinatorAutoComplete, self).autocomplete_html()
#         html += '<span data-value="create"><i class="glyphicon glyphicon-save"></i> Save New Person</span>'
#         return html
#
# al.register(StaffPerson, TransplantCoordinatorAutoComplete)
#
#
# class TheatreContactAutoComplete(al.AutocompleteModelBase):
#     choices = StaffPerson.objects.filter(jobs__in=[StaffJob.PERFUSION_TECHNICIAN])
#     search_fields = ['first_names', 'last_names']
#     attrs = {
#         # This will set the input placeholder attribute:
#         'placeholder': "Please type in the their name",
#         # This will set the yourlabs.Autocomplete.minimumCharacters
#         # options, the naming conversion is handled by jQuery
#         'data-autocomplete-minimum-characters': 1,
#     }
#     widget_attrs = {
#         'data-widget-maximum-values': 1,
#         'data-widget-bootstrap': 'theatre-widget',
#     }
#
#     def autocomplete_html(self):
#         html = super(TheatreContactAutoComplete, self).autocomplete_html()
#         html += '<span data-value="create"><i class="glyphicon glyphicon-save"></i> Save New Person</span>'
#         return html
#
# al.register(StaffPerson, TheatreContactAutoComplete)
