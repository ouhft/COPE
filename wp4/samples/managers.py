#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from livefield.managers import LiveManager

from django.db import models

from wp4.compare.managers.core import ModelForUserManagerMixin


class EventModelForUserManager(LiveManager, ModelForUserManagerMixin):
    """
    Sample Events manager
    """
    def get_queryset(self):
        """
        :return: Queryset
        """
        qs = super(EventModelForUserManager, self).get_queryset().\
            prefetch_related('bloodsample_set', 'tissuesample_set', 'perfusatesample_set', 'urinesample_set')

        return qs


class SampleFromPersonModelForUserManager(LiveManager, ModelForUserManagerMixin):
    """
    Samples manager
    """
    def get_queryset(self):
        """
        :return: Queryset
        """
        qs = super(SampleFromPersonModelForUserManager, self).get_queryset().\
            select_related('event', 'person__donor', 'person__recipient__organ').\
            prefetch_related('wp7_location')

        return qs


class SampleFromOrganModelForUserManager(LiveManager, ModelForUserManagerMixin):
    """
    Samples manager
    """
    def get_queryset(self):
        """
        :return: Queryset
        """
        qs = super(SampleFromOrganModelForUserManager, self).get_queryset().\
            select_related('event', 'organ__donor', 'organ__recipient')

        return qs

