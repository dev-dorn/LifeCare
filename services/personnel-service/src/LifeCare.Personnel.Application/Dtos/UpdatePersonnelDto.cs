using System.Reflection.Metadata;
using LifeCare.Personnel.Domain.Enums;

namespace LifeCare.Personnel.Application.Dtos;

public class UpdatePersonnelDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? LicenseNumber { get; set; }
    public Guid? DepartmentId { get; set; }
    public PersonnelRole Role { get; set; }
    public List<string?> Privileges { get; set; }
    public string? PhoneNumber { get; set; }
}