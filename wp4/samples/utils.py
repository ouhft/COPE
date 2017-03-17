#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.db import transaction

from wp4.locations.models import UNITED_KINGDOM
from .models import Event, BloodSample, UrineSample, PerfusateSample, TissueSample


@transaction.atomic
def create_donor_samples(donor):
    """
    For a given Donor record, create the related events and samples for the protocol
    :param donor: Donor instance
    :return: True, if created, False otherwise
    """

    if donor.retrieval_team.based_at.country != UNITED_KINGDOM:
        person = donor.person
        # No donor samples are taken in the UK
        # ======================================================  BLOOD 1
        blood_event_1 = Event()
        blood_event_1.type = Event.TYPE_BLOOD
        blood_event_1.name = Event.NAME_DONOR_BLOOD1
        blood_event_1.save()

        blood_event_1_sample_1 = BloodSample()
        blood_event_1_sample_1.person = person
        blood_event_1_sample_1.event = blood_event_1
        blood_event_1_sample_1.blood_type = BloodSample.SAMPLE_SST
        blood_event_1_sample_1.save()

        blood_event_1_sample_2 = BloodSample()
        blood_event_1_sample_2.person = person
        blood_event_1_sample_2.event = blood_event_1
        blood_event_1_sample_2.blood_type = BloodSample.SAMPLE_EDSA
        blood_event_1_sample_2.save()

        # ======================================================  URINE 1
        urine_event_1 = Event()
        urine_event_1.type = Event.TYPE_URINE
        urine_event_1.name = Event.NAME_DONOR_URINE1
        urine_event_1.save()

        urine_event_1_sample_1 = UrineSample()
        urine_event_1_sample_1.person = person
        urine_event_1_sample_1.event = urine_event_1
        urine_event_1_sample_1.save()

        # ======================================================  URINE 2
        urine_event_2 = Event()
        urine_event_2.type = Event.TYPE_URINE
        urine_event_2.name = Event.NAME_DONOR_URINE2
        urine_event_2.save()

        urine_event_2_sample_1 = UrineSample()
        urine_event_2_sample_1.person = person
        urine_event_2_sample_1.event = urine_event_2
        urine_event_2_sample_1.save()

    for kidney in (donor.left_kidney, donor.right_kidney):
        # ==========================================================  PERFUSATE 1
        perfusate_event_1 = Event()
        perfusate_event_1.type = Event.TYPE_PERFUSATE
        perfusate_event_1.name = Event.NAME_ORGAN_PERFUSATE1
        perfusate_event_1.save()

        perfusate_event_1_sample_1 = PerfusateSample()
        perfusate_event_1_sample_1.organ = kidney
        perfusate_event_1_sample_1.event = perfusate_event_1
        perfusate_event_1_sample_1.save()

        # ==========================================================  PERFUSATE 2
        perfusate_event_2 = Event()
        perfusate_event_2.type = Event.TYPE_PERFUSATE
        perfusate_event_2.name = Event.NAME_ORGAN_PERFUSATE2
        perfusate_event_2.save()

        perfusate_event_2_sample_1 = PerfusateSample()
        perfusate_event_2_sample_1.organ = kidney
        perfusate_event_2_sample_1.event = perfusate_event_2
        perfusate_event_2_sample_1.save()

        # ==========================================================  PERFUSATE 3
        perfusate_event_3 = Event()
        perfusate_event_3.type = Event.TYPE_PERFUSATE
        perfusate_event_3.name = Event.NAME_ORGAN_PERFUSATE3
        perfusate_event_3.save()

        perfusate_event_3_sample_1 = PerfusateSample()
        perfusate_event_3_sample_1.organ = kidney
        perfusate_event_3_sample_1.event = perfusate_event_3
        perfusate_event_3_sample_1.save()

        # ==========================================================  TISSUE 1
        tissue_event_1 = Event()
        tissue_event_1.type = Event.TYPE_TISSUE
        tissue_event_1.name = Event.NAME_ORGAN_TISSUE
        tissue_event_1.save()

        tissue_event_1_sample_1 = TissueSample()
        tissue_event_1_sample_1.organ = kidney
        tissue_event_1_sample_1.event = tissue_event_1
        tissue_event_1_sample_1.tissue_type = TissueSample.SAMPLE_R
        tissue_event_1_sample_1.save()

        tissue_event_1_sample_2 = TissueSample()
        tissue_event_1_sample_2.organ = kidney
        tissue_event_1_sample_2.event = tissue_event_1
        tissue_event_1_sample_2.tissue_type = TissueSample.SAMPLE_F
        tissue_event_1_sample_2.save()


@transaction.atomic
def create_recipient_samples(recipient):
    """
    For a given Recipient record, look to see if a corresponding Worksheet exists, and if not, create it
    and the related events and samples for the protocol
    :param recipient: Recipient instance
    :return: True, if created, False otherwise
    """
    person = recipient.person

    # ==========================================================  BLOOD 1
    blood_event_1 = Event()
    blood_event_1.type = Event.TYPE_BLOOD
    blood_event_1.name = Event.NAME_RECIPIENT_BLOOD1
    blood_event_1.save()

    blood_event_1_sample_1 = BloodSample()
    blood_event_1_sample_1.person = person
    blood_event_1_sample_1.event = blood_event_1
    blood_event_1_sample_1.blood_type = BloodSample.SAMPLE_SST
    blood_event_1_sample_1.save()

    blood_event_1_sample_2 = BloodSample()
    blood_event_1_sample_2.person = person
    blood_event_1_sample_2.event = blood_event_1
    blood_event_1_sample_2.blood_type = BloodSample.SAMPLE_EDSA
    blood_event_1_sample_2.save()

    # ==========================================================  BLOOD 2
    blood_event_2 = Event()
    blood_event_2.type = Event.TYPE_BLOOD
    blood_event_2.name = Event.NAME_RECIPIENT_BLOOD2
    blood_event_2.save()

    blood_event_2_sample_1 = BloodSample()
    blood_event_2_sample_1.person = person
    blood_event_2_sample_1.event = blood_event_2
    blood_event_2_sample_1.blood_type = BloodSample.SAMPLE_SST
    blood_event_2_sample_1.save()

    blood_event_2_sample_2 = BloodSample()
    blood_event_2_sample_2.person = person
    blood_event_2_sample_2.event = blood_event_2
    blood_event_2_sample_2.blood_type = BloodSample.SAMPLE_EDSA
    blood_event_2_sample_2.save()
