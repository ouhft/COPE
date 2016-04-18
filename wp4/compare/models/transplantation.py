#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals
from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models
from django.utils.translation import ugettext_lazy as _
from wp4.staff_person.models import StaffJob, StaffPerson
from wp4.locations.models import Hospital
from ..validators import validate_between_1900_2050, validate_not_in_future
from .core import VersionControlModel, OrganPerson
from .core import YES_NO_UNKNOWN_CHOICES, LOCATION_CHOICES
from .organ import Organ


class OrganAllocation(VersionControlModel):
    organ = models.ForeignKey(Organ)  # Internal link

    #  Allocation data
    REALLOCATION_CROSSMATCH = 1
    REALLOCATION_UNKNOWN = 2
    REALLOCATION_OTHER = 3
    REALLOCATION_CHOICES = (
        (REALLOCATION_CROSSMATCH, _('OAc01 Positive crossmatch')),
        (REALLOCATION_UNKNOWN, _('OAc02 Unknown')),
        (REALLOCATION_OTHER, _('OAc03 Other'))
    )
    perfusion_technician = models.ForeignKey(
        StaffPerson,
        verbose_name=_('OA01 name of transplant technician'),
        limit_choices_to={"jobs": StaffJob.PERFUSION_TECHNICIAN},
        related_name="recipient_perfusion_technician_set",
        blank=True, null=True
    )
    call_received = models.DateTimeField(
        verbose_name=_('OA02 call received from transplant co-ordinator at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future]
    )
    call_received_unknown = models.BooleanField(default=False)  # Internal flag
    transplant_hospital = models.ForeignKey(Hospital, verbose_name=_('OA03 transplant hospital'), blank=True, null=True)
    theatre_contact = models.ForeignKey(
        StaffPerson,
        verbose_name=_('OA04 name of the theatre contact'),
        limit_choices_to={"jobs": StaffJob.THEATRE_CONTACT},
        related_name="recipient_transplant_coordinator_set",
        blank=True, null=True
    )
    scheduled_start = models.DateTimeField(
        verbose_name=_('OA05 scheduled start'),
        blank=True, null=True,
        validators=[validate_between_1900_2050]
    )
    scheduled_start_unknown = models.BooleanField(default=False)  # Internal flag
    technician_arrival = models.DateTimeField(
        verbose_name=_('OA06 arrival time at hub'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future]
    )
    technician_arrival_unknown = models.BooleanField(default=False)  # Internal flag
    depart_perfusion_centre = models.DateTimeField(
        verbose_name=_('OA07 departure from hub'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future]
    )
    depart_perfusion_centre_unknown = models.BooleanField(default=False)  # Internal flag
    arrival_at_recipient_hospital = models.DateTimeField(
        verbose_name=_('OA08 arrival at transplant hospital'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future]
    )
    arrival_at_recipient_hospital_unknown = models.BooleanField(default=False)  # Internal flag
    journey_remarks = models.TextField(verbose_name=_("OA09 journey notes"), blank=True)
    reallocated = models.NullBooleanField(verbose_name=_("OA10 reallocated"), blank=True, default=None)
    reallocation_reason = models.PositiveSmallIntegerField(
        verbose_name=_('OA11 reason for re-allocation'),
        choices=REALLOCATION_CHOICES,
        blank=True, null=True
    )
    reallocation_reason_other = models.CharField(verbose_name=_('OA12 other reason'), max_length=250, blank=True)
    reallocation = models.OneToOneField('OrganAllocation', default=None, blank=True, null=True)

    class Meta:
        order_with_respect_to = 'organ'
        verbose_name = _('OAm1 organ allocation')
        verbose_name_plural = _('OAm2 organ allocations')
        get_latest_by = 'created_on'

    def clean(self):
        # Clean the fields that at Not Known
        if self.call_received_unknown:
            self.call_received = None
        if self.scheduled_start_unknown:
            self.scheduled_start = None
        if self.technician_arrival_unknown:
            self.technician_arrival = None
        if self.depart_perfusion_centre_unknown:
            self.depart_perfusion_centre = None
        if self.arrival_at_recipient_hospital_unknown:
            self.arrival_at_recipient_hospital = None

        if self.reallocated is not None:
            if self.transplant_hospital is None:
                raise ValidationError(_("OAv01 Please enter a transplant hospital to continue"))
            if self.perfusion_technician is None:
                raise ValidationError(_("OAv02 Please enter the name of the transplant technician"))

    def __unicode__(self):
        try:
            recipient_string = self.recipient
        except AttributeError:
            recipient_string = "None"
        return 'Organ: %s | Recipient: %s' % (self.organ.pk, recipient_string)


class Recipient(VersionControlModel):
    person = models.OneToOneField(OrganPerson)  # Internal link
    organ = models.OneToOneField(Organ)  # Internal link
    allocation = models.OneToOneField(OrganAllocation)  # Internal link

    # Trial signoffs
    signed_consent = models.NullBooleanField(
        verbose_name=_("RE13 informed consent given"), blank=True, default=None
    )
    single_kidney_transplant = models.NullBooleanField(
        verbose_name=_("RE14 receiving one kidney"), blank=True, default=None
    )

    # Recipient details (in addition to OrganPerson)
    RENAL_DISEASE_CHOICES = (
        (1, _('REc04 Glomerular diseases')),
        (2, _('REc05 Polycystic kidneys')),
        (3, _('REc06 Uncertain etiology')),
        (4, _('REc07 Tubular and interstitial diseases')),
        (5, _('REc08 Retransplant graft failure')),
        (6, _('REc09 diabetic nephropathyes')),
        (7, _('REc10 hypertensive nephropathyes')),
        (8, _('REc11 congenital rare disorders')),
        (9, _('REc12 renovascular and other diseases')),
        (10, _('REc13 neoplasms')),
        (11, _('REc14 other'))
    )
    renal_disease = models.PositiveSmallIntegerField(
        verbose_name=_('RE15 renal disease'),
        choices=RENAL_DISEASE_CHOICES,
        blank=True, null=True
    )
    renal_disease_other = models.CharField(verbose_name=_('RE16 other renal disease'), max_length=250, blank=True)
    pre_transplant_diuresis = models.PositiveSmallIntegerField(
        verbose_name=_('RE17 diuresis (ml/24hr)'),
        blank=True, null=True
    )

    # Peri-operative data
    INCISION_CHOICES = (
        (1, _('REc15 midline laparotomy')),
        (2, _('REc16 hockey stick')),
        (3, _('REc17 unknown'))
    )
    ARTERIAL_PROBLEM_CHOICES = (
        (1, _('REc18 None')),
        (2, _('REc19 ligated polar artery')),
        (3, _('REc20 reconstructed polar artery')),
        (4, _('REc21 repaired intima dissection')),
        (5, _('REc22 other'))
    )
    VENOUS_PROBLEM_CHOICES = (
        (1, _('REc23 none')),
        (2, _('REc24 laceration')),
        (3, _('REc25 elongation plasty')),
        (4, _('REc26 other'))
    )
    knife_to_skin = models.DateTimeField(
        verbose_name=_('RE18 knife to skin time'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future]
    )
    perfusate_measure = models.FloatField(verbose_name=_('RE19 pO2 perfusate'), blank=True, null=True)
    # TODO: Check the value range for perfusate_measure
    perfusion_stopped = models.DateTimeField(
        verbose_name=_('RE20 stop machine perfusion'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future]
    )
    organ_cold_stored = models.BooleanField(verbose_name=_('RE21 kidney was cold stored?'), default=False)
    tape_broken = models.NullBooleanField(verbose_name=_('RE22 tape over regulator broken'), blank=True, null=True)
    removed_from_machine_at = models.DateTimeField(
        verbose_name=_('RE23 kidney removed from machine at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future])
    oxygen_full_and_open = models.PositiveSmallIntegerField(
        verbose_name=_('RE24 oxygen full and open'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True)
    organ_untransplantable = models.NullBooleanField(
        verbose_name=_('RE25 kidney discarded'),
        help_text=_("REh25 Either answer means further questions will open below"),
        blank=True, null=True
    )
    organ_untransplantable_reason = models.CharField(
        verbose_name=_('RE26 untransplantable because'),
        max_length=250,
        blank=True
    )
    anesthesia_started_at = models.DateTimeField(
        verbose_name=_('RE27 start anesthesia at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future]
    )
    incision = models.PositiveSmallIntegerField(
        verbose_name=_('RE28 incision'),
        choices=INCISION_CHOICES,
        blank=True, null=True
    )
    transplant_side = models.CharField(
        verbose_name=_('RE29 transplant side'),
        max_length=1,
        choices=LOCATION_CHOICES,
        blank=True
    )
    arterial_problems = models.PositiveSmallIntegerField(
        verbose_name=_('RE30 arterial problems'),
        choices=ARTERIAL_PROBLEM_CHOICES,
        blank=True, null=True
    )
    arterial_problems_other = models.CharField(
        verbose_name=_('RE31 arterial problems other'),
        max_length=250,
        blank=True
    )
    venous_problems = models.PositiveSmallIntegerField(
        verbose_name=_('RE32 venous problems'),
        choices=VENOUS_PROBLEM_CHOICES,
        blank=True, null=True
    )
    venous_problems_other = models.CharField(verbose_name=_('RE33 venous problems other'), max_length=250, blank=True)
    anastomosis_started_at = models.DateTimeField(
        verbose_name=_('RE34 start anastomosis at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future]
    )
    anastomosis_started_at_unknown = models.BooleanField(default=False)  # Internal flag
    reperfusion_started_at = models.DateTimeField(
        verbose_name=_('RE35 start reperfusion at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future]
    )
    reperfusion_started_at_unknown = models.BooleanField(default=False)  # Internal flag
    mannitol_used = models.PositiveSmallIntegerField(
        verbose_name=_('RE36 mannitol used'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )
    other_diurectics = models.PositiveSmallIntegerField(
        verbose_name=_('RE37 other diurectics used'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )
    other_diurectics_details = models.CharField(
        verbose_name=_('RE38 other diurectics detail'),
        max_length=250,
        blank=True
    )
    systolic_blood_pressure = models.PositiveSmallIntegerField(
        verbose_name=_('RE39 systolic blood pressure at reperfusion'),
        validators=[MinValueValidator(10), MaxValueValidator(200)],
        blank=True, null=True
    )
    cvp = models.PositiveSmallIntegerField(verbose_name=_('RE40 cvp at reperfusion'), blank=True, null=True)
    intra_operative_diuresis = models.PositiveSmallIntegerField(
        verbose_name=_('RE41 intra-operative diuresis'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )
    successful_conclusion = models.BooleanField(verbose_name=_("RE42 successful conclusion"), default=False)
    operation_concluded_at = models.DateTimeField(
        verbose_name=_("RE43 operation concluded at"),
        null=True, blank=True,
        validators=[validate_between_1900_2050, validate_not_in_future]
    )

    # Machine cleanup record
    probe_cleaned = models.NullBooleanField(
        verbose_name=_('RE44 temperature and flow probe cleaned'),
        blank=True, null=True
    )
    ice_removed = models.NullBooleanField(verbose_name=_('RE45 ice and water removed'), blank=True, null=True)
    oxygen_flow_stopped = models.NullBooleanField(verbose_name=_('RE46 oxygen flow stopped'), blank=True, null=True)
    oxygen_bottle_removed = models.NullBooleanField(verbose_name=_('RE47 oxygen bottle removed'), blank=True, null=True)
    box_cleaned = models.NullBooleanField(verbose_name=_('RE48 box kidney assist cleaned'), blank=True, null=True)
    batteries_charged = models.NullBooleanField(verbose_name=_('RE49 batteries charged'), blank=True, null=True)
    cleaning_log = models.TextField(verbose_name=_("RE50 cleaning log notes"), blank=True)

    class Meta:
        order_with_respect_to = 'organ'
        verbose_name = _('REm1 recipient')
        verbose_name_plural = _('REm2 recipients')
        get_latest_by = 'created_on'

    def clean(self):
        # Clean the fields that at Not Known
        if self.anastomosis_started_at_unknown:
            self.anastomosis_started_at = None
        if self.reperfusion_started_at_unknown:
            self.reperfusion_started_at = None

        if self.organ.transplantation_form_completed:
            # Things to check if the form is being marked as complete...
            if self.perfusion_stopped is None:  # RE20
                raise ValidationError(_("REv01 Missing time machine perfusion stopped"))
            if self.removed_from_machine_at is None:  # RE23
                raise ValidationError(_("REv02 Missing time kidney removed from machine"))
            if self.anesthesia_started_at is None:  # RE27
                raise ValidationError(_("REv03 Missing Start time of anaesthesia"))
            if self.anastomosis_started_at is None and self.anastomosis_started_at_unknown is False:  # RE34
                raise ValidationError(_("REv04 Missing Anastomosis Start Time"))
            if self.reperfusion_started_at is None and self.reperfusion_started_at_unknown is False:  # RE35
                raise ValidationError(_("REv05 Missing Reperfusion Start Time"))

    def __unicode__(self):
        return '%s (%s)' % (self.person.number, self.trial_id)

    @property
    def age_from_dob(self):
        return self.person.age_from_dob

    @property
    def trial_id(self):
        return self.organ.trial_id
