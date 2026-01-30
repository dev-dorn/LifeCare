namespace LifeCare.Domain.Patients
{
    public static class MRNHelper
    {
        public static string GenerateMRN(int sequenceNumber)
        {
            int year = DateTime.Now.Year;
            string sequence = sequenceNumber.ToString("D4");
            return $"LC-{year}-{sequence}";
        }
    }
}