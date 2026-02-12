
using LifeCare.Application.Interfaces.Repositories;
using LifeCare.Domain.Patients;
using MediatR;

namespace LifeCare.Application.Patients.Queries;

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
