using LifeCare.Modules.Patients.Domain.ValuedObjects;
using LifeCare.Modules.Shared.Domain.Common;

namespace LifeCare.Modules.Patients.Domain.Events;

public class PatientMovedToTriageEvent : DomainEvent
{
    public PatientMovedToTriageEvent(PatientId patientId)
    {
        PatientId = patientId;
    }

    public PatientId PatientId { get; }
}