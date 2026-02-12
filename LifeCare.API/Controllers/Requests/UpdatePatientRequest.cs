namespace LifeCare.API.Controllers.Requests;
 
public class UpdatePatientRequest
{
    public string NationalId { get; set; }  
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string County { get; set; }
    public string SubCounty { get; set; }
    
    public string Country { get; set; }
    public string ZipCode { get; set; }
    public string? GuardianName { get; set; }
    public string? GuardianRelationship { get; set; }
    public string? GuardianPhone { get; set; }
}