namespace Patients.Commands;
using MediaR;

public class RegisterPatientCommand : IRequest<Result<PatientDto>>
{
    public string NationalId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string PhoneNumber { get; set; }
    
    //Optional Fields
    public string Email { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    
    //Guardian information (if patient is minor)
    public GuardianDto Guardian { get; set; }
    //System information
    public string ReceptionistId { get; set; }
    
}
public class GuardianDto
{
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Relationship { get; set; }
  public string PhoneNumber { get; set; }
}
