using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rayn.Services.Models;

namespace Rayn.Services.Database.Interfaces;

public interface IThreadDbReader
{
    ValueTask<ThreadModel?> SearchThreadModelAsync(Guid threadId);
    ValueTask<IEnumerable<ThreadModel>> SearchThreadByUserId(Guid userId);
}
