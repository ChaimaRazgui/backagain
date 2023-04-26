using System.Linq.Expressions;
using Naxxum.WeeCare.Authentification.Domain.Entities;

namespace Naxxum.WeeCare.Authentification.Application.Abstractions;

public interface IRepository<T> where T : AggregateRoot
{
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    void Add(T entity);
    void Remove(T entity);
}