using System;

namespace LifeCare.Domain.Patients.ValuedObjects
{
    public class MedicalRecordNumber
    {
        // The MRN string, e.g., "LC-2026-0001"
        public string Value { get; }
        
        // When this MRN was created
        public DateTime CreatedDate { get; }

        private MedicalRecordNumber(string value, DateTime createdDate)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            CreatedDate = createdDate;
        }

        /// <summary>
        /// Generate a new MRN with a sequential number
        /// </summary>
        public static MedicalRecordNumber Generate(int sequenceNumber)
        {
            var prefix = "LC"; // Your hospital/company prefix
            var year = DateTime.Now.Year;
            var sequence = sequenceNumber.ToString("D4"); // Pads 1 -> "0001"
            var mrn = $"{prefix}-{year}-{sequence}";

            return new MedicalRecordNumber(mrn, DateTime.UtcNow);
        }

        /// <summary>
        /// Check if this MRN belongs to a specific year
        /// </summary>
        public bool IsForYear(int year)
        {
            // MRN format: "LC-2026-0001"
            var parts = Value.Split('-');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int mrnYear))
            {
                return mrnYear == year;
            }
            return false;
        }

        /// <summary>
        /// Try to get the numeric sequence part of the MRN
        /// </summary>
        public int? TryGetSequence()
        {
            // MRN format: "LC-2026-0001"
            var parts = Value.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int seq))
            {
                return seq;
            }
            return null;
        }

        public override string ToString() => Value;

        // Optional: equality based on MRN value
        public override bool Equals(object? obj)
        {
            return obj is MedicalRecordNumber other && other.Value == Value;
        }

        public override int GetHashCode() => Value.GetHashCode();
    }
}
