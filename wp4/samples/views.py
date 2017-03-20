#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib import messages
from django.contrib.auth.decorators import login_required
from django.core.exceptions import PermissionDenied
from django.db.models import Q
from django.db import transaction
from django.shortcuts import render
from django.views.generic import ListView, UpdateView, DetailView

from braces.views import LoginRequiredMixin, MultiplePermissionsRequiredMixin, OrderableListMixin

from wp4.compare.models.donor import Donor, Organ
from wp4.compare.models.transplantation import Recipient

from .models import Event
from .forms import EventForm, BloodSampleFormSet, UrineSampleFormSet, PerfusateSampleFormSet, TissueSampleFormSet


# ============================================  Mixins

class UserBasedQuerysetMixin(object):
    """
    Ensure that the queries we run include the current user to allow for the filtering and permissions to take hold
    """

    def get_queryset(self):
        queryset = self.model.objects.for_user(self.request.user).all()

        try:
            ordering = self.get_ordering()
            if ordering:
                if isinstance(ordering, six.string_types):
                    ordering = (ordering,)
                queryset = queryset.order_by(*ordering)
        except AttributeError:
            # get_ordering doesn't apply to nonlists, but this queryset does
            pass

        return queryset

class FormSaveMixin(object):
    def form_valid(self, form):
        messages.success(
            self.request,
            'Form was <strong>saved SUCCESSFULLY</strong>, please review it below'
        )
        return super(FormSaveMixin, self).form_valid(form)

    def form_invalid(self, form):
        # print("DEBUG: form_invalid() errors: %s" % form.errors)
        error_count = len(form.errors)
        error_pluralise = "" if error_count == 1 else "s"
        if error_count > 0:
            messages.error(
                self.request,
                '<strong>Form was NOT saved</strong>, please correct the %d error%s below' %
                (error_count, error_pluralise)
            )
        return super(FormSaveMixin, self).form_invalid(form)


# ============================================  CBVs
class EventListView(LoginRequiredMixin, MultiplePermissionsRequiredMixin, OrderableListMixin, ListView):
    model = Event
    permissions = {
        "all": (),
        "any": ("samples.change_event", "samples.view_event"),
    }

    paginate_by = 100
    paginate_orphans = 5
    orderable_columns = ("id", "type", "name",)
    orderable_columns_default = "id"


class EventDetailView(LoginRequiredMixin, MultiplePermissionsRequiredMixin, DetailView):
    model = Event
    permissions = {
        "all": (),
        "any": ("samples.change_event", "samples.view_event"),
    }


class EventUpdateView(LoginRequiredMixin, MultiplePermissionsRequiredMixin, FormSaveMixin, UpdateView):
    model = Event
    form_class = EventForm
    permissions = {
        "all": ("samples.change_event", ),
        "any": (),
    }

    def get_formsets(self):
        return {
            'bloodsamples': BloodSampleFormSet(self.request.POST or None, prefix='bloodsamples', instance=self.object),
            'urinesamples': UrineSampleFormSet(self.request.POST or None, prefix='urinesamples', instance=self.object),
            'perfusatesamples': PerfusateSampleFormSet(self.request.POST or None, prefix='perfusatesamples', instance=self.object),
            'tissuesamples': TissueSampleFormSet(self.request.POST or None, prefix='tissuesamples', instance=self.object),
        }

    def get_context_data(self, **kwargs):
        context = super(EventUpdateView, self).get_context_data(**kwargs)
        context['formsets'] = self.get_formsets()
        return context

    def form_valid(self, form):
        context = self.get_context_data()
        formsets = context['formsets']

        with transaction.atomic():
            self.object = form.save()

            for key, formset in formsets.items():
                if formset.is_valid():
                    formset.instance = self.object
                    formset.save()
                else:
                    print("DEBUG: Formset invalid. errors={0}".format(formset.errors))
                    error_count = len(formset.errors)
                    error_pluralise = "" if error_count == 1 else "s"
                    messages.error(
                        self.request,
                        '<strong>Formset was NOT saved</strong>, please correct the %d error%s below' %
                        (error_count, error_pluralise)
                    )
                    return super(EventUpdateView, self).form_invalid(form)
        return super(EventUpdateView, self).form_valid(form)


# ------------------------------------------------
class DonorSamplesListView(LoginRequiredMixin, MultiplePermissionsRequiredMixin,
                           UserBasedQuerysetMixin, OrderableListMixin, ListView):
    model = Donor
    template_name = 'samples/donor_list.html'  # Rather than compare/donor_list
    permissions = {
        "all": (),
        "any": ("samples.change_event", "samples.view_event"),
    }

    paginate_by = 25
    paginate_orphans = 5
    orderable_columns = ("id", "trial_id", )
    orderable_columns_default = "id"


class DonorSamplesDetailView(LoginRequiredMixin, MultiplePermissionsRequiredMixin,
                             UserBasedQuerysetMixin, DetailView):
    model = Donor
    template_name = 'samples/donor_detail.html'  # Rather than compare/donor_detail
    permissions = {
        "all": (),
        "any": ("samples.change_event", "samples.view_event"),
    }


# ------------------------------------------------
class OrganSamplesListView(LoginRequiredMixin, MultiplePermissionsRequiredMixin,
                           UserBasedQuerysetMixin, OrderableListMixin, ListView):
    model = Organ
    template_name = 'samples/organ_list.html'  # Rather than compare/organ_list
    permissions = {
        "all": (),
        "any": ("samples.change_event", "samples.view_event"),
    }

    paginate_by = 25
    paginate_orphans = 5
    orderable_columns = ("id", "trial_id", )
    orderable_columns_default = "id"


class OrganSamplesDetailView(LoginRequiredMixin, MultiplePermissionsRequiredMixin,
                             UserBasedQuerysetMixin, DetailView):
    model = Organ
    template_name = 'samples/organ_detail.html'  # Rather than compare/organ_detail
    permissions = {
        "all": (),
        "any": ("samples.change_event", "samples.view_event"),
    }


# ------------------------------------------------
class RecipientSamplesListView(LoginRequiredMixin, MultiplePermissionsRequiredMixin,
                           UserBasedQuerysetMixin, OrderableListMixin, ListView):
    model = Recipient
    template_name = 'samples/recipient_list.html'  # Rather than compare/recipient_list
    permissions = {
        "all": (),
        "any": ("samples.change_event", "samples.view_event"),
    }

    paginate_by = 25
    paginate_orphans = 5
    orderable_columns = ("id", "trial_id", )
    orderable_columns_default = "id"


class RecipientSamplesDetailView(LoginRequiredMixin, MultiplePermissionsRequiredMixin,
                             UserBasedQuerysetMixin, DetailView):
    model = Recipient
    template_name = 'samples/recipient_detail.html'  # Rather than compare/recipient_detail
    permissions = {
        "all": (),
        "any": ("samples.change_event", "samples.view_event"),
    }


# ============================================  FBVs

@login_required
def index(request):
    current_person = request.user

    if not current_person.has_perm('samples.change_event') and not current_person.has_perm('samples.view_event'):
        raise PermissionDenied

    return render(request, 'samples/index.html', {})


@login_required
def sample_form(request, pk=None):
    pass
    # """
    # Process the sample form and formsets
    # :param request:
    # :param pk: worksheet ID
    # :return: Page response
    # """
    # worksheet = Worksheet.objects.get(pk=pk)
    # current_person = request.user
    #
    # worksheet_form = WorksheetForm(request.POST or None, instance=worksheet, prefix="worksheet")
    # if worksheet_form.is_valid():
    #     worksheet_form.instance.created_by = current_person.user
    #     worksheet = worksheet_form.save()
    #
    # events = []
    # for i, event in enumerate(worksheet.event_set.all()):
    #     event_prefix = "event_%d" % i
    #     event_form = EventForm(request.POST or None, instance=event, prefix=event_prefix)
    #     if event_form.is_valid():
    #         event_form.instance.created_by = current_person
    #         event = event_form.save()
    #
    #     if event.type == Event.TYPE_BLOOD:
    #         event_formset = BloodSampleFormSet(
    #             request.POST or None,
    #             instance=event,
    #             prefix="blood_%d" % i
    #         )
    #     elif event.type == Event.TYPE_URINE:
    #         event_formset = UrineSampleFormSet(
    #             request.POST or None,
    #             instance=event,
    #             prefix="urine_%d" % i
    #         )
    #     elif event.type == Event.TYPE_PERFUSATE:
    #         event_formset = PerfusateSampleFormSet(
    #             request.POST or None,
    #             instance=event,
    #             prefix="perfusate_%d" % i
    #         )
    #     elif event.type == Event.TYPE_TISSUE:
    #         event_formset = TissueSampleFormSet(
    #             request.POST or None,
    #             instance=event,
    #             prefix="tissue_%d" % i
    #         )
    #
    #     if event_formset.is_valid():
    #         for subform in event_formset:
    #             subform.instance.created_by = current_person
    #         event_formset.save()
    #     # print("DEBUG: event_formset is valid (%s) and errors=%s" % (event_formset.is_valid(), event_formset.errors))
    #
    #     events.append({"form": event_form, "formset": event_formset})
    #
    # return render(
    #     request=request,
    #     template_name="samples/sample_form.html",
    #     context={
    #         "worksheet_form": worksheet_form,
    #         "events": events
    #     }
    # )
