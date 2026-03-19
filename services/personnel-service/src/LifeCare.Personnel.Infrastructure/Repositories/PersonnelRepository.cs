using LifeCare.Personnel.Application.Dtos;
using LifeCare.Personnel.Application.Interfaces;
using LifeCare.Personnel.Application.Queries;
using LifeCare.Personnel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LifeCare.Personnel.Infrastructure.Repositories;

public class PersonnelRepository : IPersonnelRepository
{
    private readonly PersonnelDbContext _context;

    public PersonnelRepository(PersonnelDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Personnel?> GetByIdAsync(Guid id)
    {
        return await _context.Personnel.FindAsync(id);
    }

    public async Task<Domain.Personnel?> GetByEmailAsync(string email)
    {
        return await _context.Personnel
            .FirstOrDefaultAsync(p => p.Email == email.ToLower());
    }

    public async Task<List<Domain.Personnel>> GetAllAsync()
    {
        return await _context.Personnel
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Domain.Personnel>> GetByRoleAsync(Domain.Enums.PersonnelRole role)
    {
        return await _context.Personnel
            .Where(p => p.Role == role)
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .ToListAsync();
    }

    public async Task AddAsync(Domain.Personnel personnel)
    {
        await _context.Personnel.AddAsync(personnel);
    }

    public Task UpdateAsync(Domain.Personnel personnel)
    {
        _context.Personnel.Update(personnel);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Domain.Personnel>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
         return await _context.Personnel
             .AsNoTracking()
             .OrderByDescending(p => p.CreatedAt)
             .Skip((page - 1) * pageSize)
             .Take(pageSize)
             .ToListAsync(cancellationToken);
                 
                 
    }
}