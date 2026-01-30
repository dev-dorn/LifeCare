// LifeCare.Infrastructure/Services/MrnGenerator.cs

using LifeCare.Application.Interfaces;
using LifeCare.Application.Interfaces.Repositories;

namespace LifeCare.Infrastructure.Services
{
    public class MrnGenerator : IMrnGenerator
    {
        private readonly IPatientRepository _patientRepository;
        
        public MrnGenerator(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        
        public async Task<string> GenerateAsync()
        {
            int sequence = await _patientRepository.GetNextMrnSequenceAsync();
            return GenerateMRN(sequence);
        }
        
        private string GenerateMRN(int sequenceNumber)
        {
            int year = DateTime.Now.Year;
            string sequence = sequenceNumber.ToString("D4");
            return $"LC-{year}-{sequence}";
        }
    }
}