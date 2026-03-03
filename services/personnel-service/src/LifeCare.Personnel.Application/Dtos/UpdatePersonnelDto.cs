using System.Reflection.Metadata;
using LifeCare.Personnel.Domain.Enums;

namespace LifeCare.Personnel.Application.Dtos;

public class UpdatePersonnelDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public PersonnelRole Role { get; set; }
    public string? PhoneNumber { get; set; }
}