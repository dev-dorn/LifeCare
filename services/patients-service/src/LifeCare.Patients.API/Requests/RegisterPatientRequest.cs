using System.ComponentModel.DataAnnotations;

namespace LifeCare.API.Controllers.Requests;

public class RegisterPatientRequest
{
    [Required] public required string ShifNumber { get; set; }

    public string? NationalId { get; set; }

    [Required] public required string FirstName { get; set; }

    [Required] public required string LastName { get; set; }

    [Required] public required DateTime DateOfBirth { get; set; }

    [Required] public required string Gender { get; set; }

    [Required] public required string PhoneNumber { get; set; }

    // Optional fields → nullable
    public string? Email { get; set; }
    public string? County { get; set; }
    public string? SubCounty { get; set; }
    public string? Country { get; set; }
    public string? ZipCode { get; set; }

    // Guardian is optional
    public GuardianRequest? Guardian { get; set; }
}