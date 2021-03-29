using System;
using System.Collections.Concurrent;
using System.Linq;
using ScreenOverwriterServer.Services.Database.Models;

namespace ScreenOverwriterServer.Services.Database
{
    public class MemoryDatabase
    {
        private readonly ConcurrentBag<ThreadModel> _threadDataStore = new();
        private readonly ConcurrentBag<CommentModel> _commentDataStore = new();
        private int _commentId = 0;

        public ThreadModel CreateThread(DateTime beginningTime, string title)
        {
            var thread = new ThreadModel(Guid.NewGuid(), beginningTime, title, Guid.NewGuid());

            _threadDataStore.Add(thread);
            return thread;
        }

        public ThreadModel SearchThread(Guid guid)
        {
            var model = _threadDataStore.FirstOrDefault(x => x.ThreadId == guid);
            return model;
        }

        public CommentModel[] FetchAlreadyExistComments(Guid threadId)
        {
            var comments = _commentDataStore
                .Where(x => x.ThreadId == threadId)
                .OrderBy(x => x.WrittenTime)
                .ToArray();

            return comments;
        }

        public void InsertComment(byte[] message, DateTime writtenTime, Guid threadId)
        {
            var commentModel = new CommentModel(_commentId, threadId, writtenTime, message);
            _commentDataStore.Add(commentModel);
            _commentId++;
        }
    }
}