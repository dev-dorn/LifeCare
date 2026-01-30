using Microsoft.EntityFrameworkCore;
using LifeCare.Domain.Patients;
using LifeCare.Application.Interfaces.Repositories;
using LifeCare.Infrastructure.Persistence;

namespace LifeCare.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HospitalDbContext _context;
        
        public PatientRepository(HospitalDbContext context)
        {
            _context = context;
        }
        
        public async Task<Patient> GetByIdAsync(Guid id)
        {
            return await _context.Patients.FindAsync(id);
        }
        
        public async Task<Patient> GetByMrnAsync(string mrn)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.MRN == mrn);
        }
        
        public async Task<Patient> GetByNationalIdAsync(string nationalId)
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
            int currentYear = DateTime.Now.Year;
            
            // Get the highest sequence for current year
            var lastPatient = await _context.Patients
                .Where(p => p.MRN != null && p.MRN.StartsWith($"LC-{currentYear}-"))
                .OrderByDescending(p => p.MRN)
                .FirstOrDefaultAsync();
                
            if (lastPatient == null)
                return 1;
                
            // Extract sequence from MRN (LC-2024-0001 -> 1)
            string[] parts = lastPatient.MRN.Split('-');
            if (parts.Length == 3)
            {
                string sequencePart = parts[2];
                if (int.TryParse(sequencePart, out int lastSequence))
                {
                    return lastSequence + 1;
                }
            }
            
            // Fallback: count patients from current year
            int count = await _context.Patients
                .CountAsync(p => p.MRN != null && p.MRN.StartsWith($"LC-{currentYear}-"));
                
            return count + 1;
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
        
        public async Task<List<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}