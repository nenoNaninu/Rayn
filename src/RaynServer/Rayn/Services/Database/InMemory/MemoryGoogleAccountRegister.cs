using System.Threading.Tasks;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;

namespace Rayn.Services.Database.InMemory
{
    public class MemoryGoogleAccountRegister : IGoogleAccountRegister
    {
        private readonly MemoryDatabase _memoryDatabase;

        public MemoryGoogleAccountRegister(MemoryDatabase memoryDatabase)
        {
            _memoryDatabase = memoryDatabase;
        }

        public ValueTask RegisterAsync(GoogleAccount account)
        {
            _memoryDatabase.AddGoogleAccount(account);
            return ValueTask.CompletedTask;
        }
    }
}
