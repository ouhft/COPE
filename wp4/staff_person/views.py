#!/usr/bin/python
# coding: utf-8

from django import forms
from django.contrib import messages
from django.db.models import Q
from django.views.generic import ListView, CreateView, UpdateView, DetailView

from braces.views import LoginRequiredMixin
from dal import autocomplete

from wp4.theme.layout import AjaxReturnIDMixin
from .models import StaffPerson, StaffJob
from .forms import StaffPersonForm


class TechnicianAutoComplete(autocomplete.Select2QuerySetView):
    def get_queryset(self):
        if not self.request.user.is_authenticated():
            return StaffPerson.objects.none()

        qs = StaffPerson.objects.filter(jobs__in=[StaffJob.PERFUSION_TECHNICIAN])

        if self.q:
            qs = qs.filter(Q(first_names__icontains=self.q) | Q(last_names__icontains=self.q))

        return qs


# ============================================  MIXINS
class AjaxListSearchMixin(object):
    def get_queryset(self):
        # If a q value is set, filter based on jobs
        queryset = super(AjaxListSearchMixin, self).get_queryset()

        q = self.request.GET.get("q")
        if q and q.isdigit():
            return queryset.filter(jobs__id__in=[int(q)])
        return queryset

    def get_template_names(self):
        # print("DEBUG: get_template_names():is_ajax? %s" % self.request.is_ajax())
        if self.request.is_ajax():
            self.template_name = "staff_person/staffperson_list.ajax.html"
        return super(AjaxListSearchMixin, self).get_template_names()


class AjaxFormMixin(object):
    form_class = StaffPersonForm

    def form_valid(self, form):
        form.instance.created_by = self.request.user
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
            form.fields['user'].widget = forms.HiddenInput()
            form.fields['jobs'].widget = forms.HiddenInput()
            self.template_name = "staff_person/staffperson_form.ajax.html"
        return form


# ============================================  CBVs
class StaffPersonListView(AjaxReturnIDMixin, AjaxListSearchMixin, LoginRequiredMixin, ListView):
    model = StaffPerson


class StaffPersonDetailView(AjaxReturnIDMixin, LoginRequiredMixin, DetailView):
    model = StaffPerson


class StaffPersonCreateView(AjaxReturnIDMixin, AjaxFormMixin, LoginRequiredMixin, CreateView):
    model = StaffPerson


class StaffPersonUpdateView(AjaxReturnIDMixin, AjaxFormMixin, LoginRequiredMixin, UpdateView):
    model = StaffPerson
