// LifeCare.Application/Patients/Dtos/PatientDto.cs

using LifeCare.Domain.Patients;

namespace LifeCare.Application.Patients.Dtos
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        public required string Mrn { get; set; }
        public required string ShifNumber { get; set; }
        public string? NationalId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public required string Gender { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public string? County { get; set; }
        public string? SubCounty { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public required string Status { get; set; }
        public bool RequiresGuardian { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianRelationship { get; set; }
        public string? GuardianPhone { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string CreatedBy { get; set; }
        
        public static PatientDto FromPatient(Patient patient)
        {
            if (patient == null) return null;
            
            return new PatientDto
            {
                Id = patient.Id,
                Mrn = patient.MRN,
                ShifNumber = patient.ShifNumber,
                NationalId = patient.NationalId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Age = patient.Age,
                Gender = patient.Gender,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                County = patient.County,
                SubCounty = patient.SubCounty,
                Country = patient.Country,
                ZipCode = patient.ZipCode,
                Status = patient.Status.ToString(),
                RequiresGuardian = patient.RequiresGuardian,
                GuardianName = patient.GuardianName,
                GuardianRelationship = patient.GuardianRelationship,
                GuardianPhone = patient.GuardianPhone,
                CreatedAt = patient.CreatedAt,
                CreatedBy = patient.CreatedBy
            };
        }
    }
}