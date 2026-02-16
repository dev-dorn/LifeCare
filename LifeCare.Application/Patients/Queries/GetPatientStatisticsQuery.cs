using LifeCare.Application.Patients.Dtos;
using MediatR;

namespace LifeCare.Application.Patients.Queries;

public class GetPatientStatisticsQuery : IRequest<PatientStatisticsDto>;
