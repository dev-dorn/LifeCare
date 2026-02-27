using LifeCare.Modules.Patients.Domain;
using LifeCare.Modules.Shared.Application.Interfaces.Repositories;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Queries;

public class GetPatientByShifQueryHandler : IRequestHandler<GetPatientByShifQuery, Patient?>

{
    private readonly IPatientRepository _patientRepository;

    public GetPatientByShifQueryHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Patient?> Handle(GetPatientByShifQuery request, CancellationToken cancellationToken)
    {
        return await _patientRepository.GetByShifNumberAsync(request.ShifNumber);
    }
}