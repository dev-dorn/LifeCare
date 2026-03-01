using LifeCare.Personnel.Application.Dtos;
using LifeCare.Personnel.Application.Interfaces;
using MediatR;

namespace LifeCare.Personnel.Application.Queries;

public class GetPersonnelByIdQueryHandler : IRequestHandler<GetPersonnelByIdQuery, PersonnelDto?>
{
    private readonly IPersonnelRepository _repository;
    private readonly ICacheServices _cache;


    public GetPersonnelByIdQueryHandler(
        IPersonnelRepository repository,
        ICacheServices cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<PersonnelDto?> Handle(GetPersonnelByIdQuery request, CancellationToken cancellationToken)
    {
        //Try cache first
        var cacheKey = "personnel:" + request.Id.ToString();
        var cached = await _cache.GetAsync<PersonnelDto>(cacheKey);

        if (cached != null)
            return cached;
        //Get from database
        var personnel = await _repository.GetByIdAsync(request.Id);
        if (personnel == null)
            return null;

        var dto = PersonnelDto.FromPersonnel(personnel);
        
        //Cache it
        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromHours(1));
        return dto;
    }
}