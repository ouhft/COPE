#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from livefield.managers import LiveManager

from django.db import models

from ..models import PRESERVATION_NOT_SET


class ModelForUserManagerMixin(object):
    """
    Relies on permissions being set on a model so that we can restrict based on the location of the active user

    .. note::
    
        Class names are not yet dynamically added to permissions as we would want. May appear in django 1.11?
        http://stackoverflow.com/questions/4963428/how-to-dynamically-name-permissions-in-a-django-abstract-model-class

    
    Remember, these permissions are set for models using this manager:
    
        permissions = (
            ("restrict_to_national", "Can only use data from the same location country"),
            ("restrict_to_local", "Can only use data from a specific location"),
        )
    
    """
    country_id = None
    hospital_id = None
    current_user = None

    class MissingUser(Exception):
        """A request was missing from the object manager"""
        pass

    class MissingUserLocation(Exception):
        """A request.user has no valid based_at location"""
        pass

    def for_user(self, user=None):
        """
        To enable per user filtering, we need to have this passed into the manager by a mechanism such as this. If
        no user is specified, then the manager should continue to work as the default manager

        :param user:
        :return:
        """
        if user is None:
            raise self.MissingUser("ObjectManager.for_user has no user specified")
        else:
            self.current_user = user

            if self.current_user.based_at is None:
                raise self.MissingUserLocation(
                    "ObjectManager.get_queryset current user has no location set in profile"
                )
            else:
                self.country_id = self.current_user.based_at.country
                self.hospital_id = self.current_user.based_at.id

        return self.get_queryset()


class RetrievalTeamModelForUserManager(models.Manager, ModelForUserManagerMixin):
    """
    Test for permissions to view and restrict based on rules. Relies on for_user() having been called prior
    """
    def get_queryset(self):
        qs = super(RetrievalTeamModelForUserManager, self).get_queryset().select_related('based_at')

        if self.current_user is not None:
            if self.current_user.is_superuser:
                # http://stackoverflow.com/questions/2507086/django-auth-has-perm-returns-true-while-list-of-permissions-is-empty/2508576
                # Superusers get *all* permissions :-/
                return qs

            if self.current_user.has_perm('compare.restrict_to_local'):
                return qs.filter(based_at__id=self.hospital_id)
            elif self.current_user.has_perm('compare.restrict_to_national'):
                return qs.filter(based_at__country=self.country_id)

        return qs


class DonorModelForUserManager(LiveManager, ModelForUserManagerMixin):
    def get_queryset(self):
        """
        Test for permissions to view and restrict based on rules. Relies on for_user() having been called prior
        :return:
        """
        qs = super(DonorModelForUserManager, self).get_queryset().\
            select_related('_left_kidney', '_right_kidney', 'retrieval_team').\
            prefetch_related('randomisation')

        if self.current_user is not None:
            if self.current_user.is_superuser:
                # http://stackoverflow.com/questions/2507086/django-auth-has-perm-returns-true-while-list-of-permissions-is-empty/2508576
                # Superusers get *all* permissions :-/
                return qs

            if self.current_user.has_perm('compare.restrict_to_local'):
                return qs.filter(retrieval_team__based_at__id=self.hospital_id)
            elif self.current_user.has_perm('compare.restrict_to_national'):
                return qs.filter(retrieval_team__based_at__country=self.country_id)

        return qs


class OrganModelForUserManager(LiveManager, ModelForUserManagerMixin):
    """
    For the more mundate cases of listing organs for a given user
    """
    def get_queryset(self):
        """
        Test for permissions to view and restrict based on rules. Relies on for_user() having been called prior

        :return: Queryset
        """
        qs = super(OrganModelForUserManager, self).get_queryset().\
            select_related('recipient', 'recipient__person', 'recipient__allocation__perfusion_technician').\
            select_related('donor', 'donor__person', 'donor__randomisation','donor__retrieval_team').\
            prefetch_related('organallocation_set', 'organallocation_set__transplant_hospital')  # 'recipient__person__worksheet_set',

        if self.current_user is not None:
            if not self.current_user.is_superuser:
                if self.current_user.has_perm('compare.restrict_to_local'):
                    qs = qs.filter(
                        models.Q(donor__retrieval_team__based_at_id=self.hospital_id) |
                        models.Q(organallocation__transplant_hospital_id=self.hospital_id)
                    )
                elif self.current_user.has_perm('compare.restrict_to_national'):
                    qs = qs.filter(
                        models.Q(donor__retrieval_team__based_at__country=self.country_id) |
                        models.Q(organallocation__transplant_hospital__country=self.country_id)
                    )

        return qs


class ClosedOrganModelForUserManager(LiveManager, ModelForUserManagerMixin):
    """
    Get only the Organs that have had completed outcomes. These are:

    * Organ.not_allocated_reason has a value... which results in...
    * Recipient.transplantation_form_completed is True

    So wait, what about if the Organ is allocated to a non project site hospital, doesn't that also
    close the case? Yes, yes it does, and we can more trivially cover this by writing a message into
    not_allocated_reason of $MESSAGE$, and then setting the transplantation_form_completed flag
    """
    def get_queryset(self):
        """
        Filter Organs results by only returning where transplantation_form_completed=True

        :return: Queryset
        """
        qs = super(ClosedOrganModelForUserManager, self).get_queryset().\
            select_related('recipient', 'recipient__person', 'recipient__allocation__perfusion_technician').\
            select_related('donor', 'donor__person', 'donor__randomisation','donor__retrieval_team').\
            prefetch_related('organallocation_set', 'organallocation_set__transplant_hospital')  # 'recipient__person__worksheet_set',

        if self.current_user is not None:
            if not self.current_user.is_superuser:
                if self.current_user.has_perm('compare.restrict_to_local'):
                    qs = qs.filter(
                        models.Q(donor__retrieval_team__based_at_id=self.hospital_id) |
                        models.Q(organallocation__transplant_hospital__id=self.hospital_id)
                    )
                elif self.current_user.has_perm('compare.restrict_to_national'):
                    qs = qs.filter(
                        models.Q(donor__retrieval_team__based_at__country=self.country_id) |
                        models.Q(organallocation__transplant_hospital__country=self.country_id)
                    )

        return qs.filter(
                transplantation_form_completed=True
            )


class AllocatableModelForUserManager(LiveManager, ModelForUserManagerMixin):
    """
    Get only the Organs that are available for allocation, which means they're Unallocated (not
    deliberately), Randomised, and still Transplantable (but not necessarily a completed P form).
    """
    def get_queryset(self):
        """
        Filter Organ results by organallocation__isnull=True AND transplantable=True, excluding results
        where preservation=PRESERVATION_NOT_SET OR transplantation_form_completed=True

        :return: Queryset
        """
        qs = super(AllocatableModelForUserManager, self).get_queryset().\
            select_related('donor', 'donor__person', 'donor__randomisation', 'donor__retrieval_team').\
            prefetch_related('organallocation_set', 'organallocation_set__transplant_hospital')

        if self.current_user is not None:
            if not self.current_user.is_superuser:
                if self.current_user.has_perm('compare.restrict_to_local'):
                    qs = qs.filter(donor__retrieval_team__based_at_id=self.hospital_id)
                elif self.current_user.has_perm('compare.restrict_to_national'):
                    qs = qs.filter(donor__retrieval_team__based_at__country=self.country_id)

        return qs.filter(organallocation__isnull=True, transplantable=True).\
            exclude(preservation=PRESERVATION_NOT_SET).\
            exclude(transplantation_form_completed=True)


class OpenOrganModelForUserManager(LiveManager, ModelForUserManagerMixin):
    """
    Get only the Organs that are open for transplantation, which means they've one or more allocations,
    and have not had their form closed (either by allocation outside of project area, or completed form).
    """
    def get_queryset(self):
        """
        Filter Organ results by transplantable=True, excluding results where
        transplantation_form_completed=True OR organallocation__isnull=True OR
        preservation=PRESERVATION_NOT_SET

        :return: Queryset
        """
        qs = super(OpenOrganModelForUserManager, self).get_queryset().\
            select_related('recipient', 'recipient__person', 'recipient__allocation__perfusion_technician').\
            select_related('donor', 'donor__person', 'donor__randomisation','donor__retrieval_team').\
            prefetch_related('organallocation_set', 'organallocation_set__transplant_hospital')  # 'recipient__person__worksheet_set',

        if self.current_user is not None:
            if not self.current_user.is_superuser:
                if self.current_user.has_perm('compare.restrict_to_local'):
                    qs = qs.filter(
                        models.Q(donor__retrieval_team__based_at_id=self.hospital_id) |
                        models.Q(organallocation__transplant_hospital_id=self.hospital_id)
                    )
                elif self.current_user.has_perm('compare.restrict_to_national'):
                    qs = qs.filter(
                        models.Q(donor__retrieval_team__based_at__country=self.country_id) |
                        models.Q(organallocation__transplant_hospital__country=self.country_id)
                )

        return qs.filter(transplantable=True).\
            exclude(transplantation_form_completed=True).\
            exclude(organallocation__isnull=True).\
            exclude(preservation=PRESERVATION_NOT_SET)


class ProcurementResourceModelForUserManager(models.Manager, ModelForUserManagerMixin):
    """
    Test for permissions to view and restrict based on rules. Relies on for_user() having been called prior
    """
    def get_queryset(self):
        qs = super(ProcurementResourceModelForUserManager, self).get_queryset().select_related('organ')

        return qs


class OrganAllocationModelForUserManager(LiveManager, ModelForUserManagerMixin):
    def get_queryset(self):
        """
        Test for permissions to view and restrict based on rules
        :return:
        """
        qs = super(OrganAllocationModelForUserManager, self).get_queryset().select_related('transplant_hospital')

        if self.current_user is not None:
            if self.current_user.is_superuser:
                # http://stackoverflow.com/questions/2507086/django-auth-has-perm-returns-true-while-list-of-permissions-is-empty/2508576
                # Superusers get *all* permissions :-/
                return qs

            if self.current_user.has_perm('compare.restrict_to_local'):
                return qs.filter(transplant_hospital_id=self.hospital_id)

            if self.current_user.has_perm('compare.restrict_to_national'):
                return qs.filter(transplant_hospital__country=self.country_id)

        return qs


class RecipientModelForUserManager(LiveManager, ModelForUserManagerMixin):
    def get_queryset(self):
        """
        Test for permissions to view and restrict based on rules
        :return:
        """
        qs = super(RecipientModelForUserManager, self).get_queryset()

        if self.current_user is not None:
            if self.current_user.is_superuser:
                # http://stackoverflow.com/questions/2507086/django-auth-has-perm-returns-true-while-list-of-permissions-is-empty/2508576
                # Superusers get *all* permissions :-/
                return qs

            if self.current_user.has_perm('compare.restrict_to_local'):
                return qs.filter(allocation__transplant_hospital_id=self.hospital_id)
            elif self.current_user.has_perm('compare.restrict_to_national'):
                return qs.filter(allocation__transplant_hospital__country=self.country_id)

        return qs
