using System.Threading.Tasks;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;

namespace Rayn.Services.Database.InMemory
{
    public class MemoryAccountRegister : IAccountRegister
    {
        private readonly MemoryDatabase _memoryDatabase;

        public MemoryAccountRegister(MemoryDatabase memoryDatabase)
        {
            _memoryDatabase = memoryDatabase;
        }

        public ValueTask RegisterAsync(Account account)
        {
            _memoryDatabase.AddAccount(account);
            return ValueTask.CompletedTask;
        }
    }
}