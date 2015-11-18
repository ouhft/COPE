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
from django.utils import timezone

from ..staff_person.models import StaffJob, StaffPerson
from .models import OrganPerson, Donor, Organ, Recipient, ProcurementResource, OrganAllocation
from .forms import OrganPersonForm, DonorForm, DonorStartForm, OrganForm, AllocationFormSet, RecipientForm
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
        # This is a new set of objects, so remember to create the Person for the Donor first
        person = OrganPerson()
        person.gender = donor_form.cleaned_data.get("gender")
        person.created_by = request.user
        person.created_on = timezone.now()
        person.version += 1
        person.save()
        donor = donor_form.save(request.user, commit=False)
        donor.person = person
        donor.save()

        return redirect(reverse(
            'compare:procurement_detail',
            kwargs={'pk': donor.id}
        ))

    if current_person.has_job(
            (StaffJob.SYSTEMS_ADMINISTRATOR, StaffJob.CENTRAL_COORDINATOR, StaffJob.NATIONAL_COORDINATOR)
    ):
        donors = Donor.objects.all().order_by('-pk')
    elif current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
        donors = Donor.objects.filter(perfusion_technician=current_person).order_by('-pk')
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
    """
    :param request:
    :param pk: Donor ID. We get all related information from the donor record
    :return:
    """
    all_valid = 0
    donor = get_object_or_404(Donor, pk=int(pk))
    current_person = StaffPerson.objects.get(user__id=request.user.id)

    def procurement_initial_data(organ, created_by_user):
        if len(organ.procurementresource_set.all()) < 7:
            for resource in ProcurementResource.TYPE_CHOICES:
                new_resource = ProcurementResource(organ=organ, type=resource[0], created_by=created_by_user)
                new_resource.save()

    # ================================================ DONOR
    person_form = OrganPersonForm(request.POST or None, request.FILES or None, instance=donor.person,
                                  prefix="donor-person")
    if person_form.is_valid():
        person = person_form.save(request.user)
        all_valid += 1

    donor_form = DonorForm(request.POST or None, request.FILES or None, instance=donor, prefix="donor")
    if donor_form.is_valid():
        donor = donor_form.save(request.user)
        all_valid += 1
    # print("DEBUG: donor_form has errors: %s" % donor_form.errors)

    # ================================================ LEFT ORGAN
    left_organ_instance = donor.left_kidney()
    procurement_initial_data(donor.left_kidney(), current_person.user)
    # print("DEBUG: left_organ_instance=%s" % left_organ_instance)
    left_organ_form = OrganForm(request.POST or None, request.FILES or None,
                                instance=left_organ_instance, prefix="left-organ")
    if left_organ_form.is_valid():
        left_organ_instance = left_organ_form.save(request.user)
        all_valid += 1

    left_organ_procurement_forms = ProcurementResourceLeftInlineFormSet(
        request.POST or None,
        prefix="left-organ-procurement",
        instance=left_organ_instance)
    if left_organ_procurement_forms.is_valid():
        left_organ_procurement_forms.save()
        all_valid += 1

    left_organ_error_count = left_organ_procurement_forms.total_error_count() + len(left_organ_form.errors)

    # =============================================== RIGHT ORGAN
    right_organ_instance = donor.right_kidney()
    procurement_initial_data(donor.right_kidney(), current_person.user),
    right_organ_form = OrganForm(request.POST or None, request.FILES or None, instance=right_organ_instance,
                                 prefix="right-organ")
    if right_organ_form.is_valid():
        right_organ_instance = right_organ_form.save(request.user)
        all_valid += 1

    right_organ_procurement_forms = ProcurementResourceRightInlineFormSet(
        request.POST or None,
        prefix="right-organ-procurement",
        # initial=procurement_initial_data(donor.right_kidney(), current_person.user),
        instance=right_organ_instance)
    if right_organ_procurement_forms.is_valid():
        right_organ_procurement_forms.save()
        all_valid += 1

    right_organ_error_count = right_organ_procurement_forms.total_error_count() + len(right_organ_form.errors)

    # =============================================== MESSAGES
    is_randomised = donor.left_kidney().preservation != Organ.PRESERVATION_NOT_SET
    # print("DEBUG: is_randomised=%s" % is_randomised)

    print("DEBUG: all_valid=%d" % all_valid)
    if all_valid == 6:
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

    return render_to_response(
        "compare/procurement_form.html",
        {
            "person_form": person_form,
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
        ).annotate(copies=Count('recipient__id'))
        new_cases = Organ.objects.filter(preservation__lte=1).exclude(recipient__isnull=False)\
            .exclude(transplantable=False)
    elif current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
        existing_cases = Organ.objects.filter(recipient__perfusion_technician=current_person)
        new_cases = Organ.objects.filter(preservation__lte=1).exclude(recipient__isnull=False)\
            .exclude(transplantable=False)
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

    Process Allocation results until an allocation is set for one location, and then create and manage the
    Recipient record.
    """
    print("DEBUG: Got an organ pk of %s" % pk)
    organ = get_object_or_404(Organ, pk=int(pk))
    current_person = StaffPerson.objects.get(user__id=request.user.id)
    person_form = None
    recipient_form = None
    recipient_form_loaded = False
    if len(organ.organallocation_set.all()) < 1:
        initial_organallocation = OrganAllocation(organ=organ, created_by=current_person.user)
        initial_organallocation.save()

    try:
        if organ.recipient is not None:
            print("DEBUG: Starting the Recipient form")
            person_form = OrganPersonForm(request.POST or None, request.FILES or None, instance=organ.recipient.person,
                                          prefix="donor-person")
            if person_form.is_valid():
                person_form.save(request.user)
            else:
                print("DEBUG: Person Errors! %s" % person_form.errors)

            recipient_form = RecipientForm(request.POST or None, request.FILES or None,
                                           instance=organ.recipient, prefix="recipient")
            if recipient_form.is_valid():
                recipient_form.save()
            else:
                print("DEBUG: Recipient Errors! %s" % recipient_form.errors)

            recipient_form_loaded = True
    except AttributeError:  # This is the base class for RelatedObjectDoesNotExist exception
        pass

    # Process Allocations
    allocation_formset = AllocationFormSet(request.POST or None, prefix="allocation",
                                           queryset=organ.organallocation_set.all())
    if allocation_formset.is_valid():
        last_form_index = len(allocation_formset)-1
        for i, form in enumerate(allocation_formset):
            allocation = form.save(current_person.user)

            if i == last_form_index:
                # If this is the latest Allocation and reallocation has occurred, create a new Allocation
                if allocation.reallocated:
                    # print("DEBUG: Do the reallocation thing!")
                    new_allocation = OrganAllocation()
                    new_allocation.organ = organ
                    new_allocation.created_by = request.user
                    if current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
                        new_allocation.perfusion_technician = current_person
                    new_allocation.save()

                    allocation.reallocation = new_allocation
                    allocation.save()

                    messages.success(request, 'Form has been <strong>successfully saved</strong>')

                    return redirect(reverse(
                        'compare:transplantation_detail',
                        kwargs={'pk': organ.id}
                    ))
                # For a new set of objects, remember to create the Person for the Recipient first
                elif allocation.reallocated is not None and not recipient_form_loaded:
                    person = OrganPerson()
                    person.created_by = current_person.user
                    person.created_on = timezone.now()
                    person.version += 1
                    person.save()

                    recipient = Recipient()
                    recipient.person = person
                    recipient.allocation = allocation
                    recipient.organ = organ
                    recipient.created_by = current_person.user
                    recipient.created_on = timezone.now()
                    recipient.version += 1
                    recipient.save()

                    messages.success(request, 'Form has been <strong>successfully saved</strong>')

                    return redirect(reverse(
                        'compare:transplantation_detail',
                        kwargs={'pk': organ.id}
                    ))
    else:
        print("DEBUG: Errors! %s" % allocation_formset.errors)

    return render_to_response(
        "compare/transplantation_form.html",
        {
            "allocation_formset": allocation_formset,
            "person_form": person_form,
            "recipient_form": recipient_form,
            "organ": organ,
            "recipient_form_loaded": recipient_form_loaded
        },
        context_instance=RequestContext(request))
