#!/usr/bin/python
# coding: utf-8

from django.contrib.auth.decorators import login_required
from django.http import Http404
from django.shortcuts import get_object_or_404, render, render_to_response
from django.template import RequestContext
from django.views.decorators.csrf import csrf_protect
from django.views.generic import ListView, CreateView, UpdateView, DetailView

from .models import Sample
from .forms import SampleForm


@login_required
@csrf_protect
# @ajax
def sample_editor(request, pk=None, type=None):
    valid_types = [t[0] for t in Sample.TYPE_CHOICES]
    if pk is not None:
        sample = get_object_or_404(Sample, pk=int(pk))
    elif type is not None and int(type) in valid_types:
        sample = Sample(type=type)
    else:
        raise Http404("This is a page isn't happy")

    sample_form = SampleForm(request.POST or None, request.FILES or None, instance=sample, prefix="sample")
    if sample_form.is_valid():
        sample = sample_form.save(request.user)

    return render_to_response(
        "includes/sample-form.html",
        {"sample_form": sample_form},
        context_instance=RequestContext(request)
    )
