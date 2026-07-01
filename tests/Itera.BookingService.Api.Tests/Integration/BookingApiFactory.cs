using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Application.Estimate.Abstractions;
using Itera.BookingService.Application.Security.Dtos;
using Itera.BookingService.Application.Security.Services;
using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Branch;
using Itera.BookingService.Contracts.Legacy.Estimate;
using Itera.BookingService.Contracts.Legacy.Vehicle;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Itera.BookingService.Api.Tests.Integration;

public sealed class BookingApiFactory : WebApplicationFactory<IApiMarker>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureServices(services =>
		{
			services.RemoveAll<ITokenValidationService>();
			services.RemoveAll<IBranchInfoQueryService>();
			services.RemoveAll<ISecurityService>();
			services.RemoveAll<IVehicleQueryService>();
			services.RemoveAll<IProvinceQueryService>();

			services.AddSingleton<ITokenValidationService, FakeTokenValidationService>();
			services.AddSingleton<IBranchInfoQueryService, FakeBranchInfoQueryService>();
			services.AddSingleton<ISecurityService, FakeSecurityService>();
			services.AddSingleton<IVehicleQueryService, FakeVehicleQueryService>();
			services.AddSingleton<IProvinceQueryService, FakeProvinceQueryService>();
		});
	}

	// ------------------------------------------------------------------
	// Fakes
	// ------------------------------------------------------------------

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

	private sealed class FakeSecurityService : ISecurityService
	{
		private static readonly Guid ValidToken = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001");

		public Task<ApiResponse<WsAuth>> GetTokenAsync(GetTokenRequest request, CancellationToken ct)
		{
			if (request.Username == "utente_ok" && request.Password == "password_ok")
			{
				return Task.FromResult(new ApiResponse<WsAuth>
				{
					Esito = true,
					CodiceErrore = LegacyErrorCodes.Success.ToString(),
					Messaggio = string.Empty,
					Data = new WsAuth(ValidToken.ToString())
				});
			}

			return Task.FromResult(new ApiResponse<WsAuth>
			{
				Esito = false,
				CodiceErrore = LegacyErrorCodes.InvalidToken.ToString(),
				Messaggio = "Credenziali non valide",
				Data = null
			});
		}

		public Task<ApiResponse<object?>> ValidateTokenAsync(ValidateTokenRequest request, CancellationToken ct)
		{
			if (request.Token == ValidToken.ToString())
			{
				return Task.FromResult(new ApiResponse<object?>
				{
					Esito = true,
					CodiceErrore = LegacyErrorCodes.Success.ToString(),
					Messaggio = string.Empty,
					Data = null
				});
			}

			return Task.FromResult(new ApiResponse<object?>
			{
				Esito = false,
				CodiceErrore = LegacyErrorCodes.InvalidToken.ToString(),
				Messaggio = "Token non valido",
				Data = null
			});
		}

		public Task<ApiResponse<object?>> ResetKeyCacheAsync(ResetKeyCacheRequest request, CancellationToken ct)
		{
			return Task.FromResult(new ApiResponse<object?>
			{
				Esito = true,
				CodiceErrore = LegacyErrorCodes.Success.ToString(),
				Messaggio = string.Empty,
				Data = null
			});
		}
	}

	private sealed class FakeBranchInfoQueryService : IBranchInfoQueryService
	{
		public Task<List<FilialeDto>> GetAllBranchesAsync(short brandId, bool getExtraData, bool getFilialiExtra, byte languageId, DateTime selectedDate, CancellationToken cancellationToken)
		{
			var list = new List<FilialeDto>
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
						? new FilialeExtraDataDto { Address = "Via Roma 1", City = "Milano", Province = "MI", Region = "Lombardia" }
						: new FilialeExtraDataDto()
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
					? new FilialeExtraDataDto { Address = "Via Appia 2", City = "Roma", Province = "RM", Region = "Lazio" }
					: new FilialeExtraDataDto()
				}
			};

			if (!getFilialiExtra)
			{
				list = list.Where(x => x.StateID == 1).ToList();
			}

			return Task.FromResult(list);
		}

		public Task<FilialeDto?> GetInfoBranchAsync(short brandId, int branchId, bool getFilialiExtra, byte languageId, DateTime selectedDate, CancellationToken cancellationToken)
		{
			if (branchId != 10)
			{
				return Task.FromResult<FilialeDto?>(null);
			}

			return Task.FromResult<FilialeDto?>(new FilialeDto
			{
				BranchID = 10,
				Description = "Milano Centrale",
				FranchiseID = 77,
				KeyBox = true,
				StateID = 1,
				ExcludeVAL = false,
				ExtraData = new FilialeExtraDataDto
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

	private sealed class FakeVehicleQueryService : IVehicleQueryService
	{
		private static readonly List<MezzoSegmento> AllMezzi =
		[
			new()
			{
				ModelloMezzoID          = 1,
				Marca                   = "Fiat",
				ModelloDescr            = "Panda",
				CodiceSegmento          = "ECO",
				SegmentoDescrizione     = "Economy",
				AlimentazioneModelloID  = 1,
				AlimentazioneDescr      = "Benzina",
				SegmentoModelloClasseID = 1,
				SegmentoModelloClasseIDDescrizione = "Utilitaria"
			},
			new()
			{
				ModelloMezzoID          = 2,
				Marca                   = "Volkswagen",
				ModelloDescr            = "Golf",
				CodiceSegmento          = "MID",
				SegmentoDescrizione     = "Intermediate",
				AlimentazioneModelloID  = 2,
				AlimentazioneDescr      = "Diesel",
				SegmentoModelloClasseID = 2,
				SegmentoModelloClasseIDDescrizione = "Berlina"
			}
		];

		public Task<List<MezzoSegmento>> GetMezziAsync(
			string? fleetMulti,
			string? segmentoMulti,
			bool? mezzoSpeciale,
			int? gruppoId,
			CancellationToken cancellationToken)
		{
			var result = AllMezzi.ToList();

			if (!string.IsNullOrWhiteSpace(segmentoMulti))
			{
				var segmenti = segmentoMulti.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
				result = result.Where(m => segmenti.Contains(m.CodiceSegmento, StringComparer.OrdinalIgnoreCase)).ToList();
			}

			return Task.FromResult(result);
		}
	}

	private sealed class FakeProvinceQueryService : IProvinceQueryService
	{
		private static readonly List<GetProvince> Province =
		[
			new() { CodiceProvincia = "MI", DescrizioneProvincia = "Milano" },
			new() { CodiceProvincia = "RM", DescrizioneProvincia = "Roma" },
			new() { CodiceProvincia = "TO", DescrizioneProvincia = "Torino" }
		];

		public Task<List<GetProvince>> GetProvinceAsync(CancellationToken ct = default)
			=> Task.FromResult(Province);
	}
}
