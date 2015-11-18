#!/usr/bin/python
# coding: utf-8

from django.contrib.auth.decorators import login_required
from django.http import Http404
from django.shortcuts import get_object_or_404, render, render_to_response
from django.template import RequestContext
from django.views.decorators.csrf import csrf_protect
from django.views.generic import ListView, CreateView, UpdateView, DetailView

from braces.views import LoginRequiredMixin

from .models import Event, Worksheet
from .forms import EventForm, UrineSampleEventFormSet, BloodSampleEventFormSet, TissueSampleEventFormSet,\
    PerfusateSampleEventFormSet
from .forms import WorksheetForm, UrineSampleWorksheetFormSet, BloodSampleWorksheetFormSet, \
    PerfusateSampleWorksheetFormSet, TissueSampleWorksheetFormSet


class EventListView(LoginRequiredMixin, ListView):
    model = Event


class EventDetailView(LoginRequiredMixin, DetailView):
    model = Event


class EventCreateView(LoginRequiredMixin, CreateView):
    model = Event
    form_class = EventForm
    # fields = ('type', 'taken_at')

    def get(self, request, *args, **kwargs):
        """
        Handles GET requests and instantiates blank versions of the form
        and its inline formsets.
        """
        self.object = None
        form_class = self.get_form_class()
        form = self.get_form(form_class)
        urinesample_formset = UrineSampleEventFormSet()
        bloodample_formset = BloodSampleEventFormSet()
        perfusatesample_formset = PerfusateSampleEventFormSet()
        tissuesample_formset = TissueSampleEventFormSet()
        return self.render_to_response(
            self.get_context_data(form=form,
                                  urinesample_formset=urinesample_formset,
                                  bloodample_formset=bloodample_formset,
                                  perfusatesample_formset=perfusatesample_formset,
                                  tissuesample_formset=tissuesample_formset))


class WorksheetListView(LoginRequiredMixin, ListView):
    model = Worksheet


class WorksheetDetailView(LoginRequiredMixin, DetailView):
    model = Worksheet




@login_required
def sample_home(request):
    return render_to_response(
        "samples/home.html",
        context_instance=RequestContext(request)
    )

@login_required
@csrf_protect
# @ajax
def sample_editor(request, pk=None, type=None):
    # valid_types = [t[0] for t in Sample.TYPE_CHOICES]
    # if pk is not None:
    #     sample = get_object_or_404(Sample, pk=int(pk))
    # elif type is not None and int(type) in valid_types:
    #     sample = Sample(type=type)
    # else:
    #     raise Http404("This is a page isn't happy")
    #
    # sample_form = SampleForm(request.POST or None, request.FILES or None, instance=sample, prefix="sample")
    # if sample_form.is_valid():
    #     sample = sample_form.save(request.user)

    return render_to_response(
        "samples/sample-form.html",
        {"sample_form": sample_form},
        context_instance=RequestContext(request)
    )
