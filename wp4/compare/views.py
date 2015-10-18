# coding=utf-8

from django.contrib import messages
from django.db.models import Q, Count
from django.http import Http404
from django.template import RequestContext
from django.shortcuts import get_object_or_404, render, render_to_response
from django.contrib.auth.decorators import login_required
from django.core.urlresolvers import reverse
from django.core.exceptions import PermissionDenied
from django.shortcuts import redirect

from ..staff_person.models import StaffJob, StaffPerson
from .models import Donor, Organ, Recipient, ProcurementResource
from .forms import DonorForm, DonorStartForm, OrganForm, RecipientFormASet, RecipientFormB
from .forms import ProcurementResourceLeftInlineFormSet, ProcurementResourceRightInlineFormSet


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
            'compare:procurement_detail',
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
        "compare/procurement_list.html",
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
    current_person = StaffPerson.objects.get(user__id=request.user.id)

    def procurement_initial_data(organ, creator):
        return [
            {'organ': organ.pk, 'type': ProcurementResource.DISPOSABLES, 'created_by': creator.pk},
            {'organ': organ.pk, 'type': ProcurementResource.EXTRA_CANNULA_SMALL, 'created_by': creator.pk},
            {'organ': organ.pk, 'type': ProcurementResource.EXTRA_CANNULA_LARGE, 'created_by': creator.pk},
            {'organ': organ.pk, 'type': ProcurementResource.EXTRA_PATCH_HOLDER_SMALL, 'created_by': creator.pk},
            {'organ': organ.pk, 'type': ProcurementResource.EXTRA_PATCH_HOLDER_LARGE, 'created_by': creator.pk},
            {'organ': organ.pk, 'type': ProcurementResource.EXTRA_DOUBLE_CANNULA_SET, 'created_by': creator.pk},
            {'organ': organ.pk, 'type': ProcurementResource.PERFUSATE_SOLUTION, 'created_by': creator.pk},
        ]

    donor_form = DonorForm(request.POST or None, request.FILES or None, instance=donor, prefix="donor")
    if donor_form.is_valid():
        donor = donor_form.save(request.user)
        all_valid += 1

    # ================================================ LEFT ORGAN
    left_organ_form = OrganForm(request.POST or None, request.FILES or None,
                                instance=donor.left_kidney(), prefix="left-organ")
    left_organ_procurement_forms = ProcurementResourceLeftInlineFormSet(
        request.POST or None,
        prefix="left-organ-procurement",
        initial=procurement_initial_data(donor.left_kidney(), current_person),
        instance=donor.left_kidney())
    if left_organ_form.is_valid() and left_organ_procurement_forms.is_valid():
        left_organ_form.save(request.user)
        for p_form in left_organ_procurement_forms:
            p_form.save()
        all_valid += 1
    left_organ_error_count = left_organ_procurement_forms.total_error_count() + len(left_organ_form.errors)

    # =============================================== RIGHT ORGAN
    right_organ_form = OrganForm(
        request.POST or None,
        request.FILES or None,
        instance=donor.right_kidney(),
        prefix="right-organ")
    right_organ_procurement_forms = ProcurementResourceRightInlineFormSet(
        request.POST or None,
        prefix="right-organ-procurement",
        initial=procurement_initial_data(donor.right_kidney(), current_person),
        instance=donor.right_kidney())
    if right_organ_form.is_valid() and right_organ_procurement_forms.is_valid():
        right_organ_form.save(request.user)
        for p_form in right_organ_procurement_forms:
            p_form.save()
        all_valid += 1
    right_organ_error_count = right_organ_procurement_forms.total_error_count() + len(right_organ_form.errors)

    # =============================================== MESSAGES
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
            messages.info(
                request,
                '<strong>This case has now been randomised!</strong> Preservation results: Left=%s and Right=%s'
                % (donor.left_kidney().get_preservation_display(), donor.right_kidney().get_preservation_display()))
    elif request.POST:
        messages.error(request,
                       '<strong>Form was NOT saved</strong>, please correct the %d errors below' %
                       (left_organ_error_count + right_organ_error_count + len(donor_form.errors)))

    # messages.add_message(request, messages.INFO, 'Hello world.')
    # messages.debug(request, '%s SQL <i>statements</i> were executed.' % 5)
    # messages.info(request, 'Three credits remain in your account.')
    # messages.success(request, 'Profile details updated.')
    # messages.warning(request, 'Your account expires in three days.')
    # messages.error(request, '<strong>Document</strong> deleted.')

    # print("DEBUG: Formset contains %s" % left_organ_procurement_form)
    return render_to_response(
        "compare/procurement_form.html",
        {
            "donor_form": donor_form,
            "left_organ_form": left_organ_form,
            "left_organ_procurement_forms": left_organ_procurement_forms,
            "left_organ_error_count": left_organ_error_count,
            "right_organ_form": right_organ_form,
            "right_organ_procurement_forms": right_organ_procurement_forms,
            "right_organ_error_count": right_organ_error_count,
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
        existing_cases = Organ.objects.filter(
            Q(recipient__isnull=False),
            Q(recipient__successful_conclusion=False)
        ).annotate(copies=Count('recipient__id'))
        new_cases = Organ.objects.filter(preservation__lte=1).exclude(recipient__isnull=False)
    elif current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
        existing_cases = Organ.objects.filter(recipient__perfusion_technician=current_person)
        new_cases = Organ.objects.filter(preservation__lte=1).exclude(recipient__isnull=False)
    else:
        existing_cases = {}
        new_cases = {}

    return render_to_response(
        "compare/transplantation_list.html",
        {
            "existing_cases": existing_cases,
            "new_cases": new_cases
        },
        context_instance=RequestContext(request)
    )


@login_required
def transplantation_form(request, pk=None):
    """
    :param request:
    :param pk: Organ primary key, as all recipients are tied to an organ
    :return:
    """
    print("DEBUG: Got an organ pk of %s" % pk)
    organ = get_object_or_404(Organ, pk=int(pk))
    current_person = StaffPerson.objects.get(user__id=request.user.id)
    recipient_form = {}
    recipient_form_loaded = False
    recipient = Recipient()

    # First time in, we need to create an initial recipient FormA, and recipient record
    if len(organ.recipient_set.all()) == 0:
        recipient.organ = organ
        recipient.created_by = request.user
        if current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
            recipient.perfusion_technician = current_person
        recipient.save()
    else:  # Otherwise load the last recipient created
        recipient = organ.recipient_set.latest()

    # Process Form A
    recipient_formset = RecipientFormASet(request.POST or None, prefix="recipient-a",
                                          queryset=organ.recipient_set.all())
    if recipient_formset.is_valid():
        last_form_index = len(recipient_formset)-1
        for i, form in enumerate(recipient_formset):
            print("DEBUG: i=%d" % i)
            recipient = form.save(current_person.user)

            if i == last_form_index:
                # If this is the latest form A and reallocation has occurred, create a new recipient
                if recipient.reallocated:
                    print("DEBUG: Do the reallocation thing!")
                    new_recipient = Recipient()
                    new_recipient.organ = organ
                    new_recipient.created_by = request.user
                    if current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
                        new_recipient.perfusion_technician = current_person
                    new_recipient.save()

                    recipient.recipient = new_recipient
                    recipient.save()

                    # Reload the formset to pick up the new addition
                    recipient_formset = RecipientFormASet(prefix="recipient-a", queryset=organ.recipient_set.all())
                    recipient = new_recipient
    else:
        print("DEBUG: Errors! %s" % recipient_formset.errors)

    if recipient.reallocated is not None and not recipient.reallocated:
        # Now process the potential Form B
        recipient_form = RecipientFormB(request.POST or None, request.FILES or None,
                                        instance=recipient, prefix="form-b")
        recipient_form_loaded = True
        if recipient_form.is_valid():
            recipient_form.save()
        else:
            print("DEBUG: Recipient Errors! %s" % recipient_form.errors)

    return render_to_response(
        "compare/transplantation_form.html",
        {
            "recipient_formset": recipient_formset,
            "recipient_form": recipient_form,
            "organ": organ,
            "recipient_form_loaded": recipient_form_loaded
        },
        context_instance=RequestContext(request)
    )
