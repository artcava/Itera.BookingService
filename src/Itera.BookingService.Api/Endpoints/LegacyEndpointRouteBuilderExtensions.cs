namespace Itera.BookingService.Api.Endpoints;

public static class LegacyEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapLegacyServiceEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapBranchEndpoints();
        app.MapEstimateEndpoints();
        app.MapVehicleEndpoints();
        // Security endpoints registrati separatamente in Program.cs via MapSecurityEndpoints()

        return app;
    }
}
