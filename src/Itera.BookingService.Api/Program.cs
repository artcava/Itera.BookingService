using Itera.BookingService.Api.Endpoints;
using Itera.BookingService.Application.DependencyInjection;
using Itera.BookingService.Infrastructure.DependencyInjection;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.OpenApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry();

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
	loggerConfiguration
		.ReadFrom.Configuration(context.Configuration)
		.ReadFrom.Services(services)
		.Enrich.FromLogContext()
		.WriteTo.Console();

	var aiConnectionString = context.Configuration["ApplicationInsights:ConnectionString"]
		?? Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING");

	if (!string.IsNullOrWhiteSpace(aiConnectionString))
	{
		var telemetryConfiguration = services.GetService<TelemetryConfiguration>();
		if (telemetryConfiguration is not null)
		{
			loggerConfiguration.WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces);
		}
	}
});

builder.Services.AddBookingApplication();
builder.Services.AddBookingInfrastructure(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Itera BookingService API",
		Version = "v1",
		Description = "Minimal API endpoints."
	});

	var legacyTokenScheme = new OpenApiSecurityScheme
	{
		Name = "X-Api-Token",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Description = "Legacy token header. If payload Token and header differ, payload takes precedence."
	};

	options.AddSecurityDefinition("LegacyToken", legacyTokenScheme);
	options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
	{
		[new OpenApiSecuritySchemeReference("LegacyToken", document, externalResource: null)] = new List<string>()
	});
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
	options.SwaggerEndpoint("/swagger/v1/swagger.json", "Itera BookingService API v1");
	options.RoutePrefix = "swagger";
	options.DisplayRequestDuration();
});

app.MapGet("/", () => Results.Ok(new
{
	Service = "Itera BookingService API",
	Mode = "Legacy compatibility",
	Runtime = ".NET 10 Minimal API"
}));

app.MapHealthChecks("/health");

app.MapServiceEndpoints();
app.MapOpenApi();

app.Run();
