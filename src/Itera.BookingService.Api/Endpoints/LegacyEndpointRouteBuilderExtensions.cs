namespace Itera.BookingService.Api.Endpoints;

public static class LegacyEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapLegacyServiceEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapSecurityEndpoints();
        app.MapBranchEndpoints();
        app.MapEstimateEndpoints();
        app.MapVehicleEndpoints();

        return app;
    }
}
