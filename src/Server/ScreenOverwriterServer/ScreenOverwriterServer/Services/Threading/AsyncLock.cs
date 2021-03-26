using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenOverwriterServer.Services.Threading
{
    public class AsyncLock
    {

        private readonly SemaphoreSlim _semaphore;
        private readonly LockReleaser _lockReleaser;

        public AsyncLock()
        {
            _semaphore = new SemaphoreSlim(1, 1);
            _lockReleaser = new LockReleaser(_semaphore);
        }


        public async ValueTask<IDisposable> LockAsync()
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);
            return _lockReleaser;
        }

        private class LockReleaser : IDisposable
        {
            private readonly SemaphoreSlim _semaphore;

            public LockReleaser(SemaphoreSlim semaphore)
            {
                _semaphore = semaphore;
            }

            public void Dispose()
            {
                _semaphore?.Release();
            }
        }
    }
}