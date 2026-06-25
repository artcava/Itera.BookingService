namespace Itera.BookingService.Api.Endpoints;

public static class LegacyEndpointFilters
{
    public static RouteHandlerBuilder RequireLegacyToken(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<LegacyTokenEndpointFilter>();
    }
}
