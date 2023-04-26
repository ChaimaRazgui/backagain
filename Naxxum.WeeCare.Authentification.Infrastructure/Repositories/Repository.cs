using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Naxxum.WeeCare.Authentification.Application.Abstractions;
using Naxxum.WeeCare.Authentification.Domain.Entities;
using Naxxum.WeeCare.Authentification.Infrastructure.Data;

namespace Naxxum.WeeCare.Authentification.Infrastructure.Repositories;

public abstract class Repository<T> : IRepository<T> where T : AggregateRoot
{
    private readonly AppDbContext _dbContext;

    protected Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        => _dbContext.Set<T>().AnyAsync(expression, cancellationToken);

    public void Add(T entity)
        => _dbContext.Add(entity);

    public void Remove(T entity)
        => _dbContext.Remove(entity);
}