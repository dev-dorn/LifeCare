using Lifecare.Domain.Common;
using LifeCare.Domain.Patients.ValuedObjects;

namespace LifeCare.Domain.Patients.Enums;

public class PatientMovedToTriageEvent : DomainEvent
{
    public PatientId PatientId { get; }

    public PatientMovedToTriageEvent(PatientId patientId)
    {
        PatientId = patientId;
        
    }
}