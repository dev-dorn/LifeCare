using Lifecare.Domain.Common;
using LifeCare.Domain.Patients.ValuedObjects;

namespace Lifecare.Domain.Events;

public class PatientRegisteredEvent:DomainEvent
{
    public PatientId PatientId { get; }
    public string MRN { get; }
    public PatientRegisteredEvent (PatientId patientId, string mrn)
    {
        PatientId = patientId;
        MRN = mrn;
    }
    
}