#!/usr/bin/python
# coding: utf-8
from itertools import chain

from django.contrib.auth.decorators import login_required
from django.core.urlresolvers import reverse
from django.shortcuts import get_object_or_404, render, render_to_response, redirect
from django.template import RequestContext

from wp4.staff_person.models import StaffPerson, StaffJob

from .models import Worksheet, Event
from .forms import WorksheetForm, EventForm, BloodSampleFormSet, UrineSampleFormSet, PerfusateSampleFormSet, TissueSampleFormSet


@login_required
def sample_home(request):
    #TODO: Make this into a search by Trial ID page, that will then get or create the correct worksheet
    #TODO: Have a worksheet editor form similar to the one in the admin interface

    current_person = StaffPerson.objects.get(user__id=request.user.id)

    if current_person.has_job(
            (StaffJob.SYSTEMS_ADMINISTRATOR, StaffJob.CENTRAL_COORDINATOR, StaffJob.NATIONAL_COORDINATOR)
    ):
        current_worksheets = Worksheet.objects.all()
    elif current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
        donor_worksheets = Worksheet.objects\
            .filter(person__donor__perfusion_technician=current_person)
        recipient_worksheets = Worksheet.objects\
            .filter(person__recipient__allocation__perfusion_technician=current_person)
        current_worksheets = list(chain(donor_worksheets, recipient_worksheets))
        # current_worksheets = current_worksheets.order_by('person')
    else:
        current_worksheets = {}

    return render_to_response(
        "samples/home.html",
        {
            "current_worksheets": current_worksheets,
        },
        context_instance=RequestContext(request)
    )


@login_required
def sample_form(request, pk=None):
    """
    Process the sample form and formsets
    :param request:
    :param pk: worksheet ID
    :return: Page response
    """
    worksheet = get_object_or_404(Worksheet, pk=int(pk))
    current_person = StaffPerson.objects.get(user__id=request.user.id)

    worksheet_form = WorksheetForm(request.POST or None, instance=worksheet, prefix="worksheet")
    if worksheet_form.is_valid():
        worksheet_form.instance.created_by = current_person.user
        worksheet = worksheet_form.save()

    events = []
    for i, event in enumerate(worksheet.event_set.all()):
        event_prefix = "event_%d" % i
        # print("DEBUG: event1 id=%s" % event.id)
        event_form = EventForm(request.POST or None, instance=event, prefix=event_prefix)
        if event_form.is_valid():
            event_form.instance.created_by = current_person.user
            event = event_form.save()

        # print("DEBUG: event_form: %s" % event_form)
        # print("DEBUG: event2 id=%s" % event.id)

        if event.type == Event.TYPE_BLOOD:
            event_formset = BloodSampleFormSet(request.POST or None, instance=event, prefix="blood_%d" % i)
        elif event.type == Event.TYPE_URINE:
            event_formset = UrineSampleFormSet(request.POST or None, instance=event, prefix="urine_%d" % i)
        elif event.type == Event.TYPE_PERFUSATE:
            event_formset = PerfusateSampleFormSet(request.POST or None, instance=event, prefix="perfusate_%d" % i)
        elif event.type == Event.TYPE_TISSUE:
            event_formset = TissueSampleFormSet(request.POST or None, instance=event, prefix="tissue_%d" % i)

        # print("DEBUG: event_formset: %s" % event_formset)
        # print("DEBUG: event3 id=%s" % event.id)
        if event_formset.is_valid():
            for subform in event_formset:
                subform.instance.created_by = current_person.user
            event_formset.save()
        # print("DEBUG: event_formset is valid (%s) and errors=%s" % (event_formset.is_valid(), event_formset.errors))

        events.append({"form": event_form, "formset": event_formset})

    return render_to_response(
        "samples/sample_form.html",
        {
            "worksheet_form": worksheet_form,
            "events": events
        },
        context_instance=RequestContext(request)
    )
