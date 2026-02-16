using LifeCare.Modules.Shared.Application.Interfaces.Repositories;
using LifeCare.Modules.Patients.Domain;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Queries

{
    public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, Patient?>
    {
        private readonly IPatientRepository _patientRepository;

        public GetPatientByIdQueryHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Patient?> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            return await _patientRepository.GetByIdAsync(request.Id);
        }
    }
}