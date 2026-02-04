// LifeCare.Application/Patients/Dtos/PatientDto.cs

using LifeCare.Domain.Patients;

namespace LifeCare.Application.Patients.Dtos
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        public required string Mrn { get; set; }
        public string NationalId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Status { get; set; }
        public bool RequiresGuardian { get; set; }
        public string GuardianName { get; set; }
        public string GuardianRelationship { get; set; }
        public string GuardianPhone { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        
        public static PatientDto FromPatient(Patient patient)
        {
            if (patient == null) return null;
            
            return new PatientDto
            {
                Id = patient.Id,
                Mrn = patient.MRN,
                NationalId = patient.NationalId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Age = patient.Age,
                Gender = patient.Gender,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                Street = patient.Street,
                City = patient.City,
                State = patient.State,
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