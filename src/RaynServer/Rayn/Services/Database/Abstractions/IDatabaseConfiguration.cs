namespace Rayn.Services.Database.Abstractions;

public interface IDatabaseConfiguration
{
    public string ConnectionString { get; }
    public bool InMemoryMode { get; }
}
