#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

# from django.contrib import messages
from django.http import Http404, HttpResponse
from django.shortcuts import render
from django.conf import settings
from django.contrib.auth.decorators import login_required
from django.core.exceptions import PermissionDenied


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
    file_path = settings.ROOT_DIR + 'docs/source/changelog.md'
    with open(str(file_path), 'r') as markdown_file:
        document = markdown_file.read()
        return render(request, 'dashboard/changelog.html', {'document': document})


def dashboard_usermanual(request):
    file_path = settings.ROOT_DIR + 'docs/source/user_manual.md'
    with open(str(file_path), 'r') as markdown_file:
        document = markdown_file.read()
        return render(request, 'dashboard/user_manual.html', {'document': document})


@login_required
def wp4_index(request):
    return render(request, 'dashboard/wp4_index.html', {})
