using Microsoft.EntityFrameworkCore;

namespace DealRept.Models
{
    public class DealDbContext : DbContext
    {
        public DealDbContext(DbContextOptions<DealDbContext> options)
            : base(options)
        {

        }
        public DbSet<City> Cities { get; set; }

        public DbSet<Branch> Branches { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Bank> Banks { get; set; }

        public DbSet<Specialization> Specializations { get; set; }

        public DbSet<ContractStatus> ContractStatuses { get; set; }

        public DbSet<CurrentDocument> CurrentDocuments { get; set; }

        public DbSet<PastDocument> PastDocuments { get; set; }

        public DbSet<CurrentContract> CurrentContracts { get; set; }

        public DbSet<PastContract> PastContracts { get; set; }

        public DbSet<CurrentEvent> CurrentEvents { get; set; }

        public DbSet<PastEvent> PastEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>().HasOne(e => e.City).WithMany(e => e.Branches).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Branch>().HasAlternateKey(e => e.Code);

            modelBuilder.Entity<Supplier>().HasOne(e => e.Bank).WithMany(e => e.Suppliers).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Supplier>().HasOne(e => e.City).WithMany(e => e.Suppliers).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Supplier>().HasOne(e => e.Specialization).WithMany(e => e.Suppliers).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Supplier>().HasAlternateKey(e => e.LegalCode);

            modelBuilder.Entity<CurrentDocument>().HasOne(e => e.CurrentContract).WithMany(e => e.CurrentDocuments).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CurrentDocument>().HasAlternateKey(d => new { d.Name, d.CurrentContractID });

            modelBuilder.Entity<CurrentContract>().Property(b => b.IsProlonged).HasDefaultValue(0);
            modelBuilder.Entity<CurrentContract>().HasOne(e => e.Branch).WithMany(e => e.CurrentContracts).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<CurrentContract>().HasOne(e => e.Supplier).WithMany(e => e.CurrentContracts).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<CurrentContract>().HasOne(e => e.ContractStatus).WithMany(e => e.CurrentContracts).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<CurrentContract>().HasAlternateKey(e => new { e.ContractNumber, e.ConclusionDate });
            modelBuilder.Entity<CurrentContract>(entity => entity.HasCheckConstraint("CHK_TermDate", "ExpirationDate > ConclusionDate"));

            modelBuilder.Entity<PastContract>().HasOne(e => e.Branch).WithMany(e => e.PastContracts).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<PastContract>().HasOne(e => e.Supplier).WithMany(e => e.PastContracts).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<PastContract>().HasOne(e => e.ContractStatus).WithMany(e => e.PastContracts).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<PastContract>().HasAlternateKey(e => new { e.ContractNumber, e.ConclusionDate });
            modelBuilder.Entity<PastContract>(entity => entity.HasCheckConstraint("CHK_TransDate", "TransitionDate >= ConclusionDate"));

            modelBuilder.Entity<PastDocument>().HasOne(e => e.PastContract).WithMany(e => e.PastDocuments).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bank>().HasAlternateKey(e => e.Code);
            //modelBuilder.Entity<Bank>().HasAlternateKey(e => e.Name);

            //modelBuilder.Entity<City>().HasAlternateKey(e => e.Name);

            modelBuilder.Entity<PastEvent>().HasOne(e => e.PastContract).WithMany(e => e.PastEvents).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CurrentEvent>().HasOne(e => e.CurrentContract).WithMany(e => e.CurrentEvents).OnDelete(DeleteBehavior.Cascade);

        }

    }
}
