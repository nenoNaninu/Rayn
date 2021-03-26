using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ScreenOverwriterServer.Services.Database.Interfaces;
using ScreenOverwriterServer.Services.Database.Models;

namespace ScreenOverwriterServer.Services.Realtime
{
    public class DefaultThreadRoomCreator : IThreadRoomCreator
    {
        private readonly ICommentAccessor _commentAccessor;
        private readonly ILogger<IThreadRoom> _logger;
        public DefaultThreadRoomCreator(ICommentAccessor commentAccessor, ILogger<IThreadRoom> logger)
        {
            _commentAccessor = commentAccessor;
            _logger = logger;
        }

        public ValueTask<IThreadRoom> CreateRoomAsync(ThreadModel threadModel)
        {
            IThreadRoom threadRoom = new ThreadRoom(threadModel, _commentAccessor, _logger);
            return ValueTask.FromResult(threadRoom);
        }
    }
}