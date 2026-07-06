using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Itera.BookingService.Api.Tests.Integration;

public class LegacyRoutesTests : IClassFixture<BookingApiFactory>
{
    private readonly HttpClient _client;
    private readonly BookingApiFactory _factory;

    public LegacyRoutesTests(BookingApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Remove("X-Api-Token");
        _client.DefaultRequestHeaders.Add("X-Api-Token", "test-token");
    }

    // -------------------------------------------------------------------------
    // Infrastructure
    // -------------------------------------------------------------------------

    [Fact]
    public async Task Root_Returns_Service_Metadata()
    {
        var response = await _client.GetAsync("/");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Itera BookingService API", payload.GetProperty("service").GetString());
        Assert.Equal("Legacy compatibility", payload.GetProperty("mode").GetString());
    }

    [Fact]
    public async Task Health_Returns_Success()
    {
        var response = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // -------------------------------------------------------------------------
    // Stub endpoints (NOT_IMPLEMENTED) — solo Estimate (non ancora migrati)
    // -------------------------------------------------------------------------

    [Theory]
    [MemberData(nameof(NotImplementedEndpoints))]
    public async Task Legacy_Json_Endpoints_Return_Strict_Contract_Snapshot(string serviceName, string endpointName)
    {
        var path = $"/{serviceName}.svc/{endpointName}";
        var response = await _client.PostAsJsonAsync(path, new { });

        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();

        var propertyNames = payload.EnumerateObject().Select(x => x.Name).OrderBy(x => x).ToArray();
        Assert.Equal(["codiceErrore", "data", "esito", "messaggio"], propertyNames);

        Assert.False(payload.GetProperty("esito").GetBoolean());
        Assert.Equal("NOT_IMPLEMENTED", payload.GetProperty("codiceErrore").GetString());
        Assert.Equal($"Endpoint '{serviceName}/{endpointName}' non ancora migrato.", payload.GetProperty("messaggio").GetString());
        Assert.Equal(JsonValueKind.Null, payload.GetProperty("data").ValueKind);
    }

    public static IEnumerable<object[]> NotImplementedEndpoints()
    {
        return
        [
            ["EstimateService", "GetEstimate"],
            ["EstimateService", "EstimateConfirmation"],
            ["EstimateService", "GetAccessoryBooking"],
            ["EstimateService", "GetAccessoryBookingFromEstimate"],
            ["EstimateService", "GetInsuranceExtra"],
            ["EstimateService", "GetInsuranceExtraFromEstimate"],
            ["EstimateService", "GetAmountEstimate"],
            ["EstimateService", "GetWholeEstimate"]
        ];
    }

    // -------------------------------------------------------------------------
    // Estimate
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetAllCategory_Returns_Categories_For_Default_Brand()
    {
        var response = await _client.PostAsJsonAsync("/EstimateService.svc/GetAllCategory", new
        {
            language = "ita"
        });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.GetProperty("esito").GetBoolean());

        var data = payload.GetProperty("data");
        Assert.Equal(JsonValueKind.Array, data.ValueKind);
        Assert.Equal(4, data.GetArrayLength());
        Assert.Equal("A", data[0].GetProperty("categoryID").GetString());
        Assert.Equal("Auto", data[0].GetProperty("description").GetString());
    }

    [Fact]
    public async Task GetAllCategory_English_Returns_Translated_Descriptions()
    {
        var response = await _client.PostAsJsonAsync("/EstimateService.svc/GetAllCategory", new
        {
            language = "en"
        });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.GetProperty("esito").GetBoolean());

        var data = payload.GetProperty("data");
        Assert.Equal("Car", data[0].GetProperty("description").GetString());
        Assert.Equal("Van", data[1].GetProperty("description").GetString());
    }

    [Fact]
    public async Task GetKms_Empty_Payload_Returns_ValidationError()
    {
        var response = await _client.PostAsJsonAsync("/EstimateService.svc/GetKms", new { });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.False(payload.GetProperty("esito").GetBoolean());
        Assert.Equal("VALIDATION_ERROR", payload.GetProperty("codiceErrore").GetString());
        Assert.False(string.IsNullOrWhiteSpace(payload.GetProperty("messaggio").GetString()));
    }

    [Fact]
    public async Task GetKms_Response_Shape_Is_WsResponse_Contract()
    {
        var response = await _client.PostAsJsonAsync("/EstimateService.svc/GetKms", new { });

        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        var propertyNames = payload.EnumerateObject().Select(x => x.Name).OrderBy(x => x).ToArray();
        Assert.Equal(["codiceErrore", "data", "esito", "messaggio"], propertyNames);
    }

    [Fact]
    public async Task GetProvince_Returns_Esito_True_With_Province_List()
    {
        var response = await _client.PostAsJsonAsync("/EstimateService.svc/GetProvince", new { });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.GetProperty("esito").GetBoolean());

        var data = payload.GetProperty("data");
        Assert.Equal(JsonValueKind.Array, data.ValueKind);
        Assert.True(data.GetArrayLength() > 0);
    }

    [Fact]
    public async Task GetProvince_Response_Shape_Is_WsResponse_Contract()
    {
        var response = await _client.PostAsJsonAsync("/EstimateService.svc/GetProvince", new { });

        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        var propertyNames = payload.EnumerateObject().Select(x => x.Name).OrderBy(x => x).ToArray();
        Assert.Equal(["codiceErrore", "data", "esito", "messaggio"], propertyNames);
    }

    [Fact]
    public async Task GetProvince_Each_Item_Has_CodiceProvincia_And_Descrizione()
    {
        var response = await _client.PostAsJsonAsync("/EstimateService.svc/GetProvince", new { });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        var data    = payload.GetProperty("data");
        Assert.True(data.GetArrayLength() > 0);

        var first = data[0];
        Assert.True(first.TryGetProperty("codiceProvincia",      out var codice));
        Assert.True(first.TryGetProperty("descrizioneProvincia", out _));
        Assert.False(string.IsNullOrWhiteSpace(codice.GetString()));
    }

    // -------------------------------------------------------------------------
    // Security
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetToken_With_Valid_Credentials_Returns_Token()
    {
        var response = await _client.PostAsJsonAsync("/SecurityService.svc/GetToken", new
        {
            username = "utente_ok",
            password = "password_ok"
        });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.GetProperty("esito").GetBoolean());
        var token = payload.GetProperty("data").GetProperty("token").GetString();
        Assert.False(string.IsNullOrWhiteSpace(token));
        Assert.True(Guid.TryParse(token, out _));
    }

    [Fact]
    public async Task GetToken_With_Invalid_Credentials_Returns_Esito_False()
    {
        var response = await _client.PostAsJsonAsync("/SecurityService.svc/GetToken", new
        {
            username = "utente_sbagliato",
            password = "password_errata"
        });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.False(payload.GetProperty("esito").GetBoolean());
        Assert.Equal(JsonValueKind.Null, payload.GetProperty("data").ValueKind);
    }

    [Fact]
    public async Task ValidateToken_With_Valid_Token_Returns_Esito_True()
    {
        var response = await _client.PostAsJsonAsync("/SecurityService.svc/ValidateToken", new
        {
            token = "aaaaaaaa-0000-0000-0000-000000000001"
        });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.GetProperty("esito").GetBoolean());
    }

    [Fact]
    public async Task ValidateToken_With_Invalid_Token_Returns_Esito_False()
    {
        var response = await _client.PostAsJsonAsync("/SecurityService.svc/ValidateToken", new
        {
            token = "00000000-0000-0000-0000-000000000000"
        });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.False(payload.GetProperty("esito").GetBoolean());
    }

    [Fact]
    public async Task Security_Endpoints_Are_Accessible_Without_Token_Header()
    {
        using var anonymousClient = _factory.CreateClient();

        var response = await anonymousClient.PostAsJsonAsync("/SecurityService.svc/GetToken", new
        {
            username = "utente_ok",
            password = "password_ok"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // -------------------------------------------------------------------------
    // Branch
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetInfoBranch_With_Valid_Token_Returns_Branch_Data()
    {
        var response = await _client.PostAsJsonAsync("/BranchService.svc/GetInfoBranch", new
        {
            branchID = 10,
            language = "ita",
            dateStart = "not-a-date"
        });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.GetProperty("esito").GetBoolean());

        var data = payload.GetProperty("data");
        Assert.Equal(10, data.GetProperty("branchID").GetInt32());
        Assert.Equal("Milano Centrale", data.GetProperty("description").GetString());
    }

    [Fact]
    public async Task GetInfoBranch_With_Invalid_BranchId_Returns_Legacy_Error()
    {
        var response = await _client.PostAsJsonAsync("/BranchService.svc/GetInfoBranch", new
        {
            branchID = 0,
            language = "ita"
        });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.False(payload.GetProperty("esito").GetBoolean());
        Assert.Equal("-201", payload.GetProperty("codiceErrore").GetString());
        Assert.Equal("Invalid BranchID parameter", payload.GetProperty("messaggio").GetString());
    }

    [Fact]
    public async Task GetInfoBranch_Payload_Token_Overrides_Header_Token()
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/BranchService.svc/GetInfoBranch")
        {
            Content = JsonContent.Create(new
            {
                token = "test-token",
                branchID = 10,
                language = "eng"
            })
        };
        request.Headers.Add("X-Api-Token", "expired-token");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.GetProperty("esito").GetBoolean());
        Assert.Equal(10, payload.GetProperty("data").GetProperty("branchID").GetInt32());
    }

    [Fact]
    public async Task GetAllBranches_With_Valid_Token_Returns_Data()
    {
        var response = await _client.PostAsJsonAsync("/BranchService.svc/GetAllBranches", new
        {
            language = "ita",
            getExtraData = true,
            getFilialiExtra = true
        });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.GetProperty("esito").GetBoolean());

        var data = payload.GetProperty("data");
        Assert.Equal(JsonValueKind.Array, data.ValueKind);
        Assert.Equal(2, data.GetArrayLength());
        Assert.Equal("Milano Centrale", data[0].GetProperty("description").GetString());
        Assert.Equal("Via Roma 1", data[0].GetProperty("extraData").GetProperty("address").GetString());
    }

    [Fact]
    public async Task GetAllBranches_Without_FilialiExtra_Filters_State2()
    {
        var response = await _client.PostAsJsonAsync("/BranchService.svc/GetAllBranches", new
        {
            language = "eng",
            getExtraData = false,
            getFilialiExtra = false,
            token = "test-token"
        });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.GetProperty("esito").GetBoolean());

        var data = payload.GetProperty("data");
        Assert.Equal(1, data.GetArrayLength());
        Assert.Equal("Milan Central", data[0].GetProperty("description").GetString());
    }

    // -------------------------------------------------------------------------
    // Vehicle
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetVehicle_Response_Shape_Is_WsResponse_Contract()
    {
        var response = await _client.PostAsJsonAsync("/VehicleService.svc/GetVehicle", new { });

        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        var propertyNames = payload.EnumerateObject().Select(x => x.Name).OrderBy(x => x).ToArray();
        Assert.Equal(["codiceErrore", "data", "esito", "messaggio"], propertyNames);
    }

    [Fact]
    public async Task GetVehicle_NoFilters_Returns_Esito_True_With_Array()
    {
        var response = await _client.PostAsJsonAsync("/VehicleService.svc/GetVehicle", new { });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.GetProperty("esito").GetBoolean());
        Assert.Equal(JsonValueKind.Array, payload.GetProperty("data").ValueKind);
        Assert.Equal(2, payload.GetProperty("data").GetArrayLength());
    }

    [Fact]
    public async Task GetVehicle_SegmentoMulti_Filters_Result()
    {
        var response = await _client.PostAsJsonAsync("/VehicleService.svc/GetVehicle", new
        {
            segmentoMulti = "ECO"
        });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.GetProperty("esito").GetBoolean());

        var data = payload.GetProperty("data");
        Assert.Equal(1, data.GetArrayLength());
        Assert.Equal("ECO", data[0].GetProperty("codiceSegmento").GetString());
        Assert.Equal("Fiat", data[0].GetProperty("marca").GetString());
    }

    [Fact]
    public async Task GetVehicle_Invalid_GruppoID_Returns_ValidationError()
    {
        var response = await _client.PostAsJsonAsync("/VehicleService.svc/GetVehicle", new
        {
            gruppoID = 0
        });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.False(payload.GetProperty("esito").GetBoolean());
        Assert.Equal("VALIDATION_ERROR", payload.GetProperty("codiceErrore").GetString());
        Assert.False(string.IsNullOrWhiteSpace(payload.GetProperty("messaggio").GetString()));
    }

    [Fact]
    public async Task GetVehicle_Without_Token_Returns_Esito_False()
    {
        using var anonymousClient = _factory.CreateClient();

        var response = await anonymousClient.PostAsJsonAsync("/VehicleService.svc/GetVehicle", new { });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.False(payload.GetProperty("esito").GetBoolean());
    }
}
