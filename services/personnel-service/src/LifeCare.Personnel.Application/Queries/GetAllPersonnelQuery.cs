using LifeCare.Personnel.Application.Dtos;
using LifeCare.Personnel.Domain.Enums;
using MediatR;

namespace LifeCare.Personnel.Application.Queries;

public class GetAllPersonnelQuery : IRequest<List<PersonnelDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortDirection { get; set; } = "desc";    
    public PersonnelRole? RoleFilter { get; set; }
}
