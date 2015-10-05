# coding=utf-8

from django.contrib import messages
from django.http import Http404
from django.template import RequestContext
from django.shortcuts import get_object_or_404, render, render_to_response
from django.contrib.auth.decorators import login_required
from django.views.decorators.csrf import csrf_protect
from django.core.urlresolvers import reverse
from django.core.exceptions import PermissionDenied
from django.shortcuts import redirect

from ..staff_person.models import StaffJob, StaffPerson
from .models import Donor, Organ, Recipient
from .forms import DonorForm, DonorStartForm, OrganForm, RecipientForm


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


@login_required
def procurement_list(request):
    new_donor = Donor()
    current_person = StaffPerson.objects.get(user__id=request.user.id)
    if current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
        new_donor.perfusion_technician = current_person
    donor_form = DonorStartForm(request.POST or None, request.FILES or None, prefix="donor", instance=new_donor)
    if request.method == 'POST' and donor_form.is_valid():
        donor = donor_form.save(request.user)
        return redirect(reverse(
            'compare:procurement-detail',
            kwargs={'pk': donor.id}
        ))

    if current_person.has_job(
            (StaffJob.SYSTEMS_ADMINISTRATOR, StaffJob.CENTRAL_COORDINATOR, StaffJob.NATIONAL_COORDINATOR)
    ):
        donors = Donor.objects.all()
    elif current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
        donors = Donor.objects.filter(perfusion_technician=current_person)
    else:
        donors = {}

    return render_to_response(
        "dashboard/procurement_list.html",
        {
            "donor_form": donor_form,
            "donors": donors
        },
        context_instance=RequestContext(request)
    )


@login_required
def procurement_form(request, pk):
    all_valid = 0
    donor = get_object_or_404(Donor, pk=int(pk))
    donor_form = DonorForm(request.POST or None, request.FILES or None, instance=donor, prefix="donor")
    if donor_form.is_valid():
        donor = donor_form.save(request.user)
        all_valid += 1

    left_organ_form = OrganForm(
        request.POST or None,
        request.FILES or None,
        instance=donor.left_kidney(),
        prefix="left-organ")
    if left_organ_form.is_valid():
        left_organ_form.save(request.user)
        all_valid += 1

    right_organ_form = OrganForm(
        request.POST or None,
        request.FILES or None,
        instance=donor.right_kidney(),
        prefix="right-organ")
    if right_organ_form.is_valid():
        right_organ_form.save(request.user)
        all_valid += 1

    is_randomised = donor.left_kidney().preservation != Organ.PRESERVATION_NOT_SET
    print("DEBUG: is_randomised=%s" % is_randomised)

    # TODO: Add the extra test of if the Randomise button was pressed, opposed to just saving
    print("DEBUG: all_valid=%d" % all_valid)
    if all_valid == 3:
        messages.success(request, 'Form has been <strong>successfully saved</strong>')
        if not is_randomised and donor.randomise():  # Has to wait till the organ forms are saved
            # Reload the forms with the modified results
            left_organ_form = OrganForm(instance=donor.left_kidney(), prefix="left-organ")
            right_organ_form = OrganForm(instance=donor.right_kidney(), prefix="right-organ")
            is_randomised = True
            messages.info(request, '<strong>This case has now been randomised!</strong> Preservation results: ' +
                          'Left=%s and Right=%s' % (
                              donor.left_kidney().get_preservation_display(),
                              donor.right_kidney().get_preservation_display()
                          ))
    elif request.POST:
        messages.warning(request, '<strong>Form was not saved</strong>, please check for errors below')

    # messages.add_message(request, messages.INFO, 'Hello world.')
    # messages.debug(request, '%s SQL <i>statements</i> were executed.' % 5)
    # messages.info(request, 'Three credits remain in your account.')
    # messages.success(request, 'Profile details updated.')
    # messages.warning(request, 'Your account expires in three days.')
    # messages.error(request, '<strong>Document</strong> deleted.')

    return render_to_response(
        "dashboard/procurement_form.html",
        {
            "donor_form": donor_form,
            "left_organ_form": left_organ_form,
            "right_organ_form": right_organ_form,
            "donor": donor,
            "is_randomised": is_randomised
        },
        context_instance=RequestContext(request)
    )


@login_required
def transplantation_list(request):
    current_person = StaffPerson.objects.get(user__id=request.user.id)
    if current_person.has_job(
            (StaffJob.SYSTEMS_ADMINISTRATOR, StaffJob.CENTRAL_COORDINATOR, StaffJob.NATIONAL_COORDINATOR)
    ):
        recipients = Recipient.objects.filter(successful_conclusion=False)
        organs = Organ.objects.exclude(preservation__isnull=True)  # TODO: Add a filter to exclude organs with recipient forms
    elif current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
        recipients = Recipient.objects.filter(perfusion_technician=current_person)
        organs = Organ.objects.exclude(preservation__isnull=True)
    else:
        recipients = {}
        organs = {}

    return render_to_response(
        "dashboard/transplantation_list.html",
        {
            "recipients": recipients,
            "organs": organs
        },
        context_instance=RequestContext(request)
    )


@login_required
def transplantation_form_new(request, pk):
    """
    Setup a new Transplanation Form for a given Organ ID

    :param request:
    :param pk: Organ primary key
    :return:
    """
    organ = get_object_or_404(Organ, pk=int(pk))
    if len(organ.recipient_set.all()) > 0:
        # There's a form already created... use it!
        recipient = organ.recipient_set.latest()  # TODO: This throws an error!
    else:
        recipient = Recipient()
        recipient.organ = organ
        recipient.created_by = request.user

        current_person = StaffPerson.objects.get(user__id=request.user.id)
        if current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
            recipient.perfusion_technician = current_person

        recipient.save()

    recipient_form = RecipientForm(prefix="recipient", instance=recipient)

    return render_to_response(
        "dashboard/transplantation_form.html",
        {
            "recipient_form": recipient_form,
            "recipient": recipient
        },
        context_instance=RequestContext(request)
    )


@login_required
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
            if current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
                new_recipient.perfusion_technician = current_person
            recipient_form = RecipientForm(instance=new_recipient, prefix="recipient")
            recipient = new_recipient

    return render_to_response(
        "dashboard/transplantation_form.html",
        {
            "recipient_form": recipient_form,
            "recipient": recipient
        },
        context_instance=RequestContext(request)
    )
