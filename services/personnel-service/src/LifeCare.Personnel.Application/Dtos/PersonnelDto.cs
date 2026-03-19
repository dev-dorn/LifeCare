namespace LifeCare.Personnel.Application.Dtos;

public class PersonnelDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? LicenseNumber { get; set; } // Nullable for non-medical staff
    public Guid? DepartmentId { get; set; }
    public List<string> Privileges { get; set; } = new();
    public DateTime CreatedAt { get; set; }

    public static PersonnelDto FromPersonnel(Domain.Personnel personnel)
    {
        return new PersonnelDto
        {
            Id = personnel.Id,
            FirstName = personnel.FirstName,
            LastName = personnel.LastName,
            Email = personnel.Email,
            PhoneNumber = personnel.PhoneNumber,
            Role = personnel.Role.ToString(),
            Status = personnel.Status.ToString(),
            LicenseNumber = personnel.LicenseNumber,
            DepartmentId = personnel.DepartmentId,
            Privileges = personnel.Privileges.ToList(),
            CreatedAt = personnel.CreatedAt
        };
    }
}