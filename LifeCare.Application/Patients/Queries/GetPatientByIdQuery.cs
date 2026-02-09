using LifeCare.Domain.Patients;
using MediatR;

namespace LifeCare.Application.Patients.Queries

{
    public record GetPatientByIdQuery(Guid Id) : IRequest<Patient?>;
}