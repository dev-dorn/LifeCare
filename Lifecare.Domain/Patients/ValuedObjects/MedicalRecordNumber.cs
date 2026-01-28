using System.Runtime.InteropServices.JavaScript;

namespace LifeCare.Domain.Patients.ValuedObjects;

public class MedicalRecordNumber
{
    public string Value { get; }
    public DateTime CreatedDate { get; }

    private MedicalRecordNumber(string value, DateTime createdDate)
    {
        Value = value;
        CreatedDate = createdDate;
    }

    public static MedicalRecordNumber Generate(int sequenceNumber)
    {
        var prefix = "LC";
        var year = DateTime.Now.Year;
        var sequence = sequenceNumber.ToString(("D4"));
        var mrn = $"{prefix}-{year}-{sequence}";

        return new MedicalRecordNumber(mrn, DateTime.UtcNow);
        
    }

    public override string ToString() => Value;
}

