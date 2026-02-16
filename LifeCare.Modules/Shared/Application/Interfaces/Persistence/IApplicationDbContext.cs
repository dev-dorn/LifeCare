
using LifeCare.Modules.Patients.Domain;
using Microsoft.EntityFrameworkCore;

namespace LifeCare.Modules.Shared.Application.Interfaces.Persistence;

public interface IApplicationDbContext
{
    DbSet<Patient> Patients { get; set; }
    DbSet<PatientStatusHistory> PatientStatusHistory { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
}