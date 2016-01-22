#!/usr/bin/python
# coding: utf-8
from django.contrib import admin
from django.utils import timezone
from reversion_compare.admin import CompareVersionAdmin

from .models import FollowUpInitial, FollowUp3M, FollowUp6M, FollowUp1Y


class VersionControlAdmin(CompareVersionAdmin):
    exclude = ('version', 'created_on', 'created_by', 'record_locked')

    def save_model(self, request, obj, form, change):
        # TODO: this will have to respect the version control work later...
        obj.created_by = request.user
        obj.created_on = timezone.now()
        obj.version += 1
        obj.save()


class FollowUpInitialAdmin(VersionControlAdmin):
    list_display = ('trial_id', 'start_date', 'completed')
    ordering = ('completed', 'start_date')
    fields = (
        'organ', 'start_date', 'graft_failure', 'graft_failure_date', 'graft_failure_type', 'graft_failure_type_other',
        'graft_removal', 'graft_removal_date',
        'serum_creatinine_1', 'serum_creatinine_1_unit',
        'serum_creatinine_2', 'serum_creatinine_2_unit',
        'serum_creatinine_3', 'serum_creatinine_3_unit',
        'serum_creatinine_4', 'serum_creatinine_4_unit',
        'serum_creatinine_5', 'serum_creatinine_5_unit',
        'serum_creatinine_6', 'serum_creatinine_6_unit',
        'serum_creatinine_7', 'serum_creatinine_7_unit',
        'dialysis_requirement_1', 'dialysis_requirement_2', 'dialysis_requirement_3',
        'dialysis_requirement_4', 'dialysis_requirement_5', 'dialysis_requirement_6',
        'dialysis_requirement_7', 'dialysis_type', 'dialysis_cause', 'dialysis_cause_other',
        'hla_mismatch_a', 'hla_mismatch_b', 'hla_mismatch_dr', 'induction_therapy',
        'immunosuppression', 'immunosuppression_other',
        'rejection', 'rejection_prednisolone', 'rejection_drug', 'rejection_drug_other',
        'rejection_biopsy', 'calcineurin', 'discharge_date', 'notes', 'completed'
    )

admin.site.register(FollowUpInitial, FollowUpInitialAdmin)


class FollowUp3MonthAdmin(VersionControlAdmin):
    list_display = ('trial_id', 'start_date', 'completed')
    ordering = ('completed', 'start_date')
    fields = (
        'organ', 'start_date', 'graft_failure', 'graft_failure_date', 'graft_failure_type', 'graft_failure_type_other',
        'graft_removal', 'graft_removal_date',
        'serum_creatinine_1', 'serum_creatinine_1_unit',
        'urine_creatinine', 'urine_creatinine_unit',
        'creatinine_clearance', 'currently_on_dialysis', 'dialysis_type',
        'dialysis_requirement_1', 'number_of_dialysis_sessions',
        'immunosuppression', 'immunosuppression_other',
        'rejection', 'rejection_periods', 'rejection_prednisolone', 'rejection_drug', 'rejection_drug_other',
        'rejection_biopsy', 'calcineurin', 'graft_complications',
        'qol_mobility', 'qol_selfcare', 'qol_usual_activities', 'qol_pain', 'qol_anxiety', 'vas_score',
        'notes', 'completed'
    )

admin.site.register(FollowUp3M, FollowUp3MonthAdmin)


class FollowUp6MonthAdmin(VersionControlAdmin):
    list_display = ('trial_id', 'start_date', 'completed')
    ordering = ('completed', 'start_date')
    fields = (
        'organ', 'start_date', 'graft_failure', 'graft_failure_date', 'graft_failure_type', 'graft_failure_type_other',
        'graft_removal', 'graft_removal_date',
        'serum_creatinine_1', 'serum_creatinine_1_unit',
        'urine_creatinine', 'urine_creatinine_unit',
        'creatinine_clearance', 'currently_on_dialysis', 'dialysis_type',
        'dialysis_requirement_1', 'number_of_dialysis_sessions',
        'immunosuppression', 'immunosuppression_other',
        'rejection', 'rejection_periods', 'rejection_prednisolone', 'rejection_drug', 'rejection_drug_other',
        'rejection_biopsy', 'calcineurin', 'graft_complications',
        'notes', 'completed'
    )

admin.site.register(FollowUp6M, FollowUp6MonthAdmin)


class FollowUp1YearAdmin(VersionControlAdmin):
    # This is essentially a duplicate of the 3M admin model
    list_display = ('trial_id', 'start_date', 'completed')
    ordering = ('completed', 'start_date')
    fields = (
        'organ', 'start_date', 'graft_failure', 'graft_failure_date', 'graft_failure_type', 'graft_failure_type_other',
        'graft_removal', 'graft_removal_date',
        'serum_creatinine_1', 'serum_creatinine_1_unit',
        'urine_creatinine', 'urine_creatinine_unit',
        'creatinine_clearance', 'currently_on_dialysis', 'dialysis_type',
        'dialysis_requirement_1', 'number_of_dialysis_sessions',
        'immunosuppression', 'immunosuppression_other',
        'rejection', 'rejection_periods', 'rejection_prednisolone', 'rejection_drug', 'rejection_drug_other',
        'rejection_biopsy', 'calcineurin', 'graft_complications',
        'qol_mobility', 'qol_selfcare', 'qol_usual_activities', 'qol_pain', 'qol_anxiety', 'vas_score',
        'notes', 'completed'
    )

admin.site.register(FollowUp1Y, FollowUp1YearAdmin)

