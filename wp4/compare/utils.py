#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from wp4.compare.models import Donor
from wp4.compare.models import PAPER_EUROPE, PAPER_UNITED_KINGDOM, LIVE_UNITED_KINGDOM, LIVE_EUROPE
from wp4.compare.models import LEFT, RIGHT


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
            return [PAPER_EUROPE, PAPER_UNITED_KINGDOM]
        else:
            return [LIVE_UNITED_KINGDOM, LIVE_EUROPE]

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

    def __unicode__(self):
        return self.trial_id


def get_person_from_trial_id(trial_id_string):
    trial_object = TrialIDUtility(trial_id_string)
    if trial_object.is_organ:
        return trial_object.to_organ().recipient.person
    if trial_object.is_donor:
        return trial_object.to_donor().person
    return None
