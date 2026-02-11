using LifeCare.Application.common;
using LifeCare.Application.Interfaces.Repositories;
using LifeCare.Application.Patients.Dtos;
using MediatR;


namespace LifeCare.Application.Patients.Commands
{
    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Result<PatientDto>>
    {
        private readonly IPatientRepository _patientRepository;

        public UpdatePatientCommandHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Result<PatientDto>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetByIdAsync(request.Id);
            if (patient == null)
                return Result<PatientDto>.Failure("Patient not found");

            // Use your domain update methods
            patient.UpdateBasicInfo(
                request.NationalId,
                request.FirstName,
                request.LastName,
                request.Gender,
                request.DateOfBirth
            );

            patient.UpdateContactInfo(
                request.Email,
                request.Street,
                request.City,
                request.State,
                request.ZipCode
            );
            

            if (request.GuardianName != null)
            {
                patient.UpdateGuardianInfo(
                    request.GuardianName,
                    request.GuardianRelationship,
                    request.GuardianPhone
                );
            }

            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.SaveChangesAsync(cancellationToken);

            // Convert to DTO using your existing PatientDto mapping
            return Result<PatientDto>.Success(PatientDto.FromPatient(patient));
        }
    }
}