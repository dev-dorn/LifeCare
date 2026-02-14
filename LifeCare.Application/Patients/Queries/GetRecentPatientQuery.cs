using LifeCare.Domain.Patients;
using MediatR;

namespace LifeCare.Application.Patients.Queries;

public record GetRecentPatientsQuery(int Count = 10) : IRequest<List<Patient>>;
