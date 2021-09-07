using System.Threading.Tasks;
using Rayn.Services.Database.Models;

namespace Rayn.Services.Database.Interfaces
{
    public interface IAccountRegister
    {
        ValueTask RegisterAsync(Account account);
    }
}