using System.Text.RegularExpressions;
using Lifecare.Domain.Common;

public sealed class MedicalRecordNumber
{
    public string Value { get; }
    public DateTime CreatedDate { get; }

    private MedicalRecordNumber(string value, DateTime createdDate)
    {
        if (!Regex.IsMatch(value, @"^LC-\d{4}-\d{4}$"))
            throw new DomainException("Invalid MRN format");

        Value = value;
        CreatedDate = createdDate;
    }

    public static MedicalRecordNumber Generate(int sequenceNumber)
    {
        var year = DateTime.UtcNow.Year;
        var sequence = sequenceNumber.ToString("D4");
        var mrn = $"LC-{year}-{sequence}";

        return new MedicalRecordNumber(mrn, DateTime.UtcNow);
    }

    public int? TryGetSequence()
    {
        var parts = Value.Split('-');
        return parts.Length == 3 && int.TryParse(parts[2], out var seq)
            ? seq
            : null;
    }

    public bool IsForYear(int year) => Value.Contains($"-{year}-");

    public override string ToString() => Value;

    public override bool Equals(object? obj)
        => obj is MedicalRecordNumber other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
}