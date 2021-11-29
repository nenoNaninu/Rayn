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

    private readonly IDatabaseConfiguration _databaseConfiguration;

    public MySqlAccountRegister(IDatabaseConfiguration databaseConfiguration)
    {
        _databaseConfiguration = databaseConfiguration;
    }

    public async ValueTask RegisterAsync(Account account)
    {
        await using var connection = new MySqlConnection(_databaseConfiguration.ConnectionString);
        await connection.ExecuteAsync(AccountRegisterQuery, account);
    }
}
