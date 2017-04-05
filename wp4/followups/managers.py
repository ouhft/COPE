#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from livefield.managers import LiveManager

from django.core.exceptions import FieldError

from wp4.compare.managers.core import ModelForUserManagerMixin


class FollowupModelForUserManager(LiveManager, ModelForUserManagerMixin):
    """
    Followups have their location based on the organ recipient's location
    """
    def get_queryset(self):
        """
        :return: Queryset
        """
        qs = super(FollowupModelForUserManager, self).get_queryset().\
            select_related('organ', 'organ__recipient', 'organ__recipient__allocation')

        # print("DEBUG: FollowupModelForUserManager.get_queryset(): user={0}.".format(self.current_user))
        # print("DEBUG: FollowupModelForUserManager.get_queryset(): permissions={0}.".format(self.current_user.get_all_permissions()))

        if self.current_user is not None:
            if self.current_user.is_superuser:
                # http://stackoverflow.com/questions/2507086/django-auth-has-perm-returns-true-while-list-of-permissions-is-empty/2508576
                # Superusers get *all* permissions :-/
                return qs

            if self.current_user.has_perm('followups.restrict_to_local'):
                # print("DEBUG: FollowupModelForUserManager.get_queryset(): - LOCAL")
                return qs.filter(organ__recipient__allocation__transplant_hospital_id=self.hospital_id)
            elif self.current_user.has_perm('followups.restrict_to_national'):
                # print("DEBUG: FollowupModelForUserManager.get_queryset(): - NATIONAL")
                return qs.filter(organ__recipient__allocation__transplant_hospital__country=self.country_id)

        return qs


class FollowupWithQOLModelForUserManager(LiveManager, ModelForUserManagerMixin):
    """
    Followups have their location based on the organ recipient's location
    """
    def get_queryset(self):
        """
        :return: Queryset
        """
        qs = super(FollowupWithQOLModelForUserManager, self).get_queryset().\
            select_related('organ', 'organ__recipient', 'organ__recipient__allocation').\
            select_related('quality_of_life', 'quality_of_life__recipient__organ')

        # print("DEBUG: FollowupModelForUserManager.get_queryset(): user={0}.".format(self.current_user))
        # print("DEBUG: FollowupModelForUserManager.get_queryset(): permissions={0}.".format(self.current_user.get_all_permissions()))

        if self.current_user is not None:
            if self.current_user.is_superuser:
                # http://stackoverflow.com/questions/2507086/django-auth-has-perm-returns-true-while-list-of-permissions-is-empty/2508576
                # Superusers get *all* permissions :-/
                return qs

            if self.current_user.has_perm('followups.restrict_to_local'):
                # print("DEBUG: FollowupModelForUserManager.get_queryset(): - LOCAL")
                return qs.filter(organ__recipient__allocation__transplant_hospital_id=self.hospital_id)
            elif self.current_user.has_perm('followups.restrict_to_national'):
                # print("DEBUG: FollowupModelForUserManager.get_queryset(): - NATIONAL")
                return qs.filter(organ__recipient__allocation__transplant_hospital__country=self.country_id)

        return qs

