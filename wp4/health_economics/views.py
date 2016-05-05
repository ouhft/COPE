#!/usr/bin/python
# coding: utf-8
from django.contrib.auth.decorators import login_required, permission_required
from django.shortcuts import get_object_or_404, render, render_to_response


@permission_required('health_economics.add_qualityoflife')
@login_required
def index(request):
    return render(request, 'health_economics/index.html', {})
