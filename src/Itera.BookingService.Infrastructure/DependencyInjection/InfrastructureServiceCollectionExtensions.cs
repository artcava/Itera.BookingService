using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Application.Security;
using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Infrastructure.Auth;
using Itera.BookingService.Infrastructure.Branch;
using Itera.BookingService.Infrastructure.Execution;
using Itera.BookingService.Infrastructure.Persistence;
using Itera.BookingService.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Itera.BookingService.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddBookingInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LegacyAuthOptions>(configuration.GetSection(LegacyAuthOptions.SectionName));

        var optionsSection = configuration.GetSection(LegacyInfrastructureOptions.SectionName);
        var options = new LegacyInfrastructureOptions
        {
            EnableDetailedErrors = bool.TryParse(optionsSection["EnableDetailedErrors"], out var detailed) && detailed,
            CommandTimeoutSeconds = int.TryParse(optionsSection["CommandTimeoutSeconds"], out var timeout) ? timeout : 30
        };

        var connectionString = configuration.GetConnectionString("LegacyMain")
            ?? throw new InvalidOperationException("Connection string 'LegacyMain' is missing.");

        services.AddSingleton(options);

        services.AddDbContext<LegacyDbContext>(db =>
        {
            db.UseSqlServer(connectionString, sql =>
            {
                sql.CommandTimeout(options.CommandTimeoutSeconds);
                sql.EnableRetryOnFailure();
            });

            if (options.EnableDetailedErrors)
            {
                db.EnableDetailedErrors();
                db.EnableSensitiveDataLogging();
            }

            // Intentional: no migrations pipeline here. The existing DB schema is authoritative.
        });

        services.AddScoped<ILegacyEndpointExecutor, LegacyEndpointExecutor>();
        services.AddScoped<ITokenValidationService, LegacyTokenValidationService>();
        services.AddScoped<IBranchInfoQueryService, LegacyBranchInfoQueryService>();

        // Security
        services.AddScoped<ISecurityQueryService, SecurityQueryService>();

        return services;
    }
}
