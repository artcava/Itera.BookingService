using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Application.Estimate.Abstractions;
using Itera.BookingService.Application.Security;
using Itera.BookingService.Contracts.Options;
using Itera.BookingService.Infrastructure.Auth;
using Itera.BookingService.Infrastructure.Branch;
using Itera.BookingService.Infrastructure.Estimate;
using Itera.BookingService.Infrastructure.Persistence;
using Itera.BookingService.Infrastructure.Security;
using Itera.BookingService.Infrastructure.Vehicle;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Itera.BookingService.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddBookingInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthOptions>(configuration.GetSection(AuthOptions.SectionName));

        var optionsSection = configuration.GetSection(InfrastructureOptions.SectionName);
        var options = new InfrastructureOptions
        {
            EnableDetailedErrors  = bool.TryParse(optionsSection["EnableDetailedErrors"],  out var detailed) && detailed,
            CommandTimeoutSeconds = int.TryParse(optionsSection["CommandTimeoutSeconds"], out var timeout)  ? timeout : 30
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

        services.AddScoped<ITokenValidationService, LegacyTokenValidationService>();
        services.AddScoped<IBranchInfoQueryService, LegacyBranchInfoQueryService>();

        // Security
        services.AddScoped<ISecurityQueryService, SecurityQueryService>();

        // Vehicle
        services.AddScoped<IVehicleQueryService, VehicleQueryService>();

        // Estimate
        services.AddScoped<IKmQueryService, KmQueryService>();
        services.AddScoped<IProvinceQueryService, ProvinceQueryService>();
        services.AddScoped<INationQueryService, NationQueryService>();
        services.AddScoped<IEstimateAccessoryQueryService, EstimateAccessoryQueryService>();
        services.AddScoped<IEstimateInsuranceQueryService, EstimateInsuranceQueryService>();

        return services;
    }
}
