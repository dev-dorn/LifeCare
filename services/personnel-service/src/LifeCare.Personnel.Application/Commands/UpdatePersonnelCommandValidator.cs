using FluentValidation;
using LifeCare.Personnel.Domain.Enums; // Ensure your Enums are imported

namespace LifeCare.Personnel.Application.Commands;

public class UpdatePersonnelCommandValidator : AbstractValidator<UpdatePersonnelCommand>
{
    public UpdatePersonnelCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
        RuleFor(p => p.Personnel).NotNull();

        // Standard Name & Email Rules
        RuleFor(p => p.Personnel.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.Personnel.LastName).NotEmpty().MaximumLength(50);
        
        RuleFor(p => p.Personnel.Email)
            .NotEmpty()
            .EmailAddress();

        // --- CONDITIONAL MEDICAL RULES ---

        // 1. Require LicenseNumber ONLY for Doctors and Nurses
        RuleFor(p => p.Personnel.LicenseNumber)
            .NotEmpty()
            .WithMessage("Medical License Number is required for clinical staff.")
            .When(p => p.Personnel.Role == PersonnelRole.Doctor || 
                       p.Personnel.Role == PersonnelRole.Nurse)
            .MaximumLength(20).WithMessage("License number is too long.");

        // 2. Require a Department for all staff except 'Admin'
        RuleFor(p => p.Personnel.DepartmentId)
            .NotEmpty()
            .WithMessage("Please assign a department for this staff member.")
            .When(p => p.Personnel.Role != PersonnelRole.Admin);

        // 3. Ensure 'Privileges' aren't empty for 'Pharmacist'
        RuleFor(p => p.Personnel.Privileges)
            .NotEmpty()
            .WithMessage("Pharmacists must have assigned dispensing privileges.")
            .When(p => p.Personnel.Role == PersonnelRole.Pharmacist);
        
        // Validate Phone Number format if provided
        RuleFor(p => p.Personnel.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Please enter a valid international phone number.")
            .When(p => !string.IsNullOrEmpty(p.Personnel.PhoneNumber));

// Ensure Privileges isn't just a list of empty strings
        RuleFor(p => p.Personnel.Privileges)
            .Must(list => list == null || list.All(s => !string.IsNullOrWhiteSpace(s)))
            .WithMessage("Privileges cannot contain empty values.");
        
    }
}