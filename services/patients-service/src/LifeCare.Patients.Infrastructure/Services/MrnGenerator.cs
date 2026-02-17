// LifeCare.Infrastructure/Services/MrnGenerator.cs


using LifeCare.Modules.Shared.Application.Interfaces.Repositories;

namespace LifeCare.Modules.Shared.Infrastructure
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