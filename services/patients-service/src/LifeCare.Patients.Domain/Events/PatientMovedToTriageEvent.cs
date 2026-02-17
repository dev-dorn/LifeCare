using LifeCare.Modules.Shared.Domain.Common;
using LifeCare.Modules.Patients.Domain.ValuedObjects;

namespace LifeCare.Modules.Patients.Domain.Events
{
    public class PatientMovedToTriageEvent : DomainEvent
    {
        public PatientId PatientId { get; }
        
        public PatientMovedToTriageEvent(PatientId patientId)
        {
            PatientId = patientId;
        }
    }
}