using LifeCare.Modules.Patients.Application.Dtos;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Queries;

public class GetPatientStatisticsQuery : IRequest<PatientStatisticsDto>;