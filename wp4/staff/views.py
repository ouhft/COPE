#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib import messages
from django.db.models import Q
from django.views.generic import ListView, CreateView, UpdateView, DetailView

from braces.views import LoginRequiredMixin
from dal import autocomplete

from wp4.theme.layout import AjaxReturnIDMixin
from .models import Person
from .forms import PersonForm


class TechnicianAutoComplete(autocomplete.Select2QuerySetView):
    def get_queryset(self):
        if not self.request.user.is_authenticated():
            return Person.objects.none()

        qs = Person.objects.filter(groups__in=[Person.PERFUSION_TECHNICIAN])

        if self.q:
            qs = qs.filter(Q(first_names__icontains=self.q) | Q(last_names__icontains=self.q))

        return qs


# ============================================  MIXINS
class AjaxListSearchMixin(object):
    def get_queryset(self):
        # If a q value is set, filter based on jobs/groups
        queryset = super(AjaxListSearchMixin, self).get_queryset().order_by('first_name')

        q = self.request.GET.get("q")
        if q and q.isdigit():
            return queryset.filter(groups__id__in=[int(q)])  # TODO: Test this after the 0.8.0 changes
        return queryset

    def get_template_names(self):
        # print("DEBUG: get_template_names():is_ajax? %s" % self.request.is_ajax())
        if self.request.is_ajax():
            self.template_name = "staff/person_list.ajax.html"
        return super(AjaxListSearchMixin, self).get_template_names()


class AjaxFormMixin(object):
    form_class = PersonForm

    def form_valid(self, form):
        if self.request.is_ajax():
            self.object = form.save()
            return self.render_to_response(self.get_context_data(form=form))
        else:
            return super(AjaxFormMixin, self).form_valid(form)

    def form_invalid(self, form):
        print("DEBUG: form_invalid() errors: %s" % form.errors)
        error_count = len(form.errors)
        error_pluralise = "" if error_count == 1 else "s"
        messages.error(
            self.request,
            '<strong>Form was NOT saved</strong>, please correct the %d error%s below' %
            (error_count, error_pluralise)
        )
        return super(AjaxFormMixin, self).form_invalid(form)

    def get_form(self, form_class=None):
        form = super(AjaxFormMixin, self).get_form(form_class)
        # Both post() and get() call get_form() first, so this is best place to intercept ajax changes
        if self.request.is_ajax():
            # form.fields['groups'].widget = forms.HiddenInput()
            self.template_name = "staff/person_form.ajax.html"
        return form


# ============================================  CBVs
class PersonListView(AjaxReturnIDMixin, AjaxListSearchMixin, LoginRequiredMixin, ListView):
    model = Person


class PersonDetailView(AjaxReturnIDMixin, LoginRequiredMixin, DetailView):
    model = Person


class PersonCreateView(AjaxReturnIDMixin, AjaxFormMixin, LoginRequiredMixin, CreateView):
    model = Person


class PersonUpdateView(AjaxReturnIDMixin, AjaxFormMixin, LoginRequiredMixin, UpdateView):
    model = Person
