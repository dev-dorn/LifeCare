using LifeCare.Personnel.Application.Dtos;
using MediatR;

namespace LifeCare.Personnel.Application.Queries;

public record GetPersonnelByIdQuery(Guid Id) : IRequest<PersonnelDto>;