using FluentValidation;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Application.Branch;
using Microsoft.Extensions.DependencyInjection;

namespace Itera.BookingService.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddBookingApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidator<Contracts.Legacy.Branch.WsGetAllFilialiRequest>, WsGetAllFilialiRequestValidator>();
        services.AddScoped<IValidator<Contracts.Legacy.Branch.WsGetFilialeInfoRequest>, WsGetFilialeInfoRequestValidator>();
        services.AddScoped<ILegacyBranchService, LegacyBranchService>();

        return services;
    }
}
