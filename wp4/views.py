#!/usr/bin/python
# coding: utf-8

# from django.contrib import messages
from django.http import Http404
from django.shortcuts import render
from django.conf import settings
from django.contrib.auth.decorators import login_required
from django.core.exceptions import PermissionDenied
from django.utils import timezone

from wp4.compare.models import Randomisation, PAPER_UNITED_KINGDOM, PAPER_EUROPE, Organ

from .utils import group_required


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
def dashboard_changelog(request):
    file_path = settings.ROOT_DIR + 'docs/changelog.md'
    with open(str(file_path), 'r') as markdown_file:
        document = markdown_file.read()
        return render(request, 'dashboard/changelog.html', {'document': document})


def dashboard_usermanual(request):
    file_path = settings.ROOT_DIR + 'docs/user_manual.md'
    with open(str(file_path), 'r') as markdown_file:
        document = markdown_file.read()
        return render(request, 'dashboard/user_manual.html', {'document': document})


@login_required
def wp4_index(request):
    return render(request, 'dashboard/wp4_index.html', {})


@group_required('Central Co-ordinators')
@login_required
def administrator_index(request):
    return render(request, 'dashboard/administrator_index.html', {})


@group_required('Central Co-ordinators')
@login_required
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


@group_required('Central Co-ordinators')
@login_required
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

@login_required
def administrator_datalist(request):
    donors = Organ.objects.all()
    return render(
        request,
        'dashboard/administrator_data_list.html',
        {
            'listing': donors,
            # 'location': "Europe",
            # 'timestamp': timezone.now()
        }
    )
