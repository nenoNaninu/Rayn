using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;

namespace Rayn.Services.Database.MySql
{
    public class MySqlGoogleAccountReader : IGoogleAccountReader
    {
        private const string SearchQuery = "select * from rayn_db.google_accounts where Identifier = @Identifier;";

        private readonly IDatabaseConfig _databaseConfig;

        public MySqlGoogleAccountReader(IDatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async ValueTask<GoogleAccount?> SearchAsync(string identifier)
        {
            var connection = new MySqlConnection(_databaseConfig.ConnectionString);

            var accounts = await connection.QueryAsync<GoogleAccount>(SearchQuery, new { Identifier = identifier });

            return accounts?.FirstOrDefault();
        }
    }
}