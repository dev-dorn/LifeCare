using LifeCare.Application.Interfaces.Repositories;
using LifeCare.Domain.Patients;
using MediatR;

namespace LifeCare.Application.Patients.Queries;

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
