namespace LifeCare.Modules.Patients.Domain.ValuedObjects
{
    public class Address(string county, string subCounty, string zipCode, string country = "Kenya")
    {
        public string County { get; } = county.Trim();
        public string SubCounty { get; } = subCounty.Trim();
        public string ZipCode { get; } = zipCode.Trim();
        public string Country { get; } = country.Trim();

        public bool IsEmpty =>
            string.IsNullOrWhiteSpace(County) &&
            string.IsNullOrWhiteSpace(SubCounty) &&
            string.IsNullOrWhiteSpace(Country) &&
            string.IsNullOrWhiteSpace(ZipCode);

        public override string ToString() =>
            $"{County}, {SubCounty}, {Country} {ZipCode}";
    }
}