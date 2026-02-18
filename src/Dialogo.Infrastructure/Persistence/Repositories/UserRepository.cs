using Dialogo.Domain.Entities;
using Dialogo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dialogo.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users.FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Users.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Users.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
    }

    public void Update(User entity)
    {
        _context.Users.Update(entity);
    }

    public void Remove(User entity)
    {
        _context.Users.Remove(entity);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email.ToLower(), cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email.ToLower(), cancellationToken);
    }

    public async Task<User?> GetByPublicCodeAsync(string publicCode, CancellationToken cancellationToken)
    {
        return await _context.Users
            .SingleOrDefaultAsync(u => u.PublicCode == publicCode, cancellationToken);
    }
}
