using System.ComponentModel.DataAnnotations;
using LifeCare.Personnel.Domain.Enums;

namespace LifeCare.Personnel.API.Request;

public class RegisterPersonnelRequest
{
    [Required] public string FullName { get; set; } = string.Empty;
    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public required PersonnelRole Role { get; set; }
     public List<string>? Privileges { get; set; }
}