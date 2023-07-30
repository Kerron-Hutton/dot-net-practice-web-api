using System.Net;
using System.Net.Http.Json;
using SampleApp.DTO;

namespace SampleAppTests.Integration.SuperHero;

[Collection(nameof(SharedTestCollection))]
public class WriteApiIntegrationTests : IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;

    private readonly HttpClient _httpClient;

    public WriteApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _resetDatabase = factory.ResetDatabaseAsync;
        _httpClient = factory.HttpClient;
    }

    [Fact]
    public async Task GetAllSuperHeroes()
    {
        var response = await _httpClient.PostAsJsonAsync("api/SuperHero", new AddSuperHeroDTO
        {
            Name = "Spider Man",
            FirstName = "Peter",
            LastName = "Parker",
            Place = "New York"
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _resetDatabase();
    }
}