#!/usr/bin/python
# coding: utf-8

from django.contrib.auth.decorators import login_required
from django.core.urlresolvers import reverse, resolve
from django.http import Http404, HttpResponseRedirect
from django.views.decorators.csrf import csrf_protect
# from django.views.generic import ListView, CreateView, UpdateView, DetailView
from vanilla import ListView, CreateView, UpdateView, DetailView
from braces.views import LoginRequiredMixin

from ..compare.models import Recipient
from .models import FollowUpInitial, FollowUp3M, FollowUp6M, FollowUp1Y
from .forms import FollowUpInitialForm


class FollowUpList(ListView):
    # List all Organs, that have successfully been transplanted, by date of transplantation
    model = Recipient
    queryset = Recipient.objects.filter(successful_conclusion=True).order_by('operation_concluded_at')


class FollowUpInitialList(LoginRequiredMixin, ListView):
    model = FollowUpInitial
    context_object_name = 'initial_list'


class FollowUpInitialDetail(LoginRequiredMixin, DetailView):
    model = FollowUpInitial
    context_object_name = 'initial_obj'


class FollowUpInitialAdd(LoginRequiredMixin, CreateView):
    model = FollowUpInitial
    fields = ['organ']


class FollowUpInitialUpdate(LoginRequiredMixin, UpdateView):
    model = FollowUpInitial
    form_class = FollowUpInitialForm

    def form_valid(self, form):
        instance = form.save(self.request.user)
        return HttpResponseRedirect(instance.get_absolute_url())
