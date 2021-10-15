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
        private const string CreateThreadQuery =
@"insert into threads(ThreadId, OwnerId, ThreadTitle, BeginningDate, DateOffset, CreatedDate, AuthorId) 
values (@ThreadId, @OwnerId, @ThreadTitle, @BeginningDate, @DateOffset, @CreatedDate, @AuthorId)";

        private readonly IDatabaseConfig _databaseConfig;

        public MySqlThreadCreator(IDatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async ValueTask CreateThreadAsync(ThreadModel thread)
        {
            using IDbConnection conn = new MySqlConnection(_databaseConfig.ConnectionString);

            // 例外処理は後で...
            await conn.ExecuteAsync(CreateThreadQuery, thread);
        }
    }
}