#!/usr/bin/python
# coding: utf-8

from django.contrib.auth.decorators import login_required
from django.core.urlresolvers import reverse, resolve
from django.http import Http404, HttpResponseRedirect
from django.shortcuts import get_object_or_404, render_to_response
from django.template import RequestContext
from django.views.decorators.csrf import csrf_protect
# from django.views.generic import ListView, CreateView, UpdateView, DetailView

from vanilla import ListView, CreateView, UpdateView, DetailView
from braces.views import LoginRequiredMixin

from wp4.compare.models import Recipient
from wp4.staff_person.models import StaffPerson

from .models import FollowUpInitial, FollowUp3M, FollowUp6M, FollowUp1Y
from .forms import FollowUpInitialForm, FollowUpDayInlineFormSet, FollowUp3MForm


class FollowUpList(LoginRequiredMixin, ListView):
    # List all Organs, that have successfully been transplanted, by date of transplantation
    # NB: Template is compare/recipient_list.html
    model = Recipient
    queryset = Recipient.objects.filter(successful_conclusion=True).order_by('operation_concluded_at')


class FollowUpInitialList(LoginRequiredMixin, ListView):
    model = FollowUpInitial
    context_object_name = 'followup_list'


class FollowUpInitialDetail(LoginRequiredMixin, DetailView):
    model = FollowUpInitial
    context_object_name = 'followup_obj'


@login_required
def follow_up_initial_update(request, pk=None):
    current_person = StaffPerson.objects.get(user__id=request.user.id)

    initial_object = get_object_or_404(FollowUpInitial, pk=int(pk))

    daily_formset = FollowUpDayInlineFormSet(
        request.POST or None,
        prefix="daily",
        initial=[
            initial_object.day1(),
            initial_object.day2(),
            initial_object.day3(),
            initial_object.day4(),
            initial_object.day5(),
            initial_object.day6(),
            initial_object.day7(),
        ]
    )
    if daily_formset.is_valid():
        last_form_index = len(daily_formset)-1
        for i, day_form in enumerate(daily_formset):
            # Now we have to map the days into the one model
            if i == 0:
                initial_object.day1(
                    recipient_alive=day_form.cleaned['recipient_alive']
                )

            if i == last_form_index and i < 7 and day_form.recipient_alive:
                pass

    initial_form = FollowUpInitialForm(
        request.POST or None,
        request.FILES or None,
        instance=initial_object,
        prefix="initial"
    )
    if initial_form.is_valid():
        initial_object = initial_form.save(current_person.user)

    return render_to_response(
        "followups/followupinitial_form.html",
        {
            "form": initial_form,
            "daily_forms": daily_formset,
            "initial_obj": initial_object
        },
        context_instance=RequestContext(request)
    )



class FollowUp3MList(LoginRequiredMixin, ListView):
    model = FollowUp3M
    context_object_name = 'followup_list'


class FollowUp3MDetail(LoginRequiredMixin, DetailView):
    model = FollowUp3M
    context_object_name = 'followup_obj'


class FollowUp3MUpdate(LoginRequiredMixin, UpdateView):
    model = FollowUp3M
    form_class = FollowUp3MForm
    context_object_name = 'followup_obj'

    def form_valid(self, form):
        instance = form.save(self.request.user)
        return HttpResponseRedirect(instance.get_absolute_url())
