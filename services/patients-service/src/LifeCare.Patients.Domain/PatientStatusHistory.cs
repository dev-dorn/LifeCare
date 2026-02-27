using LifeCare.Modules.Patients.Domain.Enums;

namespace LifeCare.Modules.Patients.Domain;

public class PatientStatusHistory
{
    private PatientStatusHistory()
    {
    }

    public Guid Id { get; private set; }
    public Guid PatientId { get; private set; }
    public PatientStatus Status { get; private set; }
    public DateTime ChangedAt { get; private set; }
    public string ChangedBy { get; private set; }
    public string? Notes { get; private set; }

    public static PatientStatusHistory Create(
        Guid patientId,
        PatientStatus status,
        string changedBy,
        string? notes = null
    )
    {
        return new PatientStatusHistory
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            Status = status,
            ChangedAt = DateTime.UtcNow,
            ChangedBy = changedBy,
            Notes = notes
        };
    }
}