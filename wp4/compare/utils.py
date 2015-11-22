#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals

from .models import Organ, Donor


def get_person_from_trial_id(trial_id_string):
    if validate_trial_id_string(trial_id_string):
        centre_code = int(trial_id_string[3:5])
        sequence_number = int(trial_id_string[5:8])
        last_character = trial_id_string[-1]
        if last_character == "l" or last_character == "r":
            organ = Organ.objects.filter(donor__retrieval_team__centre_code=centre_code)\
                .filter(donor__sequence_number__exact=sequence_number).filter(location=last_character.upper())
            if len(organ) > 1:
                # TODO: Raise an exception here!
                return None
            # TODO: Add likely exception handling here!
            if organ[0].recipient:
                return organ[0].recipient.person
        else:
            donor = Donor.objects.filter(retrieval_team__centre_code=centre_code)\
                .filter(sequence_number=sequence_number)
            if len(donor) > 1:
                # TODO: Raise an exception here!
                return None
            return donor[0].person
    return None


def validate_trial_id_string(trial_id_string):
    trial_id_string = trial_id_string.lower()
    centre_code = int(trial_id_string[3:5])
    sequence_number = int(trial_id_string[5:8])
    if len(trial_id_string) > 9 or len(trial_id_string) < 8:
        return False
    if trial_id_string[:3] != "wp4":
        return False
    if sequence_number < 1:
        return False
    last_character = trial_id_string[-1]
    if last_character == "l" or last_character == "r":
        # Search the Recipients
        organ = Organ.objects.filter(donor__retrieval_team__centre_code=centre_code)\
            .filter(donor__sequence_number__exact=sequence_number).filter(location=last_character.upper())
        print("DEBUG: Organ=%s" % organ)
        if organ is None:
            return False
        # TODO: What happens if there's more than one??
    else:
        # Search the Donors
        donor = Donor.objects.filter(retrieval_team__centre_code=centre_code)\
            .filter(sequence_number=sequence_number)
        print("DEBUG: Donor=%s" % donor)
        if donor is None:
            return False
    return True