using System;
using System.Threading.Tasks;
using ScreenOverwriterServer.Services.Database.Models;

namespace ScreenOverwriterServer.Services.Database.Interfaces
{
    public interface ICommentAccessor
    {
        ValueTask<CommentModel[]> ReadCommentAsync(Guid threadId);
        ValueTask InsertCommentAsync(byte[] message, Guid threadId, DateTime writtenTime);
    }
}