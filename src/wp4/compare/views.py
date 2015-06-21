from django.http import HttpResponse, Http404
from django.views.generic.edit import CreateView, UpdateView, DeleteView
from django.core.urlresolvers import reverse_lazy

from django.shortcuts import get_object_or_404, render, render_to_response
from django.http import HttpResponseRedirect
from django.core.urlresolvers import reverse
from django.views import generic
from django.contrib.auth.decorators import login_required

from .models import Donor, Person
from .forms import DonorForm, OrganForm

# class PersonIndexView(generic.ListView):
#     template_name = 'person/index.html'
#     context_object_name = 'people_list'
#
#     def get_queryset(self):
#         return Person.objects.order_by('-created_on')
#
#
# class PersonDetailView(generic.DetailView):
#     model = Person
#     template_name = 'person/detail.html'
#
#
# class PersonResultsView(generic.DetailView):
#     model = Person
#     template_name = 'person/results.html'
#
#
# class PersonCreate(CreateView):
#     model = Person
#     template_name = 'person/form.html'
#     fields = ['first_names', 'last_names', 'job', 'telephone', 'user']
#
#     def form_valid(self, form):
#         form.instance.created_by = self.request.user
#         return super(PersonCreate, self).form_valid(form)
#
#
# class PersonUpdate(UpdateView):
#     model = Person
#     template_name = 'person/form.html'
#     fields = ['first_names', 'last_names', 'job', 'telephone', 'user']
#
#
# class PersonDelete(DeleteView):
#     model = Person
#     success_url = reverse_lazy('person_index')



def error404(request):
    raise Http404("This is a page holder")

@login_required
def procurement_form(request):
    donor_form = DonorForm(prefix="donor")
    left_organ_form = OrganForm(prefix="left-organ")
    right_organ_form = OrganForm(prefix="right-organ")

    if request.method == 'POST':
        donor_form = DonorForm(request.POST, request.FILES)
        if donor_form.is_valid():
            donor_form.save()
            # do something.

    return render_to_response("dashboard/procurement.html", {
        "donor_form": donor_form,
        "left_organ_form": left_organ_form,
        "right_organ_form": right_organ_form
    })


def dashboard_index(request):
    return render(request, 'dashboard/index.html', {})

