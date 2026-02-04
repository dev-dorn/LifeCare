using LifeCare.Domain.Common;
using Microsoft.EntityFrameworkCore;
using LifeCare.Domain.Patients;

namespace LifeCare.Infrastructure.Persistence
{
    public class HospitalDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<DomainEvent>();
            
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Ignore(p => p.DomainEvents);
                
                entity.Property(p => p.MRN)
                    .IsRequired()
                    .HasMaxLength(50);
                    
                entity.HasIndex(p => p.MRN)
                    .IsUnique();
                    
                entity.Property(p => p.NationalId)
                    .IsRequired()
                    .HasMaxLength(50);
                    
                entity.HasIndex(p => p.NationalId)
                    .IsUnique();
                    
                entity.Property(p => p.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.Property(p => p.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.Property(p => p.DateOfBirth)
                    .IsRequired();
                    
                entity.Property(p => p.Gender)
                    .IsRequired()
                    .HasMaxLength(10);
                    
                entity.Property(p => p.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);
                    
                entity.Property(p => p.Email)
                    .HasMaxLength(255);
                    
                entity.Property(p => p.Street)
                    .HasMaxLength(200);
                    
                entity.Property(p => p.City)
                    .HasMaxLength(100);
                    
                entity.Property(p => p.State)
                    .HasMaxLength(50);
                    
                entity.Property(p => p.ZipCode)
                    .HasMaxLength(20);
                    
                entity.Property(p => p.Status)
                    .HasConversion<string>()
                    .HasDefaultValue(PatientStatus.AwaitingTriage);
                    
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
                    //.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None); // for SQL Server

                entity.HasIndex(p => new { p.LastName, p.FirstName });
                entity.HasIndex(p => p.CreatedAt);
            });
        }
    }
}