using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Rayn.Services.Database.Abstractions;
using Rayn.Services.Models;

namespace Rayn.Services.Database.MySql;

public class MySqlGoogleAccountRegister : IGoogleAccountRegister
{
    private const string GoogleAccountRegisterQuery =
        "insert into rayn_db.google_accounts (UserId, Identifier, Email) values (@UserId, @Identifier, @Email);";

    private readonly MySqlConnection _connection;

    public MySqlGoogleAccountRegister(MySqlConnection connection)
    {
        _connection = connection;
    }

    public async ValueTask RegisterAsync(GoogleAccount account)
    {
        await _connection.ExecuteAsync(GoogleAccountRegisterQuery, account);
    }
}
