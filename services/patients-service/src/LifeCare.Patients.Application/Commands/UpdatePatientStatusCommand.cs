using LifeCare.Modules.Shared.Application.common;
using LifeCare.Modules.Patients.Application.Dtos;
using LifeCare.Modules.Patients.Domain;
using LifeCare.Modules.Patients.Domain.Enums;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Commands;

public record UpdatePatientStatusCommand(
    Guid Id,
    PatientStatus NewStatus,
    string? Notes,
    string ChangedBy ,
    DateTime ChangedAt 
) : IRequest<Result<PatientDto>>;