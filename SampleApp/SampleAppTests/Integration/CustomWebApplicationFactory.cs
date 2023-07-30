using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Respawn.Graph;
using SampleApp.Data;
using Testcontainers.PostgreSql;

namespace SampleAppTests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:14.5-alpine")
        .WithPortBinding(50432, 5432)
        .Build();

    public HttpClient HttpClient { get; private set; } = default!;

    private NpgsqlConnection _dbConnection = default!;
    private Respawner _respawn = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DataContext>)
            );


            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbConnection)
            );

            services.Remove(dbConnectionDescriptor!);
            services.Remove(dbContextDescriptor!);

            services.AddSingleton<DbConnection>(_ =>
            {
                _dbConnection = new NpgsqlConnection(_postgres.GetConnectionString());
                _dbConnection.Open();
                return _dbConnection;
            });

            services.AddDbContext<DataContext>(
                options => options.UseNpgsql(_postgres.GetConnectionString())
            );
        });

        builder.UseEnvironment("Development");
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawn.ResetAsync(_dbConnection);
    }

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        _dbConnection = new NpgsqlConnection(_postgres.GetConnectionString());

        HttpClient = CreateClient();
        await InitializeRespawner();
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();

        _respawn = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            TablesToIgnore = new Table[] { "__EFMigrationsHistory" },
            SchemasToInclude = new[] { "public" },
            DbAdapter = DbAdapter.Postgres,
            WithReseed = true
        });
    }

    public new async Task DisposeAsync()
    {
        await _postgres.DisposeAsync().AsTask();
    }
}