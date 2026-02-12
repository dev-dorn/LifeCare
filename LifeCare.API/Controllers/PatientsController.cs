// LifeCare.API/Controllers/PatientsController.cs

using System.ComponentModel.DataAnnotations;
using LifeCare.API.Controllers.Requests;
using LifeCare.Application.Patients.Commands;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using LifeCare.Application.Patients.Queries;
using LifeCare.Application.Patients.Dtos;
using LifeCare.Domain.Patients;
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

        public PatientsController(
            IMediator mediator,
            ILogger<PatientsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // -----------------------------
        // REGISTER PATIENT
        // -----------------------------
        [HttpPost("register")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterPatient(
            [FromBody] RegisterPatientRequest request)
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
                County = request.County,
                SubCounty = request.SubCounty,
                Country = request.Country,
                ZipCode = request.ZipCode,
                ReceptionistId = User?.Identity?.Name ?? "System",

                Guardian = request.Guardian == null
                    ? null
                    : new LifeCare.Application.Patients.Commands.GuardianRequest
                    {
                        FirstName = request.Guardian.FirstName,
                        LastName = request.Guardian.LastName,
                        Relationship = request.Guardian.Relationship,
                        PhoneNumber = request.Guardian.PhoneNumber
                    }
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                _logger.LogWarning(
                    "Patient registration failed: {Error}",
                    result.Error);

                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Error = result.Error
                });
            }

            _logger.LogInformation(
                "Patient registered successfully. MRN: {MRN}",
                result.MRN);

            return CreatedAtAction(
                nameof(GetPatientByMrn),
                new { mrn = result.MRN },
                new ApiResponse<PatientDto>
                {
                    Success = true,
                    Data = result.PatientDto,
                    Message = "Patient registered successfully"
                });
        }

        // -----------------------------
        // GET PATIENT BY MRN
        // -----------------------------
        [HttpGet("mrn/{mrn}")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPatientByMrn(string mrn)
        {
            var patient =
                await _mediator.Send(new GetPatientByMrnQuery { MRN = mrn });

            if (patient == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Error = "Patient not found"
                });
            }

            return Ok(new ApiResponse<PatientDto>
            {
                Success = true,
                Data = patient
            });
        }

        // -----------------------------
        // GET ALL PATIENTS
        // -----------------------------
        [HttpGet]
        [ProducesResponseType(typeof(List<PatientDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPatients(
            [FromQuery] string? status = null,
            [FromQuery] string? search = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var patients = await _mediator.Send(
                new GetAllPatientsQuery
                {
                    Page = page,
                    PageSize = pageSize
                });

            return Ok(new ApiResponse<List<PatientDto>>
            {
                Success = true,
                Data = patients
            });
        }
        
        // 
        // GET PATIENTS BY ID
        //
        [HttpGet("{id:guid}")]  // Only matches GUIDs
        public async Task<IActionResult> GetPatient(Guid id)
        {
            var query = new GetPatientByIdQuery(id);
            var patient = await _mediator.Send(query);
    
            if (patient == null)
            {
                return NotFound(new { message = $"Patient with ID {id} not found" });
            }
    
            return Ok(new { success = true, data = patient });
        }
        //
        // GET PATIENT BY PHONE NUMBER
        //
        [HttpGet("phone/{phoneNumber}")]
        public async Task<IActionResult> GetPatientByPhoneNumber(string phoneNumber)
        {
            var query = new GetPatientByPhoneQuery(phoneNumber);
            var patient = await _mediator.Send(query);

            if (patient == null)
            {
                return NotFound(new { message = $"Patient with PhoneNumber {phoneNumber} not found" });
            }
            return Ok(new{success = true, data = patient });

        }
        //
        // SEARCH PATIENTS
        //
        [HttpGet("search")]
        public async Task<IActionResult> SearchPatients([FromQuery] string? name, [FromQuery] string? city)
        {
            var query = new SearchPatientQuery(name, city);
            var patients = await _mediator.Send(query);
    
            return Ok(new { success = true, data = patients });
        }
        [HttpGet("recent")]
        [ProducesResponseType(typeof(List<Patient>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecentPatients([FromQuery] int count = 10)
        {
            var patients = await _mediator.Send(new GetRecentPatientsQuery { Count = count });

            return Ok(new ApiResponse<List<Patient>>
            {
                Success = true,
                Data = patients
            });
        }

        //
        //PUT /api/patients/{id}
        //
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] UpdatePatientRequest request)
        {
            var command = new UpdatePatientCommand
            {
                Id = id,
                NationalId = request.NationalId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                County = request.County,
                SubCounty = request.SubCounty,
                Country = request.Country,
                ZipCode = request.ZipCode,
                GuardianName = request.GuardianName,
                GuardianRelationship = request.GuardianRelationship,
                GuardianPhone = request.GuardianPhone
            };
    
            var result = await _mediator.Send(command);
    
            if (!result.IsSuccess)
                return NotFound(new { success = false, error = result.Error });
        
            return Ok(new { success = true, data = result.Data });
        }
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdatePatientStatus(Guid id, [FromBody] UpdateStatusRequest request)
        {
            // Convert string → enum
            if (!Enum.TryParse<PatientStatus>(request.NewStatus, true, out var status))
                return BadRequest(new { success = false, error = "Invalid patient status" });

            // Create command with enum
            var command = new UpdatePatientStatusCommand(
                id,
                status,
                request.Notes,
                request.ChangedBy,
                request.ChangedAt
            );

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return NotFound(new { success = false, error = result.Error });

            return Ok(new { success = true, data = result.Data });
        }
       



       
    }

    // =============================
    // REQUEST DTOs (API LAYER)
    // =============================
    public class RegisterPatientRequest
    {
        [Required]
        public string NationalId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public string County { get; set; }
        public string SubCounty { get; set; }
        public string Country { get; set; }
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

    // =============================
    // API RESPONSE WRAPPER
    // =============================
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }
    }
    
}
