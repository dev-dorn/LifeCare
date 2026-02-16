using LifeCare.Modules.Patients.Domain;
using MediatR;

namespace LifeCare.Modules.Patients.Application.Queries;

public record GetPatientByPhoneQuery(string PhoneNumber) : IRequest<Patient?>;