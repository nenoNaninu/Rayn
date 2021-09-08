using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;

namespace Rayn.Services.Database.MySql
{
    public class MySqlThreadCreator : IThreadCreator
    {
        private readonly IDatabaseConfig _databaseConfig;

        private const string _query = "insert into threads(ThreadId, OwnerId, ThreadTitle, BeginningDate) values (@ThreadId, @OwnerId, @ThreadTitle, @BeginningDate)";

        public MySqlThreadCreator(IDatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async ValueTask CreateThreadAsync(ThreadModel thread)
        {
            using IDbConnection conn = new MySqlConnection(_databaseConfig.ConnectionString);

            // 例外処理は後で...
            await conn.ExecuteAsync(_query, thread);
        }
    }
}