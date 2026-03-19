using System.Net.Mail;
using LifeCare.Personnel.Domain.Common;
using LifeCare.Personnel.Domain.Enums;
using LifeCare.Personnel.Domain.Events;

namespace LifeCare.Personnel.Domain;

public class Personnel : AggregateRoot
{
    private Personnel()
    {
        Privileges = new List<string>();
    }

    public Guid Id { get; internal set; }
    public string FirstName { get; internal set; } = string.Empty;
    public string LastName { get; internal set; } = string.Empty;
    public string PhoneNumber { get; internal set; } = string.Empty;
    public string? LicenseNumber { get; internal set; }
    public Guid? DepartmentId { get; internal set; }
    
    public string Email { get; internal set; } = string.Empty;
    public PersonnelRole Role { get; internal set; }
    public EmploymentStatus Status { get; internal set; }
    public List<string> Privileges { get; internal set; }
    public DateTime CreatedAt { get; internal set; }
    public DateTime UpdatedAt { get; internal set; }

    public static Personnel Create(
        string firstName,
        string lastName,
        string email,
        PersonnelRole role,
        List<string>? privileges,
        string createdBy)


    {
        ValidateInput(firstName, lastName, email);
        var personnel = new Personnel
        {
            Id = Guid.NewGuid(),
            FirstName =firstName.Trim(),
            LastName = lastName.Trim(),
            Email = email.Trim().ToLower(),
            Role = role,
            Status = EmploymentStatus.Active,
            Privileges = privileges ?? new List<string>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        personnel.AddDomainEvent(new PersonnelRegisteredEvent(
            personnel.Id,
            personnel.FirstName,
            personnel.LastName,
            personnel.Role));
        return personnel;
    }

    public void UpdateInfo(
        string firstName,
        string lastName,
        string email,
        string? phoneNumber,
        string? licenseNumber,
        Guid? departmentId)
    {
        ValidateInput(firstName,lastName, email);
        FirstName=firstName.Trim();
        LastName=lastName.Trim();
        PhoneNumber = phoneNumber;
        Email = email.Trim().ToLower();
        UpdatedAt = DateTime.UtcNow;
        LicenseNumber = licenseNumber;
        DepartmentId = departmentId;
        UpdatedAt = DateTime.UtcNow;

    }

    public void ChangeRole(PersonnelRole newRole, List<string>? newPrivileges)
    {
        Role = newRole;
        Privileges = newPrivileges ?? new List<string>();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = EmploymentStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reactivate()
    {
        Status = EmploymentStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateInput(string firstName, string email, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("Full name is required");
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("last name is required");
        if(string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required");
        if (!IsValidEmail(email))
            throw new DomainException("Invalid email format");
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    // In Domain/Personnel.cs
  
}