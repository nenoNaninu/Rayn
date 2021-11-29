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

    private readonly IDatabaseConfiguration _databaseConfiguration;

    public MySqlGoogleAccountReader(IDatabaseConfiguration databaseConfiguration)
    {
        _databaseConfiguration = databaseConfiguration;
    }

    public async ValueTask<GoogleAccount?> SearchAsync(string identifier)
    {
        await using var connection = new MySqlConnection(_databaseConfiguration.ConnectionString);

        var accounts = await connection.QueryAsync<GoogleAccount>(SearchQuery, new { Identifier = identifier });

        return accounts?.FirstOrDefault();
    }
}
