using LifeCare.Personnel.Application.Dtos;
using MediatR;

namespace LifeCare.Personnel.Application.Commands;

public record UpdatePersonnelCommand : IRequest<bool>
{
    public Guid Id { get; init; }
    public UpdatePersonnelDto Personnel { get; init; } = null!;

    public UpdatePersonnelCommand(Guid id, UpdatePersonnelDto personnel)
    {
        Id = id;
        Personnel = personnel;
    }
}