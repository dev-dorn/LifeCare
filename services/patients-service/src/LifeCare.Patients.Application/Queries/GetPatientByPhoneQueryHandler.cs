using LifeCare.Modules.Patients.Domain;
using LifeCare.Modules.Shared.Application.Interfaces.Repositories;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Queries;

public class GetPatientByPhoneQueryHandler : IRequestHandler<GetPatientByPhoneQuery, Patient?>
{
    private readonly IPatientRepository _patientRepository;

    public GetPatientByPhoneQueryHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Patient?> Handle(GetPatientByPhoneQuery request, CancellationToken cancellationToken)
    {
        return await _patientRepository.GetByPhoneNumberAsync(request.PhoneNumber);
    }
}