using LifeCare.Domain.Patients;
using MediatR;

namespace LifeCare.Application.Patients.Queries;

public record GetPatientByPhoneQuery(string PhoneNumber) : IRequest<Patient?>;
