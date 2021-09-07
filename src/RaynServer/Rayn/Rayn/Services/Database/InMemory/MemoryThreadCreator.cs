using System;
using System.Threading.Tasks;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Database.Models;

namespace Rayn.Services.Database.InMemory
{
    public class MemoryThreadCreator : IThreadCreator
    {
        private readonly MemoryDatabase _memoryDatabase;

        public MemoryThreadCreator(MemoryDatabase memoryDatabase)
        {
            _memoryDatabase = memoryDatabase;
        }

        public ValueTask<ThreadModel> CreateThreadAsync(string title, DateTime beginningDate)
        {
            var thread = _memoryDatabase.CreateThread(beginningDate, title);
            return ValueTask.FromResult(thread);
        }
    }
}