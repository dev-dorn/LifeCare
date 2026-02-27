namespace LifeCare.API.Controllers.Requests;

public class GuardianRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Relationship { get; set; }
    public required string PhoneNumber { get; set; }
}