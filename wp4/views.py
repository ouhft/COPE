#!/usr/bin/python
# coding: utf-8

# from django.contrib import messages
from django.http import Http404
from django.shortcuts import get_object_or_404, render, render_to_response
# from django.contrib.auth.decorators import login_required
# from django.core.urlresolvers import reverse
from django.core.exceptions import PermissionDenied
# from django.shortcuts import redirect


# Some forced errors to allow for testing the Error Page Templates
def error404(request):
    raise Http404("This is a page holder")  # This message is only for debug view


def error403(request):
    raise PermissionDenied


def error500(request):
    1 / 0


# def hello_world(request, count):
#     if request.LANGUAGE_CODE == 'de-at':
#         return HttpResponse("You prefer to read Austrian German.")
#     elif request.LANGUAGE_CODE == 'en-gb':
#         return HttpResponse("You prefer to read British English.")
#     elif request.LANGUAGE_CODE == 'fr-fr':
#         return HttpResponse("You prefer to read Crazy French.")
#     else:
#         return HttpResponse("You prefer to read another language.")


# Legitimate pages
def dashboard_index(request):
    return render(request, 'dashboard/index.html', {})
