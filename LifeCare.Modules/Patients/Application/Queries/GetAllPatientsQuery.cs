using LifeCare.Modules.Shared.Application.Interfaces.Repositories;
using LifeCare.Modules.Patients.Application.Dtos;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Queries;

public class GetAllPatientsQuery : IRequest<List<PatientDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, List<PatientDto>>
{
    private readonly IPatientRepository _patientRepository;

    public GetAllPatientsQueryHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<List<PatientDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
        var patients = await _patientRepository.GetAllAsync();

        return patients
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(PatientDto.FromPatient)
            .ToList();
    }
}