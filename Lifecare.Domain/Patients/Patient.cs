// LifeCare.Domain/Patients/Patient.cs

using Lifecare.Domain.Common;
using LifeCare.Domain.Common;
using Lifecare.Domain.Events;
using LifeCare.Domain.Patients.Enums;
using LifeCare.Domain.Patients.ValuedObjects;

namespace LifeCare.Domain.Patients
{
    public class Patient : AggregateRoot
    {
        public PatientId Id { get; private set; }
        public MedicalRecordNumber MRN { get; private set; }
        public NationalId NationalId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string Gender { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }
        public PatientStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public string GuardianName { get; private set; }
        public string GuardianRelationship { get; private set; }
        public string GuardianPhone { get; private set; }
        
        // Computed properties
        public int Age => CalculateAge();
        public bool RequiresGuardian => Age < 18;
        
        // Private constructor for EF Core
        private Patient() { }
        
        // Factory method
        public static Patient Create(
            NationalId nationalId,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string gender,
            string phoneNumber,
            MedicalRecordNumber mrn,
            string createdBy)
        {
            ValidateInput(nationalId.Value, firstName, lastName, dateOfBirth, gender, phoneNumber);
            
            var patient = new Patient
            {
                Id = PatientId.New(),
                NationalId = nationalId,
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                DateOfBirth = dateOfBirth,
                Gender = gender,
                PhoneNumber = phoneNumber.Trim(),
                MRN = mrn,
                Status = PatientStatus.AwaitingTriage,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy
            };
            
            // Add domain event
            patient.AddDomainEvent(
                new PatientRegisteredEvent(patient.Id, patient.MRN.Value));
            
            return patient;
        }
        
        public void UpdateContactInfo(string email, string street, string city, string state, string zipCode)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!IsValidEmail(email))
                    throw new DomainException("Invalid email format");
                    
                Email = email.Trim();
            }
            
            Street = street?.Trim();
            City = city?.Trim();
            State = state?.Trim();
            ZipCode = zipCode?.Trim();
        }
        
        public void AssignGuardian(string name, string relationship, string phone)
        {
            if (!RequiresGuardian)
                throw new DomainException("Guardian is only required for patients under 18");
                
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Guardian name is required");
                
            GuardianName = name.Trim();
            GuardianRelationship = relationship?.Trim();
            GuardianPhone = phone?.Trim();
        }
        
        private static void ValidateInput(
            string nationalId,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string gender,
            string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(nationalId))
                throw new DomainException("National ID is required");
                
            if (string.IsNullOrWhiteSpace(firstName))
                throw new DomainException("First name is required");
                
            if (string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("Last name is required");
                
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new DomainException("Phone number is required");
                
            if (dateOfBirth > DateTime.Today)
                throw new DomainException("Date of birth cannot be in the future");
                
            // Validate gender
            var validGenders = new[] { "Male", "Female", "Other", "Unknown" };
            if (!validGenders.Contains(gender))
                throw new DomainException($"Gender must be one of: {string.Join(", ", validGenders)}");
        }
        
        private int CalculateAge()
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            
            if (DateOfBirth.Date > today.AddYears(-age))
                age--;
                
            return age;
        }
        
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        
        public void MoveToTriage()
        {
            if (Status != PatientStatus.AwaitingTriage)
                throw new DomainException($"Patient must be in AwaitingTriage status. Current: {Status}");
                
            Status = PatientStatus.InTriage;
        }
    }
}