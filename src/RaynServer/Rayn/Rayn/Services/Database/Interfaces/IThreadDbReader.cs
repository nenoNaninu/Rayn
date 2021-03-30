using System;
using System.Threading.Tasks;
using Rayn.Services.Database.Models;

namespace Rayn.Services.Database.Interfaces
{
    public interface IThreadDbReader
    {
        ValueTask<ThreadModel> SearchThreadModelAsync(Guid threadId);
    }
}