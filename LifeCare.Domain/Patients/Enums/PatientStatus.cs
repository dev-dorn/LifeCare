namespace LifeCare.Domain.Patients
{
    public enum PatientStatus
    {
        AwaitingTriage = 1,
        InTriage = 2,
        InConsultation = 3,
        InLab = 4,
        AwaitingDischarge = 5,
        Discharged = 6
    }
}