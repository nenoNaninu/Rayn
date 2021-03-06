using System;
using System.Threading.Tasks;
using Rayn.Services.Models;

namespace Rayn.Services.Database.Abstractions;

public interface ICommentAccessor
{
    ValueTask<CommentModel[]> ReadCommentAsync(Guid threadId);
    ValueTask InsertCommentAsync(string message, Guid threadId, DateTime writtenTime);
}
