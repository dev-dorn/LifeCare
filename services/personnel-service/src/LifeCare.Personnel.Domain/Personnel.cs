using LifeCare.Personnel.Domain.Common;
using LifeCare.Personnel.Domain.Enum;
using LifeCare.Personnel.Domain.Events;

namespace LifeCare.Personnel.Domain;

public class Personnel : AggregateRoot
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public PersonnelRole Role { get; private set; } 
    public EmploymentStatus Status {get; private set;}
    public List<string> Privileges { get; private set; }
    public DateTime CreatedAt  { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Personnel()
    {
        Privileges = new List<string>();
    }

    public static Personnel Create(
        string fullName,
        string email,
        PersonnelRole role,
        List<string>? privileges,
        string createdBy)
    {
        ValidateInput(fullName, email);

        var personnel = new Personnel
        {
            Id = Guid.NewGuid(),
            FullName = fullName.Trim(),
            Email = email.Trim().ToLower(),
            Role = role,
            Status = EmploymentStatus.Active,
            Privileges = privileges ?? new List<string>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow

        };
        personnel.AddDomainEvent(new PersonnelRegisteredEvent(
            personnel.Id,
            personnel.FullName,
            personnel.Role
        ));
        return personnel;


    }

    public void UpdateInfo(string fullName, string email)
    {
        ValidateInput(fullName, email);
        
        FullName = fullName.Trim();
        Email = email.Trim();
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
    public void ReActivate()
    {
        Status = EmploymentStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateInput(string fullName, string email)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Full name is required");
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required");
        if (!IsValidEmail(email))
            throw new DomainException("Email is invalid");
        
        
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}