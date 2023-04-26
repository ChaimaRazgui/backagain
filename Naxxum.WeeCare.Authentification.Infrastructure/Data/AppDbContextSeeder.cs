using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Naxxum.WeeCare.Authentification.Application.Services;
using Naxxum.WeeCare.Authentification.Domain.Entities;

namespace Naxxum.WeeCare.Authentification.Infrastructure.Data;

public class AppDbContextSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContextSeeder>>();
        await SeedAsync(dbContext, logger);
    }

    private static async Task SeedAsync(AppDbContext dbContext, ILogger<AppDbContextSeeder> logger)
    {
        logger.LogInformation("Checking for pending database migrations...");
        var pendingMigrations = (await dbContext.Database.GetPendingMigrationsAsync()).ToArray();
        logger.LogInformation("Found {0} pending migrations", pendingMigrations.Length);
        if (pendingMigrations.Any())
        {
            logger.LogInformation("Applying pending migrations...");
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Applying pending migrations is done.");
        }

        logger.LogInformation("Checking if database has records...");
        if (await dbContext.Set<User>().AnyAsync())
        {
            logger.LogInformation("Database is not empty, stopping seeding...");
            return;
        }

        logger.LogInformation("Database is empty, adding new user 'admin' with default password");

        var (passwordHash, passwordSalt) = Sha512PasswordService.Generate("password");
        var user = User.Create("c.razgui2001@gmail.com", passwordHash, passwordSalt, true);

        dbContext.Add(user);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("User added successfully!");
        logger.LogInformation("Seeding is done!");

    }
}