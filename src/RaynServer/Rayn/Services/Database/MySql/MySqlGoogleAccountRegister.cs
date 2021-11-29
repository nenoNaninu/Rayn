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

    private readonly IDatabaseConfiguration _databaseConfiguration;

    public MySqlGoogleAccountRegister(IDatabaseConfiguration databaseConfiguration)
    {
        _databaseConfiguration = databaseConfiguration;
    }

    public async ValueTask RegisterAsync(GoogleAccount account)
    {
        await using var connection = new MySqlConnection(_databaseConfiguration.ConnectionString);
        await connection.ExecuteAsync(GoogleAccountRegisterQuery, account);
    }
}
