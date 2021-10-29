using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;

namespace Rayn.Services.Database.InMemory
{
    public class MemoryThreadReader : IThreadDbReader
    {
        private readonly MemoryDatabase _memoryDatabase;
        public MemoryThreadReader(MemoryDatabase memoryDatabase)
        {
            _memoryDatabase = memoryDatabase;
        }

        public ValueTask<IEnumerable<ThreadModel>> SearchThreadByUserId(Guid userId)
        {
            var threads = _memoryDatabase.SearchThreadByUserId(userId);
            return ValueTask.FromResult(threads);
        }

        public ValueTask<ThreadModel?> SearchThreadModelAsync(Guid threadId)
        {
            var threadModel = _memoryDatabase.SearchThread(threadId);
            return ValueTask.FromResult(threadModel);
        }
    }
}
