using LifeCare.Application.Interfaces;
using LifeCare.Domain.Patients;

public class InMemoryPatientRepository : IPatientRepository
{
    private readonly List<Patient> _patients = new();

    public Task<Patient?> GetByMrnAsync(string mrn)
    {
        var patient = _patients.FirstOrDefault(p => p.MRN.Value == mrn);
        return Task.FromResult(patient);
    }

    // Required by interface
    public Task<Patient?> GetByNationalIdAsync(string nationalId)
    {
        var patient = _patients.FirstOrDefault(p => p.NationalId.Value== nationalId);
        return Task.FromResult(patient);
    }

    // Overload with countryCode (optional convenience)
    public Task<Patient?> GetByNationalIdAsync(string nationalId, string countryCode)
    {
        var patient = _patients.FirstOrDefault(p =>
            p.NationalId.Value == nationalId &&
            p.NationalId.CountryCode == countryCode);
        return Task.FromResult(patient);
    }

    public Task<bool> ExistsByNationalIdAsync(string nationalId)
    {
        var exists = _patients.Any(p => p.NationalId.Value == nationalId);
        return Task.FromResult(exists);
    }

    public Task<List<Patient>> GetAllAsync()
    {
        return Task.FromResult(_patients.ToList());
    }

    public Task<Patient?> GetByIdAsync(Guid id)
    {
        var patient = _patients.FirstOrDefault(p => p.Id.Value == id);
        return Task.FromResult(patient);
    }

    public Task<int> GetNextMrnSequenceAsync()
    {
        var currentYear = DateTime.Now.Year;
        var patientsThisYear = _patients.Where(p => p.MRN.IsForYear(currentYear)).ToList();

        if (!patientsThisYear.Any())
            return Task.FromResult(1);

        var lastSequence = patientsThisYear
            .Select(p => p.MRN.TryGetSequence())
            .Where(seq => seq.HasValue)
            .Max() ?? 0;

        return Task.FromResult(lastSequence + 1);
    }

    public Task AddAsync(Patient patient)
    {
        _patients.Add(patient);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Patient patient)
    {
        var existing = _patients.FirstOrDefault(p => p.Id == patient.Id);
        if (existing != null)
        {
            _patients.Remove(existing);
            _patients.Add(patient);
        }
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        // No-op for in-memory
        return Task.CompletedTask;
    }
}
