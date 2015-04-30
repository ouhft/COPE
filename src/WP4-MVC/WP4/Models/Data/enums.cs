using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WP4.Models.Data
{
    public enum Gender
    {
        Male = 1,
        Female = 2
    }

    public enum Ethnicity
    {
        Caucasian = 1,
        Black = 2,
        Other = 3
    }

    public enum BloodGroup
    {
        O = 1,
        A = 2,
        B = 3,
        AB = 4,
        Unknown = 5
    }

    public enum OrgansForDonation
    {
        Liver = 1,
        Lung = 2,
        Pancreas = 3
    }

    public enum Diagnosis
    {
        CerebrivascularAccident = 1,
        Hypoxia = 2,
        Trauma = 3,
        Other = 4
    }

    public enum YesNo4
    {
        No = 0,
        Yes = 1,
        Unknown = 2,
        Unanswered = 3
    }

    public enum YesNo3
    {
        No = 0,
        Yes = 1,
        Unanswered = 3
    }
    public enum CreatineUnit
    {
        mgDL = 1,
        umolL = 2
    }

    public enum GraftDamage
    {
        ArterialDamage = 1,
        VenousDamage = 2,
        UreteralDamage = 3,
        ParenchymalDamage = 4,
        None = 5
    }

    public enum WashoutPerfusion
    {
        Homegenous = 1,
        Patchy = 2,
        Blue = 3,
        Unknown = 4
    }

    public enum KidneyPreservation
    {
        HMP = 1,
        HMPO2 = 2
    }

    public enum Side
    {
        Left = 1,
        Right = 2
    }

    public enum PatchHolderSize
    {
        Small = 1,
        Large = 2,
        DoubleArtery = 3
    }

    public enum ArtificalPatchSize
    {
        Small = 1,
        Large = 2
    }

    public enum ReallocationReason
    {
        PositiveCrossmatch = 1,
        Other = 2,
        Unknown = 3
    }
    public enum RenalDisease
    {
        GlomerularDiseases = 1,
        PolycysticKidneys = 2,
        UncertainEtiology = 3,
        TubularAndInterstitialDiseases = 4,
        RetransplantGraftFailure = 5,
        DiabeticNephropathyes = 6,
        HypertensiveNephroangiosclerosis = 7,
        CongenitalRareFamilialMetabolicDisorders = 8,
        RenovascularAndOtherRenalVascularDiseases = 9,
        Neoplasms = 10,
        Other = 11,
    }

    public enum Incision
    {
        MidlineLaparotomy = 1,
        Extraperitoneal = 2, // ie Hockey Stick Incision
        Unknown = 3
    }

    public enum ArterialProblems
    {
        None = 1,
        LigatedPolarArtery = 2,
        ReconstructedPolarOrHilarArtery = 3,
        RepairedIntimaDissection = 4,
        Other = 5
    }

    public enum VenousProblems
    {
        None = 1,
        Laceration = 2,
        ElongationPlasty = 3
    }

    public enum FollowupDay
    {
        Day1 = 1,
        Day2 = 2,
        Day3 = 3,
        Day4 = 4,
        Day5 = 5,
        Day6 = 6,
        Day7 = 7
    }

    public enum GraftFailureCause
    {
        Immunologic = 1,
        Preservation = 2,
        TechnicalArterial = 3,
        TechnicalVenous = 4,
        InfectionBacterial = 5,
        InfectionViral = 6,
        Other = 7
    }

    public enum DialysisType
    {
        CAPD = 1,
        Hemodialysis = 2,
        Unknown = 3
    }

    public enum InductionTherapy
    {
        IL2receptorAntagonist = 1,
        ATG = 2
    }

    public enum Immunosuppressive
    {
        Azathioprine = 1,
        Cyclosporin = 2,
        MMF = 3,
        Prednisolone = 4,
        Sirolomus = 5,
        Tacrolimus = 6,
        Other = 7
    }


    public enum FollowupOccasion
    {
        ThreeMonths = 1,
        SixMonths = 2,
        Year = 3
    }

    public enum UrineUnit
    {
        gDL = 1,
        mmolL = 2
    }

    public enum CreatineClearanceUnit
    {
        ml = 1
    }

    public enum QOLLocation
    {
        Inpatient = 1,
        Outpatient = 2,
        Community = 3
    }

    public enum QOLRating
    {
        NoProblems = 1,
        SlightProblems = 2,
        ModerateProblems = 3,
        SevereProblems = 4,
        Unable = 5,
        Unknown = 9
    }

    public enum AdverseEventType
    {
        RecipientInfection,
        BiopsyProvenAcuteRejection,
        Bleeding,
        Lymphocoele,
        HepaticArteryThrombosis,
        PortalVeinThrombosis,
        HepaticArteryStenosis,
        PortalVeinStenosis,
        Reoperation,
        UrinaryLeak,
        UretherStricture,
        UretherNecrosis,
        Other
    }

    public enum InfectionType
    {
        Pneumonia,
        SurgicalSiteInfection,
        IntraabdominalCollection,
        Other
    }

    public enum BanffGrading
    {
        Indeterminate,
        Mild,
        Moderate,
        Severe
    }

    public enum ClavianGrading
    {
        I,
        II,
        IIa,
        IIb,
        IIIa,
        IIIb,
        IVa,
        IVb,
        V
    }

    public enum CauseOfDeath
    {
        MI,
        PE,
        CVA,
        Pneumonia,
        Sepsis,
        MultiOrganFailure,
        TransplantRelated
    }


    public enum WithdrawalReason
    {
        WithdrawnConsent,
        WithdrawnClinician,
        Other
    }

    public enum SpecimenType
    {
        EDTA,
        SST,
        Urine,
        Perfusate,
        Kidney
    }

    public enum SpecimenOccasion
    {
        DB1_1,
        DB1_2,
        DU1_1,
        DU2_1,
        RKP1_1,
        LKP1_1,
        RKP2_1,
        P3_1,
        RB1_1,
        RB1_2,
        RB2_1,
        RB2_2,
        KT1R,
        KT1F
    }

    public enum SpecimenState
    {
        DryIce,
        RNALater,
        Formalin
    }

}