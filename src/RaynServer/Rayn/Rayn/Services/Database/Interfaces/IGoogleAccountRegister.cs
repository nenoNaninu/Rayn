using System;
using System.Threading.Tasks;
using Rayn.Services.Database.Models;

namespace Rayn.Services.Database.Interfaces
{
    public interface IGoogleAccountRegister
    {
        ValueTask RegisterAsync(GoogleAccount account);
    }
}