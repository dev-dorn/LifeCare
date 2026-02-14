using LifeCare.Application.common;
using LifeCare.Application.Interfaces.Repositories;
using LifeCare.Application.Patients.Dtos;
using MediatR;

namespace LifeCare.Application.Patients.Commands
{
    public class UpdatePatientCommandHandler(IPatientRepository patientRepository) 
        : IRequestHandler<UpdatePatientCommand, Result<PatientDto>>
    {
        private readonly IPatientRepository _patientRepository = patientRepository;

        public async Task<Result<PatientDto>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetByIdAsync(request.Id);
            if (patient == null)
                return Result<PatientDto>.Failure("Patient not found");

            patient.UpdateBasicInfo(
                request.NationalId,
                request.FirstName,
                request.LastName,
                request.Gender,
                request.DateOfBirth
            );

            patient.UpdateContactInfo(
                request.Email ?? string.Empty,
                request.County ?? string.Empty,
                request.SubCounty ?? string.Empty,
                request.Country ?? "Kenya",
                request.ZipCode ?? string.Empty
            );

            if (request.GuardianName is not null)
            {
                patient.UpdateGuardianInfo(
                    request.GuardianName,
                    request.GuardianRelationship,
                    request.GuardianPhone
                );
            }

            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.SaveChangesAsync(cancellationToken);

            return Result<PatientDto>.Success(PatientDto.FromPatient(patient));
        }
    }
}