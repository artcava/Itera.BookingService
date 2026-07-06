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
        services.AddValidatorsFromAssemblyContaining<EstimateService>(ServiceLifetime.Scoped);

        // Branch
        services.AddScoped<IBranchService, BranchService>();

        // Security
        services.AddScoped<ISecurityService, LegacySecurityService>();

        // Vehicle
        services.AddScoped<IVehicleService, VehicleService>();

        // Estimate
        services.AddScoped<IDurationService, DurationService>();
        services.AddScoped<IEstimateService, EstimateService>();

        return services;
    }
}
