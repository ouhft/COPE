from django.http import Http404
from django.template import RequestContext
from django.shortcuts import get_object_or_404, render, render_to_response
from django.contrib.auth.decorators import login_required
from django.views.decorators.csrf import csrf_protect
from django.views.generic.edit import FormView
from django.core.urlresolvers import reverse, reverse_lazy
from django.core.exceptions import PermissionDenied
from django.contrib.auth import authenticate, login, logout
from django.forms.models import inlineformset_factory
from django.shortcuts import redirect
import datetime
from random import random

from .models import Donor, Person, Organ
from .forms import DonorForm, DonorStartForm, OrganForm


# Some forced errors to allow for testing the Error Page Templates
def error404(request):
    raise Http404("This is a page holder")  # This message is only for debug view


def error403(request):
    raise PermissionDenied


def error500(request):
    1/0


# def hello_world(request, count):
#     if request.LANGUAGE_CODE == 'de-at':
#         return HttpResponse("You prefer to read Austrian German.")
#     elif request.LANGUAGE_CODE == 'en-gb':
#         return HttpResponse("You prefer to read British English.")
#     elif request.LANGUAGE_CODE == 'fr-fr':
#         return HttpResponse("You prefer to read Crazy French.")
#     else:
#         return HttpResponse("You prefer to read another language.")


def dashboard_index(request):
    return render(request, 'dashboard/index.html', {})


# Legitimate pages
@login_required
@csrf_protect
def procurement_form(request, pk):
    donor = get_object_or_404(Donor, pk=int(pk))
    donor_form = DonorForm(request.POST or None, request.FILES or None, instance=donor, prefix="donor")
    if donor_form.is_valid():
        donor = donor_form.save(request.user)

    left_organ_form = OrganForm(request.POST or None, request.FILES or None, instance=donor.left_kidney(), prefix="left-organ")
    if left_organ_form.is_valid():
        left_organ_form.save(request.user)

    right_organ_form = OrganForm(request.POST or None, request.FILES or None, instance=donor.right_kidney(), prefix="right-organ")
    if right_organ_form.is_valid():
        right_organ_form.save(request.user)


    # Randomise if eligible and not already done
    if donor.left_kidney().preservation is None \
            and donor.multiple_recipients is not False \
            and donor.left_kidney().transplantable \
            and donor.right_kidney().transplantable:
        left_o2 = random() >= 0.5  # True/False
        left_kidney = donor.left_kidney()
        right_kidney = donor.right_kidney()
        if left_o2:
            left_kidney.preservation = Organ.HMPO2
            right_kidney.preservation = Organ.HMP
        else:
            left_kidney.preservation = Organ.HMP
            right_kidney.preservation = Organ.HMPO2
        left_kidney.save()
        right_kidney.save()
        left_organ_form = OrganForm(instance=left_kidney, prefix="left-organ")
        right_organ_form = OrganForm(instance=right_kidney, prefix="right-organ")

    return render_to_response(
        "dashboard/procurement.html",
        {
            "donor_form": donor_form,
            "left_organ_form": left_organ_form,
            "right_organ_form": right_organ_form,
            "donor": donor
        },
        context_instance=RequestContext(request)
    )


@login_required
@csrf_protect
def procurement_form_blank(request):
    if request.method == 'POST':
        donor_form = DonorStartForm(request.POST, request.FILES, prefix="donor")
        if donor_form.is_valid():
            donor = donor_form.save(request.user)
            return redirect(reverse(
                'compare:procurement-detail',
                kwargs={'pk': donor.id}
            ))

    new_donor = Donor()
    current_person = Person.objects.get(user__id=request.user.id)
    if current_person.job == Person.PERFUSION_TECHNICIAN:
        new_donor.perfusion_technician = current_person
    donor_form = DonorStartForm(prefix="donor", instance=new_donor)

    if current_person.job in (Person.SYSTEMS_ADMINISTRATOR, Person.CENTRAL_COORDINATOR, Person.NATIONAL_COORDINATOR):
        donors = Donor.objects.all()
    else:
        donors = {}

    return render_to_response(
        "dashboard/procurement-start.html",
        {
            "donor_form": donor_form,
            "donors" : donors
        },
        context_instance=RequestContext(request)
    )

