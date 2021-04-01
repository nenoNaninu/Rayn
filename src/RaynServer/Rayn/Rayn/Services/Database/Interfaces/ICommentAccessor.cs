﻿using System;
using System.Threading.Tasks;
using Rayn.Services.Database.Models;

namespace Rayn.Services.Database.Interfaces
{
    public interface ICommentAccessor
    {
        ValueTask<CommentModel[]> ReadCommentAsync(Guid threadId);
        ValueTask InsertCommentAsync(byte[] message, Guid threadId, DateTime writtenTime);
    }
}