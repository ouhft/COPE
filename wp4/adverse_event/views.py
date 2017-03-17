#!/usr/bin/python
# coding: utf-8

from django.conf import settings
from django.contrib import messages
from django.core.mail import EmailMessage
from django.http import HttpResponseRedirect
from django.views.generic import ListView, CreateView, UpdateView, DetailView

from braces.views import LoginRequiredMixin, PermissionRequiredMixin, OrderableListMixin

from wp4.staff.utils import get_emails_from_ids
from wp4.staff.utils import JACQUES_PIREENE, INA_JOCHMANS, SARAH_MERTENS, ALLY_BRADLEY
from .models import Event
from .forms import EventForm


# ============================================  MIXINS

class EmailOnSaveMixin(object):
    def form_valid(self, form):
        """
        Override the ModelFormMixin and FormMixin form_valid methods, by inserting the email sending between them

        :param form:
        :return:
        """
        # From ModelFormMixin.form_valid()
        self.object = form.save()

        # Create and send the email if this is serious (Issue #155)
        if self.object.is_serious:
            message_text = "Visit https://{0}{1} for more details. The Local Investigator is {2} who has been " + \
                "emailed at [{3}].".format(
                    self.request.get_host(),
                    self.object.get_absolute_url(),
                    self.object.contact.full_name,
                    self.object.contact.email if self.object.contact.email is not None else "Address Unknown"
                )
            send_to = [
                JACQUES_PIREENE,
                INA_JOCHMANS,
                SARAH_MERTENS,
                self.object.contact.email if self.object.contact else 0
            ]
            cc_to = [ALLY_BRADLEY, ]
            subject_text = "Serious Adverse Event Updated - {0}".format(self.object.organ.trial_id)
            from_email = settings.DEFAULT_FROM_EMAIL
            email = EmailMessage(
                subject=subject_text,
                body=message_text,
                from_email=from_email,
                to=get_emails_from_ids(send_to),
                cc=get_emails_from_ids(cc_to)
            )
            email.send()

        # From FormMixin.form_valid()
        return HttpResponseRedirect(self.get_success_url())


class AjaxFormMixin(object):
    form_class = EventForm

    def form_valid(self, form):
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


# ============================================  CBVs
class AdverseEventListView(LoginRequiredMixin, PermissionRequiredMixin, OrderableListMixin, ListView):
    model = Event
    permission_required = "adverse_event.add_event"
    ordering = ['onset_at_date']
    paginate_by = 50
    paginate_orphans = 5
    orderable_columns = ("id", "onset_at_date", "death", "event_ongoing")
    orderable_columns_default = "onset_at_date"


class AdverseEventDetailView(LoginRequiredMixin, PermissionRequiredMixin, DetailView):
    model = Event
    permission_required = "adverse_event.add_event"


class AdverseEventCreateView(LoginRequiredMixin, PermissionRequiredMixin, AjaxFormMixin, EmailOnSaveMixin, CreateView):
    model = Event
    permission_required = "adverse_event.add_event"


class AdverseEventUpdateView(LoginRequiredMixin, PermissionRequiredMixin, AjaxFormMixin, EmailOnSaveMixin, UpdateView):
    model = Event
    permission_required = "adverse_event.add_event"

