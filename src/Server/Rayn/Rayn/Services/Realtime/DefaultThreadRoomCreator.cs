using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Database.Models;

namespace Rayn.Services.Realtime
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