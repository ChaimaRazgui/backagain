using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Naxxum.WeeCare.Authentification.Application.Abstractions;
using Naxxum.WeeCare.Authentification.Domain.Entities;
using Naxxum.WeeCare.Authentification.Infrastructure.Data;

namespace Naxxum.WeeCare.Authentification.Infrastructure.Repositories;

public class UsersRepository : Repository<User>, IUsersRepository
{
    private readonly AppDbContext _dbContext;

    public UsersRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default) =>
        GetUserAsync(u => u.UserId == id, cancellationToken);

    public Task<User?> GetUserAsync(Expression<Func<User, bool>> expression,
        CancellationToken cancellationToken = default) =>
        _dbContext.Set<User>().FirstOrDefaultAsync(expression, cancellationToken);
    public async Task UpdateUserActiveStatusAsync(int userId, bool Active)
    {
        var user = await _dbContext.Users.FindAsync(userId);

        if (user is not null)
        {
            user.Active = Active;
            await _dbContext.SaveChangesAsync();
        }
    }
    public List<User> GetAllUsers()
    {
        return _dbContext.Users.ToList();
    }
    public bool DeleteUser(int Id)
    {
        var filteredData = _dbContext.Users.Where(x => x.UserId == Id).FirstOrDefault();
        var result = _dbContext.Remove(filteredData);
        _dbContext.SaveChanges();
        return result != null ? true : false;
    }
}