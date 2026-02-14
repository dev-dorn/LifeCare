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
        _logger.LogInformation(
            "Starting patient registration for National ID: {NationalId}",
            request.NationalId);

        // Check duplicate
        var existingPatient =
            await _patientRepository.GetByNationalIdAsync(request.NationalId);

        if (existingPatient != null)
        {
            return RegisterPatientResult.Failure(
                $"Patient with National ID '{request.NationalId}' already exists (MRN: {existingPatient.MRN})");
        }

        // Generate MRN string
        var sequence = await _patientRepository.GetNextMrnSequenceAsync();
        var mrn = MedicalRecordNumber.Generate(sequence).Value;

        // Create patient (STRINGS ONLY)
        var patient = Patient.Create(
            request.NationalId,
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Gender,
            request.PhoneNumber,
            mrn,
            request.ReceptionistId,
            request.County,        // county (or street in your DTO)
            request.SubCounty,          // subCounty
            request.Country,         // country
            request.ZipCode,
            request.Email,
            request.Guardian != null
                ? $"{request.Guardian.FirstName} {request.Guardian.LastName}"
                : null,
            request.Guardian?.Relationship,
            request.Guardian?.PhoneNumber
        );

        // Optional contact info
        patient.UpdateContactInfo(
            request.Email,
            request.County,
            request.SubCounty,
            request.Country,
            request.ZipCode);

        // Guardian logic (matches domain)
        

        await _patientRepository.AddAsync(patient);
        await _patientRepository.SaveChangesAsync(cancellationToken);
        var patientDto = PatientDto.FromPatient(patient);


        _logger.LogInformation(
            "Patient registered successfully. MRN: {MRN}, Name: {Name}",
            patient.MRN,
            $"{patient.FirstName} {patient.LastName}");

        return RegisterPatientResult.Success(
            patient.MRN,
            patientDto,
            $"{patient.FirstName} {patient.LastName}",
            patient.Age,
            patient.RequiresGuardian
            );
    }
    catch (DomainException ex)
    {
        _logger.LogWarning(ex, "Domain validation failed");
        return RegisterPatientResult.Failure(ex.Message);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unexpected error during patient registration");
        return RegisterPatientResult.Failure(
            "An unexpected error occurred. Please try again.");
    }
}

    }
}
