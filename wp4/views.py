#!/usr/bin/python
# coding: utf-8

# from django.contrib import messages
from django.http import Http404
from django.shortcuts import render
from django.contrib.auth.decorators import login_required, user_passes_test
from django.core.exceptions import PermissionDenied
from django.utils import timezone
from .compare.models import Randomisation, PAPER_UNITED_KINGDOM, PAPER_EUROPE


# Some forced errors to allow for testing the Error Page Templates
def error404(request):
    raise Http404("This is a page holder")  # This message is only for debug view


def error403(request):
    raise PermissionDenied


def error500(request):
    1 / 0


# Legitimate pages
def dashboard_index(request):
    return render(request, 'dashboard/index.html', {})

@login_required
def wp4_index(request):
    return render(request, 'dashboard/wp4_index.html', {})


def _is_coordinator(user):
    if user.is_superuser:
        return True
    return user.groups.filter(name='Central Co-ordinators').exists()

@login_required
@user_passes_test(_is_coordinator)
def administrator_index(request):
    return render(request, 'dashboard/administrator_index.html', {})

@login_required
@user_passes_test(_is_coordinator)
def administrator_uk_list(request):
    randomisation_listing = Randomisation.objects.filter(list_code=PAPER_UNITED_KINGDOM)
    return render(
        request,
        'dashboard/administrator_offline_list.html',
        {
            'listing': randomisation_listing,
            'location': "United Kingdom",
            'timestamp': timezone.now()
        }
    )

@login_required
@user_passes_test(_is_coordinator)
def administrator_europe_list(request):
    randomisation_listing = Randomisation.objects.filter(list_code=PAPER_EUROPE)
    return render(
        request,
        'dashboard/administrator_offline_list.html',
        {
            'listing': randomisation_listing,
            'location': "Europe",
            'timestamp': timezone.now()
        }
    )
