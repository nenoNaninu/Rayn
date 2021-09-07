using System;
using System.Collections.Concurrent;
using System.Linq;
using Rayn.Services.Database.Models;

namespace Rayn.Services.Database.InMemory
{
    public class MemoryDatabase
    {
        private readonly ConcurrentBag<ThreadModel> _threadDataStore = new();
        private readonly ConcurrentBag<CommentModel> _commentDataStore = new();
        private readonly ConcurrentBag<GoogleAccount> _googleAccountStore = new();
        private readonly ConcurrentBag<Account> _accountStore = new();

        private int _commentId = 0;

        public ThreadModel CreateThread(DateTime beginningTime, string title)
        {
            var thread = new ThreadModel()
            {
                ThreadId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                ThreadTitle = title,
                BeginningDate = beginningTime
            };

            _threadDataStore.Add(thread);
            return thread;
        }

        public ThreadModel? SearchThread(Guid guid)
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

        public void InsertComment(string message, DateTime writtenTime, Guid threadId)
        {
            var commentModel = new CommentModel
            {
                Id = _commentId,
                ThreadId = threadId,
                WrittenTime = writtenTime,
                Message = message
            };

            _commentDataStore.Add(commentModel);
            _commentId++;
        }

        public void AddAccount(Account account)
        {
            _accountStore.Add(account);
        }

        public void AddGoogleAccount(GoogleAccount account)
        {
            _googleAccountStore.Add(account);
        }
    }
}