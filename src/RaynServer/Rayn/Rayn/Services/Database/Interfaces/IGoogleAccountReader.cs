using Rayn.Services.Database.Models;

namespace Rayn.Services.Database.Interfaces
{
    public interface IGoogleAccountReader
    {
        GoogleAccount? Search(string identifier);
    }
}