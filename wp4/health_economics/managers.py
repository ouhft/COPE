#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from livefield.managers import LiveManager

from wp4.compare.managers.core import ModelForUserManagerMixin


class HealthEconomicsModelForUserManager(LiveManager, ModelForUserManagerMixin):
    """
    QualityOfLife & Resource Logs have their location based on the recipient's location
    """
    def get_queryset(self):
        """
        :return: Queryset
        """
        qs = super(HealthEconomicsModelForUserManager, self).get_queryset().\
            select_related('recipient', 'recipient__allocation').\
            select_related('recipient__allocation__transplant_hospital__based_at')

        if self.current_user is not None:
            if self.current_user.is_superuser:
                # http://stackoverflow.com/questions/2507086/django-auth-has-perm-returns-true-while-list-of-permissions-is-empty/2508576
                # Superusers get *all* permissions :-/
                return qs

            if self.current_user.has_perm('restrict_to_local'):
                return qs.filter(recipient__allocation__transplant_hospital_id=self.hospital_id)
            elif self.current_user.has_perm('restrict_to_national'):
                return qs.filter(recipient__allocation__transplant_hospital__based_at__country=self.country_id)

        return qs


class ResourceLogModelForUserManager(LiveManager, ModelForUserManagerMixin):
    """
    Resource Logs have their location based on the recipient's location
    """
    def get_queryset(self):
        """
        :return: Queryset
        """
        qs = super(ResourceLogModelForUserManager, self).get_queryset().\
            select_related('log', 'log__recipient', 'log__recipient__allocation').\
            select_related('log__recipient__allocation__transplant_hospital__based_at')

        if self.current_user is not None:
            if self.current_user.is_superuser:
                # http://stackoverflow.com/questions/2507086/django-auth-has-perm-returns-true-while-list-of-permissions-is-empty/2508576
                # Superusers get *all* permissions :-/
                return qs

            if self.current_user.has_perm('restrict_to_local'):
                return qs.filter(log__recipient__allocation__transplant_hospital_id=self.hospital_id)
            elif self.current_user.has_perm('restrict_to_national'):
                return qs.filter(log__recipient__allocation__transplant_hospital__based_at__country=self.country_id)

        return qs
