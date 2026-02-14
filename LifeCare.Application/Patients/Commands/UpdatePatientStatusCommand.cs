using LifeCare.Application.common;
using LifeCare.Application.Patients.Dtos;
using LifeCare.Domain.Patients;
using MediatR;

namespace LifeCare.Application.Patients.Commands;

public record UpdatePatientStatusCommand(
    Guid Id,
    PatientStatus NewStatus,
    string? Notes,
    string ChangedBy ,
    DateTime ChangedAt 
) : IRequest<Result<PatientDto>>;