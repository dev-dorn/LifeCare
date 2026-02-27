namespace LifeCare.Modules.Patients.Domain;

public static class MRNHelper
{
    public static string GenerateMRN(int sequenceNumber)
    {
        var year = DateTime.Now.Year;
        var sequence = sequenceNumber.ToString("D4");
        return $"LC-{year}-{sequence}";
    }
}