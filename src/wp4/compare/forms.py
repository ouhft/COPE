from django import forms
from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, Submit, Div, Fieldset, HTML, Field
from crispy_forms.bootstrap import FormActions
from .models import Donor, Organ, YES_NO_UNKNOWN_CHOICES
from django.utils import timezone
from django.contrib.auth.forms import AuthenticationForm
from django.utils.translation import ugettext_lazy as _, ungettext_lazy as __
from wp4.settings import LOGIN_REDIRECT_URL

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

YES_NO_CHOICES = (
    (False, _("No")),
    (True, _("Yes"))
)


def bootstrap_panel(title, layout):
    return Div(
        Div(
            # TODO: Work out how to i18n this later!
            Div(HTML("<h3 class=\"panel-title\">%s</h3>" % title), css_class="panel-heading"),
            Div(Div(layout, style="padding: 10px;"), css_class="panel-body"),
            css_class="panel panel-default"
        ), css_class="col-md-6", style="margin-top: 10px;"
    )


def bootstrap_datetimefield(field_name):
    return Field(field_name, template="bootstrap3/layout/datetimefield.html",
        data_date_format="DD-MM-YYYY HH:mm", placeholder="DD-MM-YYYY HH:mm")


def bootstrap_datefield(field_name):
    return Field(field_name, template="bootstrap3/layout/datetimefield.html",
        data_date_format="DD-MM-YYYY", placeholder="DD-MM-YYYY")


class DonorForm(forms.ModelForm):
    layout_1 = Layout(
        'retrieval_team',
        'sequence_number',  # TODO: Work out how to hide this field if not admin
        'perfusion_technician',  # TODO: Make this field read only
        'transplant_coordinator',
        bootstrap_datetimefield('call_received'),
        'retrieval_hospital',
        bootstrap_datetimefield('scheduled_start'),  # -- Not needed for UK, identical to withdrawal
        bootstrap_datetimefield('technician_arrival'),  # -- Not needed for UK, identical to withdrawal
        bootstrap_datetimefield('ice_boxes_filled'),  # -- Not needed for UK, identical to withdrawal
        bootstrap_datetimefield('depart_perfusion_centre'),
        bootstrap_datetimefield('arrival_at_donor_hospital')
    )
    layout_2 = Layout(
        Field('number', placeholder="___ ___ ____"),
        bootstrap_datefield('date_of_birth'),
        'age',
        bootstrap_datefield('date_of_admission'),
        Field('admitted_to_itu', template="bootstrap3/layout/radioselect-buttons.html"),
        bootstrap_datefield('date_admitted_to_itu'),
        bootstrap_datefield('date_of_procurement'),
        Field('gender', template="bootstrap3/layout/radioselect-buttons.html"),
        'weight', 'height',
        Field('ethnicity', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('blood_group', template="bootstrap3/layout/radioselect-buttons.html")
    )
    layout_3 = Layout(
        'diagnosis', 'diagnosis_other',
        Field('diabetes_melitus', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('alcohol_abuse', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('cardiac_arrest', template="bootstrap3/layout/radioselect-buttons.html"),
        'systolic_blood_pressure', 'diastolic_blood_pressure', 'diuresis_last_day', 'diuresis_last_day_unknown',
        'diuresis_last_hour', 'diuresis_last_hour_unknown',
        Field('dopamine', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('dobutamine', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('nor_adrenaline', template="bootstrap3/layout/radioselect-buttons.html"),
        Field('vasopressine', template="bootstrap3/layout/radioselect-buttons.html"),
        'other_medication_details',
    )
    layout_4 = Layout(
        'last_creatinine', 'last_creatinine_unit', 'max_creatinine', 'max_creatinine_unit',
        bootstrap_datetimefield('life_support_withdrawal'),
        bootstrap_datetimefield('systolic_pressure_low'),
        bootstrap_datetimefield('o2_saturation'),
        bootstrap_datetimefield('circulatory_arrest'),
        'length_of_no_touch',
        bootstrap_datetimefield('death_diagnosed'),
        bootstrap_datetimefield('perfusion_started'),
        Field('systemic_flush_used', template="bootstrap3/layout/radioselect-buttons.html"),
        'systemic_flush_used_other',
        Field('heparin', template="bootstrap3/layout/radioselect-buttons.html"),
    )
    layout_5 = Layout(
        'donor_blood_1_EDTA', 'donor_blood_1_SST', 'donor_urine_1', 'donor_urine_2',
    )

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        bootstrap_panel("Procedure Data", layout_1),
        bootstrap_panel("Donor Details", layout_2),
        bootstrap_panel("Donor Preop Data", layout_3),
        bootstrap_panel("Lab Results", layout_4),
        bootstrap_panel("Sampling Data", layout_5),
        Div(css_class="clearfix")
    )

    def __init__(self, *args, **kwargs):
        super(DonorForm, self).__init__(*args, **kwargs)
        self.fields['retrieval_team'].widget = forms.HiddenInput()
        self.fields['sequence_number'].widget = forms.HiddenInput()
        self.fields['call_received'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['scheduled_start'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['technician_arrival'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['ice_boxes_filled'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['depart_perfusion_centre'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['arrival_at_donor_hospital'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['number'].required = False
        self.fields['date_of_birth'].input_formats = DATE_INPUT_FORMATS
        self.fields['date_of_admission'].input_formats = DATE_INPUT_FORMATS
        self.fields['admitted_to_itu'].choices = YES_NO_CHOICES
        self.fields['date_admitted_to_itu'].input_formats = DATE_INPUT_FORMATS
        self.fields['date_of_procurement'].input_formats = DATE_INPUT_FORMATS
        self.fields['gender'].choices = Donor.GENDER_CHOICES
        self.fields['ethnicity'].choices = Donor.ETHNICITY_CHOICES
        self.fields['blood_group'].choices = Donor.BLOOD_GROUP_CHOICES
        self.fields['diabetes_melitus'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['alcohol_abuse'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['cardiac_arrest'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['dopamine'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['dobutamine'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['nor_adrenaline'].choices = YES_NO_UNKNOWN_CHOICES
        self.fields['vasopressine'].choices = YES_NO_UNKNOWN_CHOICES

        self.fields['life_support_withdrawal'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['systolic_pressure_low'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['o2_saturation'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['circulatory_arrest'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['death_diagnosed'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['perfusion_started'].input_formats = DATETIME_INPUT_FORMATS
        self.fields['systemic_flush_used'].choices = Donor.FLUSH_SOLUTION_CHOICES
        self.fields['heparin'].choices = YES_NO_UNKNOWN_CHOICES
        # self.fields[''].

    class Meta:
        model = Donor
        # fields = ('__all__')
        exclude = ['created_by', 'version', 'created_on']
        localized_fields = ('__all__')

    # https://docs.djangoproject.com/en/1.8/topics/forms/modelforms/#overriding-the-default-fields
    labels = {}
    help_texts = {}
    error_messages = {}

    def save(self, user):
        donor = super(DonorForm, self).save(commit=False)
        donor.created_by = user
        donor.created_on = timezone.now()
        donor.version += 1
        donor.save()
        return donor

class DonorStartForm(forms.ModelForm):
    # gender = CharField(required=True, widget=RadioSelect(choices=Donor.GENDER_CHOICES))

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        Div('retrieval_team', css_class="col-md-6"),
        Div('perfusion_technician', css_class="col-md-6"),
        Div('age', css_class="col-md-6"),
        Div('gender', css_class="col-md-6"),
    )

    class Meta:
        model = Donor
        fields = ['retrieval_team', 'perfusion_technician', 'age', 'gender']
        localized_fields = ('__all__')

    def save(self, user):
        donor = super(DonorStartForm, self).save(commit=False)
        donor.created_by = user
        donor.created_on = timezone.now()
        donor.version = 1
        donor.save()
        return donor

class OrganForm(forms.ModelForm):
    removal = forms.DateTimeField(widget=forms.DateTimeInput(), input_formats=DATETIME_INPUT_FORMATS,
        required=False)
    perfusion_started = forms.DateTimeField(widget=forms.DateTimeInput(), input_formats=DATETIME_INPUT_FORMATS,
        required=False)
    oxygen_bottle_changed_at = forms.DateTimeField(widget=forms.DateTimeInput(),
        input_formats=DATETIME_INPUT_FORMATS,
        required=False)
    ice_container_replenished_at = forms.DateTimeField(widget=forms.DateTimeInput(),
        input_formats=DATETIME_INPUT_FORMATS,
        required=False)

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        Div(
            Div(
                Div(
                    HTML("<h3 class=\"panel-title\">Hidden Data</h3>"),  # TODO: Work out how to i18n this later!
                    css_class="panel-heading"
                ),
                Div(
                    Div(
                        'donor', 'location', 'preservation',
                        style="padding: 10px;"
                    ),
                    css_class="panel-body"
                ),
                css_class="panel panel-default"
            ),
            css_class="col-md-6",
            style="margin-top: 10px;"
        ),

        Div(
            Div(
                Div(
                    HTML("<h3 class=\"panel-title\">Sampling Data</h3>"),  # TODO: Work out how to i18n this later!
                    css_class="panel-heading"
                ),
                Div(
                    Div(
                        'perfusate_1', 'perfusate_2',
                        style="padding: 10px;"
                    ),
                    css_class="panel-body"
                ),
                css_class="panel panel-default"
            ),
            css_class="col-md-6",
            style="margin-top: 10px;"
        ),

        Div(css_class="clearfix"),

        Div(
            Div(
                Div(
                    HTML("<h3 class=\"panel-title\">Inspection</h3>"),  # TODO: Work out how to i18n this later!
                    css_class="panel-heading"
                ),
                Div(
                    Div(
                        Field('removal', template="bootstrap3/layout/datetimefield.html",
                            data_date_format="DD-MM-YYYY HH:mm", placeholder="DD-MM-YYYY HH:mm"),
                        'renal_arteries', 'graft_damage', 'washout_perfusion', 'transplantable',
                        'not_transplantable_reason',
                        style="padding: 10px;"
                    ),
                    css_class="panel-body"
                ),
                css_class="panel panel-default"
            ),
            css_class="col-md-6",
            style="margin-top: 10px;"
        ),

        Div(
            Div(
                Div(
                    HTML("<h3 class=\"panel-title\">Perfusion</h3>"),  # TODO: Work out how to i18n this later!
                    css_class="panel-heading"
                ),
                Div(
                    Div(
                        'perfusion_possible', 'perfusion_not_possible_because',
                        Field('perfusion_started', template="bootstrap3/layout/datetimefield.html",
                            data_date_format="DD-MM-YYYY HH:mm", placeholder="DD-MM-YYYY HH:mm"),
                        'patch_holder',
                        'artificial_patch_used', 'artificial_patch_size', 'artificial_patch_number',
                        'oxygen_bottle_full',
                        'oxygen_bottle_open', 'oxygen_bottle_changed',
                        Field('oxygen_bottle_changed_at', template="bootstrap3/layout/datetimefield.html",
                            data_date_format="DD-MM-YYYY HH:mm", placeholder="DD-MM-YYYY HH:mm"),
                        'ice_container_replenished',
                        Field('ice_container_replenished_at', template="bootstrap3/layout/datetimefield.html",
                            data_date_format="DD-MM-YYYY HH:mm", placeholder="DD-MM-YYYY HH:mm"),
                        'perfusate_measurable', 'perfusate_measure',
                        'perfusion_machine',
                        'perfusion_file',
                        style="padding: 10px;"
                    ),
                    css_class="panel-body"
                ),
                css_class="panel panel-default"
            ),
            css_class="col-md-6",
            style="margin-top: 10px;"
        ),
    )

    class Meta:
        model = Organ
        fields = [
            'donor', 'location',

            'removal', 'renal_arteries', 'graft_damage', 'washout_perfusion', 'transplantable',
            'not_transplantable_reason',

            'preservation',

            'perfusion_possible', 'perfusion_not_possible_because', 'perfusion_started', 'patch_holder',
            'artificial_patch_used', 'artificial_patch_size', 'artificial_patch_number', 'oxygen_bottle_full',
            'oxygen_bottle_open', 'oxygen_bottle_changed', 'oxygen_bottle_changed_at', 'ice_container_replenished',
            'ice_container_replenished_at', 'perfusate_measurable', 'perfusate_measure', 'perfusion_machine',
            'perfusion_file',

            'perfusate_1', 'perfusate_2',
        ]

    def save(self, user):
        organ = super(OrganForm, self).save(commit=False)
        organ.created_by = user
        organ.created_on = timezone.now()
        organ.version += 1
        organ.save()
        return organ

class LoginForm(AuthenticationForm):
    next = forms.CharField(
        initial=LOGIN_REDIRECT_URL,
        widget=forms.HiddenInput()
    )

    helper = FormHelper()
    helper.layout = Layout(
        'username',
        'password',
        'next',
        FormActions(
            Submit('login', 'Login', css_class='btn-primary')
        )
    )

    class Meta:
        fields = '__all__'

    def __init__(self, *args, **kwargs):
        super(LoginForm, self).__init__(*args, **kwargs)
