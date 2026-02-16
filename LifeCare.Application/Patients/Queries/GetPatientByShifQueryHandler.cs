using LifeCare.Application.Interfaces.Repositories;
using LifeCare.Domain.Patients;
using MediatR;

namespace LifeCare.Application.Patients.Queries;

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