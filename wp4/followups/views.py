#!/usr/bin/python
# coding: utf-8
from django.conf import settings
from django.contrib import messages
from django.views.generic import ListView, CreateView, UpdateView, DetailView

from braces.views import LoginRequiredMixin

from .models import FollowUpInitial, FollowUp3M, FollowUp6M, FollowUp1Y
from .forms import FollowUpInitialForm, FollowUp3MForm, FollowUp6MForm


class FormSaveMixin(object):
    def form_valid(self, form):
        messages.success(
            self.request,
            'Form was <strong>saved SUCCESSFULLY</strong>, please review it below'
        )
        form.instance.created_by = self.request.user
        return super(FormSaveMixin, self).form_valid(form)

    def form_invalid(self, form):
        print("DEBUG: form_invalid() errors: %s" % form.errors)
        error_count = len(form.errors)
        error_pluralise = "" if error_count == 1 else "s"
        messages.error(
            self.request,
            '<strong>Form was NOT saved</strong>, please correct the %d error%s below' %
            (error_count, error_pluralise)
        )
        return super(FormSaveMixin, self).form_invalid(form)


# ============================================  CBVs
# Initial
class FollowUpInitialListView(LoginRequiredMixin, ListView):
    model = FollowUpInitial


class FollowUpInitialDetailView(LoginRequiredMixin, DetailView):
    model = FollowUpInitial


class FollowUpInitialCreateView(LoginRequiredMixin, FormSaveMixin, CreateView):
    model = FollowUpInitial
    form_class = FollowUpInitialForm


class FollowUpInitialUpdateView(LoginRequiredMixin, FormSaveMixin, UpdateView):
    model = FollowUpInitial
    form_class = FollowUpInitialForm


# 3 Months
class FollowUp3MListView(LoginRequiredMixin, ListView):
    model = FollowUp3M


class FollowUp3MDetailView(LoginRequiredMixin, DetailView):
    model = FollowUp3M


class FollowUp3MCreateView(LoginRequiredMixin, FormSaveMixin, CreateView):
    model = FollowUp3M
    form_class = FollowUp3MForm


class FollowUp3MUpdateView(LoginRequiredMixin, FormSaveMixin, UpdateView):
    model = FollowUp3M
    form_class = FollowUp3MForm


# 6 Months
class FollowUp6MListView(LoginRequiredMixin, ListView):
    model = FollowUp6M


class FollowUp6MDetailView(LoginRequiredMixin, DetailView):
    model = FollowUp6M


class FollowUp6MCreateView(LoginRequiredMixin, FormSaveMixin, CreateView):
    model = FollowUp6M
    form_class = FollowUp6MForm


class FollowUp6MUpdateView(LoginRequiredMixin, FormSaveMixin, UpdateView):
    model = FollowUp6M
    form_class = FollowUp6MForm


# 1 Year
class FollowUp1YListView(LoginRequiredMixin, ListView):
    model = FollowUp1Y


class FollowUp1YDetailView(LoginRequiredMixin, DetailView):
    model = FollowUp1Y


class FollowUp1YCreateView(LoginRequiredMixin, FormSaveMixin, CreateView):
    model = FollowUp1Y
    form_class = FollowUpInitialForm


class FollowUp1YUpdateView(LoginRequiredMixin, FormSaveMixin, UpdateView):
    model = FollowUp1Y
    form_class = FollowUpInitialForm
