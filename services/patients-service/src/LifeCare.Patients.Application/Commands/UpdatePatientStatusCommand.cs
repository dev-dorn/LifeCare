using LifeCare.Modules.Patients.Domain.Enums;
using LifeCare.Modules.Shared.Application.common;
using LifeCare.Patients.Application.Dtos;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Commands;

public record UpdatePatientStatusCommand(
    Guid Id,
    PatientStatus NewStatus,
    string? Notes,
    string ChangedBy,
    DateTime ChangedAt
) : IRequest<Result<PatientDto>>;