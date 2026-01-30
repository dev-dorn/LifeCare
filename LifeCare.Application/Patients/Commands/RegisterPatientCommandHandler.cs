using LifeCare.Application.Interfaces;
using LifeCare.Application.Interfaces.Repositories;
using LifeCare.Application.Patients.Dtos;
using MediatR;
using LifeCare.Domain.Patients;
using LifeCare.Domain.Common;
using LifeCare.Domain.Patients.ValuedObjects;
using Microsoft.Extensions.Logging;

namespace LifeCare.Application.Patients.Commands
{
    public class RegisterPatientCommandHandler : IRequestHandler<RegisterPatientCommand, RegisterPatientResult>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<RegisterPatientCommandHandler> _logger;
        
        public RegisterPatientCommandHandler(
            IPatientRepository patientRepository,
            ILogger<RegisterPatientCommandHandler> logger)
        {
            _patientRepository = patientRepository;
            _logger = logger;
        }
        
        public async Task<RegisterPatientResult> Handle(
            RegisterPatientCommand request, 
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting patient registration for National ID: {NationalId}", request.NationalId);
                
                // Check for duplicate National ID
                var existingPatient = await _patientRepository.GetByNationalIdAsync(request.NationalId);
                if (existingPatient != null)
                {
                    return RegisterPatientResult.Failure(
                        $"Patient with National ID '{request.NationalId}' already exists (MRN: {existingPatient.MRN.Value})");
                }
                
                // Generate MRN using value object
                var nextSequence = await _patientRepository.GetNextMrnSequenceAsync();
                var mrn = MedicalRecordNumber.Generate(nextSequence);

                // Create NationalId value object
                var nationalIdVo = new NationalId(request.NationalId);

                // Create patient aggregate
                var patient = Patient.Create(
                    nationalIdVo,
                    request.FirstName,
                    request.LastName,
                    request.DateOfBirth,
                    request.Gender,
                    request.PhoneNumber,
                    mrn,
                    request.ReceptionistId);
                
                // Add optional contact information
                patient.UpdateContactInfo(
                    request.Email,
                    request.Street,
                    request.City,
                    request.State,
                    request.ZipCode);
                
                // Add guardian if patient is a minor
                if (patient.RequiresGuardian)
                {
                    if (string.IsNullOrWhiteSpace(request.GuardianName))
                    {
                        return RegisterPatientResult.Failure(
                            "Guardian information is required for patients under 18 years old");
                    }
                    
                    patient.AssignGuardian(
                        request.GuardianName,
                        request.GuardianRelationship,
                        request.GuardianPhone);
                }
                
                // Save patient
                await _patientRepository.AddAsync(patient);
                await _patientRepository.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation(
                    "Patient registered successfully. MRN: {MRN}, Name: {Name}, Age: {Age}",
                    patient.MRN,
                    $"{patient.FirstName} {patient.LastName}",
                    patient.Age);
                
                // Return success
                var patientDto = PatientDto.FromPatient(patient);
                return RegisterPatientResult.Success(
                    patient.MRN,
                    $"{patient.FirstName} {patient.LastName}",
                    patient.Age,
                    patient.RequiresGuardian);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Domain validation failed during patient registration");
                return RegisterPatientResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during patient registration");
                return RegisterPatientResult.Failure("An unexpected error occurred. Please try again.");
            }
        }
    }
}
