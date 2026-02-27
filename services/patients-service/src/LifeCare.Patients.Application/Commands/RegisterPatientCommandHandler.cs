using LifeCare.Modules.Patients.Domain;
using LifeCare.Modules.Patients.Domain.ValuedObjects;
using LifeCare.Modules.Shared.Application.Interfaces.Repositories;
using LifeCare.Modules.Shared.Domain.Common;
using LifeCare.Patients.Application.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LifeCare.Modules.Patients.Application.Commands;

public class RegisterPatientCommandHandler : IRequestHandler<RegisterPatientCommand, RegisterPatientResult>
{
    private readonly ILogger<RegisterPatientCommandHandler> _logger;
    private readonly IPatientRepository _patientRepository;

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
                "Starting patient registration for National ID: {ShifNumber}",
                request.ShifNumber);
            // Check duplicate

            if (!string.IsNullOrWhiteSpace(request.NationalId))
            {
                var existingPatient =
                    await _patientRepository.GetByNationalIdAsync(request.NationalId);

                if (existingPatient != null)
                    return RegisterPatientResult.Failure(
                        $"Patient with National ID '{request.NationalId}' already exists (MRN: {existingPatient.MRN})");
            }

            var existingByShif = await _patientRepository.GetByShifNumberAsync(request.ShifNumber);
            if (existingByShif != null)
                return RegisterPatientResult.Failure(
                    $"Patient with SHIF Number '{request.ShifNumber}' already exists (MRN: {existingByShif.MRN})");

            // Generate MRN string
            var sequence = await _patientRepository.GetNextMrnSequenceAsync();
            var mrn = MedicalRecordNumber.Generate(sequence).Value;

            // Create patient (STRINGS ONLY)
            var patient = Patient.Create(
                request.ShifNumber,
                request.NationalId,
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.Gender,
                request.PhoneNumber,
                mrn,
                request.ReceptionistId ?? string.Empty,
                request.County ?? string.Empty, // county (or street in your DTO)
                request.SubCounty ?? string.Empty, // subCounty
                request.Country ?? string.Empty, // country
                request.ZipCode ?? string.Empty,
                request.Email ?? string.Empty,
                request.Guardian != null
                    ? $"{request.Guardian.FirstName} {request.Guardian.LastName}"
                    : null,
                request.Guardian?.Relationship,
                request.Guardian?.PhoneNumber
            );

            // Optional contact info
            patient.UpdateContactInfo(
                request.Email ?? string.Empty,
                request.County ?? string.Empty,
                request.SubCounty ?? string.Empty,
                request.Country ?? string.Empty,
                request.ZipCode ?? string.Empty);

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