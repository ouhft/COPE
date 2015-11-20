#!/usr/bin/python
# coding: utf-8

from django.contrib.auth.decorators import login_required
from django.core.urlresolvers import reverse
from django.http import Http404
from django.shortcuts import get_object_or_404, render, render_to_response, redirect
from django.template import RequestContext
from django.views.decorators.csrf import csrf_protect
from django.views.generic import ListView, CreateView, UpdateView, DetailView

from braces.views import LoginRequiredMixin

# from .models import Event, Worksheet
# from .forms import EventForm, UrineSampleEventFormSet, BloodSampleEventFormSet, TissueSampleEventFormSet,\
#     PerfusateSampleEventFormSet
# from .forms import WorksheetForm, UrineSampleWorksheetFormSet, BloodSampleWorksheetFormSet, \
#     PerfusateSampleWorksheetFormSet, TissueSampleWorksheetFormSet
#
#
# class EventListView(LoginRequiredMixin, ListView):
#     model = Event
#
#
# class EventDetailView(LoginRequiredMixin, DetailView):
#     model = Event
#
#
# class EventCreateView(LoginRequiredMixin, CreateView):
#     model = Event
#     form_class = EventForm
#
#     def get(self, request, *args, **kwargs):
#         """
#         Handles GET requests and instantiates blank versions of the form
#         and its inline formsets.
#         """
#         self.object = None
#         form_class = self.get_form_class()
#         form = self.get_form(form_class)
#         urinesample_formset = UrineSampleEventFormSet()
#         bloodample_formset = BloodSampleEventFormSet()
#         perfusatesample_formset = PerfusateSampleEventFormSet()
#         tissuesample_formset = TissueSampleEventFormSet()
#         return self.render_to_response(
#             self.get_context_data(form=form,
#                                   urinesample_formset=urinesample_formset,
#                                   bloodample_formset=bloodample_formset,
#                                   perfusatesample_formset=perfusatesample_formset,
#                                   tissuesample_formset=tissuesample_formset))
#
#     def post(self, request, *args, **kwargs):
#         """
#         Handles POST requests, instantiating a form instance and its inline
#         formsets with the passed POST variables and then checking them for
#         validity.
#         """
#         self.object = None
#         form_class = self.get_form_class()
#         form = self.get_form(form_class)
#         urinesample_formset = UrineSampleEventFormSet(self.request.POST)
#         bloodample_formset = BloodSampleEventFormSet(self.request.POST)
#         perfusatesample_formset = PerfusateSampleEventFormSet(self.request.POST)
#         tissuesample_formset = TissueSampleEventFormSet(self.request.POST)
#         if (form.is_valid() and urinesample_formset.is_valid() and
#             bloodample_formset.is_valid() and perfusatesample_formset.is_valid() and
#             tissuesample_formset.is_valid()):
#             return self.form_valid(form, urinesample_formset, bloodample_formset,
#                                    perfusatesample_formset, tissuesample_formset)
#         else:
#             return self.form_invalid(form, urinesample_formset, bloodample_formset,
#                                      perfusatesample_formset, tissuesample_formset)
#
#     def form_valid(self, form, urinesample_formset, bloodample_formset, perfusatesample_formset, tissuesample_formset):
#         """
#         Called if all forms are valid. Creates a Recipe instance along with
#         associated Ingredients and Instructions and then redirects to a
#         success page.
#         """
#         self.object = form.save()
#         urinesample_formset.instance = self.object
#         urinesample_formset.save()
#         bloodample_formset.instance = self.object
#         bloodample_formset.save()
#         perfusatesample_formset.instance = self.object
#         perfusatesample_formset.save()
#         tissuesample_formset.instance = self.object
#         tissuesample_formset.save()
#         return redirect(reverse('samples:event_detail', kwargs={'pk': self.object.id}))
#
#     def form_invalid(self, form, urinesample_formset, bloodample_formset, perfusatesample_formset, tissuesample_formset):
#         """
#         Called if a form is invalid. Re-renders the context data with the
#         data-filled forms and errors.
#         """
#         return self.render_to_response(
#             self.get_context_data(form=form,
#                                   urinesample_formset=urinesample_formset,
#                                   bloodample_formset=bloodample_formset,
#                                   perfusatesample_formset=perfusatesample_formset,
#                                   tissuesample_formset=tissuesample_formset))
#
#
# class WorksheetListView(LoginRequiredMixin, ListView):
#     model = Worksheet
#
#
# class WorksheetDetailView(LoginRequiredMixin, DetailView):
#     model = Worksheet




@login_required
def sample_home(request):
    #TODO: Make this into a search by Trial ID page, that will then get or create the correct worksheet
    #TODO: Have a worksheet editor form similar to the one in the admin interface
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
