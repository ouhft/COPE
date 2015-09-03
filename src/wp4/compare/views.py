# coding=utf-8
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
from django_ajax.decorators import ajax

from .models import Donor, StaffPerson, Organ, Sample, Recipient, AdverseEvent
from .forms import DonorForm, DonorStartForm, OrganForm, SampleForm, RecipientForm, AdverseEventForm


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

    left_organ_form = OrganForm(request.POST or None, request.FILES or None, instance=donor.left_kidney(),
                                prefix="left-organ")
    if left_organ_form.is_valid():
        left_organ_form.save(request.user)

    right_organ_form = OrganForm(request.POST or None, request.FILES or None, instance=donor.right_kidney(),
                                 prefix="right-organ")
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
    current_person = StaffPerson.objects.get(user__id=request.user.id)
    if current_person.job == StaffPerson.PERFUSION_TECHNICIAN:
        new_donor.perfusion_technician = current_person
    donor_form = DonorStartForm(prefix="donor", instance=new_donor)

    if current_person.job in (
    StaffPerson.SYSTEMS_ADMINISTRATOR, StaffPerson.CENTRAL_COORDINATOR, StaffPerson.NATIONAL_COORDINATOR):
        donors = Donor.objects.all()
    else:
        donors = {}

    return render_to_response(
        "dashboard/procurement-start.html",
        {
            "donor_form": donor_form,
            "donors": donors
        },
        context_instance=RequestContext(request)
    )


@login_required
@csrf_protect
@ajax
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


@login_required
@csrf_protect
def transplantation_form_list(request):
    current_person = StaffPerson.objects.get(user__id=request.user.id)
    if current_person.job in (StaffPerson.SYSTEMS_ADMINISTRATOR, StaffPerson.CENTRAL_COORDINATOR,
                              StaffPerson.NATIONAL_COORDINATOR):
        recipients = Recipient.objects.all()
        organs = Organ.objects.exclude(preservation__isnull=True)
    else:
        recipients = {}
        organs = {}

    return render_to_response(
        "dashboard/transplantation-list.html",
        {
            "recipients": recipients,
            "organs": organs
        },
        context_instance=RequestContext(request)
    )

@login_required
@csrf_protect
def transplantation_form_new(request, pk):
    """
    Setup a new Transplanation Form for a given Organ ID

    :param request:
    :param pk: Organ primary key
    :return:
    """
    organ = get_object_or_404(Organ, pk=int(pk))
    print("hello")
    print(organ.recipient_set.all())
    if len(organ.recipient_set.all()) > 0:
        # There's a form already created... use it!
        recipient = organ.recipient_set.latest()  # TODO: This throws an error!
    else:
        recipient = Recipient()
        recipient.organ = organ
        recipient.created_by = request.user

        current_person = StaffPerson.objects.get(user__id=request.user.id)
        if current_person.job == StaffPerson.PERFUSION_TECHNICIAN:
            recipient.perfusion_technician = current_person

        recipient.save()

    recipient_form = RecipientForm(prefix="recipient", instance=recipient)

    return render_to_response(
        "dashboard/transplantation.html",
        {
            "recipient_form": recipient_form,
            "recipient": recipient
        },
        context_instance=RequestContext(request)
    )


@login_required
@csrf_protect
def transplantation_form(request, pk):
    recipient = get_object_or_404(Recipient, pk=int(pk))
    recipient_form = RecipientForm(request.POST or None, request.FILES or None, instance=recipient, prefix="recipient")
    if recipient_form.is_valid():
        recipient = recipient_form.save(request.user)

        if recipient.reallocated:
            # Start a new recipient form if the minimum information has been entered
            new_recipient = Recipient()
            new_recipient.organ = recipient.organ
            new_recipient.created_by = request.user
            current_person = StaffPerson.objects.get(user__id=request.user.id)
            if current_person.job == StaffPerson.PERFUSION_TECHNICIAN:
                new_recipient.perfusion_technician = current_person
            recipient_form = RecipientForm(instance=new_recipient, prefix="recipient")
            recipient = new_recipient

    return render_to_response(
        "dashboard/transplantation.html",
        {
            "recipient_form": recipient_form,
            "recipient": recipient
        },
        context_instance=RequestContext(request)
    )


@login_required
def adverse_events_list(request):
    events = AdverseEvent.objects.all()
    organs = Organ.objects.exclude(transplantable=False)

    return render_to_response(
        "dashboard/adverseevents-list.html",
        {
            "events": events,
            "organs": organs
        },
        context_instance=RequestContext(request)
    )


@login_required
def adverse_event_form_new(request):
    raise Http404("This is a page holder")  # This message is only for debug view


@login_required
def adverse_event_form(request):
    raise Http404("This is a page holder")  # This message is only for debug view
