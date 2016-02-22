#!/usr/bin/python
# coding: utf-8

from django.contrib import messages
from django.db.models import Q
from django.template import RequestContext
from django.shortcuts import get_object_or_404, render, render_to_response
from django.contrib.auth.decorators import login_required, permission_required
from django.core.urlresolvers import reverse
from django.shortcuts import redirect
from django.utils import timezone

from dal import autocomplete

from wp4.staff_person.models import StaffJob, StaffPerson
from wp4.samples.utils import create_donor_worksheet, create_recipient_worksheet
# from wp4.utils import job_required, group_required

from .models import OrganPerson, Donor, Organ, Recipient, ProcurementResource, OrganAllocation, RetrievalTeam
from .models import PRESERVATION_HMPO2, PRESERVATION_HMP
from .forms.core import DonorStartForm, OrganPersonForm, AllocationStartForm
from .forms.procurement import DonorForm, OrganForm
from .forms.procurement import ProcurementResourceLeftInlineFormSet, ProcurementResourceRightInlineFormSet
from .forms.transplantation import AllocationFormSet, RecipientForm, TransplantOrganForm


@login_required
def index(request):
    return render(request, 'compare/index.html', {})


class RetrievalTeamAutoComplete(autocomplete.Select2QuerySetView):
    def get_queryset(self):
        if not self.request.user.is_authenticated():
            return RetrievalTeam.objects.none()

        qs = RetrievalTeam.objects.all()

        if self.q:
            qs = qs.filter(Q(centre_code__icontains=self.q) | Q(based_at__name__icontains=self.q))

        return qs


@permission_required('compare.add_donor')
@login_required
def procurement_list(request):
    new_donor = Donor()
    current_person = StaffPerson.objects.get(user__id=request.user.id)
    if current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
        new_donor.perfusion_technician = current_person

    def create_procurement_initial_data(organ, created_by_user):
        for resource in ProcurementResource.TYPE_CHOICES:
            new_resource = ProcurementResource(organ=organ, type=resource[0], created_by=created_by_user)
            new_resource.save()

    # Process the new case form
    donor_form = DonorStartForm(request.POST or None, request.FILES or None, prefix="donor", instance=new_donor)
    if request.method == 'POST' and donor_form.is_valid():
        # This is a new set of objects, so remember to create the Person for the Donor first
        person = OrganPerson()
        person.gender = donor_form.cleaned_data.get("gender")
        person.save(created_by=request.user)

        donor = donor_form.save(request.user, commit=False)
        donor.person = person
        donor.save(created_by=request.user)

        # Create the organs and the procurement resources
        create_procurement_initial_data(donor.left_kidney, current_person.user)
        create_procurement_initial_data(donor.right_kidney, current_person.user)

        # Create the sample place holders for this form
        create_donor_worksheet(donor, request.user)

        is_online = donor_form.cleaned_data.get("online")
        print("DEBUG: donor_start_form: online: %s" % is_online)
        if not is_online:
            # Randomisation has been done, so link to the donor, and set the basic organ parameters
            randomisation = donor_form.cleaned_data.get("randomisation")
            print("DEBUG: donor_start_form: randomisation: %s" % randomisation)
            # randomisation = Randomisation.objects.get(randomisation_id)
            randomisation.donor = donor
            randomisation.allocated_on = timezone.now()
            randomisation.save()

            donor.sequence_number = donor.retrieval_team.next_sequence_number(False)
            donor.save(created_by=request.user)

            left_kidney = donor.left_kidney
            left_kidney.transplantable = True
            left_kidney.preservation = PRESERVATION_HMPO2 if randomisation.result else PRESERVATION_HMP
            left_kidney.save(created_by=request.user)

            right_kidney = donor.right_kidney
            right_kidney.transplantable = True
            right_kidney.preservation = PRESERVATION_HMP if randomisation.result else PRESERVATION_HMPO2
            right_kidney.save(created_by=request.user)

            messages.success(request, '<strong>Offline</strong> case has been successfully started')

        return redirect(reverse(
            'wp4:compare:procurement_detail',
            kwargs={'pk': donor.id}
        ))
    else:
        print("DEBUG: donor form errors: %s" % donor_form.errors)

    # Build the list display for current cases
    if current_person.has_job(
            (StaffJob.SYSTEMS_ADMINISTRATOR, StaffJob.CENTRAL_COORDINATOR, StaffJob.NATIONAL_COORDINATOR)
    ):
        open_donors = Donor.objects.filter(procurement_form_completed=False).order_by('retrieval_team__centre_code', '-pk')
        closed_donors = Donor.objects.filter(procurement_form_completed=True).order_by('retrieval_team__centre_code', '-pk')
    elif current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
        open_donors = Donor.objects.filter(perfusion_technician=current_person, procurement_form_completed=False).order_by('-pk')
        closed_donors = []
    else:
        open_donors = []
        closed_donors = []

    return render_to_response(
        "compare/procurement_list.html",
        {
            "donor_form": donor_form,
            "open_donors": open_donors,
            "closed_donors": closed_donors,
        },
        context_instance=RequestContext(request)
    )


@permission_required('compare.change_donor')
@login_required
def procurement_form(request, pk):
    """
    Process a procurement form collection for a given donor primary key
    :param request:
    :param pk: Donor ID. We get all related information from the donor record
    :return:
    """
    all_valid = 0
    donor = get_object_or_404(Donor, pk=int(pk))
    if donor.procurement_form_completed:
        messages.error(request, 'That case has been <strong>closed</strong>.')
        return redirect(reverse('wp4:compare:procurement_list'))

    current_person = StaffPerson.objects.get(user__id=request.user.id)

    def randomise(donor, donor_form, left_organ_form, right_organ_form):
        # NB: Offline randomisations will have happened on case creation, so this is only ever online randomisations
        if not donor.is_randomised and donor.randomise():
            # Reload the forms with the modified results
            donor_form = DonorForm(instance=donor, prefix="donor")
            left_organ_form = OrganForm(instance=donor.left_kidney, prefix="left-organ")
            right_organ_form = OrganForm(instance=donor.right_kidney, prefix="right-organ")
            messages.warning(
                request,
                '<strong>This case has now been randomised!</strong> Preservation results: Left=%s and Right=%s'
                % (donor.left_kidney.get_preservation_display(), donor.right_kidney.get_preservation_display()))
        return donor_form, left_organ_form, right_organ_form


    # ================================================ DONOR
    person_form = OrganPersonForm(
        request.POST or None,
        request.FILES or None,
        instance=donor.person,
        prefix="donor-person"
    )
    if person_form.is_valid():
        person = person_form.save(request.user)
        all_valid += 1
    else:
        print("DEBUG: person form errors: %s" % person_form.errors)

    donor_form = DonorForm(request.POST or None, request.FILES or None, instance=donor, prefix="donor")
    if donor_form.is_valid():
        donor = donor_form.save(request.user)
        all_valid += 1
    else:
        print("DEBUG: donor form errors: %s" % donor_form.errors)

    # ================================================ LEFT ORGAN
    left_organ_instance = donor.left_kidney
    # print("DEBUG: left_organ_instance=%s" % left_organ_instance)
    left_organ_form = OrganForm(
        request.POST or None,
        request.FILES or None,
        instance=left_organ_instance,
        prefix="left-organ"
    )
    if left_organ_form.is_valid():
        left_organ_instance = left_organ_form.save(request.user)
        all_valid += 1

    left_organ_procurement_forms = ProcurementResourceLeftInlineFormSet(
        request.POST or None,
        prefix="left-organ-procurement",
        instance=left_organ_instance
    )
    if left_organ_procurement_forms.is_valid():
        left_organ_procurement_forms.save()
        all_valid += 1

    left_organ_error_count = left_organ_procurement_forms.total_error_count() + len(left_organ_form.errors)

    # =============================================== RIGHT ORGAN
    right_organ_instance = donor.right_kidney
    right_organ_form = OrganForm(
        request.POST or None,
        request.FILES or None,
        instance=right_organ_instance,
        prefix="right-organ"
    )
    if right_organ_form.is_valid():
        right_organ_instance = right_organ_form.save(request.user)
        all_valid += 1

    right_organ_procurement_forms = ProcurementResourceRightInlineFormSet(
        request.POST or None,
        prefix="right-organ-procurement",
        instance=right_organ_instance)
    if right_organ_procurement_forms.is_valid():
        right_organ_procurement_forms.save()
        all_valid += 1

    right_organ_error_count = right_organ_procurement_forms.total_error_count() + len(right_organ_form.errors)

    # =============================================== MESSAGES

    # print("DEBUG: all_valid=%d" % all_valid)
    if all_valid == 6:
        # This has to wait till the organ forms are saved...
        donor_form, left_organ_form, right_organ_form = randomise(donor, donor_form, left_organ_form, right_organ_form)

        if donor.procurement_form_completed:
            messages.success(request, 'Form has been successfully saved <strong>and CLOSED</strong>')
            return redirect(reverse('wp4:compare:procurement_list'))

        else:
            messages.success(request, 'Form has been <strong>successfully saved</strong>')

        # TODO: If a kidney is marked as not transplantable, we may need to write that reason into
        # the Organ not allocated reason field (and close any open T forms?)
    elif request.POST:
        donor.procurement_form_completed = False  # Can't say the form is completed if there are errors
        donor.save(created_by=request.user)

        error_count = left_organ_error_count + right_organ_error_count + len(donor_form.errors) + \
            len(person_form.errors)
        messages.error(
            request,
            '<strong>Form was NOT saved</strong>, please correct the %d %s below' % (
                error_count,
                "errors" if error_count != 1 else "error"
            )
        )

    # MESSAGES NOTES
    # messages.add_message(request, messages.INFO, 'Hello world.')
    # messages.debug(request, '%s SQL <i>statements</i> were executed.' % 5)
    # messages.info(request, 'Three credits remain in your account.')
    # messages.success(request, 'Profile details updated.')
    # messages.warning(request, 'Your account expires in three days.')
    # messages.error(request, '<strong>Document</strong> deleted.')

    # Load the relevant samples worksheet
    if len(donor.person.worksheet_set.all()) > 0:
        worksheet = donor.person.worksheet_set.all()[0]
    else:
        worksheet = None

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
            "worksheet": worksheet,
        },
        context_instance=RequestContext(request)
    )


@permission_required('compare.add_recipient')
@login_required
def transplantation_list(request):
    current_person = StaffPerson.objects.get(user__id=request.user.id)

    # Process the new case form
    allocation_form = AllocationStartForm(request.POST or None, request.FILES or None, prefix="allocation")
    if request.method == 'POST' and allocation_form.is_valid():
        organ = allocation_form.cleaned_data.get("organ")
        if allocation_form.cleaned_data.get("allocated"):
            # print("DEBUG: transplantation_list: Allocated, Yes")
            organ.save(created_by=current_person.user)  # Update the organ
            # First time in? Create an allocation record (and set the TT if user is a Perfusion Technician
            initial_organ_allocation = OrganAllocation(organ=organ, created_by=current_person.user)
            if current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
                initial_organ_allocation.perfusion_technician = current_person
            initial_organ_allocation.save()
            return redirect(reverse(
                'wp4:compare:transplantation_detail',
                kwargs={'pk': organ.id}
            ))
        else:
            # print("DEBUG: transplantation_list: Allocated, No")
            # Otherwise close the Organ record with the reason, and do nothing more
            organ.not_allocated_reason = allocation_form.cleaned_data["not_allocated_reason"]
            organ.transplantation_form_completed = True
            organ.save(created_by=current_person.user)
            allocation_form = AllocationStartForm(prefix="allocation")

    if current_person.has_job(
        (StaffJob.SYSTEMS_ADMINISTRATOR, StaffJob.CENTRAL_COORDINATOR, StaffJob.NATIONAL_COORDINATOR)
    ):
        existing_cases = Organ.open_objects.order_by('-created_on')
        closed_cases = Organ.closed_objects.order_by('-created_on')
    elif current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
        existing_cases = Organ.open_objects.filter(recipient__allocation__perfusion_technician=current_person)
        closed_cases = []
    else:
        existing_cases = []
        closed_cases = []

    organs_available_count = Organ.allocatable_objects.count()

    return render_to_response(
        "compare/transplantation_list.html",
        {
            "existing_cases": existing_cases,
            "closed_cases": closed_cases,
            "allocation_form": allocation_form,
            "organs_available_count": organs_available_count
        },
        context_instance=RequestContext(request)
    )


@permission_required('compare.change_recipient')
@login_required
def transplantation_form(request, pk=None):
    """
    :param request:
    :param pk: Organ primary key, as all recipients are tied to an organ
    :return:

    Process Allocation results until an allocation is set for one location, and then create and manage the
    Recipient record.
    """
    # print("DEBUG: Got an organ pk of %s" % pk)
    organ = get_object_or_404(Organ, pk=int(pk))
    if organ.transplantation_form_completed:
        messages.error(request, 'That case has been <strong>closed</strong>.')
        return redirect(reverse('wp4:compare:transplantation_list'))

    current_person = StaffPerson.objects.get(user__id=request.user.id)
    person_form = None
    recipient_form = None
    recipient_form_loaded = False
    errors_found = 0

    # Form metadata is on the Organ record, so we have a TransplantOrganForm
    organ_form = TransplantOrganForm(
        request.POST or None,
        request.FILES or None,
        instance=organ,
        prefix="transplant-organ"
    )
    if organ_form.is_valid():
        # TODO: WORKING HERE TO SORT OUT FORM COMPLETION

        organ = organ_form.save(request.user)
        print("DEBUG: hello organ form, nice to meet you")

    # See if we can process the forms associated with the eventual recipient
    if organ.safe_recipient is not None:
        # print("DEBUG: Starting the Recipient form")
        person_form = OrganPersonForm(
            request.POST or None,
            request.FILES or None,
            instance=organ.recipient.person,
            prefix="donor-person"
        )
        if person_form.is_valid():
            person_form.save(request.user)
        else:
            errors_found += 1
            print("DEBUG: Person Errors! %s" % person_form.errors)

        recipient_form = RecipientForm(
            request.POST or None,
            request.FILES or None,
            instance=organ.recipient,
            prefix="recipient"
        )
        if recipient_form.is_valid():
            recipient_instance = recipient_form.save()
            # Check for closing criteria
            if recipient_instance.signed_consent is False:
                no_consent_confirmed = True
                for element in recipient_form.changed_data:  # Was this changed this time, or previously?
                    if element == "signed_consent":
                        no_consent_confirmed = False

                if no_consent_confirmed:
                    organ.transplantation_form_completed = True
                    organ.save(created_by=current_person.user)
                    messages.success(
                        request,
                        'Case %s has been <strong>successfully saved and closed</strong>' % organ.trial_id
                    )
                    return redirect(reverse('wp4:compare:transplantation_list'))

                else:
                    messages.warning(
                        request,
                        "Recipient consent is required to proceed. This form will be <strong>closed upon " +
                        "the next save</strong> unless the signed consent is set to Yes"
                    )

            if recipient_instance.single_kidney_transplant is False:
                more_than_one_kidney_confirmed = True
                for element in recipient_form.changed_data:  # Was this changed this time, or previously?
                    if element == "single_kidney_transplant":
                        more_than_one_kidney_confirmed = False

                if more_than_one_kidney_confirmed:
                    organ.transplantation_form_completed = True
                    organ.save(created_by=current_person.user)
                    messages.success(
                        request,
                        'Case %s has been <strong>successfully saved and closed</strong>' % organ.trial_id
                    )
                    return redirect(reverse('wp4:compare:transplantation_list'))

                else:
                    messages.warning(
                        request,
                        "Recipient may only have one kidney transplanted in this procedure to proceed. " +
                        "This form will be <strong>closed upon " +
                        "the next save</strong> unless the single kidney transplant question is set to Yes"
                    )
        else:
            errors_found += 1
            print("DEBUG: Recipient Errors! %s" % recipient_form.errors)

        recipient_form_loaded = True

    # Process Allocations
    allocation_formset = AllocationFormSet(
        request.POST or None,
        prefix="allocation",
        queryset=organ.organallocation_set.all()
    )
    if allocation_formset.is_valid() and errors_found == 0:
        last_form_index = len(allocation_formset)-1
        for i, form in enumerate(allocation_formset):
            allocation = form.save(current_person.user)

            if i == last_form_index:
                if allocation.transplant_hospital and not allocation.transplant_hospital.is_project_site:
                    # If allocated, and to a non-project site, then we stop data collection
                    allocation_confirmed = True
                    for element in form.changed_data:  # Was this changed this time, or previously?
                        if element == "transplant_hospital":
                            allocation_confirmed = False

                    if allocation_confirmed:
                        organ.not_allocated_reason = "Allocated to a non-Project Site"
                        organ.transplantation_form_completed = True
                        organ.save(created_by=current_person.user)
                        messages.success(
                            request,
                            'Case %s has been <strong>successfully saved and closed</strong>' % organ.trial_id
                        )
                        return redirect(reverse('wp4:compare:transplantation_list'))

                    else:
                        messages.warning(
                            request,
                            "Last allocation was to a non-project hospital. This form will be <strong>closed upon " +
                            "the next save</strong> unless the transplant hospital is changed to a project site."
                        )

                elif allocation.reallocated:
                    # If this is the latest Allocation and reallocation has occurred, create a new Allocation
                    new_allocation = OrganAllocation()
                    new_allocation.organ = organ
                    new_allocation.created_by = request.user
                    if current_person.has_job(StaffJob.PERFUSION_TECHNICIAN):
                        new_allocation.perfusion_technician = current_person
                    new_allocation.save()

                    allocation.reallocation = new_allocation
                    allocation.save()

                elif allocation.reallocated is not None and not recipient_form_loaded:
                    # For a new set of objects, remember to create the Person for the Recipient first
                    person = OrganPerson()
                    person.save(created_by=current_person.user)

                    recipient = Recipient()
                    recipient.person = person
                    recipient.allocation = allocation
                    recipient.organ = organ
                    recipient.save(created_by=current_person.user)

                    # create the related sample placeholder for this recipient
                    create_recipient_worksheet(recipient, request.user)

        # There should be no errors when we redirect here
        version_string = " Organ v:" + str(organ.version) if request.user.is_superuser else ""
        messages.success(request, 'Form has been <strong>successfully saved</strong>' + version_string)
        return redirect(reverse('wp4:compare:transplantation_detail', kwargs={'pk': organ.id}))
    else:
        if request.POST:
            errors_found += 1
            print("DEBUG: allocation_formset Errors! %s" % allocation_formset.errors)
        else:
            errors_found = -1
            # Else we're just loading the page/form

    # Disable the reallocation question for any that have been saved with a reallocation value
    # for subform in allocation_formset:
    #     print("DEBUG: subform=%s" % subform.instance.reallocated)
    #     if subform.instance.reallocated is not None:
    #         subform.fields["reallocated"].disabled = True
    # TODO: FIX THIS - disabling the radiobuttons causes the fields to not be submitted, and thus the value is reset to None

    # Load the relevant samples worksheet
    if recipient_form_loaded and len(organ.recipient.person.worksheet_set.all()):
        worksheet = organ.recipient.person.worksheet_set.all()[0]
    else:
        worksheet = None

    # print("DEBUG: Second Error Message Update")
    if errors_found > 0:
        messages.error(request, 'Form has <strong>NOT</strong> been saved. Please correct the errors below')
        organ.transplantation_form_completed = False
        organ.save()
        # organ_form = TransplantOrganForm(instance=organ, prefix="transplant-organ")

    print("DEBUG: errors_found %d" % errors_found)

    return render_to_response(
        "compare/transplantation_form.html",
        {
            "allocation_formset": allocation_formset,
            "person_form": person_form,
            "recipient_form": recipient_form,
            "organ_form": organ_form,
            "organ": organ,
            "recipient_form_loaded": recipient_form_loaded,
            "worksheet": worksheet
        },
        context_instance=RequestContext(request))
