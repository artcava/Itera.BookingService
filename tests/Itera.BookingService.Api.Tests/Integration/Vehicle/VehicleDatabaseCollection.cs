using Xunit;

namespace Itera.BookingService.Api.Tests.Integration.Vehicle;

/// <summary>
/// Collega la fixture al collection xUnit: il container Docker viene
/// avviato una sola volta e condiviso da tutti i test della classe.
/// </summary>
[CollectionDefinition(nameof(VehicleDatabaseCollection))]
public sealed class VehicleDatabaseCollection : ICollectionFixture<VehicleDatabaseFixture>;
