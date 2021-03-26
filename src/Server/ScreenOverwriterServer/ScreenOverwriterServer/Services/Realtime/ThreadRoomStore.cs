using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RxWebSocket.Threading;

namespace ScreenOverwriterServer.Services.Realtime
{
    // シングルトンでDI
    public class ThreadRoomStore : IThreadRoomStore
    {
        private readonly AsyncLock _asyncLock = new();
        private readonly List<IThreadRoom> _threadRoomList = new();
        private readonly ILogger<IThreadRoom> _logger;

        public ThreadRoomStore(ILogger<IThreadRoom> logger)
        {
            _logger = logger;
        }

        public async ValueTask<IThreadRoom> FetchThreadRoom(Guid threadId)
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

                var newRoom = this.CreateThreadRoom();
                _threadRoomList.Add(newRoom);

                return newRoom;
            }
        }

        private IThreadRoom CreateThreadRoom(Guid threadId)
        {
            var room = new ThreadRoom(threadId, _logger);
        }
    }
}