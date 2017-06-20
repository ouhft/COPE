#!/usr/bin/python
# coding: utf-8

from django.conf import settings
from django.contrib import messages
from django.core.mail import EmailMessage
from django.core.urlresolvers import reverse
from django.core.validators import validate_email, ValidationError
from django.http import HttpResponseRedirect
from django.views.generic import ListView, CreateView, UpdateView, DetailView
from django.views.generic.edit import FormMixin
from django.utils import six

from braces.views import LoginRequiredMixin, MultiplePermissionsRequiredMixin, OrderableListMixin

from wp4.staff.utils import get_emails_from_ids
from wp4.staff.utils import JACQUES_PIREENE, INA_JOCHMANS, SARAH_MERTENS, ALLY_BRADLEY
from .models import Event
from .forms import EventForm, AdminEventForm, EventStartForm


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
            valid_email = True
            try:
                validate_email(self.object.contact.email)
            except ValidationError:
                valid_email = False

            message_text = "Visit https://{0}{1} for more details. ".format(
                    self.request.get_host(),
                    self.object.get_absolute_url()
            )
            message_text += "The Local Investigator is {0} ".format(
                self.object.contact.get_full_name()
            )
            if valid_email:
                message_text += "and has been emailed at {0}.".format(
                    self.object.contact.email
                )
            else:
                message_text += "but HAS NOT been notified due to a missing or invalid email address for them."

            send_to = [
                JACQUES_PIREENE,
                INA_JOCHMANS,
                SARAH_MERTENS,
                self.object.contact.id if self.object.contact else 0
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
    def form_valid(self, form):
        if self.request.is_ajax():
            self.object = form.save()
            return self.render_to_response(self.get_context_data(form=form))
        else:
            messages.success(
                self.request,
                'Form was <strong>saved SUCCESSFULLY</strong>, please review it below'
            )
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

    def get_form_class(self):
        """
        If this is an admin user, we want to display the categories, unless this is a form being created
        :return:
        """
        if self.request.user.is_administrator and self.form_class is not EventStartForm:
            return AdminEventForm
        return super(AjaxFormMixin, self).get_form_class()

    def get_success_url(self):
        if self.form_class is EventStartForm:
            return reverse('wp4:adverse_event:update', args=(self.object.id,))
        else:
            return super(AjaxFormMixin, self).get_success_url()


class UserBasedQuerysetMixin(object):
    """
    Ensure that the queries we run include the current user to allow for the filtering and permissions to take hold
    """

    def get_queryset(self):
        queryset = self.model.objects.for_user(self.request.user).all()

        try:
            ordering = self.get_ordering()
            if ordering:
                if isinstance(ordering, six.string_types):
                    ordering = (ordering, )
                queryset = queryset.order_by(*ordering)
        except AttributeError:
            # get_ordering doesn't apply to nonlists, but this queryset does
            pass

        return queryset


# ============================================  CBVs
class AdverseEventListView(LoginRequiredMixin, MultiplePermissionsRequiredMixin,
                           OrderableListMixin, UserBasedQuerysetMixin,
                           ListView):
    model = Event
    permissions = {
        "all": (),
        "any": ("adverse_event.change_event", "adverse_event.view_event"),
    }
    paginate_by = 50
    paginate_orphans = 5
    orderable_columns = (
        "id",
        "organ__trial_id",
        "onset_at_date",
        "death",
        "event_ongoing",
        "treatment_related",
        "contact"
    )
    orderable_columns_default = "onset_at_date"


class AdverseEventDetailView(LoginRequiredMixin, MultiplePermissionsRequiredMixin,
                             UserBasedQuerysetMixin,
                             DetailView):
    model = Event
    permissions = {
        "all": (),
        "any": ("adverse_event.change_event", "adverse_event.view_event"),
    }


class AdverseEventCreateView(LoginRequiredMixin, MultiplePermissionsRequiredMixin,
                             AjaxFormMixin, EmailOnSaveMixin,
                             CreateView):
    model = Event
    permissions = {
        "all": ("adverse_event.add_event", ),
        "any": (),
    }
    form_class = EventStartForm


class AdverseEventUpdateView(LoginRequiredMixin, MultiplePermissionsRequiredMixin,
                             AjaxFormMixin, EmailOnSaveMixin, UserBasedQuerysetMixin,
                             UpdateView):
    model = Event
    permissions = {
        "all": ("adverse_event.change_event", ),
        "any": (),
    }

