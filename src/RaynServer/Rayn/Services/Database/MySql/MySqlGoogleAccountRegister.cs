using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;

namespace Rayn.Services.Database.MySql;

public class MySqlGoogleAccountRegister : IGoogleAccountRegister
{
    private const string GoogleAccountRegisterQuery =
        "insert into rayn_db.google_accounts (UserId, Identifier, Email) values (@UserId, @Identifier, @Email);";

    private readonly IDatabaseConfig _databaseConfig;

    public MySqlGoogleAccountRegister(IDatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public async ValueTask RegisterAsync(GoogleAccount account)
    {
        var connection = new MySqlConnection(_databaseConfig.ConnectionString);
        await connection.ExecuteAsync(GoogleAccountRegisterQuery, account);
    }
}
