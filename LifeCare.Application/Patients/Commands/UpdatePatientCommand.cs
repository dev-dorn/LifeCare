using LifeCare.Application.common;
using LifeCare.Application.Patients.Dtos;
using MediatR;

namespace LifeCare.Application.Patients.Commands;

public class UpdatePatientCommand : IRequest<Result<PatientDto>>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string County { get; set; }
    public string SubCounty { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }
    public int Status { get; set; }
    public string? UpdatedBy { get; set; }
    public string? GuardianName{get; set;}
    public string? GuardianRelationship{get; set;}
    public string? GuardianPhone{get; set;}
    public string NationalId{get; set;}
    
}