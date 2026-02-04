// LifeCare.Application/Patients/Commands/RegisterPatientCommand.cs

using LifeCare.Application.Patients.Dtos;
using MediatR;

namespace LifeCare.Application.Patients.Commands
{
    public class RegisterPatientCommand : IRequest<RegisterPatientResult>
    {
        public string NationalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public GuardianRequest? Guardian { get; set; }
        public string? ReceptionistId { get; set; }
    }
    public class GuardianRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Relationship { get; set; }
        public string? PhoneNumber { get; set; }
    }
    
    public class RegisterPatientResult
    { 
        public bool IsSuccess { get; set; }
        public string MRN { get; set; } 
        public string Error { get; set; }
        public PatientDto PatientDto { get; set; }
        public string Name { get; set; } 
        public int Age { get; set; }
        public bool RequiresGuardian { get; set; }
        public static RegisterPatientResult Success(string mrn, PatientDto patientDto, string name, int age, bool requiresGuardian) 
        {
            return new RegisterPatientResult 
            { IsSuccess = true, MRN = mrn, PatientDto = patientDto, Name = name, Age = age, RequiresGuardian = requiresGuardian };
            
        }

        public static RegisterPatientResult Failure(string error)
        {
            return new RegisterPatientResult { IsSuccess = false, Error = error };
            
        } 
    }
}
