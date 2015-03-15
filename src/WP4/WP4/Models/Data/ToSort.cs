using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WP4.Models.Data
{
    public class Trial
    {
        public int ID { get; set; }

        [Required]
        public int RetrievalTeam_ID { get; set; }
        [ForeignKey("RetrievalTeam_ID")]
        public virtual RetrievalTeam RetrievalTeam { get; set; }

        /*
        [Required]
        public int Donor_ID { get; set; }
        [ForeignKey("Donor_ID")]
        public virtual Donor Donor { get; set; }
         * */

        [Display(Name="a Maastricht category III DCD donor")]
        public bool CriteriaCheck1 { get; set; }
        [Display(Name="aged 50 years or older")]
        public bool CriteriaCheck2 { get; set; }
        [Display(Name="with both kidneys registered for donation")]
        public bool CriteriaCheck3 { get; set; }
        [Display(Name="from the collaborating donor regions reported to Eurotransplant (ET) / National Health Service Blood and Transplant (NHSBT)")]
        public bool CriteriaCheck4 { get; set; }

        [Display(Name="")]
        public bool IsActive { get; set; }
        
        [Display(Name="")]
        public string TrialID { get; set; }

        [Timestamp] 
        public Byte[] TimeStamp { get; set; }

        public DateTime CreatedOn { get; set; }
        public User CreatedBy { get; set; }
    }

    public class RetrievalTeam
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, Range(10,99)]
        public int CentreCode { get; set; }
    }

    public abstract class TrialPlus
    {
        public int ID { get; set; }

        [Required]
        public int Trial_ID { get; set; }
        [ForeignKey("Trial_ID")]
        public virtual Trial Trial { get; set; }
    
        // Concurrency checking - remember to handle the DbUpdateConcurrencyException if triggered
        // https://msdn.microsoft.com/en-gb/data/jj591583.aspx#TimeStamp
        [Timestamp] 
        public Byte[] TimeStamp { get; set; }

        public string Comments { get; set; }
        public bool IsDataLocked { get; set; }
        public DateTime DataLockedOn { get; set; }
        public User DataLockedBy { get; set; }
        public bool DataFinalised { get; set; }
        public DateTime DataFinalisedOn { get; set; }
        public User DataFinalisedBy { get; set; }
    }

    public class Donor : TrialPlus
    {
        [Required]
        [Display(Name="ET Donor number/ NHSBT Number")]
        public string Number { get; set; }

        public int Age { get; set; } // or DoB
        public DateTime DoB { get; set; }  // Not Known

        [Required, Display(Name="Date of admission")]
        public DateTime Admission { get; set; }

        [Required, Display(Name="Admitted to ITU")]
        public bool AdmittedToITU { get; set; }

        [Display(Name="Date admitted to ITU")]
        public DateTime DateAdmitedToITU { get; set; }

        [Required, Display(Name="Date of procurement")]
        public DateTime DateOfProcurement { get; set; }

        [Required, Display(Name="Gender")]
        public Gender Gender { get; set; }

        [Display(Name="Ethnicity")]
        public Ethnicity Ethnicity { get; set; }

        [Required, Display(Name="Weight (kg)"), Range(20,200)]
        public int Weight { get; set; }

        [Required, Display(Name="Height (cm)"), Range(100,250)]
        public int Height { get; set; }

        [NotMapped, Display(Name="BMI Derived Value")]
        public int BMI { get; set; }

        [Display(Name="Blood group")]
        public BloodGroup BloodGroup { get; set; }

        [Display(Name="Other organs offered for donation")]
        public virtual ICollection<OrgansOffered> OrgansOfferedForDonation { get; set; }
    }
        
    public class OrgansOffered
    {
        public int ID { get; set; }
        public OrgansForDonation Organ { get; set; }
        public Donor Donor { get; set; }
    }

    public class User
    {
        public int ID { get; set; }
        // Placeholder
    }

    public class Procedure : TrialPlus
    {
        public virtual PerfusionTechnician PerfusionTechnician { get; set; }
        public virtual TransplantCoordinator TransplantCoordinator { get; set; }
        public DateTime TCCallReceived { get; set; }
        public virtual RetrievalHospital RetrievalHospital { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime TechnicianArrival { get; set; }
        public DateTime IceBoxesFilled { get; set; }
        public DateTime DepartPerfusionCentre { get; set; }
        public DateTime ArrivalDonorHospital { get; set; }
    }

    public class PerfusionTechnician
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class TransplantCoordinator
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class RetrievalHospital
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive;
    }

    public class DonorPreop : TrialPlus // Should be donor
    {
        public Diagnosis Diagnosis { get; set; }
        public string DiagnosisOther { get; set; }
        public YesNo4 DiabetesMelitus { get; set; }
        public YesNo4 AlcoholAbuse { get; set; }
        public YesNo3 CardiacArrest { get; set; }
        public int SystolicBloodPressure { get; set; }
        public int DiastolicBloodPressure { get; set; }
        public YesNo3 Hypotensive { get; set; }
        public string DiuresisLastDay { get; set; }
        public bool DiuresisLastDay_IsUnknown { get; set; }
        public string DiuresisLastHour { get; set; }
        public bool DiuresisLastHour_IsUnknown { get; set; }
        public YesNo4 Dopamine { get; set; }
        public YesNo4 Dobutamine { get; set; }
        public YesNo4 Noradrenaline { get; set; }
        public YesNo4 Vasopressine { get; set; }
        public string OtherMedication1 { get; set; }
        public string OtherMedication2 { get; set; }
    }

    public class LabResult : TrialPlus // Should be Donor
    {
        public CreatineUnit LastCreatinineUnit { get; set; }
        public int LastCreatinine { get; set; }
        public CreatineUnit MaxCreatinineUnit { get; set; }
        public int MaxCreatinine { get; set; }
    }

    public class OperationData : TrialPlus // Extraction, Donor related
    {
        public DateTime LifeSupportWithdrawl { get; set; }
        public DateTime SystolicPressureLow { get; set; }
        public DateTime CirculatoryArrest { get; set; }
        public string LengthOfNoTouch { get; set; }
        public DateTime ConfirmationDeath { get; set; }
        public DateTime StartPerfusion { get; set; }
        public DateTime SystemicFlushUsed { get; set; }
        public YesNo3 Heparin { get; set; }

    }

    public class Inspection : TrialPlus
    {
        public DateTime LeftRemovalDate { get; set; }
        public DateTime RightRemovalDate { get; set; }
        public int LeftRenalArteries { get; set; }
        public int RightRenalArteries { get; set; }
        public GraftDamage LeftGraftDamage { get; set; }
        public GraftDamage RightGraftDamage { get; set; }
        public WashoutPerfusion LeftWashout { get; set; }
        public WashoutPerfusion RightWashout { get; set; }
        public bool LeftTransplantable { get; set; }
        public bool RightTransplantable { get; set; }
        public string LeftUntransplantableReason { get; set; }
        public string RightUntransplantableReason { get; set; }
        public bool RandomisationComplete { get; set; }
        public bool RandomisationOnline { get; set; }
        public KidneyPreservation LeftPreservation { get; set; }
        public KidneyPreservation RightPreservation { get; set; }
    }

    public class Randomisation : TrialPlus
    {
        public bool CanDonateLeft { get; set; }
        public bool CanDonateRight { get; set; }
        public bool CanTransplantLeft { get; set; }
        public bool CanTransplantRight { get; set; }
        public bool InclusionCriteria1 { get; set; }
        public KidneyPreservation LeftCategory { get; set; }
        public KidneyPreservation RightCategory { get; set; }
        // Identifier??
    }

    public class MachinePerfusion : TrialPlus
    {
        public Side Side { get; set; }
        public bool IsPerfusionPossible { get; set; }
        public string PerfusionNotPossibleBecause { get; set; }
        public DateTime Start { get; set; }
        public string MachineSerialNumber { get; set; }
        public string MachineReferenceModel { get; set; }
        public string PerfusionSolutionLotNumber { get; set; }
        public string DisposiblesLotNumber { get; set; }
        public PatchHolderSize UsedPatchHolder { get; set; }
        public bool ArtificalPatchUsed { get; set; }
        public ArtificalPatchSize ArtificalPatchSize { get; set; }
        public int ArtificialPatchNumber { get; set; }
        public bool OxygenBottleFull { get; set; }
        public bool OxygenBottle { get; set; }
        public bool OxygenTankChanged { get; set; }
        public DateTime OxygenTankChangedOn { get; set; }
        public bool IceContainerReplenished { get; set; }
        public DateTime IceContrainerReplenished { get; set; }
        public bool PerfusateMeasurable { get; set; }
        public int PerfusateMeasure { get; set; }
    }

    public class PerfusionFile
    {
        public int ID { get; set; }
        public virtual Trial Trial { get; set; }
        public string FileName { get; set; }
        public string IPAddress { get; set; }
        public string Comments { get; set; }
    }

    public class KidneyAllocation : TrialPlus
    {
        public Side Side { get; set; }
        public string TransplantTechnicianName { get; set; }
        public DateTime DonorTechnicianPhonedOn { get; set; }
        public string DonorProcedureTechnicianName { get; set; }
        public virtual TransplantHospital TransplantHospital { get; set; }
        public string TransplantHospitalContactName { get; set; }
        public string TransplantHospitalContactPhone { get; set; }
        public DateTime ScheduledTransplantStart { get; set; }
        public DateTime TechnicianArrivalPerfusionCentre { get; set; }
        public DateTime TechnicianDeparturePerfusionCentre { get; set; }
        public DateTime TransplantHospitalArrival { get; set; }
        public bool WasReallocated { get; set; }
        public ReallocationReason ReallocationReason { get; set; }
        public string ReallocationOtherReason { get; set; }
        public string NewRecipientHospitalContactName { get; set; }
        public string NewRecipientHospitalContactPhone { get; set; }
        public TransplantHospital NewTransplantHospital { get; set; }
        public DateTime NewScheduledTransplantStart { get; set; }
        public DateTime NewTransplantHospitalArrival { get; set; }
        public string NewComment { get; set; }
    }

    public class TransplantHospital
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Recipient : TrialPlus
    {
        public Side Side { get; set; }
        public TransplantHospital TransplantCentre { get; set; }
        public bool Consented { get; set; }
        public bool IsOver18 { get; set; }
        public bool IsDualTransplant { get; set; }
        public string RecipientTrialID { get; set; }
        public string NHSBTNumber { get; set; }
        public DateTime DoB { get; set; }
        public Gender Gender { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public int BMI { get; set; }
        public Ethnicity Ethnicity { get; set; }
        public RenalDisease RenalDisease { get; set; }
        public string OtherRenalDisease { get; set; }
        public int PreTransplantDiuresis { get; set; }
        public BloodGroup BloodGroup { get; set; }
    }

    public abstract class RecipientPlus
    {
        public int ID { get; set; }
        public virtual Recipient Recipient { get; set; }
    
        public string Comments { get; set; }
        public bool IsDataLocked { get; set; }
        public DateTime DataLockedOn { get; set; }
        public User DataLockedBy { get; set; }
        public bool DataFinalised { get; set; }
        public DateTime DataFinalisedOn { get; set; }
        public User DataFinalisedBy { get; set; }
    }

    public class RecipientPeriOperativeData : RecipientPlus
    {
        public DateTime TranplantationDate { get; set; }
        public bool PerfusateMeasurable { get; set; }
        public int PerfusateMeasure { get; set; }
        public DateTime PerfusateMachineStopped { get; set; }
        public bool TapeBroken { get; set; }
        public DateTime KidneyRemovedFromMachineOn { get; set; }
        public bool OxygenBottleFullAndOpen { get; set; }
        public bool KidneyDiscarded { get; set; }
        public string KidneyDiscardedReason { get; set; }
        public DateTime OperationStartedOn { get; set; }
        public Incision Incision { get; set; }
        public Side TransplantSide { get; set; }
        public ArterialProblems ArterialProblem { get; set; }
        public string OtherArterialProblem { get; set; }
        public VenousProblems VenousProblem  { get; set; }
        public string OtherVenousProblem { get; set; }
        public DateTime StartAnastomosisOn { get; set; }
        public DateTime ReperfusionOn { get; set; }
        public YesNo3 MannitolUsed { get; set; }
        public YesNo3 DiureticsUsed { get; set; }
        public string DiureticsUsedReason { get; set; }
        public YesNo3 HypnotensivePeriod { get; set; }
        public int SystolicBloodPressure { get; set; }
        public string CVPReperfusion { get; set; }
        public YesNo3 IntraoperativeDiuresis { get; set; }

    }

    public class RecipientFollowUpDay : RecipientPlus
    {
        public FollowupDay Day { get; set; }
        public DateTime Date { get; set; }
        public YesNo3 GraftFailure { get; set; }
        public DateTime GraftFailureOn { get; set; }
        public GraftFailureCause GraftFailureCause { get; set; }
        public string OtherGraftFailureCause { get; set; }
        public bool GraftRemoved { get; set; }
        public DateTime GraftRemovedOn { get; set; }
        public CreatineUnit CreatineUnit { get; set; }
        public int SerumCreatine { get; set; }
        public bool NeededDialysis { get; set; }
        public DialysisType DialysisType { get; set; }
        public YesNo3 RequiredHyperkalemia { get; set; }
        public int HLAMismatchA { get; set; }
        public int HLAMismatchB { get; set; }
        public int HLAMismatchDR { get; set; }
        public InductionTherapy InductionTherapy { get; set; }
        public Immunosuppressive PostTxImmunosuppressive { get; set; }
        public string OtherPostTxImmunosuppressive { get; set; }
        public bool Rejected { get; set; }
        public bool PostTxPrednisolon { get; set; }
        public bool PostTxOther { get; set; }
        public string PostTxOtherName { get; set; }
        public bool RejectionBiopsyProven { get; set; }
        public bool CalcineurinToxicity { get; set; }
        public DateTime PrimaryPostTxDischarge { get; set; }
    }

    public class RecipientFollowUp : RecipientPlus
    {
        public FollowupOccasion Occasion { get; set; }
        public DateTime Date { get; set; }
        public YesNo3 GraftFailure { get; set; }
        public DateTime GraftFailureOn { get; set; }
        public GraftFailureCause GraftFailureCause { get; set; }
        public string OtherGraftFailureCause { get; set; }
        public bool GraftRemoved { get; set; }
        public DateTime GraftRemovedOn { get; set; }
        public CreatineUnit CreatineUnit { get; set; }
        public int SerumCreatine { get; set; }
        public UrineUnit UrineUnit { get; set; }
        public int UrineCreatine { get; set; }
        public CreatineClearanceUnit CreatineClearanceUnit { get; set; }
        public int CreatineClearance { get; set; }
        public bool CurrentlyOnDialysis { get; set; }
        public DialysisType DialysisType { get; set; }
        public DateTime LastDialysisDate { get; set; }
        public int NumberOfDialysisSessions { get; set; }
        public InductionTherapy InductionTherapy { get; set; }
        public Immunosuppressive PostTxImmunosuppressive { get; set; }
        public string OtherPostTxImmunosuppressive { get; set; }
        public bool Rejected { get; set; }
        public bool PostTxPrednisolon { get; set; }
        public bool PostTxOther { get; set; }
        public string PostTxOtherName { get; set; }
        public bool RejectionBiopsyProven { get; set; }
        public bool CalcineurinToxicity { get; set; }
        public string ComplicationsWithGraftFunction { get; set; }
    }

    public class ResourceUseLog : RecipientPlus // See Resource Use log
    {
        public FollowupOccasion Occasion { get; set; }
        public string GPAppointment { get; set; }
        public string GPHomeVisit { get; set; }
        public string GPTelephoneConversation { get; set; }
        public string SpecialistAppointment { get; set; }
        public string TreatedAE { get; set; }
        public string AmbulanceToAE { get; set; }
        public string NurseHomeVisit { get; set; }
        public string NursePracticeAppointment { get; set; }
        public string PhysiotherapistAppointment { get; set; }
        public string OccupationalTherapistAppointment { get; set; }
        public string PsychologistAppointment { get; set; }
        public string CounsellorAppointment { get; set; }
        public string AttendedDayHospital { get; set; }
    }

    public class QualityOfLife : RecipientPlus
    {
        public DateTime FilledOn { get; set; }
        public QOLLocation FilledAt { get; set; }
        public QOLRating Mobility { get; set; }
        public QOLRating SelfCare { get; set; }
        public QOLRating UsualActivities { get; set; }
        public QOLRating Pain { get; set; }
        public QOLRating Anxiety { get; set; }
        public int VAScore { get; set; }
    }

    public class AdverseEvent
    {
        public int ID { get; set; }
        public virtual Recipient Recipient { get; set; }
        public bool Death { get; set; }
        public bool Injury { get; set; }
        public bool Impairment { get; set; }
        public bool Rehospitalisation { get; set; }
        public bool Prolongation { get; set; }
        public bool Intervention { get; set; }
    }

    public class AdverseEventDetail : RecipientPlus
    {
        public DateTime DateOf { get; set; }
        public AdverseEventType Type { get; set; }
        public InfectionType InfectionType { get; set; }
        public string OtherInfectionType { get; set; }
        public YesNo4 OrganismBacteria { get; set; }
        public string OrganismBacteriaDetail { get; set; }
        public YesNo4 OrganismViral { get; set; }
        public string OrganismViralDetail { get; set; }
        public YesNo4 OrganismFungal { get; set; }
        public string OrganismFungalDetail { get; set; }
        public BanffGrading BiopsyRejection { get; set; }
        public string ReOperationDetails { get; set; }
        public string OtherAdverseEvent { get; set; }
        public ClavianGrading ClavienGrading { get; set; }
    }

    public class SeriousAdverseEventDetail : RecipientPlus
    {
        public DateTime Onset { get; set; }
        public YesNo4 Ongoing { get; set; }
        public DateTime Resolution { get; set; }
        public string Description { get; set; }
        public string ActionTaken { get; set; }
        public string Outcome { get; set; }
        public string ContactDetails { get; set; }
        public bool Incapacity { get; set; }
        public bool Interferes { get; set; }
        public bool NoSequelae { get; set; }
        public bool DeviceDeficiency { get; set; }
        public bool DeviceUserError { get; set; }
        public DateTime DoD { get; set; }
        public bool DeathTxRelated { get; set; }
        public CauseOfDeath CauseOfDeath { get; set; }
        public string OtherCauseOfDeath { get; set; }
        public DateTime AdmissionDate { get; set; }
        public bool ICU { get; set; }
        public bool NeededDialysis { get; set; }
        public bool BiopsyTaken { get; set; }
        public bool HadSurgery { get; set; }
    }

    public class RecipientWithdrawal : RecipientPlus
    {
        public DateTime WithdrawnOn { get; set; }
        public WithdrawalReason Reason { get; set; }
        public string OtherReason { get; set; }
    }

    public class Specimen : TrialPlus
    {
        public virtual Recipient Recipent { get; set; }
        public bool AdditionalSampleConsented { get; set; }
        public string WorksheetBarcode { get; set; }
        public DateTime ReperfusionOn { get; set; }
        public string ResearcherName { get; set; }
        public string ResearcherEmail { get; set; }
        public bool Collected { get; set; }
        public string SpecimenBarcode { get; set; }
        public SpecimenType Type { get; set; }
        public SpecimenOccasion Occasion { get; set; }
        public DateTime CollectedOn { get; set; }
        public DateTime Centrefugation { get; set; }
        public SpecimenState State { get; set; }
    }

}


