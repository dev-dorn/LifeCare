using LifeCare.Modules.Patients.Domain;
using LifeCare.Modules.Shared.Application.Interfaces.Repositories;

namespace LifeCare.Modules.Patients.Infrastructure.Repositories;

public class InMemoryPatientRepository : IPatientRepository
{
    private readonly List<Patient> _patients = new();
    private readonly List<PatientStatusHistory> _patientStatusHistory = new();

    public Task<Patient?> GetByMrnAsync(string mrn)
    {
        return Task.FromResult(
            _patients.FirstOrDefault(p => p.MRN == mrn)
        );
    }

    public Task<Patient?> GetByNationalIdAsync(string nationalId)
    {
        return Task.FromResult(
            _patients.FirstOrDefault(p => p.NationalId == nationalId)
        );
    }

    public Task<bool> ExistsByNationalIdAsync(string nationalId)
    {
        return Task.FromResult(
            _patients.Any(p => p.NationalId == nationalId)
        );
    }

    public Task<List<Patient>> GetAllAsync()
    {
        return Task.FromResult(_patients.ToList());
    }


    public Task<Patient?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return Task.FromResult(
            _patients.FirstOrDefault(p => p.PhoneNumber == phoneNumber)
        );
    }

    public Task<Patient?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(
            _patients.FirstOrDefault(p => p.Id == id)
        );
    }


    public Task<int> GetNextMrnSequenceAsync()
    {
        var currentYear = DateTime.Now.Year;

        var sequences = _patients
            .Select(p => p.MRN)
            .Select(ParseMrn)
            .Where(x => x.Year == currentYear)
            .Select(x => x.Sequence);

        if (!sequences.Any())
            return Task.FromResult(1);

        return Task.FromResult(sequences.Max() + 1);
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

    public Task<List<Patient>> GetRecentPatientsAsync(int count)
    {
        var recentPatients = _patients
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToList();
        return Task.FromResult(recentPatients);
    }

    public Task<Patient?> GetByShifNumberAsync(string shifNumber)
    {
        return Task.FromResult(
            _patients.FirstOrDefault(p => p.ShifNumber == shifNumber));
    }


    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task<List<PatientStatusHistory>> GetStatusHistoryAsync(Guid patientId)
    {
        var history = _patientStatusHistory
            .Where(h => h.PatientId == patientId)
            .OrderByDescending(h => h.ChangedAt)
            .ToList();

        return Task.FromResult(history);
    }

    public Task<IReadOnlyList<Patient>> SearchPatientsAsync(string? name, string? city)
    {
        IEnumerable<Patient> query = _patients;

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p =>
                p.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                p.LastName.Contains(name, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(p =>
                p.SubCounty.Contains(city, StringComparison.OrdinalIgnoreCase));

        var result = query
            .OrderByDescending(p => p.CreatedAt)
            .ToList();

        return Task.FromResult<IReadOnlyList<Patient>>(result);
    }

    public Task AddStatusHistoryAsync(PatientStatusHistory history)
    {
        _patientStatusHistory.Add(history);
        return Task.CompletedTask;
    }


    // -----------------------
    // MRN PARSER (PRIVATE)
    // -----------------------
    private static (int Year, int Sequence) ParseMrn(string mrn)
    {
        // Expected: LC-2026-0001
        var parts = mrn.Split('-');

        if (parts.Length != 3)
            return (0, 0);

        int.TryParse(parts[1], out var year);
        int.TryParse(parts[2], out var sequence);

        return (year, sequence);
    }
}