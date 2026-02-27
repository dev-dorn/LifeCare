using LifeCare.Modules.Patients.Domain;
using LifeCare.Modules.Shared.Application.common;
using LifeCare.Modules.Shared.Application.Interfaces.Repositories;
using LifeCare.Patients.Application.Dtos;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Commands;

public class UpdatePatientStatusCommandHandler : IRequestHandler<UpdatePatientStatusCommand, Result<PatientDto>>
{
    private readonly IPatientRepository _patientRepository;

    public UpdatePatientStatusCommandHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Result<PatientDto>> Handle(UpdatePatientStatusCommand request,
        CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByIdAsync(request.Id);
        if (patient == null)
            return Result<PatientDto>.Failure("Patient not found");

        // Update patient status
        patient.UpdateStatus(request.NewStatus);

        // Create history record
        var history = PatientStatusHistory.Create(
            patient.Id,
            request.NewStatus,
            request.ChangedBy ?? "System",
            request.Notes
        );

        // Save both via repository
        await _patientRepository.UpdateAsync(patient);
        await _patientRepository.AddStatusHistoryAsync(history);
        await _patientRepository.SaveChangesAsync(cancellationToken);

        return Result<PatientDto>.Success(PatientDto.FromPatient(patient));
    }
}