using FluentValidation;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Application.Branch;
using Itera.BookingService.Application.Estimate;
using Itera.BookingService.Application.Estimate.Abstractions;
using Itera.BookingService.Application.Security.Services;
using Itera.BookingService.Application.Vehicle;
using Microsoft.Extensions.DependencyInjection;

namespace Itera.BookingService.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddBookingApplication(this IServiceCollection services)
    {
        // Registra automaticamente tutti gli IValidator<T> presenti nell'assembly Application.
        // Nessuna registrazione manuale necessaria: aggiungere il validator è sufficiente.
        services.AddValidatorsFromAssemblyContaining<ApplicationServiceCollectionExtensions>(ServiceLifetime.Scoped);

        // Branch
        services.AddScoped<ILegacyBranchService, LegacyBranchService>();

        // Security
        services.AddScoped<ISecurityService, LegacySecurityService>();

        // Vehicle
        services.AddScoped<ILegacyVehicleService, LegacyVehicleService>();

        // Estimate
        services.AddScoped<IDurationService, DurationService>();
        services.AddScoped<ILegacyEstimateService, LegacyEstimateService>();

        return services;
    }
}
