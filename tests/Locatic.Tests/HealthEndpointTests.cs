using System.Net;

namespace Locatic.Tests;

public class HealthEndpointTests : IClassFixture<LocaticWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HealthEndpointTests(LocaticWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Health_ReturnsOk()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task HomePage_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
