using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Realtime.Interfaces;
using RxWebSocket.Threading;

namespace Rayn.Services.Realtime
{
    // シングルトン想定
    public sealed class ThreadRoomStore : IThreadRoomStore
    { 
        private readonly AsyncLock _asyncLock = new();
        // AsyncLockかけるから普通のListでいいや、という。
        private readonly List<IThreadRoom> _threadRoomList = new();
        private readonly ILogger<IThreadRoom> _logger;
        private readonly IThreadRoomCreator _threadRoomCreator;
        private readonly IThreadDbReader _threadDbReader;

        public ThreadRoomStore(IThreadRoomCreator threadRoomCreator, IThreadDbReader threadDbReader, ILogger<IThreadRoom> logger)
        {
            _logger = logger;
            _threadRoomCreator = threadRoomCreator;
            _threadDbReader = threadDbReader;
        }

        public async ValueTask<IThreadRoom> FetchThreadRoomAsync(Guid threadId)
        {
            // 重複してroomが作成される事をおそれてセマフォかけてる。
            using (await _asyncLock.LockAsync())
            {
                var room = _threadRoomList
                    .FirstOrDefault(x => x.ThreadModel.ThreadId == threadId);

                if (room != null)
                {
                    return room;
                }

                var newRoom = await this.CreateThreadRoomAsync(threadId);
                _threadRoomList.Add(newRoom);

                newRoom
                    .OnDispose()
                    .Subscribe(async _ =>
                    {
                        using (await _asyncLock.LockAsync())
                        {
                            _threadRoomList.Remove(newRoom);
                        }
                    });

                return newRoom;
            }
        }

        private async ValueTask<IThreadRoom> CreateThreadRoomAsync(Guid threadId)
        {
            var model = await _threadDbReader.SearchThreadModelAsync(threadId);
            var room = await _threadRoomCreator.CreateRoomAsync(model);
            return room;
        }
    }
}