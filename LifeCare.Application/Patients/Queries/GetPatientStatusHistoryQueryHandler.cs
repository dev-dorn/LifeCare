// LifeCare.Application/Patients/Queries/GetPatientStatusHistoryQueryHandler.cs
using LifeCare.Application.Interfaces.Repositories;
using LifeCare.Domain.Patients;
using MediatR;

namespace LifeCare.Application.Patients.Queries;

public class GetPatientStatusHistoryQueryHandler : IRequestHandler<GetPatientStatusHistoryQuery, List<PatientStatusHistory>>
{
    private readonly IPatientRepository _patientRepository;

    public GetPatientStatusHistoryQueryHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<List<PatientStatusHistory>> Handle(GetPatientStatusHistoryQuery request, CancellationToken cancellationToken)
    {
        return await _patientRepository.GetStatusHistoryAsync(request.PatientId);
    }
}