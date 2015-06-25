from django.http import HttpResponseRedirect, Http404
from django.template import RequestContext
from django.shortcuts import get_object_or_404, render, render_to_response
from django.contrib.auth.decorators import login_required
from django.views.decorators.csrf import csrf_protect
from django.core.urlresolvers import reverse
from django.shortcuts import redirect

from .models import Donor
from .forms import DonorForm, DonorStartForm, OrganForm


def error404(request):
    raise Http404("This is a page holder")

@login_required
@csrf_protect
def procurement_form(request, pk):
    donor = get_object_or_404(Donor, pk=int(pk))
    donor_form = DonorForm(instance=donor, prefix="donor")
    left_organ_form = OrganForm(instance=donor.left_kidney(), prefix="left-organ")
    right_organ_form = OrganForm(instance=donor.right_kidney(), prefix="right-organ")

    if request.method == 'POST':
        donor_form = DonorForm(request.POST, request.FILES, instance=donor, prefix="donor")
        if donor_form.is_valid():
            donor = donor_form.save(request.user)

        left_organ_form = OrganForm(request.POST, request.FILES, instance=donor.left_kidney(), prefix="left-organ")
        if left_organ_form.is_valid():
            left_organ_form.save(request.user)

        right_organ_form = OrganForm(request.POST, request.FILES, instance=donor.right_kidney(), prefix="right-organ")
        if right_organ_form.is_valid():
            right_organ_form.save(request.user)

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
    donor_form = DonorStartForm(prefix="donor")

    if request.method == 'POST':
        donor_form = DonorStartForm(request.POST, request.FILES, prefix="donor")
        if donor_form.is_valid():
            donor = donor_form.save(request.user)
            return redirect(reverse(
                'compare:procurement-detail',
                kwargs={'pk': donor.id}
            ))

    return render_to_response(
        "dashboard/procurement-start.html",
        {
            "donor_form": donor_form
        },
        context_instance=RequestContext(request)
    )


def dashboard_index(request):
    return render(request, 'dashboard/index.html', {})

