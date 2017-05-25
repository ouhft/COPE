#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.core.validators import MinValueValidator, MaxValueValidator, ValidationError
from django.db import models
from django.utils.functional import cached_property
from django.utils.translation import ugettext_lazy as _

from wp4.staff.models import Person
from wp4.locations.models import Hospital

from . import YES_NO_UNKNOWN_CHOICES, LOCATION_CHOICES
from . import AuditControlModelBase
from .core import Patient
from .donor import Organ
from ..validators import validate_between_1900_2050, validate_not_in_future
from ..managers.core import OrganAllocationModelForUserManager, RecipientModelForUserManager


class OrganAllocation(AuditControlModelBase):
    """
    Organs can be allocated multiple times before finding a definitive recipient. This class acts as
    the record of these allocations and a link between Organ and Recipient.
    """
    organ = models.ForeignKey(Organ)  # Internal link to the Organ

    #  Allocation data
    REALLOCATION_CROSSMATCH = 1  #: Constant for REALLOCATION_CHOICES
    REALLOCATION_UNKNOWN = 2  #: Constant for REALLOCATION_CHOICES
    REALLOCATION_OTHER = 3  #: Constant for REALLOCATION_CHOICES
    REALLOCATION_CHOICES = (
        (REALLOCATION_CROSSMATCH, _('OAc01 Positive crossmatch')),
        (REALLOCATION_UNKNOWN, _('OAc02 Unknown')),
        (REALLOCATION_OTHER, _('OAc03 Other'))
    )  #: OrganAllocation reallocation_reason choices

    perfusion_technician = models.ForeignKey(
        Person,
        verbose_name=_('OA01 name of transplant technician'),
        related_name="recipient_perfusion_technician_set",
        blank=True, null=True
    )
    call_received = models.DateTimeField(
        verbose_name=_('OA02 call received from transplant co-ordinator at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    call_received_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    transplant_hospital = models.ForeignKey(Hospital, verbose_name=_('OA03 transplant hospital'), blank=True, null=True)
    theatre_contact = models.ForeignKey(
        Person,
        verbose_name=_('OA04 name of the theatre contact'),
        related_name="recipient_transplant_coordinator_set",
        blank=True, null=True
    )
    scheduled_start = models.DateTimeField(
        verbose_name=_('OA05 scheduled start'),
        blank=True, null=True,
        validators=[validate_between_1900_2050],
        help_text="Date must be fall within 1900-2050"
    )
    scheduled_start_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    technician_arrival = models.DateTimeField(
        verbose_name=_('OA06 arrival time at hub'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    technician_arrival_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    depart_perfusion_centre = models.DateTimeField(
        verbose_name=_('OA07 departure from hub'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    depart_perfusion_centre_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    arrival_at_recipient_hospital = models.DateTimeField(
        verbose_name=_('OA08 arrival at transplant hospital'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    arrival_at_recipient_hospital_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    journey_remarks = models.TextField(verbose_name=_("OA09 journey notes"), blank=True)
    reallocated = models.NullBooleanField(verbose_name=_("OA10 reallocated"), blank=True, default=None)
    reallocation_reason = models.PositiveSmallIntegerField(
        verbose_name=_('OA11 reason for re-allocation'),
        choices=REALLOCATION_CHOICES,
        blank=True, null=True
    )  #: Limit choices to REALLOCATION_CHOICES
    reallocation_reason_other = models.CharField(verbose_name=_('OA12 other reason'), max_length=250, blank=True)
    reallocation = models.OneToOneField(
        'OrganAllocation',
        default=None,
        blank=True, null=True,
        help_text="Internal forward link value to another OrganAllocation record"
    )

    objects = OrganAllocationModelForUserManager()

    class Meta:
        order_with_respect_to = 'organ'
        verbose_name = _('OAm1 organ allocation')
        verbose_name_plural = _('OAm2 organ allocations')
        get_latest_by = 'pk'
        permissions = (
            ("view_organallocation", "Can only view the data"),
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )

    def country_for_restriction(self):
        """
        Get the country to be used for geographic restriction of this data
        :return: Int: Value from list in Locations.Models. Should be in range [None, 1,4,5]
        """
        if self.transplant_hospital:
            return self.transplant_hospital.based_at.country
        return None

    def location_for_restriction(self):
        """
        Get the location to be used for geographic restriction of this data
        :return: Int: Hospital object id
        """
        if self.transplant_hospital:
            return self.transplant_hospital.based_at.id
        return None

    def clean(self):
        """
        Clears the following fields of data if their corresponding unknown flag is set to True

        * call_received
        * scheduled_start
        * technician_arrival
        * depart_perfusion_centre
        * arrival_at_recipient_hospital

        Error if:

        * reallocated is set (True or False), but transplant_hospital is empty (OAv01)
        * reallocated is set (True or False), but perfusion_technician is empty (OAv02)

        """
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
            #     Removed this as per Issue #192
            # if self.perfusion_technician is None:
            #     raise ValidationError(_("OAv02 Please enter the name of the transplant technician"))

    def __str__(self):
        try:
            recipient_string = self.recipient
        except AttributeError:
            recipient_string = "None"
        return 'Organ: %s | Recipient: %s' % (self.organ.pk, recipient_string)


class Recipient(AuditControlModelBase):
    """
    Extension of an Patient record (via OneToOne link for good ORM/DB management) to capture
    the Recipient specific data.

    Linked also to a single Organ (Kidney), and, for convenience, an OrganAllocation (once confirmed)

    Also holds the meta-data specific to the Transplantation Form
    """
    person = models.OneToOneField(Patient, help_text="Internal link to Patient")
    organ = models.OneToOneField(Organ, help_text="Internal link to Organ")
    allocation = models.OneToOneField(OrganAllocation, help_text="Internal link to OrganAllocation")

    # Trial signoffs
    signed_consent = models.NullBooleanField(
        verbose_name=_("RE13 informed consent given"), blank=True, default=None
    )
    single_kidney_transplant = models.NullBooleanField(
        verbose_name=_("RE14 receiving one kidney"), blank=True, default=None
    )

    # Recipient details (in addition to Patient)
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
    )  #: Recipient renal_disease choices
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
    )  #: Recipient incision choices
    ARTERIAL_PROBLEM_CHOICES = (
        (1, _('REc18 None')),
        (2, _('REc19 ligated polar artery')),
        (3, _('REc20 reconstructed polar artery')),
        (4, _('REc21 repaired intima dissection')),
        (5, _('REc22 other'))
    )  #: Recipient arterial_problems choices
    VENOUS_PROBLEM_CHOICES = (
        (1, _('REc23 none')),
        (2, _('REc24 laceration')),
        (3, _('REc25 elongation plasty')),
        (4, _('REc26 other'))
    )  #: Recipient venous_problems choices
    knife_to_skin = models.DateTimeField(
        verbose_name=_('RE18 knife to skin time'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    perfusate_measure = models.FloatField(verbose_name=_('RE19 pO2 perfusate'), blank=True, null=True)
    # TODO: Check the value range for perfusate_measure
    perfusion_stopped = models.DateTimeField(
        verbose_name=_('RE20 stop machine perfusion'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    organ_cold_stored = models.BooleanField(verbose_name=_('RE21 kidney was cold stored?'), default=False)
    tape_broken = models.PositiveSmallIntegerField(
        verbose_name=_('RE22 tape over regulator broken'),
        blank=True, null=True,
        choices=YES_NO_UNKNOWN_CHOICES,
    )  #: Limit choices to YES_NO_UNKNOWN_CHOICES
    removed_from_machine_at = models.DateTimeField(
        verbose_name=_('RE23 kidney removed from machine at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    oxygen_full_and_open = models.PositiveSmallIntegerField(
        verbose_name=_('RE24 oxygen full and open'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )  #: Limit choices to YES_NO_UNKNOWN_CHOICES
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
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    incision = models.PositiveSmallIntegerField(
        verbose_name=_('RE28 incision'),
        choices=INCISION_CHOICES,
        blank=True, null=True
    )  #: Limit choices to INCISION_CHOICES
    transplant_side = models.CharField(
        verbose_name=_('RE29 transplant side'),
        max_length=1,
        choices=LOCATION_CHOICES,
        blank=True
    )  #: Limit choices to LOCATION_CHOICES
    arterial_problems = models.PositiveSmallIntegerField(
        verbose_name=_('RE30 arterial problems'),
        choices=ARTERIAL_PROBLEM_CHOICES,
        blank=True, null=True
    )  #: Limit choices to ARTERIAL_PROBLEM_CHOICES
    arterial_problems_other = models.CharField(
        verbose_name=_('RE31 arterial problems other'),
        max_length=250,
        blank=True
    )
    venous_problems = models.PositiveSmallIntegerField(
        verbose_name=_('RE32 venous problems'),
        choices=VENOUS_PROBLEM_CHOICES,
        blank=True, null=True
    )  #: Limit choices to VENOUS_PROBLEM_CHOICES
    venous_problems_other = models.CharField(verbose_name=_('RE33 venous problems other'), max_length=250, blank=True)
    anastomosis_started_at = models.DateTimeField(
        verbose_name=_('RE34 start anastomosis at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    anastomosis_started_at_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    reperfusion_started_at = models.DateTimeField(
        verbose_name=_('RE35 start reperfusion at'),
        blank=True, null=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
    )
    reperfusion_started_at_unknown = models.BooleanField(default=False, help_text="Internal unknown flag")
    mannitol_used = models.PositiveSmallIntegerField(
        verbose_name=_('RE36 mannitol used'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )  #: Limit choices to YES_NO_UNKNOWN_CHOICES
    other_diurectics = models.PositiveSmallIntegerField(
        verbose_name=_('RE37 other diurectics used'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )  #: Limit choices to YES_NO_UNKNOWN_CHOICES
    other_diurectics_details = models.CharField(
        verbose_name=_('RE38 other diurectics detail'),
        max_length=250,
        blank=True
    )
    systolic_blood_pressure = models.PositiveSmallIntegerField(
        verbose_name=_('RE39 systolic blood pressure at reperfusion'),
        validators=[MinValueValidator(10), MaxValueValidator(200)],
        blank=True, null=True,
        help_text="Value must be in range 10-200"
    )
    cvp = models.PositiveSmallIntegerField(verbose_name=_('RE40 cvp at reperfusion'), blank=True, null=True)
    intra_operative_diuresis = models.PositiveSmallIntegerField(
        verbose_name=_('RE41 intra-operative diuresis'),
        choices=YES_NO_UNKNOWN_CHOICES,
        blank=True, null=True
    )  #: Limit choices to YES_NO_UNKNOWN_CHOICES
    successful_conclusion = models.BooleanField(verbose_name=_("RE42 successful conclusion"), default=False)
    operation_concluded_at = models.DateTimeField(
        verbose_name=_("RE43 operation concluded at"),
        null=True, blank=True,
        validators=[validate_between_1900_2050, validate_not_in_future],
        help_text="Date must be fall within 1900-2050, and not be in the future"
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

    objects = RecipientModelForUserManager()

    class Meta:
        order_with_respect_to = 'organ'
        verbose_name = _('REm1 recipient')
        verbose_name_plural = _('REm2 recipients')
        get_latest_by = 'pk'
        permissions = (
            ("view_recipient", "Can only view the data"),
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )

    def country_for_restriction(self):
        """
        Get the country to be used for geographic restriction of this data
        :return: Int: Value from list in Locations.Models. Should be in range [1,4,5]
        """
        return self.allocation.country_for_restriction

    def location_for_restriction(self):
        """
        Get the location to be used for geographic restriction of this data
        :return: Int: Hospital object id
        """
        return self.allocation.location_for_restriction

    def clean(self):
        """
        Clears the following fields of data if their corresponding unknown flag is set to True

        * anastomosis_started_at
        * reperfusion_started_at

        Error if transplantation_form_completed is True, and:

        * perfusion_stopped is empty (REv01)
        * removed_from_machine_at is empty (REv02)
        * anesthesia_started_at is empty (REv03)
        * anastomosis_started_at is empty (REv04)
        * reperfusion_started_at is empty (REv05)
        """
        # Clean the fields that at Not Known
        if self.anastomosis_started_at_unknown:
            self.anastomosis_started_at = None
        if self.reperfusion_started_at_unknown:
            self.reperfusion_started_at = None

        if self.organ.transplantation_form_completed:
            # Things to check if the form is being marked as complete...
            # if self.perfusion_stopped is None:  # RE20 -- Removed for Issue #104
            #     raise ValidationError(_("REv01 Missing time machine perfusion stopped"))
            if self.anesthesia_started_at is None:  # RE27
                raise ValidationError(_("REv03 Missing Start time of anaesthesia"))
            if self.anastomosis_started_at is None and self.anastomosis_started_at_unknown is False:  # RE34
                raise ValidationError(_("REv04 Missing Anastomosis Start Time"))
            # Modifed v02 and v05 based on Issue #146
            if not self.organ.was_cold_stored:
                if self.removed_from_machine_at is None:  # RE23
                    raise ValidationError(_("REv02 Missing time kidney removed from machine"))
                if self.reperfusion_started_at is None and self.reperfusion_started_at_unknown is False:  # RE35
                    raise ValidationError(_("REv05 Missing Reperfusion Start Time"))

    def __str__(self):
        return "#{0}: {1} ({3}) with trial id {2}".format(self.id, self.person.number, self.trial_id, self.person.id)

    def _age_from_dob(self):
        """
        Returns the calculated age of the Recipient

        :return: Recipient's age in years as calculated from their Date of Birth
        :rtype: int
        """
        return self.person.age_from_dob
    age_from_dob = cached_property(_age_from_dob, name='age_from_dob')

    def _trial_id(self):
        """
        Returns the Donor Trial ID combined with the Location (L or R) for the Organ

        :return: 'WP4cctnns'
        :rtype: str
        """
        return self.organ.trial_id
    trial_id = cached_property(_trial_id, name='trial_id')
