#!/usr/bin/python
# coding: utf-8

from django.views.generic import ListView, CreateView, UpdateView, DetailView

from braces.views import LoginRequiredMixin

from .models import StaffPerson
from .forms import StaffPersonForm


class StaffPersonListView(LoginRequiredMixin, ListView):
    model = StaffPerson


class StaffPersonDetailView(LoginRequiredMixin, DetailView):
    model = StaffPerson


class StaffPersonCreateView(LoginRequiredMixin, CreateView):
    model = StaffPerson
    form_class = StaffPersonForm

    def form_valid(self, form):
        form.instance.created_by = self.request.user
        return super(StaffPersonCreateView, self).form_valid(form)

    def get_template_names(self):
        print("DEBUG: get_template_names():is_ajax? %s" % self.request.is_ajax())
        if self.request.is_ajax():
            self.template_name = "staff_person/staffperson_form.ajax.html"
        return super(StaffPersonCreateView, self).get_template_names()


class StaffPersonUpdateView(LoginRequiredMixin, UpdateView):
    model = StaffPerson
    form_class = StaffPersonForm

    def form_valid(self, form):
        form.instance.created_by = self.request.user
        return super(StaffPersonUpdateView, self).form_valid(form)

