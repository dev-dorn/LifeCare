namespace LifeCare.Modules.Patients.Domain.ValuedObjects;

public record PatientId(Guid Value)
{
    public static PatientId New()
    {
        return new PatientId(Guid.NewGuid());
    }

    public static PatientId From(Guid value)
    {
        return new PatientId(value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}