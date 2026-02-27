using LifeCare.Modules.Patients.Domain;
using LifeCare.Modules.Patients.Domain.Enums;
using LifeCare.Modules.Shared.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace LifeCare.Modules.Patients.Infrastructure.Persistence;

public class HospitalDbContext : DbContext
{
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<PatientStatusHistory> PatientStatusHistory { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<DomainEvent>();

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Ignore(p => p.DomainEvents);

            entity.Property(p => p.ShifNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(p => p.ShifNumber)
                .IsUnique();

            entity.Property(p => p.MRN)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(p => p.MRN)
                .IsUnique();

            entity.Property(p => p.NationalId)
                .HasMaxLength(50);

            entity.HasIndex(p => p.NationalId)
                .IsUnique()
                .HasFilter("\"NationalId\" IS NOT NULL");

            entity.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(100);
            entity.HasIndex(p => new { p.LastName, p.FirstName });


            entity.Property(p => p.DateOfBirth)
                .IsRequired();

            entity.Property(p => p.Gender)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(p => p.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);
            entity.HasIndex(p => p.PhoneNumber);


            entity.Property(p => p.Email)
                .HasMaxLength(255);

            entity.Property(p => p.County)
                .HasMaxLength(200);

            entity.Property(p => p.SubCounty)
                .HasMaxLength(100);

            entity.Property(p => p.Country)
                .HasMaxLength(50);

            entity.Property(p => p.ZipCode)
                .HasMaxLength(20);

            entity.Property(p => p.Status)
                .HasConversion<string>()
                .HasDefaultValue(PatientStatus.AwaitingTriage);
            entity.HasIndex(p => p.Status);


            entity.Property(p => p.CreatedAt)
                .IsRequired();

            entity.Property(p => p.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.GuardianName)
                .IsRequired(false)
                .HasMaxLength(100);

            entity.Property(p => p.GuardianRelationship)
                .IsRequired(false)
                .HasMaxLength(50);

            entity.Property(p => p.GuardianPhone)
                .IsRequired(false)
                .HasMaxLength(20);

            // Indexes
            entity.Property(p => p.Status)
                .HasConversion<string>()
                .HasDefaultValue(PatientStatus.AwaitingTriage);
            entity.HasIndex(p => new { p.Status, p.CreatedAt });

            //.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None); // for SQL Server
        });
        modelBuilder.Entity<PatientStatusHistory>(entity =>
        {
            entity.HasKey(h => h.Id);
            entity.Property(h => h.Status).HasConversion<string>();
            entity.HasIndex(h => h.PatientId);
            entity.HasIndex(h => h.ChangedAt);
        });
    }
}