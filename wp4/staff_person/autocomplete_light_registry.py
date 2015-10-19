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

al.register(
    StaffPerson,
    name='TransplantCoordinatorAutoComplete',
    choices=StaffPerson.objects.filter(jobs__in=[StaffJob.TRANSPLANT_COORDINATOR]),
    search_fields=['first_names', 'last_names']
)

al.register(
    StaffPerson,  # TODO: Determine a new Job Title for this role?
    name='TheatreContactAutoComplete',
    choices=StaffPerson.objects.filter(jobs__in=[StaffJob.PERFUSION_TECHNICIAN]),
    search_fields=['first_names', 'last_names']
)
