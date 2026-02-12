using LifeCare.Application.Interfaces.Repositories;
using LifeCare.Domain.Patients;
using MediatR;

namespace LifeCare.Application.Patients.Queries;

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
