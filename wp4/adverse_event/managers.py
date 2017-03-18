#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from livefield.managers import LiveManager

from wp4.compare.managers.core import ModelForUserManagerMixin


class EventModelForUserManager(LiveManager, ModelForUserManagerMixin):
    """
    Events have their location based on the organ recipient's location
    """
    def get_queryset(self):
        """
        :return: Queryset
        """
        qs = super(EventModelForUserManager, self).get_queryset().\
            select_related('organ', 'organ__recipient', 'organ__recipient__allocation').\
            prefetch_related('organ__organallocation_set')

        # print("DEBUG: EventModelForUserManager.get_queryset(): self.current_user={0}".format(self.current_user))
        if self.current_user is not None:
            if self.current_user.is_superuser:
                # http://stackoverflow.com/questions/2507086/django-auth-has-perm-returns-true-while-list-of-permissions-is-empty/2508576
                # Superusers get *all* permissions :-/
                return qs

            if self.current_user.has_perm('adverse_event.restrict_to_local'):
                return qs.filter(organ__recipient__allocation__transplant_hospital_id=self.hospital_id)
            elif self.current_user.has_perm('adverse_event.restrict_to_national'):
                return qs.filter(organ__recipient__allocation__transplant_hospital__country=self.country_id)

        return qs
