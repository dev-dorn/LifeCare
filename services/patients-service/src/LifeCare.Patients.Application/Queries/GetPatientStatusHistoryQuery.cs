using LifeCare.Modules.Patients.Domain;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Queries;

public record GetPatientStatusHistoryQuery(Guid PatientId): IRequest<List<PatientStatusHistory>>;