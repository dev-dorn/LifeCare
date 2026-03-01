using LifeCare.Personnel.Application.Common;
using LifeCare.Personnel.Application.Dtos;
using LifeCare.Personnel.Domain.Enums;
using MediatR;

namespace LifeCare.Personnel.Application.Commands;

public class RegisterPersonnelCommand : IRequest<Result<PersonnelDto>>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public PersonnelRole Role { get; set; }
    public List<string>? Privileges { get; set; }
    public string Createdby { get; set; } = "System";
}