using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Application.Security.Dtos;
using Itera.BookingService.Application.Security.Services;
using Itera.BookingService.Contracts.General;
using Itera.BookingService.Contracts.Branch;
using Itera.BookingService.Contracts.Estimate;
using Itera.BookingService.Contracts.Vehicle;
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
			services.RemoveAll<IBranchQueryService>();
			services.RemoveAll<ISecurityService>();
			services.RemoveAll<IVehicleQueryService>();
			services.RemoveAll<IProvinceQueryService>();
			services.RemoveAll<IEstimateQueryService>();

			services.AddSingleton<ITokenValidationService, FakeTokenValidationService>();
			services.AddSingleton<IBranchQueryService, FakeBranchInfoQueryService>();
			services.AddSingleton<ISecurityService, FakeSecurityService>();
			services.AddSingleton<IVehicleQueryService, FakeVehicleQueryService>();
			services.AddSingleton<IProvinceQueryService, FakeProvinceQueryService>();
			services.AddSingleton<IEstimateQueryService, FakeEstimateQueryService>();
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
					ErrorCode = ApiErrorCodes.Success,
					WsUserId = 123,
					BrandId = 1
				});
			}

			if (string.Equals(token, "expired-token", StringComparison.Ordinal))
			{
				return Task.FromResult(new TokenValidationResult
				{
					IsValid = false,
					ErrorCode = ApiErrorCodes.ExpiredToken
				});
			}

			return Task.FromResult(new TokenValidationResult
			{
				IsValid = false,
				ErrorCode = ApiErrorCodes.InvalidToken
			});
		}
	}

	private sealed class FakeSecurityService : ISecurityService
	{
		private static readonly Guid ValidToken = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001");

		public Task<ApiResponse<AuthTokenData>> GetTokenAsync(GetTokenRequest request, CancellationToken ct)
		{
			if (request.Username == "utente_ok" && request.Password == "password_ok")
			{
				return Task.FromResult(new ApiResponse<AuthTokenData>
				{
					Esito = true,
					CodiceErrore = ApiErrorCodes.Success.ToString(),
					Messaggio = string.Empty,
					Data = new AuthTokenData(ValidToken.ToString())
				});
			}

			return Task.FromResult(new ApiResponse<AuthTokenData>
			{
				Esito = false,
				CodiceErrore = ApiErrorCodes.InvalidToken.ToString(),
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
					CodiceErrore = ApiErrorCodes.Success.ToString(),
					Messaggio = string.Empty,
					Data = null
				});
			}

			return Task.FromResult(new ApiResponse<object?>
			{
				Esito = false,
				CodiceErrore = ApiErrorCodes.InvalidToken.ToString(),
				Messaggio = "Token non valido",
				Data = null
			});
		}

		public Task<ApiResponse<object?>> ResetKeyCacheAsync(ResetKeyCacheRequest request, CancellationToken ct)
		{
			return Task.FromResult(new ApiResponse<object?>
			{
				Esito = true,
				CodiceErrore = ApiErrorCodes.Success.ToString(),
				Messaggio = string.Empty,
				Data = null
			});
		}
	}

	private sealed class FakeBranchInfoQueryService : IBranchQueryService
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

	private sealed class FakeEstimateQueryService : IEstimateQueryService
	{
		private const string ValidEstimateToken = "bbbbbbbb-0000-0000-0000-000000000001";
		private const string LegacyShapeEstimateToken = "bbbbbbbb-0000-0000-0000-000000000002";

		public Task<EstimateTokenValidationResult> ValidateEstimateTokenAsync(
			string estimateToken,
			int tokenValidPeriodSeconds,
			CancellationToken cancellationToken)
		{
			if (!string.Equals(estimateToken, ValidEstimateToken, StringComparison.OrdinalIgnoreCase)
				&& !string.Equals(estimateToken, LegacyShapeEstimateToken, StringComparison.OrdinalIgnoreCase))
				return Task.FromResult(new EstimateTokenValidationResult(-3, null));

			var objectDynParam = """
			{
			  "KmType": { "S": 1 },
			  "StateSegment": { "ECO": "V" },
			  "Accessory": { "ECO": [ 10 ] }
			}
			""";

			var objectEstimate = string.Equals(estimateToken, LegacyShapeEstimateToken, StringComparison.OrdinalIgnoreCase)
				? """
				{
				  "Segmenti": [
				    {
				      "CodiceSegmento": "ECO",
				      "ImportiPreventivoSegmento": [
				        {
				          "KmTypeDescr": "S",
				          "Importo": "100.00",
				          "ImportoNoIva": "81.97",
				          "ImportiSenzaSconto": {
				            "Importo": "120.00",
				            "ImportoNoIva": "98.36"
				          },
				          "CodiceSconto": [
				            {
				              "ValoreSconto": -20.0,
				              "KeySconto": "PROMO20",
				              "DiscountTypeID": 3,
				              "DiscountTypeDescription": "PROMOWEB",
				              "RegolaDiVenditaID": 777
				            }
				          ]
				        }
				      ]
				    }
				  ]
				}
				"""
				: """
			{
			  "Segments": [
			    {
			      "CodeSegment": "ECO",
			      "AmountSegmentEstimate": [
			        {
			          "KmType": "S",
			          "Amount": "100.00",
			          "AmountWithoutIVA": "81.97",
			          "AmountsWithoutDiscount": {
			            "Amount": "120.00",
			            "AmountWithoutIVA": "98.36"
			          },
			          "DiscountList": [
			            {
			              "HDN_SCN": -20.0,
			              "HDN_SCN_KEY": "PROMO20",
			              "DiscountTypeID": 3,
			              "DiscountTypeDescription": "PROMOWEB",
			              "RegolaDiVenditaID": 777
			            }
			          ]
			        }
			      ]
			    }
			  ]
			}
			""";

			var snapshot = new EstimateTokenSnapshot(
				WsUserId: 123,
				FilialeId: 10,
				FilialeDestinazioneId: 10,
				DataFromPreventivo: DateTime.Today,
				DataToPreventivo: DateTime.Today.AddDays(1),
				Giorni: 1,
				ListinoId: 100,
				CodiceDurata: "GG",
				CodiceCategoria: "A",
				ObjectDynParam: objectDynParam,
				ObjectEstimate: objectEstimate,
				VoucherCliente: null);

			return Task.FromResult(new EstimateTokenValidationResult(0, snapshot));
		}

		public Task<bool> AcceptsNonSellableSegmentAsync(int wsUserId, CancellationToken cancellationToken)
			=> Task.FromResult(false);

		public Task<Dictionary<short, string>> GetAccessoryCodesByIdsAsync(
			IReadOnlyCollection<short> accessoryIds,
			CancellationToken cancellationToken)
		{
			var map = new Dictionary<short, string>();
			foreach (var id in accessoryIds)
				map[id] = "OTH";
			return Task.FromResult(map);
		}

		public Task<List<EstimateInsuranceOption>> GetInsuranceOptionsAsync(
			string segmentCode,
			int rentalDays,
			int catalogId,
			DateTime dateFrom,
			DateTime dateTo,
			CancellationToken cancellationToken)
		{
			return Task.FromResult(new List<EstimateInsuranceOption>
			{
				new(1, "SERENITY")
			});
		}

		public Task<short?> GetCurrentIvaIdAsync(CancellationToken cancellationToken)
			=> Task.FromResult<short?>(22);

		public Task<decimal?> GetCurrentIvaPercentageAsync(CancellationToken cancellationToken)
			=> Task.FromResult<decimal?>(22m);

		public Task<List<AccessoryBookingDto>> GetAccessoryBookingAsync(
			short brandId,
			int branchId,
			int branchDestinationId,
			int catalogId,
			int rentalDays,
			DateTime dateFrom,
			DateTime dateTo,
			string? categoryId,
			string? segmentCode,
			CancellationToken cancellationToken)
		{
			return Task.FromResult(new List<AccessoryBookingDto>
			{
				new()
				{
					AccessoryId = 10,
					Code = "OTH",
					Amount = 0m,
					AmountVat = 0m
				},
				new()
				{
					AccessoryId = 11,
					Code = "OTH",
					Amount = 10m,
					AmountVat = 12.2m
				}
			});
		}

		public Task<List<InsuranceExtraDto>> GetInsuranceExtraAsync(
			string segmentCode,
			DateTime dateFrom,
			DateTime dateTo,
			int rentalDays,
			int catalogId,
			CancellationToken cancellationToken)
		{
			return Task.FromResult(new List<InsuranceExtraDto>
			{
				new()
				{
					InsuranceExtraID = 1,
					InsuranceExtra = "SERENITY",
					InsuranceExtraDescr = "Copertura Serenity",
					InsuranceExtraWithoutIVA = "10.00"
				}
			});
		}

        public Task<EstimateDto> GetWholeEstimateAsync(short brandId, string estimateToken, string? segmentCode, string? KmType, IReadOnlyCollection<int>? InsuranceExtraList, IReadOnlyCollection<InsuranceRequest>? InsuranceList, IReadOnlyCollection<AccessoryRequest>? AccessoryList, string? BookingCode, bool Prepaid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
