#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib import messages
from django.contrib.auth.decorators import login_required, permission_required
from django.shortcuts import render
from django.utils import six
from django.views.generic import ListView, UpdateView, DetailView

from braces.views import LoginRequiredMixin, MultiplePermissionsRequiredMixin, OrderableListMixin

from .models import FollowUpInitial, FollowUp3M, FollowUp6M, FollowUp1Y
from .forms import FollowUpInitialForm, FollowUp3MForm, FollowUp6MForm, FollowUp1YForm


@permission_required('followups.change_followupinitial')
@login_required
def index(request):
    return render(request, 'followups/index.html', {})


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
        messages.error(
            self.request,
            '<strong>Form was NOT saved</strong>, please correct the %d error%s below' %
            (error_count, error_pluralise)
        )
        return super(FormSaveMixin, self).form_invalid(form)


# ============================================  CBVs
# Initial
class FollowUpInitialListView(LoginRequiredMixin, MultiplePermissionsRequiredMixin, OrderableListMixin,
                              UserBasedQuerysetMixin, ListView):
    model = FollowUpInitial
    permissions = {
        "all": (),
        "any": ("followups.change_followupinitial", "followups.view_followupinitial"),
    }
    paginate_by = 100
    orderable_columns = ("organ__trial_id", "start_date",)
    orderable_columns_default = "organ__trial_id"


class FollowUpInitialDetailView(LoginRequiredMixin, MultiplePermissionsRequiredMixin, UserBasedQuerysetMixin,
                                DetailView):
    model = FollowUpInitial
    permissions = {
        "all": (),
        "any": ("followups.change_followupinitial", "followups.view_followupinitial"),
    }


class FollowUpInitialUpdateView(LoginRequiredMixin, MultiplePermissionsRequiredMixin, UserBasedQuerysetMixin,
                                FormSaveMixin, UpdateView):
    model = FollowUpInitial
    form_class = FollowUpInitialForm
    permissions = {
        "all": ("followups.change_followupinitial", ),
        "any": (),
    }


# 3 Months
class FollowUp3MListView(LoginRequiredMixin, MultiplePermissionsRequiredMixin, OrderableListMixin,
                         UserBasedQuerysetMixin, ListView):
    model = FollowUp3M
    permissions = {
        "all": (),
        "any": ("followups.change_followup3m", "followups.view_followup3m"),
    }
    paginate_by = 100
    orderable_columns = ("organ__trial_id", "start_date",)
    orderable_columns_default = "organ__trial_id"


class FollowUp3MDetailView(LoginRequiredMixin, MultiplePermissionsRequiredMixin, UserBasedQuerysetMixin, DetailView):
    model = FollowUp3M
    permissions = {
        "all": (),
        "any": ("followups.change_followup3m", "followups.view_followup3m"),
    }


class FollowUp3MUpdateView(LoginRequiredMixin, MultiplePermissionsRequiredMixin, UserBasedQuerysetMixin, FormSaveMixin,
                           UpdateView):
    model = FollowUp3M
    form_class = FollowUp3MForm
    permissions = {
        "all": ("followups.change_followup3m", ),
        "any": (),
    }


# 6 Months
class FollowUp6MListView(LoginRequiredMixin, MultiplePermissionsRequiredMixin, OrderableListMixin,
                         UserBasedQuerysetMixin, ListView):
    model = FollowUp6M
    permissions = {
        "all": (),
        "any": ("followups.change_followup6m", "followups.view_followup6m"),
    }
    paginate_by = 100
    orderable_columns = ("organ__trial_id", "start_date",)
    orderable_columns_default = "organ__trial_id"


class FollowUp6MDetailView(LoginRequiredMixin, MultiplePermissionsRequiredMixin, UserBasedQuerysetMixin, DetailView):
    model = FollowUp6M
    permissions = {
        "all": (),
        "any": ("followups.change_followup6m", "followups.view_followup6m"),
    }


class FollowUp6MUpdateView(LoginRequiredMixin, MultiplePermissionsRequiredMixin, UserBasedQuerysetMixin, FormSaveMixin,
                           UpdateView):
    model = FollowUp6M
    form_class = FollowUp6MForm
    permissions = {
        "all": ("followups.change_followup6m",),
        "any": (),
    }


# 1 Year
class FollowUp1YListView(LoginRequiredMixin, MultiplePermissionsRequiredMixin, OrderableListMixin,
                         UserBasedQuerysetMixin, ListView):
    model = FollowUp1Y
    permissions = {
        "all": (),
        "any": ("followups.change_followup1y", "followups.view_followup1y"),
    }
    paginate_by = 100
    orderable_columns = ("organ__trial_id", "start_date",)
    orderable_columns_default = "organ__trial_id"


class FollowUp1YDetailView(LoginRequiredMixin, MultiplePermissionsRequiredMixin, UserBasedQuerysetMixin, DetailView):
    model = FollowUp1Y
    permissions = {
        "all": (),
        "any": ("followups.change_followup1y", "followups.view_followup1y"),
    }


class FollowUp1YUpdateView(LoginRequiredMixin, MultiplePermissionsRequiredMixin, UserBasedQuerysetMixin, FormSaveMixin,
                           UpdateView):
    model = FollowUp1Y
    form_class = FollowUp1YForm
    permissions = {
        "all": ("followups.change_followup1y", ),
        "any": (),
    }
