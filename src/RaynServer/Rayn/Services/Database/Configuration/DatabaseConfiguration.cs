using Rayn.Services.Database.Abstractions;

namespace Rayn.Services.Database.Configuration;

public class DatabaseConfiguration : IDatabaseConfiguration
{
    public string ConnectionString { get; init; } = default!;
    public bool InMemoryMode { get; init; } = default!;
}
