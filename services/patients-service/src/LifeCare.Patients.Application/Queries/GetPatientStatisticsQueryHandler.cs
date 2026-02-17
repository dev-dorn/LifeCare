using System.Reflection.Metadata;
using LifeCare.Modules.Shared.Application.Interfaces.Repositories;
using LifeCare.Modules.Patients.Application.Dtos;
using LifeCare.Modules.Patients.Domain;
using LifeCare.Modules.Patients.Domain.Enums;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Queries;

public class GetPatientStatisticsQueryHandler : IRequestHandler<GetPatientStatisticsQuery, PatientStatisticsDto>

{
    private readonly IPatientRepository _patientRepository;
    
    public GetPatientStatisticsQueryHandler(IPatientRepository patientRepository){
        _patientRepository = patientRepository;
        
    }
    public async Task<PatientStatisticsDto> Handle(GetPatientStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var allPatients = await _patientRepository.GetAllAsync();
        var today = DateTime.Today;
        var weekAgo = today.AddDays(-7);
        var monthAgo = today.AddMonths(-1);

        var stats = new PatientStatisticsDto
        {
            TotalPatients = allPatients.Count,
            NewToday = allPatients.Count(p => p.CreatedAt.Date == today),
            NewThisWeek = allPatients.Count(p => p.CreatedAt.Date == weekAgo),
            NewThisMonth = allPatients.Count(p => p.CreatedAt.Date == monthAgo),
            ActivePatients = allPatients.Count(p => p.Status != PatientStatus.Inactive),
            InactivePatients = allPatients.Count(p => p.Status == PatientStatus.Inactive),
            WithGuardians = allPatients.Count(p => !string.IsNullOrWhiteSpace(p.GuardianName)),
            ByStatus = allPatients
                .GroupBy(p => p.Status?.ToString() ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Count())
        };
        return stats;

    }
}