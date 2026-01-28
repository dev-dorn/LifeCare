// LifeCare.Domain/Patients/Patient.cs
using System;
using LifeCare.Domain.Common;
using LifeCare.Domain.Events;
using LifeCare.Domain.Patients.Enums;
using LifeCare.Domain.Patients.ValuedObjects;


namespace LifeCare.Domain.Patients
{
    public class Patient : AggregateRoot
    {
        // Identity
        public PatientId Id { get; private set; }
        public MedicalRecordNumber MRN { get; private set; }
        public NationalId NationalId { get; private set; }
        
        // Demographics
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string Gender { get; private set; }
        
        // Contact Information
        public string PhoneNumber { get; private set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        
        // Medical Status
        public PatientStatus Status { get; private set; }
        
        // Computed Properties
        public int Age => CalculateAge();
        public bool RequiresGuardian => Age < 18;
        
        // Guardian Information (for minors)
        public Guardian Guardian { get; private set; }
        
        // Audit
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        
        // Private constructor for EF Core
        private Patient() { }
        
        // Factory method for creating new patients
        public static Patient Create(
            string nationalId,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string gender,
            string phoneNumber,
            MedicalRecordNumber mrn,
            string createdBy)
        {
            // Validate required fields
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
            
            var patient = new Patient
            {
                Id = PatientId.New(),
                NationalId = new NationalId(nationalId),
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
            patient.AddDomainEvent(new PatientRegisteredEvent(patient.Id, patient.MRN.Value));
            
            return patient;
        }
        
        public void UpdateContactInfo(string email, Address address)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!IsValidEmail(email))
                    throw new DomainException("Invalid email format");
                    
                Email = email.Trim();
            }
            
            Address = address;
        }
        
        public void AssignGuardian(string firstName, string lastName, string relationship, string phoneNumber)
        {
            if (!RequiresGuardian)
                throw new DomainException("Guardian is only required for patients under 18");
                
            Guardian = new Guardian(firstName, lastName, relationship, phoneNumber);
        }
        
        public void MoveToTriage()
        {
            if (Status != PatientStatus.AwaitingTriage)
                throw new DomainException($"Patient must be in AwaitingTriage status to move to triage. Current status: {Status}");
                
            Status = PatientStatus.InTriage;
            AddDomainEvent(new PatientMovedToTriageEvent(Id));
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
    }
}