
using LifeCare.Modules.Shared.Application.Interfaces;
using LifeCare.Modules.Shared.Application.Interfaces.Repositories;
using MediatR;
using LifeCare.Modules.Patients.Application.Dtos;

namespace LifeCare.Modules.Patients.Application.Queries
{
    public class GetPatientByMrnQuery : IRequest<PatientDto>
    {
        public required string MRN { get; set; }
    }
    
    public class GetPatientByMrnQueryHandler : IRequestHandler<GetPatientByMrnQuery, PatientDto>
    {
        private readonly IPatientRepository _patientRepository;
        
        public GetPatientByMrnQueryHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        
        public async Task<PatientDto> Handle(GetPatientByMrnQuery request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetByMrnAsync(request.MRN);
            
            if (patient is null)
                return null;
                
            return PatientDto.FromPatient(patient);
        }
    }
}

