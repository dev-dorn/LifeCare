

using LifeCare.Modules.Patients.Application.Dtos;
using LifeCare.Modules.Shared.Application.common;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Commands;

public record DeactivatePatientCommand(Guid Id) : IRequest<Result<PatientDto>>;
