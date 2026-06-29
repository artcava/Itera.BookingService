using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Itera.BookingService.Api.Tests.Integration;

public class LegacyRoutesTests : IClassFixture<BookingApiFactory>
{
    private readonly HttpClient _client;

    public LegacyRoutesTests(BookingApiFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Remove("X-Api-Token");
        _client.DefaultRequestHeaders.Add("X-Api-Token", "test-token");
    }

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

    [Theory]
    [MemberData(nameof(JsonEndpoints))]
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

    public static IEnumerable<object[]> JsonEndpoints()
    {
        return
        [
            ["EstimateService", "GetAllCategory"],
            ["EstimateService", "GetKms"],
            ["EstimateService", "GetEstimate"],
            ["EstimateService", "EstimateConfirmation"],
            ["EstimateService", "GetDefaultValues"],
            ["EstimateService", "GetProvince"],
            ["EstimateService", "GetAccessoryBooking"],
            ["EstimateService", "GetAccessoryBookingFromEstimate"],
            ["EstimateService", "GetNation"],
            ["EstimateService", "GetInsuranceExtra"],
            ["EstimateService", "GetInsuranceExtraFromEstimate"],
            ["EstimateService", "GetAmountEstimate"],
            ["EstimateService", "GetWholeEstimate"],

            ["SecurityService", "GetToken"],
            ["SecurityService", "ValidateToken"],
            ["SecurityService", "ResetKeyCache"],

            ["VehicleService", "GetVehicle"]
        ];
    }
}
