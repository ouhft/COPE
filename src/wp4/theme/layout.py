from crispy_forms import layout
import string, random


class ComboField(layout.LayoutObject):
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
        self.flat_attrs = layout.flatatt(kwargs)

    def render(self, form, form_style, context, template_pack=layout.TEMPLATE_PACK):
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

        primary_output = layout.render_field(
            self.field1, form, form_style, context,
            self.field_template1, self.label_class, layout_object=self,
            template_pack=template_pack
        )
        secondary_output = layout.render_field(
            self.field2, form, form_style, context,
            self.field_template2, self.label_class, layout_object=self,
            template_pack=template_pack
        )

        context.update({'multifield': self, 'primary_field': primary_output, 'secondary_field': secondary_output})
        return layout.render_to_string(self.template, context)


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


class YesNoFieldWithAlternativeFollowups(layout.LayoutObject):
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
        self.flat_attrs = layout.flatatt(kwargs)

    def render(self, form, form_style, context, template_pack=layout.TEMPLATE_PACK):
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

        primary_output = layout.render_field(
            self.field1, form, form_style, context,
            self.field_template1, self.label_class, layout_object=self,
            template_pack=template_pack
        )
        secondary_output = layout.render_field(
            self.field2, form, form_style, context,
            self.field_template2, self.label_class, layout_object=self,
            template_pack=template_pack
        )
        tertiary_output = layout.render_field(
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
        return layout.render_to_string(self.template, context)
