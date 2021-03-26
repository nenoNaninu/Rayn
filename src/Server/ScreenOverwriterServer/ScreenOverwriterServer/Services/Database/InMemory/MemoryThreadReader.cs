using System;
using System.Threading.Tasks;
using ScreenOverwriterServer.Services.Database.Interfaces;
using ScreenOverwriterServer.Services.Database.Models;

namespace ScreenOverwriterServer.Services.Database
{
    public class MemoryThreadReader : IThreadDbReader
    {
        private readonly MemoryDatabase _memoryDatabase;
        public MemoryThreadReader(MemoryDatabase memoryDatabase)
        {
            _memoryDatabase = memoryDatabase;
        }

        public ValueTask<ThreadModel> SearchThreadModel(Guid threadId)
        {
            var threadModel = _memoryDatabase.SearchThread(threadId);
        }
    }
}