using Lifecare.Domain.Common;

namespace LifeCare.Domain.Patients.ValuedObjects;

public record NationalId
{
    public string Value { get; }
    public string CountryCode { get; }

    public NationalId(string value, string countryCode = "Kenya")
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("National ID cannot be empty");

        if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[A-Za-z0-9]+$"))
            throw new DomainException("National ID must be alphanumeric");

        Value = value.Trim();
        CountryCode = countryCode?.Trim().ToUpper() ?? "KENYA";
    }

    public bool Matches(string value, string countryCode) =>
        Value.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase) &&
        CountryCode.Equals(countryCode.Trim().ToUpper(), StringComparison.OrdinalIgnoreCase);

    public override string ToString() => $"{CountryCode}:{Value}";
}