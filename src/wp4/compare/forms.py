from django.forms import ModelForm, RadioSelect, CharField, HiddenInput, DateTimeField, DateField, DateTimeInput
from crispy_forms.helper import FormHelper
from crispy_forms.layout import Layout, Submit, Div, Fieldset, HTML, Field
from crispy_forms.bootstrap import FormActions
from .models import Donor, Organ
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


class DonorForm(ModelForm):
    # retrieval_team = CharField(widget=HiddenInput())
    # perfusion_technician = CharField(widget=HiddenInput())
    # TODO: Restore the now-missing verbose names from the models
    call_received = DateTimeField(widget=DateTimeInput(), input_formats=DATETIME_INPUT_FORMATS, required=False)
    scheduled_start = DateTimeField(widget=DateTimeInput(), input_formats=DATETIME_INPUT_FORMATS, required=False)
    technician_arrival = DateTimeField(widget=DateTimeInput(), input_formats=DATETIME_INPUT_FORMATS, required=False)
    ice_boxes_filled = DateTimeField(widget=DateTimeInput(), input_formats=DATETIME_INPUT_FORMATS, required=False)
    depart_perfusion_centre = DateTimeField(
        widget=DateTimeInput(),
        input_formats=DATETIME_INPUT_FORMATS,
        required=False
    )
    arrival_at_donor_hospital = DateTimeField(
        widget=DateTimeInput(),
        input_formats=DATETIME_INPUT_FORMATS,
        required=False
    )

    number = CharField(required=False, label=_('NHSBT Number'))

    life_support_withdrawal = DateTimeField(widget=DateTimeInput(), input_formats=DATETIME_INPUT_FORMATS, required=False)
    systolic_pressure_low = DateTimeField(widget=DateTimeInput(), input_formats=DATETIME_INPUT_FORMATS, required=False)
    o2_saturation = DateTimeField(widget=DateTimeInput(), input_formats=DATETIME_INPUT_FORMATS, required=False)
    circulatory_arrest = DateTimeField(widget=DateTimeInput(), input_formats=DATETIME_INPUT_FORMATS, required=False)
    death_diagnosed = DateTimeField(widget=DateTimeInput(), input_formats=DATETIME_INPUT_FORMATS, required=False)
    perfusion_started = DateTimeField(widget=DateTimeInput(), input_formats=DATETIME_INPUT_FORMATS, required=False)

    layout_1 = Layout(
        Div(
            Field('retrieval_team'),  # TODO: Work out how to hide this field
            Field('sequence_number'),  # TODO: Work out how to hide this field if not admin
            Field('perfusion_technician'),  # TODO: Work out how to hide this field
            'transplant_coordinator',
            Field('call_received', template="bootstrap3/layout/datetimefield.html",
                data_date_format="DD-MM-YYYY HH:mm", placeholder="DD-MM-YYYY HH:mm"),
            'retrieval_hospital',
            Field(
                'scheduled_start',   # -- Not needed for UK, identical to withdrawal
                template="bootstrap3/layout/datetimefield.html",
                data_date_format="DD-MM-YYYY HH:mm",
                placeholder="DD-MM-YYYY HH:mm"
            ),
            Field(
                'technician_arrival',   # -- Not needed for UK, identical to withdrawal
                template="bootstrap3/layout/datetimefield.html",
                data_date_format="DD-MM-YYYY HH:mm",
                placeholder="DD-MM-YYYY HH:mm"
            ),
            Field(
                'ice_boxes_filled',   # -- Not needed for UK, identical to withdrawal
                template="bootstrap3/layout/datetimefield.html",
                data_date_format="DD-MM-YYYY HH:mm",
                placeholder="DD-MM-YYYY HH:mm"
            ),
            Field('depart_perfusion_centre', template="bootstrap3/layout/datetimefield.html",
                data_date_format="DD-MM-YYYY HH:mm", placeholder="DD-MM-YYYY HH:mm"),
            Field('arrival_at_donor_hospital', template="bootstrap3/layout/datetimefield.html",
                data_date_format="DD-MM-YYYY HH:mm", placeholder="DD-MM-YYYY HH:mm"),
            style="padding: 10px;"
        )
    )
    layout_2 = Layout(
        Div(
            Field('number', placeholder="___ ___ ____"),
            'date_of_birth',
            Field('age'),
            'date_of_admission',
            'admitted_to_itu',
            'date_admitted_to_itu',
            'date_of_procurement',
            Field('gender'),
            'weight',
            'height',
            'ethnicity',
            'blood_group',
            style="padding: 10px;"
        )
    )
    layout_3 = Layout(
        Div(
            'diagnosis', 'diagnosis_other', 'diabetes_melitus', 'alcohol_abuse', 'cardiac_arrest',
            'systolic_blood_pressure', 'diastolic_blood_pressure', 'diuresis_last_day', 'diuresis_last_day_unknown',
            'diuresis_last_hour', 'diuresis_last_hour_unknown', 'dopamine', 'dobutamine', 'nor_adrenaline',
            'vasopressine', 'other_medication_details',
            style="padding: 10px;"
        )
    )
    layout_4 = Layout(
        Div(
            'last_creatinine', 'last_creatinine_unit', 'max_creatinine', 'max_creatinine_unit',

            Field('life_support_withdrawal', template="bootstrap3/layout/datetimefield.html",
                data_date_format="DD-MM-YYYY HH:mm", placeholder="DD-MM-YYYY HH:mm"),
            Field('systolic_pressure_low', template="bootstrap3/layout/datetimefield.html",
                data_date_format="DD-MM-YYYY HH:mm", placeholder="DD-MM-YYYY HH:mm"),
            Field('o2_saturation', template="bootstrap3/layout/datetimefield.html",
                data_date_format="DD-MM-YYYY HH:mm", placeholder="DD-MM-YYYY HH:mm"),
            Field('circulatory_arrest', template="bootstrap3/layout/datetimefield.html",
                data_date_format="DD-MM-YYYY HH:mm", placeholder="DD-MM-YYYY HH:mm"),
            'length_of_no_touch',
            Field('death_diagnosed', template="bootstrap3/layout/datetimefield.html",
                data_date_format="DD-MM-YYYY HH:mm", placeholder="DD-MM-YYYY HH:mm"),
            Field('perfusion_started', template="bootstrap3/layout/datetimefield.html",
                data_date_format="DD-MM-YYYY HH:mm", placeholder="DD-MM-YYYY HH:mm"),
            'systemic_flush_used', 'systemic_flush_used_other',
            'heparin',
            style="padding: 10px;"
        )
    )
    layout_5 = Layout(
        Div(
            'donor_blood_1_EDTA', 'donor_blood_1_SST', 'donor_urine_1', 'donor_urine_2',
            style="padding: 10px;"
        )
    )

    helper = FormHelper()
    helper.form_tag = False
    helper.html5_required = True
    helper.layout = Layout(
        Div(
            Div(
                Div(
                    HTML("<h3 class=\"panel-title\">Procedure Data</h3>"),  # TODO: Work out how to i18n this later!
                    css_class="panel-heading"
                ),
                Div(layout_1, css_class="panel-body"),
                css_class="panel panel-default"
            ),
            css_class="col-md-6",
            style="margin-top: 10px;"
        ),

        Div(
            Div(
                Div(
                    HTML("<h3 class=\"panel-title\">Donor Details</h3>"),  # TODO: Work out how to i18n this later!
                    css_class="panel-heading"
                ),
                Div(layout_2, css_class="panel-body"),
                css_class="panel panel-default"
            ),
            css_class="col-md-6",
            style="margin-top: 10px;"
        ),

        Div(
            Div(
                Div(
                    HTML("<h3 class=\"panel-title\">Donor Preop Data</h3>"),  # TODO: Work out how to i18n this later!
                    css_class="panel-heading"
                ),
                Div(layout_3, css_class="panel-body"),
                css_class="panel panel-default"
            ),
            css_class="col-md-6",
            style="margin-top: 10px;"
        ),

        Div(
            Div(
                Div(
                    HTML("<h3 class=\"panel-title\">Lab Results</h3>"),  # TODO: Work out how to i18n this later!
                    css_class="panel-heading"
                ),
                Div(layout_4, css_class="panel-body"),
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
                Div(layout_5, css_class="panel-body"),
                css_class="panel panel-default"
            ),
            css_class="col-md-6",
            style="margin-top: 10px;"
        ),

        Div(css_class="clearfix")
    )

    class Meta:
        model = Donor
        fields = [
            'retrieval_team',
            'sequence_number',
            'perfusion_technician',
            'transplant_coordinator',
            'call_received',
            'retrieval_hospital',
            'scheduled_start',
            'technician_arrival',
            'ice_boxes_filled',
            'depart_perfusion_centre',
            'arrival_at_donor_hospital',

            'number', 'date_of_birth', 'age', 'date_of_admission', 'admitted_to_itu', 'date_admitted_to_itu',
            'date_of_procurement', 'gender', 'weight', 'height', 'ethnicity', 'blood_group',

            'diagnosis', 'diagnosis_other', 'diabetes_melitus', 'alcohol_abuse', 'cardiac_arrest',
            'systolic_blood_pressure', 'diastolic_blood_pressure', 'diuresis_last_day', 'diuresis_last_day_unknown',
            'diuresis_last_hour', 'diuresis_last_hour_unknown', 'dopamine', 'dobutamine', 'nor_adrenaline',
            'vasopressine', 'other_medication_details',

            'last_creatinine', 'last_creatinine_unit', 'max_creatinine', 'max_creatinine_unit',

            'life_support_withdrawal', 'systolic_pressure_low', 'o2_saturation', 'circulatory_arrest',
            'length_of_no_touch',
            'death_diagnosed', 'perfusion_started', 'systemic_flush_used', 'systemic_flush_used_other',
            'heparin',

            'donor_blood_1_EDTA', 'donor_blood_1_SST', 'donor_urine_1', 'donor_urine_2',
        ]
        widgets = {
            # 'name': Textarea(attrs={'cols': 80, 'rows': 20}),
            # 'admitted_to_itu' : RadioSelect(),
        }
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


class DonorStartForm(ModelForm):
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
        widgets = {
            # 'name': Textarea(attrs={'cols': 80, 'rows': 20}),
            # 'gender': RadioSelect(choices=Donor.GENDER_CHOICES)
        }
        localized_fields = ('__all__')

    def save(self, user):
        donor = super(DonorStartForm, self).save(commit=False)
        donor.created_by = user
        donor.created_on = timezone.now()
        donor.version = 1
        donor.save()
        return donor


class OrganForm(ModelForm):
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
                        'removal', 'renal_arteries', 'graft_damage', 'washout_perfusion', 'transplantable',
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
                        'perfusion_possible', 'perfusion_not_possible_because', 'perfusion_started', 'patch_holder',
                        'artificial_patch_used', 'artificial_patch_size', 'artificial_patch_number',
                        'oxygen_bottle_full',
                        'oxygen_bottle_open', 'oxygen_bottle_changed', 'oxygen_bottle_changed_at',
                        'ice_container_replenished',
                        'ice_container_replenished_at', 'perfusate_measurable', 'perfusate_measure',
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
    next = CharField(
        initial=LOGIN_REDIRECT_URL,
        widget=HiddenInput()
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
