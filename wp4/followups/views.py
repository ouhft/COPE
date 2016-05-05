#!/usr/bin/python
# coding: utf-8
from django.contrib import messages
from django.contrib.auth.decorators import login_required, permission_required
from django.shortcuts import get_object_or_404, render, render_to_response
from django.views.generic import ListView, CreateView, UpdateView, DetailView

from braces.views import LoginRequiredMixin, PermissionRequiredMixin

from .models import FollowUpInitial, FollowUp3M, FollowUp6M, FollowUp1Y
from .forms import FollowUpInitialForm, FollowUp3MForm, FollowUp6MForm, FollowUp1YForm


@permission_required('followups.add_followupinitial')
@login_required
def index(request):
    return render(request, 'followups/index.html', {})


# ============================================  CBVs
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


# Initial
class FollowUpInitialListView(LoginRequiredMixin, PermissionRequiredMixin, ListView):
    model = FollowUpInitial
    permission_required = "followups.add_followupinitial"


class FollowUpInitialDetailView(LoginRequiredMixin, PermissionRequiredMixin, DetailView):
    model = FollowUpInitial
    permission_required = "followups.add_followupinitial"


class FollowUpInitialCreateView(LoginRequiredMixin, PermissionRequiredMixin, FormSaveMixin, CreateView):
    model = FollowUpInitial
    form_class = FollowUpInitialForm
    permission_required = "followups.add_followupinitial"


class FollowUpInitialUpdateView(LoginRequiredMixin, PermissionRequiredMixin, FormSaveMixin, UpdateView):
    model = FollowUpInitial
    form_class = FollowUpInitialForm
    permission_required = "followups.add_followupinitial"


# 3 Months
class FollowUp3MListView(LoginRequiredMixin, PermissionRequiredMixin, ListView):
    model = FollowUp3M
    permission_required = "followups.add_followup3m"


class FollowUp3MDetailView(LoginRequiredMixin, PermissionRequiredMixin, DetailView):
    model = FollowUp3M
    permission_required = "followups.add_followup3m"


class FollowUp3MCreateView(LoginRequiredMixin, PermissionRequiredMixin, FormSaveMixin, CreateView):
    model = FollowUp3M
    form_class = FollowUp3MForm
    permission_required = "followups.add_followup3m"


class FollowUp3MUpdateView(LoginRequiredMixin, PermissionRequiredMixin, FormSaveMixin, UpdateView):
    model = FollowUp3M
    form_class = FollowUp3MForm
    permission_required = "followups.add_followup3m"


# 6 Months
class FollowUp6MListView(LoginRequiredMixin, PermissionRequiredMixin, ListView):
    model = FollowUp6M
    permission_required = "followups.add_followup6m"


class FollowUp6MDetailView(LoginRequiredMixin, PermissionRequiredMixin, DetailView):
    model = FollowUp6M
    permission_required = "followups.add_followup6m"


class FollowUp6MCreateView(LoginRequiredMixin, PermissionRequiredMixin, FormSaveMixin, CreateView):
    model = FollowUp6M
    form_class = FollowUp6MForm
    permission_required = "followups.add_followup6m"


class FollowUp6MUpdateView(LoginRequiredMixin, PermissionRequiredMixin, FormSaveMixin, UpdateView):
    model = FollowUp6M
    form_class = FollowUp6MForm
    permission_required = "followups.add_followup6m"


# 1 Year
class FollowUp1YListView(LoginRequiredMixin, PermissionRequiredMixin, ListView):
    model = FollowUp1Y
    permission_required = "followups.add_followup1y"


class FollowUp1YDetailView(LoginRequiredMixin, PermissionRequiredMixin, DetailView):
    model = FollowUp1Y
    permission_required = "followups.add_followup1y"


class FollowUp1YCreateView(LoginRequiredMixin, PermissionRequiredMixin, FormSaveMixin, CreateView):
    model = FollowUp1Y
    form_class = FollowUp1YForm
    permission_required = "followups.add_followup1y"


class FollowUp1YUpdateView(LoginRequiredMixin, PermissionRequiredMixin, FormSaveMixin, UpdateView):
    model = FollowUp1Y
    form_class = FollowUp1YForm
    permission_required = "followups.add_followup1y"
