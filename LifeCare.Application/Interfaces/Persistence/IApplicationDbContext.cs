using LifeCare.Domain.Patients;
using Microsoft.EntityFrameworkCore;

namespace LifeCare.Application.Interfaces.Persistence;

public interface IApplicationDbContext
{
    DbSet<Patient> Patients { get; set; }
    DbSet<PatientStatusHistory> PatientStatusHistory { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
}