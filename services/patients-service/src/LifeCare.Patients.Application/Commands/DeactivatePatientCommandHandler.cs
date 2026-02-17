// LifeCare.Application/Patients/Commands/DeactivatePatientCommandHandler.cs
using LifeCare.Modules.Shared.Application.common;
using LifeCare.Modules.Shared.Application.Interfaces.Repositories;
using LifeCare.Modules.Patients.Application.Dtos;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Commands;

public class DeactivatePatientCommandHandler : IRequestHandler<DeactivatePatientCommand, Result<PatientDto>>
{
    private readonly IPatientRepository _patientRepository;

    public DeactivatePatientCommandHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Result<PatientDto>> Handle(DeactivatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByIdAsync(request.Id);
        if (patient == null)
            return Result<PatientDto>.Failure("Patient not found");

        patient.Deactivate();
        await _patientRepository.UpdateAsync(patient);
        await _patientRepository.SaveChangesAsync(cancellationToken);

        return Result<PatientDto>.Success(PatientDto.FromPatient(patient));
    }
}