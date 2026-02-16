using LifeCare.Domain.Patients;
using MediatR;

namespace LifeCare.Application.Patients.Queries;

public record GetPatientByShifQuery(string ShifNumber) : IRequest<Patient?>;