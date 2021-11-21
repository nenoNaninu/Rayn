using System;
using System.Threading.Tasks;
using Rayn.Services.Models;

namespace Rayn.Services.Database.Abstractions;

public interface IThreadCreator
{
    ValueTask CreateThreadAsync(ThreadModel threadModel);
}
