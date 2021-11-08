using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;

namespace Rayn.Services.Database.MySql;

public class MySqlAccountRegister : IAccountRegister
{
    private const string AccountRegisterQuery =
        "insert into rayn_db.accounts(UserId, Email, LinkToGoogle) value (@UserId, @Email, @LinkToGoogle)";

    private readonly IDatabaseConfig _databaseConfig;

    public MySqlAccountRegister(IDatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public async ValueTask RegisterAsync(Account account)
    {
        var connection = new MySqlConnection(_databaseConfig.ConnectionString);
        await connection.ExecuteAsync(AccountRegisterQuery, account);
    }
}
