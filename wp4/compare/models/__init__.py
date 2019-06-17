#!/usr/bin/python
# coding: utf-8
from __future__ import unicode_literals
from livefield import LiveField, LiveManager

from django.db import models
from django.utils.translation import ugettext_lazy as _

# Common CONSTANTS
NO = 0  #: CONSTANT for YES_NO_UNKNOWN_CHOICES
YES = 1  #: CONSTANT for YES_NO_UNKNOWN_CHOICES
UNKNOWN = 2  #: CONSTANT for YES_NO_UNKNOWN_CHOICES
YES_NO_UNKNOWN_CHOICES = (
    (UNKNOWN, _("MMc03 Unknown")),
    (NO, _("MMc01 No")),
    (YES, _("MMc02 Yes"))
)  #: Need Yes to be the last choice for any FieldWithFollowUp where additional elements appear on Yes

NOT_APPLICABLE = 3  #: CONSTANT for YES_NO_NA_CHOICES
YES_NO_NA_CHOICES = (
    (NOT_APPLICABLE, _("MMc04 Not Applicable")),
    (NO, _("MMc01 No")),
    (YES, _("MMc02 Yes"))
)

# Originally from Organ
LEFT = "L"  #: CONSTANT for LOCATION_CHOICES
RIGHT = "R"  #: CONSTANT for LOCATION_CHOICES
LOCATION_CHOICES = (
    (LEFT, _('ORc01 Left')),
    (RIGHT, _('ORc02 Right'))
)   #: Organ location choices

# Originally from Organ
PRESERVATION_HMP = 0  #: CONSTANT for PRESERVATION_CHOICES
PRESERVATION_HMPO2 = 1  #: CONSTANT for PRESERVATION_CHOICES
PRESERVATION_NOT_SET = 9  #: CONSTANT for PRESERVATION_CHOICES
PRESERVATION_CHOICES = (
    (PRESERVATION_NOT_SET, _("ORc11 Not Set")),
    (PRESERVATION_HMP, "HMP"),
    (PRESERVATION_HMPO2, "HMP O2")
)  #: Organ preservation choices


class AuditControlModelBase(models.Model):
    """
    Internal common attributes to aid systematic auditing of records.

    `live` (aka record_active) will allow us to soft delete data.
    `record_locked` will allow the admin team to mark records as having been reviewed and put to rest.
    """
    __record_locked_on_load = False  # Need an internal representation of this independent of the main instance value
    record_locked = models.BooleanField(
        default=False,
        help_text="Locked by the admin team. This can only be reversed by the System Administrator"
    )
    live = LiveField()  #: Wanted this to be record_active, but that means modifying the LiveField code

    objects = LiveManager()
    all_objects = LiveManager(include_soft_deleted=True)

    # NB: Used in multiple apps
    class Meta:
        abstract = True

    def __init__(self, *args, **kwargs):
        super(AuditControlModelBase, self).__init__(*args, **kwargs)
        self.__record_locked_on_load = True if self.record_locked else False

    def save(self, force_insert=False, force_update=False, using=None, update_fields=None):
        """
        Meta save function that stops changes when record is locked
        """
        if self.__record_locked_on_load is True:
            # Use the internal representation from when the data was last loaded because it may be overwritten on this
            # save and thus the instance value can't be used to judge it.
            # TODO: Add an override for a SuperUser to save over this in the future
            raise Exception("%s Record is locked, and can not be saved" % type(self).__name__)
        return super(AuditControlModelBase, self).save(
            force_insert,
            force_update,
            using,
            update_fields
        )

    def delete(self, using=None):
        self.live = False
        self.save(using=using)

    def undelete(self, using=None):
        self.live = True
        self.save(using=using)


from .core import Patient, Randomisation, RetrievalTeam

from .donor import Donor, Organ, ProcurementResource

from .transplantation import OrganAllocation, Recipient
