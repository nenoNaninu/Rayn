using System;
using System.Threading.Tasks;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;
using Rayn.Services.Threading;

namespace Rayn.Services.Database.InMemory
{
    public class MemoryCommentAccessor : ICommentAccessor
    {
        private readonly MemoryDatabase _memoryDatabase;
        private readonly AsyncLock _asyncLock = new();

        public MemoryCommentAccessor(MemoryDatabase memoryDatabase)
        {
            _memoryDatabase = memoryDatabase;
        }

        public async ValueTask<CommentModel[]> ReadCommentAsync(Guid threadId)
        {
            using (await _asyncLock.LockAsync())
            {
                return _memoryDatabase.FetchAlreadyExistComments(threadId);
            }
        }

        public async ValueTask InsertCommentAsync(string message, Guid threadId, DateTime writtenTime)
        {
            using (await _asyncLock.LockAsync())
            {
                _memoryDatabase.InsertComment(message, writtenTime, threadId);
            }
        }
    }
}
