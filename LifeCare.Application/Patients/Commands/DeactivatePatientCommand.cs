using LifeCare.Application.common;
using LifeCare.Application.Patients.Dtos;
using MediatR;

namespace LifeCare.Application.Patients.Commands;

public record DeactivatePatientCommand(Guid Id) : IRequest<Result<PatientDto>>;
