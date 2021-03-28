using System;
using System.Threading.Tasks;
using ScreenOverwriterServer.Services.Database.Models;

namespace ScreenOverwriterServer.Services.Database.Interfaces
{
    public interface IThreadCreator
    {
        ValueTask<ThreadModel> CreateThreadAsync(string title, DateTime beginningDate);
    }
}