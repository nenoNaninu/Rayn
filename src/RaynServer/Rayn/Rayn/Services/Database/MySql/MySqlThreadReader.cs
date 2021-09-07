using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Database.Models;

namespace Rayn.Services.Database.MySql
{
    public class MySqlThreadReader : IThreadDbReader
    {
        private readonly IDatabaseConfig _databaseConfig;

        public MySqlThreadReader(IDatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async ValueTask<ThreadModel?> SearchThreadModelAsync(Guid threadId)
        {
            using IDbConnection conn = new MySqlConnection(_databaseConfig.ConnectionString);

            var searchResult = await conn.QueryAsync<ThreadModel>("select * from rayn_db.threads where ThreadId = @ThreadId;", new { ThreadId = threadId });

            return searchResult.FirstOrDefault();
        }
    }
}