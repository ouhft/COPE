#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.contrib import messages
from django.contrib.auth.models import Group
from django.db.models import Q
from django.http import HttpResponseRedirect
from django.views.generic import ListView, CreateView, UpdateView, DetailView

from braces.views import LoginRequiredMixin, PermissionRequiredMixin, OrderableListMixin, MultiplePermissionsRequiredMixin
from dal import autocomplete

from wp4.theme.layout import AjaxReturnIDMixin
from .models import Person
from .forms import PersonForm, PersonAjaxForm
from .utils import generate_username


class TechnicianAutoComplete(autocomplete.Select2QuerySetView):
    def get_queryset(self):
        if not self.request.user.is_authenticated():
            return Person.objects.none()

        qs = Person.objects.filter(groups__in=[Person.PERFUSION_TECHNICIAN])

        if self.q:
            qs = qs.filter(Q(first_name__icontains=self.q) | Q(last_name__icontains=self.q))

        return qs


# ============================================  MIXINS
class AjaxListSearchMixin(object):
    def get_queryset(self):
        queryset = super(AjaxListSearchMixin, self).get_queryset()
        if self.request.is_ajax():
            queryset = queryset.order_by('first_name')

        # If a q value is set, filter based on groups
        q = self.request.GET.get("q")
        if q and q.isdigit():
            return queryset.filter(groups__id__in=[int(q)])
        return queryset

    def get_template_names(self):
        # print("DEBUG: get_template_names():is_ajax? %s" % self.request.is_ajax())
        if self.request.is_ajax():
            self.template_name = "staff/person_list.ajax.html"
        return super(AjaxListSearchMixin, self).get_template_names()

    def get_context_data(self, **kwargs):
        # Call the base implementation first to get a context
        context = super(AjaxListSearchMixin, self).get_context_data(**kwargs)
        # Add in a QuerySet of all the groups
        context['group_list'] = Group.objects.all()
        return context


class AjaxFormMixin(object):
    form_class = PersonForm

    def form_valid(self, form):
        self.object = form.save(commit=False)
        self.object.username = generate_username(self.object)
        self.object.save()
        form.save_m2m()

        if self.request.is_ajax():
            return self.render_to_response(self.get_context_data(form=form))
        else:
            return HttpResponseRedirect(self.get_success_url())

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
        if not self.request.user.is_administrator:
            form_class = PersonAjaxForm  # Stop non admins from changing groups
        form = super(AjaxFormMixin, self).get_form(form_class)

        # Both post() and get() call get_form() first, so this is best place to intercept ajax changes
        if self.request.is_ajax():
            form = super(AjaxFormMixin, self).get_form(PersonAjaxForm)
            # form.fields['groups'].widget = forms.HiddenInput()
            self.template_name = "staff/person_form.ajax.html"
        return form


# ============================================  CBVs
class PersonListView(AjaxReturnIDMixin, AjaxListSearchMixin,
                     LoginRequiredMixin, MultiplePermissionsRequiredMixin,
                     OrderableListMixin, ListView):
    model = Person
    permissions = {
        "all": (),
        "any": ("staff.change_person", "staff.single_person"),
    }
    ordering = ['first_name']
    paginate_by = 50
    paginate_orphans = 5
    orderable_columns = ("first_name", "last_name", "email", "telephone", "based_at")
    orderable_columns_default = "first_name"

    def get_paginate_by(self, queryset):
        """
        Get the number of items to paginate by, or ``None`` for no pagination.
        """
        if self.request.is_ajax():
            # Don't paginate for ajax listings
            return None

        return self.paginate_by


class PersonDetailView(AjaxReturnIDMixin, LoginRequiredMixin, MultiplePermissionsRequiredMixin, DetailView):
    model = Person
    permissions = {
        "all": (),
        "any": ("staff.change_person", "staff.single_person"),
    }


class PersonCreateView(AjaxReturnIDMixin, AjaxFormMixin,
                       LoginRequiredMixin, MultiplePermissionsRequiredMixin,
                       CreateView):
    model = Person
    permissions = {
        "all": ("staff.add_person", ),
        "any": (),
    }


class PersonUpdateView(AjaxReturnIDMixin, AjaxFormMixin,
                       LoginRequiredMixin, MultiplePermissionsRequiredMixin,
                       UpdateView):
    model = Person
    permissions = {
        "all": ("staff.change_person", ),
        "any": (),
    }
