using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Rayn.Services.Database.Abstractions;
using Rayn.Services.Models;

namespace Rayn.Services.Database.MySql;

public class MySqlAccountRegister : IAccountRegister
{
    private const string AccountRegisterQuery =
        "insert into rayn_db.accounts(UserId, Email, LinkToGoogle) value (@UserId, @Email, @LinkToGoogle)";

    private readonly MySqlConnection _connection;

    public MySqlAccountRegister(MySqlConnection connection)
    {
        _connection = connection;
    }

    public async ValueTask RegisterAsync(Account account)
    {
        await _connection.ExecuteAsync(AccountRegisterQuery, account);
    }
}
