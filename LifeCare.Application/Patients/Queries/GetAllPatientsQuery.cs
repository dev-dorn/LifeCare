using LifeCare.Application.Interfaces.Repositories;
using LifeCare.Application.Patients.Dtos;
using MediatR;

namespace LifeCare.Application.Patients.Queries;

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