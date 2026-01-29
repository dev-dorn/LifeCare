namespace LifeCare.Domain.Patients.ValuedObjects
{
    public record PatientId(Guid Value)
    {
        public static PatientId New() => new PatientId(Guid.NewGuid());
        public static PatientId From(Guid value) => new(value);
        public override string ToString() => Value.ToString();
    }
    
}