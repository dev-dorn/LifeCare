namespace LifeCare.Domain.Patients.ValuedObjects;

public class NationalId
{
    public string Value { get; }
    public string CountryCode { get; }

    public NationalId(string value, string countryCode = "Kenya")
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("National ID cannot be empty");
        Value = value.Trim();
        CountryCode = countryCode.ToUpper();
        
    }
    public override string ToString() => Value;
}