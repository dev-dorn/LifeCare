using LifeCare.Modules.Shared.Domain.Common;

namespace LifeCare.Modules.Patients.Domain.Events;

public class PatientRegisteredEvent : DomainEvent
{
    public PatientRegisteredEvent(Guid patientId, string mrn)
    {
        PatientId = patientId;
        MRN = mrn;
    }

    public Guid PatientId { get; }
    public string MRN { get; }
}