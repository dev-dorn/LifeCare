// LifeCare.API/Controllers/PatientsController.cs

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using LifeCare.Application.Patients.Commands;
using LifeCare.Application.Patients.Queries;
using LifeCare.Application.Common;
using LifeCare.Application.Patients.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LifeCare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PatientsController> _logger;
        
        public PatientsController(IMediator mediator, ILogger<PatientsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpPost("register")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RegisterPatient([FromBody] RegisterPatientRequest request)
        {
            _logger.LogInformation("Patient registration request received");
            
            var command = new RegisterPatientCommand
            {
                NationalId = request.NationalId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Street = request.Street,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                Guardian = request.Guardian,
                ReceptionistId = User?.Identity?.Name ?? "System"
            };
            
            var result = await _mediator.Send(command);
            
            if (result.IsSuccess)
            {
                _logger.LogInformation("Patient registered successfully. MRN: {MRN}", result.Data.MRN);
                
                return CreatedAtAction(
                    nameof(GetPatient),
                    new { mrn = result.Data.MRN },
                    new ApiResponse<PatientDto>
                    {
                        Success = true,
                        Data = result.Data,
                        Message = "Patient registered successfully"
                    });
            }
            
            _logger.LogWarning("Patient registration failed: {Error}", result.Error);
            
            return BadRequest(new ApiResponse<PatientDto>
            {
                Success = false,
                Error = result.Error
            });
        }
        
        [HttpGet("{mrn}")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPatient(string mrn)
        {
            var query = new GetPatientByMrnQuery { MRN = mrn };
            var result = await _mediator.Send(query);
            
            if (result.IsSuccess)
            {
                return Ok(new ApiResponse<PatientDto>
                {
                    Success = true,
                    Data = result.Data
                });
            }
            
            return NotFound(new ApiResponse<PatientDto>
            {
                Success = false,
                Error = result.Error
            });
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(List<PatientDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPatients(
            [FromQuery] string? status = null,
            [FromQuery] string? search = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetPatientsQuery
            {
                Status = status,
                Search = search,
                Page = page,
                PageSize = pageSize
            };
            
            var result = await _mediator.Send(query);
            
            return Ok(new ApiResponse<List<PatientDto>>
            {
                Success = true,
                Data = result.Data
            });
        }
    }
    
    // Request DTOs
    public class RegisterPatientRequest
    {
        [Required(ErrorMessage = "National ID is required")]
        [StringLength(50, MinimumLength = 5)]
        public string NationalId { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        [RegularExpression("^(Male|Female|Other|Unknown)$", ErrorMessage = "Invalid gender")]
        public string Gender { get; set; }
        
        [Required]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        
        public GuardianRequest Guardian { get; set; }
    }
    
    public class GuardianRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Relationship { get; set; }
        public string PhoneNumber { get; set; }
    }
    
    // API Response Wrapper
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }
    }
}