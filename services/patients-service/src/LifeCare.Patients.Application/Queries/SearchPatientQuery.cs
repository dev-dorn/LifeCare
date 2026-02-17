using LifeCare.Modules.Patients.Domain;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Queries;

public record SearchPatientQuery(string? Name, string? City) : IRequest<IReadOnlyList<Patient>>;