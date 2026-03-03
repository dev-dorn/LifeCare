using LifeCare.Personnel.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LifeCare.Personnel.Application.Commands;

public class UpdatePersonnelCommandHandler : IRequestHandler<UpdatePersonnelCommand, bool>
{
    private readonly IPersonnelRepository _personnelRepository;
    private readonly ILogger<UpdatePersonnelCommandHandler> _logger;

    public UpdatePersonnelCommandHandler(IPersonnelRepository personnelRepository,
        ILogger<UpdatePersonnelCommandHandler> logger)
    {
        _personnelRepository = personnelRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdatePersonnelCommand request, CancellationToken cancellationToken)
    {
        var personnel = await _personnelRepository.GetByIdAsync(request.Id);
        if (personnel is null)
        {
            _logger.LogWarning("Personnel not found: {PersonnelId}", request.Id);
            return false;
        }
        personnel.UpdateInfo(request.Personnel.FullName, request.Personnel.Email);
        personnel.ChangeRole(request.Personnel.Role, null); 

        await _personnelRepository.UpdateAsync(personnel);
        await _personnelRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Personnel updated: {PersonnelId}", request.Id);
        return true;
    }
}
