#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals
import datetime

from .models import FollowUpInitial, FollowUp3M, FollowUp6M, FollowUp1Y
from wp4.health_economics.models import QualityOfLife
from wp4.compare.models.donor import Organ


def generate_followups_from_recipient(recipient):
    """
    For a given recipient, get or create 4 follow up forms, with the latter three "hidden" until they're needed. Add
    associated QoL for the forms that reference it.

    :param recipient:
    :return:
    """
    # print("DEBUG: generate_followups_from_recipient called")
    try:
        f1 = recipient.organ.followup_initial.id
    except FollowUpInitial.DoesNotExist:
        f1 = FollowUpInitial.objects.create(organ=recipient.organ)

    try:
        f2 = recipient.organ.followup_3m
        print(f2.id)
        # Check for an existing QoL record linked to this
        try:
            q1 = f2.quality_of_life.id
        except QualityOfLife.DoesNotExist:
            q1 = QualityOfLife.objects.create(recipient=recipient, live=f2.live)
    except FollowUp3M.DoesNotExist:
        q1 = QualityOfLife.objects.create(recipient=recipient, live=False)
        f2 = FollowUp3M.objects.create(organ=recipient.organ, quality_of_life=q1, live=False)

    try:
        f3 = recipient.organ.followup_6m.id
    except FollowUp6M.DoesNotExist:
        f3 = FollowUp6M.objects.create(organ=recipient.organ, live=False)

    try:
        f4 = recipient.organ.followup_1y
        print(f4.id)
        # Check for an existing QoL record linked to this
        try:
            q2 = f4.quality_of_life.id
        except QualityOfLife.DoesNotExist:
            q2 = QualityOfLife.objects.create(recipient=recipient, live=f4.live)
    except FollowUp1Y.DoesNotExist:
        q2 = QualityOfLife.objects.create(recipient=recipient, live=False)
        f4 = FollowUp1Y.objects.create(organ=recipient.organ, quality_of_life=q2, live=False)


def activate_followups_in_window():
    """
    Helper function that is anticipated to be run daily, where it will work through all Organs and "undelete" the
    followups that have passed the start_by date. On undelete there will be alerts sent out (eventually)
    :return:
    """
    date_now = datetime.datetime.now().date()
    for organ in Organ.all_objects.prefetch_related('followup_3m').filter(followup_3m__isnull=False):
        if organ.followup_3m_begin_by <= date_now:
            organ.followup_3m.undelete()
            organ.followup_3m.quality_of_life.undelete()

    for organ in Organ.all_objects.prefetch_related('followup_6m').filter(followup_6m__isnull=False):
        if organ.followup_6m_begin_by <= date_now:
            organ.followup_6m.undelete()

    for organ in Organ.all_objects.prefetch_related('followup_1y').filter(followup_1y__isnull=False):
        if organ.followup_final_begin_by <= date_now:
            organ.followup_1y.undelete()
            organ.followup_1y.quality_of_life.undelete()
