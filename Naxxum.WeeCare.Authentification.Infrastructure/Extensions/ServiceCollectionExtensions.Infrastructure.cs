using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Naxxum.WeeCare.Authentification.Infrastructure.Repositories;
using Naxxum.WeeCare.Authentification.Application.Abstractions;

namespace Naxxum.WeeCare.Authentification.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IUsersRepository, UsersRepository>();
        return services;
    }
}