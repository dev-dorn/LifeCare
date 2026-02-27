namespace LifeCare.Modules.Patients.Domain.ValuedObjects;

public class Guardian
{
    public Guardian(string firstName, string lastName, string relationship, string phoneNumber)
    {
        FirstName = firstName?.Trim() ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName?.Trim() ?? throw new ArgumentNullException(nameof(lastName));
        Relationship = relationship?.Trim() ?? throw new ArgumentNullException(nameof(relationship));
        PhoneNumber = phoneNumber?.Trim() ?? throw new ArgumentNullException(nameof(phoneNumber));
    }

    public string FirstName { get; }
    public string LastName { get; }
    public string Relationship { get; }
    public string PhoneNumber { get; }

    public string FullName => $"{FirstName} {LastName}";
}