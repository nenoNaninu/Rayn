using System;
using System.Threading.Tasks;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Database.Models;

namespace Rayn.Services.Database
{
    public class MemoryThreadReader : IThreadDbReader
    {
        private readonly MemoryDatabase _memoryDatabase;
        public MemoryThreadReader(MemoryDatabase memoryDatabase)
        {
            _memoryDatabase = memoryDatabase;
        }

        public ValueTask<ThreadModel?> SearchThreadModelAsync(Guid threadId)
        {
            var threadModel = _memoryDatabase.SearchThread(threadId);
            return ValueTask.FromResult(threadModel);
        }
    }
}