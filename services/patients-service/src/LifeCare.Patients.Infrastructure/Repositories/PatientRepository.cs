using LifeCare.Modules.Patients.Domain;
using LifeCare.Modules.Patients.Infrastructure.Persistence;
using LifeCare.Modules.Shared.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LifeCare.Modules.Patients.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly HospitalDbContext _context;

    public PatientRepository(HospitalDbContext context)
    {
        _context = context;
    }


    public async Task<Patient?> GetByMrnAsync(string mrn)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.MRN == mrn);
    }

    public async Task<Patient?> GetByNationalIdAsync(string nationalId)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.NationalId == nationalId);
    }

    public async Task<bool> ExistsByNationalIdAsync(string nationalId)
    {
        return await _context.Patients
            .AnyAsync(p => p.NationalId == nationalId);
    }

    public async Task<int> GetNextMrnSequenceAsync()
    {
        var currentYear = DateTime.Now.Year;

        // Get the highest sequence for current year
        var lastPatient = await _context.Patients
            .Where(p => p.MRN != null && p.MRN.StartsWith($"LC-{currentYear}-"))
            .OrderByDescending(p => p.MRN)
            .FirstOrDefaultAsync();

        if (lastPatient == null)
            return 1;

        // Extract sequence from MRN (LC-2024-0001 -> 1)
        var parts = lastPatient.MRN.Split('-');
        if (parts.Length == 3)
        {
            var sequencePart = parts[2];
            if (int.TryParse(sequencePart, out var lastSequence)) return lastSequence + 1;
        }

        // Fallback: count patients from current year
        var count = await _context.Patients
            .CountAsync(p => p.MRN != null && p.MRN.StartsWith($"LC-{currentYear}-"));

        return count + 1;
    }

    public async Task<List<PatientStatusHistory>> GetStatusHistoryAsync(Guid patientId)
    {
        return await _context.PatientStatusHistory
            .Where(h => h.PatientId == patientId)
            .OrderByDescending(h => h.ChangedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
    }

    public Task UpdateAsync(Patient patient)
    {
        _context.Patients.Update(patient);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Patient?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
    }


    public async Task<List<Patient>> GetAllAsync()
    {
        return await _context.Patients
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Patient?> GetByIdAsync(Guid id)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyList<Patient>> SearchPatientsAsync(string? name, string? city)
    {
        IQueryable<Patient> query = _context.Patients;

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p =>
                p.FirstName.Contains(name) ||
                p.LastName.Contains(name)
            );

        if (!string.IsNullOrWhiteSpace(city)) query = query.Where(p => p.SubCounty == city);
        return await query.ToListAsync();
    }

    public async Task AddStatusHistoryAsync(PatientStatusHistory history)
    {
        await _context.PatientStatusHistory.AddAsync(history);
    }


    public async Task<List<Patient>> GetRecentPatientsAsync(int count)
    {
        return await _context.Patients
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<Patient?> GetByShifNumberAsync(string shifNumber)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.ShifNumber == shifNumber);
    }
}