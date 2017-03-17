#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from .models import FollowUpInitial, FollowUp3M, FollowUp6M, FollowUp1Y
from wp4.health_economics.models import QualityOfLife


def generate_followups_from_recipient(recipient):
    """
    For a given recipient, create 4 follow up forms, with the latter three "hidden" until they're needed. Add associated
    QoL for the forms that reference it.

    :param recipient:
    :return:
    """
    print("DEBUG: generate_followups_from_recipient called")
    f1 = FollowUpInitial.objects.create(organ=recipient.organ)
    q1 = QualityOfLife.objects.create(recipient=recipient, live=False)
    f2 = FollowUp3M.objects.create(organ=recipient.organ, quality_of_life=q1, live=False)
    f3 = FollowUp6M.objects.create(organ=recipient.organ, live=False)
    q2 = QualityOfLife.objects.create(recipient=recipient, live=False)
    f4 = FollowUp1Y.objects.create(organ=recipient.organ, quality_of_life=q2, live=False)
