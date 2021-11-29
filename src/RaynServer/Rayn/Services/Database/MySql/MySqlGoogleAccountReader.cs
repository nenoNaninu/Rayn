using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Rayn.Services.Database.Abstractions;
using Rayn.Services.Models;

namespace Rayn.Services.Database.MySql;

public class MySqlGoogleAccountReader : IGoogleAccountReader
{
    private const string SearchQuery = "select * from rayn_db.google_accounts where Identifier = @Identifier;";

    private readonly MySqlConnection _connection;

    public MySqlGoogleAccountReader(MySqlConnection connection)
    {
        _connection = connection;
    }

    public async ValueTask<GoogleAccount?> SearchAsync(string identifier)
    {
        var accounts = await _connection.QueryAsync<GoogleAccount>(SearchQuery, new { Identifier = identifier });

        return accounts?.FirstOrDefault();
    }
}
