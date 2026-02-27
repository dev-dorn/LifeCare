using LifeCare.Modules.Shared.Application.common;
using LifeCare.Patients.Application.Dtos;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Commands;

public record DeactivatePatientCommand(Guid Id) : IRequest<Result<PatientDto>>;