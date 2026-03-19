using Microsoft.EntityFrameworkCore;


namespace LifeCare.Personnel.Infrastructure.Persistence;

public class PersonnelDbContext : DbContext
{
    public DbSet<Domain.Personnel> Personnel { get; set; } = null!;

    public PersonnelDbContext(DbContextOptions<PersonnelDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.Personnel>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(255);
            entity.HasIndex(p => p.Email)
                .IsUnique();

            entity.Property(p => p.Role)
                .HasConversion<string>()
                .IsRequired();

            entity.Property(p => p.Status)
                .HasConversion<string>()
                .IsRequired();

            entity.Property(p => p.Privileges)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim())
                        .ToList())
                .HasColumnType("text");
                

            entity.Property(p => p.CreatedAt)
                .IsRequired();

            entity.Property(p => p.UpdatedAt)
                .IsRequired();
            
            entity.Ignore(p => p.DomainEvents);



        });
    }
}