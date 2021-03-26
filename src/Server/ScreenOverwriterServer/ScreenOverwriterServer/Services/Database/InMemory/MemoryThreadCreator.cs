using System;
using System.Threading.Tasks;
using ScreenOverwriterServer.Services.Database.Interfaces;
using ScreenOverwriterServer.Services.Database.Models;

namespace ScreenOverwriterServer.Services.Database
{
    public class MemoryThreadCreator : IThreadCreator
    {
        private readonly MemoryDatabase _memoryDatabase;

        public MemoryThreadCreator(MemoryDatabase memoryDatabase)
        {
            _memoryDatabase = memoryDatabase;
        }

        public ValueTask<ThreadModel> CreateThread(DateTime beginningTime)
        {
            var thread = _memoryDatabase.CreateThread(beginningTime);
            return ValueTask.FromResult(thread);
        }
    }
}