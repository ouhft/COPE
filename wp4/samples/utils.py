#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from django.db import transaction

from openpyxl import Workbook, load_workbook

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


class ReadOnlyWorkbook(object):
    __workbook__ = None
    __worksheet__ = None
    __max_columns__ = 0
    __headers_by_id__ = dict()
    __headers_by_title__ = dict()
    __current_row_id__ = 2  # First row is headers, so start with data
    __current_row_data__ = dict()  # Store the row in a dictionary based on the column titles

    def __init__(self, filename=""):
        if filename is not "":
            self.load_workbook(filename=filename)

    def load_workbook(self, filename="", column_count=1):
        if isinstance(filename, str) and filename is not "":
            self.__workbook__ = load_workbook(filename=filename, read_only=True)
            self.__worksheet__ = self.__workbook__.active
            self.__max_columns__ = column_count + 1  # NB: Excel references start at 1, not 0
            print("DEBUG: Max columns = {0}".format(self.__max_columns__))
            for column_id in range(1, self.__max_columns__):
                self.__headers_by_id__[column_id] = self.__worksheet__.cell(row=1, column=column_id).value
                self.__headers_by_title__[self.__worksheet__.cell(row=1, column=column_id).value] = column_id

            print("DEBUG: load_workbook: headers = {0}".format(self.__headers_by_id__))
            return True
        return False

    def get_rows(self):
        if self.__worksheet__ is not None:
            return self.__worksheet__.iter_rows()
        return None

    def load_row(self, row=None):
        """
        Load the current row contents into a dictionary keyed on the column title. The title is
        reduced to lower case so that it matches model naming conventions and is consistent.
        :param row: Worksheet row to be loaded into dict
        :return: dict() of row data if row id provided, otherwise, int of the current row id
        """
        column_id = 1
        for cell in row:
            self.__current_row_data__[self.__headers_by_id__[column_id].lower()] = cell.value
            column_id += 1
        return self.__current_row_data__
