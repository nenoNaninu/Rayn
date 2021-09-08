using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;

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

        public ValueTask<IEnumerable<ThreadModel>> SearchThreadByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}