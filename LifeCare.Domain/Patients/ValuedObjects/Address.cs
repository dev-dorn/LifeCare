namespace LifeCare.Domain.Patients
{
    public class Address
    {
        public string County { get; }
        public string SubCounty { get; }
        public string Country { get; }
        public string ZipCode { get; }
        
        public Address(string county, string subCounty, string zipCode, string country = "Kenya")
        {
            County =county?.Trim();
            SubCounty = subCounty?.Trim();
            ZipCode = zipCode?.Trim();
            Country = country?.Trim();
        }
        
        public bool IsEmpty => 
            string.IsNullOrWhiteSpace(County) &&
            string.IsNullOrWhiteSpace(SubCounty) &&
            string.IsNullOrWhiteSpace(Country) &&
            string.IsNullOrWhiteSpace(ZipCode);
            
        public override string ToString() => 
            $"{County}, {SubCounty}, {Country} {ZipCode}, {Country}";
    }
}