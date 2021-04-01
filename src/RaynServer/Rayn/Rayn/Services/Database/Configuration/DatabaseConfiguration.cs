using Rayn.Services.Database.Interfaces;

namespace Rayn.Services.Database.Configuration
{
    public class DatabaseConfiguration : IDatabaseConfig
    {
        public string ConnectionString { get; set; }
        public bool InMemoryMode { get; set; }
    }
}