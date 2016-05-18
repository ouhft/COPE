#!/usr/bin/python
# coding: utf-8
from itertools import chain

from django.contrib.auth.decorators import login_required
from django.db.models import Q
from django.shortcuts import get_object_or_404, render_to_response
from django.template import RequestContext
from django.views.generic import ListView, CreateView, UpdateView, DetailView

from braces.views import LoginRequiredMixin, OrderableListMixin

from wp4.staff_person.models import StaffPerson, StaffJob

from .models import Worksheet, Event
from .forms import WorksheetForm, EventForm, BloodSampleFormSet, UrineSampleFormSet, PerfusateSampleFormSet, TissueSampleFormSet


# ============================================  CBVs
class WorksheetListView(LoginRequiredMixin, OrderableListMixin, ListView):
    model = Worksheet
    current_person = None
    paginate_by = 20
    paginate_orphans = 5

    def get_orderable_columns(self):
        # return an iterable
        return (u"id", u"barcode", u"person__number", u"person__date_of_birth", u"person__gender")

    def get_orderable_columns_default(self):
        # return a string
        return u"id"

    def get(self, request, *args, **kwargs):
        self.current_person = StaffPerson.objects.get(user__id=request.user.id)
        return super(WorksheetListView, self).get(request, *args, **kwargs)

    def get_queryset(self):
        if self.current_person.has_job(
            (StaffJob.SYSTEMS_ADMINISTRATOR, StaffJob.CENTRAL_COORDINATOR, StaffJob.NATIONAL_COORDINATOR)
        ):
            self.queryset = Worksheet.objects.all().\
                select_related('person').\
                select_related('person__donor').\
                select_related('person__donor__retrieval_team__based_at').\
                select_related('person__donor__randomisation').\
                select_related('person__recipient')

        elif self.current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
            self.queryset = Worksheet.objects.filter(
                Q(person__donor__perfusion_technician=self.current_person) |
                Q(person__recipient__allocation__perfusion_technician=self.current_person)
            )

        return super(WorksheetListView, self).get_queryset()


# ============================================  FBVs
@login_required
def sample_form(request, pk=None):
    """
    Process the sample form and formsets
    :param request:
    :param pk: worksheet ID
    :return: Page response
    """
    worksheet = Worksheet.objects.\
        select_related('person__donor').\
        prefetch_related('event_set').\
        filter(pk=int(pk))[0]  # TODO: Clean this up, as it's not very smart!
    current_person = StaffPerson.objects.get(user__id=request.user.id)

    worksheet_form = WorksheetForm(request.POST or None, instance=worksheet, prefix="worksheet")
    if worksheet_form.is_valid():
        worksheet_form.instance.created_by = current_person.user
        worksheet = worksheet_form.save()

    events = []
    for i, event in enumerate(worksheet.event_set.all()):
        event_prefix = "event_%d" % i
        event_form = EventForm(request.POST or None, instance=event, prefix=event_prefix)
        if event_form.is_valid():
            event_form.instance.created_by = current_person.user
            event = event_form.save()

        if event.type == Event.TYPE_BLOOD:
            event_formset = BloodSampleFormSet(
                request.POST or None,
                instance=event,
                prefix="blood_%d" % i
            )
        elif event.type == Event.TYPE_URINE:
            event_formset = UrineSampleFormSet(
                request.POST or None,
                instance=event,
                prefix="urine_%d" % i
            )
        elif event.type == Event.TYPE_PERFUSATE:
            event_formset = PerfusateSampleFormSet(
                request.POST or None,
                instance=event,
                prefix="perfusate_%d" % i
            )
        elif event.type == Event.TYPE_TISSUE:
            event_formset = TissueSampleFormSet(
                request.POST or None,
                instance=event,
                prefix="tissue_%d" % i
            )

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
