using Rayn.Services.Database.Interfaces;

namespace Rayn.Services.Database.Configuration
{
    public class DatabaseConfiguration : IDatabaseConfig
    {
        public string ConnectionString { get; init; } = default!;
        public bool InMemoryMode { get; init; } = default!;
    }
}
