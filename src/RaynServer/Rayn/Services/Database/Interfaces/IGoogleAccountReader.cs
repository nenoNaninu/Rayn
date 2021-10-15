using System.Threading.Tasks;
using Rayn.Services.Models;

namespace Rayn.Services.Database.Interfaces
{
    public interface IGoogleAccountReader
    {
        ValueTask<GoogleAccount?> SearchAsync(string identifier);
    }
}