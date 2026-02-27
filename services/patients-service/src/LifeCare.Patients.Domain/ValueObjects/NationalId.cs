using LifeCare.Modules.Shared.Domain.Common;

namespace LifeCare.Modules.Patients.Domain.ValuedObjects;

public class NationalId
{
    public NationalId(string value, string countryCode = "US")
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("National ID cannot be empty");

        Value = value.Trim();
        CountryCode = countryCode.ToUpper();
    }

    public string Value { get; }
    public string CountryCode { get; }

    public override string ToString()
    {
        return Value;
    }
}