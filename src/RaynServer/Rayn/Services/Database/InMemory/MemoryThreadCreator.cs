using System;
using System.Threading.Tasks;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;

namespace Rayn.Services.Database.InMemory
{
    public class MemoryThreadCreator : IThreadCreator
    {
        private readonly MemoryDatabase _memoryDatabase;

        public MemoryThreadCreator(MemoryDatabase memoryDatabase)
        {
            _memoryDatabase = memoryDatabase;
        }

        public ValueTask CreateThreadAsync(ThreadModel thread)
        {
            _memoryDatabase.CreateThread(thread);
            return ValueTask.CompletedTask;
        }
    }
}