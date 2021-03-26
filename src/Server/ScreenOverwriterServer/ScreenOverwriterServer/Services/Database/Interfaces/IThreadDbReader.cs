using System;
using System.Threading.Tasks;
using ScreenOverwriterServer.Services.Database.Models;

namespace ScreenOverwriterServer.Services.Database.Interfaces
{
    public interface IThreadDbReader
    {
        ValueTask<ThreadModel> SearchThreadModelAsync(Guid threadId);
    }
}