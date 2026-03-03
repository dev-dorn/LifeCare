using LifeCare.Personnel.Application.Queries;

namespace LifeCare.Personnel.Application.Interfaces;

public interface IPersonnelRepository
{
    Task<Domain.Personnel?> GetByIdAsync(Guid id);
    Task<Domain.Personnel?> GetByEmailAsync(string email);
    Task AddAsync(Domain.Personnel personnel);
    Task UpdateAsync(Domain.Personnel personnel);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task <List<Domain.Personnel>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken= default);
}