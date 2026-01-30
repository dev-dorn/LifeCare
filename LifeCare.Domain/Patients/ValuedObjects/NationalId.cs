using LifeCare.Domain.Common;

namespace LifeCare.Domain.Patients
{
    public class NationalId
    {
        public string Value { get; }
        public string CountryCode { get; }
        
        public NationalId(string value, string countryCode = "US")
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("National ID cannot be empty");
                
            Value = value.Trim();
            CountryCode = countryCode.ToUpper();
        }
        
        public override string ToString() => Value;
    }
}