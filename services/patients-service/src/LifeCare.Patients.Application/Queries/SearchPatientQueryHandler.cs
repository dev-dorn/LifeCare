using LifeCare.Modules.Patients.Domain;
using LifeCare.Modules.Shared.Application.Interfaces.Repositories;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Queries;

public class SearchPatientQueryHandler
    : IRequestHandler<SearchPatientQuery, IReadOnlyList<Patient>>
{
    private readonly IPatientRepository _patientRepository;

    public SearchPatientQueryHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<IReadOnlyList<Patient>> Handle(
        SearchPatientQuery request,
        CancellationToken cancellationToken)
    {
        return await _patientRepository.SearchPatientsAsync(
            request.Name,
            request.City);
    }
}