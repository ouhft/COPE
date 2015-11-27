#!/usr/bin/python
# coding: utf-8

from django import forms
from django.views.generic import FormView
from django.views.generic import ListView, CreateView, UpdateView, DetailView

from braces.views import LoginRequiredMixin

from ..theme.layout import AjaxReturnIDMixin
from .models import StaffPerson
from .forms import StaffPersonForm


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
            context = self.get_context_data()
            context['form_saved'] = True
        return super(AjaxFormMixin, self).form_valid(form)

    def get_form(self, form_class=None):
        form = super(AjaxFormMixin, self).get_form(form_class)
        if self.request.is_ajax():
            form.fields['user'].widget = forms.HiddenInput()
            form.fields['jobs'].widget = forms.HiddenInput()
        return form

    def get_template_names(self):
        # print("DEBUG: get_template_names():is_ajax? %s" % self.request.is_ajax())
        if self.request.is_ajax():
            self.template_name = "staff_person/staffperson_form.ajax.html"
        return super(AjaxFormMixin, self).get_template_names()


# ============================================  CBVs
class StaffPersonListView(AjaxReturnIDMixin, AjaxListSearchMixin, LoginRequiredMixin, ListView):
    model = StaffPerson


class StaffPersonDetailView(LoginRequiredMixin, DetailView):
    model = StaffPerson


class StaffPersonCreateView(AjaxReturnIDMixin, AjaxFormMixin, LoginRequiredMixin, CreateView):
    model = StaffPerson


class StaffPersonUpdateView(AjaxReturnIDMixin, AjaxFormMixin, LoginRequiredMixin, UpdateView):
    model = StaffPerson

