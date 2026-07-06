namespace Itera.BookingService.Api.Endpoints;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapServiceEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapSecurityEndpoints();
        app.MapBranchEndpoints();
        app.MapEstimateEndpoints();
        app.MapVehicleEndpoints();

        return app;
    }
}
