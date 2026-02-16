using LifeCare.Modules.Patients.Domain.Enums;
using LifeCare.Modules.Shared.Domain.Common;
using LifeCare.Modules.Patients.Domain.Events;

namespace LifeCare.Modules.Patients.Domain
{
    public class Patient : AggregateRoot
    {
        public Guid Id { get; private set; }
        public string MRN { get; private set; }
        public string ShifNumber { get; private set; }
        public string? NationalId { get; private set; }
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

        // Computed properties
        public int Age => CalculateAge();

        private int CalculateAge()
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year; 
            if (DateOfBirth.Date > today.AddYears(-age)) age--; 
            return age;
        }
        public string FullName => $"{FirstName} {LastName}";
        public bool RequiresGuardian => Age < 13;

        private Patient()
        {
            MRN = string.Empty; ShifNumber = string.Empty; FirstName = string.Empty; LastName = string.Empty; Gender = string.Empty; PhoneNumber = string.Empty; Email = string.Empty; County = string.Empty; SubCounty = string.Empty; Country = string.Empty; ZipCode = string.Empty; CreatedBy = string.Empty;
        }

        public static Patient Create(
            string shifNumber,
            string? nationalId,
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
            ValidateInput(shifNumber, firstName, lastName, dateOfBirth, gender, phoneNumber, nationalId);

            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                ShifNumber = shifNumber.Trim(),
                NationalId = nationalId?.Trim(),
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                DateOfBirth = dateOfBirth,
                Gender = gender,
                PhoneNumber = phoneNumber.Trim(),
                MRN = mrn,
                Status = PatientStatus.AwaitingTriage,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy,
                Email = email?.Trim() ?? string.Empty,
                County = county?.Trim() ?? string.Empty,
                SubCounty = subCounty?.Trim() ?? string.Empty,
                Country = country?.Trim() ?? string.Empty,
                ZipCode = zipCode?.Trim() ?? string.Empty
            };

            if (patient.RequiresGuardian)
            {
                patient.AssignGuardian(guardianName, guardianRelationship, guardianPhone);
            }

            patient.AddDomainEvent(new PatientRegisteredEvent(patient.Id, patient.MRN));
            return patient;
        }

        public static Patient CreateForSeed(
            Guid id,
            string mrn,
            string shifNumber,
            string? nationalId,
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
            ValidateInput(shifNumber, firstName, lastName, dateOfBirth, gender, phoneNumber, nationalId);

            var patient = new Patient
            {
                Id = id,
                MRN = mrn,
                ShifNumber = shifNumber.Trim(),
                NationalId = nationalId?.Trim(),
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
                GuardianName = guardianName?.Trim(),
                GuardianRelationship = guardianRelationship?.Trim(),
                GuardianPhone = guardianPhone?.Trim()
            };

            return patient;
        }

        public void Deactivate()
        {
            Status = PatientStatus.Inactive;
        }

        private static void ValidateInput(
            string shifNumber,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string gender,
            string phoneNumber,
            string? nationalId)
        {
            if (string.IsNullOrWhiteSpace(shifNumber))
                throw new DomainException("ShifNumber is required");

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

            var age = CalculateAge(dateOfBirth);
            if (age >= 18 && string.IsNullOrWhiteSpace(nationalId))
                throw new DomainException("National ID is required for patients 18 and older");
        }

        private static int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
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

        public void UpdateBasicInfo(string shifNumber, string? nationalId, string firstName, string lastName, string gender, DateTime dob)
        {
            ValidateInput(shifNumber, firstName, lastName, dob, gender, PhoneNumber, nationalId);

            ShifNumber = shifNumber.Trim();
            NationalId = nationalId?.Trim();
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            DateOfBirth = dob;
            Gender = gender;
        }

        public void UpdateStatus(PatientStatus newStatus) => Status = newStatus;

        public void UpdateGuardianInfo(string? name, string? relationship, string? phone)
        {
            GuardianName = name?.Trim();
            GuardianRelationship = relationship?.Trim();
            GuardianPhone = phone?.Trim();
        }

        public void AssignGuardian(string? name, string? relationship, string? phone)
        {
            if (!RequiresGuardian)
                throw new DomainException("Guardian is only required for patients under 13");

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
