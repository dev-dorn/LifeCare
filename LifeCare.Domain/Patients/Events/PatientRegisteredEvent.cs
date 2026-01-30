using LifeCare.Domain.Common;

namespace LifeCare.Domain.Patients.Events
{
    public class PatientRegisteredEvent : DomainEvent
    {
        public Guid PatientId { get; }
        public string MRN { get; }
        
        public PatientRegisteredEvent(Guid patientId, string mrn)
        {
            PatientId = patientId;
            MRN = mrn;
        }
    }
}