using System.Threading.Tasks;
using Rayn.Services.Models;

namespace Rayn.Services.Database.Abstractions;

public interface IAccountRegister
{
    ValueTask RegisterAsync(Account account);
}
