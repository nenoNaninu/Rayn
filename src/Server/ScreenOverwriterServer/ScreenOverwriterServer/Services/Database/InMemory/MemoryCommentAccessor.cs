using System;
using System.Threading.Tasks;
using ScreenOverwriterServer.Services.Database.Interfaces;
using ScreenOverwriterServer.Services.Database.Models;
using ScreenOverwriterServer.Services.Threading;

namespace ScreenOverwriterServer.Services.Database
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

        public async ValueTask InsertCommentAsync(byte[] message, Guid threadId, DateTime writtenTime)
        {
            using (await _asyncLock.LockAsync())
            {
                _memoryDatabase.InsertComment(message, writtenTime, threadId);
            }
        }
    }
}