using System.Threading.Tasks;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;

namespace Rayn.Services.Database.InMemory
{
    public class MemoryGoogleAccountReader : IGoogleAccountReader
    {
        private readonly MemoryDatabase _memoryDatabase;

        public MemoryGoogleAccountReader(MemoryDatabase memoryDatabase)
        {
            _memoryDatabase = memoryDatabase;
        }
        public ValueTask<GoogleAccount?> SearchAsync(string identifier)
        {
            var account = _memoryDatabase.SearchGoogleAccount(identifier);
            return ValueTask.FromResult(account);
        }
    }
}