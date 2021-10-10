namespace Rayn.Services.Database.Interfaces
{
    public interface IDatabaseConfig
    {
        public string ConnectionString { get; }
        public bool InMemoryMode { get; }
    }
}