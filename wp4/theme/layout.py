#!/usr/bin/python
# coding: utf-8
import string, random

from crispy_forms.layout import LayoutObject, Div, HTML, Field, render_field, render_to_string, TEMPLATE_PACK, flatatt


DATETIME_INPUT_FORMATS = [
    '%d-%m-%Y %H:%M',  # '25-10-2006 14:30'
    '%Y-%m-%d %H:%M',  # '2006-10-25 14:30'
    '%d/%m/%Y %H:%M',  # '25/10/2006 14:30'
    '%Y/%m/%d %H:%M',  # '2006/10/25 14:30'
]

DATE_INPUT_FORMATS = [
    '%d-%m-%Y',  # '25-10-2006'
    '%Y-%m-%d',  # '2006-10-25'
    '%d/%m/%Y',  # '25/10/2006'
    '%Y/%m/%d',  # '2006/10/25'
]


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
    return Field(field_name, template="bootstrap3/layout/datetimefield.html",
                 data_date_format="DD-MM-YYYY HH:mm", placeholder="DD-MM-YYYY HH:mm", **kwargs)


def DateField(field_name, **kwargs):
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

        context.update({'multifield': self, 'primary_field': primary_output, 'secondary_field': secondary_output})
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
