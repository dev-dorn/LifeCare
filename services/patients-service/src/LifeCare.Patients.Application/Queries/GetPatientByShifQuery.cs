using LifeCare.Modules.Patients.Domain;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Queries;

public record GetPatientByShifQuery(string ShifNumber) : IRequest<Patient?>;