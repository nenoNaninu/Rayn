using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Database.Models;

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

        public async ValueTask<ThreadModel> CreateThreadAsync(string title, DateTime beginningDate)
        {
            var newThread = new ThreadModel(Guid.NewGuid(), Guid.NewGuid(), title, beginningDate);
            using IDbConnection conn = new MySqlConnection(_databaseConfig.ConnectionString);

            // 例外処理は後で...
            await conn.ExecuteAsync(_query, newThread);
            
            return newThread;
        }
    }
}