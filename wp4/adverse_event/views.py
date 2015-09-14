#!/usr/bin/python
# coding: utf-8

from django.contrib.auth.decorators import login_required
from django.http import Http404
from django.shortcuts import get_object_or_404, render, render_to_response
from django.template import RequestContext
from django.views.generic import ListView, CreateView, UpdateView, DetailView

from ..compare.models import Organ
from .models import AdverseEvent
from .forms import AdverseEventForm


@login_required
def adverse_events_list(request):
    events = AdverseEvent.objects.all()
    organs = Organ.objects.exclude(transplantable=False)

    return render_to_response(
        "dashboard/adverseevents-list.html",
        {
            "events": events,
            "organs": organs
        },
        context_instance=RequestContext(request)
    )


@login_required
def adverse_event_form_new(request):
    raise Http404("This is a page holder")  # This message is only for debug view


@login_required
def adverse_event_form(request):
    raise Http404("This is a page holder")  # This message is only for debug view
