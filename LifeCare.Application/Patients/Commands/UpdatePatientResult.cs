using LifeCare.Application.Patients.Dtos;

namespace LifeCare.Application.Patients.Commands;

public class UpdatePatientResult
{
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }
    public PatientDto? PatientDto { get; set; }

    public static UpdatePatientResult Success(PatientDto patientDto)
    {
        return new UpdatePatientResult
        {
            IsSuccess = true,
            PatientDto = patientDto,
        };
    }

    public static UpdatePatientResult Failure(string error)
    {
        return new UpdatePatientResult
        {
            IsSuccess = false,
            Error = error,
        };
    }
}