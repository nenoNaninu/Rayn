using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using Rayn.Services.Database.Abstractions;
using Rayn.Services.Models;

namespace Rayn.Services.Database.MySql;

public class MySqlThreadCreator : IThreadCreator
{
    private const string CreateThreadQuery =
@"insert into threads(ThreadId, OwnerId, ThreadTitle, BeginningDate, DateOffset, CreatedDate, AuthorId) 
values (@ThreadId, @OwnerId, @ThreadTitle, @BeginningDate, @DateOffset, @CreatedDate, @AuthorId)";

    private readonly IDatabaseConfiguration _databaseConfiguration;

    public MySqlThreadCreator(IDatabaseConfiguration databaseConfiguration)
    {
        _databaseConfiguration = databaseConfiguration;
    }

    public async ValueTask CreateThreadAsync(ThreadModel thread)
    {
        await using var conn = new MySqlConnection(_databaseConfiguration.ConnectionString);

        // 例外処理は後で...
        await conn.ExecuteAsync(CreateThreadQuery, thread);
    }
}
