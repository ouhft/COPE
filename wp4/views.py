#!/usr/bin/python
# coding: utf-8

# from django.contrib import messages
from django.http import Http404
from django.shortcuts import render
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
def wp4_index(request):
    return render(request, 'dashboard/wp4_index.html', {})
