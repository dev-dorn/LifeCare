using LifeCare.Modules.Patients.Domain;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Queries;

public record GetRecentPatientsQuery(int Count = 10) : IRequest<List<Patient>>;