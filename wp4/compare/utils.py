#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals
from __future__ import print_function

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


def procurement_report():
    """
    List of specified items, with a donor per line, CSV output
    """
    from django.utils.encoding import force_str
    from wp4.compare.models import Donor

    f = open("procurement_report.csv", "w")
    print(
        u"donor.trial_id, " +\
        u"donor.person.date_of_birth, " +\
        u"donor.person.gender, " +\
        u"donor.person.weight, " +\
        u"donor.person.height, " +\
        u"donor.person.get_ethnicity_display, " +\
        u"donor.person.get_blood_group_display, " +\
        u"donor.age, " +\
        u"donor.date_of_procurement, " +\
        u"donor.retrieval_team, " +\
        u"donor.retrieval_hospital, " +\
        u"donor.multiple_recipients, " +\
        u"donor.life_support_withdrawal, " +\
        u"donor.death_diagnosed, " +\
        u"donor.perfusion_started, " +\
        u"donor.get_systemic_flush_used_display, " +\
        u"donor.get_diagnosis_display, " +\
        u"donor.get_diabetes_melitus_display, " +\
        u"donor.alcohol_abuse, " +\
        u"donor.diuresis_last_day, " +\
        u"donor.last_creatinine".encode('utf8'),
        file=f
    )
    donors = Donor.objects.all()
    for donor in donors:
        out = u""
        out += donor.trial_id + u", "
        if donor.person.date_of_birth_unknown:
            dob = u"Unknown"
        elif donor.person.date_of_birth is not None:
            dob = donor.person.date_of_birth.strftime("%d-%m-%Y")
        else:
            dob = u""
        out += dob + u", "
        out += force_str(donor.person.gender) + u", "
        out += force_str(donor.person.weight) + u", "
        out += force_str(donor.person.height) + u", "
        out += force_str(donor.person.get_ethnicity_display()) + u", "
        out += force_str(donor.person.get_blood_group_display()) + u", "
        out += force_str(donor.age) + u", "
        dop = donor.date_of_procurement.strftime("%d-%m-%Y") if donor.date_of_procurement is not None else u""
        out += dop + u", "

        if donor.retrieval_team is not None:
            out += u"\"" + donor.retrieval_team.__unicode__() + u"\", "
        else:
            out += u", "
        if donor.retrieval_hospital is not None:
            out += u"\"" + donor.retrieval_hospital.__unicode__() + u"\", "
        else:
            out += u", "
        out += u"Y, " if donor.multiple_recipients else u"N, "
        lsw = donor.life_support_withdrawal.strftime("%d-%m-%Y %H:%M") if donor.life_support_withdrawal is not None else u""
        out += lsw + u", "
        dd = donor.death_diagnosed.strftime("%d-%m-%Y %H:%M") if donor.death_diagnosed is not None else u""
        out += dd + u", "
        if donor.perfusion_started_unknown:
            ps = u"Unknown"
        elif donor.perfusion_started is not None:
            ps = donor.perfusion_started.strftime("%d-%m-%Y %H:%M")
        else:
            ps = u""
        out += ps + u", "
        out += force_str(donor.get_systemic_flush_used_display()) + u", "

        out += force_str(donor.get_diagnosis_display()) + u", "
        out += force_str(donor.get_diabetes_melitus_display()) + u", "
        out += force_str(donor.alcohol_abuse) + u", "
        out += force_str(donor.diuresis_last_day) + u", "
        out += force_str(donor.last_creatinine) + u", "

        print(out.encode('utf8'), file=f)
        # print(out)
    f.close()


def organ_report():
    """
    List of specified items, with a organ/recipients per line, CSV output
    """
    from django.core.exceptions import ObjectDoesNotExist
    from django.utils.encoding import force_str
    from wp4.compare.models import Organ

    f = open("organ_report.csv", "w")
    print(
        u"organ.trial_id, " +
        u"organ.transplantable, " +
        u"organ.not_transplantable_reason, " +
        u"organ.removal, " +
        u"organ.renal_arteries, " +
        u"organ.get_graft_damage_display, " +
        u"organ.get_washout_perfusion_display, " +
        u"organ.perfusion_possible, " +
        u"organ.perfusion_not_possible_because, " +
        u"organ.perfusion_machine, " +
        u"organ.perfusion_started, " +
        u"organ.get_preservation_display, " +
        u"organ.recipient.person.date_of_birth, " +
        u"organ.recipient.person.gender, " +
        u"organ.recipient.person.weight, " +
        u"organ.recipient.person.height, " +
        u"organ.recipient.person.get_ethnicity_display, " +
        u"organ.recipient.person.get_blood_group_display, " +
        u"organ.recipient.person.get_renal_disease_display, " +
        u"organ.recipient.person.pre_transplant_diuresis, " +
        u"organ.recipient.perfusion_stopped, " +
        u"organ.recipient.organ_cold_stored, " +
        u"organ.recipient.removed_from_machine_at, " +
        u"organ.recipient.organ_untransplantable, " +
        u"organ.recipient.organ_untransplantable_reason, " +
        u"organ.recipient.knife_to_skin, " +
        u"organ.recipient.get_incision_display, " +
        u"organ.recipient.get_transplant_side_display, " +
        u"organ.recipient.get_arterial_problems_display, " +
        u"organ.recipient.get_venous_problems_display, " +
        u"organ.recipient.anastomosis_started_at, " +
        u"organ.recipient.reperfusion_started_at, " +
        u"organ.recipient.successful_conclusion, " +
        u"organ.recipient.operation_concluded_at, ".encode('utf8'),
        file=f
    )
    organs = Organ.objects.all()
    for organ in organs:
        out = u""
        out += organ.trial_id + u", "
        if organ.transplantable is None:
            out += u", "
        elif organ.transplantable:
            out += u"Y, "
        else:
            out += u"N, "
        out += u"\"" + organ.not_transplantable_reason + u"\", "
        removal = organ.removal.strftime("%d-%m-%Y") if organ.removal is not None else u""
        out += removal + u", "
        out += force_str(organ.renal_arteries) + u", "
        out += force_str(organ.get_graft_damage_display()) + u", "
        out += force_str(organ.get_washout_perfusion_display()) + u", "
        if organ.perfusion_possible is None:
            out += u", "
        elif organ.perfusion_possible:
            out += u"Y, "
        else:
            out += u"N, "
        out += u"\"" + organ.perfusion_not_possible_because + u"\", "
        if organ.perfusion_machine is not None:
            out += u"\"" + organ.perfusion_machine.__unicode__() + u"\", "
        else:
            out += u", "
        ps = organ.perfusion_started.strftime("%d-%m-%Y %H:%M") if organ.perfusion_started is not None else u""
        out += ps + u", "
        out += force_str(organ.get_preservation_display()) + u", "

        try:
            recipient = organ.recipient
            rp = recipient.person
            if rp.date_of_birth_unknown:
                dob = u"Unknown"
            elif rp.date_of_birth is not None:
                dob = rp.date_of_birth.strftime("%d-%m-%Y")
            else:
                dob = u""
            out += dob + u", "
            out += force_str(rp.gender) + u", "
            out += force_str(rp.weight) + u", "
            out += force_str(rp.height) + u", "
            out += force_str(rp.get_ethnicity_display()) + u", "
            out += force_str(rp.get_blood_group_display()) + u", "
            out += force_str(recipient.get_renal_disease_display()) + u", "
            out += force_str(recipient.pre_transplant_diuresis) + u", "

            ps = recipient.perfusion_stopped.strftime(
                "%d-%m-%Y %H:%M") if recipient.perfusion_stopped is not None else u""
            out += ps + u", "
            if recipient.organ_cold_stored:
                out += u"Y, "
            else:
                out += u"N, "
            rma = recipient.removed_from_machine_at.strftime(
                "%d-%m-%Y %H:%M") if recipient.removed_from_machine_at is not None else u""
            out += rma + u", "
            if recipient.organ_untransplantable is None:
                out += u", "
            elif recipient.organ_untransplantable:
                out += u"Y, "
            else:
                out += u"N, "
            out += u"\"" + recipient.organ_untransplantable_reason + u"\", "
            kts = recipient.knife_to_skin.strftime("%d-%m-%Y %H:%M") if recipient.knife_to_skin is not None else u""
            out += kts + u", "
            out += force_str(recipient.get_incision_display()) + u", "
            out += force_str(recipient.get_transplant_side_display()) + u", "
            out += force_str(recipient.get_arterial_problems_display()) + u", "
            out += force_str(recipient.get_venous_problems_display()) + u", "
            if recipient.anastomosis_started_at_unknown:
                ps = u"Unknown"
            elif recipient.anastomosis_started_at is not None:
                ps = recipient.anastomosis_started_at.strftime("%d-%m-%Y %H:%M")
            else:
                ps = u""
            out += ps + u", "
            if recipient.reperfusion_started_at_unknown:
                ps = u"Unknown"
            elif recipient.reperfusion_started_at is not None:
                ps = recipient.reperfusion_started_at.strftime("%d-%m-%Y %H:%M")
            else:
                ps = u""
            out += ps + u", "
            if recipient.successful_conclusion:
                out += u"Y, "
            else:
                out += u"N, "
            con = recipient.operation_concluded_at.strftime(
                "%d-%m-%Y %H:%M") if recipient.operation_concluded_at is not None else u""
            out += con + u", "
        except ObjectDoesNotExist:
            out += u"No Recipient,,,,,,,,,,,,,,,,,,,,,,, "

        print(out.encode('utf8'), file=f)
        # print(out)
    f.close()


def allocation_report():
    """
    List of specified items, with an OrganAllocation per line, CSV output
    """
    from django.utils.encoding import force_str
    from wp4.compare.models import OrganAllocation

    f = open("allocation_report.csv", "w")
    print(
        u"allocation.organ.trial_id, " +
        u"allocation.reallocated, " +
        u"allocation.transplant_hospital, " +
        u"allocation.get_reallocation_reason_display ".encode('utf8'),
        file=f
    )

    allocations = OrganAllocation.objects.all()
    for allocation in allocations:
        out = u""
        out += allocation.organ.trial_id + u", "
        if allocation.reallocated is None:
            out += u", "
        elif allocation.reallocated:
            out += u"Y, "
        else:
            out += u"N, "
        if allocation.transplant_hospital is not None:
            out += u"\"" + allocation.transplant_hospital.__unicode__() + u"\", "
        else:
            out += u", "
        out += force_str(allocation.get_reallocation_reason_display()) + u", "

        print(out.encode('utf8'), file=f)
        # print(out)
    f.close()
