namespace LifeCare.Domain.Patients.ValuedObjects;

public class Address
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }
    public string Country { get; }

    public Address(string street, string city, string state, string zipCode, string country = "Kenya")
    {
        Street = street?.Trim();
        City = city?.Trim();
        State = state?.Trim();
        ZipCode = zipCode?.Trim();
        Country = country?.Trim();
            
    }

    public bool IsEmpty =>
        string.IsNullOrWhiteSpace(Street) &&
        string.IsNullOrWhiteSpace(City) &&
        string.IsNullOrWhiteSpace(State) &&
        string.IsNullOrWhiteSpace(ZipCode);
    
    public override string ToString() => 
        $"{Street}, {City}, {State}, {ZipCode}, {Country}";
}