using LifeCare.Domain.Patients;
using MediatR;

namespace LifeCare.Application.Patients.Queries;

public record GetPatientStatusHistoryQuery(Guid PatientId): IRequest<List<PatientStatusHistory>>;