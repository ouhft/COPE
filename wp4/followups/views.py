#!/usr/bin/python
# coding: utf-8
from django.contrib import messages
from django.contrib.auth.decorators import login_required, permission_required
from django.core.urlresolvers import reverse
from django.http import HttpResponseRedirect
from django.shortcuts import get_object_or_404, render, render_to_response
from django.views.generic import ListView, CreateView, UpdateView, DetailView
from django.views.generic.edit import ModelFormMixin

from braces.views import LoginRequiredMixin, PermissionRequiredMixin

from wp4.health_economics.models import QualityOfLife

from .models import FollowUpInitial, FollowUp3M, FollowUp6M, FollowUp1Y
from .forms import FollowUpInitialForm, FollowUp3MForm, FollowUp6MForm, FollowUp1YForm
from .forms import FollowUpInitialStartForm, FollowUp3MStartForm, FollowUp6MStartForm, FollowUp1YStartForm


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


class FormStartMixin(ModelFormMixin):
    object = None

    def get_context_data(self, **kwargs):
        context = super(FormStartMixin, self).get_context_data(**kwargs)
        context['form'] = self.get_form()
        return context

    def post(self, request, *args, **kwargs):
        self.object = None
        form = self.get_form()
        if form.is_valid():
            return self.form_valid(form)
        else:
            return self.form_invalid(form)


class FormSaveAndCreateQOLMixin(FormSaveMixin):
    def form_valid(self, form):
        qol_object = QualityOfLife()
        qol_object.recipient = form.instance.organ.safe_recipient
        qol_object.save(created_by=self.request.user)

        form.instance.created_by = self.request.user
        form.instance.quality_of_life = qol_object
        self.object = form.save()

        messages.success(
            self.request,
            'Form was <strong>saved SUCCESSFULLY</strong>, please review it below'
        )
        return HttpResponseRedirect(self.get_success_url())


# Initial
class FollowUpInitialListView(LoginRequiredMixin, PermissionRequiredMixin, ListView, FormSaveMixin, FormStartMixin):
    model = FollowUpInitial
    permission_required = "followups.add_followupinitial"
    form_class = FollowUpInitialStartForm

    def get_success_url(self):
        return reverse('wp4:followup:initial_update', kwargs={'pk': self.object.pk})


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
class FollowUp3MListView(LoginRequiredMixin, PermissionRequiredMixin, ListView, FormSaveAndCreateQOLMixin, FormStartMixin):
    model = FollowUp3M
    permission_required = "followups.add_followup3m"
    form_class = FollowUp3MStartForm

    def get_success_url(self):
        return reverse('wp4:followup:month3_update', kwargs={'pk': self.object.pk})


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
class FollowUp6MListView(LoginRequiredMixin, PermissionRequiredMixin, ListView, FormSaveMixin, FormStartMixin):
    model = FollowUp6M
    permission_required = "followups.add_followup6m"
    form_class = FollowUp6MStartForm

    def get_success_url(self):
        return reverse('wp4:followup:month6_update', kwargs={'pk': self.object.pk})


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
class FollowUp1YListView(LoginRequiredMixin, PermissionRequiredMixin, ListView, FormSaveAndCreateQOLMixin, FormStartMixin):
    model = FollowUp1Y
    permission_required = "followups.add_followup1y"
    form_class = FollowUp1YStartForm

    def get_success_url(self):
        return reverse('wp4:followup:final_update', kwargs={'pk': self.object.pk})


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
