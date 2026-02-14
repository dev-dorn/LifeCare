using LifeCare.Application.common;
using LifeCare.Application.Patients.Dtos;
using MediatR;

namespace LifeCare.Application.Patients.Commands;

public class UpdatePatientCommand : IRequest<Result<PatientDto>>
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Gender { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? County { get; set; }
    public string? SubCounty { get; set; }
    public string? Country { get; set; }
    public string? ZipCode { get; set; }
    public int Status { get; set; }
    public string? UpdatedBy { get; set; }
    public string? GuardianName{get; set;}
    public string? GuardianRelationship{get; set;}
    public string? GuardianPhone{get; set;}
    public required string NationalId{get; set;}
    
}