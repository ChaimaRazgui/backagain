using Naxxum.WeeCare.Authentification.API.Middlewares;
using Naxxum.WeeCare.Authentification.Application.Options;

namespace Naxxum.WeeCare.Authentification.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JwtConfigKey));
        services.AddTransient<GlobalExceptionHandlerMiddleware>();
        return services;
    }
}