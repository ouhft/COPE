#!/usr/bin/python
# coding: utf-8
import string, random

from django.utils import formats, translation

from crispy_forms.layout import LayoutObject, Div, HTML, Field, render_field, render_to_string, TEMPLATE_PACK, flatatt


def FormPanel(title, layout, panel_status=None):
    css_status = "default"
    if panel_status is not None:
        css_status = panel_status
    return Div(
        # TODO: Work out how to i18n this later!
        Div(HTML("<h3 class=\"panel-title\">%s</h3>" % title), css_class="panel-heading"),
        Div(Div(layout, style="padding: 0 1.2em;"), css_class="panel-body"),
        css_class="panel panel-%s" % css_status
    )


def FormColumnPanel(title, layout):
    return Div(
        FormPanel(title, layout),
        css_class="col-md-6", style="margin-top: 10px;"
    )


def DateTimeField(field_name, **kwargs):
    # correct_format = formats.get_format("SHORT_DATETIME_FORMAT", lang=translation.get_language())
    # print("DEBUG: DateTimeField:correct_format=%s" % correct_format)
    # print("DEBUG: DateTimeField:kwargs=%s" % kwargs)
    # print("DEBUG: DateTimeField:translation.get_language()=%s" % translation.get_language())
    date_format_string = "DD-MM-YYYY HH:mm"
    # NB: Can't set correct date_format here because this only fires when initialised, and thus any subsequent
    # language changes are not picked up here. Also, django date format is not the same as the format used
    # by the date picker
    return Field(field_name, template="bootstrap3/layout/datetimefield.html",
                 data_date_format=date_format_string, placeholder=date_format_string, **kwargs)


def DateField(field_name, **kwargs):
    # correct_format = formats.get_format("SHORT_DATE_FORMAT", lang=translation.get_language())
    # print("DEBUG: DateField:correct_format=%s" % correct_format)
    return Field(field_name, template="bootstrap3/layout/datetimefield.html",
                 data_date_format="DD-MM-YYYY", placeholder="DD-MM-YYYY", **kwargs)


class ComboField(LayoutObject):
    template = 'bootstrap3/layout/multi-other-field.html'
    field_template1 = "bootstrap3/field.html"
    field_template2 = "bootstrap3/field.html"

    def __init__(self, primary_field, secondary_field, **kwargs):
        self.field1 = primary_field
        self.field2 = secondary_field
        self.label_html = kwargs.pop('label', u'unset')
        self.label_class = kwargs.pop('label_class', u'control-label')
        self.css_class = kwargs.pop('css_class', u'')
        self.css_id = kwargs.pop('css_id', None)
        self.template = kwargs.pop('template', self.template)
        self.field_template1 = kwargs.pop('template_field1', self.field_template1)
        self.field_template2 = kwargs.pop('template_field2', self.field_template2)
        self.flat_attrs = flatatt(kwargs)

    def render(self, form, form_style, context, template_pack=TEMPLATE_PACK):
        # If a field within MultiField contains errors
        has_errors = False
        if context['form_show_errors']:
            if self.field1 in form.errors or self.field2 in form.errors:
                has_errors = True

        if has_errors:
            if "has-error" not in self.css_class:
                self.css_class += " has-error"
            if "has-error" not in self.label_class:
                self.label_class += " has-error"

        if self.label_html == u'unset':
            try:
                field_instance = form.fields[self.field1]
                self.label_html = field_instance.label
                # self.label_for = field_instance.
                # TODO: sort out the actual primary field label id so we can put it in the html
                # print("DEBUG: label %s" % dir(field_instance))
            except KeyError:
                # If we can't find a name to use, generate a random one
                # http://stackoverflow.com/questions/2257441/random-string-generation-with-upper-case-letters-and-digits-in-python
                self.label_html = ''.join(random.SystemRandom().choice(
                    string.ascii_uppercase + string.digits) for _ in range(6))
                # Because of crispy's rendered_fields caching, we can't use the form_prefix here meaningfully

        primary_output = render_field(
            self.field1, form, form_style, context,
            self.field_template1, self.label_class, layout_object=self,
            template_pack=template_pack
        )
        secondary_output = render_field(
            self.field2, form, form_style, context,
            self.field_template2, self.label_class, layout_object=self,
            template_pack=template_pack
        )
        # print("DEBUG: self.field1=%s" % self.field1)
        # print("DEBUG: self.field2=%s" % self.field2)
        # print("DEBUG: render():form=%s" % form.fields.keys())
        # print("DEBUG: render():form=%s" % dir(form))
        # print("DEBUG: render():form.prefix=%s" % form.prefix)

        group_id = "%s-%s" % (form.prefix, self.label_html)

        context.update({
            'multifield': self,
            'primary_field': primary_output,
            'secondary_field': secondary_output,
            'group_id': group_id
        })
        return render_to_string(self.template, context)


class InlineFields(ComboField):
    template = 'bootstrap3/layout/multi-inline-field.html'
    field_template1 = "bootstrap3/multi-inline-field.html"
    field_template2 = "bootstrap3/multi-inline-field.html"


class FieldWithFollowup(ComboField):
    template = 'bootstrap3/layout/multi-other-field.html'
    # Presumes the last value in the select list is the "other" option


class FieldWithNotKnown(ComboField):
    template = 'bootstrap3/layout/multi-notknown-field.html'
    field_template1 = "bootstrap3/multi-notknown-field.html"
    field_template2 = "bootstrap3/multi-notknown-field.html"


class YesNoFieldWithAlternativeFollowups(LayoutObject):
    template = 'bootstrap3/layout/multi-alternative-field.html'
    field_template1 = "bootstrap3/layout/radioselect-buttons.html"
    field_template2 = "bootstrap3/field.html"
    field_template3 = "bootstrap3/field.html"

    def __init__(self, primary_field, secondary_field, tertiary_field, **kwargs):
        self.field1 = primary_field
        self.field2 = secondary_field
        self.field3 = tertiary_field
        self.label_html = kwargs.pop('label', u'unset')
        self.label_class = kwargs.pop('label_class', u'control-label')
        self.css_class = kwargs.pop('css_class', u'')
        self.css_id = kwargs.pop('css_id', None)
        self.template = kwargs.pop('template', self.template)
        self.field_template1 = kwargs.pop('template_field1', self.field_template1)
        self.field_template2 = kwargs.pop('template_field2', self.field_template2)
        self.field_template3 = kwargs.pop('template_field2', self.field_template3)
        self.flat_attrs = flatatt(kwargs)

    def render(self, form, form_style, context, template_pack=TEMPLATE_PACK):
        # If a field within MultiField contains errors
        has_errors = False
        if context['form_show_errors']:
            if self.field1 in form.errors or self.field2 in form.errors or self.field3 in form.errors:
                has_errors = True

        if has_errors:
            if "has-error" not in self.css_class:
                self.css_class += " has-error"
            if "has-error" not in self.label_class:
                self.label_class += " has-error"

        if self.label_html == u'unset':
            try:
                field_instance = form.fields[self.field1]
                self.label_html = field_instance.label
            except KeyError:
                # If we can't find a name to use, generate a random one
                # http://stackoverflow.com/questions/2257441/random-string-generation-with-upper-case-letters-and-digits-in-python
                self.label_html = ''.join(random.SystemRandom().choice(
                    string.ascii_uppercase + string.digits) for _ in range(6))
                # Because of crispy's rendered_fields caching, we can't use the form_prefix here meaningfully

        primary_output = render_field(
            self.field1, form, form_style, context,
            self.field_template1, self.label_class, layout_object=self,
            template_pack=template_pack
        )
        secondary_output = render_field(
            self.field2, form, form_style, context,
            self.field_template2, self.label_class, layout_object=self,
            template_pack=template_pack
        )
        tertiary_output = render_field(
            self.field3, form, form_style, context,
            self.field_template3, self.label_class, layout_object=self,
            template_pack=template_pack
        )

        context.update({
            'multifield': self,
            'primary_field': primary_output,
            'secondary_field': secondary_output,
            'tertiary_field': tertiary_output
        })
        return render_to_string(self.template, context)


def ForeignKeyModal(field_name, **kwargs):
    return Field(field_name, template="bootstrap3/foreign-key-modal.html", **kwargs)


class AjaxReturnIDMixin(object):
    def get_context_data(self, **kwargs):
        # Get the DOM id from the request data on return_id and make available to template
        context = super(AjaxReturnIDMixin, self).get_context_data(**kwargs)
        context['return_id'] = self.request.GET.get("return_id")
        return context