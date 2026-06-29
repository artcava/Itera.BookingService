using FluentValidation;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Application.Branch;
using Itera.BookingService.Application.Security.Dtos;
using Itera.BookingService.Application.Security.Services;
using Itera.BookingService.Application.Security.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Itera.BookingService.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddBookingApplication(this IServiceCollection services)
    {
        // Branch
        services.AddScoped<IValidator<Contracts.Legacy.Branch.WsGetAllFilialiRequest>, WsGetAllFilialiRequestValidator>();
        services.AddScoped<IValidator<Contracts.Legacy.Branch.WsGetFilialeInfoRequest>, WsGetFilialeInfoRequestValidator>();
        services.AddScoped<ILegacyBranchService, LegacyBranchService>();

        // Security
        services.AddScoped<IValidator<GetTokenRequest>, GetTokenRequestValidator>();
        services.AddScoped<IValidator<ValidateTokenRequest>, ValidateTokenRequestValidator>();
        services.AddScoped<ISecurityService, LegacySecurityService>();

        return services;
    }
}
