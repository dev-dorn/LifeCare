namespace LifeCare.Domain.Patients.ValuedObjects;

public class Guardian
{
    public string FirstName { get; set; }
    public string LastName { get; }
    public string Relationship { get; }
    public string PhoneNumber { get; }

    public Guardian(string firstName, string lastName, string relationship, string phoneNumber)
    {
        FirstName = firstName?.Trim() ?? throw new ArgumentException(nameof(firstName));
        LastName = lastName?.Trim() ?? throw new ArgumentException(nameof(lastName));
        Relationship = relationship?.Trim() ?? throw new ArgumentException(nameof(relationship));
        PhoneNumber = phoneNumber?.Trim() ?? throw new ArgumentException(nameof(phoneNumber));
        
    }

    public string FullName => $"{FirstName}{LastName}";
}