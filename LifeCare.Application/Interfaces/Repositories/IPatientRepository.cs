using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LifeCare.Domain.Patients;

namespace LifeCare.Application.Interfaces.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient?> GetByIdAsync(Guid id);
        Task<Patient?> GetByMrnAsync(string mrn);
        Task<Patient?> GetByNationalIdAsync(string nationalId);
        Task<bool> ExistsByNationalIdAsync(string nationalId);
        Task<int> GetNextMrnSequenceAsync();
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<List<Patient>> GetAllAsync();
    }
}