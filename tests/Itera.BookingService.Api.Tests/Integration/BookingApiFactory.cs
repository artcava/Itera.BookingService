using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Branch;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Itera.BookingService.Api.Tests.Integration;

public sealed class BookingApiFactory : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureServices(services =>
		{
			services.RemoveAll<ITokenValidationService>();
			services.RemoveAll<IBranchInfoQueryService>();

			services.AddSingleton<ITokenValidationService, FakeTokenValidationService>();
			services.AddSingleton<IBranchInfoQueryService, FakeBranchInfoQueryService>();
		});
	}

	private sealed class FakeTokenValidationService : ITokenValidationService
	{
		public Task<TokenValidationResult> ValidateAsync(string token, int tokenValidPeriodHours, CancellationToken cancellationToken)
		{
			if (string.Equals(token, "test-token", StringComparison.Ordinal))
			{
				return Task.FromResult(new TokenValidationResult
				{
					IsValid = true,
					ErrorCode = LegacyErrorCodes.Success,
					WsUserId = 123,
					BrandId = 1
				});
			}

			if (string.Equals(token, "expired-token", StringComparison.Ordinal))
			{
				return Task.FromResult(new TokenValidationResult
				{
					IsValid = false,
					ErrorCode = LegacyErrorCodes.ExpiredToken
				});
			}

			return Task.FromResult(new TokenValidationResult
			{
				IsValid = false,
				ErrorCode = LegacyErrorCodes.InvalidToken
			});
		}
	}

	private sealed class FakeBranchInfoQueryService : IBranchInfoQueryService
	{
		public Task<List<WsFiliale>> GetAllBranchesAsync(short brandId, bool getExtraData, bool getFilialiExtra, byte languageId, DateTime selectedDate, CancellationToken cancellationToken)
		{
			var list = new List<WsFiliale>
			{
				new()
				{
					BranchID = 10,
					Description = languageId == 2 ? "Milan Central" : "Milano Centrale",
					FranchiseID = 77,
					KeyBox = true,
					StateID = 1,
					ExcludeVAL = false,
					ExtraData = getExtraData
						? new WsFilialeExtraData { Address = "Via Roma 1", City = "Milano", Province = "MI", Region = "Lombardia" }
						: new WsFilialeExtraData()
				},
				new()
				{
					BranchID = 20,
					Description = languageId == 2 ? "Rome Airport" : "Roma Aeroporto",
					FranchiseID = 88,
					KeyBox = false,
					StateID = 2,
					ExcludeVAL = false,
					ExtraData = getExtraData
						? new WsFilialeExtraData { Address = "Via Appia 2", City = "Roma", Province = "RM", Region = "Lazio" }
						: new WsFilialeExtraData()
				}
			};

			if (!getFilialiExtra)
			{
				list = list.Where(x => x.StateID == 1).ToList();
			}

			return Task.FromResult(list);
		}

		public Task<WsFiliale?> GetInfoBranchAsync(short brandId, int branchId, bool getFilialiExtra, byte languageId, DateTime selectedDate, CancellationToken cancellationToken)
		{
			if (branchId != 10)
			{
				return Task.FromResult<WsFiliale?>(null);
			}

			return Task.FromResult<WsFiliale?>(new WsFiliale
			{
				BranchID = 10,
				Description = "Milano Centrale",
				FranchiseID = 77,
				KeyBox = true,
				StateID = 1,
				ExcludeVAL = false,
				ExtraData = new WsFilialeExtraData
				{
					Address = "Via Roma 1",
					City = "Milano",
					Province = "MI",
					Region = "Lombardia",
					PostalCode = "20100",
					Telephone = "020000000"
				}
			});
		}
	}
}
