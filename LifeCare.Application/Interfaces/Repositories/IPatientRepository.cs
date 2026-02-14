
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
        Task<Patient?> GetByPhoneNumberAsync(string phoneNumber);
        Task<IReadOnlyList<Patient>> SearchPatientsAsync(string? name, string? city);
        Task AddStatusHistoryAsync(PatientStatusHistory history);
        Task<List<Patient>> GetRecentPatientsAsync(int count);
    }
}