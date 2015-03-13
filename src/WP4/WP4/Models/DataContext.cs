
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using WP4.Models.Data;

namespace WP4.DAL
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DataContext")
        {
        
        }

        public DbSet<Trial> Trials { get; set; }
        public DbSet<RetrievalTeam> RetrievalTeams { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<OrgansOffered> OrganOfferings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<PerfusionTechnician> PerfusionTechnicians { get; set; }
        public DbSet<TransplantCoordinator> TransplantCoordinators { get; set; }
        public DbSet<RetrievalHospital> RetrievalHospitals { get; set; }
        public DbSet<DonorPreop> DonorPreops { get; set; }
        public DbSet<LabResult> LabResults { get; set; }
        public DbSet<OperationData> OperationData { get; set; }
        public DbSet<Inspection> Inspections { get; set; }
        public DbSet<Randomisation> Randomisations { get; set; }
        public DbSet<MachinePerfusion> MachinePerfusions { get; set; }
        public DbSet<PerfusionFile> PerfusionFiles { get; set; }
        public DbSet<KidneyAllocation> KidneyAllocations { get; set; }
        public DbSet<TransplantHospital> TransplantHospitals { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<RecipientPeriOperativeData> RecipientPeriOperativeData { get; set; }
        public DbSet<RecipientFollowUpDay> RecipientFollowUpDays { get; set; }
        public DbSet<RecipientFollowUp> RecipientFollowUps { get; set; }
        public DbSet<ResourceUseLog> ResourceUseLogs { get; set; }
        public DbSet<QualityOfLife> QualityOfLife { get; set; }
        public DbSet<AdverseEvent> AdverseEvents { get; set; }
        public DbSet<AdverseEventDetail> AdverseEventDetails { get; set; }
        public DbSet<SeriousAdverseEventDetail> SeriousAdverseEventDetails { get; set; }
        public DbSet<RecipientWithdrawal> RecipientWithdrawals { get; set; }
        public DbSet<Specimen> Specimens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}