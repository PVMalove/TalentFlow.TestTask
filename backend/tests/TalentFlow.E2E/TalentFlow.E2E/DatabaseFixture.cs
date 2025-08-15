using Testcontainers.MsSql;
using Xunit;

namespace TalentFlow.E2E;

public class DatabaseFixture : IAsyncLifetime
{
    public MsSqlContainer DbContainer { get; } = new MsSqlBuilder()
        .WithName("database-test")
        .WithAutoRemove(true)
        .WithCleanUp(true)
        .Build();

    public async Task InitializeAsync()
    {
        await DbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContainer.StopAsync();
        await DbContainer.DisposeAsync();
    }
}
