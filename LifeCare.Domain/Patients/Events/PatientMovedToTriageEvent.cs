using LifeCare.Domain.Common;
using LifeCare.Domain.Patients;
using LifeCare.Domain.Patients.ValuedObjects;

namespace LifeCare.Domain.Events
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