#!/usr/bin/python
# coding: utf-8
from __future__ import absolute_import, unicode_literals


from django.contrib import admin

from wp4.compare.admin import VersionControlAdmin
from wp4.health_economics.models import QualityOfLife
from .models import FollowUpInitial, FollowUp3M, FollowUp6M, FollowUp1Y


class QualityOfLifeInline(admin.TabularInline):
    model = QualityOfLife
    fields = (
        'recipient', 
        'date_recorded', 
        'qol_mobility', 
        'qol_selfcare', 
        'qol_usual_activities',
        'qol_pain',
        'qol_anxiety',
        'vas_score',
    )
    can_delete = False
    extra = 1


class FollowUpInitialAdmin(VersionControlAdmin):
    fields = (
        'organ',  #FI01
        'start_date',  # FB01
        'graft_failure',  # FB10
        'graft_failure_date',  # FB11
        'graft_failure_type',  # FB12
        'graft_failure_type_other',  # FB13
        'graft_removal',  # FB14
        'graft_removal_date',  # FB15
        'serum_creatinine_unit',  #FI02
        'serum_creatinine_1',  #FI03
        'serum_creatinine_2',  #FI04
        'serum_creatinine_3',  #FI05
        'serum_creatinine_4',  #FI06
        'serum_creatinine_5',  #FI07
        'serum_creatinine_6',  #FI08
        'serum_creatinine_7',  #FI09
        'dialysis_requirement_1',  #FI10
        'dialysis_requirement_2',  #FI11
        'dialysis_requirement_3',  #FI12
        'dialysis_requirement_4',  #FI13
        'dialysis_requirement_5',  #FI14
        'dialysis_requirement_6',  #FI15
        'dialysis_requirement_7',  #FI16
        'dialysis_type',  # FB16
        'dialysis_cause',  #FI20
        'dialysis_cause_other',  #FI21
        'hla_mismatch_a',  #FI22
        'hla_mismatch_b',  #FI23
        'hla_mismatch_dr',  #FI24
        'induction_therapy',  #FI25
        'immunosuppression_1',  # FB30
        'immunosuppression_2',  # FB31
        'immunosuppression_3',  # FB32
        'immunosuppression_4',  # FB33
        'immunosuppression_5',  # FB34
        'immunosuppression_6',  # FB35
        'immunosuppression_7',  # FB36
        'immunosuppression_other',  # FB37
        'rejection',  # FB19
        'rejection_prednisolone',  # FB20
        'rejection_drug',  # FB21
        'rejection_drug_other',  # FB22
        'rejection_biopsy',  # FB23
        'calcineurin',  # FB24
        'discharge_date',  #FI26
        'notes',  # FB03
    )

admin.site.register(FollowUpInitial, FollowUpInitialAdmin)


class FollowUp3MAdmin(VersionControlAdmin):
    fields = (
        'organ',  # F301
        'start_date',  # FB01
        'graft_failure',  # FB10
        'graft_failure_date',  # FB11
        'graft_failure_type',  # FB12
        'graft_failure_type_other',  # FB13
        'graft_removal',  # FB14
        'graft_removal_date',  # FB15
        'serum_creatinine_unit',  # F310
        'serum_creatinine',  # F311
        'creatinine_clearance',  # F302
        'currently_on_dialysis',  # F303
        'dialysis_type',  # FB16
        'dialysis_date',  # F304
        'number_of_dialysis_sessions',  # F305
        'immunosuppression_1',  # FB30
        'immunosuppression_2',  # FB31
        'immunosuppression_3',  # FB32
        'immunosuppression_4',  # FB33
        'immunosuppression_5',  # FB34
        'immunosuppression_6',  # FB35
        'immunosuppression_7',  # FB36
        'immunosuppression_other',  # FB37
        'rejection',  # FB19
        'rejection_periods',  # F306
        'rejection_prednisolone',  # FB20
        'rejection_drug',  # FB21
        'rejection_drug_other',  # FB22
        'rejection_biopsy',  # FB23
        'calcineurin',  # FB24
        'graft_complications',  # F307
        'quality_of_life',  # F308
        'notes',  # FB03
    )
    # inlines = [QualityOfLifeInline]
    # Causes <class 'wp4.followups.admin.QualityOfLifeInline'>: (admin.E202) 'health_economics.QualityOfLife' has no ForeignKey to 'followups.FollowUp3M'. Error
    # However, can add 'quality_of_life' to field list and use pop up in admin

admin.site.register(FollowUp3M, FollowUp3MAdmin)


class FollowUp6MAdmin(VersionControlAdmin):
    fields = (
        'organ',  # F601
        'start_date',  # FB01
        'graft_failure',  # FB10
        'graft_failure_date',  # FB11
        'graft_failure_type',  # FB12
        'graft_failure_type_other',  # FB13
        'graft_removal',  # FB14
        'graft_removal_date',  # FB15
        'serum_creatinine_unit',  # F610
        'serum_creatinine',  # F611
        'creatinine_clearance',  # F602
        'currently_on_dialysis',  # F603
        'dialysis_type',  # FB16
        'dialysis_date',  # F604
        'number_of_dialysis_sessions',  # F605
        'immunosuppression_1',  # FB30
        'immunosuppression_2',  # FB31
        'immunosuppression_3',  # FB32
        'immunosuppression_4',  # FB33
        'immunosuppression_5',  # FB34
        'immunosuppression_6',  # FB35
        'immunosuppression_7',  # FB36
        'immunosuppression_other',  # FB37
        'rejection',  # FB19
        'rejection_periods',  # F606
        'rejection_prednisolone',  # FB20
        'rejection_drug',  # FB21
        'rejection_drug_other',  # FB22
        'rejection_biopsy',  # FB23
        'calcineurin',  # FB24
        'graft_complications',  # F607
        'notes',  # FB03
    )

admin.site.register(FollowUp6M, FollowUp6MAdmin)


class FollowUp1YAdmin(VersionControlAdmin):
    fields = (
        'organ',  # FY01
        'start_date',  # FB01
        'graft_failure',  # FB10
        'graft_failure_date',  # FB11
        'graft_failure_type',  # FB12
        'graft_failure_type_other',  # FB13
        'graft_removal',  # FB14
        'graft_removal_date',  # FB15
        'serum_creatinine_unit',  # FY10
        'serum_creatinine',  # FY11
        'creatinine_clearance',  # FY02
        'currently_on_dialysis',  # FY03
        'dialysis_type',  # FB16
        'dialysis_date',  # FY04
        'number_of_dialysis_sessions',  # FY05
        'immunosuppression_1',  # FB30
        'immunosuppression_2',  # FB31
        'immunosuppression_3',  # FB32
        'immunosuppression_4',  # FB33
        'immunosuppression_5',  # FB34
        'immunosuppression_6',  # FB35
        'immunosuppression_7',  # FB36
        'immunosuppression_other',  # FB37
        'rejection',  # FB19
        'rejection_periods',  # FY06
        'rejection_prednisolone',  # FB20
        'rejection_drug',  # FB21
        'rejection_drug_other',  # FB22
        'rejection_biopsy',  # FB23
        'calcineurin',  # FB24
        'graft_complications',  # FY07
        'quality_of_life',  # FY08
        'notes',  # FB03
    )
    # inlines = [QualityOfLifeInline]
    # Causes <class 'wp4.followups.admin.QualityOfLifeInline'>: (admin.E202) 'health_economics.QualityOfLife' has no ForeignKey to 'followups.FollowUp1Y'. Error
    # However, can add 'quality_of_life' to field list and use pop up in admin

admin.site.register(FollowUp1Y, FollowUp1YAdmin)

