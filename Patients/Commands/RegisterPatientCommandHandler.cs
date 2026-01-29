using MediatR;
using LifeCare.Domain.Patients;
using LifeCare.Application.Common;
using LifeCare.Domain.Common;

namespace Patients.Commands
{
    public class RegisterPatientCommandHandler : IRequestHandler<RegisterPatientCommand, Result<PatientDto>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMrnGenerator _mrnGenerator;
        private readonly ILogger<RegisterPatientCommandHandler> _logger;
        
        public RegisterPatientCommandHandler(
            IPatientRepository patientRepository,
            IMrnGenerator mrnGenerator,
            ILogger<RegisterPatientCommandHandler> logger)
        {
            _patientRepository = patientRepository;
            _mrnGenerator = mrnGenerator;
            _logger = logger;
        }
        
        public async Task<Result<PatientDto>> Handle(RegisterPatientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check for duplicate National ID
                var existingPatient = await _patientRepository.GetByNationalIdAsync(request.NationalId);
                if (existingPatient != null)
                {
                    return Result<PatientDto>.Failure(
                        $"Patient with National ID '{request.NationalId}' already exists (MRN: {existingPatient.MRN.Value})");
                }
                
                // Generate MRN
                var nextSequence = await _patientRepository.GetNextMrnSequenceAsync();
                var mrn = MedicalRecordNumber.Generate(nextSequence);
                
                // Create patient
                var patient = Patient.Create(
                    request.NationalId,
                    request.FirstName,
                    request.LastName,
                    request.DateOfBirth,
                    request.Gender,
                    request.PhoneNumber,
                    mrn,
                    request.ReceptionistId);
                
                // Add optional contact information
                if (!string.IsNullOrWhiteSpace(request.Email) ||
                    !string.IsNullOrWhiteSpace(request.Street))
                {
                    var address = new Address(
                        request.Street,
                        request.City,
                        request.State,
                        request.ZipCode);
                        
                    patient.UpdateContactInfo(request.Email, address);
                }
                
                // Add guardian if patient is a minor
                if (patient.RequiresGuardian)
                {
                    if (request.Guardian == null)
                    {
                        return Result<PatientDto>.Failure(
                            "Guardian information is required for patients under 18 years old");
                    }
                    
                    patient.AssignGuardian(
                        request.Guardian.FirstName,
                        request.Guardian.LastName,
                        request.Guardian.Relationship,
                        request.Guardian.PhoneNumber);
                }
                
                // Save patient
                await _patientRepository.AddAsync(patient);
                await _patientRepository.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation(
                    "Patient registered successfully. MRN: {MRN}, Name: {Name}, Age: {Age}",
                    patient.MRN.Value,
                    $"{patient.FirstName} {patient.LastName}",
                    patient.Age);
                
                // Return success
                var patientDto = PatientDto.FromPatient(patient);
                return Result<PatientDto>.Success(patientDto);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Domain validation failed during patient registration");
                return Result<PatientDto>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during patient registration");
                return Result<PatientDto>.Failure("An unexpected error occurred. Please try again.");
            }
        }
    }
}