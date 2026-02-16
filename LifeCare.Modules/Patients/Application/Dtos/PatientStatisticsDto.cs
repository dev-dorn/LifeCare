namespace LifeCare.Modules.Patients.Application.Dtos;

public class PatientStatisticsDto
{
    public int TotalPatients { get; set; }
    public int NewToday { get; set; }
    public int NewThisWeek { get; set; }
    public int NewThisMonth { get; set; }
    public Dictionary<string, int> ByStatus { get; set; } = new();
    public int ActivePatients { get; set; }
    public int InactivePatients { get; set; }
    public int WithGuardians { get; set; }
}