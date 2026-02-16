namespace LifeCare.Domain.Patients
{
    public enum PatientStatus
    {
        Unknown = 0,
        AwaitingTriage = 1,
        InTriage = 2,
        InConsultation = 3,
        InLab = 4,
        AwaitingDischarge = 5,
        Discharged = 6,
        Inactive = 7
    }
}