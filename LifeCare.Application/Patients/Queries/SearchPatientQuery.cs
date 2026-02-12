using LifeCare.Domain.Patients;
using MediatR;

namespace LifeCare.Application.Patients.Queries;

public record SearchPatientQuery(string? Name, string? City) : IRequest<IReadOnlyList<Patient>>;