#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals
from __future__ import print_function

from .models import Donor, Randomisation, Recipient
from .models import LEFT, RIGHT


def update_trial_ids_and_save(donor):
    """
    For a given donor record, update the trial ids
    :param donor: Compare.Donor record to be processed
    :return:
    """
    donor.trial_id = donor.make_trial_id()
    donor.save()
    donor.left_kidney.trial_id = donor.trial_id + 'L'
    donor.left_kidney.save()
    donor.right_kidney.trial_id = donor.trial_id + 'R'
    donor.right_kidney.save()


class TrialIDUtility(object):
    trial_id = None
    is_donor = False
    is_organ = False

    def _get_centre_code(self):
        return self.trial_id[3:5]

    def _get_sequence_number(self):
        if self.is_donor:
            return self.trial_id[-2:]
        elif self.is_organ:
            return self.trial_id[-3:-1]
        return None

    def _get_is_offline(self):
        offline_character = self.trial_id[5]
        if offline_character == "9":
            return True
        elif offline_character == "0":
            return False
        return None

    def _get_randomisation_list_codes(self):
        if self._get_is_offline():
            return [Randomisation.PAPER_EUROPE, Randomisation.PAPER_UNITED_KINGDOM]
        else:
            return [Randomisation.LIVE_UNITED_KINGDOM, Randomisation.LIVE_EUROPE]

    def _get_organ_location(self):
        last_character = self.trial_id[-1]
        if last_character == "L":
            return LEFT
        elif last_character == "R":
            return RIGHT
        return None

    def to_donor(self):
        """
        Return a Donor record for a given Trial ID. Should always work for any type of Trial ID
        :return: Donor record with matching Trial ID
        """

        results = Donor.objects.\
            filter(sequence_number=self._get_sequence_number()).\
            filter(retrieval_team__centre_code=self._get_centre_code()).\
            filter(randomisation__list_code__in=self._get_randomisation_list_codes())

        if results is None:
            raise Exception("No donor match found")
        else:
            return results[0]

    def to_organ(self):
        """
        Return an Organ record for a given Trial ID
        :return:
        """
        if not self.is_organ:
            raise Exception("Not an organ trial id")
        if self._get_organ_location() == LEFT:
            return self.to_donor().left_kidney
        elif self._get_organ_location() == RIGHT:
            return self.to_donor().right_kidney
        raise Exception("No organ match found")

    def validate_trial_id_string(self):
        trial_id_string = self.trial_id.lower()
        try:
            centre_code = int(trial_id_string[3:5])
            sequence_number = int(trial_id_string[5:8])
        except ValueError():
            print("DEBUG: centre code or sequence number are not ints for %s" % trial_id_string)
            return False
        if len(trial_id_string) > 9 or len(trial_id_string) < 8:
            print("DEBUG: %s is the wrong length" % trial_id_string)
            return False
        if trial_id_string[:3] != "wp4":
            print("DEBUG: %s does not start with wp4" % trial_id_string)
            return False
        if sequence_number < 1:
            print("DEBUG: sequence number invalid for %s" % trial_id_string)
            return False
        if centre_code < 1 or centre_code > 99:
            print("DEBUG: centre code invalid for %s" % trial_id_string)
            return False
        if self.is_organ:
            try:
                if self.to_organ():
                    return True
            except Exception:
                print("DEBUG: Organ record not found for %s" % trial_id_string)
                return False
        if self.is_donor:
            try:
                if self.to_donor():
                    return True
            except Exception:
                print("DEBUG: Donor record not found for %s" % trial_id_string)
                return False
        print("DEBUG: This should not occur...")
        return False

    def __init__(self, trial_id=None):
        print("DEBUG: __init__: trial_id=%s" % trial_id)
        if trial_id is not None:
            self.trial_id = trial_id
            if self._get_organ_location() is not None:
                self.is_organ = True
            else:
                self.is_donor = True
            if not self.validate_trial_id_string():
                raise TypeError("Not a valid Trial ID string")
        else:
            raise TypeError("Valid trail_id was not supplied")

    def __str__(self):
        return self.trial_id


def get_person_from_trial_id(trial_id_string):
    trial_object = TrialIDUtility(trial_id_string)
    if trial_object.is_organ:
        return trial_object.to_organ().recipient.person
    if trial_object.is_donor:
        return trial_object.to_donor().person
    return None


def get_donor_id_from_trial_id(trial_id_string):
    trial_object = TrialIDUtility(trial_id_string)
    if trial_object.is_donor:
        return trial_object.to_donor().id
    return None


def delete_donor(donor_pk):
    """
    Soft deletes (i.e. uses livefield) all the records that spawn from the specified Donor ID
    :param donor_pk: int, the primary key of a Donor record
    """
    # Work from the end of the dependencies backwards
    donor = Donor.objects.get(pk=donor_pk)

    for organ in [donor.left_kidney, donor.right_kidney]:
        for event in organ.event_set.all():
            if event.is_serious:
                print("WARNING: Serious Adverse Event (id:{0}) has been deleted".format(event.id))
            event.delete()

        for allocation in organ.organallocation_set.all():
            allocation.delete()

        for resource in organ.procurementresource_set.all():
            resource.delete()

        for sample in organ.perfusatesample_set.all():
            sample.event.delete()
            sample.delete()

        for sample in organ.tissuesample_set.all():
            sample.event.delete()
            sample.delete()

        if organ.safe_recipient is not None:
            organ.safe_recipient.delete()
        organ.delete()

    for sample in donor.person.bloodsample_set.all():
        sample.event.delete()
        sample.delete()

    for sample in donor.person.urinesample_set.all():
        sample.event.delete()
        sample.delete()

    donor.person.delete()
    donor.delete()


def delete_recipient(recipient_pk):
    """
    Soft deletes (i.e. uses livefield) all the records that spawn from the specified Recipient ID, e.g. for
    non-consented Recipients
    :param recipient_pk: int, the primary key of a Recipient record
    """
    # Work from the end of the dependencies backwards
    recipient = Recipient.objects.get(pk=recipient_pk)

    for qol in recipient.qualityoflife_set.all():
        qol.delete()

    for sample in recipient.person.bloodsample_set.all():
        sample.event.delete()
        sample.delete()

    recipient.organ.followup_initial.delete()
    recipient.organ.followup_3m.delete()
    recipient.organ.followup_6m.delete()
    recipient.organ.followup_1y.delete()

    recipient.person.delete()
    recipient.delete()
    print("DEBUG: delete_recipient({0}) completed".format(recipient_pk))
