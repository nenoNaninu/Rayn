namespace Rayn.Services.Database.Abstractions;

public interface IDatabaseConfig
{
    public string ConnectionString { get; }
    public bool InMemoryMode { get; }
}
