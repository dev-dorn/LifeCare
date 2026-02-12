using LifeCare.Domain.Common;
using LifeCare.Domain.Patients.Events;

namespace LifeCare.Domain.Patients
{
    public class Patient : AggregateRoot
    {
        public Guid Id { get; private set; }
        public string MRN { get; private set; }
        public string NationalId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string Gender { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string County { get; private set; }
        public string SubCounty { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }
        public PatientStatus? Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public string? GuardianName { get; private set; }
        public string? GuardianRelationship { get; private set; }
        public string? GuardianPhone { get; private set; }
        //Computed properties
        
        public int Age => CalculateAge();
        public string FullName => $"{FirstName} {LastName}";
        
        public bool RequiresGuardian => Age < 13;
        
        private Patient() { }
        
        public static Patient Create(
            string nationalId,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string gender,
            string phoneNumber,
            string mrn,
            string createdBy,
            string county,
            string subCounty,
            string country,
            string zipCode,
            string email,
            string? guardianName,
            string? guardianRelationship,
            string? guardianPhone
            )
        {
            ValidateInput(nationalId, firstName, lastName, dateOfBirth, gender, phoneNumber);
            
            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                NationalId = nationalId.Trim(),
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
            if (patient.RequiresGuardian)
            {
                patient.AssignGuardian(
                    guardianName,
                    guardianRelationship,
                    guardianPhone);
            }
            patient.AddDomainEvent(new PatientRegisteredEvent(patient.Id, patient.MRN));
            
            return patient;
        }
        // Add this method to your Patient class
        public static Patient CreateForSeed(
            Guid id,
            string mrn,
            string nationalId,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string gender,
            string phoneNumber,
            string email,
            string county,
            string subCounty,
            string country,
            string zipCode,
            DateTime createdAt,
            string createdBy,
            string guardianName = null,
            string guardianRelationship = null,
            string guardianPhone = null)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(nationalId)) 
                throw new DomainException("National ID is required");
            if (string.IsNullOrWhiteSpace(firstName)) 
                throw new DomainException("First name is required");
            if (string.IsNullOrWhiteSpace(lastName)) 
                throw new DomainException("Last name is required");
            if (string.IsNullOrWhiteSpace(phoneNumber)) 
                throw new DomainException("Phone number is required");
            if (dateOfBirth > DateTime.UtcNow) 
                throw new DomainException("Date of birth cannot be in the future");
    
            var patient = new Patient
            {
                Id = id,
                MRN = mrn,
                NationalId = nationalId.Trim(),
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                DateOfBirth = dateOfBirth,
                Gender = gender,
                PhoneNumber = phoneNumber.Trim(),
                Email = email?.Trim() ?? string.Empty,
                County = county?.Trim() ?? string.Empty,
                SubCounty = subCounty?.Trim() ?? string.Empty,
                Country = country?.Trim() ?? string.Empty,
                ZipCode = zipCode?.Trim() ?? string.Empty,
                Status = PatientStatus.AwaitingTriage,
                CreatedAt = createdAt,
                CreatedBy = createdBy,
                GuardianName = guardianName?.Trim() ?? string.Empty,
                GuardianRelationship = guardianRelationship?.Trim() ?? string.Empty,
                GuardianPhone = guardianPhone?.Trim() ?? string.Empty
            };
    
            return patient;
        }
        
        //Factory method for Api/commands(with optional
        
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
        
        public void UpdateContactInfo(string email, string county, string subCounty, string country, string zipCode)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!IsValidEmail(email))
                    throw new DomainException("Invalid email format");
                    
                Email = email.Trim();
            }
            
            County = county?.Trim();
            SubCounty = subCounty?.Trim();
            Country = country?.Trim();
            ZipCode = zipCode?.Trim();
        }

        public void UpdateBasicInfo(string nationalId, string firstName, string lastName, string gender, DateTime dob)
        {
            NationalId = nationalId;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dob;
            Gender = gender;

        }

        public void UpdateStatus(PatientStatus newStatus)
        {
            Status = newStatus;
        }

        public void UpdateGuardianInfo(string? name, string? relationship, string? phone)
        {
            GuardianName = name;
            GuardianRelationship = relationship;
            GuardianPhone = phone;
        }
        public void AssignGuardian(string name, string relationship, string phone)
        {
            if (!RequiresGuardian)
                throw new DomainException("Guardian is only required for patients under 18");
                
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Guardian name is required");
            if (string.IsNullOrWhiteSpace(relationship))
                throw new DomainException("Guardian relationship is required");
            if (string.IsNullOrWhiteSpace(phone))
                throw new DomainException("Guardian phone is required");
                
            GuardianName = name.Trim();
            GuardianRelationship = relationship.Trim();
            GuardianPhone = phone.Trim();
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
    }
}
