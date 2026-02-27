// LifeCare.Infrastructure/Services/MrnGenerator.cs


using LifeCare.Modules.Shared.Application.Interfaces.Repositories;

namespace LifeCare.Modules.Shared.Infrastructure;

public class MrnGenerator : IMrnGenerator
{
    private readonly IPatientRepository _patientRepository;

    public MrnGenerator(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<string> GenerateAsync()
    {
        var sequence = await _patientRepository.GetNextMrnSequenceAsync();
        return GenerateMRN(sequence);
    }

    private string GenerateMRN(int sequenceNumber)
    {
        var year = DateTime.Now.Year;
        var sequence = sequenceNumber.ToString("D4");
        return $"LC-{year}-{sequence}";
    }
}