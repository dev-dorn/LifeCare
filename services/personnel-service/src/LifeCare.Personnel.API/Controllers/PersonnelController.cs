using LifeCare.Personnel.API.Request;
using LifeCare.Personnel.Application.Commands;
using LifeCare.Personnel.Application.Dtos;
using LifeCare.Personnel.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeCare.Personnel.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonnelController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PersonnelController> _logger;

    public PersonnelController(
        IMediator mediator,
        ILogger<PersonnelController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(PersonnelDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterPersonnel([FromBody] RegisterPersonnelRequest request)
    {
        var command = new RegisterPersonnelCommand
        {
            FullName = request.FullName,
            Email = request.Email,
            Role = request.Role,
            Privileges = request.Privileges
        };

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { success = false, error = result.Error });

        return CreatedAtAction(
            nameof(GetPersonnelById),
            new { id = result.Data!.Id },
            new { success = true, data = result.Data });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PersonnelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPersonnelById(Guid id)
    {
        var query = new GetPersonnelByIdQuery(id);
        var personnel = await _mediator.Send(query);

        if (personnel == null)
            return NotFound(new { success = false, error = "Personnel not found" });

        return Ok(new { success = true, data = personnel });
    }
    // get all personnel
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<PersonnelDto>>> GetAll([FromQuery] GetAllPersonnelQuery query)
    {
        try
        {
            _logger.LogInformation("Fetching personnel - Page: {Page}, PageSize: {PageSize}", 
                query.Page, query.PageSize);

            var personnel = await _mediator.Send(query, HttpContext.RequestAborted);
            
            return Ok(personnel);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid pagination parameters: Page={Page}, PageSize={PageSize}", 
                query.Page, query.PageSize);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching personnel");
            return StatusCode(500, new { message = "An error occurred while fetching personnel" });
        }
    }
}