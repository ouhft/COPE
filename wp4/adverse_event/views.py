#!/usr/bin/python
# coding: utf-8

from django.contrib import messages
from django.views.generic import ListView, CreateView, UpdateView, DetailView

from braces.views import LoginRequiredMixin

from .models import AdverseEvent
from .forms import AdverseEventForm


# ============================================  MIXINS
class AjaxFormMixin(object):
    form_class = AdverseEventForm

    def form_valid(self, form):
        form.instance.created_by = self.request.user
        if self.request.is_ajax():
            self.object = form.save()
            return self.render_to_response(self.get_context_data(form=form))
        else:
            return super(AjaxFormMixin, self).form_valid(form)

    def form_invalid(self, form):
        print("DEBUG: form_invalid() errors: %s" % form.errors)
        error_count = len(form.errors)
        error_pluralise = "" if error_count == 1 else "s"
        messages.error(
            self.request,
            '<strong>Form was NOT saved</strong>, please correct the %d error%s below' %
            (error_count, error_pluralise)
        )
        return super(AjaxFormMixin, self).form_invalid(form)

    # def get_form(self, form_class=None):
    #     form = super(AjaxFormMixin, self).get_form(form_class)
    #     # Both post() and get() call get_form() first, so this is best place to intercept ajax changes
    #     if self.request.is_ajax():
    #         form.fields['user'].widget = forms.HiddenInput()
    #         form.fields['jobs'].widget = forms.HiddenInput()
    #         self.template_name = "staff_person/staffperson_form.ajax.html"
    #     return form


# ============================================  CBVs
class AdverseEventListView(LoginRequiredMixin, ListView):
    model = AdverseEvent


class AdverseEventDetailView(LoginRequiredMixin, DetailView):
    model = AdverseEvent


class AdverseEventCreateView(AjaxFormMixin, LoginRequiredMixin, CreateView):
    model = AdverseEvent


class AdverseEventUpdateView(AjaxFormMixin, LoginRequiredMixin, UpdateView):
    model = AdverseEvent
