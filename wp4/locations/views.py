#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib import messages
from django.db.models import Q
from django.views.generic import ListView, CreateView, UpdateView, DetailView

from braces.views import LoginRequiredMixin
from dal import autocomplete

from ..theme.layout import AjaxReturnIDMixin
from .models import Hospital
from .forms import HospitalForm


class HospitalAutoComplete(autocomplete.Select2QuerySetView):
    def get_queryset(self):
        if not self.request.user.is_authenticated:
            return Hospital.objects.none()

        qs = Hospital.objects.all()

        if self.q:
            qs = qs.filter(Q(name__icontains=self.q) | Q(country__icontains=self.q))

        return qs


# ============================================  MIXINS
class AjaxListSearchMixin(object):
    def get_template_names(self):
        # print("DEBUG: get_template_names():is_ajax? %s" % self.request.is_ajax())
        if self.request.is_ajax():
            self.template_name = "locations/hospital_list.ajax.html"
        return super(AjaxListSearchMixin, self).get_template_names()


class AjaxFormMixin(object):
    form_class = HospitalForm

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
            self.template_name = "locations/hospital_form.ajax.html"
        return form


# ============================================  CBVs
class HospitalListView(AjaxReturnIDMixin, AjaxListSearchMixin, LoginRequiredMixin, ListView):
    model = Hospital


class HospitalDetailView(AjaxReturnIDMixin, LoginRequiredMixin, DetailView):
    model = Hospital


class HospitalCreateView(AjaxReturnIDMixin, AjaxFormMixin, LoginRequiredMixin, CreateView):
    model = Hospital


class HospitalUpdateView(AjaxReturnIDMixin, AjaxFormMixin, LoginRequiredMixin, UpdateView):
    model = Hospital
