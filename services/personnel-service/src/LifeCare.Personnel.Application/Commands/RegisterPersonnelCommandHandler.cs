using LifeCare.Personnel.Application.Common;
using LifeCare.Personnel.Application.Dtos;
using LifeCare.Personnel.Application.Interfaces;
using LifeCare.Personnel.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LifeCare.Personnel.Application.Commands;

public class RegisterPersonnelCommandHandler : IRequestHandler<RegisterPersonnelCommand, Result<PersonnelDto>>
{
    private readonly IPersonnelRepository _repository;
    private readonly ICacheServices _cache;
    private readonly IEventBus _eventBus;
    private readonly ILogger<RegisterPersonnelCommandHandler> _logger;

    public RegisterPersonnelCommandHandler(
        IPersonnelRepository repository,
        ICacheServices cache,
        IEventBus eventBus,
        ILogger<RegisterPersonnelCommandHandler> logger
    )
    {
        _repository = repository;
        _cache = cache;
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task<Result<PersonnelDto>> Handle(RegisterPersonnelCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Registering personnel: {Email}, Role:{Role}",
                request.Email,
                request.Role);

            //check if email already exists
            var existing = await _repository.GetByEmailAsync(request.Email);
            if (existing != null)
            {
                return Result<PersonnelDto>.Failure(
                    $"Personnel with email `{request.Email}` already exists");
            }

            //create personnel
            var personnel = Domain.Personnel.Create(
                request.FullName,
                request.Email,
                request.Role,
                request.Privileges,
                request.Createdby
            );
            //Save the database
            await _repository.AddAsync(personnel);
            await _repository.SaveChangesAsync(cancellationToken);
            //Cache the personnel
            var dto = PersonnelDto.FromPersonnel(personnel);
            await _cache.SetAsync(
                $"personnel: {personnel.Id}",
                dto,
                TimeSpan.FromMinutes(5));
            // Publish events
            foreach (var domainEvent in personnel.DomainEvents)
            {
                await _eventBus.PublishAsync(domainEvent);
            }

            _logger.LogInformation(
                "Personnel registered successfully: {id}",
                personnel.Id);
            return Result<PersonnelDto>.Success(dto);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Error registering personnel");
            return Result<PersonnelDto>.Failure(ex.Message);


        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering personnel");
            return Result<PersonnelDto>.Failure(
                "An unexpected error occurred");
        }
    }
}
