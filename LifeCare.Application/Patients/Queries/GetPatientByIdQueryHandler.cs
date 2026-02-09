using LifeCare.Application.Interfaces.Repositories;
using LifeCare.Domain.Patients;
using MediatR;

namespace LifeCare.Application.Patients.Queries

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