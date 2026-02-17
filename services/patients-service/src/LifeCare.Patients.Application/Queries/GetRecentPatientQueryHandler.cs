using LifeCare.Modules.Shared.Application.Interfaces.Repositories;
using LifeCare.Modules.Patients.Domain;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Queries;

public class GetRecentPatientsQueryHandler : IRequestHandler<GetRecentPatientsQuery, List<Patient>>
{
    private  readonly IPatientRepository _patientRepository;

    public GetRecentPatientsQueryHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<List<Patient>> Handle(GetRecentPatientsQuery request, CancellationToken cancellationToken)
    {
        return await _patientRepository.GetRecentPatientsAsync(request.Count);
    }
    
}
