using LifeCare.Personnel.Application.Dtos;
using LifeCare.Personnel.Application.Interfaces;
using MediatR;

namespace LifeCare.Personnel.Application.Queries;

public class GetAllPersonnelQueryHandler : IRequestHandler<GetAllPersonnelQuery, List<PersonnelDto>>  // ✅ Fixed class name
{
    private readonly IPersonnelRepository _personnelRepository;

    public GetAllPersonnelQueryHandler(IPersonnelRepository personnelRepository)  // ✅ Fixed constructor name
    {
        _personnelRepository = personnelRepository;
    }

    public async Task<List<PersonnelDto>> Handle(GetAllPersonnelQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Min(100, Math.Max(1, request.PageSize));  // ✅ Fixed: Min(100, ...) prevents huge pages
        
        var personnel = await _personnelRepository.GetPagedAsync(page, pageSize, cancellationToken);
        return personnel.Select(PersonnelDto.FromPersonnel).ToList();
    }
}