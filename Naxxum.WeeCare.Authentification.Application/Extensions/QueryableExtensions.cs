using AutoMapper.QueryableExtensions;
using Naxxum.WeeCare.Authentification.Application.Configurations;

namespace Naxxum.WeeCare.Authentification.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TResult> MapTo<TResult>(this IQueryable queryable)

    {
        return queryable.ProjectTo<TResult>(AutoMapperConfigurations.MapperConfiguration);
    }
}